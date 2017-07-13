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
    }
}
