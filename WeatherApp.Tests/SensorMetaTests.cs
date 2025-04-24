
using Xunit;
namespace WeatherApp.Core
{
    public class SensorMetaTests
    {
        [Fact]
        public void SensorMeta_ShouldInitializeCorrectly()
        {
            var sensor = new SensorMeta
            {
                SensorID = "001",
                Location = "55.95,-3.19",
                Status = "OK",
                Installed = DateTime.Now,
                IsFlagged = false
            };

            Assert.Equal("001", sensor.SensorID);
            Assert.Equal("55.95,-3.19", sensor.Location);
            Assert.Equal("OK", sensor.Status);
            Assert.False(sensor.IsFlagged);
        }
    }
}