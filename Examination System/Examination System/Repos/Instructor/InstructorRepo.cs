using Examination_System.Data;
using Examination_System.Models;
using Examination_System.ViewModels;
using Microsoft.EntityFrameworkCore;
using Examination_System.ViewModel.Instructor;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.Operations;
using NuGet.DependencyResolver;

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
        public async Task<Models.Instructor> GetInstructorById(string instructorId)
        {
            return await db.Instructors.FirstOrDefaultAsync(i => i.InsId == instructorId);
        }
        public async Task<List<Read_Exam_QuestionsResult>> Read_Exam_Questions(int id)
        {
            return await dbProcedures.Read_Exam_QuestionsAsync(id);
        }
        public async Task<List<Read_All_Instructor_CoursesResult>> InstructorCourses(string instructorId)
        {
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
        public async Task<List<Read_All_Exams_For_CourseIdResult>> GetAllExamsForCourseId(int courseId)
        {
            return await dbProcedures.Read_All_Exams_For_CourseIdAsync(courseId);
        }
        public async Task<List<Read_All_Exams_For_CourseIdResult>> GetAllExamsForMyCourses(List<Read_Instructor_Courses_By_Instructor_IdResult> source)
        {
            List<Read_All_Exams_For_CourseIdResult> result = new();
            foreach (var course in source)
            {
                result.AddRange(await GetAllExamsForCourseId(course.crs_id));
            }
            return result;
        }
        public async Task GenerateExam(string InstructorId, int crsId, int TF, int duration)
        {
            await dbProcedures.Exam_GenerationAsync(InstructorId, crsId, TF, duration);
        }
        public async Task<int> DeleteExam(int examId)
        {
            try
            {
                await dbProcedures.Delete_Exam_By_IdAsync(examId);
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public async Task AssignExamForTrack(int trackId, int BranchId, int ExamId, DateTime datetime)
        {
                await db.Procedures.Assign_Exam_For_TrackAsync(trackId, BranchId, ExamId, datetime);
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
        public async Task<List<Read_Track_From_Instructor_Course_BranchResult>>Read_Track_From_Instructor_Course_Branch(string InstructorId, int crs, int BranchId)
        {
            return await dbProcedures.Read_Track_From_Instructor_Course_BranchAsync(InstructorId, crs, BranchId);
        }

    }
}
