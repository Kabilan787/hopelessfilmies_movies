using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string Language { get; set; }
        public int Year { get; set; }
        public string GenreString { get; set; }  // Store as string in DB
        [NotMapped]
        public List<string> Genre
        {
            get => string.IsNullOrEmpty(GenreString)
                ? new List<string>()
                : GenreString.Split(',').Select(g => g.Trim()).ToList();
            set => GenreString = string.Join(", ", value);
        }
        public string Writer { get; set; }
        public string Director { get; set; }
        public string StarsString { get; set; }  // Store as string in DB
        [NotMapped]
        public List<string> Stars
        {
            get => string.IsNullOrEmpty(StarsString)
                ? new List<string>()
                : StarsString.Split(',').Select(s => s.Trim()).ToList();
            set => StarsString = string.Join(", ", value);
        }
        public string Category { get; set; }  // shortfilms or movies
    }
}
