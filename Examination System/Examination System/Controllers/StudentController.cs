using Examination_System.Models;
using Examination_System.Filters;
using Examination_System.Repos.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Reporting.WebForms;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examination_System.Controllers
{
    [ExceptionFiltercustomed]
    [Authorize(Roles="Student")]

    public class StudentController : Controller
    {
        IStudentRepo studentRepo;
        Student currentStudent;
        private readonly IWebHostEnvironment _hostingEnvironment;
        
        public StudentController(IStudentRepo _studentRepo, IWebHostEnvironment hostingEnvironment)
        {
            studentRepo = _studentRepo;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            currentStudent = studentRepo.GetStudentById(userId).Result;
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            ViewBag.Courses = studentRepo.GetCourses(currentStudent.StdId);
            return View(currentStudent);
        }
        public IActionResult Courses()
        {
            var track = currentStudent.Track;
            var Courses = track.Crs;
            ViewBag.track = track;
            ViewBag.courses = Courses;
            return View(currentStudent);
        }
        
        public IActionResult StudentExamAnswers(int ExamId, string StudentId)
        {
            return Redirect($"http://localhost/ReportServer/Pages/ReportViewer.aspx?%2fITIExamReports%2fStudent_Exam_Answers&rs:Command=Render&examId={ExamId}&studentId={StudentId}");
        }
        
        public IActionResult StudentPastExams()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<StudentExamGrade> studentExamGrade = studentRepo.GetPastExams(userId);
            return View(studentExamGrade);
        }
        
        public async Task<IActionResult> IncomingExams()
        {
            var model = studentRepo.GetStudentById(currentStudent.StdId);
            var Exams = await studentRepo.GetIncomingExamsForStudent(currentStudent.StdId);
            ViewBag.Exams = Exams;
            return View(model);
        }

        public async Task<IActionResult> TakeExam(int courseId, int examId)
        {
            var exams = await studentRepo.GetIncomingExamsForStudent(currentStudent.StdId);
            ViewBag.StdId = currentStudent.StdId;
            ViewBag.CrsId = courseId;
            ViewBag.examId = examId;
            ViewBag.Duration = studentRepo.GetExamById(examId).ExDuration;
            ViewBag.StudentName = currentStudent.User.Name;
            ViewBag.CrsName = studentRepo.GetCourseInfo(courseId).CrsName;
            var examQuestions = await studentRepo.GetExamQuestions(examId);
            return View(examQuestions);
        }

        public async Task<IActionResult> AnswerHandling(Dictionary<int, string> Answer,int ExamId )
        {
            studentRepo.SaveAnswers(Answer, ExamId,currentStudent.StdId);
            await studentRepo.ExamCorrection(ExamId, currentStudent.StdId);
            var grade = studentRepo.GetGrade(ExamId, currentStudent.StdId);
            var totalGrade = studentRepo.GetExamById(ExamId).ExGrade;
            ViewBag.Grade = grade;
            ViewBag.TotalGrade = totalGrade;
            return View();
        }

        
    }
}
