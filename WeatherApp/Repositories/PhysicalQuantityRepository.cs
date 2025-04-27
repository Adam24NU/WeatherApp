using Microsoft.Data.SqlClient;
using System.Diagnostics;
using WeatherApp.Models;

namespace WeatherApp.Repositories
{
    /// <summary>
    /// Repository class for CRUD functionality for physical quantity data.
    /// </summary>
    public class PhysicalQuantityRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public PhysicalQuantityRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Get all physical quantities.
        /// </summary>
        public virtual async Task<List<PhysicalQuantity>> GetPhysicalQuantitiesAsync()
        {
            var quantities = new List<PhysicalQuantity>();
            using var connection = _dbConnection.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT * FROM PhysicalQuantities", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                quantities.Add(new PhysicalQuantity
                {
                    QuantityId = reader.GetInt32(reader.GetOrdinal("quantity_id")),
                    SensorId = reader.GetInt32(reader.GetOrdinal("sensor_id")),
                    QuantityName = reader.GetString(reader.GetOrdinal("quantity_name")),
                    Symbol = reader.GetString(reader.GetOrdinal("symbol")),
                    Unit = reader.GetString(reader.GetOrdinal("unit")),
                    UnitDescription = reader.GetString(reader.GetOrdinal("unit_description")),
                    MeasurementFrequency = reader.GetString(reader.GetOrdinal("measurement_frequency")),
                    SafeLevel = reader.IsDBNull(reader.GetOrdinal("safe_level")) ? null : reader.GetString(reader.GetOrdinal("safe_level")),
                    Reference = reader.IsDBNull(reader.GetOrdinal("reference")) ? null : reader.GetString(reader.GetOrdinal("reference")),
                    SensorUrl = reader.GetString(reader.GetOrdinal("sensor_url"))
                });
            }

            return quantities;
        }

        /// <summary>
        /// Get a physical quantity by its ID.
        /// </summary>
        public virtual async Task<PhysicalQuantity> GetPhysicalQuantityByIdAsync(int id)
        {
            using var connection = _dbConnection.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT * FROM PhysicalQuantities WHERE quantity_id = @QuantityId", connection);
            command.Parameters.AddWithValue("@QuantityId", id);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new PhysicalQuantity
                {
                    QuantityId = reader.GetInt32(reader.GetOrdinal("quantity_id")),
                    SensorId = reader.GetInt32(reader.GetOrdinal("sensor_id")),
                    QuantityName = reader.GetString(reader.GetOrdinal("quantity_name")),
                    Symbol = reader.GetString(reader.GetOrdinal("symbol")),
                    Unit = reader.GetString(reader.GetOrdinal("unit")),
                    UnitDescription = reader.GetString(reader.GetOrdinal("unit_description")),
                    MeasurementFrequency = reader.GetString(reader.GetOrdinal("measurement_frequency")),
                    SafeLevel = reader.IsDBNull(reader.GetOrdinal("safe_level")) ? null : reader.GetString(reader.GetOrdinal("safe_level")),
                    Reference = reader.IsDBNull(reader.GetOrdinal("reference")) ? null : reader.GetString(reader.GetOrdinal("reference")),
                    SensorUrl = reader.GetString(reader.GetOrdinal("sensor_url"))
                };
            }

            return null;
        }

        public virtual async Task<int?> GetQuantityIdBySymbolAsync(string symbol)
        {
            using var connection = _dbConnection.GetConnection();
            await connection.OpenAsync();

            string query = "SELECT quantity_id FROM PhysicalQuantities WHERE symbol = @Symbol";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Symbol", symbol);

            var result = await command.ExecuteScalarAsync();
            return result != null ? (int?)Convert.ToInt32(result) : null;
        }

        public virtual async Task<string?> GetSymbolByQuantityIdAsync(int quantityId)
        {

            using var connection = _dbConnection.GetConnection();
            await connection.OpenAsync();

            string query = "SELECT symbol FROM PhysicalQuantities WHERE quantity_id = @QId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@QId", quantityId);

            var result = await command.ExecuteScalarAsync();

            if (result != null)
            {
                return result.ToString();
            }
            else
            {
                Debug.WriteLine($"[WARN] No symbol found for Quantity ID: {quantityId}");
                return null;
            }
        }

        public virtual async Task<string?> GetUnitByQIdAsync(int Id)
        {
            string query = "SELECT Unit FROM PhysicalQuantities WHERE quantity_id = @QId";

            using var connection = _dbConnection.GetConnection();
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@QId", Id);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return result as string;
        }

    }
}
