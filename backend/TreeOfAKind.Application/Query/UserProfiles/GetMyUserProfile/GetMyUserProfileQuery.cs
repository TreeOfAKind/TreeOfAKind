using TreeOfAKind.Application.Configuration.Queries;

namespace TreeOfAKind.Application.Query.UserProfiles.GetMyUserProfile
{
    public class GetMyUserProfileQuery : IQuery<UserProfileDto>
    {
        public string AuthId { get; }
        public GetMyUserProfileQuery(string authId)
        {
            AuthId = authId;
        }
    }
}