using Xunit;
using WeatherApp;

namespace WeatherApp.UnitTests
{
    public class MaintenanceTaskTests
    {
        [Fact]
        public void MaintenanceTask_ShouldSetAllPropertiesCorrectly()
        {
            var task = new MaintenanceTask
            {
                SensorId = 202,
                SensorType = "Water Quality",
                MaintainerEmail = "maintainer@example.com",
                Status = "Pending",
                MaintenanceDate = "2025-05-01",
                MaintainerComments = "Needs filter replacement",
                Timestamp = "2025-04-28 10:00"
            };

            Assert.Equal(202, task.SensorId);
            Assert.Equal("Water Quality", task.SensorType);
            Assert.Equal("maintainer@example.com", task.MaintainerEmail);
            Assert.Equal("Pending", task.Status);
            Assert.Equal("2025-05-01", task.MaintenanceDate);
            Assert.Equal("Needs filter replacement", task.MaintainerComments);
            Assert.Equal("2025-04-28 10:00", task.Timestamp);
        }
    }
}