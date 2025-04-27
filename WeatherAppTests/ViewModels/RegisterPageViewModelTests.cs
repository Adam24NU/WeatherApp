using Moq;
using WeatherApp.Models;
using WeatherApp.Repositories;
using WeatherApp.Tools;
using WeatherApp.ViewModels;

namespace WeatherAppTests.ViewModels
{
    public class RegisterPageViewModelTests
    {
        private Mock<DatabaseConnection> _mockDbConnection;
        private Mock<UserRepository> _mockUserRepository;
        private RegisterPageViewModel _viewModel;
        private readonly Mock<INavigationService> _mockNavigationService;

        public RegisterPageViewModelTests()
        {
            // Mock the DatabaseConnection
            _mockDbConnection = new Mock<DatabaseConnection>();

            // Create the UserRepository, passing in the mocked DatabaseConnection
            _mockUserRepository = new Mock<UserRepository>(_mockDbConnection.Object);

            // Initialize the mock for INavigationService
            _mockNavigationService = new Mock<INavigationService>();

            // Now create the ViewModel with the mocked UserRepository
            _viewModel = new RegisterPageViewModel(_mockUserRepository.Object, _mockNavigationService.Object);
        }

        [Fact]
        public async Task Register_SuccessfulRegistration_SetsStatusMessage()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.InsertUserAsync(It.IsAny<User>())).Returns(Task.FromResult(1));
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.Email = "john.doe@example.com";
            _viewModel.Password = "password123";
            _viewModel.Role = "Admin";

            // Act
            await _viewModel.Register();

            // Assert
            Assert.Equal("Registration successful!", _viewModel.StatusMessage);

            // Verify that the correct navigation method was called
            _mockNavigationService.Verify(nav => nav.NavigateToAsync("LoginPage"), Times.Once);
        }

        [Fact]
        public async Task Register_EmptyFields_SetsStatusMessage()
        {
            // Arrange
            _viewModel.FirstName = string.Empty;
            _viewModel.LastName = string.Empty;
            _viewModel.Email = string.Empty;
            _viewModel.Password = string.Empty;

            // Act
            await _viewModel.Register();

            // Assert
            Assert.Equal("Please fill all fields.", _viewModel.StatusMessage);
            _mockUserRepository.Verify(repo => repo.InsertUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Register_ExceptionDuringRegistration_SetsStatusMessage()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.InsertUserAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Database error"));

            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.Email = "john.doe@example.com";
            _viewModel.Password = "password123";
            _viewModel.Role = "Admin";

            // Act
            await _viewModel.Register();

            // Assert
            Assert.Equal("Registration failed: Database error", _viewModel.StatusMessage);
        }
    }
}
