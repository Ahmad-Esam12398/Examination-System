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
        private string instructorId = "29040512000017";
        private string crs_id = "2";

        public InstructorRepo(ITI_EXAMContext context)
        {
            db = context;
        }
        public List<ExamQuestionsViewModel> Read_Exam_Questions(int id)
        {
            var check = db.Exams.FirstOrDefault(e => e.ExId == id);
            if (check == null)
            {
                return null;
            }
            var examQuestions = new List<ExamQuestionsViewModel>();

            using (var command = db.Database.GetDbConnection().CreateCommand())
            { 
                command.CommandText = "Read_Exam_Questions";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ExamId", id));

                db.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    while(result.Read())
                    {
                        var examQuestion = new ExamQuestionsViewModel()
                        {
                            Id = result.GetInt32(0),
                            Title = result.GetString(1),
                            Choices = result.GetString(2)
                        };
                        examQuestions.Add(examQuestion);
                    }
                }
            }
            return examQuestions;
        }
        public List<CourseViewModel> InstructorCourses(string instructorId)
        {
            List<CourseViewModel> InstructorCourses = new List<CourseViewModel>();
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "Read_All_Instructor_Courses";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@instructorId", instructorId));

                db.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var course = new CourseViewModel()
                        {
                            Id = result.GetInt32(0),
                            Name = result.GetString(1)
                        };
                        InstructorCourses.Add(course);
                    }
                }
            }
            return InstructorCourses;
        }
    }
}
