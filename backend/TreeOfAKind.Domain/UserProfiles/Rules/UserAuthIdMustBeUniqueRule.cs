using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles.Rules
{
    public class UserAuthIdMustBeUniqueRule : IBusinessRule
    {
        private readonly string _userAuthId;
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;

        public UserAuthIdMustBeUniqueRule(string userAuthId, IUserAuthIdUniquenessChecker userAuthIdUniquenessChecker)
        {
            _userAuthId = userAuthId;
            _userAuthIdUniquenessChecker = userAuthIdUniquenessChecker;
        }

        public bool IsBroken()
        {
            return !_userAuthIdUniquenessChecker.IsUnique(_userAuthId);
        }

        public string Message => "Profile for this user is already created";
    }
}