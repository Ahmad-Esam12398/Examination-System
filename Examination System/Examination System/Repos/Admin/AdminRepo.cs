using Examination_System.Data;

namespace Examination_System.Repos.Admin
{
    public class AdminRepo : IAdminRepo
    {
        private readonly ITI_EXAMContext db;

        public AdminRepo(ITI_EXAMContext context)
        {
            db = context;
        }
    }
}
