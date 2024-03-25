using Examination_System.Enums;
using Examination_System.Models;
using Examination_System.Repos;
using Examination_System.Repos.Instructor;
using Examination_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Examination_System.Controllers;

[Authorize(Roles = "Instructor")]
public class QuestionController : Controller
{
    private readonly IQuestionRepo _questionRepo;
    private readonly IInstructorRepo _instructorRepo;
    private Instructor currentInstructor;
    public QuestionController(IQuestionRepo questionRepo ,IInstructorRepo instructorRepo)
    {
        _questionRepo = questionRepo;
        _instructorRepo = instructorRepo;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        currentInstructor =  _instructorRepo.GetInstructorById(userId).Result;

        base.OnActionExecuting(context);
    }

    //https://localhost:7178/question?instructorId=56072526000098&courseId=1


    public IActionResult Index(int? courseId)
    {
        IEnumerable<Question> questions = _questionRepo.GetAll(currentInstructor.InsId, courseId);

        List<SelectListItem> instructorCourses = new List<SelectListItem>();

        if(courseId == null)
            instructorCourses.Add(new SelectListItem() { Text = "All Courses", Value = null ,Selected= true});
        else
            instructorCourses.Add(new SelectListItem() { Text = "All Courses", Value = null , Selected = false });


        foreach (Course crs in _instructorRepo.GetInstructorCourses(currentInstructor.InsId))
        {
            if(crs.CrsId == courseId)
                instructorCourses.Add(new SelectListItem() { Text = crs.CrsName, Value = crs.CrsId.ToString() ,Selected=true});
            else
                instructorCourses.Add(new SelectListItem() { Text = crs.CrsName, Value = crs.CrsId.ToString(), Selected = false });
        }

        SelectList instructorCoursesSL = new SelectList(instructorCourses ,"Value" ,"Text" ,"Selected");
        
        ViewBag.instructorCoursesSLVB = instructorCoursesSL;

        return View(questions);
    }


    public IActionResult Create()
    {
       
        (ViewBag.InstructorCourses, ViewBag.questionTypeSL, ViewBag.questionWeightsSL) = InitializeSelectsForQuestions(currentInstructor.InsId);

        
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
        question.InsId = currentInstructor.InsId;
        _questionRepo.TryAdd(question);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int questionId)
    {
        Question? question = _questionRepo.GetById(questionId);
        if (question == null)
            return BadRequest();

        (ViewBag.InstructorCourses, ViewBag.questionTypeSL, ViewBag.questionWeightsSL) = InitializeSelectsForQuestions(currentInstructor.InsId);

        return View(question);
    }

    [HttpPost]
    public IActionResult Edit(Question question)
    {

        if (question == null)
        {
            return BadRequest();
        }


        if (ModelState.IsValid)
        {

            if(! _questionRepo.TryUpdate(question))
            {
                return BadRequest();
            }
        }
        return RedirectToAction(nameof(Index));
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
        return RedirectToAction(nameof(Index));
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
