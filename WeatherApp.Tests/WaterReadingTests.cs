using Xunit;
using WeatherApp.Models;

namespace WeatherApp.Tests
{
    public class WaterReadingTests
    {
        [Fact]
        public void WaterReading_ShouldInitializeCorrectly()
        {
            var waterReading = new WaterReading
            {
                Date = "2025-04-23",
                Time = "10:00 AM",
                Nitrate = "30",
                Nitrite = "10",
                Phosphate = "0.5",
                EC = "1000"
            };

            Assert.Equal("2025-04-23", waterReading.Date);
            Assert.Equal("10:00 AM", waterReading.Time);
            Assert.Equal("30", waterReading.Nitrate);
            Assert.Equal("10", waterReading.Nitrite);
            Assert.Equal("0.5", waterReading.Phosphate);
            Assert.Equal("1000", waterReading.EC);
        }

        [Fact]
        public void WaterReading_ShouldDetectThresholdBreached()
        {
            var waterReading = new WaterReading
            {
                Nitrate = "60", // Exceeds threshold (50)
                Nitrite = "10",
                Phosphate = "0.5",
                EC = "1000"
            };

            Assert.True(waterReading.IsThresholdBreached); // It should breach the threshold because Nitrate > 50
        }

        [Fact]
        public void WaterReading_ShouldNotDetectThresholdBreached()
        {
            var waterReading = new WaterReading
            {
                Nitrate = "30",  // Below threshold (50)
                Nitrite = "10",
                Phosphate = "0.5",
                EC = "1000"
            };

            Assert.False(waterReading.IsThresholdBreached); // It shouldn't breach the threshold
        }
    }
}
