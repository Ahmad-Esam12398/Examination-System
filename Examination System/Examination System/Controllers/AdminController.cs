using Examination_System.Models;
using Examination_System.Repos.Admin;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Examination_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepo adminRepo;
        public AdminController(IAdminRepo _adminRepo)
        {
            adminRepo = _adminRepo;
        }
        public async Task<IActionResult> Index()
        {
            var branches = await adminRepo.Read_All_Branches();
            ViewBag.Branches = branches;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBranch(Branch branch)
        {
            try
            {
                await adminRepo.AddBranch(branch.BranchName, branch.MgrId);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
    }
}
