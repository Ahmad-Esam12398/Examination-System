using Examination_System.Data;
using Examination_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Examination_System.Repos.Student
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ITI_EXAMContext db;
        private readonly IITI_EXAMContextProcedures dbProcedures;

        public StudentRepo(ITI_EXAMContext context, IITI_EXAMContextProcedures _dbProcedures)
        {
            db = context;
            dbProcedures = _dbProcedures;
        }
        public Models.Student GetStudentById(string studentId)
        {
            var student = db.Students.Include(s=> s.Track).Include(s=>s.Branch).Include(s=> s.User).FirstOrDefault(s => s.StdId == studentId);
            return student;
        }
        public Track GetTrack(string studentId)
        {
            var model = db.Students.Include(s => s.Track).ThenInclude(t => t.Crs).FirstOrDefault(s => s.StdId == studentId);
            if (model == null)
                return null;

            return model.Track;
        }
        public Branch GetBranch(string studentId)
        {
            var model = db.Students.Include(s => s.Branch).FirstOrDefault(s => s.StdId == studentId);
            if (model == null)
                return null;

            return model.Branch;
        }
        public ICollection<Course> GetCourses(string studentId)
        {
            var track = GetTrack(studentId);
            if (track == null)
                return [];

            return track.Crs;
        }
        public List<StudentExamGrade> GetPastExams(string StudentId)
        {
            return db.StudentExamGrades.Where(x => x.StdId==StudentId).ToList();
        }
        public async Task<List<Read_Exams_For_Student_IdResult>> GetIncomingExamsForStudent(string id)
        {
            return await db.Procedures.Read_Exams_For_Student_IdAsync(id);
        }

        public async Task<List<Read_Exam_QuestionsResult>> GetExamQuestions(int id)
        {
            return await db.Procedures.Read_Exam_QuestionsAsync(id);
        }
    }
}
