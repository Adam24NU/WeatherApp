using System;
using Xunit;
using WeatherApp;

namespace WeatherApp.Tests
{
    public class UserModelTests
    {
        [Fact]
        public void User_ShouldInitializeCorrectly()
        {
            // Arrange
            var user = new User
            {
                Username = "JohnDoe",
                Password = "Password123",
                Role = "Scientist",
            };

            // Assert
            Assert.Equal("JohnDoe", user.Username);
            Assert.Equal("Password123", user.Password);
            Assert.Equal("Scientist", user.Role);
        }

        
    }
}
