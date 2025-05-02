using Microsoft.Data.SqlClient;
using WeatherApp.Core.Models;
using WeatherApp.Core.ViewModels;
using Dapper;
using System.Diagnostics;

namespace WeatherApp.Core.Repositories;

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

    // Gets measurement values for a given quantity id
    public async Task<Measurement> GetMeasurementByQuantityIdAsync(int quantityId)
    {
        // Debug line to show the quantityId being used for the query
        Debug.WriteLine($"[GetMeasurementByQuantityIdAsync] Fetching measurement for quantity ID: {quantityId}");

        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        string query = "SELECT TOP 1 * FROM Measurements WHERE quantity_id = @QuantityId ORDER BY measurement_id DESC";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@QuantityId", quantityId);

        // Debug line to show the query being executed
        Debug.WriteLine($"[GetMeasurementByQuantityIdAsync] Executing query: {query} with quantityId: {quantityId}");

        using var reader = await command.ExecuteReaderAsync();

        // Check if any records are returned
        if (!reader.HasRows)
        {
            Debug.WriteLine($"[GetMeasurementByQuantityIdAsync] No records found for quantity ID: {quantityId}");
            return null;
        }

        if (await reader.ReadAsync())
        {
            var measurement = new Measurement
            {
                Value = reader.GetDouble(reader.GetOrdinal("value")),
                Date = reader.GetString(reader.GetOrdinal("date")),
                Time = reader.GetString(reader.GetOrdinal("time"))
            };

            // Debug line to show the measurement data being returned
            Debug.WriteLine($"[GetMeasurementByQuantityIdAsync] Found measurement: Value={measurement.Value}, Date={measurement.Date}, Time={measurement.Time}");

            return measurement;
        }

        return null;
    }
}
