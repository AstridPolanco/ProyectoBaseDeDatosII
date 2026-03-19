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

        [HttpPost("pagarcheque")]
        public async Task<IActionResult> PagarCheque([FromBody] PagarChequeRequest req)
        {
            try   { return Ok(await _service.PagarCheque(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpPost("notadebito")]
        public async Task<IActionResult> NotaDebito([FromBody] NotaDebitoRequest req)
        {
            try   { return Ok(await _service.NotaDebito(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerBitacora(
            [FromQuery] int idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try   { return Ok(await _service.ObtenerBitacora(idCuenta, fechaInicio, fechaFin)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpGet("movimientos")]
        public async Task<IActionResult> ObtenerMovimientos(
            [FromQuery] int? idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] int? idTipoMovimiento)
        {
            try   { return Ok(await _service.ObtenerMovimientos(idCuenta, fechaInicio, fechaFin, idTipoMovimiento)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpGet("errores")]
        public async Task<IActionResult> ObtenerErrores(
            [FromQuery] int? idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try   { return Ok(await _service.ObtenerErrores(idCuenta, fechaInicio, fechaFin)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}
