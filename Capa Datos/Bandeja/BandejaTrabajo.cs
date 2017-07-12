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

        public DataTable SelectFormularios(int tipo_tramite = 0, string fecha_inicial = "", string fecha_final = "")
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select pe.no_expediente, pe.no_preingreso, "+
                " gf.tipo_tramite as cmd,  "+
                " pie.id_usuario_solicita, "+
                " gu.nombres, "+
                " gf.no_formulario,  "+
                " gf.nombre as nombre_formulario, pe.fecha_creacion,   "+
                " case  pe.estado when 'T' then  'Borrador' else 'Enviado' end as estado_txt "+
                " from "+
                " expediente_encabezado pe,  "+
	            " g_formularios gf, "+
                " preingreso_encabezado pie, "+
	            " g_usuarios gu "+
                " where "+
                " pe.no_formulario = gf.no_formulario and "+
                " pie.no_preingreso = pe.no_preingreso and "+
                " gu.id_usuario = pie.id_usuario_solicita and "+
                " pe.estado = 'E' ";

            if (tipo_tramite > 0)
            {
                sql_query = sql_query + " and gf.tipo_tramite = @tipo_tramite ";
            }

            if (fecha_inicial.Length > 0)
            {
                sql_query = sql_query + " and pe.fecha_creacion >= @fecha_inicial ";
            }

            if (fecha_final.Length > 0)
            {
                sql_query = sql_query + " and pe.fecha_creacion <= @fecha_final ";
            }

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);

                if (tipo_tramite > 0)
                {
                    command.Parameters.AddWithValue("tipo_tramite", tipo_tramite);
                }

                if (fecha_inicial.Length > 0)
                {
                    command.Parameters.AddWithValue("fecha_inicial", Convert.ToDateTime(fecha_inicial));
                }

                if (fecha_final.Length > 0)
                {
                    command.Parameters.AddWithValue("fecha_final", Convert.ToDateTime(fecha_final));
                }

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
