using Examination_System.Data;
using Examination_System.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Examination_System.Repos;

public class QuestionRepo : IQuestionRepo
{
    private readonly ITI_EXAMContext _context;

    public QuestionRepo(ITI_EXAMContext context)
    {
        _context = context;
    }

    public IEnumerable<Question> GetAll(string? InstructorId = null ,int? courseId =null)
    {

        try
        {
            IEnumerable<Question> questions;

            if(InstructorId != null && courseId !=null)
            {
                questions = _context.Questions.Include(q => q.Choice)
                                            .Include(q => q.Crs)
                                            .Where(q => q.CrsId == courseId && q.InsId == InstructorId);

                //SqlParameter instructorIdParam = new SqlParameter("@InstructorId",InstructorId);
                //SqlParameter courseIdParam = new SqlParameter("@courseId" ,courseId.ToString());

                //questions =
                //_context.Questions.FromSqlRaw($"exec Read_All_Questions_For_Course_By_Instructor @InstructorId ,@courseId" ,instructorIdParam,courseIdParam);
            }
            else if(InstructorId !=null)
            {
                questions = _context.Questions.Include(q => q.Choice)
                                               .Include(q => q.Crs)        
                                            .Where(q => q.InsId == InstructorId);
                //SqlParameter instructorIdParam = new SqlParameter(InstructorId, System.Data.SqlDbType.VarChar);
                //questions = _context.Questions.FromSql($"exec Read_All_Questions_For_Course_By_Instructor {instructorIdParam} ,{courseIdParam}");


            }
            else if(courseId != null)
            {
                questions = _context.Questions.Include(q => q.Choice)
                                                .Include(q => q.Crs)
                                                .Where(q => q.CrsId == courseId);
            }
            else
            {
                questions = _context.Questions.Include(q => q.Choice)
                                                .Include(q => q.Crs);
            }

            return questions;

        }catch
        {
            throw new Exception("can't get all questions");
        }

    }

    public Question? GetById(int id)
    {
        try
        {
            return _context.Questions
                            .Include(q => q.Choice)
                            .FirstOrDefault(q => q.QuesId == id);
        }
        catch
        {
            throw;
            throw new Exception($"can't get question with id ={id}");
        }

    }

    public IEnumerable<string> GetQuestionAvailableWeights()
    {
        throw new NotImplementedException();
    }

    public bool TryAdd(Question question)
    {
        try
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool TryRemove(int id)
    {
        try
        {
            Question question = GetById(id);

            if (question == null)
                return false;

            _context.Choices.Remove(question.Choice);
            _context.Questions.Remove(question);
         
            _context.SaveChanges();
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool TryUpdate(Question question)
    {
        Question existingQuestion = GetById(question.QuesId);

        if (existingQuestion == null)
            throw new Exception($"can't update question");

        try
        {
            existingQuestion.QuesTittle = question.QuesTittle;
            existingQuestion.QuesAnswer = question.QuesAnswer;
            existingQuestion.QuesWeight = question.QuesWeight;
            existingQuestion.Choice = question.Choice;
            existingQuestion.Crs = question.Crs;

            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
