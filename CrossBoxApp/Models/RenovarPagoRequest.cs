using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class RenovarPagoRequest
    {
        public Guid UsuarioID { get; set; }
        public DateTime NuevaFechaInicio { get; set; }
    }
}
