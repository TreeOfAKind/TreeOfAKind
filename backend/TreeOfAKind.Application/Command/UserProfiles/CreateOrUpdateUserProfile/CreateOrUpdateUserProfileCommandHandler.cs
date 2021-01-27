using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Rules;

namespace TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile
{
    public class CreateOrUpdateUserProfileCommandHandler : ICommandHandler<CreateOrUpdateUserProfileCommand, UserId>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrUpdateUserProfileCommandHandler(IUserProfileRepository userProfileRepository,
            IUserAuthIdUniquenessChecker userAuthIdUniquenessChecker,
            IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
            _userAuthIdUniquenessChecker = userAuthIdUniquenessChecker;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserId> Handle(CreateOrUpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository.GetByUserAuthIdAsync(request.UserAuthId, cancellationToken);

            if (userProfile is null)
            {
                userProfile = await CreateNewUserProfile(request, cancellationToken);
            }
            else
            {
                userProfile.UpdateUserProfile(request.MailAddress, request.FirstName, request.LastName, request.BirthDate);
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            return userProfile.Id;
        }

        private async Task<UserProfile> CreateNewUserProfile(CreateOrUpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var newUserProfile = UserProfile.CreateUserProfile(
                request.UserAuthId,
                request.MailAddress,
                request.FirstName,
                request.LastName,
                request.BirthDate,
                _userAuthIdUniquenessChecker
            );

            await _userProfileRepository.AddAsync(newUserProfile, cancellationToken);
            return newUserProfile;
        }
    }
}
