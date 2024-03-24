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
    Task<List<Read_All_Exams_For_CourseIdResult>> GetAllExamsForCourseId(int courseId);
    Task<List<Read_All_Exams_For_CourseIdResult>> GetAllExamsForMyCourses(List<Read_Instructor_Courses_By_Instructor_IdResult> source);
    Task GenerateExam(string InstructorId, int crsId, int TF, int duration);
    Task<int> DeleteExam(int examId);
    Task AssignExamForTrack(int trackId, int BranchId, int ExamId, DateTime datetime);

}
