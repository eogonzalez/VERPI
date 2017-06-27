using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Capa_Entidad.General;
using System.IO;
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

        public int InsertDatosFormularioBorrador(CEFormularios objCEFormulario)
        {

            var respuesta = 0;
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

                respuesta = no_preingreso;

            }
            }
            catch (Exception)
            {

                throw;
            }


            return respuesta;
        }

        public DataTable SelectDatosCombos(int correlativo_campo, int tipo)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            if (tipo == 2)
            {
                sql_query = " select valor, texto " +
                    " from m_valor_combo " +
                    " where correlativo_campo = @correlativo_campo " +
                    " order by valor ";
            }
            else
            {
                sql_query = " SELECT[id_pais] as valor " +
                    " ,[nombre] as texto " +
                    " FROM[dbo].[G_Paises] " +
                    " where estado = 'A' " +
                    " order by nombre ";
            }



            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                if (tipo == 2)
                {
                    command.Parameters.AddWithValue("correlativo_campo", correlativo_campo);
                }
                
                con.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }


            return dt_respuesta;
        }

        public bool UpdateDatosFormularioBorrador(CEFormularios objCEFormulario)
        {
            var respuesta = false;

            var sql_query = string.Empty;

            try
            {
                /*Actualiza encabezado*/
                sql_query = " UPDATE [dbo].[PreIngreso_Encabezado] "+
                    " SET "+
                    " [fecha_modificacion] = @fecha_modificacion "+
                    " WHERE no_preingreso = @no_preingreso; ";

                using (var con = objConexion.Conectar())
                {
                    var command = new SqlCommand(sql_query, con);

                    command.Parameters.AddWithValue("no_preingreso", objCEFormulario.No_PreIngreso);                                        
                    command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);                    
                    con.Open();
                    command.ExecuteNonQuery();

                    /*Elimino Detalles*/
                    sql_query = "Delete from PreIngreso_Detalle "+
                        " where no_preingreso = @no_preingreso; ";
                    command = new SqlCommand(sql_query, con);
                    command.Parameters.AddWithValue("no_preingreso", objCEFormulario.No_PreIngreso);
                    command.ExecuteNonQuery();


                    /*Inserta Detalles*/
                    sql_query = " INSERT INTO [dbo].[PreIngreso_Detalle] " +
                            " ([no_preingreso],[correlativo_campo] " +
                            " ,[nombre_control],[valor]) " +
                            " VALUES " +
                            " (@no_preingreso, @correlativo_campo " +
                            " , @nombre_control, @valor); ";

                    foreach (DataRow row in objCEFormulario.Dt_Campos.Rows)
                    {
                        command = new SqlCommand(sql_query, con);
                        command.Parameters.AddWithValue("no_preingreso", objCEFormulario.No_PreIngreso);
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

        public DataTable SelectAnexosFormulario(int no_preingreso)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select pia.correlativo_adjunto, "+
                " pia.correlativo_campo, mcf.Etiqueta,pia.nombre_Documento, path"+
                " from PreIngreso_Adjunto pia, m_campos_formulario mcf "+
                " where "+
                " pia.correlativo_campo = mcf.correlativo_campo "+
                " and pia.no_preingreso = @no_preingreso ";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("no_preingreso", no_preingreso);
                conn.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);
            }

            return dt_respuesta;
        }

        public bool InsertDoctoAnexoFormulario(CEFormularios objCEFormulario)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " INSERT INTO [dbo].[PreIngreso_Adjunto] "+
                " ([no_preingreso],[correlativo_campo],[nombre_Documento] "+
                ",[tipo],[path],[fecha_creacion] "+
                " ,[fecha_modificacion],[estado]) "+
                " VALUES "+
                " (@no_preingreso, @correlativo_campo, @nombre_Documento "+
                " , @tipo, @path, @fecha_creacion,"+
                " @fecha_modificacion, @estado) ";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("no_preingreso", objCEFormulario.No_PreIngreso);
                command.Parameters.AddWithValue("correlativo_campo", objCEFormulario.Correlativo_Campo);
                command.Parameters.AddWithValue("nombre_documento", objCEFormulario.Nombre_Documento);
                command.Parameters.AddWithValue("tipo", 1);
                command.Parameters.AddWithValue("path", objCEFormulario.PathDocto);
                command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                command.Parameters.AddWithValue("estado", "T");
                conn.Open();
                command.ExecuteNonQuery();
                respuesta = true;
            }


            return respuesta;
        }

        public DataTable SelectValoresFormulario(int no_preingreso)
        {
            var dt_respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select correlativo_campo, nombre_control, valor " +
                " from preingreso_detalle " +
                " where no_preingreso = @no_preingreso ";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);
                command.Parameters.AddWithValue("no_preingreso", no_preingreso);
                conn.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(dt_respuesta);

            }

            return dt_respuesta;
        }

        protected bool EliminoArchivosServidor(int no_preIngreso)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            /*A.1 Eliminar Archivos */
            sql_query = " select path " +
                " from PreIngreso_Adjunto " +
                " where no_preingreso = @no_preingreso;  ";

            using (var con = objConexion.Conectar())
            {
                var command_1 = new SqlCommand(sql_query, con);
                command_1.Parameters.AddWithValue("no_preingreso", no_preIngreso);

                var da = new SqlDataAdapter(command_1);
                var dt = new DataTable();
                da.Fill(dt);

                try
                {
                    foreach (DataRow row in dt.Rows)
                    {/*Recorro archivos y elimino*/
                        if (File.Exists(row["path"].ToString()))
                        {
                            /*Elimino Archivo*/
                            File.Delete(row["path"].ToString());
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                respuesta = true;
            }



            return respuesta;
        }

        protected bool EliminoArchivoServidor(string path)
        {
            var respuesta = false;

            if (File.Exists(path))
            {
                /*Elimino Archivo*/
                File.Delete(path);
                respuesta = true;
            }                            

            return respuesta;
        }

        public bool EliminaFormulario(int no_preingreso)
        {
            var respuesta = false;

            if (EliminoArchivosServidor(no_preingreso))
            {

                using (var con = objConexion.Conectar())
                {
                    con.Open();
                    var command = con.CreateCommand();
                    SqlTransaction transaccion;

                    /*Inicio Transaccion*/
                    transaccion = con.BeginTransaction("EliminoFormulario");

                    command.Connection = con;
                    command.Transaction = transaccion;

                    /*A. Elimino Detalles*/

                    try
                    {
                        /*A.2 Query para eliminar detalles de archivos*/
                        command.CommandText = "delete preingreso_adjunto " +
                            " where no_preingreso = @no_preingreso; ";
                        command.Parameters.AddWithValue("no_preingreso", no_preingreso);
                        command.ExecuteNonQuery();

                        /*A.3 Query para eliminar valores de campos*/
                        command.CommandText = "DELETE preingreso_detalle " +
                            " where no_preingreso = @no_preingreso_2; ";
                        command.Parameters.AddWithValue("no_preingreso_2", no_preingreso);
                        command.ExecuteNonQuery();

                        /*B. Elimina Encabezado del formulario*/
                        command.CommandText = " DELETE preingreso_encabezado " +
                            " where no_preingreso = @no_preingreso_3;";
                        command.Parameters.AddWithValue("no_preingreso_3", no_preingreso);
                        command.ExecuteNonQuery();

                        transaccion.Commit();
                        respuesta = true;
                    }

                    catch (Exception)
                    {
                        try
                        {
                            transaccion.Rollback();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }

                }

            }

            return respuesta;
        }

        public bool ExisteArchivo(int no_preingreso, int correlativo_campo)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " select coalesce(count(1),0) as existe " +
                " from preingreso_adjunto " +
                " where correlativo_campo = @correlativo_campo "+
                " and no_preingreso = @no_preingreso; ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("correlativo_campo", correlativo_campo);
                command.Parameters.AddWithValue("no_preingreso", no_preingreso);
                con.Open();
                int valor = Convert.ToInt32(command.ExecuteScalar());

                if (valor > 0)
                {
                    respuesta = true;
                }

            }


            return respuesta;
        }

        public bool UpdateDoctoAnexoFormulario(CEFormularios objCEFormulario)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            sql_query = " UPDATE [dbo].[PreIngreso_Adjunto] "+
                " SET "+
                " [nombre_Documento] = @nombre_Documento "+
                " ,[tipo] = @tipo "+
                " ,[fecha_modificacion] = @fecha_modificacion "+
                " WHERE no_preingreso = @no_preingreso and correlativo_campo = @correlativo_campo";

            using (var conn = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, conn);

                command.Parameters.AddWithValue("nombre_documento", objCEFormulario.Nombre_Documento);
                command.Parameters.AddWithValue("tipo", 1);
                command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);

                command.Parameters.AddWithValue("no_preingreso", objCEFormulario.No_PreIngreso);
                command.Parameters.AddWithValue("correlativo_campo", objCEFormulario.Correlativo_Campo);

                conn.Open();
                command.ExecuteNonQuery();
                respuesta = true;
            }


            return respuesta;
        }

        public bool EliminoArchivoFormulario(int correlativo_adjunto, string path)
        {
            var respuesta = false;
            var sql_query = string.Empty;

            if (EliminoArchivoServidor(path))
            {
                sql_query = " DELETE FROM [dbo].[PreIngreso_Adjunto] "+
                    " WHERE correlativo_adjunto = @correlativo_adjunto ";

                using (var con = objConexion.Conectar())
                {
                    var command = new SqlCommand(sql_query, con);
                    command.Parameters.AddWithValue("correlativo_adjunto", correlativo_adjunto);
                    con.Open();
                    command.ExecuteNonQuery();
                    respuesta = true;
                }

            }

            return respuesta;
        }

        public DataTable SelectCamposObligatorios(int no_formulario)
        {
            var respuesta = new DataTable();
            var sql_query = string.Empty;

            sql_query = " select correlativo_campo, Etiqueta, nombre_control, tipo_control " +
                " from m_campos_formulario "+                
                " where no_formulario = @no_formulario and obligatorio = 1 and estado = 'A' "+
                " order by correlativo_campo ";

            using (var con = objConexion.Conectar())
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("no_formulario", no_formulario);
                con.Open();
                var da = new SqlDataAdapter(command);
                da.Fill(respuesta);

            }


            return respuesta;
        }

        public int GenerarExpediente(int no_preingreso)
        {
            var respuesta = 0;

            using (var con = objConexion.Conectar())
            {
                con.Open();
                var command = con.CreateCommand();
                SqlTransaction transaccion;

                //Inicia transaccion
                transaccion = con.BeginTransaction("GenerarExpedienteRPI");

                command.Connection = con;
                command.Transaction = transaccion;

                try
                {
                    /*Query para insertar el encabezado del expediente*/
                    command.CommandText = " insert into "+
                        " expediente_encabezado "+
                        " select no_preingreso, no_formulario, @fecha_creacion, @fecha_modificacion, @estado "+
                        " from preingreso_encabezado "+
                        " where no_preingreso = @no_preingreso; "+
                        " SELECT SCOPE_IDENTITY();";

                    command.Parameters.AddWithValue("fecha_creacion", DateTime.Now);
                    command.Parameters.AddWithValue("fecha_modificacion", DateTime.Now);
                    command.Parameters.AddWithValue("estado", "E");
                    command.Parameters.AddWithValue("no_preingreso", no_preingreso);
                    int no_expediente = 0;
                    no_expediente = Convert.ToInt32(command.ExecuteScalar());

                    /*Query para insertar el valor de los campos del formulario*/
                    command.CommandText = " insert into expediente_detalle "+
                        " select correlativo_detalle, @no_expediente, correlativo_campo, nombre_control, valor "+
                        " from preingreso_detalle "+
                        " where no_preingreso = @no_preingreso_a ";

                    command.Parameters.AddWithValue("no_preingreso_a", no_preingreso);
                    command.Parameters.AddWithValue("no_expediente", no_expediente);
                    command.ExecuteNonQuery();

                    /*Query para insertar los documentos anexos*/
                    command.CommandText = " insert into expediente_adjunto "+
                        " select correlativo_adjunto, @no_expediente_a, correlativo_campo, "+
                        " nombre_documento, tipo, path, @fecha_creacion_a, @fecha_modificacion_a, @estado_a "+
                        " from preingreso_adjunto "+
                        " where no_preingreso = @no_preingreso_b ";

                    command.Parameters.AddWithValue("no_expediente_a", no_expediente);
                    command.Parameters.AddWithValue("fecha_creacion_a", DateTime.Now);
                    command.Parameters.AddWithValue("fecha_modificacion_a", DateTime.Now);
                    command.Parameters.AddWithValue("estado_a", "E");
                    command.Parameters.AddWithValue("no_preingreso_b", no_preingreso);
                    command.ExecuteNonQuery();

                    transaccion.Commit();

                    /*Cambio estatuso en borrador*/
                    var sql_query = string.Empty;
                    sql_query = " update PreIngreso_Encabezado " +
                        " set " +
                        " estado = @estado_b " +
                        " where no_preingreso = @no_preingreso_c; " +                        
                        " update PreIngreso_Adjunto " +
                        " set " +
                        " estado = @estado_b " +
                        " where no_preingreso = @no_preingreso_c; ";
                    command = new SqlCommand(sql_query, con);
                    command.Parameters.AddWithValue("estado_b", "E");
                    command.Parameters.AddWithValue("no_preingreso_c", no_preingreso);                    
                    command.ExecuteScalar();
                    respuesta = no_expediente;
                }
                catch (Exception)
                {

                    try
                    {
                        transaccion.Rollback();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            return respuesta;
        }

        public string SelectEstadoPreIngreso(int no_preingreso)
        {
            var respuesta = string.Empty;
            var sql_query = string.Empty;

            sql_query = " select rtrim(estado) " +
                " from preingreso_encabezado " +
                " where no_preingreso = @no_preingreso ";

            using (var con = objConexion.Conectar() )
            {
                var command = new SqlCommand(sql_query, con);
                command.Parameters.AddWithValue("no_preingreso", no_preingreso);
                con.Open();
                respuesta = (string)command.ExecuteScalar();

            }

            return respuesta;
        }
    }
}
