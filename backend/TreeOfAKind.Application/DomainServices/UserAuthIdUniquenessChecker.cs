using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Rules;

namespace TreeOfAKind.Application.DomainServices
{
    public class UserAuthIdUniquenessChecker : IUserAuthIdUniquenessChecker
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserAuthIdUniquenessChecker(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public bool IsUnique(string userAuthId) =>
            _userProfileRepository.GetByUserAuthId(userAuthId) is null;
    }
}