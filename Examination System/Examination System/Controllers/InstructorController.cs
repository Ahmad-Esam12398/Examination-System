using Examination_System.Models;
using Examination_System.Repos.Instructor;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Index()
        {
            //List<ExamQuestionsViewModel> questionList = new List<ExamQuestionsViewModel>();
            var questionList = await instructorRepo.Read_Exam_Questions(2);
            var instructorData = await instructorRepo.GetInstructorData("29040512000017");

            ViewBag.InstructorData = instructorData;

            return View(questionList);
        }
    }
}
