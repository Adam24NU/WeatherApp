using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using WeatherApp.Models;

namespace WeatherApp.Pages
{
    public partial class MainPage : TabbedPage
    {
        // Lists to store loaded data from Excel files
        private List<AirReading> airReadings;
        private List<WeatherReading> weatherReadings;
        private List<WaterReading> waterReadings;

        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            // Set license context for EPPlus library
            ExcelPackage.License.SetNonCommercialPersonal("Adam Williams");
            LoadExcelData();
        }

        // Initializes the app by copying Excel files if needed and then loading data
        private async Task InitializeAsync()
        {
            await CopyExcelFilesIfNeededAsync();
            LoadExcelData();
        }

        // Loads data from Excel files into memory and binds to UI
        private void LoadExcelData()
        {
            var airQualityFilePath = Path.Combine(FileSystem.AppDataDirectory, "Air_quality.xlsx");
            var weatherFilePath = Path.Combine(FileSystem.AppDataDirectory, "Weather.xlsx");
            var waterFilePath = Path.Combine(FileSystem.AppDataDirectory, "WaterQuality.xlsx");

            if (File.Exists(airQualityFilePath))
                airReadings = LoadAirReadingsFromExcel(airQualityFilePath);

            if (File.Exists(weatherFilePath))
                weatherReadings = LoadWeatherReadingsFromExcel(weatherFilePath);

            if (File.Exists(waterFilePath))
                waterReadings = LoadWaterReadingsFromExcel(waterFilePath);

            // Bind loaded data to UI CollectionViews
            DataList.ItemsSource = airReadings;
            WeatherList.ItemsSource = weatherReadings;
            WaterList.ItemsSource = waterReadings;
        }

        // Loads Air Quality data from Excel file
        private List<AirReading> LoadAirReadingsFromExcel(string filePath)
        {
            var list = new List<AirReading>();
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            int rows = worksheet.Dimension.Rows;

            // Start reading from row 11 based on Excel structure
            for (int row = 11; row <= rows; row++)
            {
                list.Add(new AirReading
                {
                    Timestamp = $"{worksheet.Cells[row, 1].Text} {worksheet.Cells[row, 2].Text}", // Combine Date + Time
                    NO2 = worksheet.Cells[row, 3].Text,
                    PM25 = worksheet.Cells[row, 5].Text,
                    PM10 = worksheet.Cells[row, 6].Text
                });
            }
            return list;
        }

        // Loads Weather data from Excel file
        private List<WeatherReading> LoadWeatherReadingsFromExcel(string filePath)
        {
            var list = new List<WeatherReading>();
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            int rows = worksheet.Dimension.Rows;

            // Start reading from row 5 based on Excel structure
            for (int row = 5; row <= rows; row++)
            {
                list.Add(new WeatherReading
                {
                    Timestamp = worksheet.Cells[row, 1].Text,
                    Temperature = worksheet.Cells[row, 2].Text,
                    WindSpeed = worksheet.Cells[row, 4].Text,
                    RelativeHumidity = worksheet.Cells[row, 3].Text,
                    WindDirection = worksheet.Cells[row, 5].Text
                });
            }
            return list;
        }

        // Loads Water Quality data from Excel file
        private List<WaterReading> LoadWaterReadingsFromExcel(string filePath)
        {
            var list = new List<WaterReading>();
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            int rows = worksheet.Dimension.Rows;

            // Start reading from row 6 based on Excel structure
            for (int row = 6; row <= rows; row++)
            {
                list.Add(new WaterReading
                {
                    Date = worksheet.Cells[row, 1].Text,
                    Time = worksheet.Cells[row, 2].Text,
                    Nitrate = worksheet.Cells[row, 3].Text,
                    Nitrite = worksheet.Cells[row, 4].Text,
                    Phosphate = worksheet.Cells[row, 5].Text,
                    EC = worksheet.Cells[row, 6].Text
                });
            }
            return list;
        }

        // Generates Air Quality Report and saves it as CSV file
        [Obsolete]
        private async void OnGenerateAirQualityReportClicked(object sender, EventArgs e)
        {
            try
            {
                var documentationFolder = Path.Combine(FileSystem.AppDataDirectory, "Documentation");

                // Create folder if it does not exist
                if (!Directory.Exists(documentationFolder))
                {
                    Directory.CreateDirectory(documentationFolder);
                }

                var reportFilePath = Path.Combine(documentationFolder, "AirQualityReport.csv");
                var sb = new StringBuilder();
                sb.AppendLine("Timestamp,NO2,PM2.5,PM10");

                foreach (var reading in airReadings)
                {
                    sb.AppendLine($"{reading.Timestamp},{reading.NO2},{reading.PM25},{reading.PM10}");
                }

                await File.WriteAllTextAsync(reportFilePath, sb.ToString());

                // Update UI status label
                AirQualityStatusLabel.Text = $"Air Quality Report generated at {reportFilePath}";
                AirQualityStatusLabel.TextColor = Colors.Green;
            }
            catch (Exception ex)
            {
                AirQualityStatusLabel.Text = $"Error: {ex.Message}";
                AirQualityStatusLabel.TextColor = Colors.Red;
            }
        }

        // Generates Weather Report and saves it as CSV file
        [Obsolete]
        private async void OnGenerateWeatherReportClicked(object sender, EventArgs e)
        {
            try
            {
                var reportFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "WeatherReport.csv");
                var sb = new StringBuilder();
                sb.AppendLine("Timestamp,Temperature,WindSpeed,RelativeHumidity,WindDirection");

                foreach (var reading in weatherReadings)
                {
                    sb.AppendLine($"{reading.Timestamp},{reading.Temperature},{reading.WindSpeed},{reading.RelativeHumidity},{reading.WindDirection}");
                }

                await File.WriteAllTextAsync(reportFilePath, sb.ToString());

                // Update UI status label
                WeatherStatusLabel.Text = $"Weather Report generated at {reportFilePath}";
                WeatherStatusLabel.TextColor = Colors.Green;
            }
            catch (Exception ex)
            {
                WeatherStatusLabel.Text = $"Error: {ex.Message}";
                WeatherStatusLabel.TextColor = Colors.Red;
            }
        }

        // Generates Water Quality Report and saves it as CSV file
        [Obsolete]
        private async void OnGenerateWaterReportClicked(object sender, EventArgs e)
        {
            try
            {
                var reportFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "WaterReport.csv");
                var sb = new StringBuilder();
                sb.AppendLine("Date,Time,Nitrate,Nitrite,Phosphate,EC");

                foreach (var reading in waterReadings)
                {
                    sb.AppendLine($"{reading.Date},{reading.Time},{reading.Nitrate},{reading.Nitrite},{reading.Phosphate},{reading.EC}");
                }

                await File.WriteAllTextAsync(reportFilePath, sb.ToString());

                // Update UI status label
                WaterStatusLabel.Text = $"Water Report generated at {reportFilePath}";
                WaterStatusLabel.TextColor = Colors.Green;
            }
            catch (Exception ex)
            {
                WaterStatusLabel.Text = $"Error: {ex.Message}";
                WaterStatusLabel.TextColor = Colors.Red;
            }
        }

        // Copies Excel files from app resources (Raw folder) to AppDataDirectory if not already copied
        private async Task CopyExcelFilesIfNeededAsync()
        {
            var filesToCopy = new List<string>
            {
                "Air_quality.xlsx",
                "Weather.xlsx",
                "WaterQuality.xlsx"
            };

            foreach (var filename in filesToCopy)
            {
                var destinationPath = Path.Combine(FileSystem.AppDataDirectory, filename);

                // Only copy if file does not exist already
                if (!File.Exists(destinationPath))
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync(Path.Combine("Raw", filename));
                    using var fileStream = File.Create(destinationPath);
                    await stream.CopyToAsync(fileStream);
                }
            }
        }
    }
}
