namespace WeatherApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } // Use email for registration
        public string Password { get; set; }
        public string Role { get; set; }
    }
}