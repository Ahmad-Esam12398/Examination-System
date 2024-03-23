using Examination_System.Models;
using Examination_System.ViewModels;

namespace Examination_System.Repos.Instructor;

public interface IInstructorRepo
{
    IEnumerable<Course> GetInstructorCourses(string? instructorId);
}
