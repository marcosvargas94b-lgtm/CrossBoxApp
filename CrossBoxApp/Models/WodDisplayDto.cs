using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class WodDiaDto
    {
        public DateTime Fecha { get; set; }
        public List<SegmentoVisualDto> Segmentos { get; set; } = new List<SegmentoVisualDto>();
    }

    public class SegmentoVisualDto
    {
        public string Titulo { get; set; }
        public List<EjercicioVisualDto> Ejercicios { get; set; } = new List<EjercicioVisualDto>();
    }

    public class EjercicioVisualDto
    {
        public string Descripcion { get; set; }
        public string PesoRepeticiones { get; set; }
        public int Orden { get; set; }
    }
}
