using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PUCITians_WEB.Data;
using PUCITians_WEB.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

await Seed(app);

app.Run();

static async Task Seed(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    foreach (var role in new[] { "Student", "Teacher", "Admin" })
    {
        if (!await roles.RoleExistsAsync(role))
            await roles.CreateAsync(new IdentityRole(role));
    }

    await AddUser(users, "student@pucit.edu.pk", "Mudassir Ashraf", "BSEF24M015", "Student");
    await AddUser(users, "teacher@pucit.edu.pk", "Teacher", "", "Teacher");
    await AddUser(users, "admin@pucit.edu.pk", "Admin", "", "Admin");
}

static async Task AddUser(UserManager<ApplicationUser> users, string email, string name, string roll, string role)
{
    if (await users.FindByEmailAsync(email) != null)
        return;

    var user = new ApplicationUser
    {
        UserName = email,
        Email = email,
        EmailConfirmed = true,
        FullName = name,
        RollNumber = roll
    };

    var result = await users.CreateAsync(user, "Password@123");
    if (!result.Succeeded)
        throw new InvalidOperationException(string.Join(" ", result.Errors.Select(e => e.Description)));

    await users.AddToRoleAsync(user, role);
}
