using WeatherApp.Core.Repositories;
using WeatherApp.Core.Tools;
using WeatherApp.Core.ViewModels;

namespace WeatherApp.Views;

public partial class ScientistPage : ContentPage
{
    private ScientistPageViewModel _viewModel;

    // Constructor
    public ScientistPage(
        PhysicalQuantityRepository physicalQuantityRepository,
        MeasurementRepository measurementRepository,
        INavigationService navigationService
        )
    {
        InitializeComponent();

        // Initialize the ViewModel with required parameters
        _viewModel = new ScientistPageViewModel(physicalQuantityRepository, measurementRepository, navigationService);

        // Set the BindingContext
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Set up button commands for each quantity type  
        LoadAirCommand = new Command(async () => await _viewModel.OutputMeasurementsAsync("Air"));
        LoadWaterCommand = new Command(async () => await _viewModel.OutputMeasurementsAsync("Water"));
        LoadWeatherCommand = new Command(async () => await _viewModel.OutputMeasurementsAsync("Weather"));
    }

    // Button Command Bindings  
    public Command LoadAirCommand { get; set; }
    public Command LoadWaterCommand { get; set; }
    public Command LoadWeatherCommand { get; set; }
}
