using Xunit;
using WeatherApp.Models;

namespace WeatherApp.UnitTests
{
    public class WaterReadingTests
    {
        [Fact]
        public void WaterReading_ShouldSetPropertiesCorrectly()
        {
            var reading = new WaterReading
            {
                Date = "2025-04-28",
                Time = "10:00",
                Nitrate = "5",
                Nitrite = "3",
                Phosphate = "2",
                EC = "100"
            };

            Assert.Equal("2025-04-28", reading.Date);
            Assert.Equal("10:00", reading.Time);
            Assert.Equal("5", reading.Nitrate);
            Assert.Equal("3", reading.Nitrite);
            Assert.Equal("2", reading.Phosphate);
            Assert.Equal("100", reading.EC);
        }
    }
}