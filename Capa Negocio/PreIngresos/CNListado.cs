using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.PreIngresos;
using System.Data;
using Capa_Entidad.PreIngresos;

namespace Capa_Negocio.PreIngresos
{
    public class CNListado
    {
        Listado objCDListado = new Listado();
        public DataTable SelectListadoGridView(int tipo_listado, int no_preingreso)
        {
            return objCDListado.SelectListadoGridView(tipo_listado, no_preingreso);
        }

        public bool InsertElementoLista(CEListado objCELista)
        {
            return objCDListado.InsertElementoLista(objCELista);
        }

        public bool UpdateMantenimientoRegistro(CEListado objCELista)
        {
            return objCDListado.UpdateMantenimientoRegistro(objCELista);
        }

        public DataTable SelectElementoLista(CEListado objCELista)
        {
            return objCDListado.SelectElementoLista(objCELista);
        }

        public bool DeleteElementoLista(CEListado objCELista)
        {
            return objCDListado.DeleteElementoLista(objCELista);
        }
    }
}
