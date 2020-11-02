using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TreeOfAKind.Application.Configuration.Data;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Infrastructure.Domain;
using TreeOfAKind.Infrastructure.Domain.UserProfiles;
using TreeOfAKind.Infrastructure.SeedWork;

namespace TreeOfAKind.Infrastructure.Database
{
    public class DataAccessModule : Module
    {
        private readonly string _databaseConnectionString;

        public DataAccessModule(string databaseConnectionString)
        {
            this._databaseConnectionString = databaseConnectionString;
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<UserProfileRepository>()
                .As<IUserProfileRepository>()
                .InstancePerLifetimeScope();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _databaseConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            RegisterRepositories(builder);
            
            builder.RegisterType<StronglyTypedIdValueConverterSelector>()
                .As<IValueConverterSelector>()
                .InstancePerLifetimeScope();

            builder
                .Register(c =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<TreesContext>();
                    dbContextOptionsBuilder.UseSqlServer(_databaseConnectionString);
                    dbContextOptionsBuilder
                        .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                    return new TreesContext(dbContextOptionsBuilder.Options);
                })
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();
        }
    }
}