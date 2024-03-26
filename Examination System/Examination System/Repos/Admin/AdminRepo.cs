using Examination_System.Data;
using Examination_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Examination_System.Repos.Admin
{
    public class AdminRepo : IAdminRepo
    {
        private readonly ITI_EXAMContext db;
        private readonly IITI_EXAMContextProcedures dbProcedures;
        public AdminRepo(ITI_EXAMContext context, IITI_EXAMContextProcedures _dbProcedures)
        {
            db = context;
            dbProcedures = _dbProcedures;
        }
        
        public async Task AddBranch(string branchName, string managerId)
        {
            await dbProcedures.Add_BranchAsync(branchName, managerId);
        }
        public async Task<List<Read_All_BranchesResult>> Read_All_Branches()
        {
            return await dbProcedures.Read_All_BranchesAsync();
        }
        public async Task Update_BranchResults(int branchId, string branchName, string managerId)
        {
             await dbProcedures.Update_BranchAsync(branchId, branchName, managerId);
        }
        public async Task Delete_Branch(int branchId)
        {
            await dbProcedures.Delete_BranchAsync(branchId);
        }
       
        public async Task<Branch> BranchDetails(int BranchId)
        {
            var branch = await db.Branches.FindAsync(BranchId);
            return branch;
        }
        public async Task<List<Read_All_TracksResult>> Read_All_Tracks()
        {
            return await dbProcedures.Read_All_TracksAsync();
        }
        public Branch GetTracksByBranch(int branchId)
        {
            return db.Branches.Include(b => b.Tracks).FirstOrDefault(b => b.BranchId == branchId);
            
        }
        public Track GetCourseByTrack(int trackId)
        {
            return db.Tracks.Include(t => t.Crs).FirstOrDefault(t => t.TrackId == trackId);
        }
        public Track GetStudentsByTrack(int trackId)
        {
           return db.Tracks.Include(t => t.Students).FirstOrDefault(t => t.TrackId == trackId);
        }
        public User GetUser(string id)
        {
            return db.Users.Find(id);
        }
        public IActionResult GetCourse(int courseId)
        {
			return new JsonResult(db.Courses.Find(courseId));
		}
    }
}
