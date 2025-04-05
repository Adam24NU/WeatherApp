using OfficeOpenXml;
using WeatherApp.Models;

namespace WeatherApp.Pages
{

    public partial class MainPage : TabbedPage
    {
        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            // ✅ Set the license for non-commercial student use
            ExcelPackage.License.SetNonCommercialPersonal("Adam");
            LoadExcelData();
            LoadWeatherData();

        }


        [Obsolete]
        private async void LoadWeatherData() // Weather.xlsx
        {
            var fileName = "Weather.xlsx";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(filePath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                using var outStream = File.Create(filePath);
                await stream.CopyToAsync(outStream);
            }

            var readings = new List<WeatherReading>();

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];

            int startRow = 5; // Actual data starts from row 5
            int endRow = worksheet.Dimension.End.Row;

            for (int row = startRow; row <= endRow; row++)
            {
                readings.Add(new WeatherReading
                {
                    Timestamp = worksheet.Cells[row, 1].Text,
                    Temperature = worksheet.Cells[row, 2].Text,
                    RelativeHumidity = worksheet.Cells[row, 3].Text,
                    WindSpeed = worksheet.Cells[row, 4].Text,
                    WindDirection = worksheet.Cells[row, 5].Text
                });
            }

            WeatherList.ItemsSource = readings;
        }



        [Obsolete]
        private async void LoadExcelData() // AirQuality.xlsx 
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

            int startRow = 11;
            int endRow = worksheet.Dimension.End.Row;

            var readings = new List<AirReading>();

            for (int row = startRow; row <= endRow; row++)
            {
                var timestamp = worksheet.Cells[row, 1].Text;
                var no2 = worksheet.Cells[row, 3].Text;
                var su = worksheet.Cells[row,4].Text;
                var pm25 = worksheet.Cells[row, 5].Text;
                var pm10 = worksheet.Cells[row, 6].Text;

                if (!string.IsNullOrWhiteSpace(timestamp))
                {
                    readings.Add(new AirReading
                    {
                        Timestamp = timestamp,
                        NO2 = no2,
                        Su = su,
                        PM25 = pm25,
                        PM10 = pm10
                    });
                }
            }

            DataList.ItemsSource = readings;
        }
    }
}