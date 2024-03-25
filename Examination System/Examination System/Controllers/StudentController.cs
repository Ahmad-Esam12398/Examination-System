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
        [Authorize(Roles = "Student")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StudentExamAnswers(int ExamId,string StudentId)
        {
            //string rootpath = _hostingenvironment.contentrootpath;
            //string reportpath = "d:\\iti\\projects\\examination-system\\examination system\\itiexamreports\\student_exam_answers.rdl";
            //ReportViewer reportViewer = new ReportViewer();
            //reportViewer.ProcessingMode = ProcessingMode.Local;
            //reportviewer.sizetoreportcontent = true;
            //reportViewer.LocalReport.ReportPath = reportpath;

            //ViewBag.ReportViewer = reportViewer;
            //int exId = 1;
            //string stId = "12345678901234";
            return Redirect($"http://localhost/ReportServer/Pages/ReportViewer.aspx?%2fITIExamReports%2fStudent_Exam_Answers&rs:Command=Render&examId={ExamId}&studentId={StudentId}");
            //return View();
        }
        public IActionResult Info(string id)
        {
            var model = studentRepo.GetStudentById(id);
            var track = model.Track;
            var courses = studentRepo.GetCourses(id);
            var branch = model.Branch.BranchName;
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
            var Exams = studentRepo.GetIncomingExamsForStudent(id);
            var model = studentRepo.GetStudentById(id);
            var track = studentRepo.GetTrack(id);
            var branch = studentRepo.GetBranch(id);
            return View(model);
        }

        public IActionResult StudentPastExams()
        {
            List<StudentExamGrade> studentExamGrade = studentRepo.GetPastExams("13579246801357");
            
            return View(studentExamGrade);
        }
    }
}
