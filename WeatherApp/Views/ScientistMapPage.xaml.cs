using System.Diagnostics;
using WeatherApp.Core.Repositories;
using WeatherApp.Tools;
using WeatherApp.Core.ViewModels;
using Android.Hardware;
using Newtonsoft.Json;

namespace WeatherApp.Views
{
    public partial class ScientistMapPage : ContentPage, IMapInvoker
    {
        private ScientistMapPageViewModel _viewModel;
        private readonly DatabaseConnection _dbConnection;

        public ScientistMapPage(ISiteRepository siteRepository)
        {
            InitializeComponent();

            _dbConnection = new DatabaseConnection();

            // Initialize ViewModel  
            _viewModel = new ScientistMapPageViewModel(siteRepository, _dbConnection);
            BindingContext = _viewModel;

            // Subscribe to the map update event from the ViewModel  
            _viewModel.OnMapLocationUpdated += async (latitude, longitude, type, sensorDisplays) =>
            {
                // Ensure sensorDisplays is not null and contains data  
                if (sensorDisplays != null)
                {
                    // Pass the data to the UpdateMapLocationAsync method  
                    await UpdateMapLocationAsync(latitude, longitude, type, sensorDisplays.ToList());
                }
                else
                {
                    // Handle case where sensor display data is empty or null  
                    Debug.WriteLine("No sensor display data available.");
                }
            };
        }

        public async Task UpdateMapLocationAsync(double latitude, double longitude, string sensorType, IEnumerable<MapSensorDisplay> sensorDisplays)
        {
            if (sensorDisplays == null || !sensorDisplays.Any())
            {
                Debug.WriteLine("No sensor display data available.");
                return;
            }

            if (LeafletWebView == null)
            {
                Debug.WriteLine("LeafletWebView is null!");
                return;
            }

            // Prepare the sensor data to send to JS
            var sensorData = sensorDisplays.Select(sensorDisplay => new
            {
                sensorName = sensorDisplay.SensorName ?? "Unknown",
                quantityName = sensorDisplay.QuantityName ?? "Unknown",
                unit = sensorDisplay.Unit ?? "Unknown",
                value = sensorDisplay.Value,
                date = sensorDisplay.Date ?? "Unknown",
                time = sensorDisplay.Time ?? "Unknown",
                type = sensorDisplay.Type ?? "Unknown"
            }).ToList();

            // Convert to raw JSON (as object)
            var sensorDataJson = JsonConvert.SerializeObject(sensorData);

            // Make sure to encode the string *only* when needed (e.g. for string values)
            string script = $"updateMapLocation({latitude}, {longitude}, '{sensorType}', {sensorDataJson});";
            await LeafletWebView.EvaluateJavaScriptAsync(script);
        }

    }
}
