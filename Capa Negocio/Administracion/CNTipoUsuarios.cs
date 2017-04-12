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
    public class CNTipoUsuarios
    {
        TipoUsuarios objCDTipoUsuarios = new TipoUsuarios();
        public DataSet SelectTipoUsuarios()
        {
            return objCDTipoUsuarios.SelectTipoUsuarios();
        }

        public Boolean InsertTipoUsuarios(CETipoUsuarios objetoEntidad)
        {
            return objCDTipoUsuarios.InsertTipoUsuarios(objetoEntidad);
        }

        public DataTable SelectTipoUsuario(int id_tipousuario)
        {
            return objCDTipoUsuarios.SelectTipoUsuario(id_tipousuario);
        }

        public Boolean UpdateTipoUsuario(CETipoUsuarios objTipoUsuario)
        {
            return objCDTipoUsuarios.UpdateTipoUsuario(objTipoUsuario);
        }

        public Boolean DeleteTipoUsuario(CETipoUsuarios objTipoUsuario)
        {
            return objCDTipoUsuarios.DeleteTipoUsuario(objTipoUsuario);
        }
    }
}
