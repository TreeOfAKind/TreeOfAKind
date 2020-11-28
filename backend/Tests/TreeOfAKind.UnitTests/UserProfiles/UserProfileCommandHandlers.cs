using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Rules;
using Xunit;

namespace TreeOfAKind.UnitTests.UserProfiles
{
    public class UserProfileCommandHandlers
    {
        private readonly IAuthUserIdUniquenessChecker _authUserIdUniquenessChecker;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreateOrUpdateUserProfileCommandHandler _createOrUpdateUserProfileCommandHandler;

        private string AuthId { get; set; } = "AuthId";
        private static string FirstName => "Firstname";
        private static string LastName => "Lastname";
        private static DateTime BirthDate => new DateTime(1998, 02, 27);

        public UserProfileCommandHandlers()
        {
            _authUserIdUniquenessChecker = Substitute.For<IAuthUserIdUniquenessChecker>();
            _userProfileRepository = Substitute.For<IUserProfileRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _createOrUpdateUserProfileCommandHandler =
                new CreateOrUpdateUserProfileCommandHandler(_userProfileRepository, _authUserIdUniquenessChecker,
                    _unitOfWork);
        }

        private CreateOrUpdateUserProfileCommand CreateCommand()
        {
            return new CreateOrUpdateUserProfileCommand(AuthId, FirstName, LastName, BirthDate);
        }

        [Fact]
        public async Task CreateOrUpdateUserProfile_ProfileDoesntExist_HandlerCreatesProfile()
        {
            _userProfileRepository
                .GetByAuthUserIdAsync(Arg.Any<string>())
                .Returns(Task.FromResult<UserProfile>(null));
            
            _unitOfWork
                .CommitAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(1));

            _authUserIdUniquenessChecker
                .IsUnique(Arg.Any<string>())
                .Returns(true);

            await _createOrUpdateUserProfileCommandHandler.Handle(CreateCommand(), CancellationToken.None);

            await _unitOfWork.Received().CommitAsync(Arg.Any<CancellationToken>());
            await _userProfileRepository.Received().GetByAuthUserIdAsync(AuthId);
            await _userProfileRepository.Received().AddAsync(Arg.Any<UserProfile>());

        }
        
        [Fact]
        public async Task CreateOrUpdateUserProfile_ProfileDoesExist_HandlerUpdates()
        {
            _authUserIdUniquenessChecker
                .IsUnique(Arg.Any<string>())
                .Returns(true);

            var userProfile = UserProfile.CreateUserProfile(
                "userId",
                "pre",
                "preLastName",
                null,
                _authUserIdUniquenessChecker);

            _userProfileRepository
                .GetByAuthUserIdAsync(Arg.Any<string>())
                .Returns(Task.FromResult<UserProfile>(userProfile));
            
            _unitOfWork
                .CommitAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(1));



            await _createOrUpdateUserProfileCommandHandler.Handle(CreateCommand(), CancellationToken.None);

            await _unitOfWork.Received().CommitAsync(Arg.Any<CancellationToken>());
            await _userProfileRepository.Received().GetByAuthUserIdAsync(AuthId);
            await _userProfileRepository.DidNotReceive().AddAsync(Arg.Any<UserProfile>());
            
            Assert.Equal(FirstName, userProfile.FirstName);
            Assert.Equal(LastName, userProfile.LastName);
            Assert.Equal(BirthDate, userProfile.BirthDate);

        }
    }
}