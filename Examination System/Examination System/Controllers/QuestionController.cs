using Examination_System.Models;
using Examination_System.Repos;
using Microsoft.AspNetCore.Mvc;

namespace Examination_System.Controllers;

public class QuestionController : Controller
{
    private readonly IQuestionRepo _questionRepo;

    public QuestionController(IQuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
    }


    public IActionResult Index(int? instructorId ,int? courseId)
    {
        if(instructorId != null && courseId != null)
        {
            // all questions for a course by instructor

            IEnumerable<Question> questions = _questionRepo.GetQuestionsByInstructorForCourse(instructorId.Value ,courseId.Value);
            return View(questions);

        }
        else if(instructorId != null)
        {
            // all instructor questions
            IEnumerable<Question> questions = _questionRepo.GetAllQuestionsByInstructor(instructorId.Value);
            return View(questions);

        }
        else if(courseId != null)
        {
            // all course questions regardless whose question made by  
            IEnumerable<Question> questions = _questionRepo.GetAllQuestionsForCourse(courseId.Value);
            return View(questions);

        }
        else
        {
            // all questions for all courses by all instructors
            IEnumerable<Question> questions = _questionRepo.GetAll();
            return View(questions);
        }
    }





}
