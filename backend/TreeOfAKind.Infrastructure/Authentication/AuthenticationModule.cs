using Autofac;
using TreeOfAKind.Application.Services;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Infrastructure.Authentication
{
    public class AuthenticationModule : Module
    {
        private readonly IUserAuthIdProvider _userAuthIdProvider;
        public AuthenticationModule(IUserAuthIdProvider userAuthIdProvider)
        {
            _userAuthIdProvider = userAuthIdProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_userAuthIdProvider is {})
            {
                builder.RegisterInstance(_userAuthIdProvider)
                    .As<IUserAuthIdProvider>();
            }
            else
            {
                builder.RegisterType<UserAuthIdProvider>()
                    .As<IUserAuthIdProvider>()
                    .InstancePerLifetimeScope();
            }
        }
    }
}