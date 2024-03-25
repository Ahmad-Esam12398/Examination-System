using Examination_System.Filters;
using Examination_System.Repos.Student;
using Examination_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examination_System.Controllers
{
    [ExceptionFiltercustomed]

    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        public StudentController(IStudentRepo _studentRepo)
        {
            studentRepo = _studentRepo;
        }
        [Authorize(Roles = "Student")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
