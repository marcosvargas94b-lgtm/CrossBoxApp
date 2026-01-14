using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models
{
    public class TestGroupDto
    {
        public string CategoriaNombre { get; set; } // Viene de Test.Descripcion
        public List<TestItemDto> Items { get; set; } = new List<TestItemDto>();
    }

    public class TestItemDto
    {
        public Guid TestBoxId { get; set; }
        public string Nombre { get; set; }      // Viene de TestBox.Descripcion
        public string MedidaNombre { get; set; } // Viene de MedidaTest.Descripcion
    }

    // Para llenar los combos del Modal
    public class TestConfigCatalogsDto
    {
        public List<ItemDto> Categorias { get; set; } // Tabla Test
        public List<ItemDto> Medidas { get; set; }    // Tabla MedidaTest
    }

    public class ItemDto { public Guid Id { get; set; } public string Nombre { get; set; } }

    // Para Guardar
    public class CreateTestBoxes
    {
        public Guid BoxID { get; set; }
        public Guid TestID { get; set; }
        public Guid MedidaTestID { get; set; }
        public string Descripcion { get; set; }
    }
}
