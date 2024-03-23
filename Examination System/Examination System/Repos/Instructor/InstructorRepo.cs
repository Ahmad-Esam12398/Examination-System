using Examination_System.Data;
using Examination_System.Models;
using Examination_System.ViewModel.Instructor;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Examination_System.Repos.Instructor
{
    public class InstructorRepo : IInstructorRepo
    {
        private readonly ITI_EXAMContext db;
        private readonly IITI_EXAMContextProcedures dbProcedures;
        private string instructorId = "29040512000017";
        private string crs_id = "2";

        public InstructorRepo(ITI_EXAMContext context, IITI_EXAMContextProcedures _dbProcedures)
        {
            db = context;
            dbProcedures = _dbProcedures;
        }
        public async Task<List<Read_Exam_QuestionsResult>> Read_Exam_Questions(int id)
        {
            //var check = db.Exams.FirstOrDefault(e => e.ExId == id);
            //var examQuestions = new List<ExamQuestionsViewModel>();


            //using (var command = db.Database.GetDbConnection().CreateCommand())
            //{
            //    command.CommandText = "Read_Exam_Questions";
            //    command.CommandType = CommandType.StoredProcedure;
            //    command.Parameters.Add(new SqlParameter("@ExamId", id));

            //    db.Database.OpenConnection();

            //    using (var result = command.ExecuteReader())
            //    {
            //        while(result.Read())
            //        {
            //            var examQuestion = new ExamQuestionsViewModel()
            //            {
            //                Id = result.GetInt32(0),
            //                Title = result.GetString(1),
            //                Choices = result.GetString(2)
            //            };
            //            examQuestions.Add(examQuestion);
            //        }
            //    }
            //}
            return await dbProcedures.Read_Exam_QuestionsAsync(id);
        }
        public async Task<List<Read_All_Instructor_CoursesResult>> InstructorCourses(string instructorId)
        {
            //using (var command = db.Database.GetDbConnection().CreateCommand())
            //{
            //    command.CommandText = "Read_All_Instructor_Courses";
            //    command.CommandType = CommandType.StoredProcedure;
            //    command.Parameters.Add(new SqlParameter("@instructorId", instructorId));

            //    db.Database.OpenConnection();

            //    using (var result = command.ExecuteReader())
            //    {
            //        while (result.Read())
            //        {
            //            var course = new CourseViewModel()
            //            {
            //                Id = result.GetInt32(0),
            //                Name = result.GetString(1)
            //            };
            //            InstructorCourses.Add(course);
            //        }
            //    }
            //}
            return await dbProcedures.Read_All_Instructor_CoursesAsync(instructorId);
        }
        public async Task<List<Read_All_BranchesResult>> GetBranches()
        {
            return await dbProcedures.Read_All_BranchesAsync();
        }
        public async Task<List<Read_All_TracksResult>> GetTracks()
        {
            return await dbProcedures.Read_All_TracksAsync();
        }
        public async Task<List<Read_Instructor_Courses_By_Instructor_IdResult>> GetInstructorData(string instructorId)
        {
            return await dbProcedures.Read_Instructor_Courses_By_Instructor_IdAsync(instructorId);
        }
    }
}
