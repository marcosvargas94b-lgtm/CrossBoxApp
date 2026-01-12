using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class RunningLogs
    {        
        public Guid RunningLogID { get; set; }
        public Guid UsuarioID { get; set; }
        public DateTime Fecha { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public bool? Realizado { get; set; }
        public int? RPE { get; set; }
        public string? ComentariosAtleta { get; set; }
    }
}
