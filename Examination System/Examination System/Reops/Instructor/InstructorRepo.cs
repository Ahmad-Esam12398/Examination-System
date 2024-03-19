using Examination_System.Data;

namespace Examination_System.Reops.Instructor
{
    public class InstructorRepo
    {
        private readonly ITI_EXAMContext _context;
        public InstructorRepo(ITI_EXAMContext context)
        {
            _context = context;
        }
    }
}
