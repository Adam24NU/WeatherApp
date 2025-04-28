using Xunit;
using WeatherApp;

namespace WeatherApp.UnitTests
{
    public class AirReadingTests
    {
        [Fact]
        public void IsThresholdBreached_ShouldBeTrue_WhenNO2AboveThreshold()
        {
            var reading = new AirReading { NO2 = "50", PM25 = "10" };
            Assert.True(reading.IsThresholdBreached);
        }

        [Fact]
        public void IsThresholdBreached_ShouldBeTrue_WhenPM25AboveThreshold()
        {
            var reading = new AirReading { NO2 = "30", PM25 = "30" };
            Assert.True(reading.IsThresholdBreached);
        }

        [Fact]
        public void IsThresholdBreached_ShouldBeFalse_WhenBothBelowThreshold()
        {
            var reading = new AirReading { NO2 = "20", PM25 = "20" };
            Assert.False(reading.IsThresholdBreached);
        }
    }
}