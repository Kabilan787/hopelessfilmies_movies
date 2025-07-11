using HopelessFilmies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Interfaces.IContact
{
    public interface IContactRepository
    {
        Task AddContactAsync(ContactForm contact);
    }
}
