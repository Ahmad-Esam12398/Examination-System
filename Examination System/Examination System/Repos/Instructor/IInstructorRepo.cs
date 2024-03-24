using Examination_System.Models;
using Examination_System.ViewModels;
using Examination_System.ViewModel.Instructor;


namespace Examination_System.Repos.Instructor;

public interface IInstructorRepo
{
    Task<Models.Instructor> GetInstructorById(string instructorId);
    IEnumerable<Course> GetInstructorCourses(string? instructorId);
    Task<List<Read_Exam_QuestionsResult>> Read_Exam_Questions(int id);
    Task<List<Read_All_Instructor_CoursesResult>> InstructorCourses(string instructorId);
    Task<List<Read_All_BranchesResult>> GetBranches();
    Task<List<Read_All_TracksResult>> GetTracks();
    Task<List<Read_Instructor_Courses_By_Instructor_IdResult>> GetInstructorData(string instructorId);
}
