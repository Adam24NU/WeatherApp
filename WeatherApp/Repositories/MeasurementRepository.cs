using Microsoft.Data.SqlClient;
using WeatherApp.Models;

namespace WeatherApp.Repositories;

/// <summary>
/// Repository class for CRUD functionality for measurement data.
/// </summary>
public class MeasurementRepository
{
    private readonly DatabaseConnection _dbConnection;

    public MeasurementRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public virtual async Task<List<Measurement>> GetMeasurementsAsync()
    {
        var measurements = new List<Measurement>();

        using var conn = _dbConnection.GetConnection();
        await conn.OpenAsync();

        var query = "SELECT measurement_id, quantity_id, date, time, value FROM Measurements";
        using var cmd = new SqlCommand(query, conn);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            measurements.Add(new Measurement
            {
                MeasurementId = reader.GetInt32(0),
                QuantityId = reader.GetInt32(1),
                Date = reader.GetString(2),
                Time = reader.GetString(3),
                Value = reader.GetDouble(4)
            });
        }

        return measurements;
    }

    public virtual async Task<Measurement> GetMeasurementByIdAsync(int id)
    {
        using var conn = _dbConnection.GetConnection();
        await conn.OpenAsync();

        var query = "SELECT measurement_id, quantity_id, date, time, value FROM Measurements WHERE measurement_id = @id";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Measurement
            {
                MeasurementId = reader.GetInt32(0),
                QuantityId = reader.GetInt32(1),
                Date = reader.GetString(2),
                Time = reader.GetString(3),
                Value = reader.GetDouble(4)
            };
        }

        return null!;
    }

    public virtual async Task<List<Measurement>> GetMeasurementsByQIdAsync(int quantityId)
    {
        var measurements = new List<Measurement>();

        using var conn = _dbConnection.GetConnection();
        await conn.OpenAsync();

        var query = "SELECT measurement_id, quantity_id, date, time, value FROM Measurements WHERE quantity_id = @qid";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@qid", quantityId);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            measurements.Add(new Measurement
            {
                MeasurementId = reader.GetInt32(0),
                QuantityId = reader.GetInt32(1),
                Date = reader.GetString(2),
                Time = reader.GetString(3),
                Value = reader.GetDouble(4)
            });
        }

        return measurements; // Ensure a return statement is present in all code paths
    }
}
