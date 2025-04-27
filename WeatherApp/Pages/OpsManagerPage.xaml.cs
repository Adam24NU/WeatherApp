using System.Collections.ObjectModel;
using WeatherApp.Models;
using WeatherApp.Resources;
using WeatherApp; 

namespace WeatherApp.Pages
{
    public partial class OpsManagerPage : ContentPage
    {
        private readonly Database _database;  // Add database as a dependency (Adam's task)
        private List<SensorMeta> allData = new();
        private int itemsPerPage = 10;
        private int currentPage = 0;

        public ObservableCollection<SensorMeta> DisplayedData { get; set; } = new();

        public OpsManagerPage(Database database)  // Adam's task to inject Database
        {
            InitializeComponent();
            BindingContext = this;
            _database = database;  // Assign injected Database instance

            LoadInitialData();  // Load initial data from the database
        }

        // Adam's task to load data from SQL Server instead of Excel
        private void LoadInitialData()
        {
            allData = _database.GetSensorsData();  // Fetch data from database
            LoadNextItems();
        }

        private void LoadNextItems()
        {
            var nextBatch = allData
                .Skip(currentPage * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            foreach (var item in nextBatch)
                DisplayedData.Add(item);

            currentPage++;
        }

        // When user clicks on "Report Issue" (Bart's task to update the database)
        private async void OnReportIssueClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is SensorMeta sensor)
            {
                bool confirm = await DisplayAlert(
                    "Report Malfunction",
                    $"Do you want to flag sensor {sensor.SensorID} as malfunctioning?",
                    "Yes", "Cancel");

                if (confirm)
                {
                    // Update the status in the database (Bart's task to update status)
                    _database.FlagSensorAsMalfunctioning(sensor.SensorID);

                    // Force refresh (basic workaround)
                    var index = DisplayedData.IndexOf(sensor);
                    DisplayedData.RemoveAt(index);
                    DisplayedData.Insert(index, sensor);
                }
            }
        }

        private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            LoadNextItems();
        }
    }
}
