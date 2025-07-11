using HopelessFilmies.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Interfaces.IAdmin
{
    public interface IAdminService
    {
        Task<Admin?> CheckAdminAsync(string email, string password);

        Task<Podcast> GetPodcastsAsync(int id);

        Task<Film> GetFilmsAsync(int id);

        Task<List<Podcast>> GetPodcastsAsync();

        Task<List<Film>> GetFilmsAsync();

        Film PrepareNewFilm(string category);

        Podcast PrepareNewPodcast();

        Task<(bool, string)> SaveFilmAsync(IFormCollection form, string imagePath);

        Task<(bool, string)> SavePodcastAsync(IFormCollection form, string imagePath);

        Task SaveChangesAsync();

        Task RemoveFilmAsync(Film film);

        Task RemovePodcastAsync(Podcast podcast);

        Task<List<User>> GetUsersAsync();

        Task<List<ContactForm>> GetContactFormsAsync();

        Task<bool> DeleteContactFormAsync(int id);
    }
}
