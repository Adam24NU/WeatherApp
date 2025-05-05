namespace WeatherApp.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; } // Use email for registration

        public string Password { get; set; }

        public string Role { get; set; }

        /// <summary>
        /// Example test method for Doxygen.
        /// </summary>
        public void TestMethod() 
        {
            // Just a dummy method
        }
    }
}