namespace CajeroAutomaticoAPI.Models
{
    public class CrearTarjetaRequest
    {
        public int IdCuentaHabitante { get; set; }
        public int IdCuenta { get; set; }
        public string NoTarjeta { get; set; } = string.Empty;
        public string FechaVencimiento { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public string PIN { get; set; } = string.Empty;
    }

    public class ValidarPINRequest
    {
        public string NoTarjeta { get; set; } = string.Empty;
        public string PINIngresado { get; set; } = string.Empty;
    }

    public class CambiarPINRequest
    {
        public string NoTarjeta { get; set; } = string.Empty;
        public string PINActual { get; set; } = string.Empty;
        public string PINNuevo { get; set; } = string.Empty;
    }
}
