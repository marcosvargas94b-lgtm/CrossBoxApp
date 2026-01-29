using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class FotoFeedDto
    {
        public Guid FotoDiaID { get; set; }
        public string FotoUrl { get; set; } // Foto del entrenamiento
        public int Likes { get; set; }
        public string Descripcion { get; set; }

        // Datos del dueño de la foto
        public string NombreUsuario { get; set; }
        public string FotoPerfilUsuario { get; set; } // Avatar pequeño

        // Estado para el usuario que está viendo
        public bool YaDiLike { get; set; }
        public Guid UsuarioID { get; set; }
    }
}
