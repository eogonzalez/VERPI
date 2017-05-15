using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.Administracion;
using Capa_Entidad.Administracion;
using System.Data;

namespace Capa_Negocio.Administracion
{
    public class CNFormularios
    {
        Formularios objCDFormularios = new Formularios();

        public DataTable SelectFormularios()
        {
            return objCDFormularios.SelectFormularios();
        }

        public bool InsertFormulario(CEFormularios objCEFormulario)
        {
            return objCDFormularios.InsertFormulario(objCEFormulario);
        }

        public DataTable SelectFormulario(int no_formulario)
        {
            return objCDFormularios.SelectFormulario(no_formulario);
        }

        public bool DeleteFormulario(int no_formulario)
        {
            return objCDFormularios.DeleteFormulario(no_formulario);
        }

        public bool UpdateFormulario(CEFormularios objCEFormulario)
        {
            return objCDFormularios.UpdateFormulario(objCEFormulario);
        }


    }
}
