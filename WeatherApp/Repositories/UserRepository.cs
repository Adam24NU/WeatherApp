using Microsoft.Data.SqlClient;
using WeatherApp.Models;
using BCrypt.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherApp.Repositories;

//// <summary>
//// Repository class for CRUD functionality for user data. 
//// </summary>
public class UserRepository
{
    private readonly DatabaseConnection _dbConnection;

    public UserRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = new List<User>();
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Users", connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                UserId = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2),
                FirstName = reader.GetString(3),
                LastName = reader.GetString(4),
                Role = reader.GetString(5)
            });
        }

        return users;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Users WHERE user_id = @UserId", connection);
        command.Parameters.AddWithValue("@UserId", id);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                UserId = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2),
                FirstName = reader.GetString(3),
                LastName = reader.GetString(4),
                Role = reader.GetString(5)
            };
        }

        return null;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Users WHERE email = @Email", connection);
        command.Parameters.AddWithValue("@Email", email);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                UserId = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2),
                FirstName = reader.GetString(3),
                LastName = reader.GetString(4),
                Role = reader.GetString(5)
            };
        }

        return null;
    }

    public async Task<int> InsertUserAsync(User user)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        System.Diagnostics.Debug.WriteLine($"Hashing password for: {user.Email}");

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

        using var command = new SqlCommand(
            "INSERT INTO Users (first_name, last_name, email, role, password) " +
            "OUTPUT INSERTED.user_id " +
            "VALUES (@FirstName, @LastName, @Email, @Role, @Password)", connection);
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Role", user.Role);
        command.Parameters.AddWithValue("@Password", hashedPassword);

        return (int)await command.ExecuteScalarAsync();
    }


    public async Task UpdateUserAsync(User user)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        // Hash the password before updating
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

        using var command = new SqlCommand(
            "UPDATE Users SET first_name = @FirstName, last_name = @LastName, email = @Email, role = @Role, password = @Password " +
            "WHERE user_id = @UserId", connection);
        command.Parameters.AddWithValue("@UserId", user.UserId);
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Role", user.Role);
        command.Parameters.AddWithValue("@Password", hashedPassword);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("DELETE FROM Users WHERE user_id = @UserId", connection);
        command.Parameters.AddWithValue("@UserId", id);
        await command.ExecuteNonQueryAsync();
    }

    // Method to verify a user's password
    public async Task<bool> VerifyUserPasswordAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null)
        {
            System.Diagnostics.Debug.WriteLine($"No user found for email: {email}");
            return false;
        }

        System.Diagnostics.Debug.WriteLine($"Verifying password for user: {email}");

        try
        {
            bool isMatch = BCrypt.Net.BCrypt.Verify(password, user.Password);
            System.Diagnostics.Debug.WriteLine($"Password match: {isMatch}");
            return isMatch;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BCrypt verification error: {ex.Message}");
            return false;
        }
    }

}