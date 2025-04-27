using Moq;
using WeatherApp.Tools;
using WeatherApp.ViewModels;

namespace WeatherAppTests.ViewModels
{
    public class MainPageViewModelTests
    {
        private Mock<INavigationService> _mockNavigationService;
        private Mock<IAlertService> _mockAlertService;
        private MainPageViewModel _viewModel;

        public MainPageViewModelTests()
        {
            // Mock the INavigationService
            _mockNavigationService = new Mock<INavigationService>();

            // Mock the IAlertService
            _mockAlertService = new Mock<IAlertService>();

            // Create the ViewModel, passing in the mocked services
            _viewModel = new MainPageViewModel(_mockNavigationService.Object, _mockAlertService.Object);
        }

        [Fact]
        public async Task NavigateToLogin_Success()
        {
            // Arrange
            _mockNavigationService.Setup(ns => ns.NavigateToAsync("LoginPage")).Returns(Task.CompletedTask);

            // Act
            await _viewModel.NavigateToLogin();

            // Assert
            _mockNavigationService.Verify(ns => ns.NavigateToAsync("LoginPage"), Times.Once);
            _mockAlertService.Verify(alert => alert.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task NavigateToRegister_success()
        {
            // Arrange
            _mockNavigationService.Setup(ns => ns.NavigateToAsync("RegisterPage")).Returns(Task.CompletedTask);

            // Act
            await _viewModel.NavigateToRegister();

            // Assert
            _mockNavigationService.Verify(ns => ns.NavigateToAsync("RegisterPage"), Times.Once);
            _mockAlertService.Verify(alert => alert.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async Task NavigateToLogin_ExceptionHandling()
        {
            // Arrange
            var exceptionMessage = "Navigation failed";
            _mockNavigationService.Setup(ns => ns.NavigateToAsync("LoginPage"))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _viewModel.NavigateToLogin();

            // Assert
            _mockAlertService.Verify(alert => alert.DisplayAlert("Error", $"Navigation failed: {exceptionMessage}", "OK"), Times.Once);
        }

        [Fact]
        public async Task NavigateToRegister_ExceptionHandling()
        {
            // Arrange
            var exceptionMessage = "Navigation failed";
            _mockNavigationService.Setup(ns => ns.NavigateToAsync("RegisterPage"))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _viewModel.NavigateToRegister();

            // Assert
            _mockAlertService.Verify(alert => alert.DisplayAlert("Error", $"Navigation failed: {exceptionMessage}", "OK"), Times.Once);
        }
    }
}
