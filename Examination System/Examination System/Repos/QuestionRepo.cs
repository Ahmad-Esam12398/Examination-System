using Examination_System.Data;
using Examination_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Examination_System.Repos;

public class QuestionRepo : IQuestionRepo
{
    private readonly ITI_EXAMContext _context;

    public QuestionRepo(ITI_EXAMContext context)
    {
        _context = context;
    }

    public IEnumerable<Question> GetAll()
    {

        try
        {
            return _context.Questions.Include(q => q.Choices);

        }catch
        {
            throw new Exception("can't get all questions");
        }

    }

    public IEnumerable<Question> GetAllQuestionsByInstructor(int instructorId)
    {
        throw new NotImplementedException(); 
    }

    public IEnumerable<Question> GetAllQuestionsForCourse(int courseId)
    {
        throw new NotImplementedException();
    }

    public Question? GetById(int id)
    {
        try
        {
            return _context.Questions
                            .Include(q => q.Choices)
                            .FirstOrDefault(q => q.QuesId == id);
        }
        catch
        {
            throw new Exception($"can't get question with id ={id}");
        }

    }

    public IEnumerable<Question> GetQuestionsByInstructorForCourse(int instructorId, int courseId)
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

            _context.Questions.Remove(question);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool TryUpdate(int id, Question question)
    {
        Question existingQuestion = GetById(id);
        if(existingQuestion == null)
                throw new Exception($"There's no question with id ={id}");

        if (question.QuesId != id)
            throw new Exception($"can't update question id");



        try
        {
            _context.Questions.Update(question);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
