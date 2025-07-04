using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Interfaces.IAccount
{
    public interface IAccountRepository
    {
        Task<User> CheckUser(string email, string password);

        Task<User?> GetByEmailAndPasswordAsync(string email, string password);

        Task AddUser(User user);

        Task SaveChangesAsync();

        Task<User?> GetByEmailAsync(string email);

        Task<List<Film>> GetByTitlesAsync(List<string> titles);

        Task UpdateUserAsync(User user);
    }
}
