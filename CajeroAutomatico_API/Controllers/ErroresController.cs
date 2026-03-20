using CajeroAutomaticoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CajeroAutomaticoAPI.Controllers
{
    [ApiController]
    [Route("api/errores")]
    public class ErroresController : ControllerBase
    {
        private readonly BitacoraService _service;
        public ErroresController(BitacoraService service) => _service = service;

        /// <summary>Obtener transacciones fallidas y errores registrados</summary>
        /// <param name="idCuenta">ID de la cuenta (opcional, sin filtro devuelve todos los errores)</param>
        /// <param name="fechaInicio">Fecha inicio del filtro (opcional)</param>
        /// <param name="fechaFin">Fecha fin del filtro (opcional)</param>
        [HttpGet("listar")]
        public async Task<IActionResult> ObtenerErrores(
            [FromQuery] int? idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try { return Ok(await _service.ObtenerErrores(idCuenta, fechaInicio, fechaFin)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}