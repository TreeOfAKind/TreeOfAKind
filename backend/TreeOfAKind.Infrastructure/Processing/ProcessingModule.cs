using Autofac;
using MediatR;
using TreeOfAKind.Application.Configuration.DomainEvents;
using TreeOfAKind.Application.Configuration.Processing;
using TreeOfAKind.Infrastructure.Logging;
using TreeOfAKind.Infrastructure.Processing.InternalCommands;

namespace TreeOfAKind.Infrastructure.Processing
{
    public class ProcessingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assemblies.Application)
                .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();

            builder.RegisterGenericDecorator(
                typeof(DomainEventsDispatcherNotificationHandlerDecorator<>), 
                typeof(INotificationHandler<>));

            builder.RegisterGenericDecorator(
                typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
                typeof(IRequestHandler<,>));

            builder.RegisterType<CommandsDispatcher>()
                .As<ICommandsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandsScheduler>()
                .As<ICommandsScheduler>()
                .InstancePerLifetimeScope();
            
            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerWithResultDecorator<,>),
                typeof(IRequestHandler<,>));
        }
    }
}