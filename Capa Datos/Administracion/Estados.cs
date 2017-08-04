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

            sql_query = " SELECT GE.[id_estado] "+
                " ,CASE GE.tipo_tramite when '1' then 'Marcas' when '2' then 'Patentes' ELSE 'Derechos de Autor' END as tipo_tramite "+
                " ,GF.nombre "+
                " ,GE.[codigo_estado] "+
                " ,GE.[descripcion],GE.[dias_max] "+
                " ,GE.[dias_min] "+
                " FROM G_Estados GE, G_Formularios GF "+
                " where GE.estado = 'A' "+
                " and GE.no_formulario = GF.no_formulario "+
                " order by GE.tipo_tramite, GE.no_formulario, GE.codigo_estado  ";

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
                " ,[fecha_modificacion],[estado], [id_estadoAnterior], [codigo_anterior] " +
                " ,[id_estadoSiguiente], [codigo_siguiente], [no_Formulario]) " +
                " VALUES " +
                " (@tipo_tramite,@codigo_estado,@descripcion " +
                " ,@dias_max,@dias_min,@fecha_creacion " +
                " ,@fecha_modificacion, @estado, @id_estadoAnterior, @codigo_anterior" +
                " ,@id_estadoSiguiente, @codigo_siguiente, @no_Formulario); "+
                " SELECT SCOPE_IDENTITY();";

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
                command.Parameters.AddWithValue("id_estadoAnterior", objCEEstado.ID_EstadoAnterior);
                command.Parameters.AddWithValue("codigo_anterior", objCEEstado.EstadoAnterior);
                command.Parameters.AddWithValue("id_estadoSiguiente", objCEEstado.ID_EstadoSiguiente);
                command.Parameters.AddWithValue("codigo_siguiente", objCEEstado.EstadoSiguiente);
                command.Parameters.AddWithValue("no_Formulario", objCEEstado.NoFormulario);
                command.Parameters.AddWithValue("estado", "A");

                cn.Open();
                int id_estado = 0;
                id_estado = Convert.ToInt32(command.ExecuteScalar());


                if (objCEEstado.ID_EstadoAnterior > 0)
                {
                    sql_query = " update g_estados "+
                        " set "+
                        " codigo_siguiente = @codigo_siguiente, "+
                        " id_estadoSiguiente = @id_estadoSiguiente "+
                        " where id_estado = @id_estadoAnterior; ";

                    command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("id_estadoAnterior", objCEEstado.ID_EstadoAnterior);
                    command.Parameters.AddWithValue("codigo_siguiente", objCEEstado.Codigo_Estado);
                    command.Parameters.AddWithValue("id_estadoSiguiente", id_estado);
                    command.ExecuteNonQuery();
                }

                respuesta = true;
            }



            return respuesta;
        }

        public DataTable SelectEstado(int id_estado)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT " +
            " [tipo_tramite],[codigo_estado],[no_formulario] " +
            " ,[id_estadoAnterior],[codigo_anterior],[id_estadoSiguiente], [codigo_siguiente] " +
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
            " SET  " +            
            " [descripcion] = @descripcion " +
            " ,[dias_max] = @dias_max " +
            " ,[dias_min] = @dias_min " +
            " ,[fecha_modificacion] = @fecha_modificacion " +
            " WHERE id_estado = @id_estado ";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
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

            using (var cn = objConexion.Conectar())
            {
                /*Selecciono estado anterior y siguiente del estado a borrar*/
                sql_query = " select id_estadoAnterior, id_estadoSiguiente " +
                    " from g_Estados " +
                    " where " +
                    " id_estado = @id_estado;  ";

                var command = new SqlCommand(sql_query, cn);                
                command.Parameters.AddWithValue("id_estado", id_estado);
                cn.Open();
                var da = new SqlDataAdapter(command);
                var dt = new DataTable();
                da.Fill(dt);
                var row = dt.Rows[0];

                /*Actualizo estado siguiente del estado anterior del estado actual*/
                sql_query = " UPDATE G_Estados " +
                " SET [id_estadoSiguiente] = @id_estadoSiguiente " +                
                " WHERE id_estado = @id_estado ";

                command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("id_estadoSiguiente", row["id_estadoSiguiente"]);
                command.Parameters.AddWithValue("id_estado", row["id_estadoAnterior"]);
                command.ExecuteScalar();

                /*Actualizo Estabo a Borrar*/
                sql_query = " UPDATE G_Estados " +
                " SET [estado] = @estado " +
                " ,[fecha_modificacion] = @fecha_modificacion " +
                " WHERE id_estado = @id_estado ";

                command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("estado", "B");
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("id_estado", id_estado);

                
                command.ExecuteScalar();
                respuesta = true;

            }


            return respuesta;
        }

        public DataTable SelectEstadosTipoTramite(int tipoTramite, int noFormulario)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT [id_estado] " +
                " , descripcion " +                
                " FROM G_Estados " +
                " where estado = 'A' "+
                " and tipo_tramite = @tipo_tramite "+
                " and no_formulario = @noFormulario "+
                " order by codigo_estado  ";

            using (var cn = objConexion.Conectar())
            {
                try
                {
                    var command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("tipo_tramite", tipoTramite);
                    command.Parameters.AddWithValue("noFormulario", noFormulario);
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

        public Boolean ExisteEstado(int tipoTramite, int noFormulario)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " select coalesce(count(1),0) as existe "+
                " from g_estados "+
                " where tipo_tramite = @tipo_tramite and no_formulario = @no_formulario; ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("tipo_tramite", tipoTramite);
                command.Parameters.AddWithValue("no_formulario", noFormulario);
                con.Open();

                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    respuesta = true;
                }
            }

            return respuesta;
        }

        public int SelectCodigoEstado(int id_estado)
        {
            var respuesta = 0;
            var sql_query = string.Empty;

            sql_query = " select codigo_estado " +
                " from g_estados " +
                " where id_estado = @id_estado; ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("id_estado", id_estado);
                con.Open();

                respuesta = Convert.ToInt32(command.ExecuteScalar());
            }

            return respuesta;
        }

        public DataTable SelectFormularios(int tipoTramite)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select no_formulario, nombre "+
                " from g_formularios "+
                " where tipo_tramite = @tipo_tramite "+
                " and estado = 'A'  ";

            using (var cn = objConexion.Conectar())
            {
                try
                {
                    var command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("tipo_tramite", tipoTramite);
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

        public Boolean ExisteCodigoEstado(int tipoTramite, int noFormulario, int codigoEstado)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " select coalesce(count(1),0) as existe " +
                " from g_estados " +
                " where tipo_tramite = @tipo_tramite and no_formulario = @no_formulario and codigo_estado = @codigo_estado; ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("tipo_tramite", tipoTramite);
                command.Parameters.AddWithValue("no_formulario", noFormulario);
                command.Parameters.AddWithValue("codigo_estado", codigoEstado);
                con.Open();

                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    respuesta = true;
                }
            }

            return respuesta;
        }

    }
}
