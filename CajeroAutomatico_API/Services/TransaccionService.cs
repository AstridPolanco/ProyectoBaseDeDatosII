using CajeroAutomaticoAPI.Data;
using CajeroAutomaticoAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CajeroAutomaticoAPI.Services
{
    public class TransaccionService
    {
        private readonly DatabaseConnection _db;

        public TransaccionService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<TransaccionResponse> Deposito(DepositoRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_Deposito", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdCuenta", req.IdCuenta);
            cmd.Parameters.AddWithValue("@Monto", req.Monto);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)req.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Usuario", (object?)req.Usuario ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new TransaccionResponse
                {
                    IdMovimiento  = Convert.ToInt32(reader["IdMovimiento"]),
                    SaldoAnterior = Convert.ToDecimal(reader["SaldoAnterior"]),
                    SaldoNuevo = Convert.ToDecimal(reader["SaldoNuevo"]),
                    Resultado = reader["Resultado"].ToString()!
                };

            throw new Exception("El SP no devolvió resultado.");
        }

        public async Task<TransaccionResponse> NotaCredito(NotaCreditoRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_NotaCredito", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdCuenta", req.IdCuenta);
            cmd.Parameters.AddWithValue("@Monto", req.Monto);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)req.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Usuario", (object?)req.Usuario     ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new TransaccionResponse
                {
                    IdMovimiento = Convert.ToInt32(reader["IdMovimiento"]),
                    SaldoAnterior = Convert.ToDecimal(reader["SaldoAnterior"]),
                    SaldoNuevo = Convert.ToDecimal(reader["SaldoNuevo"]),
                    Resultado = reader["Resultado"].ToString()!
                };

            throw new Exception("El SP no devolvió resultado.");
        }
    }
}
