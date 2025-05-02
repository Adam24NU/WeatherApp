using Microsoft.Data.SqlClient;
using System.Diagnostics;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Repositories;

//// <summary>
//// Repository class for CRUD functionality for sensor data. 
//// </summary>
public class SensorRepository
{
    private readonly DatabaseConnection _dbConnection;

    public SensorRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Sensor>> GetSensorsAsync()
    {
        var sensors = new List<Sensor>();
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Sensors", connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            sensors.Add(new Sensor
            {
                SensorId = reader.GetInt32(reader.GetOrdinal("sensor_id")),
                SiteId = reader.GetInt32(reader.GetOrdinal("site_id")),
                SensorType = reader.GetString(reader.GetOrdinal("sensor_type")),
                Name = reader.GetString(reader.GetOrdinal("name"))
            });
        }

        return sensors;
    }

    public async Task<Sensor> GetSensorByIdAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Sensors WHERE sensor_id = @SensorId", connection);
        command.Parameters.AddWithValue("@SensorId", id);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Sensor
            {
                SensorId = reader.GetInt32(reader.GetOrdinal("sensor_id")),
                SiteId = reader.GetInt32(reader.GetOrdinal("site_id")),
                SensorType = reader.GetString(reader.GetOrdinal("sensor_type")),
                Name = reader.GetString(reader.GetOrdinal("name"))
            };
        }

        return null;
    }

    // Get all sensor IDs for a specific site
    public async Task<List<int>> GetSensorIDsBySiteIdAsync(int siteId)
    {
        var sensorIDs = new List<int>();

        // Debug line to show the siteId being used
        Debug.WriteLine($"[GetSensorIDsBySiteIdAsync] Fetching sensor IDs for site ID: {siteId}");

        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();
        Debug.WriteLine("[GetSensorIDsBySiteIdAsync] Connection opened successfully");

        const string query = "SELECT sensor_id FROM Sensors WHERE site_id = @SiteId";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@SiteId", siteId);

        // Debug line to show the query being executed
        Debug.WriteLine($"[GetSensorIDsBySiteIdAsync] Executing query: {query} with siteId: {siteId}");

        using var reader = await command.ExecuteReaderAsync();

        // Check if any records are returned
        if (!reader.HasRows)
        {
            Debug.WriteLine($"[GetSensorIDsBySiteIdAsync] No records found for site ID: {siteId}");
        }

        while (await reader.ReadAsync())
        {
            int sensorId = reader.GetInt32(reader.GetOrdinal("sensor_id"));
            sensorIDs.Add(sensorId);

            // Debug line to show each sensorId being added to the list
            Debug.WriteLine($"[GetSensorIDsBySiteIdAsync] Found sensor ID: {sensorId}");
        }

        // Debug line to show the final list of sensor IDs
        Debug.WriteLine($"[GetSensorIDsBySiteIdAsync] Total sensor IDs found: {sensorIDs.Count}");

        return sensorIDs;
    }


}