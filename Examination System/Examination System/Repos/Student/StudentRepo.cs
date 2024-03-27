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
        public async Task<Models.Student> GetStudentById(string studentId)
        {
            return await db.Students.Include(s => s.Track).ThenInclude(t=>t.Crs).Include(s => s.Branch).Include(s => s.User).FirstOrDefaultAsync(s => s.StdId == studentId);
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
        public async Task<List<Read_Incoming_Exams_For_Student_IdResult>> GetIncomingExamsForStudent(string id)
        {
            return await db.Procedures.Read_Incoming_Exams_For_Student_IdAsync(id);
        }
        public int GetCourseExam(List<Read_Incoming_Exams_For_Student_IdResult> source, int courseId)
        {
            return source.FirstOrDefault(e => e.crs_id == courseId).Exam_id;
        }
        
        public async Task<List<Read_Exam_QuestionsResult>> GetExamQuestions(int id)
        {
            return await db.Procedures.Read_Exam_QuestionsAsync(id);
        }
        public Exam GetExamById(int id)
        {
            return db.Exams.FirstOrDefault(e=> e.ExId == id);
        }

        public Course GetCourseInfo(int CrsId)
        {
            return db.Courses.FirstOrDefault(c => c.CrsId == CrsId);
        }

        public void SaveAnswers(Dictionary<int, string> answers, int examId,string studentId)
        {
            foreach (var answer in answers)
            {
                db.StudentTakeExams.Add(new() { ExamId = examId, StdId = studentId, QuestionId = answer.Key, Answer = answer.Value });
            }
            db.SaveChanges();
        }

        public async Task ExamCorrection(int examId, string StudentId)
        {
            await db.Procedures.Exam_CorrectionAsync(examId, StudentId);
        }

        public double GetGrade(int ExamId, string studentId)
        {
           return db.StudentExamGrades.FirstOrDefault(se => se.ExamId == ExamId && se.StdId == studentId).Grade.Value;
        }
    }
}
