using Examination_System.Repos.Student;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.Reporting.WebForms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Examination_System.Models;

namespace Examination_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public StudentController(IStudentRepo _studentRepo, IWebHostEnvironment hostingEnvironment)
        {
            studentRepo = _studentRepo;
            _hostingEnvironment = hostingEnvironment;
        }
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

        public IActionResult StudentPastExams()
        {
            List<StudentExamGrade> studentExamGrade = studentRepo.GetPastExams("13579246801357");
            
            return View(studentExamGrade);
        }
    }
}
