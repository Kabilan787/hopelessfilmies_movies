using System.ComponentModel.DataAnnotations;

namespace HopelessFilmiesMVC.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? PurchasedMovies { get; set; }
    }
}
