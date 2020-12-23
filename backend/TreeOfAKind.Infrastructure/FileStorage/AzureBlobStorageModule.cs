using Autofac;
using TreeOfAKind.Application.Services;

namespace TreeOfAKind.Infrastructure.FileStorage
{
    internal class AzureBlobStorageModule : Module
    {
        private readonly IFileSaver _fileSaver;
        private readonly AzureBlobStorageSettings _azureBlobStorageSettings;

        public AzureBlobStorageModule(AzureBlobStorageSettings azureBlobStorageSettings, IFileSaver fileSaver = null)
        {
            _azureBlobStorageSettings = azureBlobStorageSettings;
            _fileSaver = fileSaver;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_fileSaver is {})
            {
                builder.RegisterInstance(_fileSaver);
            }
            else
            {
                builder.RegisterType<AzureBlobStorageService>()
                    .As<IFileSaver>()
                    .InstancePerLifetimeScope();
            }

            builder.RegisterInstance(_azureBlobStorageSettings);
        }

    }
}
