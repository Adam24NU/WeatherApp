using Microsoft.Data.SqlClient;

namespace WeatherApp.Repositories;

public class DatabaseConnection : IDisposable
{
    private readonly string _connectionString;
    private bool _disposed = false;

    public DatabaseConnection()
    {
        _connectionString = "Server=10.0.2.2,1433,1433;Database=WeatherApp;User Id=WeatherApp;Password=W3ath3rApp;TrustServerCertificate=True;";
    }

    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task OpenAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
    }

    public SqlCommand CreateCommand(string query)
    {
        return new SqlCommand(query, new SqlConnection(_connectionString));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        _disposed = true;
    }

    ~DatabaseConnection()
    {
        Dispose(false);
    }
}