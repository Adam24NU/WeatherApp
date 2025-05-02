using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WeatherApp.Core.Models;
using WeatherApp.Core.Repositories;
using WeatherApp.Core.Tools;

namespace WeatherApp.Core.ViewModels;

    /// <summary>
    /// ViewModel for the RegisterPage.
    /// Handles user registration and navigation within the app.
    /// </summary>
    public partial class RegisterPageViewModel : ObservableObject
    {
        private readonly UserRepository _userRepository;
        private readonly INavigationService _navigationService;

        // Properties bound to the registration form fields
        [ObservableProperty]
        private string firstName;

        [ObservableProperty]
        private string lastName;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string role;

        [ObservableProperty]
        private string statusMessage;

        [ObservableProperty]
        private ObservableCollection<string> availableRoles;

        /// <summary>
        /// Constructor that initializes the user repository, navigation service, and available user roles.
        /// </summary>
        /// <param name="userRepository">Service for accessing and managing user data.</param>
        /// <param name="navigationService">Service responsible for page navigation.</param>
        public RegisterPageViewModel(UserRepository userRepository, INavigationService navigationService)
        {
            try
            {
                _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
                _navigationService = navigationService;
                AvailableRoles = new ObservableCollection<string> { "Admin", "Scientist", "OpsManager" };
                Role = "Admin"; // Set default role to Admin
            }
            catch (Exception ex)
            {
                StatusMessage = $"Initialization failed: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"RegisterPageViewModel init error: {ex}");
            }
        }

        /// <summary>
        /// Command that handles the user registration process.
        /// Validates input fields, checks for duplicate email, and inserts the new user into the database.
        /// Navigates to the LoginPage upon successful registration.
        /// </summary>
        [RelayCommand]
        public async Task Register()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Registering: {FirstName} {LastName}, {Email}, Role: {Role}");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(FirstName) ||
                    string.IsNullOrWhiteSpace(LastName) ||
                    string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Password) ||
                    string.IsNullOrWhiteSpace(Role))
                {
                    StatusMessage = "Please fill all fields.";
                    return;
                }

                // Check if email is already in use
                var existingUser = await _userRepository.GetUserByEmailAsync(Email);
                if (existingUser != null)
                {
                    StatusMessage = "Email already exists.";
                    System.Diagnostics.Debug.WriteLine("Duplicate email detected.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Password before hashing: {Password}");

                // Create and insert new user
                var newUser = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Password = Password,
                    Role = Role
                };

                await _userRepository.InsertUserAsync(newUser);
                StatusMessage = "Registration successful!";
                await _navigationService.NavigateToAsync("LoginPage");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Registration failed: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Register error: {ex}");
            }
        }

        /// <summary>
        /// Command to navigate back to the previous page.
        /// </summary>
        [RelayCommand]
        public async Task NavigateBack()
        {
            try
            {
                await _navigationService.NavigateToAsync("..");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Navigation failed: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"NavigateBack error: {ex}");
            }
        }
    }
