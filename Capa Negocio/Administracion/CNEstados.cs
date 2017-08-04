using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.Administracion;
using Capa_Entidad.Administracion;
using System.Data;

namespace Capa_Negocio.Administracion
{
    public class CNEstados
    {
        Estados objCDEstados = new Estados();
        public DataTable SelectEstadosTiempos()
        {
            return objCDEstados.SelectEstadosTiempos();
        }

        public bool InsertEstado(CEEstados objCEEstado)
        {
            return objCDEstados.InsertEstado(objCEEstado);
        }

        public DataTable SelectEstado(int id_estado)
        {
            return objCDEstados.SelectEstado(id_estado);
        }

        public Boolean UpdateEstado(CEEstados objCEEstado)
        {
            return objCDEstados.UpdateEstado(objCEEstado);
        }

        public Boolean DeleleEstado(int id_estado)
        {
            return objCDEstados.DeleleEstado(id_estado);
        }

        public DataTable SelectEstadosTipoTramite(int tipoTramite, int noFormulario)
        {
            return objCDEstados.SelectEstadosTipoTramite(tipoTramite, noFormulario);
        }

        public Boolean ExisteEstado(int tipoTramite, int noFormulario)
        {
            return objCDEstados.ExisteEstado(tipoTramite, noFormulario);
        }

        public int SelectCodigoEstado(int id_estado)
        {
            return objCDEstados.SelectCodigoEstado(id_estado);
        }

        public DataTable SelectFormularios(int tipoTramite)
        {
            return objCDEstados.SelectFormularios(tipoTramite);
        }

        public Boolean ExisteCodigoEstado(int tipoTramite, int noFormulario, int codigoEstado)
        {
            return objCDEstados.ExisteCodigoEstado(tipoTramite, noFormulario, codigoEstado);
        }
    }
}
