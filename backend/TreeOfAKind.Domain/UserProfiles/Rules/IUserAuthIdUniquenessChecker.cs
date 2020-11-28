namespace TreeOfAKind.Domain.UserProfiles.Rules
{
    public interface IUserAuthIdUniquenessChecker
    {
        public bool IsUnique(string userAuthId);
    }
}