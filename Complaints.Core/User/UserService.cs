using Complaints.Data.Entities;
using System.Collections.Generic;

namespace Complaints.Core.User
{
    public interface IUserService
    {
        UserEntity Authenticate(string username, string password);
        UserEntity Create(UserEntity user, string password);
    }
    public class UserService : IUserService
    {
        public UserEntity Authenticate(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public UserEntity Create(UserEntity user, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
