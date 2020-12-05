namespace TreeOfAKind.Infrastructure.FileStorage
{
    public class AzureBlobStorageSettings
    {
        public string ConnectionString { get; set; }
        public string BlobContainerName { get; set; }
    }
}
