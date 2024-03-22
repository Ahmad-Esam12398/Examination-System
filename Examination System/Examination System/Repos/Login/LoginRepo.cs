using Examination_System.Data;
using Examination_System.Models;
using Examination_System.ViewModels;

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
            if (db.Students.Any(s => s.StdId == login.Id && s.StdPassword == login.Password))
            {
                var name = db.Students.Where(s => s.StdId == login.Id).Select(s => s.StdName).FirstOrDefault();
                return new UserViewModel { Id = login.Id,Password=login.Password, Role = "Student",Name = name};
            }

            if (db.Instructors.Any(i => i.InsId == login.Id && i.InsPassword == login.Password))
            {
                var name = db.Instructors.Where(i => i.InsId == login.Id).Select(i => i.InsName).FirstOrDefault();
                return new UserViewModel { Id = login.Id,Password =login.Password , Role = "Instructor",Name = name };
            }
            return null;
        }
    }
}
