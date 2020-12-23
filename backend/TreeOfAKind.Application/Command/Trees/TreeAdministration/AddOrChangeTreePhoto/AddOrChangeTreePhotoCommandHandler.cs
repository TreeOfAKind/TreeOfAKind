using System;
using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Application.Services;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.AddOrChangeTreePhoto
{
    public class AddOrChangeTreePhotoCommandHandler : ICommandHandler<AddOrChangeTreePhotoCommand, Uri>
    {
        private readonly IFileSaver _fileSaver;
        private readonly ITreeRepository _treeRepository;

        public AddOrChangeTreePhotoCommandHandler(IFileSaver fileSaver, ITreeRepository treeRepository)
        {
            _fileSaver = fileSaver;
            _treeRepository = treeRepository;
        }

        public async Task<Uri> Handle(AddOrChangeTreePhotoCommand request, CancellationToken cancellationToken)
        {
            var file = request.Document;

            var uri = await _fileSaver.UploadFile(request.TreeId.Value.ToString(), file.ContentType, file.Content, cancellationToken);
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            tree!.AddOrChangeTreePhoto(uri);

            return uri;
        }
    }
}
