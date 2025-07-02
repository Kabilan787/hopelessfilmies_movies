using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Models
{
    public class Podcast
    {
        [Key]
        public int Id { get; set; }

        public string Heading { get; set; }

        public string Description { get; set; }

        public string Host { get; set; }

        // Store guest names as comma-separated string
        public string GuestsString { get; set; }

        [NotMapped]
        public List<string> Guests
        {
            get => string.IsNullOrEmpty(GuestsString) ? new List<string>() : GuestsString.Split(',').Select(g => g.Trim()).ToList();
            set => GuestsString = string.Join(", ", value);
        }

        public string Language { get; set; }

        public int Year { get; set; }

        public string Duration { get; set; }

        public string Link { get; set; }

        public string Image { get; set; }

        // Store tags (or genres) as comma-separated string
        public string GenreString { get; set; }

        [NotMapped]
        public List<string> Genre
        {
            get => string.IsNullOrEmpty(GenreString) ? new List<string>() : GenreString.Split(',').Select(t => t.Trim()).ToList();
            set => GenreString = string.Join(", ", value);
        }
    }
}
