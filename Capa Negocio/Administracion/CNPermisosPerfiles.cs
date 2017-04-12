using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Capa_Datos.Administracion;
using Capa_Entidad.Administracion;

namespace Capa_Negocio.Administracion
{
    public class CNPermisosPerfiles
    {
        PermisosPerfiles objCDPermisosPerfiles = new PermisosPerfiles();
 
        public DataSet SelectPermisosPerfiles(int id_usuarioPermiso = 0)
        {
            return objCDPermisosPerfiles.SelectPermisosPerfiles(id_usuarioPermiso);
        }

        public DataSet SelectCombosPermisosPerfiles()
        {
            return objCDPermisosPerfiles.SelectCombosPermisosPerfiles();
        }

        public Boolean InsertPermisosPerfiles(CEPermisosPerfiles objCEPermisosPerfiles)
        {
            return objCDPermisosPerfiles.InsertPermisosPerfiles(objCEPermisosPerfiles);
        }

        public DataTable SelectPermisoPerfil(int id_permisoPerfil)
        {
            return objCDPermisosPerfiles.SelectPermisoPerfil(id_permisoPerfil);
        }

        public Boolean DeletePermisoPerfil(CEPermisosPerfiles objCEPermisosPerfiles)
        {
            return objCDPermisosPerfiles.DeletePermisoPerfil(objCEPermisosPerfiles);
        }

        public Boolean UpdatePermisoPerfil(CEPermisosPerfiles objCEPermisosPerfiles)
        {
            return objCDPermisosPerfiles.UpdatePermisoPerfil(objCEPermisosPerfiles);
        }
    }
}
