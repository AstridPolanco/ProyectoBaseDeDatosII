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

        public async Task<TransaccionResponse> PagarCheque(PagarChequeRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_PagarCheque", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdCuenta",     req.IdCuenta);
            cmd.Parameters.AddWithValue("@Monto",        req.Monto);
            cmd.Parameters.AddWithValue("@FechaCheque",  req.FechaCheque);
            cmd.Parameters.AddWithValue("@NumeroCheque", req.NumeroCheque);
            cmd.Parameters.AddWithValue("@Usuario",      req.Usuario);

            var outParam = new SqlParameter("@IdMovimiento", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new TransaccionResponse
                {
                    IdMovimiento  = Convert.ToInt32(reader["IdMovimiento"]),
                    SaldoAnterior = Convert.ToDecimal(reader["SaldoAnterior"]),
                    SaldoNuevo    = Convert.ToDecimal(reader["SaldoNuevo"]),
                    Resultado     = reader["Resultado"].ToString()!
                };
            throw new Exception("El SP no devolvió resultado.");
        }

        public async Task<TransaccionResponse> NotaDebito(NotaDebitoRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_NotaDebito", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdCuenta",  req.IdCuenta);
            cmd.Parameters.AddWithValue("@IdTarjeta", req.IdTarjeta);
            cmd.Parameters.AddWithValue("@Monto",     req.Monto);
            cmd.Parameters.AddWithValue("@Usuario",   req.Usuario);

            var outParam = new SqlParameter("@IdMovimiento", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new TransaccionResponse
                {
                    IdMovimiento  = Convert.ToInt32(reader["IdMovimiento"]),
                    SaldoAnterior = Convert.ToDecimal(reader["SaldoAnterior"]),
                    SaldoNuevo    = Convert.ToDecimal(reader["SaldoNuevo"]),
                    Resultado     = reader["Resultado"].ToString()!
                };
            throw new Exception("El SP no devolvió resultado.");
        }

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

        public async Task<List<object>> ObtenerMovimientos(int? idCuenta, DateTime? fi, DateTime? ff, int? tipo)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerMovimientos", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdCuenta",         (object?)idCuenta ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaInicio",      (object?)fi       ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaFin",         (object?)ff       ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdTipoMovimiento", (object?)tipo     ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                lista.Add(new
                {
                    IdMovimiento    = reader["IdMovimiento"],
                    TipoMovimiento  = reader["TipoMovimiento"].ToString(),
                    FechaMovimiento = reader["FechaMovimiento"],
                    Monto           = reader["Monto"],
                    Detalle         = reader["Detalle"].ToString(),
                    Estado          = reader["Estado"].ToString(),
                    MotivoFallo     = reader["MotivoFallo"] == DBNull.Value ? null : reader["MotivoFallo"].ToString(),
                    NoCuenta        = reader["NoCuenta"].ToString(),
                    Cuentahabiente  = reader["Cuentahabiente"].ToString()
                });
            return lista;
        }

        public async Task<List<object>> ObtenerErrores(int? idCuenta, DateTime? fi, DateTime? ff)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerErrores", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdCuenta",    (object?)idCuenta ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaInicio", (object?)fi       ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaFin",    (object?)ff       ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                lista.Add(new
                {
                    IdMovimiento   = reader["IdMovimiento"],
                    TipoMovimiento = reader["TipoMovimiento"].ToString(),
                    FechaMovimiento= reader["FechaMovimiento"],
                    Monto          = reader["Monto"],
                    Detalle        = reader["Detalle"].ToString(),
                    MotivoFallo    = reader["MotivoFallo"].ToString(),
                    NoCuenta       = reader["NoCuenta"].ToString(),
                    Cuentahabiente = reader["Cuentahabiente"].ToString(),
                    Estado         = "ERROR"
                });
            return lista;
        }
    }
}
