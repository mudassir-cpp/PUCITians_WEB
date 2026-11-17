using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PUCITians_WEB.Models;

namespace PUCITians_WEB.Controllers;

public class AccountController : Controller
{
    private static readonly string[] Roles = ["Student", "Teacher", "Admin"];

    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
            return await RedirectToRole(user);

        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return RedirectToAction("Index", "Home");

        return await RedirectToRole(user);
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
            return await RedirectToRole(user);

        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (!model.Email.EndsWith("@pucit.edu.pk", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(nameof(model.Email), "Use your @pucit.edu.pk email.");
            return View(model);
        }

        if (!Roles.Contains(model.Role))
        {
            ModelState.AddModelError(nameof(model.Role), "Pick a role.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true,
            FullName = model.FullName,
            RollNumber = model.RollNumber
        };

        var created = await _userManager.CreateAsync(user, model.Password);
        if (!created.Succeeded)
        {
            foreach (var error in created.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, model.Role);
        await _signInManager.SignInAsync(user, false);
        return await RedirectToRole(user);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    private async Task<IActionResult> RedirectToRole(ApplicationUser user)
    {
        if (await _userManager.IsInRoleAsync(user, "Admin"))
            return RedirectToAction("Index", "Admin");
        if (await _userManager.IsInRoleAsync(user, "Teacher"))
            return RedirectToAction("Index", "Teacher");
        return RedirectToAction("Index", "Student");
    }
}
