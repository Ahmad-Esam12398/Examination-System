using Examination_System.Data;
using Examination_System.Models;
using Examination_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Examination_System.Repos.Login
{
    public class LoginRepo: ILoginRepo
    {
        private readonly ITI_EXAMContext db;
        
        public LoginRepo() { 
        
        }
        public LoginRepo(ITI_EXAMContext context)
        {
            db = context;
        }
        public UserViewModel AuthenticateUser(UserViewModel login)
        {
            var user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == login.Id && u.Password == login.Password);
            if(user == null)
            {
                return null;
            }
            var name = user.Name;
            return new UserViewModel { Id = login.Id,Password=login.Password, Role = user.Role.RoleName, Name = name};
        }
        public UserViewModel GetUserById(string id)
        {
            if (db.Users.Any(u => u.Id == id))
            {
                var user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
                var name = user.Name;
                if(user.Role.RoleName == "Student")
                    return new UserViewModel { Id = id, Role = "Student", Name = name };
                else if(user.Role.RoleName == "Instructor")
                    return new UserViewModel { Id = id, Role = "Instructor", Name = name };
                else if(user.Role.RoleName == "Admin")
                    return new UserViewModel { Id = id, Role = "Admin", Name = name };
            }
            return null;
        }
        public void changePassword(UserViewModel user)
        {
                var User = db.Users.Find(user.Id);
                User.Password = user.Password;
                db.SaveChanges();
            

        }
    }
}
