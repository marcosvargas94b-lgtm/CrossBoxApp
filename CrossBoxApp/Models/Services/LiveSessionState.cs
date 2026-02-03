using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models.Services
{
    public class LiveSessionState
    {
        public List<CheckInDto> AtletasEnClase { get; set; } = new List<CheckInDto>();
        public Guid BoxId { get; set; }
        public List<ResultadoCapturadoViewModel> ResultadosJuez { get; set; } = new();
        public bool FueModoJuez { get; set; } = false;
    }

    public class ResultadoCapturadoViewModel
    {
        public Guid IdIdentificador { get; set; } = Guid.NewGuid(); // ID temporal para la UI
        public List<CheckInDto> Atletas { get; set; } = new(); // Puede ser 1 o 2
        public string NombreEquipo { get; set; } // Opcional
        public string TiempoFinal { get; set; } // "12:45"
        public bool Finalizado { get; set; } = false; // Para ponerlo en verde/gris
    }
}
