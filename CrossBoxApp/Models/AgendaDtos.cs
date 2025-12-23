using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class HorarioDto
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
    }

    // Para visualizar las tarjetas con usuarios
    public class AgendaVisualDto
    {
        public Guid HorarioId { get; set; }
        public string HorarioNombre { get; set; }
        public List<string> UsuariosInscritos { get; set; } = new List<string>();
        public bool EsMiReserva { get; set; }
    }

    // Para enviar la reserva
    public class ReservaDto
    {
        public Guid UsuarioId { get; set; }
        public Guid HorarioId { get; set; }
        public Guid BoxId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
