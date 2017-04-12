using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Administracion
{
    public class CEPermisosPerfiles
    {
        public int ID_PermisoPerfil { get; set; }
        public int ID_TipoUsuario {get;set;}

        public int ID_Opcion { get; set; }

        public int ID_UsuarioAutoriza { get; set; }

        public Boolean Insertar { get; set; }

        public Boolean Acceder { get; set; }

        public Boolean Editar { get; set; }
        public Boolean Borrar { get; set; }
        public Boolean Aprobar { get; set; }
        public Boolean Rechazar { get; set; }
        
    }
}
