using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.Tables;

[Table("DetallesContactosRedes")]
public class DetalleContacto
{
    public int DetContactoRedId { get; set; }
    public int ContactoId { get; set; }
    public int TipoContactoId { get; set; }
    public required string URL { get; set; }
    public required string NombreUsuarioRed { get; set; }
    public DateTime FechaRegistro { get; set; }
}
