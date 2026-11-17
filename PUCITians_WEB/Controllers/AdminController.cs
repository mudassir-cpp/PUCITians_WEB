using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PUCITians_WEB.Models;

namespace PUCITians_WEB.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var model = new AdminHomeViewModel
        {
            Name = user?.FullName ?? User.Identity?.Name ?? "Admin",
            Classes = ["Web Engineering", "Software Engineering", "Database Systems"],
            PendingPosts = ["How to set up Expo for labs", "Finding past papers faster"]
        };
        return View(model);
    }
}
