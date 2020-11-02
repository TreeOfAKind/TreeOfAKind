using System;
using System.Linq;
using NSubstitute;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;
using Xunit;

namespace TreeOfAKind.UnitTests.UserProfiles
{
    public class CreateUserProfile
    {
        private readonly IAuthUserIdUniquenessChecker _authUserIdUniquenessChecker;

        private string AuthId { get; set; } = "AuthId";
        private static string Username => "Username";
        private static string FirstName => "Firstname";
        private static string LastName => "Lastname";
        private static DateTime BirthDate => new DateTime(1998, 02, 27);

        public CreateUserProfile()
        {
            _authUserIdUniquenessChecker = Substitute.For<IAuthUserIdUniquenessChecker>();
        }

        private UserProfile CreateValidUserProfile()
        {
            _authUserIdUniquenessChecker.IsUnique(Arg.Any<string>()).Returns(true);
            return CreateUserProfileFromProperties();
        }

        private UserProfile CreateUserProfileFromProperties()
        {
            return UserProfile.CreateUserProfile(
                AuthId,
                FirstName,
                LastName,
                BirthDate,
                _authUserIdUniquenessChecker
                );
        }

        [Fact]
        public void CreateUserProfile_ValidData_AuthIdAndUsernameUniquenessChecked()
        {
            var u = CreateValidUserProfile();
            _authUserIdUniquenessChecker.Received().IsUnique(AuthId);
        }

        [Fact]
        public void CreateUserProfile_ValidData_PropertiesProperlyInitialized()
        {
            var u = CreateValidUserProfile();
                        
            Assert.Equal(u.AuthUserId, AuthId);
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
            _authUserIdUniquenessChecker.IsUnique(Arg.Any<string>()).Returns(false);
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