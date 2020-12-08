using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Serilog;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Application.Configuration.Emails;
using TreeOfAKind.Application.Services;
using TreeOfAKind.Infrastructure.Authentication;
using TreeOfAKind.Infrastructure.Caching;
using TreeOfAKind.Infrastructure.Database;
using TreeOfAKind.Infrastructure.Domain;
using TreeOfAKind.Infrastructure.Emails;
using TreeOfAKind.Infrastructure.FileStorage;
using TreeOfAKind.Infrastructure.Logging;
using TreeOfAKind.Infrastructure.Processing;
using TreeOfAKind.Infrastructure.Processing.InternalCommands;
using TreeOfAKind.Infrastructure.Processing.Outbox;
using TreeOfAKind.Infrastructure.Quartz;
using TreeOfAKind.Infrastructure.SeedWork;

namespace TreeOfAKind.Infrastructure
{
    public class ApplicationStartup
    {
        public static IServiceProvider Initialize(
            IServiceCollection services,
            string connectionString,
            ICacheStore cacheStore,
            IEmailSender emailSender,
            IFileSaver fileSaver,
            EmailsSettings emailsSettings,
            AzureBlobStorageSettings azureBlobStorageSettings,
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            IUserAuthIdProvider userAuthIdProvider,
            bool runQuartz = true)
        {
            if (runQuartz)
            {
                StartQuartz(connectionString, emailsSettings, azureBlobStorageSettings, logger, executionContextAccessor, userAuthIdProvider);
            }

            services.AddSingleton(cacheStore);

            var serviceProvider = CreateAutofacServiceProvider(
                services,
                connectionString,
                emailSender,
                fileSaver,
                emailsSettings,
                azureBlobStorageSettings,
                logger,
                executionContextAccessor,
                userAuthIdProvider);

            return serviceProvider;
        }

        private static IServiceProvider CreateAutofacServiceProvider(
            IServiceCollection services,
            string connectionString,
            IEmailSender emailSender,
            IFileSaver fileSaver,
            EmailsSettings emailsSettings,
            AzureBlobStorageSettings azureBlobStorageSettings,
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            IUserAuthIdProvider userAuthIdProvider)
        {
            var container = new ContainerBuilder();

            container.Populate(services);

            container.RegisterModule(new LoggingModule(logger));
            container.RegisterModule(new DataAccessModule(connectionString));
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new DomainModule());
            container.RegisterModule(new AuthenticationModule(userAuthIdProvider));
            container.RegisterModule(new AzureBlobStorageModule(azureBlobStorageSettings, fileSaver));

            if (emailSender != null)
            {
                container.RegisterModule(new EmailModule(emailSender, emailsSettings));
            }
            else
            {
                container.RegisterModule(new EmailModule(emailsSettings));
            }

            container.RegisterModule(new ProcessingModule());

            container.RegisterInstance(executionContextAccessor);

            var buildContainer = container.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));

            var serviceProvider = new AutofacServiceProvider(buildContainer);

            CompositionRoot.SetContainer(buildContainer);

            return serviceProvider;
        }

        private static void StartQuartz(
            string connectionString,
            EmailsSettings emailsSettings,
            AzureBlobStorageSettings blobStorageSettings,
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            IUserAuthIdProvider userAuthIdProvider)
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            var container = new ContainerBuilder();

            container.RegisterModule(new LoggingModule(logger));
            container.RegisterModule(new QuartzModule());
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new DataAccessModule(connectionString));
            container.RegisterModule(new EmailModule(emailsSettings));
            container.RegisterModule(new ProcessingModule());
            container.RegisterModule(new AuthenticationModule(userAuthIdProvider));
            container.RegisterModule(new AzureBlobStorageModule(blobStorageSettings));

            container.RegisterInstance(executionContextAccessor);
            container.Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<TreesContext>();
                dbContextOptionsBuilder.UseSqlServer(connectionString);

                dbContextOptionsBuilder
                    .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                return new TreesContext(dbContextOptionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();

            scheduler.JobFactory = new JobFactory(container.Build());

            scheduler.Start().GetAwaiter().GetResult();

            var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
            var trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * ? * *")
                    .Build();

            scheduler.ScheduleJob(processOutboxJob, trigger).GetAwaiter().GetResult();

            var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
            var triggerCommandsProcessing =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * ? * *")
                    .Build();
            scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing).GetAwaiter().GetResult();
        }
    }
}
