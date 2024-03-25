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
        public UserViewModel AuthenticateUser(UserViewModel login, string Role)
        {
            if(Role == "Student")
            {
                if (db.Students.Any(s => s.StdId == login.Id && s.StdPassword == login.Password))
                {
                    var name = db.Students.Where(s => s.StdId == login.Id).Select(s => s.StdName).FirstOrDefault();
                    return new UserViewModel { Id = login.Id,Password=login.Password, Role = "Student",Name = name};
                }
            }
            else if(Role == "Instructor")
            {
                if (db.Instructors.Any(i => i.InsId == login.Id && i.InsPassword == login.Password))
                {
                    var name = db.Instructors.Where(i => i.InsId == login.Id).Select(i => i.InsName).FirstOrDefault();
                    return new UserViewModel { Id = login.Id,Password =login.Password , Role = "Instructor",Name = name };
                }
            }

            /////add admin table then uncomment\\\\\\
            //else if(Role == "Admin")
            //{
            //    if (db.Admins.Any(a => a.AdminId == login.Id && a.AdminPassword == login.Password))
            //    {
            //        var name = db.Admins.Where(a => a.AdminId == login.Id).Select(a => a.AdminName).FirstOrDefault();
            //        return new UserViewModel { Id = login.Id,Password =login.Password , Role = "Admin",Name = name };
            //    }
            //}


            //if (db.Students.Any(s => s.StdId == login.Id && s.StdPassword == login.Password))
            //{
            //    var name = db.Students.Where(s => s.StdId == login.Id).Select(s => s.StdName).FirstOrDefault();
            //    return new UserViewModel { Id = login.Id,Password=login.Password, Role = "Student",Name = name};
            //}

            //if (db.Instructors.Any(i => i.InsId == login.Id && i.InsPassword == login.Password))
            //{
            //    var name = db.Instructors.Where(i => i.InsId == login.Id).Select(i => i.InsName).FirstOrDefault();
            //    return new UserViewModel { Id = login.Id,Password =login.Password , Role = "Instructor",Name = name };
            //}
            return null;
        }
        public UserViewModel GetUserById(string id)
        {
            if (db.Students.Any(s => s.StdId == id))
            {
                var name = db.Students.Where(s => s.StdId == id).Select(s => s.StdName).FirstOrDefault();
                return new UserViewModel { Id = id, Role = "Student", Name = name };
            }

            if (db.Instructors.Any(i => i.InsId == id))
            {
                var name = db.Instructors.Where(i => i.InsId == id).Select(i => i.InsName).FirstOrDefault();
                return new UserViewModel { Id = id, Role = "Instructor", Name = name };
            }
            return null;
        }
        public void changePassword(UserViewModel user)
        {
            if (user.Role == "Student")
            {
                var student = db.Students.Find(user.Id);
                student.StdPassword = user.Password;
                db.SaveChanges();
            }
            else
            {
                var instructor = db.Instructors.Find(user.Id);
                instructor.InsPassword = user.Password;
                db.SaveChanges();
            }
        }
    }
}
