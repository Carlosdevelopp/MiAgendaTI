using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Tables
{
    public class Contactos_ET
    {
        public int ContactoId { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string FotoRuta { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
