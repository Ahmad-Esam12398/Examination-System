using Examination_System.Models;

namespace Examination_System.Repos;

public interface IQuestionRepo
{
    Question? GetById(int id);

    IEnumerable<Question> GetAll(string? InstructorId = null, int? courseId = null);


    bool TryAdd(Question question);

    bool TryRemove(int id);

    bool TryUpdate(Question question);


    IEnumerable<string> GetQuestionAvailableWeights();

}
