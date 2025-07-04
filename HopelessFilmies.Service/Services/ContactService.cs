    using HopelessFilmies.Domain.Interfaces.IContact;
using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Service.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task AddContactAsync(string name, string email, string message)
        {
            var contact = new ContactForm
            {
                Name = name,
                Email = email,
                Message = message
            };

            await _contactRepository.AddContactAsync(contact);
        }
    }
}
