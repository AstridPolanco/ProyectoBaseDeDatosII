using CajeroAutomaticoAPI.Data;
using CajeroAutomaticoAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CajeroAutomaticoAPI.Services
{
    public class CuentaHabienteService
    {
        private readonly DatabaseConnection _db;
        public CuentaHabienteService(DatabaseConnection db) => _db = db;

        public async Task<object> Crear(CrearCuentaHabienteRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_CrearCuentaHabiente", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@PNombre",      req.PNombre);
            cmd.Parameters.AddWithValue("@SNombre",      (object?)req.SNombre      ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TNombre",      (object?)req.TNombre      ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PApellido",    req.PApellido);
            cmd.Parameters.AddWithValue("@SApellido",    (object?)req.SApellido    ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ApellidoCasa", (object?)req.ApellidoCasa ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CUI",          req.CUI);
            cmd.Parameters.AddWithValue("@FechaNac",     req.FechaNac);
            cmd.Parameters.AddWithValue("@Email",        (object?)req.Email        ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono",     (object?)req.Telefono     ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new { IdCuentaHabitante = reader["IdCuentaHabitante"], Resultado = reader["Resultado"] };
            return new { Resultado = "Sin respuesta" };
        }

        public async Task<object> Actualizar(ActualizarCuentaHabienteRequest req)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ActualizarCuentaHabiente", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id",       req.Id);
            cmd.Parameters.AddWithValue("@PNombre",  req.PNombre);
            cmd.Parameters.AddWithValue("@SNombre",  (object?)req.SNombre  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TNombre",  (object?)req.TNombre  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PApellido",req.PApellido);
            cmd.Parameters.AddWithValue("@SApellido",(object?)req.SApellido ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email",    (object?)req.Email    ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", (object?)req.Telefono ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new { IdCuentaHabitante = reader["IdCuentaHabitante"], Resultado = reader["Resultado"] };
            return new { Resultado = "Sin respuesta" };
        }

        public async Task<object?> Obtener(int? id, string? cui)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ObtenerCuentaHabiente", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id",  (object?)id  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CUI", (object?)cui ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new
                {
                    IdCuentaHabitante = reader["IdCuentaHabitante"],
                    PrimerNombre      = reader["PrimerNombre"].ToString(),
                    SegundoNombre     = reader["SegundoNombre"].ToString(),
                    PrimerApellido    = reader["PrimerApellido"].ToString(),
                    SegundoApellido   = reader["SegundoApellido"].ToString(),
                    CUI               = reader["CUI"].ToString(),
                    FechaDeNacimiento = reader["FechaDeNacimiento"],
                    Email             = reader["Email"].ToString(),
                    Telefono          = reader["Telefono"].ToString()
                };
            return null;
        }

        public async Task<List<object>> Listar(string? busqueda)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand("sp_ListarCuentaHabientes", conn)
                { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Busqueda", (object?)busqueda ?? DBNull.Value);

            var lista = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                lista.Add(new
                {
                    IdCuentaHabitante = reader["IdCuentaHabitante"],
                    NombreCompleto    = reader["NombreCompleto"].ToString(),
                    PrimerNombre      = reader["PrimerNombre"].ToString(),
                    PrimerApellido    = reader["PrimerApellido"].ToString(),
                    CUI               = reader["CUI"].ToString(),
                    Email             = reader["Email"].ToString(),
                    Telefono          = reader["Telefono"].ToString(),
                    FechaDeNacimiento = reader["FechaDeNacimiento"]
                });
            return lista;
        }
    }
}
