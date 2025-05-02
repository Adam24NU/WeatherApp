using Microsoft.Data.SqlClient; 
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Repositories;

public class ConfigSettingRepository
{
    private readonly DatabaseConnection _dbConnection;

    public ConfigSettingRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<ConfigSetting>> GetConfigSettingsAsync()
    {
        var settings = new List<ConfigSetting>();
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM ConfigSettings", connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            settings.Add(new ConfigSetting
            {
                SettingId = reader.GetInt32(0),
                SensorId = reader.GetInt32(1),
                SettingName = reader.GetString(2),
                MinValue = reader.IsDBNull(3) ? null : reader.GetDouble(3),
                MaxValue = reader.IsDBNull(4) ? null : reader.GetDouble(4),
                CurrentValue = reader.IsDBNull(5) ? null : reader.GetDouble(5)
            });
        }

        return settings;
    }

    public async Task<ConfigSetting> GetConfigSettingByIdAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM ConfigSettings WHERE setting_id = @SettingId", connection);
        command.Parameters.AddWithValue("@SettingId", id);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new ConfigSetting
            {
                SettingId = reader.GetInt32(0),
                SensorId = reader.GetInt32(1),
                SettingName = reader.GetString(2),
                MinValue = reader.IsDBNull(3) ? null : reader.GetDouble(3),
                MaxValue = reader.IsDBNull(4) ? null : reader.GetDouble(4),
                CurrentValue = reader.IsDBNull(5) ? null : reader.GetDouble(5)
            };
        }

        return null;
    }

    public async Task<int> InsertConfigSettingAsync(ConfigSetting setting)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "INSERT INTO ConfigSettings (sensor_id, setting_name, min_value, max_value, current_value) " +
            "OUTPUT INSERTED.setting_id " +
            "VALUES (@SensorId, @SettingName, @MinValue, @MaxValue, @CurrentValue)", connection);
        command.Parameters.AddWithValue("@SensorId", setting.SensorId);
        command.Parameters.AddWithValue("@SettingName", setting.SettingName);
        command.Parameters.AddWithValue("@MinValue", (object)setting.MinValue ?? DBNull.Value);
        command.Parameters.AddWithValue("@MaxValue", (object)setting.MaxValue ?? DBNull.Value);
        command.Parameters.AddWithValue("@CurrentValue", (object)setting.CurrentValue ?? DBNull.Value);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task UpdateConfigSettingAsync(ConfigSetting setting)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "UPDATE ConfigSettings SET sensor_id = @SensorId, setting_name = @SettingName, " +
            "min_value = @MinValue, max_value = @MaxValue, current_value = @CurrentValue " +
            "WHERE setting_id = @SettingId", connection);
        command.Parameters.AddWithValue("@SettingId", setting.SettingId);
        command.Parameters.AddWithValue("@SensorId", setting.SensorId);
        command.Parameters.AddWithValue("@SettingName", setting.SettingName);
        command.Parameters.AddWithValue("@MinValue", (object)setting.MinValue ?? DBNull.Value);
        command.Parameters.AddWithValue("@MaxValue", (object)setting.MaxValue ?? DBNull.Value);
        command.Parameters.AddWithValue("@CurrentValue", (object)setting.CurrentValue ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteConfigSettingAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("DELETE FROM ConfigSettings WHERE setting_id = @SettingId", connection);
        command.Parameters.AddWithValue("@SettingId", id);
        await command.ExecuteNonQueryAsync();
    }
}