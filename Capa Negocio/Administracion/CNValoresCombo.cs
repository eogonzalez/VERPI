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
    public class CNValoresCombo
    {
        ValoresCombo objCDValores = new ValoresCombo();

        public DataTable SelectValoresCombo(int correlativo_campo)
        {
            return objCDValores.SelectValoresCombo(correlativo_campo);
        }

        public DataTable SelectValorCombo(int id_valor_combo)
        {
            return objCDValores.SelectValorCombo(id_valor_combo);
        }

        public bool DeleteValorCombo(int id_valor_combo)
        {
            return objCDValores.DeleteValorCombo(id_valor_combo);
        }

        public bool InsertValorCombo(CEValoresCombo objCEValores)
        {
            return objCDValores.InsertValorCombo(objCEValores);
        }

        public bool UpdateValorCombo(CEValoresCombo objCEvalores)
        {
            return objCDValores.UpdateValorCombo(objCEvalores);
        }

    }
}
