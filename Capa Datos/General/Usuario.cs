using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Capa_Entidad.General;

namespace Capa_Datos.General
{
    public class Usuario
    {
        Conexion objCDConexion = new Conexion();
        

        public DataTable SelectUsuarios()
        {
            var dt_respuesta = new DataTable();                        
            var sql_query = string.Empty;

            using (var cn = objCDConexion.Conectar())
            {
                sql_query = " SELECT gu.id_usuario ,gu.nombres ,gu.apellidos ,gu.cui " +
                    " ,gu.correo,gu.fecha_registro, gtu.nombre as permiso " +
                    " FROM g_usuarios gu, G_UsuarioPermiso gup, G_TipoUsuario gtu  " +
                    " where " +
                    " gu.estado = 'A' " +
                    " and gu.id_usuario = gup.id_usuario " +
                    " and gup.id_tipousuario = gtu.id_tipousuario ";
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

        public DataTable SelectComboPerfiles()
        {
            var respuesta = new DataTable();

            string sql_query = string.Empty;


            using (SqlConnection cn = objCDConexion.Conectar())
            {
                try
                {

                    sql_query = " SELECT [id_tipousuario] " +
                        " ,[nombre] " +
                        " FROM G_TipoUsuario " +
                        " where estado = 'A'; ";

                    var command = new SqlCommand(sql_query, cn);
                    SqlDataAdapter da = new SqlDataAdapter(command);

                    da.Fill(respuesta);

                }
                catch (Exception)
                {

                    throw;
                }

            }

            return respuesta;
        }

        //Funcion para obtener el id del usuario que hizo login
        public int ConsultaUsuarioId(string correo)
        {
            int respuesta = 0;
            var sql_query = string.Empty;

            sql_query = "  SELECT id_usuario FROM g_usuarios WHERE correo = @correo ";

            using (var conexion = objCDConexion.Conectar())
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql_query, conexion);
                    command.Parameters.AddWithValue("@correo", correo);
                    conexion.Open();
                    respuesta = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return respuesta;
        }

        public bool GuardarUsuario(CEUsuario objCEUsuario)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            //Iniciamos proceso de conexion con db
            using (var conn = objCDConexion.Conectar())
            {
                conn.Open();
                var command = conn.CreateCommand();
                SqlTransaction transaccion;

                //Iniciar Transaccion
                transaccion = conn.BeginTransaction("GuardarUsuario");

                command.Connection = conn;
                command.Transaction = transaccion;

                try
                {
                    /*
                        Query para registrar usuario
                    */
                    command.CommandText = " INSERT INTO g_usuarios " +
                        " (nombres,apellidos,cui,telefono " +
                        " ,direccion,correo " +
                        " ,password,fecha_registro,estado " +
                        " ,id_usuarioAutoriza) " +
                        " VALUES " +
                        " (@nombres,@apellidos,@cui,@telefono " +
                        " ,@direccion,@correo " +
                        " ,@password,@fecha_registro,@estado " +
                        " ,@id_usuarioAutoriza); " +
                        " SELECT SCOPE_IDENTITY(); ";

                    command.Parameters.AddWithValue("@nombres", objCEUsuario.CE_Nombres);
                    command.Parameters.AddWithValue("@apellidos", objCEUsuario.CE_Apellidos);
                    command.Parameters.AddWithValue("@cui", objCEUsuario.CE_CUI);
                    command.Parameters.AddWithValue("@telefono", objCEUsuario.CE_Telefono);
                    command.Parameters.AddWithValue("@direccion", objCEUsuario.CE_Direccion);
                    command.Parameters.AddWithValue("@correo", objCEUsuario.CE_Correo);
                    command.Parameters.AddWithValue("@fecha_registro", DateTime.Now);
                    command.Parameters.AddWithValue("@estado", 'A');
                    command.Parameters.AddWithValue("@id_usuarioAutoriza", objCEUsuario.ID_UsuarioAutoriza);

                    //Encriptamos la contrasenia
                    General encript = new General();
                    string hash = encript.EncodePassword(objCEUsuario.CE_Correo + objCEUsuario.CE_Password);
                    command.Parameters.AddWithValue("@password", hash);

                    int id_usuario = 0;
                    id_usuario = Convert.ToInt32(command.ExecuteScalar());

                    /*
                        Query para asignar permiso de usuario externo
                    */

                    command.CommandText = "INSERT INTO G_UsuarioPermiso " +
                        " ([id_usuario],[id_tipousuario],[fecha_creacion] " +
                        " ,[fecha_modificacion],[estado],[id_usuarioAutoriza]) " +
                        " VALUES " +
                        " (@id_usuario , @id_tipousuario, @fecha_creacion_permiso " +
                        " , @fecha_modificacion_permiso, @estado_permiso, @id_usuarioAutoriza_permiso);";

                    command.Parameters.AddWithValue("@id_usuario", id_usuario);
                    command.Parameters.AddWithValue("@id_tipousuario", objCEUsuario.ID_TipoUsuario);
                    command.Parameters.AddWithValue("@fecha_creacion_permiso", DateTime.Now);
                    command.Parameters.AddWithValue("@fecha_modificacion_permiso", DateTime.Now);
                    command.Parameters.AddWithValue("@estado_permiso", "A");
                    command.Parameters.AddWithValue("@id_usuarioAutoriza_permiso", objCEUsuario.ID_UsuarioAutoriza);
                    command.ExecuteNonQuery();

                    transaccion.Commit();
                    respuesta = true;


                }
                catch (Exception)
                {
                    //Manejo primera excepcion

                    try
                    {
                        transaccion.Rollback();
                    }
                    catch (Exception)
                    {
                        respuesta = false;
                        //Manejo Segunda Excepcion
                        throw;
                    }
                    respuesta = false;
                    throw;
                }

            }

            return respuesta;
        }

        public DataTable SelectUsuario(int id_usuario)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;
            sql_query = "select gu.nombres, gu.apellidos, gu.cui, " +
                " gu.telefono, gu.direccion, gu.correo, gup.id_tipousuario " +
                " from g_usuarios gu, g_usuariopermiso gup " +
                " where gu.id_usuario = @id_usuario " +
                " and gu.id_usuario = gup.id_usuario; ";

            using (var conn = objCDConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("id_usuario", id_usuario);
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }


            return dt_respuesta;
        }

    }
}
