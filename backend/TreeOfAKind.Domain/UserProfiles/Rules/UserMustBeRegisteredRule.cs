using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles.Rules
{
    public class UserMustBeRegisteredRule : IBusinessRule
    {
        private readonly string _userAuthId;
        public UserMustBeRegisteredRule(string userAuthId)
        {
            _userAuthId = userAuthId;
        }

        public bool IsBroken()
            => _userAuthId is null || _userAuthId.Length <= 0;

        public string Message => "Only registered person can have a user profile.";
    }
}