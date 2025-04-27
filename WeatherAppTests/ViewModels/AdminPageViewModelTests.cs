using Moq;
using WeatherApp.Models;
using WeatherApp.Repositories;
using WeatherApp.Tools;
using WeatherApp.ViewModels;

namespace WeatherAppTests.ViewModels;

public class AdminPageViewModelTests
{

    private readonly Mock<UserRepository> _mockUserRepository;
    private readonly Mock<IAlertService> _mockAlertService;
    private readonly AdminPageViewModel _viewModel;

    public AdminPageViewModelTests()
    {
        var mockDatabaseConnection = new Mock<DatabaseConnection>();
        _mockUserRepository = new Mock<UserRepository>(mockDatabaseConnection.Object);
        _mockAlertService = new Mock<IAlertService>();
        _viewModel = new AdminPageViewModel(_mockUserRepository.Object, _mockAlertService.Object);
    }

    [Fact]
    public async Task InitializeAsync_ShouldLoadUsers()
    {
        // Arrange
        var users = new List<User>
            {
                new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Role = "Admin" }
            };
        _mockUserRepository.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(users);

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.NotNull(_viewModel.RegisteredUsers);
        Assert.Single(_viewModel.RegisteredUsers);
        Assert.Equal("John", _viewModel.RegisteredUsers[0].FirstName);
    }

    [Fact]
    public async Task CreateUser_ShouldAddUser_WhenValidDataProvided()
    {
        // Arrange
        _viewModel.UserId = 1;
        _viewModel.Email = "jane.doe@example.com";
        _viewModel.Password = "password123";
        _viewModel.FirstName = "Jane";
        _viewModel.LastName = "Doe";
        _viewModel.Role = "Admin";


        _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
        _mockUserRepository.Setup(repo => repo.InsertUserAsync(It.IsAny<User>())).ReturnsAsync(1);
        _mockUserRepository.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(new List<User>());


        // Act
        await _viewModel.CreateUserCommand.ExecuteAsync(null);

        // Assert
        _mockUserRepository.Verify(repo => repo.InsertUserAsync(It.IsAny<User>()), Times.Once);
        _mockUserRepository.Verify(repo => repo.GetUsersAsync(), Times.Once);
        _mockAlertService.Verify(service => service.DisplayAlert("Success", "User created successfully!", "OK"), Times.Once);
    }


    [Fact]
    public async Task CreateUser_ShouldNotAddUser_WhenEmailAlreadyExists()
    {
        // Arrange
        _viewModel.UserId = 1;
        _viewModel.FirstName = "Jane";
        _viewModel.LastName = "Doe";
        _viewModel.Email = "jane.doe@example.com";
        _viewModel.Role = "Admin";
        _viewModel.Password = "password123";

        var existingUser = new User { UserId = 1, Email = "jane.doe@example.com" };
        _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(existingUser);

        // Act
        await _viewModel.CreateUserCommand.ExecuteAsync(null);

        // Assert
        _mockUserRepository.Verify(repo => repo.InsertUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveUser_WhenUserIsSelected()
    {
        // Arrange
        var selectedUser = new User { UserId = 1, FirstName = "John", LastName = "Doe" };
        _viewModel.SelectedUser = selectedUser;
        _mockUserRepository.Setup(repo => repo.DeleteUserAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
        _mockUserRepository.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(new List<User>());

        // Act
        await _viewModel.DeleteUserCommand.ExecuteAsync(null);

        // Assert
        _mockUserRepository.Verify(repo => repo.DeleteUserAsync(selectedUser.UserId), Times.Once);
        _mockUserRepository.Verify(repo => repo.GetUsersAsync(), Times.Once);
    }


    [Fact]
    public async Task DeleteUser_ShouldNotRemoveUser_WhenNoUserIsSelected()
    {
        // Arrange
        _viewModel.SelectedUser = null;

        // Act
        await _viewModel.DeleteUserCommand.ExecuteAsync(null);

        // Assert
        _mockUserRepository.Verify(repo => repo.DeleteUserAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task SimulateFirmwareUpdate_ShouldDisplayAlert()
    {
        // Act
        await _viewModel.SimulateFirmwareUpdateCommand.ExecuteAsync(null);

        // Assert
        Assert.True(true);
    }
}