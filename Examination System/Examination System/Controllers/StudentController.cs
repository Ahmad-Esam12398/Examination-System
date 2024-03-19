using Examination_System.Repos.Student;
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
        public IActionResult Index()
        {
            return View();
        }
    }
}
