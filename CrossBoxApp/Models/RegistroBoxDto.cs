using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class RegistroBoxDto
    {
        // --- DATOS DEL BOX ---
        public string NombreBox { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }

        // --- DATOS DEL ADMINISTRADOR ---
        public string NombreAdmin { get; set; }
        public string ApellidosAdmin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
