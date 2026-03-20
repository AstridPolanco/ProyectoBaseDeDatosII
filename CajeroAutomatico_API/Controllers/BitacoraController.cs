using CajeroAutomaticoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CajeroAutomaticoAPI.Controllers
{
    [ApiController]
    [Route("api/bitacora")]
    public class BitacoraController : ControllerBase
    {
        private readonly BitacoraService _service;
        public BitacoraController(BitacoraService service) => _service = service;

        /// <summary>Obtener bitácora completa de una cuenta</summary>
        /// <param name="idCuenta">ID de la cuenta bancaria</param>
        /// <param name="fechaInicio">Fecha inicio del filtro (opcional)</param>
        /// <param name="fechaFin">Fecha fin del filtro (opcional)</param>
        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerBitacora(
            [FromQuery] int idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try { return Ok(await _service.ObtenerBitacora(idCuenta, fechaInicio, fechaFin)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}
