using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Capa_Datos.General;
using System.Data.SqlClient;

namespace Capa_Datos.Bandeja
{
    public class BandejaUsuario
    {
        Conexion objConexion = new Conexion();

        public DataTable SelectFormulariosBorrador(int id_usuario_solicita)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select pe.no_preingreso, gf.tipo_tramite as cmd, gf.no_formulario, gf.nombre, pe.fecha_creacion, " +
                " case  pe.estado when 'T' then  'Borrador' else 'Enviado' end as estado_txt "+
                " from preingreso_encabezado pe, g_formularios gf "+
                " where "+
                " pe.no_formulario = gf.no_formulario and "+
                " pe.id_usuario_solicita = @id_usuario_solicita;  ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("id_usuario_solicita", id_usuario_solicita);
                con.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);

            }

            return dt_respuesta;
        }

        public int SelectCantidadBorradores(int id_usuario_solicita)
        {
            var respuesta = 0;
            var sql_query = string.Empty;

            sql_query = " select count(1) as cantidad "+
                " from "+
                " preingreso_encabezado "+
                " where "+
                " id_usuario_solicita = @id_usuario_solicita; ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("id_usuario_solicita", id_usuario_solicita);
                con.Open();
                respuesta = Convert.ToInt32(command.ExecuteScalar());
            }

            return respuesta;
        }


    }
}
