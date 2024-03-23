using Examination_System.Models;
using Examination_System.ViewModels;

namespace Examination_System.Repos.Login
{
    public interface ILoginRepo
    {
       public UserViewModel AuthenticateUser(UserViewModel login);

    }
}
