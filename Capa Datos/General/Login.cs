using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capa_Entidad.General;
using System.Data;

namespace Capa_Datos.General
{
    public class Login
    {

        Conexion objConexion = new Conexion();

        /*Funcion para validar correo y password para ingresar al sistema, para usuarios autorizados*/
        public Boolean AutenticarLogin(string correo, string password)
        {
            //Declaracion de variables
            Boolean estado = false;
            string sql_query = string.Empty;
            int count = 0;
            

            //Iniciamos proceso de conexion con db
            using (SqlConnection cn = objConexion.Conectar())
            {
                //Query de consulta usuario autorizado
                sql_query = " SELECT COUNT(*) " +
                    " FROM g_usuarios "+
                    " WHERE correo = @correo AND password = @password AND estado = 'A' ";
                try
                {
                    
                    SqlCommand command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("@correo", correo);
                    
                    //Encriptamos la contrasenia
                    General encript = new General();
                    string hash = encript.EncodePassword(correo + password);

                    command.Parameters.AddWithValue("@password", hash);

                    cn.Open();
                    count = Convert.ToInt32(command.ExecuteScalar());

                }
                catch (Exception e)
                {
                    
                    Console.WriteLine(e.Message);
                }
            }

            if (count == 0)
            {
                estado = false;
            }
            else
            {
                estado = true;
            }

            return estado;
        }

        //Funcion para validar correo en proceso de registro
        public Boolean AutenticarRegistro(string correo)
        {
            //Declaracion de variables
            Boolean estado = false;
            string sql_query = string.Empty;
            int count = 0;


            //Iniciamos proceso de conexion con db
            using (SqlConnection cn = objConexion.Conectar())
            {
                //Query de consulta usuario autorizado
                sql_query = " SELECT COUNT(*) " +
                    " FROM g_usuarios " +
                    " WHERE correo = @correo ";
                try
                {

                    SqlCommand command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("@correo", correo);
                    
                    cn.Open();
                    count = Convert.ToInt32(command.ExecuteScalar());

                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }

            if (count == 0)
            {
                estado = false;
            }
            else
            {
                estado = true;
            }

            return estado;
        }

        //Funcion para verificar si el usuario esta autorizado para login
        public Boolean AutorizaLogin(string correo)
        {
            //Declaracion de variables
            Boolean estado = false;
            string sql_query = string.Empty;
            int count = 0;


            //Iniciamos proceso de conexion con db
            using (SqlConnection cn = objConexion.Conectar())
            {
                //Query de consulta usuario autorizado
                sql_query = " SELECT COUNT(*) " +
                    " FROM g_usuarios " +
                    " WHERE correo = @correo and estado = 'A' ";
                try
                {

                    SqlCommand command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("@correo", correo);

                    cn.Open();
                    count = Convert.ToInt32(command.ExecuteScalar());

                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }

            if (count == 0)
            {
                estado = false;
            }
            else
            {
                estado = true;
            }

            return estado;
        }

        //La funcion seguridad registra en la tabla g_usuarios_seguridad el ultimo acceso del usuario
        public Boolean Seguridad(int id_usuario , DateTime fecha_acceso, string dir_ip)
        {
            Boolean respuesta = false;
            string sql_query = string.Empty;
            int int_identidad = 0;
            
            sql_query = " INSERT INTO g_usuarios_seguridad(id_usuario, fecha_ultimo_acceso, direccion_ip) "+
                " VALUES(@idUsuario,@fechaAcceso,@dirIP) Select SCOPE_IDENTITY() ";

            using(SqlConnection conexion = objConexion.Conectar())
            {
                try 
                {	        
		            
                    SqlCommand command = new SqlCommand(sql_query, conexion);
                    command.Parameters.AddWithValue("idUsuario", id_usuario);
                    command.Parameters.AddWithValue("fechaAcceso", fecha_acceso);
                    command.Parameters.AddWithValue("dirIP", dir_ip);
                    conexion.Open();
                    int_identidad = Convert.ToInt32(command.ExecuteScalar());

                    if (int_identidad > 0)
	                {
	                    respuesta = true;	 
	                }else
	                {
                        respuesta =  false;
	                }
	            }
	            catch (Exception)
	            {
		
                    respuesta = false;
		            //throw;
	            }
	 
	        }

            return respuesta;
        
        }
        
        public Boolean InsertAutorizacionPermisoUsuario(int id_usuario, int id_tipousuario, int id_usuarioAutoriza)
        {
            Boolean respuesta = false;
            string sql_query = string.Empty;


            using (SqlConnection cn = objConexion.Conectar())
            {

                try
                {
                    //Query que inserta el usuario a los permisos respectivos
                    sql_query = " INSERT INTO G_UsuarioPermiso " +
                        " (id_usuario,id_tipousuario,fecha_creacion " +
                        " ,fecha_modificacion,estado,id_usuarioAutoriza) " +
                        " VALUES " +
                        " (@id_usuario,@id_tipousuario,@fecha_creacion " +
                        " ,@fecha_modificacion,@estado,@id_usuarioAutoriza) ";

                    var command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("id_usuario", id_usuario);
                    command.Parameters.AddWithValue("id_tipousuario", id_tipousuario);
                    command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                    command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                    command.Parameters.AddWithValue("estado", 'A');
                    command.Parameters.AddWithValue("id_usuarioAutoriza", id_usuarioAutoriza);

                    cn.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        respuesta = true;
                    }
	

                    //Query que actualiza el estado del usuario en la tabla de usuarios
                    sql_query = "UPDATE g_usuarios "+
                        " SET estado = @estado "+
                        " ,id_usuarioAutoriza = @id_usuarioAutoriza "+
                        " WHERE id_usuario = @id_usuario ";

                    var command2 = new SqlCommand(sql_query, cn);
                    command2.Parameters.AddWithValue("estado", 'A');
                    command2.Parameters.AddWithValue("id_usuarioAutoriza", id_usuarioAutoriza);
                    command2.Parameters.AddWithValue("id_usuario", id_usuario);

                    if (command2.ExecuteNonQuery() > 0)
                    {
                        respuesta = true;
                    }



                }
                catch (Exception)
                {
                    
                    throw;
                }



            }

            


            return respuesta;
        }

        public Boolean UpdateRechazoPermisoUsuario(int id_usuarioAutoriza, int id_usuario)
        {
            Boolean respuesta = false;
            string sql_query = string.Empty;

            using (SqlConnection cn = objConexion.Conectar())
            {
                try
                {
                    //Query que actualiza el estado del usuario en la tabla de usuarios
                    sql_query = "UPDATE g_usuarios " +
                        " SET estado = @estado " +
                        " ,id_usuarioAutoriza = @id_usuarioAutoriza " +
                        " WHERE id_usuario = @id_usuario ";

                    var command2 = new SqlCommand(sql_query, cn);
                    command2.Parameters.AddWithValue("estado", 'B');
                    command2.Parameters.AddWithValue("id_usuarioAutoriza", id_usuarioAutoriza);
                    command2.Parameters.AddWithValue("id_usuario", id_usuario);

                    cn.Open();
                    if (command2.ExecuteNonQuery() > 0)
                    {
                        respuesta = true;
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }




            return respuesta;
        }

        public Boolean InsertCodigoRecuperacion(string correo, string codigo)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " INSERT INTO G_UsuarioRecupera "+
            " ([correo],[codigo],[fecha_solicitud]) "+
            " VALUES "+
            " (@correo, @codigo, @fecha_solicitud) ";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("correo", correo);
                command.Parameters.AddWithValue("codigo", codigo);
                command.Parameters.AddWithValue("fecha_solicitud", DateTime.Now);

                conn.Open();
                command.ExecuteNonQuery();
                respuesta = true;
            }

            return respuesta;
        }

        public bool ValidoCodigoRecuperacion(string correo, string codigo)
        {
            var respuesta = false;
            var sql_query = string.Empty;
            sql_query = " SELECT COALESCE(count(1), 0)  as existe " +
                " FROM g_usuariorecupera "+
                " where correo = @correo and codigo = @codigo ";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("correo", correo);
                command.Parameters.AddWithValue("codigo", codigo);
                //command.Parameters.AddWithValue("fecha_solicitud", DateTime.Now);

                try
                {
                    conn.Open();
                    if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                    {
                        respuesta = true;
                    }
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
