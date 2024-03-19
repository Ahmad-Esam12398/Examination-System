using Examination_System.Data;

namespace Examination_System.Reops.Admin
{
    public class AdminRepo
    {
        private readonly ITI_EXAMContext _context;
        public AdminRepo(ITI_EXAMContext context)
        {
            _context = context;
        }
    }
}
