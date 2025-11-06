namespace DataAccess.Models.Tables;

public class Sesion
{
    public int SesionId { get; set; }
    public int UsuarioId { get; set; }
    public required string TokenSesion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaExpiracion { get; set; }
    public required string IpAdress{get;set;}
    public required string Navegador { get; set; }
    public bool Estado { get; set; }
}
