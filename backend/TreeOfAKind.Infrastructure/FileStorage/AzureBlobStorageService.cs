using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MimeTypes;
using Serilog;
using TreeOfAKind.Application.Services;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Infrastructure.FileStorage
{
    public class AzureBlobStorageService : IFileSaver
    {
        private readonly ILogger _logger;
        private readonly IDictionary<string, string> _metadata;
        private readonly BlobServiceClient _blobServiceClient;

        private const string Wildcard = "*";


        private static readonly BlobRequestConditions BlobRequestConditions
            = new BlobRequestConditions {IfNoneMatch = new ETag(Wildcard)};

        public AzureBlobStorageService(AzureBlobStorageSettings settings, ILogger logger)
        {
            _logger = logger;
            var connectionString = settings.ConnectionString;
            _metadata = settings.Metadata;

            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<Uri> UploadFile(string containerName, string contentType, Stream stream,
            CancellationToken cancellationToken = default!)
        {
            try
            {
                var headers = new BlobHttpHeaders {ContentType = contentType};
                var blobName = Guid.NewGuid() + MimeTypeMap.GetExtension(contentType);

                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob, _metadata , cancellationToken);

                var blobClient = blobContainerClient.GetBlobClient(blobName);

                await blobClient.UploadAsync(
                    stream,
                    headers,
                    conditions: BlobRequestConditions,
                    cancellationToken: cancellationToken);

                return blobClient.Uri;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Uploading file to Azure Blob Storage failed");
                throw;
            }
        }
    }
}
