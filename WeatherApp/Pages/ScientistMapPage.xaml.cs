using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls;
using WeatherApp.Models;

namespace WeatherApp.Pages
{
    public partial class ScientistMapPage : ContentPage
    {
        public ScientistMapPage()
        {
            InitializeComponent();
            LoadSensors();
        }

        private void LoadSensors()
        {
            // Example sensor data (use your actual sensor data source)
            var sensors = new List<SensorMeta>
            {
                new SensorMeta { SensorID = "001", Location = "55.95,-3.19", Status = "OK" },
                new SensorMeta { SensorID = "002", Location = "55.97,-3.20", Status = "Alert" },
            };

            foreach (var sensor in sensors)
            {
                var coords = sensor.Location.Split(',');
                var position = new Location(double.Parse(coords[0]), double.Parse(coords[1]));

                var pin = new Pin
                {
                    Label = $"Sensor {sensor.SensorID}",
                    Address = sensor.Status,
                    Location = position,
                    Type = PinType.Place
                };

                sensorMap.Pins.Add(pin);
            }
        }
    }
}
