using System.Collections.Generic;

namespace CrossBoxApp.Models
{
    public class EstadoRecuperacionDto
    {
        public double DisponibilidadGlobal { get; set; }
        public string Mensaje { get; set; }
        public List<MusculoRecuperacionDto> Musculos { get; set; } = new List<MusculoRecuperacionDto>();
        public List<VolumenItem> VolumenSemanal { get; set; } = new List<VolumenItem>();
    }

    public class MusculoRecuperacionDto
    {
        public string Nombre { get; set; }
        public int Porcentaje { get; set; }
        public int HorasParaRecuperacion { get; set; }
    }

    public class VolumenItem
    {
        public string GrupoMuscular { get; set; }
        public int SeriesTotales { get; set; }
    }
}