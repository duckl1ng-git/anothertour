using anothertour.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using static System.Formats.Asn1.AsnWriter;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection_string = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddLocalization();
builder.Services.AddDbContext<anothertour.Models.ApplicationContext>(options => options.UseSqlServer(connection_string));
builder.Services.AddDbContext<anothertour.Models.IdentityContext>(options => options.UseSqlServer(connection_string));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 10;
    options.Password.RequireDigit = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
}
).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();


var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("ru-RU") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru-RU"),
    SupportedCultures = supportedCultures,
    FallBackToParentCultures = false
});
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("ru-RU");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseHsts();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


//    await roleManager.CreateAsync(new IdentityRole("admin"));
//    await roleManager.CreateAsync(new IdentityRole("user"));
//    await roleManager.CreateAsync(new IdentityRole("manager"));
//    await roleManager.CreateAsync(new IdentityRole("guide"));

//    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync("ducklig@yandex.ru"), "admin");

//}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
