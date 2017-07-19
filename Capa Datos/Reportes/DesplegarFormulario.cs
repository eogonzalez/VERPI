using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Capa_Datos.Reportes
{
    public class DesplegarFormulario
    {
        General.Conexion objConexion = new General.Conexion();

        public DataTable SelectValoresFormularioReporte(int no_preingreso, int no_formulario)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select correlativo_campo, nombre_control, valor "+
                " from preingreso_detalle "+
                "where no_preingreso = @no_preingreso "+
                " union "+
                " (select mcf.correlativo_campo,mcf.nombre_control, "+
                " case  when pia.correlativo_campo is NULL then 'False' else 'True' end as valor "+
                " from m_campos_formulario mcf "+
                " left join "+
                " preingreso_adjunto pia "+
                " on "+
                " pia.correlativo_campo = mcf.correlativo_campo "+
                " and pia.no_preingreso = @no_preingreso "+
                " where mcf.no_formulario = @no_formulario "+
                " and mcf.tipo_control = 3 "+
                " and mcf.estado = 'A') "+
                " order by correlativo_campo; ";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("no_preingreso", no_preingreso);
                command.Parameters.AddWithValue("no_formulario", no_formulario);
                conn.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);

            }

            return dt_respuesta;
        }

        public int SelectTipoCampo(int correlativo_campo)
        {
            var respuesta = 0;
            var sql_query = string.Empty;

            sql_query = " select tipo_control " +
                " from M_Campos_Formulario "+
                " where correlativo_campo = @correlativo_campo ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("correlativo_campo", correlativo_campo);
                con.Open();
                respuesta = (int)command.ExecuteScalar();
            }

            return respuesta;
        }

        public string SelectValorCombo(int correlativo_campo, string valor)
        {
            string respuesta = string.Empty;
            string sql_query = string.Empty;

            sql_query = "select texto "+
                " from m_valor_combo "+
                " where correlativo_campo = @correlativo_campo and valor = @valor ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("correlativo_campo", correlativo_campo);
                command.Parameters.AddWithValue("valor", valor);
                con.Open();
                respuesta = command.ExecuteScalar().ToString();
            }


            return respuesta;
        }

        public string SelectValorComboPais(int valor_pais)
        {
            string respuesta = string.Empty;
            string sql_query = string.Empty;

            sql_query = " select nombre "+
                " from g_Paises "+
                " where id_pais = @id_pais; ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("id_pais", valor_pais);
                con.Open();
                respuesta = command.ExecuteScalar().ToString();
            }

            return respuesta;
        }

        public string SelectValorComboNiza(int valor_niza)
        {
            string respuesta = string.Empty;
            string sql_query = string.Empty;

            sql_query = " select niza_descripcion as nombre "+
                " from G_Clase_Niza "+
                " where niza_id = @valor_niza; ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("valor_niza", valor_niza);
                con.Open();
                respuesta = command.ExecuteScalar().ToString();
            }

            return respuesta;
        }
    }
}
