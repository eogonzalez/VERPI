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
    public class Formularios
    {
        Conexion objConexion = new Conexion();

        public DataTable SelectCamposFormulario(CEFormularios objCEFormulario)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            /*
            Query que obtiene el listado de campos segun el formulario seleccionado
            */

            sql_query = " SELECT [correlativo_campo],[no_formulario],[no_orden] "+
                " ,[seccion],[Etiqueta],[nombre_control] "+
                " ,[tipo_control],[descripcion],[modo_texto],[nombre_campo_db],[obligatorio] "+
                " ,[expresion_regular] "+
                " FROM ["+objCEFormulario.Nombre_Tabla+"] "+
                " Where estado = 'A' and no_formulario = @no_formulario "+
                " order by seccion, no_orden; ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                
                command.Parameters.AddWithValue("no_formulario", objCEFormulario.No_Formulario);

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

        public DataTable SelectCantidadCamposFormulario(int no_formulario)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            /*Query para consultar cantidad de campos por seccion*/
            sql_query = " select count(1) as total, seccion " +
                " from M_Campos_Formulario " +
                " where no_formulario = @no_formulario " +
                " and estado = 'A' " +
                " group by seccion " +
                " order by seccion ";

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

        public DataTable SelectFormularios(int tipo_tramite)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT [no_formulario]    "+  
                " ,[nombre] "+
                " FROM[dbo].[G_Formularios] "+
                " where estado = 'A' and tipo_tramite = @tipo_tramite "+
                " order by nombre ";
            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("tipo_tramite", tipo_tramite);

                try
                {
                    con.Open();
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

        public bool InsertDatosFormularioBorrador(CEFormularios objCEFormulario)
        {

            var respuesta = false;
            var sql_query = string.Empty;

            try
            {


            /*Inserta encabezado*/
            sql_query = " INSERT INTO [dbo].[PreIngreso_Encabezado] "+
                " ([id_usuario_solicita],[no_formulario] "+
                " ,[fecha_creacion],[fecha_modificacion] "+
                " ,[estado]) "+
                " VALUES "+
                " (@id_usuario_solicita, @no_formulario "+
                " , @fecha_creacion, @fecha_modificacion "+
                " , @estado); "+
                " SELECT SCOPE_IDENTITY(); " ;

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);

                command.Parameters.AddWithValue("id_usuario_solicita", objCEFormulario.ID_Usuario_Solicita);
                command.Parameters.AddWithValue("no_formulario", objCEFormulario.No_Formulario);
                command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("estado", 'T');

                con.Open();
                int no_preingreso = 0;
                no_preingreso = Convert.ToInt32(command.ExecuteScalar());

                /*Inserta Detalles*/
                sql_query = " INSERT INTO [dbo].[PreIngreso_Detalle] "+
                        " ([no_preingreso],[correlativo_campo] "+
                        " ,[nombre_control],[valor]) "+
                        " VALUES "+
                        " (@no_preingreso, @correlativo_campo "+
                        " , @nombre_control, @valor); ";

                foreach (DataRow row in objCEFormulario.Dt_Campos.Rows)
                {
                    command = new SqlCommand(sql_query, con);
                    command.Parameters.AddWithValue("no_preingreso", no_preingreso);
                    command.Parameters.AddWithValue("correlativo_campo", row["correlativo_campo"]);
                    command.Parameters.AddWithValue("nombre_control", row["nombre_control"]);
                    command.Parameters.AddWithValue("valor", row["valor"]);                    
                    command.ExecuteNonQuery();
                }

                /*Inserta Archivos*/

                respuesta = true;

            }
            }
            catch (Exception)
            {

                throw;
            }


            return respuesta;
        }

        public DataTable SelectDatosCombos(int correlativo_campo)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select valor, texto "+
                " from m_valor_combo "+
                " where correlativo_campo = @correlativo_campo "+
                " order by valor ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("correlativo_campo", correlativo_campo);
                con.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }


            return dt_respuesta;
        }
    }
}
