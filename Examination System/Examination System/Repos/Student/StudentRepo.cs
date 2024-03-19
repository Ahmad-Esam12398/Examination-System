using Examination_System.Data;

namespace Examination_System.Repos.Student
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ITI_EXAMContext db;

        public StudentRepo(ITI_EXAMContext context)
        {
            db = context;
        }
    }
}
