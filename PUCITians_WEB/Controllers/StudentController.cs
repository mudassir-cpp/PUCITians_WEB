using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PUCITians_WEB.Models;

namespace PUCITians_WEB.Controllers;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var model = new StudentHomeViewModel
        {
            Name = user?.FullName ?? User.Identity?.Name ?? "Student",
            Classes = ["Web Engineering", "Software Engineering", "Database Systems"],
            Marks = ["Lab 3 attendance - Present", "Quiz 1 - 8/10"],
            Wiki = ["How to set up Expo for labs (Pending)"]
        };
        return View(model);
    }
}
