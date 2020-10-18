using System;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles
{
    public class UserNameMustBeUniqueRule : IBusinessRule
    {
        private readonly string _username;
        private readonly IUsernameUniquenessChecker _uniquenessChecker;

        public UserNameMustBeUniqueRule(string username, IUsernameUniquenessChecker uniquenessChecker)
        {
            _username = username;
            _uniquenessChecker = uniquenessChecker;
        }

        public bool IsBroken() => !_uniquenessChecker.IsUnique(_username);

        public string Message => "Username is already taken.";
    }
}