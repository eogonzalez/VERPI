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
    public class Menu
    {
        General.Conexion objConexion = new General.Conexion();

        public DataTable SelectMenu(int id_padre = 0)
        {
            string sql_query = string.Empty;
            DataTable dt = new DataTable();

            if (id_padre > 0)
            {
                sql_query = " SELECT [id_opcion] " +
                    " ,[nombre] " +
                    " ,[descripcion] " +
                    " ,[url] " +
                    " ,comando "+
                    " ,obligatorio " +
                    " ,visible " +
                    " ,login " +
                    " FROM [g_menu_opcion] " +
                    " where id_padre = @id_padre " +
                    " and estado = 'A' "+
                    " order by orden ";
            }
            else
            {
                sql_query = " SELECT [id_opcion] " +
                    " ,[nombre] " +
                    " ,[descripcion] " +
                    " ,[url] " +
                    " ,comando " +
                    " ,obligatorio " +
                    " ,visible " +
                    " ,login " +
                    " FROM [g_menu_opcion] " +
                    " where id_padre is null " +
                    " and estado = 'A' " +
                    " order by orden ";
            }

            using (SqlConnection cn = objConexion.Conectar())
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql_query, cn);

                    if (id_padre > 0)
                    {
                        command.Parameters.AddWithValue("id_padre", id_padre);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);
                }
                catch (Exception)
                {
                    
                    //throw ex;
                }
            }

            return dt;
        }

        public Boolean SaveMenu(CEMenu objCEMenu)
        {
            Boolean respuesta = false;
            string sql_query = string.Empty;

            sql_query = " INSERT INTO [g_menu_opcion] " +
                " ([nombre], [descripcion] ";

            if (objCEMenu.Id_Padre == 0)
            {
                sql_query += " , [url], [comando], [orden] " +
                    " ,[visible],[obligatorio],[login],[id_usuarioAutoriza], [estado]) " +
                    " VALUES " +
                    " (@nombre, @descripcion " +
                    " ,@url, @comando, @orden " +
                    " ,@visible, @obligatorio, @login, @id_usuarioAutoriza, @estado) ";
            }
            else
            {
                sql_query += " , [url], [comando], [id_padre], [orden] " +
                    " ,[visible],[obligatorio],[login],[id_usuarioAutoriza], [estado]) " +
                    " VALUES " +
                    " (@nombre, @descripcion " +
                    " ,@url, @comando, @id_padre, @orden " +
                    " ,@visible, @obligatorio, @login, @id_usuarioAutoriza, @estado) ";
            }

            using (SqlConnection cn = objConexion.Conectar())
            {
                SqlCommand command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("nombre", objCEMenu.Nombre);
                command.Parameters.AddWithValue("descripcion", objCEMenu.Descripcion);
                command.Parameters.AddWithValue("url", objCEMenu.URL);
                command.Parameters.AddWithValue("comando", objCEMenu.Comando);

                if (objCEMenu.Id_Padre > 0)
                {
                    command.Parameters.AddWithValue("id_padre", objCEMenu.Id_Padre);
                }

                command.Parameters.AddWithValue("orden", objCEMenu.Orden);
                command.Parameters.AddWithValue("visible", objCEMenu.Visible);
                command.Parameters.AddWithValue("obligatorio", objCEMenu.Obligatorio);
                command.Parameters.AddWithValue("login", objCEMenu.Login);
                command.Parameters.AddWithValue("id_usuarioAutoriza", objCEMenu.ID_UsuarioAutoriza);
                command.Parameters.AddWithValue("estado", 'A');
                
                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }
            return respuesta;
        }

        public DataSet MenuPrincipal(int idUsuario = 0)
        {
            string sql_query = string.Empty;
            var ds = new DataSet();

            try
            {

                if (idUsuario == 0)
                {//Si usuario es anonimo
                    sql_query = " SELECT id_opcion " +
                        " ,nombre ,descripcion " +
                        " ,url ,id_padre " +
                        " FROM dbo.g_menu_opcion " +
                        " where obligatorio = 1 Or visible = 1 and login = 0 " +
                        " and estado = 'A' " +
                        " order by orden ";
                }
                else
                {//Si usuario esta registrado
                    sql_query = "  select  "+
                        " gmo.id_opcion, gmo.nombre, gmo.descripcion, gmo.url, gmo.comando, gmo.id_padre, gmo.obligatorio , gmo.visible, gmo.login, " +
                        " gptu.id_tipousuario, gptu.acceder, gptu.insertar, gptu.editar, gptu.borrar, gptu.aprobar, gptu.rechazar "+
                        " FROM  "+                        
                        " G_Menu_Opcion gmo, "+
                        " G_PermisoTipoUsuario gptu, "+
                        " G_UsuarioPermiso gup "+
                        " where "+
                        " gmo.id_opcion = gptu.id_opcion "+
                        " and gmo.estado = 'A' "+
                        " and gptu.estado = 'A' "+
                        " and gup.id_tipousuario = gptu.id_tipousuario "+
                        " and gup.id_usuario = @id_usuario "+
                        " order by gmo.id_padre, gmo.orden ";
                }



                using (var cn = objConexion.Conectar())
                {
                    var command = new SqlCommand(sql_query, cn);

                    if (idUsuario != 0)
                    {
                        command.Parameters.AddWithValue("id_usuario", idUsuario);
                    }

                    var da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    cn.Close();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return ds;
        }

        public DataTable SelectOpcionMenu(int id_opcion)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT [nombre] "+
                " ,[descripcion],[url] "+
                " ,[comando],[id_padre] "+
                " ,[orden],[visible] "+
                " ,[obligatorio],[login] "+
                " FROM [G_Menu_Opcion] "+
                " where estado = 'A' and id_opcion = @id_opcion ";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("id_opcion", id_opcion);

                try
                {
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

        public Boolean UpdateMenuOpcion(CEMenu objCEMenu)
        {
            Boolean respuesta = false;
            string sql_query = string.Empty;

            sql_query = "UPDATE G_Menu_Opcion " +
                " SET [nombre] = @nombre " +
                " ,[descripcion] = @descripcion " +
                " ,[url] = @url " +
                " ,[comando] = @comando ";

            if (objCEMenu.Id_Padre > 0)
            {
                sql_query += " ,[id_padre] = @id_padre ";
            }
                
            sql_query += " ,[orden] = @orden "+
                " ,[visible] = @visible "+
                " ,[obligatorio] = @obligatorio "+
                " ,[login] = @login "+
                " ,[id_usuarioAutoriza] = @id_usuarioAutoriza "+
                " WHERE id_opcion = @id_opcion" ;



            using (SqlConnection cn = objConexion.Conectar())
            {
                SqlCommand command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("nombre", objCEMenu.Nombre);
                command.Parameters.AddWithValue("descripcion", objCEMenu.Descripcion);
                command.Parameters.AddWithValue("url", objCEMenu.URL);
                command.Parameters.AddWithValue("comando", objCEMenu.Comando);

                if (objCEMenu.Id_Padre > 0)
                {
                    command.Parameters.AddWithValue("id_padre", objCEMenu.Id_Padre);
                }

                command.Parameters.AddWithValue("orden", objCEMenu.Orden);
                command.Parameters.AddWithValue("visible", objCEMenu.Visible);
                command.Parameters.AddWithValue("obligatorio", objCEMenu.Obligatorio);
                command.Parameters.AddWithValue("login", objCEMenu.Login);
                command.Parameters.AddWithValue("id_usuarioAutoriza", objCEMenu.ID_UsuarioAutoriza);
                command.Parameters.AddWithValue("id_opcion", objCEMenu.ID_MenuOpcion);

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }
            return respuesta;
        }

        public Boolean DeleteMenuOpcion(CEMenu objCEMenu)
        {
            Boolean respuesta = false;
            string sql_query = string.Empty;

            sql_query = "UPDATE G_Menu_Opcion " +
                " SET [estado] = @estado " +
                " ,[id_usuarioAutoriza] = @id_usuarioAutoriza " +
                " WHERE id_opcion = @id_opcion";

            using (SqlConnection cn = objConexion.Conectar())
            {
                SqlCommand command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("estado", 'B');
                command.Parameters.AddWithValue("id_opcion", objCEMenu.ID_MenuOpcion);
                command.Parameters.AddWithValue("id_usuarioAutoriza", objCEMenu.ID_UsuarioAutoriza);
                
                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }
            return respuesta;
        }
    }
}
