using Microsoft.EntityFrameworkCore;
using HopelessFilmies.Domain.Models;

namespace HopelessFilmiesMVC.DataAccess.Database
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; } // Maps to Contact_Form table

        // Add Films
        public DbSet<Film> Films { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }

        public DbSet<Admin> Admins { get; set; }

        
    }
}
