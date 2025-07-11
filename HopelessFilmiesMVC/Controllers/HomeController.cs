using HopelessFilmies.Domain.Interfaces.IHome;
using HopelessFilmiesMVC.Models;
using HopelessFilmies.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HopelessFilmiesMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }



        public async Task<IActionResult> IndexAsync()
        {
            var shortfilms = await _homeService.GetFilmsByCategoryAsync("Shortfilm");
            var movies = await _homeService.GetFilmsByCategoryAsync("Movie");
            var exclusive = await _homeService.GetFilmsByCategoryAsync("Exclusive");
            var podcasts = await _homeService.GetAllPodcastsAsync();

            var model = new HomeViewModel
            {
                ShortFilms = shortfilms,
                Movies = movies,
                Podcasts = podcasts,
                Exclusive = exclusive
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> AllMedia(string type, string query)
        {
            if (string.IsNullOrEmpty(type))
                return NotFound();

            ViewData["MediaType"] = type.ToLower();
            ViewData["SearchQuery"] = query;

            if (type.Equals("shortfilms", StringComparison.OrdinalIgnoreCase) || type.Equals("movies", StringComparison.OrdinalIgnoreCase))
            {
                var category = type.Equals("shortfilms", StringComparison.OrdinalIgnoreCase) ? "Shortfilm" : "Movie";
                var films = await _homeService.GetFilteredFilmsAsync(category, query);
                return View(films);
            }
            else if (type.Equals("podcasts", StringComparison.OrdinalIgnoreCase))
            {
                var podcasts = await _homeService.GetFilteredPodcastsAsync(query);
                return View(podcasts);
            }
            else if (type.Equals("exclusive", StringComparison.OrdinalIgnoreCase))
            {
                var films = await _homeService.GetFilteredFilmsAsync("Exclusive", query);
                return View(films);
            }

            return NotFound();
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id, string type)
        {
            var media = await _homeService.GetMediaDetailsAsync(id, type);
            if (media == null) return NotFound();

            bool isUserSignedIn = User.Identity.IsAuthenticated;
            string userEmail = isUserSignedIn
                ? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                : null;

            string heading = null;
            string category = null;
            string link = null;

            string buttonLabel = (media is Podcast) ? "Listen Now" : "Watch Now";

            if (media is Film film)
            {
                heading = film.Heading;
                category = film.Category;
                link = film.Link;
            }
            else if (media is Podcast podcast)
            {
                heading = podcast.Heading;
                
                link = podcast.Link;
            }

            bool isExclusive = category?.Equals("Exclusive", StringComparison.OrdinalIgnoreCase) == true;
            bool isPurchased = false;

            if (isExclusive && isUserSignedIn && !string.IsNullOrEmpty(userEmail))
            {
                var user = await _homeService.GetUserByEmailAsync(userEmail);
                if (user != null && !string.IsNullOrEmpty(user.PurchasedMovies))
                {
                    isPurchased = user.PurchasedMovies
                        .Split(',')
                        .Any(title => title.Trim().Equals(heading.Trim(), StringComparison.OrdinalIgnoreCase));
                }
            }

            // Pass required values to view via ViewBag
            ViewBag.IsUserSignedIn = isUserSignedIn;
            ViewBag.IsExclusive = isExclusive;
            ViewBag.IsPurchased = isPurchased;
            ViewBag.ButtonLabel = buttonLabel;
            ViewBag.Link = link;

            return View("Details", media);
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
