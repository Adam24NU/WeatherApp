using OfficeOpenXml;
using System.Collections.ObjectModel;
using WeatherApp.Models;
using Microsoft.Maui.Controls;



namespace WeatherApp.Pages
{
    public partial class OpsManagerPage : ContentPage
    {
        private List<SensorMeta> allData = new();
        private int itemsPerPage = 10;
        private int currentPage = 0;

        public ObservableCollection<SensorMeta> DisplayedData { get; set; } = new();

        [Obsolete]
        public OpsManagerPage()
        {
            InitializeComponent();
            BindingContext = this;
            ExcelPackage.License.SetNonCommercialPersonal("Adam");
            LoadInitialData();
        }

        [Obsolete]
        private async void LoadInitialData()
        {
            allData = await LoadMetaDataFromExcel();
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

        private async Task<List<SensorMeta>> LoadMetaDataFromExcel()
        {
            var fileName = "Metadata.xlsx";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(filePath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                using var outStream = File.Create(filePath);
                await stream.CopyToAsync(outStream);
            }

            var result = new List<SensorMeta>();

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.End.Row;

            for (int row = 2; row <= rowCount; row++)
            {
                var sensor = new SensorMeta
                {
                    Category = worksheet.Cells[row, 1].Text,
                    SensorID = worksheet.Cells[row, 2].Text,
                    Symbol = worksheet.Cells[row, 3].Text,
                    Unit = worksheet.Cells[row, 4].Text,
                    UnitDescription = worksheet.Cells[row, 5].Text,
                    Frequency = worksheet.Cells[row, 6].Text,
                    SafeLevel = worksheet.Cells[row, 7].Text,
                    Reference = worksheet.Cells[row, 8].Text,
                    Model = worksheet.Cells[row, 9].Text,

                    // Dummy data for now (you can refine later)
                    Location = "Zone A",
                    Installed = DateTime.MinValue,
                    Status = "Operational",
                    MaintenanceDate = DateTime.Now.AddDays(7)
                };

                result.Add(sensor);
            }

            return result;
        }

        private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            Console.WriteLine("🚀 Threshold reached!");
            LoadNextItems(); ;
        }
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
                    sensor.IsFlagged = true;

                    // Force refresh (basic workaround)
                    var index = DisplayedData.IndexOf(sensor);
                    DisplayedData.RemoveAt(index);
                    DisplayedData.Insert(index, sensor);
                }
            }
        }

    }
}
