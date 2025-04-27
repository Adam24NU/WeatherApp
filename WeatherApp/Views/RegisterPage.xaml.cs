using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class RegisterPage : ContentPage
{
    private readonly RegisterPageViewModel _viewModel;

    public RegisterPage(RegisterPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}