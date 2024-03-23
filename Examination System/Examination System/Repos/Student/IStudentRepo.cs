using Examination_System.Models;

namespace Examination_System.Repos.Student
{
    public interface IStudentRepo
    {
        public Models.Student GetStudentById(string studentId);
        public Track GetTrack(string studentId);
        public ICollection<Course> GetCourses(string studentId);
        public Branch GetBranch(string studentId);
    }
}
