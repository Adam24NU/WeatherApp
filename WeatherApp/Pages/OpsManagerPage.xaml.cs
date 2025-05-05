using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Data.SqlClient;  // Needed for SQL operations
using System;
using System.IO;
using OfficeOpenXml;
using Microsoft.Maui.Controls;
using WeatherApp.Models;

namespace WeatherApp.Pages
{
    public partial class OpsManagerPage : TabbedPage
    {
        private readonly Database _database;

        public ObservableCollection<SensorMeta> DisplayedData { get; set; } = new();
        public ObservableCollection<MaintenanceTask> MaintenanceTasks { get; set; } = new();
        public ObservableCollection<ReferenceData> ReferenceTable { get; set; } = new();

        public OpsManagerPage(Database database)
        {
            InitializeComponent();
            BindingContext = this;
            _database = database;

            LoadInitialData();
            LoadMaintenanceData();
            LoadReferenceData();
        }

        private void LoadInitialData()
        {
            var allData = _database.GetSensorsData();
            foreach (var sensor in allData)
            {
                DisplayedData.Add(sensor);
            }
        }

        private void LoadMaintenanceData()
        {
            var maintenanceData = _database.GetMaintenanceData();
            foreach (var task in maintenanceData)
            {
                MaintenanceTasks.Add(task);
            }
        }

        private void LoadReferenceData()
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "Metadata.xlsx");
            if (!File.Exists(filePath)) return;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(filePath));
            var sheet = package.Workbook.Worksheets["Sheet1"];
            int rows = sheet.Dimension.Rows;

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

        private async void OnReportIssueClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is SensorMeta sensor)
            {
                bool confirm = await DisplayAlert(
                    "Report Malfunction",
                    $"Do you want to flag sensor {sensor.SensorId} as malfunctioning?",
                    "Yes", "Cancel");

                if (confirm)
                {
                    var currentUserId = 1;
                    _database.FlagSensorAsMalfunctioning(sensor.SensorId);
                    _database.InsertMaintenanceRecord(sensor.SensorId, currentUserId, DateTime.Now,
                        "Sensor malfunction reported");

                    var index = DisplayedData.IndexOf(sensor);
                    if (index >= 0)
                    {
                        DisplayedData.RemoveAt(index);
                        DisplayedData.Insert(index, sensor);
                    }
                }
            }
        }
    }

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
