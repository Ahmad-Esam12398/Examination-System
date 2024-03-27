using Examination_System.Models;

namespace Examination_System.Repos.Student
{
    public interface IStudentRepo
    {
        Task<Models.Student> GetStudentById(string studentId);
        public Track GetTrack(string studentId);
        public ICollection<Course> GetCourses(string studentId);
        public Branch GetBranch(string studentId);
        public List<StudentExamGrade> GetPastExams(string StudentId);
        public Task<List<Read_Incoming_Exams_For_Student_IdResult>> GetIncomingExamsForStudent(string id);
        public int GetCourseExam(List<Read_Incoming_Exams_For_Student_IdResult> source, int courseId);
        public Task<List<Read_Exam_QuestionsResult>> GetExamQuestions(int id);
    }
}
