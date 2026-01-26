using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class Boxes
    {       
        public Guid BoxID { get; set; }

        public string? Nombre { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public Guid? Logo { get; set; }

        public string? Direccion { get; set; }

        public string? Ciudad { get; set; }

        public string? Telefono { get; set; }

        public bool? Activo { get; set; }

        public int? PlanID { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public string? EstadoSuscripcion { get; set; }
        public decimal? CostoInscripcion { get; set; }
        public bool? WhatsappActivo { get; set; }
        public virtual ICollection<UsuarioSesionDto>? Usuarios { get; set; }
        public virtual Planes? Plan { get; set; }
    }
}
