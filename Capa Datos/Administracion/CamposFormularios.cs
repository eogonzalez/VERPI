using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.General;
using System.Data;
using System.Data.SqlClient;
using Capa_Entidad.Administracion;

namespace Capa_Datos.Administracion
{
    public class CamposFormularios
    {
        Conexion objConexion = new Conexion();
        

        public DataTable SelectCamposFormulario(int no_formulario)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT [correlativo_campo] "+
                " ,[no_formulario],[no_orden],[seccion] "+
                " ,[Etiqueta],[nombre_control], "+
                " case [tipo_control] when 1 then 'Texto' when 2 then 'Combo' when 3 then 'Adjunto' when 4 then 'Check' when 5 then 'Combo Paises' when 6 then 'Etiqueta' when 8 then 'Ayuda' else 'Combo de Clase' end as tipo_control " +
                " ,[descripcion],[modo_texto],[nombre_campo_db] "+
                " ,[obligatorio],[expresion_regular] "+
                " FROM[dbo].[M_Campos_Formulario] "+
                " where estado = 'A' and no_formulario = @no_formulario "+
                " order by seccion, no_orden; ";

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

        public DataTable SelectFormularios()
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT [no_formulario] "+
                " ,[tipo_tramite] "+
                " ,[nombre] "+
                " ,case tipo_tramite when 1 then 'Marcas' when 2 then 'Patentes' else 'Derechos de Autor' end as nombre_tipo "+
                " ,[descripcion_formulario] " +
                " FROM[G_Formularios] "+
                " WHERE estado = 'A' ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                con.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }

            return dt_respuesta;
        }

        public bool InsertCamposFormulario(CECamposFormularios objCECampos)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " INSERT INTO [dbo].[M_Campos_Formulario] "+
                " ([no_formulario],[no_orden],[seccion] "+
                " ,[Etiqueta],[nombre_control],[tipo_control] "+
                " ,[descripcion],[modo_texto],[nombre_campo_db] "+
                " ,[obligatorio],[expresion_regular] "+
                " ,[fecha_creacion],[fecha_modificacion],[estado]) "+
                " VALUES "+
                " (@no_formulario, @no_orden, @seccion "+
                " , @Etiqueta, @nombre_control, @tipo_control "+
                " , @descripcion, @modo_texto, @nombre_campo_db "+
                " , @obligatorio, @expresion_regular "+
                " , @fecha_creacion, @fecha_modificacion, @estado);";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("no_formulario", objCECampos.No_Formulario);
                command.Parameters.AddWithValue("no_orden", objCECampos.Orden);
                command.Parameters.AddWithValue("seccion", objCECampos.Seccion);
                command.Parameters.AddWithValue("Etiqueta", objCECampos.Etiqueta);
                command.Parameters.AddWithValue("nombre_control", objCECampos.NombreControl);
                command.Parameters.AddWithValue("tipo_control", objCECampos.TipoControl);
                command.Parameters.AddWithValue("descripcion", objCECampos.Descripcion);
                command.Parameters.AddWithValue("modo_texto", objCECampos.ModoTexto);
                command.Parameters.AddWithValue("nombre_campo_db", objCECampos.CampoBaseDatosRPI);
                command.Parameters.AddWithValue("obligatorio", objCECampos.Obligatorio);
                command.Parameters.AddWithValue("expresion_regular", objCECampos.ExpresionRegular);
                command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("estado", "A");

                try
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return respuesta;
        }

        public bool UpdateCampoFormulario(CECamposFormularios objCECampos)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE [dbo].[M_Campos_Formulario] "+
                " SET[no_formulario] = @no_formulario "+
                " ,[no_orden] = @no_orden "+
                " ,[seccion] = @seccion "+
                " ,[Etiqueta] = @Etiqueta "+
                " ,[nombre_control] = @nombre_control "+
                " ,[tipo_control] = @tipo_control "+
                " ,[descripcion] = @descripcion "+
                " ,[modo_texto] = @modo_texto "+
                " ,[nombre_campo_db] = @nombre_campo_db "+
                " ,[obligatorio] = @obligatorio "+
                " ,[expresion_regular] = @expresion_regular "+
                " ,[fecha_modificacion] = @fecha_modificacion "+
                " WHERE correlativo_campo = @correlativo_campo";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("no_formulario", objCECampos.No_Formulario);
                command.Parameters.AddWithValue("no_orden", objCECampos.Orden);
                command.Parameters.AddWithValue("seccion", objCECampos.Seccion);
                command.Parameters.AddWithValue("Etiqueta", objCECampos.Etiqueta);
                command.Parameters.AddWithValue("nombre_control", objCECampos.NombreControl);
                command.Parameters.AddWithValue("tipo_control", objCECampos.TipoControl);
                command.Parameters.AddWithValue("descripcion", objCECampos.Descripcion);
                command.Parameters.AddWithValue("modo_texto", objCECampos.ModoTexto);
                command.Parameters.AddWithValue("nombre_campo_db", objCECampos.CampoBaseDatosRPI);
                command.Parameters.AddWithValue("obligatorio", objCECampos.Obligatorio);
                command.Parameters.AddWithValue("expresion_regular", objCECampos.ExpresionRegular);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("correlativo_campo", objCECampos.No_Correlativo_Campo);

                try
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return respuesta;
        }

        public DataTable SelectCampoFormulario(int correlativo_campo)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " SELECT " +
                " [no_formulario],[no_orden],[seccion] " +
                " ,[Etiqueta],[nombre_control], [tipo_control] " +
                " ,[descripcion],[modo_texto],[nombre_campo_db] " +
                " ,[obligatorio],[expresion_regular] " +
                " FROM[dbo].[M_Campos_Formulario] " +
                " where correlativo_campo = @correlativo_campo ";

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

        public bool DeleteCampoFormulario(int correlativo_campo)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE [dbo].[M_Campos_Formulario] " +
                " SET  " +
                " [estado] = @estado " +
                " ,[fecha_modificacion] = @fecha_modificacion " +
                " WHERE correlativo_campo = @correlativo_campo";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("estado", "B");                
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("correlativo_campo", correlativo_campo);

                try
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    respuesta = true;
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
