using HopelessFilmiesMVC.Data;

namespace HopelessFilmiesMVC.Models
{
    public class HomeViewModel
    {
        public List<Film> ShortFilms { get; set; }
        public List<Film> Movies { get; set; }

        public List<Podcast> Podcasts { get; set; }

        public List<Film> Exclusive { get; set; }
    }
}
