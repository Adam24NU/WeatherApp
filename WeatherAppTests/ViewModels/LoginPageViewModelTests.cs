using Moq;
using WeatherApp.Core.Models;
using WeatherApp.Core.Repositories;
using WeatherApp.Core.Tools;
using WeatherApp.Core.ViewModels;

namespace WeatherAppTests.ViewModels
{
    public class LoginPageViewModelTests
    {
        private readonly Mock<DatabaseConnection> _mockDbConnection;
        private readonly Mock<UserRepository> _mockUserRepository;
        private readonly Mock<INavigationService> _mockNavigationService;
        private readonly LoginPageViewModel _viewModel;

        public LoginPageViewModelTests()
        {
            _mockDbConnection = new Mock<DatabaseConnection>();
            _mockUserRepository = new Mock<UserRepository>(_mockDbConnection.Object);

            // Initialize the mock for INavigationService
            _mockNavigationService = new Mock<INavigationService>();

            // Initialize the ViewModel with the mocked UserRepository and NavigationService
            _viewModel = new LoginPageViewModel(_mockUserRepository.Object, _mockNavigationService.Object);
        }

        [Fact]
        public async Task Login_ShouldSetStatusMessage_WhenEmailOrPasswordIsEmpty()
        {
            // Arrange
            _viewModel.Email = "";
            _viewModel.Password = "password123";

            // Act
            await _viewModel.Login();

            // Assert
            Assert.Equal("Please enter both email and password.", _viewModel.StatusMessage);
        }

        [Fact]
        public async Task Login_ShouldSetStatusMessage_WhenLoginIsSuccessful()
        {
            // Arrange
            _viewModel.Email = "jane.doe@example.com";
            _viewModel.Password = "password123";
            var user = new User { Email = "jane.doe@example.com", Role = "Admin" };

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.VerifyUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            // Act
            await _viewModel.Login();

            // Assert
            Assert.Equal("Login successful!", _viewModel.StatusMessage);
            _mockUserRepository.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.VerifyUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            // Verify that the correct navigation method was called
            _mockNavigationService.Verify(nav => nav.NavigateToAsync("AdminPage"), Times.Once);
        }

        [Fact]
        public async Task Login_ShouldSetStatusMessage_WhenLoginFails()
        {
            // Arrange
            _viewModel.Email = "jane.doe@example.com";
            _viewModel.Password = "wrongpassword";
            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User { Email = "jane.doe@example.com" });
            _mockUserRepository.Setup(repo => repo.VerifyUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            // Act
            await _viewModel.Login();

            // Assert
            Assert.Equal("Invalid email or password.", _viewModel.StatusMessage);
        }

        [Fact]
        public async Task Login_ShouldSetStatusMessage_WhenRoleIsUnknown()
        {
            // Arrange
            _viewModel.Email = "jane.doe@example.com";
            _viewModel.Password = "password123";
            var user = new User { Email = "jane.doe@example.com", Role = "UnknownRole" };

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.VerifyUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            // Act
            await _viewModel.Login();

            // Assert
            Assert.Equal("Unknown role.", _viewModel.StatusMessage);
        }
    }

}
