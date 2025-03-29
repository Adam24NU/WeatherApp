using OfficeOpenXml; // EPPlus
using System.Text;    // For StringBuilder
using System.Reflection;
using System.Data;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        LoadExcelData();
    }

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

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(new FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets[0];

        var sb = new StringBuilder();

        // Start from the row where actual data begins – adjust as needed
        int startRow = 6;
        int endRow = worksheet.Dimension.End.Row;

        for (int row = startRow; row <= endRow; row++)
        {
            var siteName = worksheet.Cells[1, 2].Text; // Or extract from header area if needed
            var value1 = worksheet.Cells[row, 1].Text; // e.g., Timestamp
            var value2 = worksheet.Cells[row, 2].Text; // e.g., NO2
            var value3 = worksheet.Cells[row, 3].Text; // e.g., PM2.5

            if (!string.IsNullOrWhiteSpace(value1))
            {
                sb.AppendLine($"{value1} | {value2} | {value3}");
            }
        }

        DataLabel.Text = sb.ToString();
    }
}
