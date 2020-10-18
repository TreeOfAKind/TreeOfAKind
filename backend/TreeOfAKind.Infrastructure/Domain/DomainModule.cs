using Autofac;
using TreeOfAKind.Application.DomainServices;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Infrastructure.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UsernameUniquenessChecker>()
                .As<IUsernameUniquenessChecker>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AuthUserIdUniquenessChecker>()
                .As<IUsernameUniquenessChecker>()
                .InstancePerLifetimeScope();
        }
    }
}