using Examination_System.Data;
using Examination_System.Models;
using Examination_System.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Examination_System.Repos.Instructor
{
    public class InstructorRepo : IInstructorRepo
    {
        private readonly ITI_EXAMContext db;

        public InstructorRepo(ITI_EXAMContext context)
        {
            db = context;
        }

        public IEnumerable<Course>? GetInstructorCourses(string? instructorId)
        {
            try
            {
                IEnumerable<Course> instructorCourses = db.InstructorTeachCourseForTrackInBranches
                                                    .Include(item => item.Crs)
                                                    .Where(item => item.InsId == instructorId)
                                                    .Select(item => item.Crs).Distinct();

                return instructorCourses;
            }
            catch
            {
                throw new Exception("can't get instructor courses");
            }
        }
    }
}
