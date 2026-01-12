using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models.Social
{
    public class MatchResponseDto
    {
        public bool EsMatchMutuo { get; set; } // ¿Hubo Click?
        public string WhatsappTarget { get; set; } // Solo viene lleno si es mutuo
        public string Mensaje { get; set; }
    }
}
