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

        // old code not using hashing, no longer useable
        /*
        public Boolean CheckUser(User user, HttpContext httpContext)
        {
            MyDBContext dbContext = new MyDBContext();

            User userAttempt = dbContext.Users.FirstOrDefault(u => u.Username == user.Username);

            if (userAttempt != null)
            {
              
                if (user.Password == userAttempt.Password)
                {
                    httpContext.Session.SetString("UserId", userAttempt.Username); 
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
        */
      
        private const int SaltSize = 16; 
        private const int KeySize = 32;

        public static bool ValidatePassword(User user)
        {
            MyDBContext dbContext = new MyDBContext();

            User userAttempt = dbContext.Users.FirstOrDefault(u => u.Username == user.Username);
            if (userAttempt != null)
            {
                var saltBytes = Convert.FromBase64String(userAttempt.Salt); 
                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(user.Password, saltBytes, 10000);
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(KeySize)) == userAttempt.Password;
            }
            else
            {
                return false;
            }
        }

      
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
