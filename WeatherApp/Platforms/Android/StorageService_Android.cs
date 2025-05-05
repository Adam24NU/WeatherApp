using Android.OS;
using Android.Content;
using Java.IO;
using WeatherApp.Services;

//[assembly: Dependency(typeof(WeatherApp.Platforms.Android.StorageService_Android))]
namespace WeatherApp.Platforms.Android
{
    public class StorageService_Android : IStorageService
    {
        public long GetAvailableStorageBytes()
        {
            var context = global::Android.App.Application.Context;
            var stat = new StatFs(context.FilesDir.AbsolutePath);
            return stat.AvailableBytes;
        }

        public long GetTotalStorageBytes()
        {
            var context = global::Android.App.Application.Context;
            var stat = new StatFs(context.FilesDir.AbsolutePath);
            return stat.TotalBytes;
        }
    }
}
