using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models.Social
{
    public class NotificacionDto
    {
        public Guid NotificacionID { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
    }
}
