﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Capa_Negocio.General;
using Capa_Entidad.General;
using System.IO;

namespace VERPI.PreIngresos.Marcas
{
    public partial class Documentos : System.Web.UI.Page
    {
        CNFormularios objCNFormulario = new CNFormularios();
        CEFormularios objCEFormulario = new CEFormularios();

        #region Eventos del formulario
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                divAlertCorrecto.Visible = false;
                divAlertError.Visible = false;
                Session.Add("ValidoEnvio", true);
                divContrasenia.Visible = false;


                var cmd = 0;
                if (Request.QueryString["cmd"] != null)
                {
                    cmd = Convert.ToInt32(Request.QueryString["cmd"].ToString());
                    Session.Add("cmd", cmd);
                }

                var no_PreIngreso = 0;
                if (Request.QueryString["np"] != null)
                {
                    no_PreIngreso = Convert.ToInt32(Request.QueryString["np"].ToString());
                    Session.Add("noPreIngreso", no_PreIngreso);
                    
                }

                var no_formulario = 0;
                if (Request.QueryString["nf"] != null)
                {
                    no_formulario = Convert.ToInt32(Request.QueryString["nf"].ToString());
                    Session.Add("no_formulario", no_formulario);

                }


                string estado = string.Empty;
                estado = objCNFormulario.SelectEstadoPreIngreso(no_PreIngreso);
                Session.Add("estado", estado);
                ConfiguracionEstado();

                if (estado != "E")
                {
                    LlenarPanel(no_PreIngreso, no_formulario);
                }
                
                
                Llenar_gvAnexos(no_PreIngreso);

                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
                btnEnviar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnEnviar, ""));
            }
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (ViewState["controlsadded"] != null)
            {
                if (Session["noPreingreso"] != null && Session["no_formulario"] != null)
                {
                    int no_PreIngreso = (int)Session["noPreingreso"];
                    int no_formulario = (int)Session["no_formulario"];
                    LlenarPanel(no_PreIngreso, no_formulario);
                }
            }

        }

        protected void gvAnexos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvAnexos.Rows[index];
            int id_anexo = Convert.ToInt32(row.Cells[0].Text);
            string pathAnexo = row.Cells[1].Text;
            string archivo = Path.GetFileName(pathAnexo);
            
            switch (e.CommandName)
            {
                case "mostrar":
                    Response.Redirect("~/doctos/" + archivo);
                    return;
                case "Eliminar":
                    EliminoDocumento(id_anexo, pathAnexo);
                    Llenar_gvAnexos((int)Session["noPreIngreso"]);
                    break;
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PreIngresos/Marcas/PreIngreso.aspx?cmd=" + Session["cmd"].ToString() + "&nf=" + Session["no_formulario"].ToString() + "&np=" + Session["noPreIngreso"].ToString());
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (objCNFormulario.TieneEstadoWFInicial((int)Session["no_formulario"]))
            {//Valida que el formulario tenga un estado inicial
                if ((bool)Session["ValidoEnvio"])
                {//Si Valido envio
                    //Muestro div para ingreso de contraseña
                    btnEnviar.Text = "3) Confirmar y Enviar Solicitud";
                    Session["ValidoEnvio"] = false;
                    divContrasenia.Visible = true;
                }
                else
                {

                    if (ValidoContraseña())
                    {
                        if (Session["noPreIngreso"] != null)
                        {
                            //Realiza el envio del formulario
                            /*Valida que los campos cumplan los requisitos de obligatorio*/
                            if (ValidoCamposObligatorios())
                            {
                                var idExpediente = GeneroExpediente((int)Session["noPreIngreso"]);

                                if (idExpediente > 0)
                                {
                                    MensajeCorrectoPrincipal.Text += "Se ha generado expediente correctamente. ";

                                    btnEnviar.Text = "3) Enviar Solicitud";
                                    Session["ValidoEnvio"] = true;
                                    divContrasenia.Visible = false;

                                    /*BloqueoGeneral*/
                                }
                                else
                                {
                                    ErrorMessagePrincipal.Text += "Ha ocurrido un error al generar ";
                                    divAlertError.Visible = true;
                                }

                            }
                        }
                        else
                        {
                            divAlertCorrecto.Visible = false;

                            ErrorMessagePrincipal.Text = "Debe de guardar primero el formulario para poder enviarlo.";
                            divAlertError.Visible = true;
                        }

                    }
                    else
                    {
                        divAlertCorrecto.Visible = false;

                        ErrorMessagePrincipal.Text = "Contraseña Incorrecta, Formulario No Enviado.";
                        divAlertError.Visible = true;
                    }
                }
            }
            else
            {
                divAlertCorrecto.Visible = false;
                ErrorMessagePrincipal.Text = "Formulario No tiene definido estado inicial. Formulario no enviado.";
                divAlertError.Visible = true;
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/#");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {                    
            GuardoDocumentos(pnl_seccion_adjuntos, (int)Session["noPreingreso"]);
            Llenar_gvAnexos((int)Session["noPreingreso"]);
        }


        #endregion


        #region Funciones

        protected void LlenarPanel(int noPreingreso, int no_formulario)
        {
            var dt_total_campos = new DataTable();
            var cantidad_s4 = 0;


            objCEFormulario.Nombre_Tabla = "M_Campos_Formulario";
            objCEFormulario.No_Formulario = no_formulario;

            dt_total_campos = objCNFormulario.SelectCantidadCamposFormulario(no_formulario);

            foreach (DataRow rowc in dt_total_campos.Rows)
            {
                switch (rowc["seccion"].ToString())
                {
                    case "4":
                        if (rowc["total"] != null)
                        {
                            cantidad_s4 = (int)rowc["total"];
                        }
                        break;
                }
            }

            var dt = new DataTable();
            dt = objCNFormulario.SelectCamposFormulario(objCEFormulario);

            int cont = 1;
            int cs3 = 1;

            foreach (DataRow row in dt.Rows)
            {/*Recorro los campos del formulario a construir*/

                switch (row["seccion"].ToString())
                {

                    case "4":

                        if (row["tipo_control"].ToString() == "3")
                        {//Si es documento
                            ConstruirControlesCiclo(pnl_seccion_adjuntos, row, cantidad_s4, ref cont, ref cs3);
                        }

                        break;
                }


            }
        }

        protected void ConstruirControlesCiclo(Panel pnl_contenedor, DataRow row, int cantidad_controles, ref int cont, ref int x)
        {
            if (cantidad_controles >= 4)
            {
                //Agrego Primer div
                if (cont == 1)
                {
                    pnl_contenedor.Controls.Add(new LiteralControl("<div class='form-group'>"));
                }

                //Creo controles
                ConstruirControles(pnl_contenedor, row, cantidad_controles);


                if (cont == 2 || cantidad_controles == x)
                {
                    pnl_contenedor.Controls.Add(new LiteralControl("</div>"));
                    cont = 0;
                }

                cont++;
                x++;
            }
            else {
                pnl_contenedor.Controls.Add(new LiteralControl("<div class='form-group'>"));
                //Si es la seccion 1, se agregan campos en encabezado del formulario
                ConstruirControles(pnl_contenedor, row, cantidad_controles);
                pnl_contenedor.Controls.Add(new LiteralControl("</div>"));
            }

            ViewState["controlsadded"] = true;
        }

        protected void ConstruirControles(Panel pnl_contenedor, DataRow row, int total_campos)
        {

            /*Agrego Label*/
            var label = new Label();
            label.Text = row["etiqueta"].ToString();

            label.CssClass = "control-label col-xs-2";
            pnl_contenedor.Controls.Add(label);

            if (total_campos >= 4)
            {
                pnl_contenedor.Controls.Add(new LiteralControl("<div class='col-xs-4'>"));
            }
            else
            {
                pnl_contenedor.Controls.Add(new LiteralControl("<div class='col-xs-10'>"));
            }

            int no_control = 0;
            no_control = 10000 + (int)row["correlativo_campo"];
            var identificacion = string.Empty;
            identificacion = row["nombre_control"].ToString() + "_" + no_control.ToString();

            switch (row["tipo_control"].ToString())
            {
                

                case "3":

                    //Si es Adjunto
                    FileUpload MiFileUpload = new FileUpload();
                    MiFileUpload.ID = identificacion;
                    MiFileUpload.ToolTip = row["descripcion"].ToString();
                    
                    MiFileUpload.EnableViewState = true;
                    //MiFileUpload.ViewStateMode = ViewStateMode.Enabled;

                    pnl_contenedor.Controls.Add(MiFileUpload);
                    
                    //Button MiBoton = new Button();
                    //MiBoton.ID = "btn_" + identificacion;
                    //MiBoton.Click += new System.EventHandler(subeArchivo_Click);
                    //MiBoton.Text = "Subir Documento";
                    //MiBoton.CommandName = identificacion;
                    //pnl_contenedor.Controls.Add(MiBoton);
                    

                    //AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                    //trigger.ControlID = MiBoton.ID;
                    //trigger.EventName = "Click";
                    //upnlDoctos.Triggers.Add(trigger);
                    break;

            }
            ViewState["controlsadded"] = true;
            pnl_contenedor.Controls.Add(new LiteralControl("</div>"));

        }

        protected void GuardoDocumentos(Panel pnl_contenedor, int no_preingreso)
        {
            divAlertCorrecto.Visible = false;
            divAlertError.Visible = false;

            int correlativo_campo = 0;
            string nombre_control = string.Empty;
            int numero_control = 0;
            string valor = string.Empty;

            bool existeMensajeError = false;
            bool existeMensajeCorrecto = false;
            string mensajeCorrecto = string.Empty;
            string mensajeError = string.Empty;

            foreach (Control c in pnl_contenedor.Controls)
            {//Recorro cada control del panel de documentos

                if (c is FileUpload)
                {//Si es carga de archivos

                    FileUpload flup;
                    flup = (FileUpload)c;
                    
                    if (flup.HasFile)
                    {
                        //string archivo = string.Empty;
                        //archivo = flup.FileName;
                        string carpeta = Path.Combine(Request.PhysicalApplicationPath, "doctos");

                        string prefijo = no_preingreso.ToString() + "_" + flup.ID;

                        nombre_control = flup.ID.Remove(flup.ID.Length - 6);
                        numero_control = Convert.ToInt32(flup.ID.Substring(flup.ID.Length - 5, 5));
                        correlativo_campo = numero_control - 10000;

                        /*Validacion de tipo de archivo*/
                        string extension = Path.GetExtension(flup.PostedFile.FileName);

                        string[] tiposArchivosAceptados = new string[4];
                        tiposArchivosAceptados[0] = ".pdf";
                        tiposArchivosAceptados[1] = ".jpeg";
                        tiposArchivosAceptados[2] = ".jpg";
                        tiposArchivosAceptados[3] = ".png";

                        bool archivoAceptado = false;

                        string archivo = Path.GetFileName(flup.PostedFile.FileName);

                        for (int i = 0; i <= 3; i++)
                        {//Recorro los tipos de archivos aceptados
                            if (extension.ToLower() == tiposArchivosAceptados[i])
                            {
                                //Si extension es valida
                                archivoAceptado = true;
                            }
                        }

                        if (!archivoAceptado)
                        {//Si archivo no es aceptado
                            if (!existeMensajeError)
                            {
                                mensajeError += " El archivo:'" + archivo + "' no es de tipo 'pdf', 'jpg' o 'png'. Extension no valida. ";
                            }
                            else
                            {
                                mensajeError += "; El archivo:'" + archivo + "' no es de tipo 'pdf', 'jpg' o 'png'. Extension no valida. ";
                                
                            }


                            existeMensajeError = true;
                        }

                        int tamañoArchivo = flup.PostedFile.ContentLength / 1000; //En kilobites

                        if (tamañoArchivo > (2*1000))
                        {
                            archivoAceptado = false;
                            existeMensajeError = true;

                            if (!existeMensajeError)
                            {
                                mensajeError += " Archivo excede tamaño maximo soportado (2MB). ";
                            }
                            else
                            {
                                mensajeError += "; Archivo '"+archivo+ "' excede tamaño maximo soportado (2MB). ";
                            }                            
                            
                        }

                        if (archivoAceptado)
                        {//Si archivo es aceptado lo cargo
                            try
                            {


                                string carpeta_final = Path.Combine(carpeta, prefijo + extension);

                                flup.PostedFile.SaveAs(carpeta_final);

                                objCEFormulario.No_PreIngreso = (int)Session["noPreingreso"];
                                objCEFormulario.Correlativo_Campo = correlativo_campo;
                                objCEFormulario.Nombre_Documento = archivo;
                                objCEFormulario.TipoDocto = extension;

                                if (!objCNFormulario.ExisteArchivo((int)Session["noPreingreso"], correlativo_campo))
                                {
                                    objCEFormulario.PathDocto = carpeta_final;

                                    //guarda ficha archivo
                                    objCNFormulario.InsertDoctoAnexoFormulario(objCEFormulario);
                                }
                                else
                                {
                                    //Actualiza ficha archivo
                                    objCNFormulario.UpdateDoctoAnexoFormulario(objCEFormulario);
                                }

                                if (!existeMensajeCorrecto)
                                {
                                    mensajeCorrecto += "El Archivo '" + archivo + "' ha sido cargado. ";
                                }
                                else
                                {
                                    mensajeCorrecto += ";El Archivo '" + archivo + "' ha sido cargado. ";
                                }

                                existeMensajeCorrecto = true;
                            }
                            catch (Exception ex)
                            {
                                mensajeError += " Error: " + ex.Message;

                                //throw;
                            }
                        }//if !archivoAceptado   
                        else
                        {
                            mensajeError += " [Archivo No Cargado].";
                            existeMensajeError = true;
                        }

                    }// if (flup.HasFile)

                }//if (c is FileUpload)

            }//foreach (Control c in pnl_contenedor.Controls)
            
            if (existeMensajeCorrecto)
            {
                divAlertCorrecto.Visible = true;
                MensajeCorrectoPrincipal.Text = mensajeCorrecto;
            }

            if (existeMensajeError)
            {
                divAlertError.Visible = true;
                ErrorMessagePrincipal.Text = mensajeError;
            }
        }

        protected void Llenar_gvAnexos(int no_preingreso)
        {
            DataTable tbl = new DataTable();
            tbl = objCNFormulario.SelectAnexosFormulario(no_preingreso);
            gvAnexos.DataSource = tbl;
            gvAnexos.DataBind();
        }

        protected void EliminoDocumento(int id_anexo, string path)
        {            
            if (objCNFormulario.EliminoArchivoFormulario(id_anexo, path))
            {
                divAlertCorrecto.Visible = true;
                MensajeCorrectoPrincipal.Text = "Se ha eliminado archivo correctamente.";
            }
            else
            {
                divAlertError.Visible = true;
                ErrorMessagePrincipal.Text = "Ha ocurrido un error al tratar de eliminar archivo.";
            }
        }

        protected bool ValidoCamposObligatorios()
        {
            /*Selecciono valores obligatorios del formulario*/
            var dt_obligatorios = objCNFormulario.SelectCamposObligatorios((int)Session["no_formulario"]);
            var dt_valoresControles = objCNFormulario.SelectValoresFormulario((int)Session["noPreIngreso"]);

            bool cumple = true;
            string nombre_campo = string.Empty;
            int x = 1;

            if (dt_obligatorios.Rows.Count > 0)
            {//Si trae valores obligatorios

                foreach (DataRow row in dt_obligatorios.Rows)
                {/*recorro campos obligatorios*/
                    int corr_obligatorio = (int)row["correlativo_campo"];
                    int tipo_control = Convert.ToInt32(row["tipo_control"].ToString());

                    foreach (DataRow rowValor in dt_valoresControles.Rows)
                    {/*Busco si cumple en los datos del formulario*/
                        int corr_valor = (int)rowValor["correlativo_campo"];
                        if (corr_obligatorio == corr_valor)
                        {
                            if (rowValor["valor"] != null)
                            {

                                if (tipo_control == 1)
                                {/*si es texto*/
                                    string valor = rowValor["valor"].ToString();
                                    if (valor.Length == 0)
                                    {
                                        cumple = false;
                                        if (x == 1)
                                        {
                                            nombre_campo += row["Etiqueta"].ToString();
                                        }
                                        else
                                        {
                                            nombre_campo += " , " + row["Etiqueta"].ToString();
                                        }
                                        x++;
                                        break;
                                    }
                                }
                                else if (tipo_control == 2)
                                {/*Si es combo*/
                                    int valor = Convert.ToInt32(rowValor["valor"].ToString());
                                    if (valor == 0)
                                    {
                                        cumple = false;
                                        if (x == 1)
                                        {
                                            nombre_campo += row["Etiqueta"].ToString();
                                        }
                                        else
                                        {
                                            nombre_campo += " , " + row["Etiqueta"].ToString();
                                        }
                                        x++;
                                        break;
                                    }
                                }
                                else if (tipo_control == 4)
                                {/*Si es check*/
                                    /*El tipo check es obligatorio?*/
                                }


                            }
                        }
                    }

                    if (tipo_control == 3)
                    {/*Si es documento adjunto*/
                        bool existe = objCNFormulario.ExisteArchivo((int)Session["noPreIngreso"], corr_obligatorio);
                        if (!existe)
                        {
                            cumple = false;
                            if (x == 1)
                            {
                                nombre_campo += row["Etiqueta"].ToString();
                            }
                            else
                            {
                                nombre_campo += " , " + row["Etiqueta"].ToString();
                            }
                            x++;
                            //break;
                        }
                    }
                }
            }

            if (!cumple)
            {
                divAlertError.Visible = true;
                ErrorMessagePrincipal.Text = "Los siguientes campos son obligatorios, favor verifique: " + nombre_campo;
            }
            else
            {
                divAlertCorrecto.Visible = true;
                MensajeCorrectoPrincipal.Text = "Los campos obligatorios han sido validados correctamente.";
            }

            return cumple;
        }

        protected int GeneroExpediente(int no_preingreso)
        {
            var respuesta = 0;

            respuesta = objCNFormulario.GenerarExpediente(no_preingreso);

            return respuesta;
        }

        protected void ConfiguracionEstado()
        {
            if (Session["estado"] != null)
            {
                if (Session["estado"].ToString() == "E")
                {
                    btnGuardar.Visible = false;
                    btnEnviar.Visible = false;
                    gvAnexos.Columns[5].Visible = false;
                }
            }
            
        }

        bool ValidoContraseña()
        {
            bool respuesta = false;

            if (Session["PS"].ToString() == txt_contraseña.Text)
            {
                respuesta = true;
            }

            return respuesta;
        }

        #endregion


    }
}