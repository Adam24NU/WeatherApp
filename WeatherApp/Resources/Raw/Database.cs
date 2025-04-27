using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using WeatherApp.Models;

namespace WeatherApp.Resources
{
    public class Database
    {
        private string _connectionString;

        // Constructor that accepts the connection string (from appsettings.json)
        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Method to open the database connection
        public SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        // Method to fetch all sensors from the database (Adam's task)
        public List<SensorMeta> GetSensorsData()
        {
            var sensors = new List<SensorMeta>();

            using (var connection = OpenConnection())
            {
                string query = "SELECT * FROM dbo.Sensors";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var sensor = new SensorMeta
                    {
                        SensorID = reader["sensor_id"].ToString(),
                        Category = reader["category"].ToString(),
                        Symbol = reader["symbol"].ToString(),
                        Location = reader["location"].ToString(),
                        Installed = Convert.ToDateTime(reader["installed"]),
                        Status = reader["status"].ToString(),
                        MaintenanceDate = Convert.ToDateTime(reader["maintenance_date"])
                    };

                    sensors.Add(sensor);
                }
            }

            return sensors;
        }

        // Method to flag sensor as malfunctioning (Bart's task)
        public void FlagSensorAsMalfunctioning(string sensorId)
        {
            using (var connection = OpenConnection())
            {
                string query = "UPDATE dbo.Sensors SET status = 'Malfunctioning' WHERE sensor_id = @sensorId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@sensorId", sensorId);
                command.ExecuteNonQuery();
            }
        }

        // Adam's task: Authenticate user based on username and password
        public User AuthenticateUser(string username, string password)
        {
            using (var connection = OpenConnection())
            {
                string query = "SELECT * FROM dbo.Users WHERE email = @username AND password = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    var user = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        Username = reader["email"].ToString(),
                        Password = reader["password"].ToString(),
                        Role = reader["role"].ToString(),
                    };

                    return user;  // Return user object if found
                }

                return null;  // Return null if user not found
            }
        }

        // Adam's task: Register a new user in the database
        public bool RegisterUser(string username, string password, string role)
        {
            using (var connection = OpenConnection())
            {
                // Check if the username already exists
                string checkQuery = "SELECT COUNT(*) FROM dbo.Users WHERE email = @username";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@username", username);
                int userCount = (int)checkCommand.ExecuteScalar();

                if (userCount > 0) // Username already exists
                    return false;

                // Insert new user into the database
                string query = "INSERT INTO dbo.Users (email, password, role) VALUES (@username, @password, @role)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password); // You should hash passwords in real-world apps
                command.Parameters.AddWithValue("@role", role);
                command.ExecuteNonQuery();

                return true; // Registration successful
            }
        }

        // Adam's task: Get all users from the database (for AdminPage)
        public List<User> GetUsers()
        {
            var users = new List<User>();

            using (var connection = OpenConnection())
            {
                string query = "SELECT user_id, email, role FROM dbo.Users";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var user = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        Username = reader["email"].ToString(),
                        Role = reader["role"].ToString(),
                    };

                    users.Add(user);
                }
            }

            return users;
        }

        // Adam's task: Delete a user from the database (for AdminPage)
        public bool DeleteUser(int userId)
        {
            using (var connection = OpenConnection())
            {
                string query = "DELETE FROM dbo.Users WHERE user_id = @userId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;  // Return true if the user was deleted
            }
        }
    }
}
