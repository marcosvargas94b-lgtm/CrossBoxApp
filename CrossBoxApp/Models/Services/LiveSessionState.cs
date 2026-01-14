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
    }
}
