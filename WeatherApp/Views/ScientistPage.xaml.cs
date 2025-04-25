using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class ScientistPage : ContentPage
{
    private ScientistPageViewModel _viewModel;

    public ScientistPage()
    {
        InitializeComponent();

        // Initialize the ViewModel
        _viewModel = new ScientistPageViewModel();

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
