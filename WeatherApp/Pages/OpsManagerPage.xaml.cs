// Required libraries for Excel, SQL, UI, and collections
using System.Collections.ObjectModel;
using OfficeOpenXml;


namespace WeatherApp.Pages
{
    // This page is for Operations Managers. It has two tabs: Sensors and Maintenance
    public partial class OpsManagerPage : TabbedPage
    {
        // Reference to the database class for queries
        private readonly Database _database;

        // Observable collections bind directly to CollectionViews in the XAML
        public ObservableCollection<SensorMeta> DisplayedData { get; set; } = new();          // For live sensor overview
        public ObservableCollection<MaintenanceTask> MaintenanceTasks { get; set; } = new();  // For scheduled maintenance
        public ObservableCollection<ReferenceData> ReferenceTable { get; set; } = new();      // Optional reference metadata

        // Constructor — initializes the page with database reference
        public OpsManagerPage(Database database)
        {
            InitializeComponent();
            BindingContext = this; // Enables binding to public properties above
            _database = database;

            // Load data into each tab
            LoadInitialData();       // Loads current sensor info
            LoadMaintenanceData();   // Loads maintenance entries
            LoadReferenceData();     // Loads extra metadata from Excel
        }

        // Loads all sensors and adds them to the display list
        private void LoadInitialData()
        {
            var allData = _database.GetSensorsData();
            foreach (var sensor in allData)
            {
                DisplayedData.Add(sensor); // Populates the CollectionView for the Sensors tab
            }
        }

        // Loads maintenance task records from the database
        private void LoadMaintenanceData()
        {
            var maintenanceData = _database.GetMaintenanceData();
            foreach (var task in maintenanceData)
            {
                MaintenanceTasks.Add(task); // Populates the Maintenance tab
            }
        }

        // Loads reference metadata from Metadata.xlsx (sheet name: Sheet1)
        private void LoadReferenceData()
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "Metadata.xlsx");

            // Do nothing if file doesn't exist
            if (!File.Exists(filePath)) return;

            // Read the Excel sheet
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(filePath));
            var sheet = package.Workbook.Worksheets["Sheet1"];
            int rows = sheet.Dimension.Rows;

            // Start from row 2 to skip header
            for (int row = 2; row <= rows; row++)
            {
                ReferenceTable.Add(new ReferenceData
                {
                    Category = sheet.Cells[row, 1].Text,
                    Quantity = sheet.Cells[row, 2].Text,
                    Symbol = sheet.Cells[row, 3].Text,
                    Unit = sheet.Cells[row, 4].Text,
                    SafeLevel = sheet.Cells[row, 5].Text,
                    Frequency = sheet.Cells[row, 6].Text,
                    Sensor = sheet.Cells[row, 8].Text,
                    Url = sheet.Cells[row, 9].Text
                });
            }
        }

        // This handles the Report Issue button in the Sensors tab
        private async void OnReportIssueClicked(object sender, EventArgs e)
        {
            // Get the sensor associated with the button that was clicked
            var button = sender as Button;
            if (button?.CommandParameter is SensorMeta sensor)
            {
                // Ask the user to confirm they want to report an issue
                bool confirm = await DisplayAlert(
                    "Report Malfunction",
                    $"Do you want to flag sensor {sensor.SensorId} as malfunctioning?",
                    "Yes", "Cancel");

                if (confirm)
                {
                    // In this example, user ID is hardcoded (replace with real user logic)
                    var currentUserId = 1;

                    // Update database: flag sensor and create maintenance record
                    _database.FlagSensorAsMalfunctioning(sensor.SensorId);
                    _database.InsertMaintenanceRecord(sensor.SensorId, currentUserId, DateTime.Now,
                        "Sensor malfunction reported");

                    // Optional UI update: force refresh of the displayed list
                    var index = DisplayedData.IndexOf(sensor);
                    if (index >= 0)
                    {
                        DisplayedData.RemoveAt(index);
                        DisplayedData.Insert(index, sensor); // Re-insert sensor to update the UI
                    }
                }
            }
        }
    }

    // Class for holding reference metadata (loaded from Excel)
    public class ReferenceData
    {
        public string Category { get; set; }
        public string Quantity { get; set; }
        public string Symbol { get; set; }
        public string Unit { get; set; }
        public string SafeLevel { get; set; }
        public string Frequency { get; set; }
        public string Sensor { get; set; }
        public string Url { get; set; }
    }
}