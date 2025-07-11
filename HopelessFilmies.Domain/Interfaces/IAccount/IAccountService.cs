using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Interfaces.IAccount
{
    public interface IAccountService
    {
        Task<User> CheckUser(string email, string password);

        Task<User?> ValidateUserAsync(string email, string password);

        Task AddUser(string fullName, string email, string password);

        Task SaveChangesAsync();

        Task<User?> GetByEmailAsync(string email);

        Task<List<Film>> GetPurchasedMoviesAsync(string userEmail);

        Task<bool> RemovePurchasedMovieAsync(string email, string title);
    }
}
