using Microsoft.AspNetCore.Identity;

namespace PUCITians_WEB.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? RollNumber { get; set; }
}
