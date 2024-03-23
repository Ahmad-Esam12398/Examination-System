using Examination_System.Data;
using Examination_System.Models;
using Examination_System.Repos.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Examination_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        public StudentController(IStudentRepo _studentRepo)
        {
            studentRepo = _studentRepo;
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

        [Authorize(Roles="Student")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
