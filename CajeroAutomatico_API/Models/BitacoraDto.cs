namespace CajeroAutomaticoAPI.Models
{
    public class BitacoraDto
    {
        public int IdBitacora { get; set; }
        public DateTime FechaBitacora { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoNuevo { get; set; }
        public decimal Diferencia { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public DateTime FechaMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string DetalleMovimiento { get; set; } = string.Empty;
    }
}