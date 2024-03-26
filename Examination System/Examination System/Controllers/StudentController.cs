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
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examination_System.Controllers
{
    [ExceptionFiltercustomed]

    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private Student currentStudent;
        public StudentController(IStudentRepo _studentRepo, IWebHostEnvironment hostingEnvironment)
        {
            studentRepo = _studentRepo;
            _hostingEnvironment = hostingEnvironment;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            currentStudent = studentRepo.GetStudentById(userId);
            base.OnActionExecuting(context);
        }
        public IActionResult Info()
        {
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
        public async Task<IActionResult> IncomingExams()
        {
            var model = studentRepo.GetStudentById(currentStudent.StdId);
            var Exams = await studentRepo.GetIncomingExamsForStudent(currentStudent.StdId);
            ViewBag.Exams = Exams;
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
