using Complaints.Data.Contexts;
using Complaints.Data.Entities;
using Complaints.Data.ViewModels;
using System;
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
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AuthenticationException("Password is required");
            }

            if (_context.Users.Any(x => x.Username == user.Username))
            {
                throw new AuthenticationException("Username \"" + user.Username + "\" is already taken");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return _context.Users.ToList();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));   
        }
    }
}
