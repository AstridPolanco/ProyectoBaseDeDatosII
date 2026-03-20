namespace CajeroAutomaticoAPI.Models
{
    public class DepositoRequest
    {
        public int IdCuenta { get; set; }
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
        public string? Usuario { get; set; }
    }

    public class NotaCreditoRequest
    {
        public int IdCuenta { get; set; }
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
        public string? Usuario { get; set; }
    }

    public class TransaccionResponse
    {
        public int IdMovimiento { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoNuevo { get; set; }
        public string  Resultado { get; set; } = string.Empty;
    }
    public class RetiroRequest
    {
        public int IdCuenta { get; set; }
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
        public string? Usuario { get; set; }
    }
}
