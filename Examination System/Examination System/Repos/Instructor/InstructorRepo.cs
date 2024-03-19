using Examination_System.Data;

namespace Examination_System.Repos.Instructor
{
    public class InstructorRepo : IInstructorRepo
    {
        private readonly ITI_EXAMContext db;

        public InstructorRepo(ITI_EXAMContext context)
        {
            db = context;
        }
    }
}
