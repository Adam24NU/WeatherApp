using Xunit;
using WeatherApp.Models;

namespace WeatherApp.UnitTests
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldSetAllPropertiesCorrectly()
        {
            var user = new User
            {
                UserId = 1,
                Email = "adam@example.com",
                Password = "securePassword123",
                Role = "Scientist"
            };

            Assert.Equal(1, user.UserId);
            Assert.Equal("adam@example.com", user.Email);
            Assert.Equal("securePassword123", user.Password);
            Assert.Equal("Scientist", user.Role);
        }

        [Fact]
        public void User_ShouldAllowEmptyRole()
        {
            var user = new User
            {
                UserId = 2,
                Email = "student@example.com",
                Password = "password456",
                Role = ""
            };

            Assert.Equal(2, user.UserId);
            Assert.Equal("student@example.com", user.Email);
            Assert.Equal("password456", user.Password);
            Assert.Empty(user.Role);
        }
    }
}