using HopelessFilmies.Domain.Interfaces.IAccount;
using HopelessFilmies.Domain.Interfaces.IAdmin;
using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace HopelessFilmies.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<User> CheckUser(string email, string password)
        {
            return _accountRepository.CheckUser(email, password);
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            return await _accountRepository.GetByEmailAndPasswordAsync(email, password);
        }

        public async Task AddUser(string fullName,string email, string password)
        {
            var user = new User
            {
                FullName = fullName,
                Email = email,
                Password = password
            };

            await _accountRepository.AddUser(user);
        }

        public async Task SaveChangesAsync()
        {
            await _accountRepository.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
        }

        public async Task<List<Film>> GetPurchasedMoviesAsync(string userEmail)
        {
            var user = await _accountRepository.GetByEmailAsync(userEmail);
            if (user == null || string.IsNullOrEmpty(user.PurchasedMovies))
                return new List<Film>();

            var titles = user.PurchasedMovies
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList();

            return await _accountRepository.GetByTitlesAsync(titles);
        }

        public async Task<bool> RemovePurchasedMovieAsync(string email, string title)
        {
            var user = await _accountRepository.GetByEmailAsync(email);
            if (user == null || string.IsNullOrWhiteSpace(user.PurchasedMovies))
                return false;

            var movies = user.PurchasedMovies
                             .Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(m => m.Trim())
                             .ToList();

            if (!movies.Contains(title))
                return false;

            movies.Remove(title);
            user.PurchasedMovies = string.Join(", ", movies);

            await _accountRepository.UpdateUserAsync(user);
            return true;
        }
    }
}
