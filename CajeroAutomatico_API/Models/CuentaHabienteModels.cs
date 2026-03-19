namespace CajeroAutomaticoAPI.Models
{
    public class CrearCuentaHabienteRequest
    {
        public string  PNombre      { get; set; } = string.Empty;
        public string? SNombre      { get; set; }
        public string? TNombre      { get; set; }
        public string  PApellido    { get; set; } = string.Empty;
        public string? SApellido    { get; set; }
        public string? ApellidoCasa { get; set; }
        public string  CUI          { get; set; } = string.Empty;
        public DateTime FechaNac   { get; set; }
        public string? Email        { get; set; }
        public string? Telefono     { get; set; }
    }

    public class ActualizarCuentaHabienteRequest
    {
        public int     Id        { get; set; }
        public string  PNombre   { get; set; } = string.Empty;
        public string? SNombre   { get; set; }
        public string? TNombre   { get; set; }
        public string  PApellido { get; set; } = string.Empty;
        public string? SApellido { get; set; }
        public string? Email     { get; set; }
        public string? Telefono  { get; set; }
    }
}
