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
        UserEntity GetUserById(int id);
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
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new AuthenticationException("Username or password cannot be empty");
            }

            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
            {
                throw new AuthenticationException("Username or password is incorrect");
            }

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new AuthenticationException("Username or password is incorrect");
            }

            // authentication successful
            return user;
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

        public UserEntity GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            return user != null ? user : throw new AuthenticationException($"User with id: {id} not found");
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));   
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
            
            return true;
        }
    }
}
