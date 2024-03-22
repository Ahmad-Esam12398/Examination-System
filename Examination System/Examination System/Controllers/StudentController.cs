using Examination_System.Repos.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examination_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        public StudentController(IStudentRepo _studentRepo)
        {
            studentRepo = _studentRepo;
        }
        [Authorize(Roles="Student")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
