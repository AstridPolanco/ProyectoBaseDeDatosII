using CajeroAutomaticoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CajeroAutomaticoAPI.Controllers
{
    [ApiController]
    [Route("api/movimientos")]
    public class MovimientosController : ControllerBase
    {
        private readonly BitacoraService _service;
        public MovimientosController(BitacoraService service) => _service = service;

        /// <summary>Obtener todos los movimientos de una cuenta</summary>
        /// <param name="idCuenta">ID de la cuenta (opcional, sin filtro devuelve todos)</param>
        /// <param name="fechaInicio">Fecha inicio del filtro (opcional)</param>
        /// <param name="fechaFin">Fecha fin del filtro (opcional)</param>
        /// <param name="idTipoMovimiento">1=Depósito, 2=Cheque, 3=Nota Crédito, 4=Nota Débito</param>
        [HttpGet("listar")]
        public async Task<IActionResult> ObtenerMovimientos(
            [FromQuery] int? idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] int? idTipoMovimiento)
        {
            try { return Ok(await _service.ObtenerMovimientos(idCuenta, fechaInicio, fechaFin, idTipoMovimiento)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}
