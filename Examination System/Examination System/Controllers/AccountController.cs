using Examination_System.Repos.Login;
using Microsoft.AspNetCore.Mvc;
using Examination_System.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Examination_System.Filters;

namespace Examination_System.Controllers
{
    [ExceptionFiltercustomed]
    public class AccountController: Controller
    {
        private readonly ILoginRepo loginRepo;
        public AccountController(ILoginRepo _loginRepo)
        {
            loginRepo = _loginRepo;
        }
        public IActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel login)
        {
            if(!ModelState.IsValid)
            {
                return View(login);
			}
            var user = loginRepo.AuthenticateUser(login);
            if (user == null)
            {
				ModelState.AddModelError(string.Empty, "Incorrect Id or Password");
				return View(login);
			}

            Claim c1 = new Claim(ClaimTypes.NameIdentifier,user.Id);
            Claim c2 = new Claim(ClaimTypes.Name,user.Name);
            Claim c3 = new Claim(ClaimTypes.Role,user.Role);
            ClaimsIdentity claims = new ClaimsIdentity(new[] { c1, c2,c3 }, "cookie");
            ClaimsPrincipal principal = new ClaimsPrincipal(claims);
            await HttpContext.SignInAsync(principal);

            if (user.Role == "Student")
            {
                return RedirectToAction("Info", "Student", new {id = user.Id} );
			}
			else if (user.Role == "Instructor")
            {
				return RedirectToAction("Index", "Instructor", new {id = user.Id});
			}
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public IActionResult Profile(string id)
        {
            id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
             var user = loginRepo.GetUserById(id);
            return View(user);
        }
        [HttpPost]
        public IActionResult Profile(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            loginRepo.changePassword(user);
            return RedirectToAction("Login");

        }

    }
}
