namespace CajeroAutomaticoAPI.Models
{
    public class DetalleChequeDto
    {
        public int IdDetalleCheque { get; set; }
        public DateTime FechaCheque { get; set; }
        public string NumeroCheque { get; set; } = string.Empty;
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string NoCuenta { get; set; } = string.Empty;
    }
}