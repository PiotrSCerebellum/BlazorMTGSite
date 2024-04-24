using Microsoft.EntityFrameworkCore;
using MTG.Models;
using static MTG.Services.Services;
using System.Drawing;
using System;

namespace MTG.Services
{
    public class Login
    {
        MyDBContext dbContext = new MyDBContext();

        public class UserModel
        {
            public string? Name { get; init; }
            public string? Password { get; init; }
        }

        public Boolean CheckUser(User user)
        {
            User userAttempt = dbContext.Users.FirstOrDefault(u => u.Username == user.Username);

            if (userAttempt != null)
            {
                // Check if password matches
                if (user.Password == userAttempt.Password)
                {
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
        
    }
}
