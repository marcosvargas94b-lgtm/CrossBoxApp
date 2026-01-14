using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class StaffDto
    {
        public Guid UsuarioID { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        // Lista de claves de permisos que tiene este usuario
        public List<string> PermisosActivos { get; set; } = new List<string>();
    }
}
