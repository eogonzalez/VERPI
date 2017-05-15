using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.Administracion;
using System.Data;
using Capa_Entidad.Administracion;

namespace Capa_Negocio.Administracion
{
    public class CNCamposFormularios
    {
        CamposFormularios objCDCampos = new CamposFormularios();

        public DataTable SelectCamposFormulario(int no_formulario)
        {
            return objCDCampos.SelectCamposFormulario(no_formulario);
        }

        public DataTable SelectFormularios()
        {
            return objCDCampos.SelectFormularios();
        }

        public bool InsertCamposFormulario(CECamposFormularios objCECampos)
        {
            return objCDCampos.InsertCamposFormulario(objCECampos);
        }

        public bool UpdateCampoFormulario(CECamposFormularios objCECampos)
        {
            return objCDCampos.UpdateCampoFormulario(objCECampos);
        }

        public DataTable SelectCampoFormulario(int correlativo_campo)
        {
            return objCDCampos.SelectCampoFormulario(correlativo_campo);
        }

        public bool DeleteCampoFormulario(int correlativo_campo)
        {
            return objCDCampos.DeleteCampoFormulario(correlativo_campo);
        }
    }
}
