using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PUCITians_WEB.Models;

namespace PUCITians_WEB.Controllers;

[Authorize(Roles = "Teacher")]
public class TeacherController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public TeacherController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var model = new TeacherHomeViewModel
        {
            Name = user?.FullName ?? User.Identity?.Name ?? "Teacher",
            Classes = ["Web Engineering - Lab", "Software Engineering - Morning"],
            Marksheet =
            [
                "Lab attendance - Visible to students",
                "Quiz raw scores - Hidden",
                "Commented marks - Visible"
            ]
        };
        return View(model);
    }
}
