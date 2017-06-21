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
    public class TipoUsuarios
    {
        General.Conexion objConexion = new General.Conexion();
        

        public DataSet SelectTipoUsuarios()
        {
            DataSet ds_usuarios = new DataSet();
            SqlDataAdapter da_usuarios;

            string sql_query = string.Empty;

            using (SqlConnection cn = objConexion.Conectar())
            {
                sql_query = " SELECT id_tipousuario " +
                    " ,nombre ,descripcion,tipo_permiso " +
                    " ,fecha_creacion " +
                    " FROM G_TipoUsuario " +
                    " where estado = 'A' ";
                try
                {
                    SqlCommand command = new SqlCommand(sql_query, cn);
                    da_usuarios = new SqlDataAdapter(command);

                    da_usuarios.Fill(ds_usuarios);
                }
                catch (Exception)
                {

                    throw;
                }
            }


            return ds_usuarios;
        }

        public Boolean InsertTipoUsuarios(CETipoUsuarios objetoEntidad)
        {
            Boolean respuesta = false;

            string sql_query = string.Empty;

            try
            {
                sql_query = " INSERT INTO G_TipoUsuario "+
                    " ([nombre],[descripcion], [fecha_creacion] "+
                    " ,[fecha_modificacion],[estado], [id_usuarioAutoriza]) " +
                    " VALUES "+
                    " (@nombre,@descripcion "+
                    " ,@fecha_creacion,@fecha_modificacion"+
                    " ,@estado, @id_usuarioAutoriza) ";

                using (SqlConnection cn = objConexion.Conectar())
                {
                    SqlCommand command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("nombre", objetoEntidad.Nombre);
                    command.Parameters.AddWithValue("descripcion", objetoEntidad.Descripcion);
                    //command.Parameters.AddWithValue("tipo_permiso", objetoEntidad.TipoPermiso);
                    command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                    command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                    command.Parameters.AddWithValue("estado", "A");
                    command.Parameters.AddWithValue("id_usuarioAutoriza", objetoEntidad.ID_UsuarioAutoriza);

                    cn.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        respuesta = true;
                    }
                    else
                    {
                        respuesta = false;
                    }
                    
                }
            }
            catch (Exception)
            {
                //mensaje("TU001 - Ha ocurrido un error al almacenar tipo de usuario.")

                respuesta = false;
                
            }


            return respuesta;
        }

        public DataTable SelectTipoUsuario(int id_tipousuario)
        {
            var dt_respuesta = new DataTable();
            string sql_query = string.Empty;

            sql_query = " SELECT " + 
                " nombre ,descripcion,tipo_permiso " +
                " FROM G_TipoUsuario " +
                " where estado = 'A' "+
                " AND id_tipousuario = @id_tipousuario ";

            using (var cn = objConexion.Conectar())
            {
                try
                {
                    var command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("id_tipousuario", id_tipousuario);
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

        public Boolean UpdateTipoUsuario(CETipoUsuarios objTipoUsuario)
        {
            var respuesta = false;
            string sql_query = string.Empty;

            sql_query = " UPDATE G_TipoUsuario "+
                " SET [nombre] = @nombre "+
                " ,[descripcion] = @descripcion "+
                //" ,[tipo_permiso] = @tipo_permiso "+
                " ,[fecha_modificacion] = @fecha_modificacion "+
                " ,[id_usuarioAutoriza] = @id_usuarioAutoriza "+
                " WHERE id_tipousuario = @id_tipousuario ";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("nombre", objTipoUsuario.Nombre);
                command.Parameters.AddWithValue("descripcion", objTipoUsuario.Descripcion);
                //command.Parameters.AddWithValue("tipo_permiso", objTipoUsuario.TipoPermiso);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("id_usuarioAutoriza", objTipoUsuario.ID_UsuarioAutoriza);
                command.Parameters.AddWithValue("id_tipousuario", objTipoUsuario.ID_TipoUsuario);

                try
                {
                    cn.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        respuesta = true;
                    }
                    else
                    {
                        respuesta = false;
                    }

                }
                catch (Exception)
                {
                    
                    throw;
                }
            }


            return respuesta;
        }

        public Boolean DeleteTipoUsuario(CETipoUsuarios objTipoUsuario)
        {
            Boolean respuesta = false;
            string sql_query = string.Empty;

            sql_query = "UPDATE G_TipoUsuario " +
                " SET [estado] = @estado " +
                " ,[id_usuarioAutoriza] =@id_usuarioAutoriza "+
                " ,[fecha_modificacion] = @fecha_modificacion " +
                " WHERE id_tipousuario = @id_tipousuario";

            using (SqlConnection cn = objConexion.Conectar())
            {
                SqlCommand command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("estado", 'B');
                command.Parameters.AddWithValue("id_usuarioAutoriza", objTipoUsuario.ID_UsuarioAutoriza);
                command.Parameters.AddWithValue("id_tipousuario", objTipoUsuario.ID_TipoUsuario);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }
            return respuesta;
        }
    }
}
