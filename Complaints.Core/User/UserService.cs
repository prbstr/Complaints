using Complaints.Data.Contexts;
using Complaints.Data.DataModels;
using Complaints.Data.Entities;
using Complaints.Data.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Complaints.Core.User
{
    public interface IUserService
    {
        IEnumerable<UserEntity> GetAll();
        UserEntity Create(UserEntity user, string password);
        UserEntity Authenticate(string username, string password);
        string GenerateToken(UserEntity user);
        UserEntity GetUserById(int id);
    }

    public class UserService : IUserService
    {
        private readonly ComplaintsContext _context;
        private readonly Authentication _authenticationSettings;
        public UserService(
            ComplaintsContext context,
            IOptions<Authentication> authenticationSettings)
        {
            _context = context;
            _authenticationSettings = authenticationSettings.Value;
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

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

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
            try
            {
                var user = _context.Users.Find(id);
                return user ?? throw new AuthenticationException($"User with id: {id} not found");
            }
            catch (AuthenticationException ex)
            {
                // Log authentication exception here
                return null;
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public string GenerateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
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
