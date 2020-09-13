using Complaints.Data.Contexts;
using Complaints.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Complaints.Core.User
{
    public interface IUserService
    {
        IEnumerable<UserEntity> GetAll();
        UserEntity Authenticate(string username, string password);
        UserEntity Create(UserEntity user, string password);
    }

    public class UserService : IUserService
    {
        private ComplaintsContext _context;
        public UserService(ComplaintsContext context)
        {
            _context = context;
        }

        public UserEntity Authenticate(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public UserEntity Create(UserEntity user, string password)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return _context.Users.ToList();   
        }
    }
}
