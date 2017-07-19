using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.Reportes;
using System.Data;

namespace Capa_Negocio.Reportes
{
    public class CNDesplegarFormulario
    {
        DesplegarFormulario objCDDesplegar = new DesplegarFormulario();

        public DataTable SelectValoresFormularioReporte(int no_preingreso, int no_formulario)
        {
            return objCDDesplegar.SelectValoresFormularioReporte(no_preingreso, no_formulario);
        }
        public int SelectTipoCampo(int correlativo_campo)
        {
            return objCDDesplegar.SelectTipoCampo(correlativo_campo);
        }

        public string SelectValorCombo(int correlativo_campo, string valor)
        {
            return objCDDesplegar.SelectValorCombo(correlativo_campo, valor);
        }

        public string SelectValorComboPais(int valor_pais)
        {
            return objCDDesplegar.SelectValorComboPais(valor_pais);
        }

        public string SelectValorComboNiza(int valor_niza)
        {
            return objCDDesplegar.SelectValorComboNiza(valor_niza);
        }
    }
}
