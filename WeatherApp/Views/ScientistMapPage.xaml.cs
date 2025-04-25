using Microsoft.Maui.Controls;
using WeatherApp.ViewModels;
using WeatherApp.Repositories;
using WeatherApp.Tools;
using System.Diagnostics;

namespace WeatherApp.Views
{
    public partial class ScientistMapPage : ContentPage, IMapInvoker
    {
        private ScientistMapPageViewModel _viewModel;

        public ScientistMapPage(ISiteRepository siteRepository)
        {
            InitializeComponent();

            // Initialize ViewModel
            _viewModel = new ScientistMapPageViewModel(siteRepository);
            BindingContext = _viewModel;

            // Subscribe to the map update event from the ViewModel
            _viewModel.OnMapLocationUpdated += async (latitude, longitude) => await UpdateMapLocationAsync(latitude, longitude);
        }

        // Implementation of IMapInvoker.UpdateMapLocationAsync
        public async Task UpdateMapLocationAsync(double latitude, double longitude)
        {
            Debug.WriteLine($"[UpdateMapLocationAsync] Updating map location: {latitude}, {longitude}");

            // Assuming 'LeafletWebView' is the name of your WebView control
            string script = $"updateMapLocation({latitude}, {longitude});";

            // Execute JavaScript in the WebView
            await LeafletWebView.EvaluateJavaScriptAsync(script);

            Debug.WriteLine("[UpdateMapLocationAsync] JavaScript executed to update map location.");
        }
    }
}
