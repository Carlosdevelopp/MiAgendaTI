namespace DataAccess.Models.Tables
{
    public class Usuarios_ET
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set;}
        public string SegundoApellido { get; set; }
        public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public string RutaFoto { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }
    }
}
