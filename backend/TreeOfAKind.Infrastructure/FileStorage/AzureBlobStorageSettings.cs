using System.Collections.Generic;

namespace TreeOfAKind.Infrastructure.FileStorage
{
    public class AzureBlobStorageSettings
    {
        public string ConnectionString { get; set; }
        public IDictionary<string,string> Metadata { get; set; }
    }
}
