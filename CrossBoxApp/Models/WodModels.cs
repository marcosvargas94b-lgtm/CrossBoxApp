using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class ProgramacionWodViewModel
    {
        public Guid BoxID { get; set; } 
        public DateTime FechaProgramacion { get; set; } = DateTime.Now;
        public List<BloqueWodViewModel> Bloques { get; set; } = new List<BloqueWodViewModel>();
    }

    public class BloqueWodViewModel
    {
        public Guid SegmentoClaseID { get; set; }
        public List<EjercicioViewModel> Ejercicios { get; set; } = new List<EjercicioViewModel>();
        public string NombreSegmento { get; set; }
    }

    public class EjercicioViewModel
    {
        public string Descripcion { get; set; }
        public string PesoRepeticiones { get; set; }
    }

    public class SegmentoDto // Para llenar el combo
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }
}
