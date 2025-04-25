using Microsoft.Data.SqlClient;
using WeatherApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherApp.Repositories;

public class IncidentRepository
{
    private readonly DatabaseConnection _dbConnection;

    public IncidentRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Incident>> GetIncidentsAsync()
    {
        var incidents = new List<Incident>();
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Incidents", connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            incidents.Add(new Incident
            {
                IncidentId = reader.GetInt32(reader.GetOrdinal("incident_id")),
                ResponderId = reader.GetInt32(reader.GetOrdinal("responder_id")),
                Priority = reader.IsDBNull(reader.GetOrdinal("priority")) ? null : reader.GetString(reader.GetOrdinal("priority")),
                ResponderComments = reader.IsDBNull(reader.GetOrdinal("responder_comments")) ? null : reader.GetString(reader.GetOrdinal("responder_comments")),
                ResolvedDate = reader.IsDBNull(reader.GetOrdinal("resolved_date")) ? null : reader.GetString(reader.GetOrdinal("resolved_date"))
            });
        }

        return incidents;
    }

    public async Task<Incident> GetIncidentByIdAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Incidents WHERE incident_id = @IncidentId", connection);
        command.Parameters.AddWithValue("@IncidentId", id);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Incident
            {
                IncidentId = reader.GetInt32(reader.GetOrdinal("incident_id")),
                ResponderId = reader.GetInt32(reader.GetOrdinal("responder_id")),
                Priority = reader.IsDBNull(reader.GetOrdinal("priority")) ? null : reader.GetString(reader.GetOrdinal("priority")),
                ResponderComments = reader.IsDBNull(reader.GetOrdinal("responder_comments")) ? null : reader.GetString(reader.GetOrdinal("responder_comments")),
                ResolvedDate = reader.IsDBNull(reader.GetOrdinal("resolved_date")) ? null : reader.GetString(reader.GetOrdinal("resolved_date"))
            };
        }

        return null;
    }

    public async Task<int> InsertIncidentAsync(Incident incident)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "INSERT INTO Incidents (responder_id, priority, responder_comments, resolved_date) " +
            "OUTPUT INSERTED.incident_id " +
            "VALUES (@ResponderId, @Priority, @ResponderComments, @ResolvedDate)", connection);
        command.Parameters.AddWithValue("@ResponderId", incident.ResponderId);
        command.Parameters.AddWithValue("@Priority", (object?)incident.Priority ?? DBNull.Value);
        command.Parameters.AddWithValue("@ResponderComments", (object?)incident.ResponderComments ?? DBNull.Value);
        command.Parameters.AddWithValue("@ResolvedDate", (object?)incident.ResolvedDate ?? DBNull.Value);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task UpdateIncidentAsync(Incident incident)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "UPDATE Incidents SET responder_id = @ResponderId, priority = @Priority, " +
            "responder_comments = @ResponderComments, resolved_date = @ResolvedDate " +
            "WHERE incident_id = @IncidentId", connection);
        command.Parameters.AddWithValue("@IncidentId", incident.IncidentId);
        command.Parameters.AddWithValue("@ResponderId", incident.ResponderId);
        command.Parameters.AddWithValue("@Priority", (object?)incident.Priority ?? DBNull.Value);
        command.Parameters.AddWithValue("@ResponderComments", (object?)incident.ResponderComments ?? DBNull.Value);
        command.Parameters.AddWithValue("@ResolvedDate", (object?)incident.ResolvedDate ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteIncidentAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("DELETE FROM Incidents WHERE incident_id = @IncidentId", connection);
        command.Parameters.AddWithValue("@IncidentId", id);
        await command.ExecuteNonQueryAsync();
    }
}