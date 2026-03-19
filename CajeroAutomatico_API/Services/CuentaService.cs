using CajeroAutomaticoAPI.Data;
using CajeroAutomaticoAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CajeroAutomaticoAPI.Services
{
    public class CuentaService
    {
        private readonly DatabaseConnection _db;
        public CuentaService(DatabaseConnection db) => _db = db;

        public async Task<object> Crear(CrearCuentaRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_CrearCuenta", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdHabitante",  req.IdHabitante);
            cmd.Parameters.AddWithValue("@IdTipo",       req.IdTipo);
            cmd.Parameters.AddWithValue("@SaldoInicial", req.SaldoInicial);
            cmd.Parameters.AddWithValue("@NoCuenta",     req.NoCuenta);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new { IdCuenta = reader["IdCuenta"], Resultado = reader["Resultado"] };
            return new { Resultado = "Sin respuesta" };
        }

        public async Task<object?> Obtener(string? noCuenta, int? idCuenta)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerCuenta", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@NoCuenta", (object?)noCuenta ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdCuenta", (object?)idCuenta ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new
                {
                    IdCuenta        = reader["IdCuenta"],
                    NoCuenta        = reader["NoCuenta"].ToString(),
                    TipoCuenta      = reader["TipoCuenta"].ToString(),
                    Saldo           = reader["Saldo"],
                    FechaDeApertura = reader["FechaDeApertura"],
                    Titular         = reader["Titular"].ToString()
                };
            return null;
        }

        public async Task<List<object>> ListarPorHabiente(int idHabitante)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ListarCuentasPorHabiente", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdHabitante", idHabitante);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                lista.Add(new
                {
                    IdCuenta        = reader["IdCuenta"],
                    NoCuenta        = reader["NoCuenta"].ToString(),
                    TipoCuenta      = reader["TipoCuenta"].ToString(),
                    Saldo           = reader["Saldo"],
                    FechaDeApertura = reader["FechaDeApertura"]
                });
            return lista;
        }
    }
}
