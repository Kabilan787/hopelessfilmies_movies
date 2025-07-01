using HopelessFilmiesMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using HopelessFilmiesMVC.Data;



namespace HopelessFilmiesMVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserDbContext _context;

        public AccountController(UserDbContext context)
        {
            _context = context;
        }


        // GET: /Account/SignIn
        [HttpGet]
        public IActionResult SignIn(string? message)
        {
            ViewBag.Message = message;
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password) //, string? returnUrl = null)
        {

            // Check if user exists in the database
            var user = _context.Users
                .FirstOrDefault(u =>
                    (u.Email == email ) &&
                    u.Password == password); 

            if (user != null)
            {
                // User found, create claims
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                // Optionally add: new Claim(ClaimTypes.Role, "User")
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Invalid credentials
                ViewBag.Message = "Invalid email/phone or password.";
                return View();
            }
        }





        // GET: /Account/SignOut
        [HttpGet] // Often a POST for security, but GET for simple redirects is common for logout
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Redirect to home after logout
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View(); // Create an AccessDenied.cshtml view for this
        }

        // GET: /Account/SignUp
        [HttpGet]
        public IActionResult SignUp(string? message)
        {
            ViewBag.Message = message;  
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string fullName, string email, string phone, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Message = "Passwords do not match.";
                return View();
            }

            // Save to DB
            var user = new User
            {
                FullName = fullName,
                Email = email,
                Password = password 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("SignIn", new { message = "Account created successfully. Please sign in." });
        }

        [HttpGet]
        public IActionResult MyMovies()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", new { message = "Please sign in to view your movies." });
            }

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || string.IsNullOrEmpty(user.PurchasedMovies))
            {
                return View(new List<Film>());
            }

            var movieTitles = user.PurchasedMovies
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(m => m.Trim())
                .ToList();

            var purchasedFilms = _context.Films
                .Where(f => movieTitles.Contains(f.Heading))
                .ToList();

            return View(purchasedFilms);
        }


        [HttpPost]
        public IActionResult RemoveMovie([FromBody] string title)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return Unauthorized();

            var movies = (user.PurchasedMovies ?? "")
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(m => m.Trim())
                            .ToList();

            if (movies.Contains(title))
            {
                movies.Remove(title);
                user.PurchasedMovies = string.Join(", ", movies);
                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Movie not found." });
        }


    }
}