using Microsoft.Data.SqlClient;
using WeatherApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherApp.Repositories;

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

    public async Task<Sensor> GetSensorBySiteIdAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Sensors WHERE site_id = @SiteId", connection);
        command.Parameters.AddWithValue("@SiteID", id);
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

    public async Task<int> InsertSensorAsync(Sensor sensor)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "INSERT INTO Sensors (site_id, sensor_type, name) " +
            "OUTPUT INSERTED.sensor_id " +
            "VALUES (@SiteId, @SensorType, @Name)", connection);
        command.Parameters.AddWithValue("@SiteId", sensor.SiteId);
        command.Parameters.AddWithValue("@SensorType", sensor.SensorType);
        command.Parameters.AddWithValue("@Name", sensor.Name);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task UpdateSensorAsync(Sensor sensor)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "UPDATE Sensors SET site_id = @SiteId, sensor_type = @SensorType, name = @Name " +
            "WHERE sensor_id = @SensorId", connection);
        command.Parameters.AddWithValue("@SensorId", sensor.SensorId);
        command.Parameters.AddWithValue("@SiteId", sensor.SiteId);
        command.Parameters.AddWithValue("@SensorType", sensor.SensorType);
        command.Parameters.AddWithValue("@Name", sensor.Name);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteSensorAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("DELETE FROM Sensors WHERE sensor_id = @SensorId", connection);
        command.Parameters.AddWithValue("@SensorId", id);
        await command.ExecuteNonQueryAsync();
    }
}