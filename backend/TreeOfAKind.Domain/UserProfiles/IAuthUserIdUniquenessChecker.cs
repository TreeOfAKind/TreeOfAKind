namespace TreeOfAKind.Domain.UserProfiles
{
    public interface IAuthUserIdUniquenessChecker
    {
        public bool IsUnique(string authUserId);
    }
}