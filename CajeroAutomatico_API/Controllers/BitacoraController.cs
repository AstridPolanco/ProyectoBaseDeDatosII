using CajeroAutomaticoAPI.Models;
using CajeroAutomaticoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CajeroAutomaticoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitacoraController : ControllerBase
    {
        private readonly BitacoraService _service;
        public BitacoraController(BitacoraService service) => _service = service;

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerBitacora(
            [FromQuery] int idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try   { return Ok(await _service.ObtenerBitacora(idCuenta, fechaInicio, fechaFin)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}
