using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class AsignarPermisoDto
    {
        public Guid UsuarioID { get; set; }
        public string ClavePermiso { get; set; }
        public bool Activo { get; set; } // True = Dar, False = Quitar
    }
}
