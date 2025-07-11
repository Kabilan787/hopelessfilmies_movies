using HopelessFilmiesMVC.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using HopelessFilmiesMVC.DataAccess.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HopelessFilmies.Domain.Interfaces.IAccount;
using HopelessFilmies.DataAccess.Repositories;
using HopelessFilmies.Service.Services;
using HopelessFilmies.Domain.Interfaces.IAdmin;
using HopelessFilmies.Domain.Interfaces.ICart;
using HopelessFilmies.Domain.Interfaces.IContact;
using HopelessFilmies.Domain.Interfaces.IHome;

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

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();

// THIS LINE MUST COME AFTER ALL builder.Services.Add... calls
var app = builder.Build();

// test

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
