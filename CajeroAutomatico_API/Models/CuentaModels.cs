namespace CajeroAutomaticoAPI.Models
{
    public class CrearCuentaRequest
    {
        public int IdHabitante { get; set; }
        public int IdTipo { get; set; }
        public decimal SaldoInicial { get; set; } = 0;
        public string NoCuenta { get; set; } = string.Empty;
    }
}
