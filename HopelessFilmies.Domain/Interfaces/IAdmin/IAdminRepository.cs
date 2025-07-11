using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Interfaces.IAdmin
{
    public interface IAdminRepository 
    {
        Task<Admin?> CheckAdminAsync(string email, string password);

        Task<Podcast> GetPodcastsAsync(int id);

        Task<Film> GetFilmsAsync(int id);

        Task<List<Podcast>> GetPodcastsAsync();

        Task<List<Film>> GetFilmsAsync();

        Task AddFilmAsync(Film film);
        Task AddPodcastAsync(Podcast podcast);

        Task SaveChangesAsync();

        Task RemoveFilmAsync(Film film);

        Task RemovePodcastAsync(Podcast podcast);

        Task<List<User>> GetUsersAsync();

        Task<List<ContactForm>> GetContactFormsAsync();

        Task<bool> DeleteContactFormAsync(int id);

    }
}
