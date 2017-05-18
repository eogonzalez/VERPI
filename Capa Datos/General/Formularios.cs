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
                " order by no_formulario ";
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

    }
}
