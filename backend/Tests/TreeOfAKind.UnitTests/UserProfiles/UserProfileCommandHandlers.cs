﻿using System;
using System.Net.Mail;
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
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreateOrUpdateUserProfileCommandHandler _createOrUpdateUserProfileCommandHandler;

        private string AuthId { get; set; } = "AuthId";
        private static string FirstName => "Firstname";
        private static string LastName => "Lastname";
        private static MailAddress Mail => new MailAddress("example@example.com");
        private static DateTime BirthDate => new DateTime(1998, 02, 27);

        public UserProfileCommandHandlers()
        {
            _userAuthIdUniquenessChecker = Substitute.For<IUserAuthIdUniquenessChecker>();
            _userProfileRepository = Substitute.For<IUserProfileRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _createOrUpdateUserProfileCommandHandler =
                new CreateOrUpdateUserProfileCommandHandler(_userProfileRepository, _userAuthIdUniquenessChecker,
                    _unitOfWork);
        }

        private CreateOrUpdateUserProfileCommand CreateCommand()
        {
            return new CreateOrUpdateUserProfileCommand(AuthId, Mail, FirstName, LastName, BirthDate);
        }

        [Fact]
        public async Task CreateOrUpdateUserProfile_ProfileDoesntExist_HandlerCreatesProfile()
        {
            _userProfileRepository
                .GetByUserAuthIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<UserProfile>(null));

            _unitOfWork
                .CommitAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(1));

            _userAuthIdUniquenessChecker
                .IsUnique(Arg.Any<string>())
                .Returns(true);

            await _createOrUpdateUserProfileCommandHandler.Handle(CreateCommand(), CancellationToken.None);

            await _unitOfWork.Received().CommitAsync(Arg.Any<CancellationToken>());
            await _userProfileRepository.Received().GetByUserAuthIdAsync(AuthId, Arg.Any<CancellationToken>());
            await _userProfileRepository.Received().AddAsync(Arg.Any<UserProfile>());

        }

        [Fact]
        public async Task CreateOrUpdateUserProfile_ProfileDoesExist_HandlerUpdates()
        {
            _userAuthIdUniquenessChecker
                .IsUnique(Arg.Any<string>())
                .Returns(true);

            var userProfile = UserProfile.CreateUserProfile(
                "userId",
                new MailAddress("example@example.com"),
                "pre",
                "preLastName",
                null,
                _userAuthIdUniquenessChecker);

            _userProfileRepository
                .GetByUserAuthIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<UserProfile>(userProfile));

            _unitOfWork
                .CommitAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(1));



            await _createOrUpdateUserProfileCommandHandler.Handle(CreateCommand(), CancellationToken.None);

            await _unitOfWork.Received().CommitAsync(Arg.Any<CancellationToken>());
            await _userProfileRepository.Received().GetByUserAuthIdAsync(AuthId, Arg.Any<CancellationToken>());
            await _userProfileRepository.DidNotReceive().AddAsync(Arg.Any<UserProfile>());

            Assert.Equal(FirstName, userProfile.FirstName);
            Assert.Equal(LastName, userProfile.LastName);
            Assert.Equal(BirthDate, userProfile.BirthDate);

        }
    }
}
