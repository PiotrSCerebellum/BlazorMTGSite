using Microsoft.EntityFrameworkCore;
using MTG.Models;
using static MTG.Services.Search;
using System.Drawing;
using System;
using System.Security.Cryptography;
using System.Linq;
using System.Text;

namespace MTG.Services
{
    public class Loginn
    {
        
        public class UserModel
        {
            public string? Name { get; init; }
            public string? Password { get; init; }
        }

        public Boolean CheckUser(User user, HttpContext httpContext)
        {
            MyDBContext dbContext = new MyDBContext();

            User userAttempt = dbContext.Users.FirstOrDefault(u => u.Username == user.Username);

            if (userAttempt != null)
            {
                // Check if password matches
                if (user.Password == userAttempt.Password)
                {
                    httpContext.Session.SetString("UserId", userAttempt.Username); // this  Store username in session
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        // attempt to hash passwords
        // the stored salt here is just the salt from user, we can change it later to use just user as input if needed
        // this was a plan for future feature where salt would be stored separately from the hash as extra security measure, either in a db or somewhere locally

        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit

        public static bool ValidatePassword(User user) //used string storedSalt before
        {
            MyDBContext dbContext = new MyDBContext();

            User userAttempt = dbContext.Users.FirstOrDefault(u => u.Username == user.Username);
            if (userAttempt != null)
            {
                var saltBytes = Convert.FromBase64String(userAttempt.Salt); //was storedSalt before
                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(user.Password, saltBytes, 10000);
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(KeySize)) == userAttempt.Password;
            }
            else
            {
                return false;
            }
        }

        // this can be used for registering a new user
        public void CreatePasswordHash(User user)
        {
            MyDBContext dbContext = new MyDBContext();

            using (var rng = new RNGCryptoServiceProvider())
            {
                var saltBytes = new byte[SaltSize];
                rng.GetBytes(saltBytes);

                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(user.Password, saltBytes, 10000);
                var salt = Convert.ToBase64String(saltBytes);
                var hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(KeySize));

                var attemptedUser = dbContext.Users.FirstOrDefault(u => u.Username == user.Username);
                if (attemptedUser == null)
                {
                    dbContext.Users.Add(user);
                }

                user.Password = hash;
                user.Salt = salt;

                dbContext.SaveChanges();
            }
        }
    }
}
