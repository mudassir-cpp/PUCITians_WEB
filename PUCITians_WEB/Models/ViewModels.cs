using System.ComponentModel.DataAnnotations;

namespace PUCITians_WEB.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
}

public class RegisterViewModel
{
    [Required]
    public string FullName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    public string? RollNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required]
    public string Role { get; set; } = "Student";
}

public class LandingViewModel
{
    public List<string> Features { get; set; } = new();
}

public class StudentHomeViewModel
{
    public string Name { get; set; } = "";
    public List<string> Classes { get; set; } = new();
    public List<string> Marks { get; set; } = new();
    public List<string> Wiki { get; set; } = new();
}

public class TeacherHomeViewModel
{
    public string Name { get; set; } = "";
    public List<string> Classes { get; set; } = new();
    public List<string> Marksheet { get; set; } = new();
}

public class AdminHomeViewModel
{
    public string Name { get; set; } = "";
    public List<string> Classes { get; set; } = new();
    public List<string> PendingPosts { get; set; } = new();
}
