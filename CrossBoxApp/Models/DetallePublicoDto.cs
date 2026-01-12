using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class DetallePublicoDto
    {
        public Guid UsuarioID { get; set; }
        public string NombreCompleto { get; set; }
        public string FotoUrl { get; set; }
        public string Descripcion { get; set; }
        public int TotalLikes { get; set; }
        public bool YaLeDiMatch { get; set; }   // ¿Yo ya le di corazón?
        public bool EsMatchMutuo { get; set; }  // ¿Ambos nos dimos?
        public string WhatsappTarget { get; set; } // El número (solo si es mutuo)
    }
}
