using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models.Services
{
    // DTO local para recibir datos
    public class CheckInDto
    {
        public Guid BoxId { get; set; }
        public Guid UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string FotoUrl { get; set; }
    }
}
