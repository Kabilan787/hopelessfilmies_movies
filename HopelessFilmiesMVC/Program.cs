using HopelessFilmiesMVC.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ALL SERVICE REGISTRATIONS GO HERE (before builder.Build())

builder.Services.AddDbContext<UserDbContext>(options=> 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/SignIn"; // Redirect to this path if unauthenticated
        options.LogoutPath = "/Account/SignOut"; // Path for logout
        options.AccessDeniedPath = "/Account/AccessDenied"; // Path if user tries to access unauthorized resources
    });

builder.Services.AddAuthorization(); // Add authorization services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


// THIS LINE MUST COME AFTER ALL builder.Services.Add... calls
var app = builder.Build();

// Configure the HTTP request pipeline (middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANT: Authentication and Authorization middleware must be placed between UseRouting and UseEndpoints
app.UseAuthentication(); // This enables authentication
app.UseAuthorization();  // This enables authorization

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
