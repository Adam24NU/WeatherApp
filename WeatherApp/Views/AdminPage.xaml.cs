using WeatherApp.Core.ViewModels;

namespace WeatherApp.Views;

public partial class AdminPage : ContentPage
{
    private readonly AdminPageViewModel _viewModel;

    public AdminPage(AdminPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}