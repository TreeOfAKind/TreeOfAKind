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
        private readonly IAuthUserIdUniquenessChecker _authUserIdUniquenessChecker;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrUpdateUserProfileCommandHandler(IUserProfileRepository userProfileRepository, 
            IAuthUserIdUniquenessChecker authUserIdUniquenessChecker,
            IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
            _authUserIdUniquenessChecker = authUserIdUniquenessChecker;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserId> Handle(CreateOrUpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository.GetByAuthUserIdAsync(request.AuthUserId);

            if (userProfile is null)
            {
                userProfile = await CreateNewUserProfile(request, cancellationToken);
            }
            else
            {
                userProfile.UpdateUserProfile(request.FirstName, request.LastName, request.BirthDate);
            }
            
            await _unitOfWork.CommitAsync(cancellationToken);
            return userProfile.Id;
        }

        private async Task<UserProfile> CreateNewUserProfile(CreateOrUpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var newUserProfile = UserProfile.CreateUserProfile(
                request.AuthUserId,
                request.FirstName,
                request.LastName,
                request.BirthDate,
                _authUserIdUniquenessChecker
            );

            await _userProfileRepository.AddAsync(newUserProfile, cancellationToken);
            return newUserProfile;
        }
    }
}