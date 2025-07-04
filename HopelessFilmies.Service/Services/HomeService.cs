using HopelessFilmies.Domain.Interfaces.IHome;
using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Service.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }
        public Task<List<Film>> GetFilmsByCategoryAsync(string category)
        {
            return _homeRepository.GetFilmsByCategoryAsync(category);
        }

        public Task<List<Podcast>> GetAllPodcastsAsync()
        {
            return _homeRepository.GetAllPodcastsAsync();
        }

        public async Task<List<Film>> GetFilteredFilmsAsync(string category, string query)
        {
            var films = await _homeRepository.GetFilmsByCategoryAsync(category);

            if (string.IsNullOrWhiteSpace(query)) return films;

            string q = query.ToLower();

            return films.AsParallel().Where(f =>
                (!string.IsNullOrEmpty(f.Heading) && f.Heading.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(f.Description) && f.Description.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(f.Language) && f.Language.ToLower().Contains(q)) ||
                f.Year.ToString().Contains(q) ||
                f.Genre.Any(g => g.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(f.Writer) && f.Writer.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(f.Director) && f.Director.ToLower().Contains(q)) ||
                f.Stars.Any(s => s.ToLower().Contains(q))
            ).ToList();
        }

        public async Task<List<Podcast>> GetFilteredPodcastsAsync(string query)
        {
            var podcasts = await _homeRepository.GetAllPodcastsAsync();

            if (string.IsNullOrWhiteSpace(query)) return podcasts;

            string q = query.ToLower();

            return podcasts.AsParallel().Where(p =>
                (!string.IsNullOrEmpty(p.Heading) && p.Heading.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(p.Host) && p.Host.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(p.Language) && p.Language.ToLower().Contains(q)) ||
                p.Year.ToString().Contains(q) ||
                (!string.IsNullOrEmpty(p.Duration) && p.Duration.ToLower().Contains(q)) ||
                p.Guests.Any(g => g.ToLower().Contains(q)) ||
                p.Genre.Any(t => t.ToLower().Contains(q))
            ).ToList();
        }

        public async Task<object> GetMediaDetailsAsync(int id, string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return null;

            switch (type.ToLower())
            {
                case "shortfilms":
                    return await _homeRepository.GetFilmByIdAndCategoryAsync(id, "Shortfilm");

                case "movies":
                    return await _homeRepository.GetFilmByIdAndCategoryAsync(id, "Movie");

                case "podcasts":
                    return await _homeRepository.GetPodcastByIdAsync(id);

                default:
                    return null;
            }
        }
    }
}
