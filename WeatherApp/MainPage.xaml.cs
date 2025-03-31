using OfficeOpenXml;
using System.Text;

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

            var sb = new StringBuilder();

            // Adjust start row as needed
            int startRow = 6;
            int endRow = worksheet.Dimension.End.Row;

            for (int row = startRow; row <= endRow; row++)
            {
                var siteName = worksheet.Cells[1, 2].Text; // Site name (if needed)
                var value1 = worksheet.Cells[row, 1].Text; // Timestamp
                var value2 = worksheet.Cells[row, 2].Text; // NO2
                var value3 = worksheet.Cells[row, 3].Text; // PM2.5

                if (!string.IsNullOrWhiteSpace(value1))
                {
                    sb.AppendLine($"{value1} | {value2} | {value3}");
                }
            }

            DataLabel.Text = sb.ToString();
        }
    }
}