using Microsoft.Data.SqlClient;
using WeatherApp.Models;

namespace WeatherApp.Repositories;

public class MaintenanceRepository
{
    private readonly DatabaseConnection _dbConnection;

    public MaintenanceRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Maintenance>> GetMaintenancesAsync()
    {
        var maintenances = new List<Maintenance>();
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Maintenance", connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            maintenances.Add(new Maintenance
            {
                MaintenanceId = reader.GetInt32(0),
                SensorId = reader.GetInt32(1),
                MaintainerId = reader.GetInt32(2),
                MaintenanceDate = reader.GetString(3),
                MaintainerComments = reader.IsDBNull(4) ? null : reader.GetString(4)
            });
        }

        return maintenances;
    }

    public async Task<Maintenance> GetMaintenanceByIdAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Maintenance WHERE maintenance_id = @MaintenanceId", connection);
        command.Parameters.AddWithValue("@MaintenanceId", id);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Maintenance
            {
                MaintenanceId = reader.GetInt32(0),
                SensorId = reader.GetInt32(1),
                MaintainerId = reader.GetInt32(2),
                MaintenanceDate = reader.GetString(3),
                MaintainerComments = reader.IsDBNull(4) ? null : reader.GetString(4)
            };
        }

        return null;
    }

    public async Task<int> InsertMaintenanceAsync(Maintenance maintenance)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "INSERT INTO Maintenance (sensor_id, maintainer_id, maintenance_date, maintainer_comments) " +
            "OUTPUT INSERTED.maintenance_id " +
            "VALUES (@SensorId, @MaintainerId, @MaintenanceDate, @MaintainerComments)", connection);
        command.Parameters.AddWithValue("@SensorId", maintenance.SensorId);
        command.Parameters.AddWithValue("@MaintainerId", maintenance.MaintainerId);
        command.Parameters.AddWithValue("@MaintenanceDate", maintenance.MaintenanceDate);
        command.Parameters.AddWithValue("@MaintainerComments", (object)maintenance.MaintainerComments ?? DBNull.Value);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task UpdateMaintenanceAsync(Maintenance maintenance)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "UPDATE Maintenance SET sensor_id = @SensorId, maintainer_id = @MaintainerId, " +
            "maintenance_date = @MaintenanceDate, maintainer_comments = @MaintainerComments " +
            "WHERE maintenance_id = @MaintenanceId", connection);
        command.Parameters.AddWithValue("@MaintenanceId", maintenance.MaintenanceId);
        command.Parameters.AddWithValue("@SensorId", maintenance.SensorId);
        command.Parameters.AddWithValue("@MaintainerId", maintenance.MaintainerId);
        command.Parameters.AddWithValue("@MaintenanceDate", maintenance.MaintenanceDate);
        command.Parameters.AddWithValue("@MaintainerComments", (object)maintenance.MaintainerComments ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteMaintenanceAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("DELETE FROM Maintenance WHERE maintenance_id = @MaintenanceId", connection);
        command.Parameters.AddWithValue("@MaintenanceId", id);
        await command.ExecuteNonQueryAsync();
    }
}