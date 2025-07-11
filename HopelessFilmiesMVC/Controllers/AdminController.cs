using HopelessFilmies.Domain.Interfaces.IAdmin;
using HopelessFilmies.Domain.Interfaces.ICart;
using HopelessFilmiesMVC.Data;
using HopelessFilmiesMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HopelessFilmiesMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IAdminService adminService, IWebHostEnvironment webHostEnvironment)
        {
            _adminService = adminService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Admin/Login
        [HttpGet]
        public IActionResult Login(string? message)
        {
            ViewBag.Message = message;
            return View();
        }

        // POST: /Admin/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var admin = _adminService.CheckAdminAsync(email, password);

            if (admin != null)
            {
                // Store a simple session flag
                HttpContext.Session.SetString("IsAdmin", "true");

                return RedirectToAction("Dashboard");
            }

            ViewBag.Message = "Invalid credentials.";
            return View();
        }

        // GET: /Admin/Dashboard
        [HttpGet]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", new { message = "Please log in as admin." });
            }

            return View(); // Will create Dashboard.cshtml later
        }


        // GET: /Admin/Manage?category=ShortFilm|Movies|Exclusive|Podcasts
        public async Task<IActionResult> Manage(string category)
        {
            var viewModel = new AdminManageViewModel
            {
                Category = category
            };


            if (category == "Podcasts")
                viewModel.Podcasts = await _adminService.GetPodcastsAsync();
            else
                viewModel.Films = await _adminService.GetFilmsAsync();

            return View("Manage", viewModel);
        }

        [HttpGet]
        public IActionResult AddMedia(string category)
        {
            ViewBag.Category = category;

            if (category == "Podcasts")
            {
                var podcast = _adminService.PrepareNewPodcast();
                return View("FilmForm", podcast);
            }

            var film = _adminService.PrepareNewFilm(category);
            return View("FilmForm", film);
        }

        [HttpGet]
        public async Task<IActionResult> EditMedia(int id, string category)
        {
            ViewBag.Category = category;

            if (category == "Podcasts")
            {
                var podcast = await _adminService.GetPodcastsAsync(id);
                if (podcast == null) return NotFound();
                return View("FilmForm", podcast);
            }
            else
            {
                var film = await _adminService.GetFilmsAsync(id);
                if (film == null) return NotFound();
                return View("FilmForm", film);
            }
        }

        

        [HttpPost]
        public async Task<IActionResult> SaveMedia(IFormCollection form)
        {
            try
            {
                string category = form["Category"];
                (bool success, string message) result;

                // Handle uploaded image
                var file = Request.Form.Files["ImageFile"];
                string imagePath = null;

                if (file != null && file.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    imagePath = "/uploads/" + uniqueFileName;
                }

                if (category == "Podcasts")
                {
                    result = await _adminService.SavePodcastAsync(form, imagePath);
                }
                else
                {
                    result = await _adminService.SaveFilmAsync(form, imagePath);
                }

                return Json(new { success = result.success, message = result.message });
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("DbUpdateException: " + dbEx.InnerException?.Message);
                return Json(new { success = false, message = $"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Exception: " + ex.Message);
                return Json(new { success = false, message = $"An unexpected error occurred: {ex.Message}" });
            }
        }



        [HttpPost]
        public async Task<IActionResult> DeleteMedia(int id, string category)
        {
            try
            {
                if (category == "Podcasts")
                {
                    var podcast = await  _adminService.GetPodcastsAsync(id);
                    if (podcast != null)
                    {
                        await _adminService.RemovePodcastAsync(podcast);
                    }
                }
                else
                {
                    var film = await _adminService.GetFilmsAsync(id);
                    if (film != null)
                    {
                        await _adminService.RemoveFilmAsync(film);
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public async Task<IActionResult> UsersDetails()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", new { message = "Please log in as admin." });
            }

            var users = await _adminService.GetUsersAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ViewContactForms()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", new { message = "Please log in as admin." });
            }

            var contacts = await _adminService.GetContactFormsAsync();
            return View(contacts);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                var success = await _adminService.DeleteContactFormAsync(id);

                if (success)
                    return Json(new { success = true });
                else
                    return Json(new { success = false, message = "Could not delete contact. Contact not found." });
            }
            catch (Exception ex)
            {
                // Optional: log ex
                return Json(new { success = false, message = "Server error while deleting contact." });
            }
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }

    }
}
