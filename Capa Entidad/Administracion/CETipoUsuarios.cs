using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Administracion
{
    public class CETipoUsuarios
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string TipoPermiso { get; set; }

        public int ID_TipoUsuario { get; set; }
        public int ID_UsuarioAutoriza { get; set; }

    }
}
