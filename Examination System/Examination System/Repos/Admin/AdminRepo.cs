using Examination_System.Data;
using Examination_System.Models;

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
    }
}
