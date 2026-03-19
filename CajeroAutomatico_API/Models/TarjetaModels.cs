namespace CajeroAutomaticoAPI.Models
{
    public class CrearTarjetaRequest
    {
        public int IdCuentaHabitante { get; set; }
        public int IdCuenta { get; set; }
        public string NoTarjeta { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public string CVV { get; set; } = string.Empty;
        public string PIN { get; set; } = string.Empty;
    }

    public class ValidarPINRequest
    {
        public string NoTarjeta { get; set; } = string.Empty;
        public string PINIngresado { get; set; } = string.Empty;
    }
    public class ObtenerTarjetaRequest
    {
        public int? IdTarjeta { get; set; }
        public string? NoTarjeta { get; set; }
    }

    public class TarjetaResponse
    {
        public int IdTarjeta { get; set; }
        public string NoTarjeta { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public string CVV { get; set; } = string.Empty;
        public bool Activa { get; set; }
        public string NoCuenta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public string Titular { get; set; } = string.Empty;
    }
}
