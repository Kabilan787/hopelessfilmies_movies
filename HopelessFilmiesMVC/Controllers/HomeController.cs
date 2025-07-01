using HopelessFilmiesMVC.Data;
using HopelessFilmiesMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HopelessFilmiesMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
        }



        public IActionResult Index()
        {
            var shortfilms = _context.Films.Where(f => f.Category == "Shortfilm").ToList();
            var movies = _context.Films.Where(f => f.Category == "Movie").ToList();
            var podcasts = _context.Podcasts.ToList();
            var exclusive = _context.Films.Where(f => f.Category == "Exclusive").ToList();

            var model = new HomeViewModel
            {
                ShortFilms = shortfilms,
                Movies = movies,
                Podcasts = podcasts,
                Exclusive = exclusive
            };

            return View(model);
        }


        public IActionResult AllMedia(string type, string query)
        {
            if (string.IsNullOrEmpty(type))
                return NotFound();

            ViewData["MediaType"] = type.ToLower();
            ViewData["SearchQuery"] = query;

            if (type.Equals("shortfilms", StringComparison.OrdinalIgnoreCase) || type.Equals("movies", StringComparison.OrdinalIgnoreCase))
            {
                var category = type.Equals("shortfilms", StringComparison.OrdinalIgnoreCase) ? "Shortfilm" : "Movie";
                var items = _context.Films
                    .Where(f => f.Category == category)
                    .AsEnumerable();

                if (!string.IsNullOrEmpty(query))
                {
                    string q = query.ToLower();
                    items = items.AsParallel().Where(f =>
                        (!string.IsNullOrEmpty(f.Heading) && f.Heading.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(f.Description) && f.Description.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(f.Language) && f.Language.ToLower().Contains(q)) ||
                        f.Year.ToString().Contains(q) ||
                        f.Genre.Any(g => g.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(f.Writer) && f.Writer.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(f.Director) && f.Director.ToLower().Contains(q)) ||
                        f.Stars.Any(s => s.ToLower().Contains(q))
                    );
                }

                return View(items.ToList());
            }
            else if (type.Equals("podcasts", StringComparison.OrdinalIgnoreCase))
            {
                var items = _context.Podcasts.AsEnumerable();

                if (!string.IsNullOrEmpty(query))
                {
                    string q = query.ToLower();
                    items = items.AsParallel().Where(p =>
                        (!string.IsNullOrEmpty(p.Heading) && p.Heading.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(p.Host) && p.Host.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(p.Language) && p.Language.ToLower().Contains(q)) ||
                        p.Year.ToString().Contains(q) ||
                        (!string.IsNullOrEmpty(p.Duration) && p.Duration.ToLower().Contains(q)) ||
                        p.Guests.Any(g => g.ToLower().Contains(q)) ||
                        p.Genre.Any(t => t.ToLower().Contains(q))
                    );
                }

                return View(items.ToList());
            }
            else if (type.Equals("exclusive", StringComparison.OrdinalIgnoreCase))
            {
                var items = _context.Films
                    .Where(f => f.Category == "Exclusive")
                    .AsEnumerable();

                if (!string.IsNullOrEmpty(query))
                {
                    string q = query.ToLower();
                    items = items.AsParallel().Where(f =>
                        (!string.IsNullOrEmpty(f.Heading) && f.Heading.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(f.Description) && f.Description.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(f.Language) && f.Language.ToLower().Contains(q)) ||
                        f.Year.ToString().Contains(q) ||
                        f.Genre.Any(g => g.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(f.Writer) && f.Writer.ToLower().Contains(q)) ||
                        (!string.IsNullOrEmpty(f.Director) && f.Director.ToLower().Contains(q)) ||
                        f.Stars.Any(s => s.ToLower().Contains(q))
                    );
                }

                return View(items.ToList());
            }


            return NotFound();
        }



        public IActionResult Details(int id, string type)
        {
            object selectedMedia = null;

            switch (type?.ToLower())
            {
                case "shortfilms":  // to support both
                    selectedMedia = _context.Films.FirstOrDefault(f => f.Id == id && f.Category.ToLower() == "shortfilm");
                    break;
             
                case "movies":
                    selectedMedia = _context.Films.FirstOrDefault(m => m.Id == id && m.Category.ToLower() == "movie");
                    break;

                case "podcasts":
                    selectedMedia = _context.Podcasts.FirstOrDefault(p => p.Id == id);
                    break;
            }

            if (selectedMedia == null)
            {
                return NotFound();
            }

            ViewData["MediaType"] = type?.ToLower();
            return View("Details", selectedMedia);
        }
    

        public IActionResult Privacy()
        {
            return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
