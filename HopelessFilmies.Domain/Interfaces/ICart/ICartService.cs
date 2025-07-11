using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Interfaces.ICart
{
    public interface ICartService
    {
        Task<List<Film>> GetCartAsync(List<int> ids);

        Task<User?> GetUserByEmailAsync(string email);

        Task SaveChangesAsync();
    }
}
