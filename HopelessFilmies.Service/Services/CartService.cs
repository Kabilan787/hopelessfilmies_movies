using HopelessFilmies.Domain.Interfaces.ICart;
using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Service.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Film>> GetCartAsync(List<int> ids)
        {
            return await _repository.GetCartAsync(ids);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _repository.GetUserByEmailAsync(email);
        }

        public async Task SaveChangesAsync()
        {
            await _repository.SaveChangesAsync();
        }
    }
}
