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
    public class Formularios
    {
        General.Conexion objConexion = new General.Conexion();

        public DataTable SelectFormularios()
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT [no_formulario] "+
                " ,[tipo_tramite] "+
                " ,[nombre] "+
                " ,case tipo_tramite when 1 then 'Marcas' when 2 then 'Patentes' else 'Derechos de Autor' end as nombre_tipo "+
                " ,[descripcion_formulario] " +
                " FROM[G_Formularios] "+
                " WHERE estado = 'A' ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                con.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }

            return dt_respuesta;
        }

        public bool InsertFormulario(CEFormularios objCEFormulario)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " INSERT INTO [dbo].[G_Formularios] "+
                " ([tipo_tramite],[nombre] "+
                " ,[descripcion_formulario],[fecha_creacion] "+
                ",[fecha_modificacion],[estado], [path_reporte], [tipo_listado]) "+
                " VALUES "+
                " (@tipo_tramite, @nombre "+
                " , @descripcion_formulario , @fecha_creacion "+
                " , @fecha_modificacion, @estado, @path_reporte, @tipo_listado)";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("tipo_tramite", objCEFormulario.TipoTramite);
                command.Parameters.AddWithValue("nombre", objCEFormulario.NombreFormulario);
                command.Parameters.AddWithValue("descripcion_formulario", objCEFormulario.Descripcion);
                command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("path_reporte", objCEFormulario.Path_Reporte);
                command.Parameters.AddWithValue("tipo_listado", objCEFormulario.TipoLista);
                command.Parameters.AddWithValue("estado", "A");

                try
                {
                    con.Open();
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

        public DataTable SelectFormulario(int no_formulario)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = "select tipo_tramite, nombre, descripcion_formulario, path_reporte, tipo_listado " +
                " from G_Formularios " +
                " where no_formulario = @no_formulario;";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("no_formulario", no_formulario);
                con.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }



            return dt_respuesta;
        }

        public bool DeleteFormulario(int no_formulario)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE [dbo].[G_Formularios] "+
                " SET "+
                " [fecha_modificacion] = @fecha_modificacion "+
                " ,[estado] = @estado "+
                " WHERE no_formulario =  @no_formulario ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("estado", "B");
                command.Parameters.AddWithValue("no_formulario", no_formulario);
                con.Open();
                command.ExecuteNonQuery();
                respuesta = true;
            }


            return respuesta;
        }

        public bool UpdateFormulario(CEFormularios objCEFormulario)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE [dbo].[G_Formularios] " +
                " SET " +
                " [tipo_tramite] = @tipo_tramite, "+
                " [nombre] = @nombre, "+                
                " [descripcion_formulario] = @descripcion_formulario,"+
                " [path_reporte] = @path_reporte, " +
                " [tipo_listado] = @tipo_listado, " +
                " [fecha_modificacion] = @fecha_modificacion " +                
                " WHERE no_formulario =  @no_formulario ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("tipo_tramite", objCEFormulario.TipoTramite);
                command.Parameters.AddWithValue("nombre", objCEFormulario.NombreFormulario);
                command.Parameters.AddWithValue("descripcion_formulario", objCEFormulario.Descripcion);
                command.Parameters.AddWithValue("path_reporte", objCEFormulario.Path_Reporte);
                command.Parameters.AddWithValue("tipo_listado", objCEFormulario.TipoLista);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);                
                command.Parameters.AddWithValue("no_formulario", objCEFormulario.No_Formulario);
                con.Open();
                command.ExecuteNonQuery();
                respuesta = true;
            }


            return respuesta;
        }
    }
}
