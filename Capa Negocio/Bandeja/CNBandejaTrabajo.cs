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

        public DataTable SelectFormularios()
        {
            return objCDBandeja.SelectFormularios();
        }

        public int SelectCantidadFormularios()
        {
            return objCDBandeja.SelectCantidadFormularios();
        }
    }
}
