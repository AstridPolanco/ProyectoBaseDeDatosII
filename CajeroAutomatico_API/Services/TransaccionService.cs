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

        public async Task<object> PagarCheque(PagarChequeRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_PagarCheque", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdCuenta", req.IdCuenta);
            cmd.Parameters.AddWithValue("@Monto", req.Monto);
            cmd.Parameters.AddWithValue("@FechaCheque", req.FechaCheque);
            cmd.Parameters.AddWithValue("@NumeroCheque", req.NumeroCheque);
            cmd.Parameters.AddWithValue("@Usuario", req.Usuario);

            var outParam = new SqlParameter("@IdMovimiento", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return new
                {
                    Success = true,
                    Message = "Cheque pagado exitosamente",
                    IdMovimiento = outParam.Value != DBNull.Value ? (int)outParam.Value : (int?)null
                };
            }
            catch (SqlException ex)
            {
                return new
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // SP 16: Nota Débito
        public async Task<object> NotaDebito(NotaDebitoRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_NotaDebito", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdCuenta", req.IdCuenta);
            cmd.Parameters.AddWithValue("@IdTarjeta", req.IdTarjeta);
            cmd.Parameters.AddWithValue("@Monto", req.Monto);
            cmd.Parameters.AddWithValue("@Usuario", req.Usuario);

            var outParam = new SqlParameter("@IdMovimiento", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return new
                {
                    Success = true,
                    Message = "Nota débito realizada exitosamente",
                    IdMovimiento = outParam.Value != DBNull.Value ? (int)outParam.Value : (int?)null
                };
            }
            catch (SqlException ex)
            {
                return new
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // SP 17: Obtener Movimientos
        public async Task<List<object>> ObtenerMovimientos(int? idCuenta, DateTime? fi, DateTime? ff, int? tipo)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerMovimientos", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdCuenta", (object?)idCuenta ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaInicio", (object?)fi?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaFin", (object?)ff?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdTipoMovimiento", (object?)tipo ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
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
            }

            return lista;
        }

        // SP 18: Obtener Detalle Cheque
        public async Task<object?> ObtenerDetalleCheque(int idMovimiento)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerDetalleCheque", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdMovimiento", idMovimiento);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new
                {
                    IdDetalleCheque = reader["IdDetalleCheque"],
                    FechaCheque = reader["FechaCheque"],
                    NumeroCheque = reader["NumeroCheque"].ToString(),
                    FechaPago = reader["FechaPago"],
                    Monto = reader["Monto"],
                    NoCuenta = reader["NoCuenta"].ToString()
                };
            }

            return null;
        }

        // SP 19: Obtener Detalle Tarjeta
        public async Task<object?> ObtenerDetalleTarjeta(int idMovimiento)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerDetalleTarjeta", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdMovimiento", idMovimiento);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new
                {
                    IdDetalleTarjeta = reader["IdDetalleTarjeta"],
                    NoTarjeta = reader["NoTarjeta"].ToString(),
                    FechaVencimiento = reader["FechaVencimiento"],
                    FechaMovimiento = reader["FechaMovimiento"],
                    Monto = reader["Monto"],
                    Descripcion = reader["Descripcion"].ToString(),
                    NoCuenta = reader["NoCuenta"].ToString()
                };
            }

            return null;
        }

        // SP 20: Obtener Bitácora
        public async Task<List<object>> ObtenerBitacora(int idCuenta, DateTime? fi, DateTime? ff)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerBitacora", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdCuenta", idCuenta);
            cmd.Parameters.AddWithValue("@FechaInicio", (object?)fi?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaFin", (object?)ff?.Date ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
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
                    DetalleMovimiento = reader["DetalleMovimiento"].ToString()
                });
            }

            return lista;
        }

        // SP 21: Obtener Errores
        public async Task<List<object>> ObtenerErrores(int? idCuenta, DateTime? fi, DateTime? ff)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerErrores", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IdCuenta", (object?)idCuenta ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaInicio", (object?)fi?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaFin", (object?)ff?.Date ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
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
            }

            return lista;
        }
    }
}
