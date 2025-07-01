using HopelessFilmiesMVC.Data;
using HopelessFilmiesMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HopelessFilmiesMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserDbContext _context;

        public AdminController(UserDbContext context)
        {
            _context = context;
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
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);

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
        public IActionResult Manage(string category)
        {
            var viewModel = new AdminManageViewModel
            {
                Category = category
            };

            if (category == "Podcasts")
                viewModel.Podcasts = _context.Podcasts.ToList();
            else
                viewModel.Films = _context.Films.Where(f => f.Category == category).ToList();

            return View("Manage", viewModel);
        }

        [HttpGet]
        public IActionResult AddMedia(string category)
        {
            ViewBag.Category = category;

            if (category == "Podcasts")
                return View("FilmForm", new Podcast { });

            return View("FilmForm", new Film { Category = category });
        }

        [HttpGet]
        public IActionResult EditMedia(int id, string category)
        {
            ViewBag.Category = category;

            if (category == "Podcasts")
            {
                var podcast = _context.Podcasts.FirstOrDefault(p => p.Id == id);
                if (podcast == null) return NotFound();
                return View("FilmForm", podcast);
            }
            else
            {
                var film = _context.Films.FirstOrDefault(f => f.Id == id);
                if (film == null) return NotFound();
                return View("FilmForm", film);
            }
        }

        [HttpPost]
        public IActionResult SaveMedia(IFormCollection form)
        {
            try
            {
                string category = form["Category"];

                if (category == "Podcasts")
                {
                    var id = string.IsNullOrEmpty(form["Id"]) ? 0 : int.Parse(form["Id"]);
                    var heading = form["Heading"]; // Use Heading for consistency with Film model and form
                    var image = form["Image"];
                    var language = form["Language"];
                    var year = int.TryParse(form["Year"], out int y) ? y : 0;
                    var description = form["Description"];
                    var host = form["Host"];
                    var guest = form["GuestsString"]; // Use GuestsString for consistency with form
                    var duration = form["Duration"];
                    var link = form["Link"];
                    var genre = form["GenreString"];

                    Podcast podcast;

                    if (id == 0)
                    {
                        podcast = new Podcast();
                        _context.Podcasts.Add(podcast);
                    }
                    else
                    {
                        podcast = _context.Podcasts.FirstOrDefault(p => p.Id == id);
                        if (podcast == null)
                        {
                            return Json(new { success = false, message = "Podcast not found." });
                        }
                    }

                    podcast.Heading = heading; // Use heading
                    podcast.Image = image;
                    podcast.Language = language;
                    podcast.Year = year;
                    podcast.Description = description;
                    podcast.Host = host;
                    podcast.GuestsString = guest;
                    podcast.Duration = duration;
                    podcast.Link = link;
                    podcast.GenreString = genre;
                }
                else // Film (ShortFilm, Movie, Exclusive)
                {
                    var id = string.IsNullOrEmpty(form["Id"]) ? 0 : int.Parse(form["Id"]);
                    var heading = form["Heading"];
                    var image = form["Image"];
                    var language = form["Language"];
                    var year = int.TryParse(form["Year"], out int y) ? y : 0;
                    var description = form["Description"];
                    var link = form["Link"];
                    var genre = form["Genre"];
                    var writer = form["Writer"];
                    var director = form["Director"];
                    var stars = form["StarsString"];

                    Film film;

                    if (id == 0)
                    {
                        film = new Film();
                        _context.Films.Add(film);
                    }
                    else
                    {
                        film = _context.Films.FirstOrDefault(f => f.Id == id);
                        if (film == null)
                        {
                            return Json(new { success = false, message = "Film not found." });
                        }
                    }

                    film.Heading = heading;
                    film.Image = image;
                    film.Language = language;
                    film.Year = year;
                    film.Description = description;
                    film.Link = link;
                    film.GenreString = genre; // Ensure this matches your Film model's property name
                    film.Writer = writer;
                    film.Director = director;
                    film.StarsString = stars;
                    film.Category = category; // Ensure Category is set for films
                }

                _context.SaveChanges(); // Save changes outside the if/else to catch all DB updates

                // Return a JSON response with success: true
                return Json(new { success = true, message = "Item saved successfully!" });
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
        public IActionResult DeleteMedia(int id, string category)
        {
            try
            {
                if (category == "Podcasts")
                {
                    var podcast = _context.Podcasts.FirstOrDefault(p => p.Id == id);
                    if (podcast != null)
                    {
                        _context.Podcasts.Remove(podcast);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    var film = _context.Films.FirstOrDefault(f => f.Id == id);
                    if (film != null)
                    {
                        _context.Films.Remove(film);
                        _context.SaveChanges();
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
