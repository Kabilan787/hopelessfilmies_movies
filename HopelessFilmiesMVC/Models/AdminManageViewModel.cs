using HopelessFilmiesMVC.Data;

namespace HopelessFilmiesMVC.Models
{
    public class AdminManageViewModel
    {
        public string Category { get; set; } // "ShortFilm", "Movies", "Exclusive", "Podcasts"

        public List<Film> Films { get; set; }
        public List<Podcast> Podcasts { get; set; }
    }
}