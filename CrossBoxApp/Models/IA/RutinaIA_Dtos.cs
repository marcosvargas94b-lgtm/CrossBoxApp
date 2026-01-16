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

        // LO QUE RECIBES (Blindado con JsonPropertyName)
        public class RutinaGeneradaDto
        {
            [JsonPropertyName("nombreRutina")] // Forzamos camelCase
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

            [JsonPropertyName("dias")]
            public List<DiaEntrenoDto> Dias { get; set; } = new List<DiaEntrenoDto>();
        }

        public class DiaEntrenoDto
        {
            [JsonPropertyName("dia")]
            public string Dia { get; set; } // "Lunes", "Martes"...

            [JsonPropertyName("grupoMuscular")]
            public string GrupoMuscular { get; set; }

            [JsonPropertyName("ejercicios")]
            public List<EjercicioDto> Ejercicios { get; set; } = new List<EjercicioDto>();
        }

        public class EjercicioDto
        {
            [JsonPropertyName("nombre")]
            public string Nombre { get; set; } // Nombre en Inglés

            [JsonPropertyName("nombreEspanol")]
            public string NombreEspanol { get; set; }

            [JsonPropertyName("series")]
            public string Series { get; set; }

            [JsonPropertyName("repeticiones")]
            public string Repeticiones { get; set; }

            [JsonPropertyName("notaTecnica")]
            public string NotaTecnica { get; set; }

            // Estos son opcionales en el JSON de Gemini, los llenamos nosotros o vienen null
            public string Alternativa { get; set; }
            public string GifUrl { get; set; }
            public string IdExerciseDB { get; set; }
        }

    }
}
