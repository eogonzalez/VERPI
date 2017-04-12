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

        public Boolean RegistrarUsuario(CEUsuario objCEUsuario)
        {
            //Declaracion de variables
            Boolean estado = true;
            string sql_query = string.Empty;           

            //Iniciamos proceso de conexion con db
            using (SqlConnection cn = objConexion.Conectar())
            {
                //Query para insertar
                sql_query = " INSERT INTO g_usuarios "+
                    " (nombres,apellidos,cui,telefono "+
                    " ,idDepartamento,direccion,correo "+
                    " ,password,fecha_registro,estado " +
                    " ,id_usuarioAutoriza) "+
                    " VALUES "+
                    " (@nombres,@apellidos,@cui,@telefono "+
                    " ,@idDepartamento,@direccion,@correo "+
                    " ,@password,@fecha_registro,@estado "+
                    " ,@id_usuarioAutoriza) ";


                try
                {

                    SqlCommand command = new SqlCommand(sql_query, cn);

                    command.Parameters.AddWithValue("@nombres", objCEUsuario.CE_Nombres);
                    command.Parameters.AddWithValue("@apellidos", objCEUsuario.CE_Apellidos);

                    command.Parameters.AddWithValue("@cui", objCEUsuario.CE_CUI);

                    command.Parameters.AddWithValue("@telefono", objCEUsuario.CE_Telefono);
                    command.Parameters.AddWithValue("@idDepartamento", objCEUsuario.CE_IDDepto);
                    command.Parameters.AddWithValue("@direccion", objCEUsuario.CE_Direccion);

                    command.Parameters.AddWithValue("@correo", objCEUsuario.CE_Correo);
                    command.Parameters.AddWithValue("@fecha_registro", DateTime.Now);
                    command.Parameters.AddWithValue("@estado", 'R');
                    command.Parameters.AddWithValue("@id_usuarioAutoriza", '0');
                    

                    //Encriptamos la contrasenia
                    General encript = new General();
                    string hash = encript.EncodePassword(objCEUsuario.CE_Correo + objCEUsuario.CE_Password);

                    command.Parameters.AddWithValue("@password", hash);

                    cn.Open();
                    command.ExecuteScalar();

                }
                catch (Exception)
                {

                    estado = false;
                }
            }
                       
            return estado;
        }

        public DataSet SelectSolicitudRegistroUsuarios()
        {
            DataSet ds_usuarios = new DataSet();
            SqlDataAdapter da_usuarios;

            string sql_query = string.Empty;

            using (SqlConnection cn = objConexion.Conectar())
            {
                sql_query = " SELECT id_usuario " +
                    " ,nombres ,apellidos ,cui " +
                    " ,correo,fecha_registro " +
                    " FROM g_usuarios " +
                    " where estado = 'R' ";
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
        
        //Funcion para obtener el id del usuario que hizo login
        public int ConsultaUsuarioId(string correo)
        {
            int respuesta = 0;
            string sql_query = string.Empty;

            sql_query = "  SELECT id_usuario FROM g_usuarios WHERE correo = @correo ";

            using (SqlConnection conexion = objConexion.Conectar())
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

        public DataTable SelectComboPerfiles()
        {
            var respuesta = new DataTable();

            string sql_query = string.Empty;


            using (SqlConnection cn = objConexion.Conectar())
            {
                try
                {

                    sql_query = " SELECT [id_tipousuario] " +
                        " ,[nombre] " +
                        " FROM [SGEODB].[dbo].[G_TipoUsuario] " +
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

        public DataTable SelectComboDepartamentos()
        {
            var dt_respuesta = new DataTable();
            string sql_query = string.Empty;

            using (var cn = objConexion.Conectar())
            {
                sql_query = " SELECT idDepartamento "+
                    " ,nombre,descripcion "+
                    " FROM G_Departamento "+
                    " where estado = 'A' ";
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

        public DataTable SelectDatosUsuario(int idUsuario)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select nombres, apellidos, correo "+
                " from g_usuarios "+
                " where id_usuario = @id_usuario ";

            using (var cn = objConexion.Conectar())
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
    }
}
