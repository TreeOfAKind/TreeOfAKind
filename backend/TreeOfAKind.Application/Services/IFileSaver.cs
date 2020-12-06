using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TreeOfAKind.Application.Services
{
    public interface IFileSaver
    {
        public Task<Uri> UploadFile(string containerName, string contentType, Stream stream, CancellationToken cancellationToken = default!);
    }
}
