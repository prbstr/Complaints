using Complaints.Core.User;
using Complaints.Data.Contexts;
using Complaints.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using Xunit;

namespace Complaints.UnitTests.Services
{
    public class DbFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public DbFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<ComplaintsContext>(options =>
                options.UseInMemoryDatabase("testDb"),
                ServiceLifetime.Transient
            );

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }

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
            using var context = _serviceProvider.GetService<ComplaintsContext>();
            var _userService = new UserService(context);
            var userEntity = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username
            };

            var createdUser = _userService.Create(userEntity, password);

            var userInDb = _userService.GetAll().First();
            Assert.Equal(createdUser, userInDb);
        }
    }
}
