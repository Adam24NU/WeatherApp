using Xunit;
using WeatherApp.Models;

namespace WeatherApp.Tests
{
    public class AirReadingTests
    {
        [Fact]
        public void AirReading_ShouldInitializeCorrectly()
        {
            var airReading = new AirReading
            {
                Timestamp = "2025-04-23T12:00:00Z",
                NO2 = "50",
                PM25 = "30",
                PM10 = "40",
                Su = "1"
            };

            Assert.Equal("2025-04-23T12:00:00Z", airReading.Timestamp);
            Assert.Equal("50", airReading.NO2);
            Assert.Equal("30", airReading.PM25);
            Assert.Equal("40", airReading.PM10);
            Assert.Equal("1", airReading.Su);
        }

        [Fact]
        public void AirReading_ShouldDetectThresholdBreached()
        {
            var airReading = new AirReading
            {
                NO2 = "50",
                PM25 = "30"
            };

            Assert.True(airReading.IsThresholdBreached);  // This should return true because NO2 > 40 or PM25 > 25
        }

        [Fact]
        public void AirReading_ShouldNotDetectThresholdBreached()
        {
            var airReading = new AirReading
            {
                NO2 = "20",
                PM25 = "20"
            };

            Assert.False(airReading.IsThresholdBreached);  // This should return false because NO2 <= 40 and PM25 <= 25
        }
    }
}
