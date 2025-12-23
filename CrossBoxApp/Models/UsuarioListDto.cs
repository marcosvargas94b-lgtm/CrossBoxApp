using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class UsuarioListDto
    {
        public Guid UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string TipoUsuario { get; set; } // "Atleta" o "Admin"

        // Datos de inscripción
        public string FechaInscripcionTexto { get; set; } // Ej: "15 de Diciembre"

        // Propiedad auxiliar para la interfaz (controlar menú abierto/cerrado)
        public bool MostrarMenu { get; set; }
        public bool Pagado { get; set; }
    }

    // NUEVO DTO: Para enviar la fecha seleccionada desde el Modal
    public class AsignarFechaDto
    {
        public Guid UsuarioId { get; set; }
        public DateTime FechaSeleccionada { get; set; }
    }
}
