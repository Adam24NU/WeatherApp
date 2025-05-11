using Microsoft.Data.SqlClient; // SQL Server client for .NET
using WeatherApp.Models;

namespace WeatherApp
{
    // This class handles all database operations: users, sensors, and maintenance records
    public class Database
    {
        private readonly string _connectionString;

        // Constructor takes connection string when Database object is created
        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Opens a connection to SQL Server
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // --------------------- USER MANAGEMENT ---------------------

        // Get all users from the database
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT user_id, email, role FROM dbo.Users";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Create a user from each row in the result
                        var user = new User
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Role = reader.GetString(reader.GetOrdinal("role"))
                        };
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        // Registers a new user in the database
        public bool RegisterUser(string email, string password, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO dbo.Users (email, password, role) VALUES (@Email, @Password, @Role)";
                SqlCommand command = new SqlCommand(query, connection);

                // Parameters are used to prevent SQL injection
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password); // Plain text (you can improve later)
                command.Parameters.AddWithValue("@Role", role);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during registration: {ex.Message}");
                    return false;
                }
            }
        }

        // Authenticates a user by matching email and password
        public User AuthenticateUser(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM dbo.Users WHERE email = @email AND password = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = (int)reader["user_id"],
                            Email = (string)reader["email"],
                            Password = (string)reader["password"],
                            Role = (string)reader["role"]
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return null; // No matching user found
        }

        // Deletes a user from the database
        public bool DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM dbo.Users WHERE user_id = @userId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // --------------------- SENSOR & MAINTENANCE MANAGEMENT ---------------------

        // Flag a sensor as malfunctioning (simple placeholder logic)
        public void FlagSensorAsMalfunctioning(int sensorId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                // (This example query does not actually flag anything — update it later for production)
                string query = "SELECT s.sensor_id, s.site_id, s.sensor_type, pq.measurement_frequency " +
                               "FROM dbo.Sensors s " +
                               "JOIN dbo.PhysicalQuantities pq ON s.sensor_id = pq.sensor_id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SensorId", sensorId);
                command.ExecuteNonQuery(); // Placeholder — this should probably be an UPDATE
            }
        }

        // Get all sensors with related physical data
        public List<SensorMeta> GetSensorsData()
        {
            List<SensorMeta> sensors = new List<SensorMeta>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT s.sensor_id, s.sensor_type, p.measurement_frequency, p.safe_level " +
                               "FROM dbo.Sensors s " +
                               "LEFT JOIN dbo.PhysicalQuantities p ON s.sensor_id = p.sensor_id";

                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sensor = new SensorMeta
                        {
                            SensorId = reader.GetInt32(reader.GetOrdinal("sensor_id")),
                            SensorType = reader.GetString(reader.GetOrdinal("sensor_type")),
                            Status = "Active", // You assume all sensors are active
                            IsFlagged = false  // Placeholder
                        };

                        // Null safety checks
                        sensor.MeasurementFrequency = !reader.IsDBNull(reader.GetOrdinal("measurement_frequency"))
                            ? reader.GetString(reader.GetOrdinal("measurement_frequency"))
                            : "Not Available";

                        sensor.SafeLevel = !reader.IsDBNull(reader.GetOrdinal("safe_level"))
                            ? reader.GetString(reader.GetOrdinal("safe_level"))
                            : "Unknown";

                        sensors.Add(sensor);
                    }
                }
            }

            return sensors;
        }

        // --------------------- MAINTENANCE MANAGEMENT ---------------------

        // Fetch maintainer email by ID
        private string GetUserEmailById(int maintainerId)
        {
            string email = string.Empty;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT email FROM dbo.Users WHERE user_id = @maintainerId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@maintainerId", maintainerId);

                var result = command.ExecuteScalar();
                if (result != null)
                {
                    email = result.ToString();
                }
            }

            return email;
        }

        // Load all maintenance records and parse dates, emails
        public List<MaintenanceTask> GetMaintenanceData()
        {
            List<MaintenanceTask> maintenanceTasks = new List<MaintenanceTask>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT m.sensor_id, s.sensor_type, m.maintenance_date, m.maintainer_id, m.maintainer_comments " +
                               "FROM dbo.Maintenance m " +
                               "INNER JOIN dbo.Sensors s ON m.sensor_id = s.sensor_id";

                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new MaintenanceTask
                        {
                            SensorId = reader.GetInt32(reader.GetOrdinal("sensor_id")),
                            SensorType = reader.GetString(reader.GetOrdinal("sensor_type")),
                            MaintainerComments = reader.IsDBNull(reader.GetOrdinal("maintainer_comments"))
                                ? "No comments"
                                : reader.GetString(reader.GetOrdinal("maintainer_comments")),
                            Status = "Scheduled"
                        };

                        // Handle various date formats
                        if (!reader.IsDBNull(reader.GetOrdinal("maintenance_date")))
                        {
                            try
                            {
                                string rawDate = reader.GetValue(reader.GetOrdinal("maintenance_date")).ToString();

                                if (DateTime.TryParse(rawDate, out DateTime parsedDate))
                                    task.MaintenanceDate = parsedDate.ToString("yyyy-MM-dd HH:mm:ss");
                                else
                                    task.MaintenanceDate = "Invalid Date";
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error parsing date: {ex.Message}");
                                task.MaintenanceDate = "Invalid Date";
                            }
                        }
                        else
                        {
                            task.MaintenanceDate = "No Date";
                        }

                        // Get the associated user's email
                        int maintainerId = reader.GetInt32(reader.GetOrdinal("maintainer_id"));
                        task.MaintainerEmail = GetUserEmailById(maintainerId);

                        task.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        maintenanceTasks.Add(task);
                    }
                }
            }

            return maintenanceTasks;
        }

        // Schedule maintenance for a sensor manually
        public void ScheduleMaintenance(int sensorId, int maintainerId, DateTime maintenanceDate, string maintainerComments)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO dbo.Maintenance (sensor_id, maintainer_id, maintenance_date, maintainer_comments) " +
                               "VALUES (@SensorId, @MaintainerId, @MaintenanceDate, @MaintainerComments)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@SensorId", sensorId);
                command.Parameters.AddWithValue("@MaintainerId", maintainerId);
                command.Parameters.AddWithValue("@MaintenanceDate", maintenanceDate);
                command.Parameters.AddWithValue("@MaintainerComments", maintainerComments);

                command.ExecuteNonQuery();
            }
        }

        // Log a maintenance record (used from UI when reporting issue)
        public void InsertMaintenanceRecord(int sensorId, int maintainerId, DateTime maintenanceDate, string maintainerComments)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO dbo.Maintenance (sensor_id, maintainer_id, maintenance_date, maintainer_comments) " +
                               "VALUES (@SensorId, @MaintainerId, @MaintenanceDate, @MaintainerComments)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@SensorId", sensorId);
                command.Parameters.AddWithValue("@MaintainerId", maintainerId);
                command.Parameters.AddWithValue("@MaintenanceDate", maintenanceDate);
                command.Parameters.AddWithValue("@MaintenanceComments", maintainerComments);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting maintenance record: {ex.Message}");
                }
            }
        }
    }
}