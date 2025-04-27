using Microsoft.Data.SqlClient;
using WeatherApp.Models;

namespace WeatherApp.Repositories;

public class IncidentMeasurementRepository
{
    private readonly DatabaseConnection _dbConnection;

    public IncidentMeasurementRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<IncidentMeasurement>> GetIncidentMeasurementsAsync()
    {
        var incidentMeasurements = new List<IncidentMeasurement>();
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM IncidentMeasurements", connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            incidentMeasurements.Add(new IncidentMeasurement
            {
                IncidentId = reader.GetInt32(0),
                MeasurementId = reader.GetInt32(1)
            });
        }

        return incidentMeasurements;
    }

    public async Task<List<IncidentMeasurement>> GetIncidentMeasurementsByIncidentIdAsync(int incidentId)
    {
        var incidentMeasurements = new List<IncidentMeasurement>();
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM IncidentMeasurements WHERE incident_id = @IncidentId", connection);
        command.Parameters.AddWithValue("@IncidentId", incidentId);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            incidentMeasurements.Add(new IncidentMeasurement
            {
                IncidentId = reader.GetInt32(0),
                MeasurementId = reader.GetInt32(1)
            });
        }

        return incidentMeasurements;
    }

    public async Task<int> InsertIncidentMeasurementAsync(IncidentMeasurement im)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "INSERT INTO IncidentMeasurements (incident_id, measurement_id) " +
            "VALUES (@IncidentId, @MeasurementId)", connection);
        command.Parameters.AddWithValue("@IncidentId", im.IncidentId);
        command.Parameters.AddWithValue("@MeasurementId", im.MeasurementId);

        await command.ExecuteNonQueryAsync();
        return im.MeasurementId; // Return MeasurementId as a simple identifier
    }

    public async Task DeleteIncidentMeasurementAsync(int incidentId, int measurementId)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "DELETE FROM IncidentMeasurements WHERE incident_id = @IncidentId AND measurement_id = @MeasurementId", connection);
        command.Parameters.AddWithValue("@IncidentId", incidentId);
        command.Parameters.AddWithValue("@MeasurementId", measurementId);

        await command.ExecuteNonQueryAsync();
    }
}