using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models.Social
{
    public partial class Avisos
    {
        public Guid AvisoID { get; set; }

        public Guid BoxID { get; set; }

        public string Titulo { get; set; } = null!;

        public string Mensaje { get; set; } = null!;

        public DateTime? FechaCreacion { get; set; }

        public bool? Activo { get; set; }
    }
}
