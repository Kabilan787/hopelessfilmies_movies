using HopelessFilmies.Domain.Interfaces.IAdmin;
using HopelessFilmies.Domain.Interfaces.ICart;
using HopelessFilmiesMVC.Data;
using HopelessFilmiesMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HopelessFilmiesMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
        public IActionResult EditMedia(int id, string category)
        {
            ViewBag.Category = category;

            if (category == "Podcasts")
            {
                var podcast = _adminService.GetPodcastsAsync(id);
                if (podcast == null) return NotFound();
                return View("FilmForm", podcast);
            }
            else
            {
                var film = _adminService.GetFilmsAsync(id);
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

                if (category == "Podcasts")
                {
                    result = await _adminService.SavePodcastAsync(form);
                }
                else
                {
                    result = await  _adminService.SaveFilmAsync(form);
                }

                return Json(new { success = result.success, message = result.message });
            }
            catch (DbUpdateException dbEx)
            {
                // Log the inner exception for more details
                Console.WriteLine("DbUpdateException: " + dbEx.InnerException?.Message);
                return Json(new { success = false, message = $"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}" });
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
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
                        _adminService.RemovePodcastAsync(podcast);
                        _adminService.SaveChangesAsync();
                    }
                }
                else
                {
                    var film = await _adminService.GetFilmsAsync(id);
                    if (film != null)
                    {
                        _adminService.RemoveFilmAsync(film);
                        _adminService.SaveChangesAsync();
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}
