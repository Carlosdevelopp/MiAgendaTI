namespace DataAccess.Models.Tables;

public class Usuario
{
    public int UsuarioId { get; set; }
    public required string Nombre { get; set; } 
    public required string PrimerApellido { get; set;}
    public required string SegundoApellido { get; set; }
    public required string Correo { get; set; }
    public required string NombreUsuario { get; set; }
    public string? Password { get; set; }
    public string? RutaFoto { get; set; } 
    public required string Telefono { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool Estado { get; set; }
}
