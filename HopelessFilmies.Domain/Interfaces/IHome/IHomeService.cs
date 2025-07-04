using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Interfaces.IHome
{
    public interface IHomeService
    {
        Task<List<Film>> GetFilmsByCategoryAsync(string category);
        Task<List<Podcast>> GetAllPodcastsAsync();
        Task<List<Film>> GetFilteredFilmsAsync(string category, string query);
        Task<List<Podcast>> GetFilteredPodcastsAsync(string query);

        Task<object> GetMediaDetailsAsync(int id, string type);
    }
}
