namespace HopelessFilmiesMVC.Models
{
    public class MediaDetailsViewModel
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int Year { get; set; }
        public List<string> Genre { get; set; }
        public string Category { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }

        // Additional Display Logic
        public string ButtonLabel { get; set; }
        public bool IsUserSignedIn { get; set; }
        public bool IsExclusive { get; set; }
        public bool IsPurchased { get; set; }

        // Optional: Film-specific
        public string Writer { get; set; }
        public string Director { get; set; }
        public List<string> Stars { get; set; }

        // Optional: Podcast-specific
        public string Host { get; set; }
        public List<string> Guests { get; set; }

        public string MediaType { get; set; } // "movie", "podcast", etc.
    }
}
