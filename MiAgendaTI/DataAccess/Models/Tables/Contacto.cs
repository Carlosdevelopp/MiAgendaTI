namespace DataAccess.Models.Tables;

public class Contacto
{
    public int ContactoId { get; set; }
    public int UsuarioId { get; set; }
    public required string Nombre { get; set; }
    public required string PrimerApellido { get; set; }
    public required string SegundoApellido { get; set; }
    public required DateTime FechaNacimiento { get; set; }
    public required string FotoRuta { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string Telefono { get; set; } = null!;

    public virtual ICollection<DetalleContacto> DetalleContacto { get; set; } = new List<DetalleContacto>();
}
