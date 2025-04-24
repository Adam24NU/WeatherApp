using Xunit;
using WeatherApp.Models;

namespace WeatherApp.Tests
{
    public class WeatherReadingTests
    {
        [Fact]
        public void WeatherReading_ShouldInitializeCorrectly()
        {
            var weatherReading = new WeatherReading
            {
                Timestamp = "2025-04-23T12:00:00Z",
                Temperature = "20.5",
                WindSpeed = "5.0",
                RelativeHumidity = "60",
                WindDirection = "North"
            };

            Assert.Equal("2025-04-23T12:00:00Z", weatherReading.Timestamp);
            Assert.Equal("20.5", weatherReading.Temperature);
            Assert.Equal("5.0", weatherReading.WindSpeed);
            Assert.Equal("60", weatherReading.RelativeHumidity);
            Assert.Equal("North", weatherReading.WindDirection);
        }
    }
}
