using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class AtletaConEstadoDto
    {
        public Guid UsuarioID { get; set; }
        public string NombreCompleto { get; set; }
        public string FotoUrl { get; set; }
        public string Estatus { get; set; } 
        public DateTime FechaVencimiento { get; set; }
        public int DiasRestantes { get; set; }
        public string ColorEstatus { get; set; }
        public bool MostrarMenu { get; set; } = false;
    }
}
