using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class EvaluacionVisualDto
    {
        // Datos del Test (Configuración)
        public Guid TestBoxId { get; set; }
        public string NombreTest { get; set; } // Ej: Fran
        public string Categoria { get; set; }  // Ej: Benchmark
        public string Medida { get; set; }     // Ej: Tiempo, Reps

        // Datos del Usuario (Resultado)
        public Guid? TestUsuarioId { get; set; } // Null si no ha registrado nada
        public float? Valor { get; set; }        // El resultado
        public DateTime? Fecha { get; set; }
    }

    public class GuardarResultadoDto
    {
        public Guid? TestUsuarioId { get; set; } // Null = Nuevo, Valor = Editar
        public Guid TestBoxId { get; set; }
        public Guid UsuarioId { get; set; }
        public float Valor { get; set; }
        public bool? EsNuevaMarca { get; set; }
    }
}