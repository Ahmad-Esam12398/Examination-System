using Examination_System.Models;

namespace Examination_System.Repos;

public interface IQuestionRepo
{
    Question? GetById(int id);

    IEnumerable<Question> GetAll();

    IEnumerable<Question> GetAllQuestionsByInstructor(int instructorId);

    IEnumerable<Question> GetAllQuestionsForCourse(int courseId);

    IEnumerable<Question> GetQuestionsByInstructorForCourse(int instructorId, int courseId);


    bool TryAdd(Question question);

    bool TryRemove(int id);

    bool TryUpdate(int id, Question question);

}
