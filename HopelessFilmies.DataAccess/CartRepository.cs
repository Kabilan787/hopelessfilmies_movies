using HopelessFilmies.Domain.Interfaces.ICart;
using HopelessFilmies.Domain.Models;
using HopelessFilmiesMVC.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.DataAccess
{
    public class CartRepository : ICartRepository
    {
        private readonly UserDbContext _userDbContext;

        public CartRepository(UserDbContext userDbContext) { 
            _userDbContext = userDbContext;
        }

        public async Task<List<Film>> GetCartAsync(List<int> ids)
        {
           var cartItems = await _userDbContext.Films.Where(f => ids.Contains(f.Id) && f.Category == "Exclusive").ToListAsync();
            return cartItems;
        }
    }
}
