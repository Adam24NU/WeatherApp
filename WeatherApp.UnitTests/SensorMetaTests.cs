using Xunit;
using WeatherApp;

namespace WeatherApp.UnitTests
{
    public class SensorMetaTests
    {
        [Fact]
        public void SensorMeta_ShouldSetAllPropertiesCorrectly()
        {
            var sensor = new SensorMeta
            {
                SensorId = 101,
                SensorType = "Air Quality",
                MeasurementFrequency = "Hourly",
                SafeLevel = "Low",
                Status = "Active",
                IsFlagged = true
            };

            Assert.Equal(101, sensor.SensorId);
            Assert.Equal("Air Quality", sensor.SensorType);
            Assert.Equal("Hourly", sensor.MeasurementFrequency);
            Assert.Equal("Low", sensor.SafeLevel);
            Assert.Equal("Active", sensor.Status);
            Assert.True(sensor.IsFlagged);
        }

        [Fact]
        public void SensorMeta_DefaultIsFlagged_ShouldBeFalse()
        {
            var sensor = new SensorMeta();
            Assert.False(sensor.IsFlagged);
        }
    }
}