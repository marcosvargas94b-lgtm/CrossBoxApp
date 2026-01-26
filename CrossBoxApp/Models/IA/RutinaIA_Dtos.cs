using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CrossBoxApp.Models.IA
{
    public class RutinaIA_Dtos
    {
        public class PerfilAtletaDto
        {
            public string Nivel { get; set; }
            public string Objetivo { get; set; }
            public int DiasPorSemana { get; set; }
            public string Lesiones { get; set; }
            public string Equipo { get; set; }
        }

        public class RutinaGeneradaDto
        {
            // Este campo NO viene del JSON de la IA, lo llenamos nosotros manualmente en el servicio
            public Guid IdBaseDatos { get; set; }

            [JsonPropertyName("nombreRutina")]
            public string NombreRutina { get; set; }

            [JsonPropertyName("explicacion")]
            public string Explicacion { get; set; }

            [JsonPropertyName("semanas")]
            public List<SemanaDto> Semanas { get; set; } = new List<SemanaDto>();
        }

        public class SemanaDto
        {
            [JsonPropertyName("numeroSemana")]
            public int NumeroSemana { get; set; }

            [JsonPropertyName("enfoque")]
            public string Enfoque { get; set; }

            [JsonPropertyName("dias")]
            public List<DiaEntrenoDto> Dias { get; set; } = new List<DiaEntrenoDto>();
        }

        public class DiaEntrenoDto
        {
            [JsonPropertyName("dia")]
            public string Dia { get; set; }

            [JsonPropertyName("grupoMuscular")]
            public string GrupoMuscular { get; set; }

            [JsonPropertyName("ejercicios")]
            public List<EjercicioDto> Ejercicios { get; set; } = new List<EjercicioDto>();
        }

        public class EjercicioDto
        {
            [JsonPropertyName("nombreEspanol")]
            public string NombreEspanol { get; set; }

            [JsonPropertyName("series")]
            public string Series { get; set; }

            [JsonPropertyName("repeticiones")]
            public string Repeticiones { get; set; }

            [JsonPropertyName("notaTecnica")]
            public string NotaTecnica { get; set; }
        }

        public class ResumenCierreDto
        {
            public string Titulo { get; set; } // Ej: "¡Excelente Trabajo!" o "Mes Difícil..."
            public string AnalisisAdherencia { get; set; } // Opinión sobre cuántos días fuiste
            public string AnalisisRendimiento { get; set; } // Opinión sobre tus cargas/cansancio
            public string EstrategiaSiguiente { get; set; } // Qué haremos en el próximo mes
            public int CalificacionGeneral { get; set; } // 1 a 100
        }

        public class FeedbackInputDto
        {
            public Guid RutinaID { get; set; }
      
            public string DiaSemana { get; set; }
            public int NivelCansancio { get; set; }
            public string DolorLesion { get; set; }
            public string Comentarios { get; set; }
        }
    }
}
