using Complaints.Core.User;
using Complaints.Data.Contexts;
using Complaints.Data.Entities;
using Complaints.Data.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Complaints.UnitTests.Services
{
    public class UserServiceTests : IClassFixture<DbFixture>
    {
        private readonly ServiceProvider _serviceProvider;
        public UserServiceTests(DbFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Theory]
        [InlineData("name", "surname", "username", "password")]
        public void ShouldCreateNewUserInInMemoryDatabase(string firstName, string lastName, string username, string password)
        {
            // Arrange 
            using var context = _serviceProvider.GetService<ComplaintsContext>();
            var _userService = new UserService(context);
            var userEntity = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username
            };

            // Act
            var createdUser = _userService.Create(userEntity, password);

            // Assert
            var userInDb = _userService.GetAll().First();
            Assert.Equal(createdUser, userInDb);
        }

        [Theory]
        [InlineData("name", "surname", "username", "")]
        [InlineData("name", "surname", "username", null)]
        public void ShouldThrowAnAuthenticationExceptionIfPasswordIsEmpty(string firstName, string lastName, string username, string password)
        {
            // Arrange
            using var context = _serviceProvider.GetService<ComplaintsContext>();
            var _userService = new UserService(context);
            var userEntity = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username
            };

            // Act + Assert
            Assert.Throws<AuthenticationException>(() => _userService.Create(userEntity, password));
        }

        [Theory]
        [InlineData("name", "surname", "username", "password")]
        public void ShouldThrowAnExceptionIfUsernameIsTaken(string firstName, string lastName, string username, string password)
        {
            // Arrange
            using var context = _serviceProvider.GetService<ComplaintsContext>();
            var _userService = new UserService(context);
            var userEntity1 = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username
            };

            var userEntity2 = new UserEntity
            {
                FirstName = "name",
                LastName = "surname",
                Username = "username"
            };

            var user1 = _userService.Create(userEntity1, password);

            // Act + Assert
            Assert.Throws<AuthenticationException>(() => _userService.Create(userEntity2, password));
        }

        [Theory]
        [InlineData("name", "surname", "username", "password")]
        public void ShouldAuthenticateUserGivenCredentialsAreCorrect(string firstName, string lastName, string username, string password)
        {
            // Arrange
            using var context = _serviceProvider.GetService<ComplaintsContext>();
            var _userService = new UserService(context);
            var userEntity = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username
            };

            var registeredUser = _userService.Create(userEntity, password);

            // Act 
            var loggedInUser = _userService.Authenticate(registeredUser.Username, password);

            // Assert
            Assert.Equal(registeredUser, loggedInUser);
        }

        [Theory]
        [InlineData("username", "wrongpassword")]
        public void ShouldThrowAnAuthenticationExceptionGivenPasswordIsIncorrect(string username, string password)
        {
            // Arrange
            using var context = _serviceProvider.GetService<ComplaintsContext>();
            var _userService = new UserService(context);
            var userEntity = new UserEntity
            {
                FirstName = "name",
                LastName = "surname",
                Username = username
            };

            var registeredUser = _userService.Create(userEntity, "password");

            // Act + Assert
            Assert.Throws<AuthenticationException>(() => _userService.Authenticate(username, password));
        }

        [Theory]
        [InlineData("wrongusername", "password")]
        public void ShouldThrowAnAuthenticationExceptionGivenUsernameIsIncorrect(string username, string password)
        {
            // Arrange
            using var context = _serviceProvider.GetService<ComplaintsContext>();
            var _userService = new UserService(context);
            var userEntity = new UserEntity
            {
                FirstName = "name",
                LastName = "surname",
                Username = "username"
            };

            var registeredUser = _userService.Create(userEntity, password);

            // Act + Assert
            Assert.Throws<AuthenticationException>(() => _userService.Authenticate(username, password));
        }
    }
}
