using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models.Services
{
    public class SesionService
    {
        public UsuarioSesionDto Usuario { get; set; }

        public bool EstaLogueado => Usuario != null;
    }
}
