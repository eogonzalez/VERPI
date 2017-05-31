using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Capa_Entidad.Administracion;

namespace Capa_Datos.Administracion
{
    public class ValoresCombo
    {
        General.Conexion objConexion = new General.Conexion();

        public DataTable SelectValoresCombo(int correlativo_campo)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT "+
                " [id_valor_combo],[Texto],[valor] "+
                " FROM[dbo].[M_Valor_Combo] "+
                " where correlativo_campo = @correlativo_campo ";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("correlativo_campo", correlativo_campo);
                conn.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }


            return dt_respuesta;
        }

        public DataTable SelectValorCombo(int id_valor_combo)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT " +
                " [Texto],[valor] " +
                " FROM [M_Valor_Combo] " +
                " where id_valor_combo = @id_valor_combo ";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("id_valor_combo", id_valor_combo);
                conn.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }


            return dt_respuesta;
        }

        public bool DeleteValorCombo(int id_valor_combo)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " DELETE FROM [dbo].[M_Valor_Combo] "+
                " WHERE id_valor_combo = @id_valor_combo; ";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("id_valor_combo", id_valor_combo);
                
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception)
                {

                    throw;
                }

            }

            return respuesta;
        }

        public bool InsertValorCombo(CEValoresCombo objCEValores)
        {
            var respuesta = false;
            var sql_query = string.Empty;


            sql_query = " INSERT INTO [dbo].[M_Valor_Combo] "+
                " ([correlativo_campo],[Texto],[valor]) "+
                " VALUES "+                
                " (@correlativo_campo, @Texto, @valor);";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("correlativo_campo", objCEValores.Correlativo_Campo);
                command.Parameters.AddWithValue("Texto", objCEValores.Texto);
                command.Parameters.AddWithValue("valor", objCEValores.Valor);

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception)
                {

                    throw;
                }
            }


            return respuesta;
        }

        public bool UpdateValorCombo(CEValoresCombo objCEvalores)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE [dbo].[M_Valor_Combo] "+
                " SET "+
                " [Texto] = @Texto "+
                " ,[valor] = @valor "+
                " WHERE id_valor_combo = @id_valor_combo";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("Texto", objCEvalores.Texto);
                command.Parameters.AddWithValue("valor", objCEvalores.Valor);
                command.Parameters.AddWithValue("id_valor_combo", objCEvalores.ID_Valor_Combo);

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return respuesta;
        }
    }
}
