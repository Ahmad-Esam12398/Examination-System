using Examination_System.Models;
using Examination_System.Repos.Instructor;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Index()
        {
            List<Question> questionList = new List<Question>
            {
                new Question { QuesId = 1, QuesTittle = "What is your name?", Choice = new Choice 
                { A = "Ahmed", B = "Ali", C = "Amr", D = "Omar"}
                },
                new Question { QuesId = 2, QuesTittle = "What is your age?" , Choice = new Choice
                {A = "20", B = "25", C = "30", D = "35" }
                },
                new Question { QuesId = 3, QuesTittle = "What is your address?", Choice = new Choice
                {A = "Cairo", B = "Giza", C = "Alex", D = "Aswan"} }
            };
            return View(questionList);
        }
    }
}
