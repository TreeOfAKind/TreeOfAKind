using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Application.DomainServices.GedcomXImport;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Rules;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile
{
    public class CreateTreeFromFileCommandHandler : ICommandHandler<CreateTreeFromFileCommand, TreeId>
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IXmlStreamToGedcomXParser _xmlStreamToGedcomXParser;
        private readonly IGedcomXToDomainTreeConverter _gedcomXToDomainTreeConverter;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;


        public CreateTreeFromFileCommandHandler(ITreeRepository treeRepository,
            IXmlStreamToGedcomXParser xmlStreamToGedcomXParser,
            IGedcomXToDomainTreeConverter gedcomXToDomainTreeConverter, IUserProfileRepository userProfileRepository,
            IUserAuthIdUniquenessChecker userAuthIdUniquenessChecker)
        {
            _treeRepository = treeRepository;
            _xmlStreamToGedcomXParser = xmlStreamToGedcomXParser;
            _gedcomXToDomainTreeConverter = gedcomXToDomainTreeConverter;
            _userProfileRepository = userProfileRepository;
            _userAuthIdUniquenessChecker = userAuthIdUniquenessChecker;
        }

        public async Task<TreeId> Handle(CreateTreeFromFileCommand request, CancellationToken cancellationToken)
        {
            var gx = _xmlStreamToGedcomXParser.Parse(request.Document.Content);

            var userProfile = await _userProfileRepository.GetByUserAuthIdAsync(request.UserAuthId, cancellationToken);

            if (userProfile is null)
            {
                userProfile = UserProfile.CreateUserProfile(
                    request.UserAuthId, null, null, null, _userAuthIdUniquenessChecker);

                await _userProfileRepository.AddAsync(userProfile, cancellationToken);
            }

            var tree = _gedcomXToDomainTreeConverter.ConvertTree(userProfile.Id, gx, request.TreeName);

            await _treeRepository.AddAsync(tree, cancellationToken);

            return tree.Id;
        }
    }
}
