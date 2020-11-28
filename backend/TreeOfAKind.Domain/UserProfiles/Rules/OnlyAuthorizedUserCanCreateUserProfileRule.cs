using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles.Rules
{
    public class OnlyAuthorizedUserCanCreateUserProfileRule : IBusinessRule
    {
        private readonly string _userAuthId;
        public OnlyAuthorizedUserCanCreateUserProfileRule(string userAuthId)
        {
            _userAuthId = userAuthId;
        }

        public bool IsBroken()
            => _userAuthId is null || _userAuthId.Length <= 0;

        public string Message => "Only authorized user can create user profile";
    }
}