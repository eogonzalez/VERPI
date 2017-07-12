using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.Bandeja;
using System.Data;

namespace Capa_Negocio.Bandeja
{
    public class CNBandejaTrabajo
    {
        BandejaTrabajo objCDBandeja = new BandejaTrabajo();

        public DataTable SelectFormularios(int tipo_tramite = 0, string fecha_inicial = "", string fecha_final = "")
        {
            return objCDBandeja.SelectFormularios(tipo_tramite, fecha_inicial, fecha_final);
        }

        public int SelectCantidadFormularios()
        {
            return objCDBandeja.SelectCantidadFormularios();
        }
    }
}
