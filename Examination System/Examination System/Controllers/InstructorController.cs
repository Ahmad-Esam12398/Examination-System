using Examination_System.Repos.Instructor;
using Microsoft.AspNetCore.Mvc;

namespace Examination_System.Controllers
{
    public class InstructorController : Controller
    {
        IInstructorRepo instructorRepo;
        public InstructorController(IInstructorRepo _instructorRepo)
        {
            instructorRepo = _instructorRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
