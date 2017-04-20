using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Capa_Entidad.General;
using Capa_Datos.General;

namespace Capa_Negocio.General
{
    public class CNMantenimientos
    {
        Mantenimientos objCNMant = new Mantenimientos();

        public DataTable SelectMantenimientoGridView(CEMantenimientos objCEMant)
        {
            return objCNMant.SelectMantenimientoGridView(objCEMant);
        }

        public bool InsertMantenimiento(CEMantenimientos objCEMant)
        {
            return objCNMant.InsertMantenimiento(objCEMant);
        }

        public bool DeleteMantenimiento(CEMantenimientos objCEMant)
        {
            return objCNMant.DeleteMantenimiento(objCEMant);
        }

        public DataTable SelectMantenimientoRegistro(CEMantenimientos objCEMant)
        {
            return objCNMant.SelectMantenimientoRegistro(objCEMant);
        }

        public bool UpdateMantenimientoRegistro(CEMantenimientos objCEMant)
        {
            return objCNMant.UpdateMantenimientoRegistro(objCEMant);
        }
    }
}
