using Examination_System.Data;

namespace Examination_System.Reops.Student
{
    public class StudentRepo
    {
        private readonly ITI_EXAMContext _context;
        public StudentRepo(ITI_EXAMContext context)
        {
            _context = context;
        }
    }
}
