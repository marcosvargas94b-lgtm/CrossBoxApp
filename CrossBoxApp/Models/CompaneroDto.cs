using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class CompaneroDto
    {
        public Guid UsuarioID { get; set; }
        public string NombreCompleto { get; set; }
        public string FotoUrl { get; set; }
    }
}
