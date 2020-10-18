namespace TreeOfAKind.Domain.UserProfiles
{
    public interface IUsernameUniquenessChecker
    {
        bool IsUnique(string username);
    }
}