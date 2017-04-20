using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Capa_Entidad.General;
using System.Data.SqlClient;

namespace Capa_Datos.General
{
    public class Mantenimientos
    {
        Conexion objConexion = new Conexion();

        public DataTable SelectMantenimientoGridView(CEMantenimientos objCEMant)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT " + objCEMant.ID_Mant+
                " ,"+objCEMant.Nombre_Mant+" , " + objCEMant.Descripcon_Mant+
                " FROM " + objCEMant.TBL_Mant+
                " where estado = 'A' ";

            using (var cn = objConexion.Conectar())
            {
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

        public bool InsertMantenimiento(CEMantenimientos objCEMant)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " INSERT INTO " +objCEMant.TBL_Mant+
                " ("+objCEMant.Nombre_Mant+","+objCEMant.Descripcon_Mant +
                " ,[fecha_creacion],[fecha_modificacion] " +
                " ,[estado]) " +
                " VALUES " +
                " (@"+objCEMant.Nombre_Mant+",@"+objCEMant.Descripcon_Mant  +
                " ,@fecha_creacion,@fecha_modificacion " +
                " ,@estado) ";

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue(objCEMant.Nombre_Mant, objCEMant.Nombre_Mant_Valor);
                command.Parameters.AddWithValue(objCEMant.Descripcon_Mant, objCEMant.Descripcion_Mant_Valor);
                command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("estado", "A");

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }

            return respuesta;
        }

        public bool DeleteMantenimiento(CEMantenimientos objCEMant)
        {
            var respuesta = false;

            var sql_query = string.Empty;

            sql_query = " UPDATE " +objCEMant.TBL_Mant+
                " SET  [estado] = @estado " +
                " ,[fecha_modificacion] = @fecha_modificacion " +
                " WHERE "+objCEMant.ID_Mant+" = @"+objCEMant.ID_Mant;

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue("estado", "B");
                command.Parameters.AddWithValue(objCEMant.ID_Mant, objCEMant.ID_Mant_Valor);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }


            return respuesta;
        }

        public DataTable SelectMantenimientoRegistro(CEMantenimientos objCEMant)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT  " +objCEMant.Nombre_Mant+
              " , " + objCEMant.Descripcon_Mant+
              " FROM " + objCEMant.TBL_Mant+
              " where estado = @estado and "+objCEMant.ID_Mant+" = @"+objCEMant.ID_Mant;

            using (SqlConnection cn = objConexion.Conectar())
            {
                try
                {
                    var command = new SqlCommand(sql_query, cn);

                    command.Parameters.AddWithValue("estado", 'A');
                    command.Parameters.AddWithValue(objCEMant.ID_Mant, objCEMant.ID_Mant_Valor);

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

        public bool UpdateMantenimientoRegistro(CEMantenimientos objCEMant)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE " +objCEMant.TBL_Mant+
                " SET "+objCEMant.Nombre_Mant+" = @"+objCEMant.Nombre_Mant+
                ", "+objCEMant.Descripcon_Mant+" = @"+objCEMant.Descripcon_Mant +
                " ,fecha_modificacion = @fecha_modificacion " +
                " WHERE "+objCEMant.ID_Mant+" = @"+objCEMant.ID_Mant;

            using (var cn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, cn);
                command.Parameters.AddWithValue(objCEMant.Nombre_Mant, objCEMant.Nombre_Mant_Valor);
                command.Parameters.AddWithValue(objCEMant.Descripcon_Mant, objCEMant.Descripcion_Mant_Valor);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue(objCEMant.ID_Mant, objCEMant.ID_Mant_Valor);

                cn.Open();
                command.ExecuteScalar();
                respuesta = true;
            }

            return respuesta;
        }
    }
}
