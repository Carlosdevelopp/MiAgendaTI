using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Tables
{
    public class Sesiones_ET
    {
        public int SesionId { get; set; }
        public int UsuarioId { get; set; }
        public string TokenSesion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string IpAdress{get;set;}
        public string Navegador { get; set; }
        public bool Estado { get; set; }
    }
}
