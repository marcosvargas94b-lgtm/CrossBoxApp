using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class RankingDto
    {
        public Guid UsuarioID { get; set; }
        public string NombreCompleto { get; set; }
        public string FotoUrl { get; set; }
        public int Likes { get; set; }
        public int Posicion { get; set; }
        public bool EsYo { get; set; } // Para saber si es el usuario logueado
    }
}
