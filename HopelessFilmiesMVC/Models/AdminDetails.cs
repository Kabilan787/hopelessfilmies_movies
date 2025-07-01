namespace HopelessFilmiesMVC.Models
{
    public class AdminDetails
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // You can later hash this for security
    }
}
