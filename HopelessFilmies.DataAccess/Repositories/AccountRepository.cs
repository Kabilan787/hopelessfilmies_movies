using HopelessFilmies.Domain.Interfaces.IAccount;
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
    public class AccountRepository : IAccountRepository
    {
        private readonly UserDbContext _userDbContext;

        public AccountRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<User> CheckUser(string email, string password)
        {
            var user = _userDbContext.Users.FirstOrDefault(u =>(u.Email == email) && u.Password == password);

            return user;
        }

        public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await _userDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task AddUser(User user)
        {
            _userDbContext.Users.Add(user);
        }

        public async Task SaveChangesAsync()
        {
            await _userDbContext.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<Film>> GetByTitlesAsync(List<string> titles)
        {
            return await _userDbContext.Films
                .Where(f => titles.Contains(f.Heading))
                .ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _userDbContext.Users.Update(user);  // Optional; EF tracks it already
            await _userDbContext.SaveChangesAsync();
        }

    }
}
