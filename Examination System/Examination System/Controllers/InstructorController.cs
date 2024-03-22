using Examination_System.Models;
using Examination_System.Repos.Instructor;
using Examination_System.ViewModel.Instructor;
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
            List<ExamQuestionsViewModel> questionList = new List<ExamQuestionsViewModel>();
                questionList = instructorRepo.Read_Exam_Questions(2);
            List<CourseViewModel> courses = new List<CourseViewModel>();
            courses = instructorRepo.InstructorCourses("29040512000017");
            ViewBag.InstructorCourses = courses;
            return View(questionList);
        }
    }
}
