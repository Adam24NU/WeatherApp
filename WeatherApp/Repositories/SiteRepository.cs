using Microsoft.Data.SqlClient;
using WeatherApp.Models;
using WeatherApp.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherApp.Repositories;

//// <summary>
//// Repository class for functionality for site data. 
//// </summary>
public class SiteRepository : ISiteRepository
{
    private readonly DatabaseConnection _dbConnection;

    public SiteRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Site>> GetSitesAsync()
    {
        var sites = new List<Site>();
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Sites", connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            sites.Add(new Site
            {
                SiteId = reader.GetInt32(0),
                Type = reader.GetString(1),
                Longitude = reader.GetDouble(2),
                Latitude = reader.GetDouble(3)
            });
        }

        return sites;
    }

    public async Task<Site> GetSiteByIdAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Sites WHERE site_id = @SiteId", connection);
        command.Parameters.AddWithValue("@SiteId", id);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Site
            {
                SiteId = reader.GetInt32(0),
                Type = reader.GetString(1),
                Longitude = reader.GetDouble(2),
                Latitude = reader.GetDouble(3)
            };
        }

        return null;
    }

    public async Task<Site> GetSiteByTypeAsync(string type)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM Sites WHERE type = @type", connection);
        command.Parameters.AddWithValue("@type", type);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Site
            {
                SiteId = reader.GetInt32(0),
                Type = reader.GetString(1),
                Longitude = reader.GetDouble(2),
                Latitude = reader.GetDouble(3)
            };
        }

        return null;
    }
}