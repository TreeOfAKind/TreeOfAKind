using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles.Rules
{
    public class OnlyAuthorizedUserCanCreateUserProfileRule : IBusinessRule
    {
        private readonly string _authUserId;
        public OnlyAuthorizedUserCanCreateUserProfileRule(string authUserId)
        {
            _authUserId = authUserId;
        }

        public bool IsBroken()
            => _authUserId is null || _authUserId.Length <= 0;

        public string Message => "Only authorized user can create user profile";
    }
}