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

        public bool UpdateUsuario(CEUsuario objCEUsuario)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE g_usuarios "+
                " SET[nombres] = @nombres "+
                " ,[apellidos] = @apellidos "+
                " ,[cui] = @cui "+
                " ,[telefono] = @telefono "+
                " ,[direccion] = @direccion "+
                " ,[id_usuarioAutoriza] = @id_usuarioAutoriza "+
                " WHERE id_usuario = @id_usuario; "+
                " "+
                " UPDATE G_UsuarioPermiso "+
                " SET [id_tipousuario] = @id_tipousuario "+
                " ,[fecha_modificacion] = @fecha_modificacion "+
                " ,[id_usuarioAutoriza] = @id_usuarioAutoriza "+
                " WHERE id_usuario = @id_usuario ";

            using (var conn = objCDConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("nombres", objCEUsuario.CE_Nombres);
                command.Parameters.AddWithValue("apellidos", objCEUsuario.CE_Apellidos);
                command.Parameters.AddWithValue("cui", objCEUsuario.CE_CUI);
                command.Parameters.AddWithValue("telefono", objCEUsuario.CE_Telefono);
                command.Parameters.AddWithValue("direccion", objCEUsuario.CE_Direccion);
                command.Parameters.AddWithValue("id_usuarioAutoriza", objCEUsuario.ID_UsuarioAutoriza);
                command.Parameters.AddWithValue("id_usuario", objCEUsuario.ID_Usuario);
                command.Parameters.AddWithValue("id_tipousuario", objCEUsuario.ID_TipoUsuario);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception)
                {
                    respuesta = false;
                    throw;
                }

            }

            return respuesta;
        }

        public Boolean RegistrarUsuario(CEUsuario objCEUsuario)
        {
            //Declaracion de variables
            var estado = false;
            string sql_query = string.Empty;

            //Iniciamos proceso de conexion con db
            using (var conn = objCDConexion.Conectar())
            {
                conn.Open();
                var command = conn.CreateCommand();
                SqlTransaction transaccion;

                //Iniciar Transaccion
                transaccion = conn.BeginTransaction("RegistrarUsuario");

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
                    command.Parameters.AddWithValue("@id_usuarioAutoriza", '0');

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
                    command.Parameters.AddWithValue("@id_tipousuario", '2');
                    command.Parameters.AddWithValue("@fecha_creacion_permiso", DateTime.Now);
                    command.Parameters.AddWithValue("@fecha_modificacion_permiso", DateTime.Now);
                    command.Parameters.AddWithValue("@estado_permiso", "A");
                    command.Parameters.AddWithValue("@id_usuarioAutoriza_permiso", '0');
                    command.ExecuteNonQuery();

                    transaccion.Commit();
                    estado = true;


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
                        estado = false;
                        //Manejo Segunda Excepcion
                        throw;
                    }
                    estado = false;
                    throw;
                }

            }

            return estado;
        }

        public DataTable SelectDatosUsuario(int idUsuario)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select nombres, apellidos, correo " +
                " from g_usuarios " +
                " where id_usuario = @id_usuario ";

            using (var cn = objCDConexion.Conectar())
            {
                try
                {
                    var command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("id_usuario", idUsuario);
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

        public Boolean UpdateContraseña(string correo, string contraseña)
        {
            //Declaracion de variables
            var respuesta = false;


            //Iniciamos proceso de conexion con db
            using (var conn = objCDConexion.Conectar())
            {
                conn.Open();
                var command = conn.CreateCommand();
                SqlTransaction transaccion;

                //Iniciar Transaccion
                transaccion = conn.BeginTransaction("ActualizoContrasenia");

                command.Connection = conn;
                command.Transaction = transaccion;

                try
                {
                    /*
                        Query para registrar usuario
                    */
                    command.CommandText = " UPDATE G_Usuarios " +
                    " SET " +
                    " [password] = @password " +
                    " WHERE correo = @correo; ";

                    command.Parameters.AddWithValue("@correo", correo);

                    //Encriptamos la contrasenia
                    General encript = new General();
                    string hash = encript.EncodePassword(correo + contraseña);
                    command.Parameters.AddWithValue("@password", hash);

                    command.ExecuteNonQuery();

                    /*
                        Query para asignar permiso de usuario externo
                    */

                    command.CommandText = "DELETE FROM G_UsuarioRecupera " +
                        " WHERE correo = @correo_recupera ";

                    command.Parameters.AddWithValue("@correo_recupera", correo);
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

        public Boolean UpdateContraseña(int id_usuario, string correo, string contraseña)
        {
            var respuesta = false;

            //Iniciamos proceso de conexion con db
            using (var conn = objCDConexion.Conectar())
            {
                conn.Open();
                var command = conn.CreateCommand();
                SqlTransaction transaccion;

                //Iniciar Transaccion
                transaccion = conn.BeginTransaction("ActualizoContrasenia");

                command.Connection = conn;
                command.Transaction = transaccion;

                try
                {
                    /*
                        Query para registrar usuario
                    */
                    command.CommandText = " UPDATE G_Usuarios " +
                    " SET " +
                    " [password] = @password " +
                    " WHERE id_usuario = @id_usuario; ";

                    command.Parameters.AddWithValue("@id_usuario", id_usuario);

                    //Encriptamos la contrasenia
                    General encript = new General();
                    string hash = encript.EncodePassword(correo + contraseña);
                    command.Parameters.AddWithValue("@password", hash);

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

        public bool DeleteUsuario(CEUsuario objCEUsuario)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE g_usuarios "+
                " SET "+
                " [estado] = @estado "+
                " ,[id_usuarioAutoriza] = @id_usuarioAutoriza "+
                " WHERE id_usuario = @id_usuario;"+
                " "+
                " UPDATE G_UsuarioPermiso "+
                " SET "+
                " [fecha_modificacion] = @fecha_modificacion "+
                " ,[estado] = @estado "+
                " ,[id_usuarioAutoriza] = @id_usuarioAutoriza "+
                " WHERE id_usuario = @id_usuario ";

            using (var conn = objCDConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("estado", "B");
                command.Parameters.AddWithValue("id_usuarioAutoriza", objCEUsuario.ID_UsuarioAutoriza);
                command.Parameters.AddWithValue("id_usuario", objCEUsuario.ID_Usuario);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception)
                {
                    respuesta = false;
                    throw;
                }
            }

            return respuesta;
        }
    }
}
