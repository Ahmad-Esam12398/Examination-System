using Examination_System.Filters;
using Examination_System.Enums;
using Examination_System.Models;
using Examination_System.Repos;
using Examination_System.Repos.Instructor;
using Examination_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Examination_System.Controllers;

[ExceptionFiltercustomed]
[Authorize(Roles="instructor")]
public class QuestionController : Controller
{

    private readonly IQuestionRepo _questionRepo;
    private readonly IInstructorRepo _instructorRepo;

    public QuestionController(IQuestionRepo questionRepo ,IInstructorRepo instructorRepo)
    {
        _questionRepo = questionRepo;
        _instructorRepo = instructorRepo;
    }

    //https://localhost:7178/question?instructorId=56072526000098&courseId=1


    public IActionResult Index(string? instructorId, int? courseId)
    {
        IEnumerable<Question> questions = _questionRepo.GetAll(instructorId, courseId);

        List<SelectListItem> instructorCourses = new List<SelectListItem>();

        if(courseId == null)
            instructorCourses.Add(new SelectListItem() { Text = "All Courses", Value = null ,Selected= true});
        else
            instructorCourses.Add(new SelectListItem() { Text = "All Courses", Value = null , Selected = false });


        foreach (Course crs in _instructorRepo.GetInstructorCourses(instructorId))
        {
            if(crs.CrsId == courseId)
                instructorCourses.Add(new SelectListItem() { Text = crs.CrsName, Value = crs.CrsId.ToString() ,Selected=true});
            else
                instructorCourses.Add(new SelectListItem() { Text = crs.CrsName, Value = crs.CrsId.ToString(), Selected = false });
        }

        SelectList instructorCoursesSL = new SelectList(instructorCourses ,"Value" ,"Text" ,"Selected");
        
         ViewBag.instructorCoursesSLVB = instructorCoursesSL;

        Console.WriteLine(ViewBag.instructorCoursesSLVB is null);
        return View(questions);
    }


    public IActionResult Create()
    {
        string id = "56072526000098";
        (ViewBag.InstructorCourses, ViewBag.questionTypeSL, ViewBag.questionWeightsSL) = InitializeSelectsForQuestions(id);

        return View();
    }

    [HttpPost]
    public IActionResult Create(Question question)
    {
        if(!ModelState.IsValid)
        {
            string errors = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage));
            return BadRequest(errors);
        }

        _questionRepo.TryAdd(question);
        return RedirectToAction(nameof(Index) ,new { instructorId = 56072526000098 });
    }

    public IActionResult Edit(int questionId)
    {
        string id = "56072526000098";

        Question? question = _questionRepo.GetById(questionId);
        if (question == null)
            return BadRequest();

        (ViewBag.InstructorCourses, ViewBag.questionTypeSL, ViewBag.questionWeightsSL) = InitializeSelectsForQuestions(id);

        return View(question);
    }

    [HttpPost]
    public IActionResult Edit(Question question)
    {

        if (question == null)
        {
            return BadRequest();
        }

        if(ModelState.IsValid)
        {
            if(! _questionRepo.TryUpdate(question))
            {
                return BadRequest();
            }
        }
        return RedirectToAction(nameof(Index), new { instructorId = "56072526000098" });
    }

    public IActionResult Details(int questionId)
    {
        Question? question = _questionRepo.GetById(questionId);

        if (question == null)
            return BadRequest();


        return View(question);
    }

    public IActionResult Delete(int courseId)
    {
        if(! _questionRepo.TryRemove(courseId))
        {
            return BadRequest();
        }
        return RedirectToAction(nameof(Index) ,new { instructorId = "56072526000098" });
    }


    public IActionResult LoadTrueFalsePartial()
    {
        return PartialView("_TrueFalsePartial");
    }

    public IActionResult LoadMultipleChoicesPartial()
    {
        return PartialView("_MultipleChoicesPartial");
    }

    private (IEnumerable<SelectListItem> , IEnumerable<SelectListItem> ,IEnumerable<SelectListItem>) InitializeSelectsForQuestions(string instructorId)
    {
        IEnumerable<SelectListItem> questionTypes = new List<SelectListItem>()
        {
            new SelectListItem(){Text="Multiple Choices" ,Value ="M"},
            new SelectListItem(){Text="True False" ,Value ="T"},
        };

        IEnumerable<SelectListItem> questionWeights = new List<SelectListItem>()
        {
            new SelectListItem(){Text="Easy" ,Value = "1"},
            new SelectListItem(){Text="Medium" ,Value = "2"},
            new SelectListItem(){Text="Hard" ,Value = "3"},
        };

        List<SelectListItem> Courses = new List<SelectListItem>();
        foreach (Course crs in _instructorRepo.GetInstructorCourses(instructorId))
        {
            Courses.Add(new SelectListItem() { Text = crs.CrsName, Value = crs.CrsId.ToString() });
        }

        SelectList CoursesSL = new SelectList(Courses, "Value", "Text");
        SelectList questionWeightsSL = new SelectList(questionWeights, "Value", "Text");
        SelectList questionTypeSL = new SelectList(questionTypes, "Value", "Text");
        
        return (CoursesSL, questionTypes, questionWeights);
    }


}
