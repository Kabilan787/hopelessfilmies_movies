using HopelessFilmies.Domain.Interfaces.IHome;
using HopelessFilmies.Domain.Models;
using HopelessFilmiesMVC.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.DataAccess.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly UserDbContext _userDbContext;

        public HomeRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<List<Podcast>> GetAllPodcastsAsync()
        {
            return await _userDbContext.Podcasts.ToListAsync();
        }

        public async Task<List<Film>> GetFilmsByCategoryAsync(string category)
        {
            return await _userDbContext.Films
            .Where(f => f.Category.ToLower() == category.ToLower())
            .ToListAsync();
        }

        public async Task<Film> GetFilmByIdAndCategoryAsync(int id, string category)
        {
            return await _userDbContext.Films
                .FirstOrDefaultAsync(f => f.Id == id && f.Category.ToLower() == category.ToLower());
        }

        public async Task<Podcast> GetPodcastByIdAsync(int id)
        {
            return await _userDbContext.Podcasts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userDbContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
