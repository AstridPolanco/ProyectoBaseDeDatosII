using CajeroAutomaticoAPI.Data;
using CajeroAutomaticoAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CajeroAutomaticoAPI.Services
{
    public class TarjetaService
    {
        private readonly DatabaseConnection _db;

        public TarjetaService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<object> CrearTarjeta(CrearTarjetaRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_CrearTarjeta", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdCuentaHabitante", req.IdCuentaHabitante);
            cmd.Parameters.AddWithValue("@IdCuenta", req.IdCuenta);
            cmd.Parameters.AddWithValue("@NoTarjeta", req.NoTarjeta);
            cmd.Parameters.AddWithValue("@FechaVencimiento", req.FechaVencimiento);
            cmd.Parameters.AddWithValue("@CVV", req.CVV);
            cmd.Parameters.AddWithValue("@PIN", req.PIN);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new
                {
                    IdTarjeta = reader["IdTarjeta"],
                    Resultado = reader["Resultado"]
                };

            return new { Resultado = "Sin respuesta del servidor" };
        }

        public async Task<object> ValidarPIN(ValidarPINRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_ValidarPIN", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@NoTarjeta",    req.NoTarjeta);
            cmd.Parameters.AddWithValue("@PINIngresado", req.PINIngresado);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new
                {
                    Valido  = Convert.ToInt32(reader["Valido"]),
                    Mensaje = reader["Mensaje"].ToString()
                };

            return new { Valido = 0, Mensaje = "Sin respuesta del servidor" };
        }

        public async Task<object?> ObtenerTarjeta(ObtenerTarjetaRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_ObtenerTarjeta", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdTarjeta", (object?)req.IdTarjeta ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NoTarjeta", (object?)req.NoTarjeta ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new TarjetaResponse
                {
                    IdTarjeta = Convert.ToInt32(reader["IdTarjeta"]),
                    NoTarjeta = reader["NoTarjeta"].ToString()!,
                    FechaVencimiento = Convert.ToDateTime(reader["FechaVencimiento"]),
                    CVV = reader["CVV"].ToString()!,
                    Activa = Convert.ToBoolean(reader["Activa"]),
                    NoCuenta = reader["NoCuenta"].ToString()!,
                    Saldo = Convert.ToDecimal(reader["Saldo"]),
                    Titular = reader["Titular"].ToString()!
                };

            return null;
        }

        public async Task<List<TarjetaResponse>> ListarTarjetasPorCuenta(int idCuenta)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_ListarTarjetasPorCuenta", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdCuenta", idCuenta);

            var lista = new List<TarjetaResponse>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new TarjetaResponse
                {
                    IdTarjeta = Convert.ToInt32(reader["IdTarjeta"]),
                    NoTarjeta = reader["NoTarjeta"].ToString()!,
                    FechaVencimiento = Convert.ToDateTime(reader["FechaVencimiento"]),
                    Activa = Convert.ToBoolean(reader["Activa"]),
                    Saldo = Convert.ToDecimal(reader["Saldo"]),           
                    NoCuenta = reader["NoCuenta"].ToString()!,            
                    Titular = reader["Titular"].ToString()!   
                });
            }

            return lista;
        }
    }
}
