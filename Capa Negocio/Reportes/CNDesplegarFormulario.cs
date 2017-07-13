using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.Reportes;

namespace Capa_Negocio.Reportes
{
    public class CNDesplegarFormulario
    {
        DesplegarFormulario objCDDesplegar = new DesplegarFormulario();

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
    }
}
