using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class HistorialDto
    {
        public DateTime Fecha { get; set; }
        public string Valor { get; set; }        // Ej: "205"
        public string Unidad { get; set; }       // Ej: "LB"
        public bool EsRecordActual { get; set; } // Para pintar de amarillo el mejor
    }
}
