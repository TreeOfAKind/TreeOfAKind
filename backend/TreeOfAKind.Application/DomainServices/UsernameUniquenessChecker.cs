using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.DomainServices
{
    public class UsernameUniquenessChecker : IUsernameUniquenessChecker
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UsernameUniquenessChecker(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public bool IsUnique(string username) => 
            _userProfileRepository.GetByUsername(username) is null;
    }
}