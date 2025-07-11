using HopelessFilmies.Domain.Interfaces.IAdmin;
using HopelessFilmies.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Service.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Admin?> CheckAdminAsync(string email, string password)
        {
            return await _adminRepository.CheckAdminAsync(email, password);
        }

        public async Task<Podcast> GetPodcastsAsync(int id)
        {
            return await _adminRepository.GetPodcastsAsync(id);
        }

        public async Task<Film> GetFilmsAsync(int id)
        {
            return await _adminRepository.GetFilmsAsync(id);
        }

        public async Task<List<Podcast>> GetPodcastsAsync()
        {
            return await _adminRepository.GetPodcastsAsync();
        }

        public async Task<List<Film>> GetFilmsAsync()
        {
            return await _adminRepository.GetFilmsAsync();
        }

        public Film PrepareNewFilm(string category)
        {
            return new Film { Category = category };
        }

        public Podcast PrepareNewPodcast()
        {
            return new Podcast(); // you can add defaults if needed
        }

        public async Task<(bool, string)> SaveFilmAsync(IFormCollection form, string imagePath)
        {
            var id = string.IsNullOrEmpty(form["Id"]) ? 0 : int.Parse(form["Id"]);
            var film = id == 0 ? new Film() : await _adminRepository.GetFilmsAsync(id);

            if (id != 0 && film == null)
                return (false, "Film not found.");

            film.Heading = form["Heading"];
            film.Language = form["Language"];
            film.Year = int.TryParse(form["Year"], out var y) ? y : 0;
            film.Description = form["Description"];
            film.Link = form["Link"];
            film.GenreString = form["Genre"];
            film.Writer = form["Writer"];
            film.Director = form["Director"];
            film.StarsString = form["StarsString"];
            film.Category = form["Category"];

            if (!string.IsNullOrEmpty(imagePath))
                film.Image = imagePath;

            if (id == 0)
                await _adminRepository.AddFilmAsync(film);

            await _adminRepository.SaveChangesAsync();
            return (true, "Film saved successfully.");
        }

        public async Task<(bool, string)> SavePodcastAsync(IFormCollection form, string imagePath)
        {
            var id = string.IsNullOrEmpty(form["Id"]) ? 0 : int.Parse(form["Id"]);
            var podcast = id == 0 ? new Podcast() : await _adminRepository.GetPodcastsAsync(id);

            if (id != 0 && podcast == null)
                return (false, "Podcast not found.");

            podcast.Heading = form["Heading"];
            podcast.Language = form["Language"];
            podcast.Year = int.TryParse(form["Year"], out var y) ? y : 0;
            podcast.Description = form["Description"];
            podcast.Host = form["Host"];
            podcast.GuestsString = form["GuestsString"];
            podcast.Duration = form["Duration"];
            podcast.Link = form["Link"];
            podcast.GenreString = form["GenreString"];

            if (!string.IsNullOrEmpty(imagePath))
                podcast.Image = imagePath;

            if (id == 0)
                await _adminRepository.AddPodcastAsync(podcast);

            await _adminRepository.SaveChangesAsync();
            return (true, "Podcast saved successfully.");
        }

        public async Task SaveChangesAsync()
        {
            await _adminRepository.SaveChangesAsync();
        }

        public async Task RemoveFilmAsync(Film film)
        {
            await _adminRepository.RemoveFilmAsync(film);
        }

        public async Task RemovePodcastAsync(Podcast podcast)
        {
           await  _adminRepository.RemovePodcastAsync(podcast);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _adminRepository.GetUsersAsync();
        }

        public async Task<List<ContactForm>> GetContactFormsAsync()
        {
            return await _adminRepository.GetContactFormsAsync();
        }

        public async Task<bool> DeleteContactFormAsync(int id)
        {
            return await _adminRepository.DeleteContactFormAsync(id);
        }
    }
}
