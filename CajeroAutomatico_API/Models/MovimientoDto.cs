namespace CajeroAutomaticoAPI.Models
{
    public class MovimientoDto
    {
        public int IdMovimiento { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public DateTime FechaMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? MotivoFallo { get; set; }
        public string NoCuenta { get; set; } = string.Empty;
    }
}