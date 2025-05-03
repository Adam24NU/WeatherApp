using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using WeatherApp.Models;

namespace WeatherApp.Pages
{
    public partial class MainPage : TabbedPage
    {
        private List<AirReading> airReadings;
        private List<WeatherReading> weatherReadings;
        private List<WaterReading> waterReadings;

        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            ExcelPackage.License.SetNonCommercialPersonal("Adam Williams");
            LoadExcelData();
        }

        private async Task InitializeAsync()
        {
            await CopyExcelFilesIfNeededAsync();
            LoadExcelData();
        }

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

            DataList.ItemsSource = airReadings;
            WeatherList.ItemsSource = weatherReadings;
            WaterList.ItemsSource = waterReadings;
        }

        private List<AirReading> LoadAirReadingsFromExcel(string filePath)
        {
            var list = new List<AirReading>();
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            int rows = worksheet.Dimension.Rows;

            for (int row = 11; row <= rows; row++)
            {
                list.Add(new AirReading
                {
                    Timestamp = $"{worksheet.Cells[row, 1].Text} {worksheet.Cells[row, 2].Text}",
                    NO2 = worksheet.Cells[row, 3].Text,
                    PM25 = worksheet.Cells[row, 5].Text,
                    PM10 = worksheet.Cells[row, 6].Text
                });
            }
            return list;
        }

        private List<WeatherReading> LoadWeatherReadingsFromExcel(string filePath)
        {
            var list = new List<WeatherReading>();
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            int rows = worksheet.Dimension.Rows;

            for (int row = 5; row <= rows; row++)
            {
                list.Add(new WeatherReading
                {
                    Timestamp = worksheet.Cells[row, 1].Text,
                    Temperature = worksheet.Cells[row, 2].Text,
                    RelativeHumidity = worksheet.Cells[row, 3].Text,
                    WindSpeed = worksheet.Cells[row, 4].Text,
                    WindDirection = worksheet.Cells[row, 5].Text
                });
            }
            return list;
        }

        private List<WaterReading> LoadWaterReadingsFromExcel(string filePath)
        {
            var list = new List<WaterReading>();
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            int rows = worksheet.Dimension.Rows;

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

        [Obsolete]
        private async void OnGenerateAirQualityReportClicked(object sender, EventArgs e)
        {
            try
            {
                var documentationFolder = Path.Combine(FileSystem.AppDataDirectory, "Documentation");
                if (!Directory.Exists(documentationFolder))
                    Directory.CreateDirectory(documentationFolder);

                var reportFilePath = Path.Combine(documentationFolder, "AirQualityReport.csv");
                var sb = new StringBuilder();
                sb.AppendLine("Timestamp,NO2,PM2.5,PM10");

                foreach (var reading in airReadings)
                {
                    sb.AppendLine($"{reading.Timestamp},{reading.NO2},{reading.PM25},{reading.PM10}");
                }

                await File.WriteAllTextAsync(reportFilePath, sb.ToString());
                DisplayAirSummary(airReadings);
                AirQualityStatusLabel.TextColor = Colors.Green;
            }
            catch (Exception ex)
            {
                AirQualityStatusLabel.Text = $"Error: {ex.Message}";
                AirQualityStatusLabel.TextColor = Colors.Red;
            }
        }

        void DisplayAirSummary(List<AirReading> airReadings)
        {
            if (airReadings == null || !airReadings.Any())
            {
                AirQualityStatusLabel.Text = "No data available.";
                return;
            }

            var no2Values = airReadings.Select(r => double.TryParse(r.NO2, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();
            var pm25Values = airReadings.Select(r => double.TryParse(r.PM25, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();
            var pm10Values = airReadings.Select(r => double.TryParse(r.PM10, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();

            if (!no2Values.Any() || !pm25Values.Any() || !pm10Values.Any())
            {
                AirQualityStatusLabel.Text = "Insufficient valid numeric data.";
                return;
            }

            StringBuilder summary = new StringBuilder();
            summary.AppendLine("📊 Air Quality Summary:");
            summary.AppendLine($"NO2: Min = {no2Values.Min()}, Max = {no2Values.Max()}, Avg = {no2Values.Average():F1}");
            summary.AppendLine($"PM2.5: Min = {pm25Values.Min()}, Max = {pm25Values.Max()}, Avg = {pm25Values.Average():F1}");
            summary.AppendLine($"PM10: Min = {pm10Values.Min()}, Max = {pm10Values.Max()}, Avg = {pm10Values.Average():F1}");

            AirQualityStatusLabel.Text = summary.ToString();
        }

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
                DisplayWeatherSummary(weatherReadings);
                WeatherStatusLabel.TextColor = Colors.Green;
            }
            catch (Exception ex)
            {
                WeatherStatusLabel.Text = $"Error: {ex.Message}";
                WeatherStatusLabel.TextColor = Colors.Red;
            }
        }

        void DisplayWeatherSummary(List<WeatherReading> weatherReadings)
        {
            if (weatherReadings == null || !weatherReadings.Any())
            {
                WeatherStatusLabel.Text = "No data available.";
                return;
            }

            var tempValues = weatherReadings.Select(r => double.TryParse(r.Temperature, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();
            var windValues = weatherReadings.Select(r => double.TryParse(r.WindSpeed, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();
            var humidityValues = weatherReadings.Select(r => double.TryParse(r.RelativeHumidity, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();

            if (!tempValues.Any() || !windValues.Any() || !humidityValues.Any())
            {
                WeatherStatusLabel.Text = "Insufficient valid numeric data.";
                return;
            }

            StringBuilder summary = new StringBuilder();
            summary.AppendLine("📊 Weather Summary:");
            summary.AppendLine($"🌡 Temperature: Min = {tempValues.Min()}°C, Max = {tempValues.Max()}°C, Avg = {tempValues.Average():F1}°C");
            summary.AppendLine($"💨 Wind Speed:  Min = {windValues.Min()} km/h, Max = {windValues.Max()} km/h, Avg = {windValues.Average():F1} km/h");
            summary.AppendLine($"💧 Humidity:    Min = {humidityValues.Min()}%, Max = {humidityValues.Max()}%, Avg = {humidityValues.Average():F1}%");

            WeatherStatusLabel.Text = summary.ToString();
        }

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
                DisplayWaterSummary(waterReadings);
                WaterStatusLabel.TextColor = Colors.Green;
            }
            catch (Exception ex)
            {
                WaterStatusLabel.Text = $"Error: {ex.Message}";
                WaterStatusLabel.TextColor = Colors.Red;
            }
        }

        void DisplayWaterSummary(List<WaterReading> waterReadings)
        {
            if (waterReadings == null || !waterReadings.Any())
            {
                WaterStatusLabel.Text = "No data available.";
                return;
            }

            var nitrateValues = waterReadings.Select(r => double.TryParse(r.Nitrate, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();
            var nitriteValues = waterReadings.Select(r => double.TryParse(r.Nitrite, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();
            var phosphateValues = waterReadings.Select(r => double.TryParse(r.Phosphate, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();
            var ecValues = waterReadings.Select(r => double.TryParse(r.EC, out var val) ? val : (double?)null).Where(v => v.HasValue).Select(v => v.Value).ToList();

            if (!nitrateValues.Any() || !nitriteValues.Any() || !phosphateValues.Any() || !ecValues.Any())
            {
                WaterStatusLabel.Text = "Insufficient valid numeric data.";
                return;
            }

            StringBuilder summary = new StringBuilder();
            summary.AppendLine("📊 Water Quality Summary:");
            summary.AppendLine($"🧪 Nitrate: Min = {nitrateValues.Min()}, Max = {nitrateValues.Max()}, Avg = {nitrateValues.Average():F1}");
            summary.AppendLine($"🧪 Nitrite: Min = {nitriteValues.Min()}, Max = {nitriteValues.Max()}, Avg = {nitriteValues.Average():F1}");
            summary.AppendLine($"🧫 Phosphate: Min = {phosphateValues.Min()}, Max = {phosphateValues.Max()}, Avg = {phosphateValues.Average():F1}");
            summary.AppendLine($"🌊 EC: Min = {ecValues.Min()}, Max = {ecValues.Max()}, Avg = {ecValues.Average():F1}");

            WaterStatusLabel.Text = summary.ToString();
        }

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
