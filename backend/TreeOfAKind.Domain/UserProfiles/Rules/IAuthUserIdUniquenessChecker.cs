namespace TreeOfAKind.Domain.UserProfiles.Rules
{
    public interface IAuthUserIdUniquenessChecker
    {
        public bool IsUnique(string authUserId);
    }
}