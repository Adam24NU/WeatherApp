using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using WeatherApp.Repositories;

public partial class ScientistMapPageViewModel
{
    private readonly ISiteRepository _siteRepository;

    // Constructor
    public ScientistMapPageViewModel(ISiteRepository siteRepository)
    {
        _siteRepository = siteRepository;
        ShowSensorCommand = new AsyncRelayCommand<string>(ShowSensorByTypeAsync);
    }

    // Command to show sensor by type
    public IAsyncRelayCommand<string> ShowSensorCommand { get; }

    private async Task ShowSensorByTypeAsync(string type)
    {
        Debug.WriteLine($"[ShowSensorByTypeAsync] Called with type: {type}");

        var site = await _siteRepository.GetSiteByTypeAsync(type);

        if (site != null)
        {
            Debug.WriteLine($"[ShowSensorByTypeAsync] Retrieved site: ID={site.SiteId}, Type={site.Type}, Latitude={site.Latitude}, Longitude={site.Longitude}");

            // Instead of calling UpdateMapLocationAsync, we invoke the callback to the View (IMapInvoker)
            OnMapLocationUpdated?.Invoke(site.Latitude, site.Longitude);
            Debug.WriteLine("[ShowSensorByTypeAsync] Map location update invoked.");
        }
        else
        {
            Debug.WriteLine($"[ShowSensorByTypeAsync] No site found for type: {type}");
        }
    }

    // Event that View will subscribe to, for updating the map
    public event Action<double, double> OnMapLocationUpdated;
}
