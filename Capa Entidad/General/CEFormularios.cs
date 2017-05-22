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
        public string Nombre_Tabla { get; set; }
        public int No_Formulario { get; set; }

        public int ID_Usuario_Solicita { get; set; } 

        public DataTable Dt_Campos { get; set; }
    }
}
