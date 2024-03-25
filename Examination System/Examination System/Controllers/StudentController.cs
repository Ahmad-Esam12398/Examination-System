using Examination_System.Data;
using Examination_System.Models;
using Examination_System.Filters;
using Examination_System.Repos.Student;
using Examination_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;
//using Microsoft.Reporting.WebForms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Examination_System.Models;
using System.Security.Claims;

namespace Examination_System.Controllers
{
    [ExceptionFiltercustomed]

    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public StudentController(IStudentRepo _studentRepo, IWebHostEnvironment hostingEnvironment)
        {
            studentRepo = _studentRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Info(string id)
        {
            var model = studentRepo.GetStudentById(id);
            var track = studentRepo.GetTrack(id);
            var courses = studentRepo.GetCourses(id);
            var branch = studentRepo.GetBranch(id);
            ViewBag.Track = track;
            ViewBag.Branch = branch;
            ViewBag.Courses = courses;
            return View(model);
        }

        public IActionResult Courses(string id)
        {
            var model = studentRepo.GetStudentById(id);
            var track = studentRepo.GetTrack(id);
            var Courses = studentRepo.GetCourses(id);
            ViewBag.track = track;
            ViewBag.courses = Courses;
            return View(model);
        }

        public IActionResult IncomingExams(string id)
        {
            var model = studentRepo.GetStudentById(id);
            var track = studentRepo.GetTrack(id);
            var branch = studentRepo.GetBranch(id);

            return View(model);
        }

        [Authorize(Roles="Student")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StudentPastExams()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<StudentExamGrade> studentExamGrade = studentRepo.GetPastExams(userId);
            return View(studentExamGrade);
        }
    }
}
