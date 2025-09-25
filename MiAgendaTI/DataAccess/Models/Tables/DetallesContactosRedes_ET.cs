using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Tables
{
    public class DetallesContactosRedes_ET
    {
        public int DetContactoRedId { get; set; }
        public int ContactoId { get; set; }
        public int TipoContactoId { get; set; }
        public string URL { get; set; }
        public string NommbreUsuarioRed { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
