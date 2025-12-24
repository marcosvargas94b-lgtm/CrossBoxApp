using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class PerfilDto
    {
        public Guid PerfilID { get; set; }
        public Guid UsuarioID { get; set; }
        public string FotoPerfil { get; set; } // Guardará la URL o nombre del archivo
        public string Descripcion { get; set; }
        public string Whatsapp { get; set; }
        public int TotalLikes { get; set; }
    }
}
