using System.Threading;
using System.Threading.Tasks;
using NodaTime;
using TreeOfAKind.Application.Configuration.Queries;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Query.UserProfiles.GetMyUserProfile
{
    public class GetMyUserProfileQueryHandler : IQueryHandler<GetMyUserProfileQuery, UserProfileDto>
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public GetMyUserProfileQueryHandler(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfileDto> Handle(GetMyUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository.GetByUserAuthIdAsync(request.AuthId, cancellationToken);
            if (userProfile is null) return null;
            return new UserProfileDto(
                userProfile.Id.Value,
                userProfile.FirstName,
                userProfile.LastName,
                userProfile.BirthDate
            );
        }
    }
}
