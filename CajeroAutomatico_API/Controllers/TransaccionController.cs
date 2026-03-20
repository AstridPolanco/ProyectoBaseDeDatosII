using CajeroAutomaticoAPI.Models;
using CajeroAutomaticoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CajeroAutomaticoAPI.Controllers
{
    [ApiController]
    [Route("api/transaccion")]
    public class TransaccionController : ControllerBase
    {
        private readonly TransaccionService _service; 
    public TransaccionController(TransaccionService service) => _service = service;

    [HttpPost("pagar-cheque")]
    public async Task<IActionResult> PagarCheque([FromBody] PagarChequeRequest req) 
    {
        try { return Ok(await _service.PagarCheque(req)); }
        catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
    }

     [HttpPost("pagarcheque")] 
        public async Task<IActionResult> PagarChequeSinGuion([FromBody] PagarChequeRequest req)
        {
            try
            {
                var result = await _service.PagarCheque(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

    [HttpPost("nota-debito")]
    public async Task<IActionResult> NotaDebito([FromBody] NotaDebitoRequest req) 
    {
        try { return Ok(await _service.NotaDebito(req)); }
        catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
    }

     [HttpGet("movimientos")]
        public async Task<IActionResult> Movimientos(
            [FromQuery] int? idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] int? idTipoMovimiento)
        {
            try
            {
                var result = await _service.ObtenerMovimientos(idCuenta, fechaInicio, fechaFin, idTipoMovimiento);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("detalle-cheque/{idMovimiento}")]
        public async Task<IActionResult> DetalleCheque(int idMovimiento)
        {
            try
            {
                var result = await _service.ObtenerDetalleCheque(idMovimiento);
                if (result == null)
                    return NotFound(new { Error = "No se encontró detalle para este movimiento." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("detalle-tarjeta/{idMovimiento}")]
        public async Task<IActionResult> DetalleTarjeta(int idMovimiento)
        {
            try
            {
                var result = await _service.ObtenerDetalleTarjeta(idMovimiento);
                if (result == null)
                    return NotFound(new { Error = "No se encontró detalle para este movimiento." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("bitacora/{idCuenta}")]
        public async Task<IActionResult> Bitacora(
            int idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var result = await _service.ObtenerBitacora(idCuenta, fechaInicio, fechaFin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("errores")]
        public async Task<IActionResult> Errores(
            [FromQuery] int? idCuenta,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var result = await _service.ObtenerErrores(idCuenta, fechaInicio, fechaFin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
