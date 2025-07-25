﻿using HopelessFilmies.Domain.Interfaces.IAdmin;
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
    public class AdminRepository : IAdminRepository
    {
        private readonly UserDbContext _userDbContext;

        public AdminRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<Admin> CheckAdminAsync(string email, string password)
        {
            var admin = _userDbContext.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);

            return admin;
        }

        public async Task<Podcast> GetPodcastsAsync(int id)
        {
            return await _userDbContext.Podcasts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Film> GetFilmsAsync(int id)
        {
            return _userDbContext.Films.FirstOrDefault(f => f.Id == id); ;
        }


        public async Task<List<Podcast>> GetPodcastsAsync()
        {
            return await _userDbContext.Podcasts.ToListAsync();
        }

        public async Task<List<Film>> GetFilmsAsync()
        {
            return await _userDbContext.Films.ToListAsync();
        }

        public async Task AddFilmAsync(Film film)
        {
            await _userDbContext.Films.AddAsync(film);
        }

        public async Task AddPodcastAsync(Podcast podcast)
        {
            await _userDbContext.Podcasts.AddAsync(podcast);
        }

        public async Task SaveChangesAsync()
        {
            await _userDbContext.SaveChangesAsync();
        }

        public async Task RemoveFilmAsync(Film film)
        {
            _userDbContext.Films.Remove(film);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task RemovePodcastAsync(Podcast podcast)
        {
            _userDbContext.Podcasts.Remove(podcast);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _userDbContext.Users.ToListAsync();
        }

        public async Task<List<ContactForm>> GetContactFormsAsync()
        {
            return await _userDbContext.ContactForms.ToListAsync();
        }

        public async Task<bool> DeleteContactFormAsync(int id)
        {
            var contact = await _userDbContext.ContactForms.FindAsync(id);

            if (contact == null)
                return false;

            _userDbContext.ContactForms.Remove(contact);

            var rowsAffected = await _userDbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
