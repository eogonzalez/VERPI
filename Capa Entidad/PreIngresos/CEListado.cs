using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.PreIngresos
{
    public class CEListado
    {
        public int Correlativo_Lista { get; set; }
        public int No_PreIngreso { get; set; }
        public int Tipo_Lista { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}
