using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Application.Services;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People.AddDocument
{
    public class AddDocumentCommandHandler : ICommandHandler<AddDocumentCommand, FileId>
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IFileSaver _fileSaver;

        public AddDocumentCommandHandler(ITreeRepository treeRepository, IFileSaver fileSaver)
        {
            _treeRepository = treeRepository;
            _fileSaver = fileSaver;
        }

        public async Task<FileId> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = request.Document;
            var fileUri = await _fileSaver.UploadFile(request.TreeId.Value.ToString(), document.ContentType,
                document.Content, cancellationToken);

            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            return tree!.AddPersonFile(request.PersonId, document.Name, document.ContentType, fileUri);
        }
    }
}
