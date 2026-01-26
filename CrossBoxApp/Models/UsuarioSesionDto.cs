using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class UsuarioSesionDto
    {
        public Guid UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public Guid BoxID { get; set; }
        public string BoxNombre { get; set; }
        public int TipoUsuarioID { get; set; }
        public string Email { get; set; }
        public bool? Validado { get; set; }
        public virtual Boxes? Box { get; set; }
        public List<string> ListaPermisos { get; set; } = new List<string>();
    }
}
