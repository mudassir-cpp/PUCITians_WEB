using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PUCITians_WEB.Models;

namespace PUCITians_WEB.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var model = new LandingViewModel
        {
            Features = new List<string>
            {
                "Login with @pucit.edu.pk email",
                "Join classes with invite link",
                "Class materials and announcements",
                "Limited marksheet view",
                "Student wiki with admin approval"
            }
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
