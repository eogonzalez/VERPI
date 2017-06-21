using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Capa_Datos.General;

namespace Capa_Datos.Bandeja
{
    public class BandejaTrabajo
    {
        Conexion objConexion = new Conexion();

        public DataTable SelectFormularios()
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = "select pe.no_expediente, gf.tipo_tramite as cmd, gf.no_formulario, "+
                " gf.nombre, pe.fecha_creacion,  "+
                " case  pe.estado when 'T' then  'Borrador' else 'Enviado' end as estado_txt "+
                " from expediente_encabezado pe, g_formularios gf "+
                " where "+
                " pe.no_formulario = gf.no_formulario ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);                
                con.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);

            }

            return dt_respuesta;
        }

        public int SelectCantidadFormularios()
        {
            var respuesta = 0;
            var sql_query = string.Empty;

            sql_query = " select count(1) as cantidad " +
                " from " +
                " expediente_encabezado ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);                
                con.Open();
                respuesta = Convert.ToInt32(command.ExecuteScalar());
            }

            return respuesta;
        }
    }
}
