using System;
using System.Linq;
using DogShowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DogShowAPI.Services
{
    public interface IUserService
    {
        User GetUserByID(int id);
    }

    public class UserService : IUserService
    {
       private DogShowContext context;

        public UserService(DogShowContext context)
        {
            this.context = context;
        }

        public User GetUserByID(int id)
        {
            return context.User.Where(u => u.UserId == id).Include(u => u.UsersSecurity).FirstOrDefault();
        }
    }
}