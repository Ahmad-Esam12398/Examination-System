using Examination_System.Models;

namespace Examination_System.Repos.Student
{
    public interface IStudentRepo
    {
        public List<StudentExamGrade> GetPastExams(string StudentId);
    }
}
