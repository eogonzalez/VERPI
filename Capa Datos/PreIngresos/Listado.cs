using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Capa_Entidad.PreIngresos;

namespace Capa_Datos.PreIngresos
{
    public class Listado
    {
        General.Conexion objConexion = new General.Conexion();

        public DataTable SelectListadoGridView(int tipo_listado, int no_preingreso)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select correlativo_lista, nombre, direccion, email, telefono "+
                " from PreIngreso_ListaGenerica "+
                " where tipo_lista = @tipo_lista and no_preingreso = @no_preingreso and estado = 'A' "+
                " order by correlativo_lista desc; ";

            using (var cn = objConexion.Conectar())
            {
                try
                {
                    var command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("tipo_lista", tipo_listado);
                    command.Parameters.AddWithValue("no_preingreso", no_preingreso);
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

        public bool InsertElementoLista(CEListado objCELista)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " INSERT INTO [dbo].[PreIngreso_ListaGenerica] "+
                " ([no_preingreso],[tipo_lista],[nombre] "+
                " ,[direccion],[email],[telefono] "+
                " ,[fecha_creacion],[fecha_modificacion],[estado]) "+
                " VALUES "+
                " (@no_preingreso, @tipo_lista , @nombre, @direccion "+
                " , @email, @telefono, @fecha_creacion "+
                " , @fecha_modificacion, @estado) ";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);

                command.Parameters.AddWithValue("no_preingreso", objCELista.No_PreIngreso);
                command.Parameters.AddWithValue("tipo_lista", objCELista.Tipo_Lista);
                command.Parameters.AddWithValue("nombre", objCELista.Nombre);
                command.Parameters.AddWithValue("direccion", objCELista.Direccion);
                command.Parameters.AddWithValue("email", objCELista.Email);
                command.Parameters.AddWithValue("telefono", objCELista.Telefono);
                command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("estado", "A");

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }

            return respuesta;
        }

        public bool UpdateMantenimientoRegistro(CEListado objCELista)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE [dbo].[PreIngreso_ListaGenerica] " +
                " SET[nombre] = @nombre ,[direccion] = @direccion " +
                " ,[email] = @email,[telefono] = @telefono " +
                " ,[fecha_modificacion] = @fecha_modificacion " +
                " WHERE correlativo_lista = @correlativo_lista ";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);

                command.Parameters.AddWithValue("nombre", objCELista.Nombre);
                command.Parameters.AddWithValue("direccion", objCELista.Direccion);
                command.Parameters.AddWithValue("email", objCELista.Email);
                command.Parameters.AddWithValue("telefono", objCELista.Telefono);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("correlativo_lista", objCELista.Correlativo_Lista);

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }

            return respuesta;
        }

        public DataTable SelectElementoLista(CEListado objCELista)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT [nombre],[direccion],[email],[telefono] " +
                " FROM[dbo].[PreIngreso_ListaGenerica] " +
                " where[correlativo_lista] = @correlativo_lista ";

            using (SqlConnection cn = objConexion.Conectar())
            {
                try
                {
                    var command = new SqlCommand(sql_query, cn);
                    command.Parameters.AddWithValue("correlativo_lista", objCELista.Correlativo_Lista);

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

        public bool DeleteElementoLista(CEListado objCELista)
        {
            var respuesta = false;

            var sql_query = string.Empty;

            sql_query = " DELETE FROM [dbo].[PreIngreso_ListaGenerica] "+
                " WHERE correlativo_lista = @correlativo_lista";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("correlativo_lista", objCELista.Correlativo_Lista);

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }


            return respuesta;
        }





    }
}
