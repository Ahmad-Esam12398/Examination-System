using Examination_System.Models;
using Examination_System.Repos.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AdminController : Controller
{
    private readonly IAdminRepo adminRepo;
    public AdminController(IAdminRepo _adminRepo)
    {
        adminRepo = _adminRepo;
    }

    public async Task<IActionResult> Index()
    {
        var branchesResult = await adminRepo.Read_All_Branches();
        var branches = branchesResult.Select(r => new Branch
        {
            BranchId = r.BranchId,
            BranchName = r.BranchName,
            MgrId = r.MgrId
        }).ToList();

        return View(branches);
    }
    public async Task<IActionResult> AddBranch(string branchName, string managerId)
    {
        await adminRepo.AddBranch(branchName, managerId);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> UpdateBranch(int id)
    {
        var branch = await adminRepo.BranchDetails(id);
        return View(branch);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBranch(int branchId, string branchName, string managerId)
    {
        await adminRepo.Update_BranchResults(branchId, branchName, managerId);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> DeleteBranch(int branchId)
    {
        await adminRepo.Delete_Branch(branchId);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> BranchDetails(int id)
    {
        var branch = await adminRepo.BranchDetails(id);
        return View(branch);
    }
    //public async Task<IActionResult> Tracks()
    //{
    //    var trackResult = await adminRepo.Read_All_Tracks();
    //    var tracks = trackResult.Select(r => new Track
    //    {
    //        TrackId = r.track_id,
    //        TrackName = r.track_name,
    //        SupId = r.sup_id
    //    }).ToList();

    //    return View();
    //}

    public IActionResult showTracksByBranch(int branchId)
    {
        var branch = adminRepo.GetTracksByBranch(branchId);
        return View(branch);
    }
    public IActionResult showCoursesByTrack(int trackId)
    {
        var track = adminRepo.GetCourseByTrack(trackId);
        return View(track);
    }
	public IActionResult showStudentsByTrack(int trackId)
	{
		var track = adminRepo.GetStudentsByTrack(trackId);

		// Fetch user data for the first student in the track
		var firstStudent = track.Students.FirstOrDefault();
		if (firstStudent != null)
		{
			var user = adminRepo.GetUser(firstStudent.StdId);
			ViewBag.StudentName = user.Name;
			ViewBag.StudentPhone = user.Mobile;
		}

		return View(track);
	}
    public IActionResult CourseIndex(int courseId)
    {
        var course = adminRepo.GetCourse(courseId);
		return View(course);
	}




}
