using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Capa_Entidad.General
{
    public class CEFormularios
    {
        public int No_PreIngreso { get; set; }
        public string Nombre_Tabla { get; set; }
        public int No_Formulario { get; set; }

        public int ID_Usuario_Solicita { get; set; } 

        public DataTable Dt_Campos { get; set; }

        public int Correlativo_Campo { get; set; }
        public string Nombre_Documento { get; set; }
        public string TipoDocto { get; set; }
        public string PathDocto { get; set; }

    }
}
