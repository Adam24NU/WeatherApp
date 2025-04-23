using WeatherApp.Models;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using Newtonsoft.Json;

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
            //// Example sensor data (use actual data from your database or API)
            //var sensors = new List<SensorMeta>
            //{
            //    new SensorMeta { SensorID = "Air Quality", Location = "55.94476,-3.183991", Status = "OK" },
            //    new SensorMeta { SensorID = "Water Quality", Location = "55.8611,-3.2540", Status = "Alert" },
            //    new SensorMeta { SensorID = "Weather", Location = "55.008785,-3.5856323", Status = "OK" }
            //};

            

            //// Pass the sensor data to the JavaScript function in the WebView
            //var javascript = $"addMarkersFromData({Newtonsoft.Json.JsonConvert.SerializeObject(sensors)});";
            //sensorMap.EvaluateJavaScriptAsync(javascript);
        }
    }
}
