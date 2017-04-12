using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Administracion
{
    public class CEMenu
    {
        public int ID_MenuOpcion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string URL { get; set; }
        public string Comando { get; set; }
        public int Orden { get; set; }
        public Boolean Obligatorio { get; set; }
        public Boolean Visible { get; set; }

        public Boolean Login { get; set; }

        public int Id_Padre { get; set; }
        public int ID_UsuarioAutoriza { get; set; }
    }
}
