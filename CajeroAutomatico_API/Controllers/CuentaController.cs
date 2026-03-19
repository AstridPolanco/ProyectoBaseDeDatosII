using CajeroAutomaticoAPI.Models;
using CajeroAutomaticoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CajeroAutomaticoAPI.Controllers
{
    [ApiController]
    [Route("api/cuenta")]
    public class CuentaController : ControllerBase
    {
        private readonly CuentaService _service;
        public CuentaController(CuentaService service) => _service = service;

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] CrearCuentaRequest req)
        {
            try   { return Ok(await _service.Crear(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> Obtener([FromQuery] string? noCuenta, [FromQuery] int? idCuenta)
        {
            try
            {
                var result = await _service.Obtener(noCuenta, idCuenta);
                if (result == null) return NotFound(new { Error = "Cuenta no encontrada." });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpGet("listar/{idHabitante}")]
        public async Task<IActionResult> Listar(int idHabitante)
        {
            try   { return Ok(await _service.ListarPorHabiente(idHabitante)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}
