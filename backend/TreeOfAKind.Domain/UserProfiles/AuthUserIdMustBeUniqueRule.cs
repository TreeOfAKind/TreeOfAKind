using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles
{
    public class AuthUserIdMustBeUniqueRule : IBusinessRule
    {
        private readonly string _authUserId;
        private readonly IAuthUserIdUniquenessChecker _authUserIdUniquenessChecker;

        public AuthUserIdMustBeUniqueRule(string authUserId, IAuthUserIdUniquenessChecker authUserIdUniquenessChecker)
        {
            _authUserId = authUserId;
            _authUserIdUniquenessChecker = authUserIdUniquenessChecker;
        }

        public bool IsBroken()
        {
            return !_authUserIdUniquenessChecker.IsUnique(_authUserId);
        }

        public string Message => "Profile for this user is already created";
    }
}