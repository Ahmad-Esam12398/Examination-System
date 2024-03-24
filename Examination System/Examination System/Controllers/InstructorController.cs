using Examination_System.Models;
using Examination_System.Repos.Instructor;
using Microsoft.AspNetCore.Authorization;
using Examination_System.ViewModel.Instructor;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examination_System.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        const string INSTRUCTORID = "29040512000017";
        IInstructorRepo instructorRepo;
        Instructor currentInstructor;
        public InstructorController(IInstructorRepo _instructorRepo)
        {
            instructorRepo = _instructorRepo;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            currentInstructor = instructorRepo.GetInstructorById(userId).Result;

            base.OnActionExecuting(context);
        }
        public async Task<IActionResult> Index()
        {
            //List<ExamQuestionsViewModel> questionList = new List<ExamQuestionsViewModel>();
            var questionList = await instructorRepo.Read_Exam_Questions(2);
            var instructorData = await instructorRepo.GetInstructorData(currentInstructor.InsId);

            ViewBag.InstructorData = instructorData;

            return View(questionList);
        }
    }
}
