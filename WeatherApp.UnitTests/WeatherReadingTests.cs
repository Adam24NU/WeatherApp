using Xunit;
using WeatherApp.Models;

namespace WeatherApp.UnitTests
{
    public class WeatherReadingTests
    {
        [Fact]
        public void WeatherReading_ShouldSetPropertiesCorrectly()
        {
            var reading = new WeatherReading
            {
                Timestamp = "2025-04-28 10:00",
                Temperature = "22",
                WindSpeed = "15",
                RelativeHumidity = "60",
                WindDirection = "N"
            };

            Assert.Equal("2025-04-28 10:00", reading.Timestamp);
            Assert.Equal("22", reading.Temperature);
            Assert.Equal("15", reading.WindSpeed);
            Assert.Equal("60", reading.RelativeHumidity);
            Assert.Equal("N", reading.WindDirection);
        }
    }
}