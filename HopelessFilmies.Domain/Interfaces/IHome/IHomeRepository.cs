using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Interfaces.IHome
{
    public interface IHomeRepository
    {
        Task<List<Film>> GetFilmsByCategoryAsync(string category);
        Task<List<Podcast>> GetAllPodcastsAsync();

        Task<Film> GetFilmByIdAndCategoryAsync(int id, string category);
        Task<Podcast> GetPodcastByIdAsync(int id);

        Task<User> GetUserByEmailAsync(string email);
    }
}
