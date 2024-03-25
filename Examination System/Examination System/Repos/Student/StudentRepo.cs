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
            var student = db.Students.Include(s => s.Track).FirstOrDefault(s => s.StdId == studentId);
            return student;
        }

        public Track GetTrack(string studentId)
        {
            var model = db.Students.Include(s => s.Track).ThenInclude(t=>t.Crs).FirstOrDefault(s=>s.StdId == studentId);
            if (model == null)                
                return null;

            return model.Track;
        }

        public Branch GetBranch(string studentId)
        {
            var model = db.Students.Include(s=>s.Branch).FirstOrDefault(s => s.StdId == studentId);
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

        //public ICollection<Exam> GetIncomingExamsForStudent(string studentId)
        //{
        //    var track = GetTrack(studentId);
        //    var branch = GetBranch(studentId);

        //    if (track == null || branch == null)
        //    {
        //        return new List<Exam>(); // Return an empty list if track or branch is not found
        //    }

        //    // Retrieve incoming exams for the student's track and branch
        //    var incomingExams = db.Exams
        //        .Include(exam => exam.TrackCourseExams)
        //        .ThenInclude(tce => tce.Crs)
        //        .Where(exam => exam.TrackCourseExams
        //            .Any(tce => tce.TrId == track.TrackId && tce.BranchId == branch.BranchId && tce.ExamDate > DateTime.Now))
        //        .ToList();

        //    return incomingExams;
        //}

        //public async Task<List<Read_All_TracksResult>> GetTracks()
        //{
        //    return await dbProcedures.Read_All_TracksAsync();
        //}

        //public async Task<List<Read_All_Track_CoursesResult>> ReadAllTrackCourses(int id)
        //{
        //    return await dbProcedures.Read_All_Track_CoursesAsync(id);
        //}

        //Task<Read_All_Track_CoursesResult> IStudentRepo.ReadAllTrackCourses(int id)
        //{
        //    throw new NotImplementedException();
        //}
        public List<StudentExamGrade> GetPastExams(string StudentId)
        {
            return db.StudentExamGrades.Where(x => x.StdId==StudentId).ToList();
        }
    }
}
