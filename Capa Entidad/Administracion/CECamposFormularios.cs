using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Administracion
{
    public class CECamposFormularios
    {
        public int No_Correlativo_Campo { get; set; }

        public int No_Formulario { get; set; }

        public int Orden { get; set; }

        public int Seccion { get; set; }

        public string Etiqueta { get; set; }

        public string NombreControl { get; set; }

        public int TipoControl { get; set; }

        public string Descripcion { get; set; }

        public string ModoTexto { get; set; }

        public string CampoBaseDatosRPI { get; set; }

        public bool Obligatorio { get; set; }

        public string ExpresionRegular { get; set; }

        public int IdTipoVariableFox { get; set; }
        public string TipoVariableFox { get; set; }

    }
}
