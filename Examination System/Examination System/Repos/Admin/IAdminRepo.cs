using Examination_System.Models;

namespace Examination_System.Repos.Admin
{
    public interface IAdminRepo
    {
        Task AddBranch(string branchName, string managerId);
        Task<List<Read_All_BranchesResult>> Read_All_Branches();
        Task Update_BranchResults(int branchId, string branchName, string managerId);
        Task Delete_Branch(int branchId);
    }
}
