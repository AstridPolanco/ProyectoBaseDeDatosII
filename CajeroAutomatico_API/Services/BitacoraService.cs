using CajeroAutomaticoAPI.Data;
using CajeroAutomaticoAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CajeroAutomaticoAPI.Services
{
    public class BitacoraService
    {
        private readonly DatabaseConnection _db;
        public BitacoraService(DatabaseConnection db) => _db = db;

        public async Task<List<object>> ObtenerBitacora(int idCuenta, DateTime? fi, DateTime? ff)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerBitacora", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdCuenta",    idCuenta);
            cmd.Parameters.AddWithValue("@FechaInicio", (object?)fi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaFin",    (object?)ff ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                lista.Add(new
                {
                    IdBitacora        = reader["IdBitacora"],
                    FechaBitacora     = reader["FechaBitacora"],
                    SaldoAnterior     = reader["SaldoAnterior"],
                    SaldoNuevo        = reader["SaldoNuevo"],
                    Diferencia        = reader["Diferencia"],
                    TipoMovimiento    = reader["TipoMovimiento"].ToString(),
                    FechaMovimiento   = reader["FechaMovimiento"],
                    Monto             = reader["Monto"],
                    DetalleMovimiento = reader["DetalleMovimiento"].ToString(),
                    Estado            = reader["Estado"].ToString()
                });
            return lista;
        }
    }
}
