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
            var allExams = await instructorRepo.GetAllExamsForMyCourses(instructorData);

            ViewBag.InstructorData = instructorData;
            ViewBag.AllExams = allExams;

            return View(questionList);
        }
        [HttpPost]
        public async Task<int> GenerateExam(int courseId, int TF, int duration)
        {
            try
            {
                await instructorRepo.GenerateExam(currentInstructor.InsId, courseId, TF, duration);
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        [HttpPost]
        public async Task<List<Read_Exam_QuestionsResult>> GetExamQuestions(int id)
        {
            return await instructorRepo.Read_Exam_Questions(id);
        }
        [HttpDelete]
        public async Task<int> DeleteExam(int examId)
        {
            return await instructorRepo.DeleteExam(examId);
        }
        [HttpPut]
        public async Task<IActionResult> AssignExamForTrack(int TrackId, int BranchId, int ExamId, DateOnly ExamDate, TimeOnly ExamTime)
        {
            DateTime datetime = new DateTime(ExamDate.Year, ExamDate.Month, ExamDate.Day, ExamTime.Hour, ExamTime.Minute, ExamTime.Second);
            try
            {
                await instructorRepo.AssignExamForTrack(TrackId, BranchId, ExamId, datetime);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception here
                return BadRequest(new { message = ex.Message });
            }
        }
        public async Task<List<Read_Track_From_Instructor_Course_BranchResult>> Read_Track_From_Instructor_Course_Branch(int crsId, int BranchId)
        {
            return await instructorRepo.Read_Track_From_Instructor_Course_Branch(currentInstructor.InsId, crsId, BranchId);
        }
    }
}
