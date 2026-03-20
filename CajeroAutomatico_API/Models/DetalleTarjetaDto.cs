namespace CajeroAutomaticoAPI.Models
{
    public class DetalleTarjetaDto
    {
        public int IdDetalleTarjeta { get; set; }
        public string NoTarjeta { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string NoCuenta { get; set; } = string.Empty;
    }
}