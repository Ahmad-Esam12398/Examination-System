using Examination_System.Models;
using Examination_System.ViewModel.Instructor;

namespace Examination_System.Repos.Instructor;

public interface IInstructorRepo
{
    List<ExamQuestionsViewModel> Read_Exam_Questions(int id);
    List<CourseViewModel> InstructorCourses(string instructorId);
}
