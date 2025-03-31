using OfficeOpenXml;
using WeatherApp.Models;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            // ✅ Set the license for non-commercial student use
            ExcelPackage.License.SetNonCommercialPersonal("Adam");
            LoadExcelData();
        }

        [Obsolete]
        private async void LoadExcelData()
        {
            var fileName = "Air_quality.xlsx";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(filePath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                using var outStream = File.Create(filePath);
                await stream.CopyToAsync(outStream);
            }

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];

            int startRow = 6;
            int endRow = worksheet.Dimension.End.Row;

            var readings = new List<AirReading>();

            for (int row = startRow; row <= endRow; row++)
            {
                var time = worksheet.Cells[row, 1].Text;
                var no2 = worksheet.Cells[row, 2].Text;
                var pm25 = worksheet.Cells[row, 3].Text;

                if (!string.IsNullOrWhiteSpace(time))
                {
                    readings.Add(new AirReading
                    {
                        Time = time,
                        NO2 = no2,
                        PM25 = pm25
                    });
                }
            }

            DataList.ItemsSource = readings;
        }
    }
}