using Microsoft.Data.SqlClient;
using WeatherApp.Models;


namespace WeatherApp
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        // GetConnection function to create and return a SqlConnection
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // Get all users from the database
        // Method to get all users from the database
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

        // Register a new user
        public bool RegisterUser(string email, string password, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO dbo.Users (email, password, role) VALUES (@Email, @Password, @Role)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password); // Storing the password as plain text
                command.Parameters.AddWithValue("@Role", role);

                try
                {
                    command.ExecuteNonQuery();
                    return true; // Registration successful
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during registration: {ex.Message}");
                    return false; // Return false in case of error (e.g., duplicate email)
                }
            }
        }

        // Authenticate a user (login)
        public User AuthenticateUser(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM dbo.Users WHERE email = @email AND password = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password); // No hashing as per your requirements

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var user = new User
                        {
                            UserId = (int)reader["user_id"],
                            Email = (string)reader["email"],
                            Password = (string)reader["password"],
                            Role = (string)reader["role"]
                        };
                        return user; // Return authenticated user
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null; // Return null if no matching user is found
        }

        // Delete a user by user_id
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
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // Flag a sensor as malfunctioning
        public void FlagSensorAsMalfunctioning(int sensorId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT s.sensor_id, s.site_id, s.sensor_type, pq.measurement_frequency " +
                               "FROM dbo.Sensors s " +
                               "JOIN dbo.PhysicalQuantities pq ON s.sensor_id = pq.sensor_id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SensorId", sensorId);
                command.ExecuteNonQuery();
            }
        }

        // Get sensor data from the database
        public List<SensorMeta> GetSensorsData()
        {
            List<SensorMeta> sensors = new List<SensorMeta>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT s.sensor_id, s.sensor_type, p.measurement_frequency, p.safe_level " +
                               "FROM dbo.Sensors s " +
                               "LEFT JOIN dbo.PhysicalQuantities p ON s.sensor_id = p.sensor_id";  // Removed the JOIN with Maintenance
        
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sensor = new SensorMeta
                        {
                            SensorId = reader.GetInt32(reader.GetOrdinal("sensor_id")),
                            SensorType = reader.GetString(reader.GetOrdinal("sensor_type")),
                            Status = "Active",  // Assuming sensor status is always active
                            IsFlagged = false // Default value
                        };

                        // Handle MeasurementFrequency (check for NULL)
                        if (!reader.IsDBNull(reader.GetOrdinal("measurement_frequency")))
                        {
                            sensor.MeasurementFrequency = reader.GetString(reader.GetOrdinal("measurement_frequency"));
                        }
                        else
                        {
                            sensor.MeasurementFrequency = "Not Available"; // Default value
                        }

                        // Handle SafeLevel (check for NULL)
                        if (!reader.IsDBNull(reader.GetOrdinal("safe_level")))
                        {
                            sensor.SafeLevel = reader.GetString(reader.GetOrdinal("safe_level"));
                        }
                        else
                        {
                            sensor.SafeLevel = "Unknown"; // Default value
                        }

                        sensors.Add(sensor);
                    }
                }
            }

            return sensors;
        }



// Helper method to fetch email by maintainer_id
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
                    MaintainerComments = reader.IsDBNull(reader.GetOrdinal("maintainer_comments")) ? "No comments" : reader.GetString(reader.GetOrdinal("maintainer_comments")),
                    Status = "Scheduled",  // or "Completed" depending on your logic, for now we can default to "Scheduled"
                };

                // Handle Maintenance Date
                if (!reader.IsDBNull(reader.GetOrdinal("maintenance_date")))
                {
                    // Use try-catch to handle invalid DateTime parsing
                    try
                    {
                        string rawDate = reader.GetValue(reader.GetOrdinal("maintenance_date")).ToString();

                        if (DateTime.TryParse(rawDate, out DateTime parsedDate))
                        {
                            task.MaintenanceDate = parsedDate.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to parse maintenance_date: {rawDate}");
                            task.MaintenanceDate = "Invalid Date";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error or handle it, e.g., set a default value
                        Console.WriteLine($"Error parsing maintenance_date: {ex.Message}");
                        task.MaintenanceDate = "Invalid Date";
                    }
                }
                else
                {
                    task.MaintenanceDate = "No Date";  // Default value if NULL
                }

                // Fetch the maintainer's email using the maintainer_id
                int maintainerId = reader.GetInt32(reader.GetOrdinal("maintainer_id"));
                task.MaintainerEmail = GetUserEmailById(maintainerId);

                // Create a timestamp (you can create this based on any logic, like using the current time)
                task.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                maintenanceTasks.Add(task);
            }
        }
    }

    return maintenanceTasks;
}







        // Schedule maintenance for a sensor
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
        // Method to insert maintenance record into the Maintenance table
        public void InsertMaintenanceRecord(int sensorId, int maintainerId, DateTime maintenanceDate, string maintainerComments)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
        
                string query = "INSERT INTO dbo.Maintenance (sensor_id, maintainer_id, maintenance_date, maintainer_comments) " +
                               "VALUES (@SensorId, @MaintainerId, @MaintenanceDate, @MaintainerComments)";
                SqlCommand command = new SqlCommand(query, connection);

                // Adding parameters to the query
                command.Parameters.AddWithValue("@SensorId", sensorId);
                command.Parameters.AddWithValue("@MaintainerId", maintainerId);
                command.Parameters.AddWithValue("@MaintenanceDate", maintenanceDate);  
                command.Parameters.AddWithValue("@MaintainerComments", maintainerComments);

                try
                {
                    // Execute the query to insert the maintenance record
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
