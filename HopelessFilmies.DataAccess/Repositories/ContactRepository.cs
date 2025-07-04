using HopelessFilmies.Domain.Interfaces.IContact;
using HopelessFilmies.Domain.Models;
using HopelessFilmiesMVC.DataAccess.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.DataAccess.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly UserDbContext _userDbContext;

        public ContactRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public async Task AddContactAsync(ContactForm contact)
        {
            _userDbContext.ContactForms.Add(contact);

            await _userDbContext.SaveChangesAsync();
        }
    }
}
