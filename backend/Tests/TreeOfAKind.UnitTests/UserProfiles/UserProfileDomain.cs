using System;
using System.Net.Mail;
using NSubstitute;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Events;
using TreeOfAKind.Domain.UserProfiles.Rules;
using Xunit;

namespace TreeOfAKind.UnitTests.UserProfiles
{
    public class CreateOrUpdateUserProfileDomain
    {
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;

        private string AuthId { get; set; } = "AuthId";
        private static string FirstName => "Firstname";
        private static string LastName => "Lastname";
        private static DateTime BirthDate => new DateTime(1998, 02, 27);

        public CreateOrUpdateUserProfileDomain()
        {
            _userAuthIdUniquenessChecker = Substitute.For<IUserAuthIdUniquenessChecker>();
        }

        private UserProfile CreateValidUserProfile()
        {
            _userAuthIdUniquenessChecker.IsUnique(Arg.Any<string>()).Returns(true);
            return CreateUserProfileFromProperties();
        }

        private UserProfile CreateUserProfileFromProperties()
        {
            return UserProfile.CreateUserProfile(
                AuthId,
                new MailAddress("example@example.com"),
                FirstName,
                LastName,
                BirthDate,
                _userAuthIdUniquenessChecker
                );
        }

        [Fact]
        public void CreateUserProfile_ValidData_AuthIdAndUsernameUniquenessChecked()
        {
            var u = CreateValidUserProfile();
            _userAuthIdUniquenessChecker.Received().IsUnique(AuthId);
        }

        [Fact]
        public void CreateUserProfile_ValidData_PropertiesProperlyInitialized()
        {
            var u = CreateValidUserProfile();

            Assert.Equal(u.UserAuthId, AuthId);
            Assert.Equal(u.FirstName, FirstName);
            Assert.Equal(u.LastName, u.LastName);
            Assert.Equal(u.BirthDate, BirthDate);
        }

        [Fact]
        public void CreateUserProfile_ValidData_UserProfileCreatedDomainEventAdded()
        {
            var u = CreateValidUserProfile();

            Assert.Contains(u.DomainEvents, e =>
            {
                var userProfileCreated = e as UserProfileCreatedEvent;
                return userProfileCreated?.UserId == u.Id;
            });
        }

        [Fact]
        public void CreateUserProfile_NonUniqueAuthId_ThrowsBusinessRuleValidationException()
        {
            _userAuthIdUniquenessChecker.IsUnique(Arg.Any<string>()).Returns(false);
            Assert.Throws<BusinessRuleValidationException>(() => _ = this.CreateUserProfileFromProperties());
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreteUserProfile_AuthIdNotProvided_ThrowsBusinessRuleException(string authId)
        {
            this.AuthId = authId;

            Assert.Throws<BusinessRuleValidationException>(() => _ = this.CreateUserProfileFromProperties());
        }
    }
}
