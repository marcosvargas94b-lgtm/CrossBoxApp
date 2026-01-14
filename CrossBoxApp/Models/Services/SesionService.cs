using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models.Services
{
    public class SesionService
    {
        // Tu propiedad donde guardas al usuario logueado
        public UsuarioSesionDto Usuario { get; set; }

        // --- EL MÉTODO QUE TE FALTA Y CAUSA LOS ERRORES ---
        public bool TienePermiso(string clavePermiso)
        {
            // 1. Si no hay usuario, nadie tiene permiso
            if (Usuario == null) return false;

            // 2. Si es el DUEÑO (Tipo 1), tiene permiso DE TODO
            if (Usuario.TipoUsuarioID == 1) return true;

            // 3. Si es Staff (Tipo 3), revisamos si tiene el permiso específico en su lista
            if (Usuario.ListaPermisos != null && Usuario.ListaPermisos.Contains(clavePermiso))
            {
                return true;
            }

            // 4. Si no cumple nada de lo anterior, denegado
            return false;
        }
    }
}
