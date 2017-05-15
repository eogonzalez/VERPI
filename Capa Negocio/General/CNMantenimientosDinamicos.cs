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
    public class CNMantenimientosDinamicos
    {
        MantenimientosDinamicos objCNMant = new MantenimientosDinamicos();

        public DataTable SelectMantenimientoGridView(CEMantenimientosDinamicos objCEMant)
        {
            return objCNMant.SelectMantenimientoGridView(objCEMant);
        }

        public bool InsertMantenimiento(CEMantenimientosDinamicos objCEMant)
        {
            return objCNMant.InsertMantenimiento(objCEMant);
        }

        public bool DeleteMantenimiento(CEMantenimientosDinamicos objCEMant)
        {
            return objCNMant.DeleteMantenimiento(objCEMant);
        }

        public DataTable SelectMantenimientoRegistro(CEMantenimientosDinamicos objCEMant)
        {
            return objCNMant.SelectMantenimientoRegistro(objCEMant);
        }

        public bool UpdateMantenimientoRegistro(CEMantenimientosDinamicos objCEMant)
        {
            return objCNMant.UpdateMantenimientoRegistro(objCEMant);
        }
    }
}
