using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DogShowAPI.Helpers;
using DogShowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DogShowAPI.Services
{
    public interface IUserService
    {
        User Login(string mail, string password);
        User Create(User user, string password, int permissionLevel);
        User GetUserByID(int id);
        int GetUserPermissionLevel(int userId);
        User Update(int userID, User newData);
        bool IsUserAnOrganizator(ClaimsIdentity identity);
        bool CanUserAccessDog(ClaimsIdentity identity, int dogId);
    }

    public class UserService : IUserService
    {
        private DogShowContext context;

        public UserService(DogShowContext context)
        {
            this.context = context;
        }

        public User Login(string mail, string password)
        {
            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = context.User.Where(u => u.Email == mail).Include(u => u.UsersSecurity).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            if (!CheckPassword(password, user.UsersSecurity.UserHash, user.UsersSecurity.UserSalt))
            {
                return null;
            }

            return user;
        }

        public User Create(User user, string password, int permissionLevel)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (context.User.Where(u => u.Email == user.Email).Any())
                throw new AppException("Mail \"" + user.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            GenerateHashSalt(password, out passwordHash, out passwordSalt);

            context.User.Add(user);
            context.SaveChanges();

            UsersSecurity userSecurity = new UsersSecurity();
            userSecurity.UserId = user.UserId;
            userSecurity.UserHash = passwordHash;
            userSecurity.UserSalt = passwordSalt;
            userSecurity.PermissionLevel = permissionLevel;

            context.UsersSecurity.Add(userSecurity);
            context.SaveChanges();

            return user;
        }

        public User GetUserByID(int id)
        {
            return context.User.Where(u => u.UserId == id).Include(u => u.UsersSecurity).FirstOrDefault();
        }

        public int GetUserPermissionLevel(int userId)
        {
            UsersSecurity userS = context.UsersSecurity.Where(us => us.UserId == userId).FirstOrDefault();

            if (userS == null)
                return -1;

            return userS.PermissionLevel;
        }

        public User Update (int userID, User newData)
        {
            User user = context.User.Where(u => u.UserId == userID).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            user.FirstName = newData.FirstName;
            user.LastName = newData.LastName;
            user.Address = newData.Address;
            context.SaveChanges();
            return user;
        }

        public bool IsUserAnOrganizator(ClaimsIdentity identity)
        {
            var userName = identity.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                throw new AppException("Błąd autoryzacji");
            }
            int userId = int.Parse(userName);
            int userPermissions = GetUserPermissionLevel(userId);
            if (userPermissions != 1 && userPermissions != 2)
            {
                throw new AppException("Brak uprawnień do wykonania akcji");
            }
            return true;
        }

        private static void GenerateHashSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool CheckPassword(string password, byte[] savedHash, byte[] salt)
        {
            if (string.IsNullOrEmpty(password)) return false;

            var hmac = new System.Security.Cryptography.HMACSHA512(salt);
            var newHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < newHash.Length; i++)
            {
                if (newHash[i] != savedHash[i])
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanUserAccessDog(ClaimsIdentity identity, int dogId)
        {
            var userName = identity.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
                throw new AppException("Błąd autoryzacji");
            int userId = int.Parse(userName);
            var dog = context.Dog.Where(d => d.DogId == dogId).FirstOrDefault();
            if (dog == null)
                throw new AppException("Nie odnaleziono psa o podanym ID");
            if (dog.OwnerId == userId)
                return true;
            int userPermissions = GetUserPermissionLevel(userId);
            if (userPermissions != 1 && userPermissions != 2)
            {
                throw new AppException("Brak uprawnień do wykonania akcji");
            }
            return true;
        }
    }
}