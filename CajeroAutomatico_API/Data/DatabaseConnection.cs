using Microsoft.Data.SqlClient;

namespace CajeroAutomaticoAPI.Data
{
    public class DatabaseConnection
    {
        private readonly IConfiguration _config;

        public DatabaseConnection(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("CajeroDb"));
        }
    }
}