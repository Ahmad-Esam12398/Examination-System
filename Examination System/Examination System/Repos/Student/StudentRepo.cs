using Examination_System.Data;
using Examination_System.Models;

namespace Examination_System.Repos.Student
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ITI_EXAMContext db;

        public StudentRepo(ITI_EXAMContext context)
        {
            db = context;
        }
        public List<StudentExamGrade> GetPastExams(string StudentId)
        {
            return db.StudentExamGrades.Where(x => x.StdId==StudentId).ToList();
        }
    }
}
