using CajeroAutomaticoAPI.Models;
using CajeroAutomaticoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CajeroAutomaticoAPI.Controllers
{
    [ApiController]
    [Route("api/tarjeta")]
    public class TarjetaController : ControllerBase
    {
        private readonly TarjetaService _service;
        public TarjetaController(TarjetaService service) => _service = service;

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] CrearTarjetaRequest req)
        {
            try { return Ok(await _service.CrearTarjeta(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpPost("validarpin")]
        public async Task<IActionResult> ValidarPIN([FromBody] ValidarPINRequest req)
        {
            try { return Ok(await _service.ValidarPIN(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpPost("cambiarpin")]
        public async Task<IActionResult> CambiarPIN([FromBody] CambiarPINRequest req)
        {
            try { return Ok(await _service.CambiarPIN(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> Obtener([FromQuery] int? idTarjeta, [FromQuery] string? noTarjeta)
        {
            try
            {
                var result = await _service.ObtenerTarjeta(idTarjeta, noTarjeta);
                if (result == null) return NotFound(new { Error = "Tarjeta no encontrada." });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpGet("listar/{idCuenta}")]
        public async Task<IActionResult> Listar(int idCuenta)
        {
            try { return Ok(await _service.ListarTarjetasPorCuenta(idCuenta)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}
