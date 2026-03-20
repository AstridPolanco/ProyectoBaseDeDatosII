namespace CajeroAutomaticoAPI.Models
{
    public class PagarChequeRequest
    {
        public int IdCuenta { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaCheque { get; set; }
        public string NumeroCheque { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
    }

    public class NotaDebitoRequest
    {
        public int IdCuenta { get; set; }
        public int IdTarjeta { get; set; }
        public decimal Monto { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
