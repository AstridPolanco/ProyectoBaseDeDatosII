using CajeroAutomaticoAPI.Data;
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

            cmd.Parameters.AddWithValue("@IdCuenta", idCuenta);
            cmd.Parameters.AddWithValue("@FechaInicio", (object?)fi?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaFin", (object?)ff?.Date ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                lista.Add(new
                {
                    IdBitacora = reader["IdBitacora"],
                    FechaBitacora = reader["FechaBitacora"],
                    SaldoAnterior = reader["SaldoAnterior"],
                    SaldoNuevo = reader["SaldoNuevo"],
                    Diferencia = reader["Diferencia"],
                    TipoMovimiento = reader["TipoMovimiento"].ToString(),
                    FechaMovimiento = reader["FechaMovimiento"],
                    Monto = reader["Monto"],
                    DetalleMovimiento = reader["DetalleMovimiento"].ToString(),
                    Estado = reader["Estado"].ToString()
                });
            return lista;
        }

        public async Task<List<object>> ObtenerMovimientos(int? idCuenta, DateTime? fi, DateTime? ff, int? tipo)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerMovimientos", conn)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdCuenta", (object?)idCuenta ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaInicio", (object?)fi?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaFin", (object?)ff?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdTipoMovimiento", (object?)tipo ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                lista.Add(new
                {
                    IdMovimiento = reader["IdMovimiento"],
                    TipoMovimiento = reader["TipoMovimiento"].ToString(),
                    FechaMovimiento = reader["FechaMovimiento"],
                    Monto = reader["Monto"],
                    Detalle = reader["Detalle"].ToString(),
                    Estado = reader["Estado"].ToString(),
                    MotivoFallo = reader["MotivoFallo"] == DBNull.Value ? null : reader["MotivoFallo"].ToString(),
                    NoCuenta = reader["NoCuenta"].ToString(),
                    Cuentahabiente = reader["Cuentahabiente"].ToString()
                });
            return lista;
        }

        public async Task<List<object>> ObtenerErrores(int? idCuenta, DateTime? fi, DateTime? ff)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerErrores", conn)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdCuenta", (object?)idCuenta ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaInicio", (object?)fi?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaFin", (object?)ff?.Date ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                lista.Add(new
                {
                    IdMovimiento = reader["IdMovimiento"],
                    TipoMovimiento = reader["TipoMovimiento"].ToString(),
                    FechaMovimiento = reader["FechaMovimiento"],
                    Monto = reader["Monto"],
                    Detalle = reader["Detalle"].ToString(),
                    MotivoFallo = reader["MotivoFallo"].ToString(),
                    NoCuenta = reader["NoCuenta"].ToString(),
                    Cuentahabiente = reader["Cuentahabiente"].ToString(),
                    Estado = "ERROR"
                });
            return lista;
        }
    }
}