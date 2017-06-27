using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Administracion
{
    public class CEEstados
    {

        public int ID_Estado { get; set; }
        public string TipoTramite { get; set; }
        public int Codigo_Estado { get; set; }
        public string Descripcion { get; set; }
        public int Dias_Max { get; set; }
        public int Dias_Min { get; set; }

    }
}
