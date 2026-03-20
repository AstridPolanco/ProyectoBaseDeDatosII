using CajeroAutomaticoAPI.Models;
using CajeroAutomaticoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CajeroAutomaticoAPI.Controllers
{
    [ApiController]
    [Route("api/cuentahabiente")]
    public class CuentaHabienteController : ControllerBase
    {
        private readonly CuentaHabienteService _service;
        public CuentaHabienteController(CuentaHabienteService service) => _service = service;

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] CrearCuentaHabienteRequest req)
        {
            try { return Ok(await _service.Crear(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCuentaHabienteRequest req)
        {
            try { return Ok(await _service.Actualizar(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> Obtener([FromQuery] int? id, [FromQuery] string? cui)
        {
            try
            {
                var result = await _service.Obtener(id, cui);
                if (result == null) return NotFound(new { Error = "Cuentahabiente no encontrado." });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar([FromQuery] string? busqueda)
        {
            try { return Ok(await _service.Listar(busqueda)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}