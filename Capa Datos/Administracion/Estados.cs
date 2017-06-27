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
    public class Estados
    {

        General.Conexion objConexion = new General.Conexion();

        public DataTable SelectEstadosTiempos()
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT [id_estado] " +
                " ,CASE tipo_tramite when '1' then 'Marcas' when '2' then 'Patentes' ELSE 'Derechos de Autor' END	 as tipo_tramite " +
                " ,[codigo_estado] " +
                " ,[descripcion],[dias_max] " +
                " ,[dias_min] " +
                " FROM G_Estados " +
                " where estado = 'A' ";

            using (var cn = objConexion.Conectar())
            {
                try
                {
                    var command = new SqlCommand(sql_query, cn);
                    var da = new SqlDataAdapter(command);
                    da.Fill(dt_respuesta);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return dt_respuesta;
        }

        public bool InsertEstado(CEEstados objCEEstado)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " INSERT INTO G_Estados " +
                " ([tipo_tramite],[codigo_estado],[descripcion] " +
                " ,[dias_max],[dias_min],[fecha_creacion] " +
                " ,[fecha_modificacion],[estado]) " +
                " VALUES " +
                " (@tipo_tramite,@codigo_estado,@descripcion " +
                " ,@dias_max,@dias_min,@fecha_creacion " +
                " ,@fecha_modificacion,@estado)";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("tipo_tramite", objCEEstado.TipoTramite);
                command.Parameters.AddWithValue("codigo_estado", objCEEstado.Codigo_Estado);
                command.Parameters.AddWithValue("descripcion", objCEEstado.Descripcion);
                command.Parameters.AddWithValue("dias_max", objCEEstado.Dias_Max);
                command.Parameters.AddWithValue("dias_min", objCEEstado.Dias_Min);
                command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("estado", "A");

                cn.Open();
                command.ExecuteNonQuery();
                respuesta = true;
            }



            return respuesta;
        }

        public DataTable SelectEstado(int id_estado)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT " +
            " [tipo_tramite],[codigo_estado] " +
            " ,[descripcion],[dias_max],[dias_min] " +
            " FROM G_Estados " +
            " where id_estado = @id_estado ";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("id_estado", id_estado);

                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }

            return dt_respuesta;
        }

        public Boolean UpdateEstado(CEEstados objCEEstado)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE G_Estados " +
            " SET [tipo_tramite] = @tipo_tramite " +
            " ,[codigo_estado] = @codigo_estado " +
            " ,[descripcion] = @descripcion " +
            " ,[dias_max] = @dias_max " +
            " ,[dias_min] = @dias_min " +
            " ,[fecha_modificacion] = @fecha_modificacion " +
            " WHERE id_estado = @id_estado ";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("tipo_tramite", objCEEstado.TipoTramite);
                command.Parameters.AddWithValue("codigo_estado", objCEEstado.Codigo_Estado);
                command.Parameters.AddWithValue("descripcion", objCEEstado.Descripcion);
                command.Parameters.AddWithValue("dias_max", objCEEstado.Dias_Max);
                command.Parameters.AddWithValue("dias_min", objCEEstado.Dias_Min);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("id_estado", objCEEstado.ID_Estado);

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;

            }


            return respuesta;
        }

        public Boolean DeleleEstado(int id_estado)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE G_Estados " +
            " SET [estado] = @estado " +
            " ,[fecha_modificacion] = @fecha_modificacion " +
            " WHERE id_WF_Tiempos = @id_estado ";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);

                command.Parameters.AddWithValue("estado", "A");
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("id_estado", id_estado);

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;

            }


            return respuesta;
        }

    }
}
