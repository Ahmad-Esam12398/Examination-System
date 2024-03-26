using Examination_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Examination_System.Repos.Admin
{
    public interface IAdminRepo
    {
        Task AddBranch(string branchName, string managerId);
        Task<List<Read_All_BranchesResult>> Read_All_Branches();
        Task Update_BranchResults(int branchId, string branchName, string managerId);
        Task Delete_Branch(int branchId);
        Task<Branch> BranchDetails(int BranchId);
        Task<List<Read_All_TracksResult>> Read_All_Tracks();
        Branch GetTracksByBranch(int branchId);
        Track GetCourseByTrack(int trackId);
        Track GetStudentsByTrack(int trackId);
        User GetUser(string id);
        IActionResult GetCourse(int courseId);

    }
}
