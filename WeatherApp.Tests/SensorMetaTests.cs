using Xunit;
using WeatherApp.Models;

namespace WeatherApp.Models
{
    public class SensorMetaTests
    {
        [Fact]
        public void SensorMeta_ShouldInitializeCorrectly()
        {
            // Arrange
            var sensor = new SensorMeta
            {
                SensorID = "001",
               
                Location = "55.95,-3.19",
                Installed = DateTime.Now,
                Status = "OK",
                IsFlagged = false
            };

            // Assert
            Assert.Equal("001", sensor.SensorID);
           
            Assert.Equal("55.95,-3.19", sensor.Location);
            Assert.Equal("OK", sensor.Status);
            Assert.False(sensor.IsFlagged);
        }

        [Fact]
        public void SensorMeta_Flagged_ShouldBeTrueWhenFlagged()
        {
            // Arrange
            var sensor = new SensorMeta
            {
                SensorID = "002",
                
                Location = "55.97,-3.20",
                Installed = DateTime.Now,
                Status = "Alert",
                IsFlagged = true
            };

            // Assert
            Assert.True(sensor.IsFlagged);
        }
    }
}
