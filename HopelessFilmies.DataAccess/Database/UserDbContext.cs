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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Seed data for Podcasts
        //    modelBuilder.Entity<Podcast>().HasData(
        //        new Podcast
        //        {
        //            Id = 1,
        //            Heading = "Voices of the Valley",
        //            Description = "Exploring untold stories from Tamil Nadu’s villages, hosted by Arvind Kumar.",
        //            Host = "Arvind Kumar",
        //            GuestsString = "Meena Ramesh,Sathya Narayan",
        //            Language = "Tamil",
        //            Year = 2024,
        //            Duration = "28 min",
        //            Link = "https://example.com/podcasts/voices-valley",
        //            Image = "https://example.com/images/podcasts/voices-valley.jpg",
        //            GenreString = "Culture,Storytelling,Rural"
        //        },
        //        new Podcast
        //        {
        //            Id = 2,
        //            Heading = "CinemaScope",
        //            Description = "Deep-dive into Tamil cinema and interviews with creators.",
        //            Host = "Kavitha Suresh",
        //            GuestsString = "Ravi,Anandhi",
        //            Language = "Tamil",
        //            Year = 2023,
        //            Duration = "35 min",
        //            Link = "https://example.com/podcasts/cinemascope",
        //            Image = "https://example.com/images/podcasts/cinemascope.jpg",
        //            GenreString = "Film,Interview,Entertainment"
        //        }
        //    );
        //}
    }
}
