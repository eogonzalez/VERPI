using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Capa_Datos.General;
using Capa_Entidad.General;

namespace Capa_Negocio.General
{
    public class CNFormularios
    {
        Formularios objCDFormulario = new Formularios();

        public DataTable SelectCamposFormulario(CEFormularios objCEFormulario)
        {
            return objCDFormulario.SelectCamposFormulario(objCEFormulario);
        }

        public DataTable SelectCantidadCamposFormulario(int no_formulario)
        {
            return objCDFormulario.SelectCantidadCamposFormulario(no_formulario);
        }

        public DataTable SelectFormularios(int tipo_tramite)
        {
            return objCDFormulario.SelectFormularios(tipo_tramite);
        }

        public int InsertDatosFormularioBorrador(CEFormularios objCEFormulario)
        {
            return objCDFormulario.InsertDatosFormularioBorrador(objCEFormulario);
        }

        public DataTable SelectDatosCombos(int correlativo_campo, int tipo)
        {
            return objCDFormulario.SelectDatosCombos(correlativo_campo, tipo);
        }

        public bool UpdateDatosFormularioBorrador(CEFormularios objCEFormulario)
        {
            return objCDFormulario.UpdateDatosFormularioBorrador(objCEFormulario);
        }

        public DataTable SelectAnexosFormulario(int no_preingreso)
        {
            return objCDFormulario.SelectAnexosFormulario(no_preingreso);
        }

        public bool InsertDoctoAnexoFormulario(CEFormularios objCEFormulario)
        {
            return objCDFormulario.InsertDoctoAnexoFormulario(objCEFormulario);
        }

        public DataTable SelectValoresFormulario(int no_preingreso)
        {
            return objCDFormulario.SelectValoresFormulario(no_preingreso);
        }

        public bool EliminaFormulario(int no_preingreso)
        {
            return objCDFormulario.EliminaFormulario(no_preingreso);
        }

        public bool ExisteArchivo(int no_preingreso, int correlativo_campo)
        {
            return objCDFormulario.ExisteArchivo(no_preingreso, correlativo_campo);
        }

        public bool UpdateDoctoAnexoFormulario(CEFormularios objCEFormulario)
        {
            return objCDFormulario.UpdateDoctoAnexoFormulario(objCEFormulario);
        }

        public bool EliminoArchivoFormulario(int correlativo_adjunto, string path)
        {
            return objCDFormulario.EliminoArchivoFormulario(correlativo_adjunto, path);
        }

        public DataTable SelectCamposObligatorios(int no_formulario)
        {
            return objCDFormulario.SelectCamposObligatorios(no_formulario);
        }

        public int GenerarExpediente(int no_preingreso)
        {
            return objCDFormulario.GenerarExpediente(no_preingreso);
        }

    }
}
