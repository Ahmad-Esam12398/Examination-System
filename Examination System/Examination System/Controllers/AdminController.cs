using Examination_System.Filters;
using Examination_System.Repos.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Examination_System.Controllers
{
    [ExceptionFiltercustomed]

    public class AdminController : Controller
    {
        private readonly IAdminRepo adminRepo;
        public AdminController(IAdminRepo _adminRepo)
        {
            adminRepo = _adminRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
