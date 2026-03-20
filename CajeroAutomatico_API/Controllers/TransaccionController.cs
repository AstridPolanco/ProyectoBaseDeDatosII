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

        [HttpPost("deposito")]
        public async Task<IActionResult> Deposito([FromBody] DepositoRequest req)
        {
            try { return Ok(await _service.Deposito(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpPost("notacredito")]
        public async Task<IActionResult> NotaCredito([FromBody] NotaCreditoRequest req)
        {
            try { return Ok(await _service.NotaCredito(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpPost("pagarcheque")]
        public async Task<IActionResult> PagarCheque([FromBody] PagarChequeRequest req)
        {
            try { return Ok(await _service.PagarCheque(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpPost("notadebito")]
        public async Task<IActionResult> NotaDebito([FromBody] NotaDebitoRequest req)
        {
            try { return Ok(await _service.NotaDebito(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }

        [HttpPost("retiro")]
        public async Task<IActionResult> Retiro([FromBody] RetiroRequest req)
        {
            try { return Ok(await _service.Retiro(req)); }
            catch (Exception ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}
