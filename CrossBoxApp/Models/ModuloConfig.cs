using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class ModuloConfig
    {

        public Guid ConfigID { get; set; }
        public Guid BoxID { get; set; }
        public string ModuloClave { get; set; } // La clave interna (AGENDA, WOD...)
        public string NombreVisible { get; set; } // El nombre bonito
        public bool Activo { get; set; }
    }
}