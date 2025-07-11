using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using HopelessFilmiesMVC.Data;
using HopelessFilmies.Domain.Interfaces.IAccount;



namespace HopelessFilmiesMVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        // GET: /Account/SignIn
        [HttpGet]
        public IActionResult SignIn(string? message)
        {
            ViewBag.Message = message;
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            var user = await _accountService.ValidateUserAsync(email, password);

            if (user != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            // new Claim(ClaimTypes.Role, "User") // Optional
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Invalid email/phone or password.";
            return View();
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

            _accountService.AddUser(fullName, email, password);
            await _accountService.SaveChangesAsync();

            return RedirectToAction("SignIn", new { message = "Account created successfully. Please sign in." });
        }

        [HttpGet]
        public async Task<IActionResult> MyMovies()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", new { message = "Please sign in to view your movies." });
            }

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("SignIn", new { message = "User email not found." });
            }

            var films = await _accountService.GetPurchasedMoviesAsync(email);
            return View(films);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveMovie([FromBody] string title)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized();

            var result = await _accountService.RemovePurchasedMovieAsync(email, title);

            if (result)
                return Json(new { success = true });
            else
                return Json(new { success = false, message = "Movie not found or couldn't be removed." });
        }


    }
}