using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Capa_Datos.General;
using Capa_Entidad.General;

namespace Capa_Negocio.General
{
    public class CNFormularios
    {
        Formularios objCDFormulario = new Formularios();

        public DataTable SelectCamposFormulario(CEFormularios objCEFormulario)
        {
            return objCDFormulario.SelectCamposFormulario(objCEFormulario);
        }

        public DataTable SelectCantidadCamposFormulario(int no_formulario)
        {
            return objCDFormulario.SelectCantidadCamposFormulario(no_formulario);
        }

        public DataTable SelectFormularios(int tipo_tramite)
        {
            return objCDFormulario.SelectFormularios(tipo_tramite);
        }

        public bool InsertDatosFormularioBorrador(CEFormularios objCEFormulario)
        {
            return objCDFormulario.InsertDatosFormularioBorrador(objCEFormulario);
        }
    }
}
