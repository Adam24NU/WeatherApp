using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Data.SqlClient;  // Needed for SQL operations
using System;

namespace WeatherApp.Pages
{
    public partial class OpsManagerPage : TabbedPage
    {
        private readonly Database _database;

        public ObservableCollection<SensorMeta> DisplayedData { get; set; } = new ObservableCollection<SensorMeta>();
        public ObservableCollection<MaintenanceTask> MaintenanceTasks { get; set; } = new ObservableCollection<MaintenanceTask>();

        public OpsManagerPage(Database database)  // Constructor to inject Database
        {
            InitializeComponent();
            BindingContext = this;
            _database = database;
            
            LoadInitialData();  // Load initial data for the Sensors tab
            LoadMaintenanceData(); // Load initial data for the Maintenance tab
        }

        // Load initial sensor data from the database
        private void LoadInitialData()
        {
            
            var allData = _database.GetSensorsData();  // Fetch data from the database
            Console.WriteLine($"Fetched {allData.Count} sensor records.");

            foreach (var sensor in allData)
            {
                DisplayedData.Add(sensor);
                Console.WriteLine($"Added sensor: {sensor.SensorId} - {sensor.SensorType}");
            }
        }

        // Load maintenance data from the database
        private void LoadMaintenanceData()
        {
            var maintenanceData = _database.GetMaintenanceData();  // Fetch maintenance records
            foreach (var task in maintenanceData)
            {
                MaintenanceTasks.Add(task);
            }
        }
        // When user clicks on "Report Issue" button
        private async void OnReportIssueClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is SensorMeta sensor)
            {
                // Show confirmation dialog to the user
                bool confirm = await DisplayAlert(
                    "Report Malfunction",
                    $"Do you want to flag sensor {sensor.SensorId} as malfunctioning?",
                    "Yes", "Cancel");

                if (confirm)
                {
                    // Assume currentUserId is the ID of the logged-in user
                    var currentUserId = 1; // Replace this with the actual logged-in user's ID

                    // Flag the sensor as malfunctioning in the database
                    _database.FlagSensorAsMalfunctioning(sensor.SensorId);

                    // Insert a new maintenance record for the sensor in the database
                    _database.InsertMaintenanceRecord(sensor.SensorId, currentUserId, DateTime.Now,
                        "Sensor malfunction reported");

                    // Force refresh the UI (this is a basic workaround)
                    var index = DisplayedData.IndexOf(sensor);
                    if (index >= 0)
                    {
                        // Update the sensor status or other relevant fields in the UI
                        DisplayedData.RemoveAt(index);
                        DisplayedData.Insert(index, sensor);
                    }
                }
            }
        }
    }
}
