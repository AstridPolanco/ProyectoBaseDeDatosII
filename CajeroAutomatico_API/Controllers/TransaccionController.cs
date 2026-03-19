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

        public TransaccionController(TransaccionService service)
        {
            _service = service;
        }

        [HttpPost("deposito")]
        public async Task<IActionResult> Deposito([FromBody] DepositoRequest req)
        {
            try
            {
                var result = await _service.Deposito(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("notacredito")]
        public async Task<IActionResult> NotaCredito([FromBody] NotaCreditoRequest req)
        {
            try
            {
                var result = await _service.NotaCredito(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
