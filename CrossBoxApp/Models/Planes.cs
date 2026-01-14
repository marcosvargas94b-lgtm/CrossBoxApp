using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class Planes
    {
        public int PlanID { get; set; }

        public string Nombre { get; set; } = null!;

        public decimal Precio { get; set; }

        public int MesesDuracion { get; set; }
      
    }
}
