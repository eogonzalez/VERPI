using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Capa_Entidad.General;
using Capa_Negocio.General;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;

namespace VERPI.PreIngresos.Marcas
{
    public partial class PreIngreso : System.Web.UI.Page
    {
        CEFormularios objCEFormulario = new CEFormularios();
        CNFormularios objCNFormulario = new CNFormularios();
        
        #region Eventos del formulario

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tipo_tramite = 0;
                if (Request.QueryString["cmd"] != null)
                {//Si se envia de query string se reasigna el valor      
                    tipo_tramite  = Convert.ToInt32(Request.QueryString["cmd"]);
                    //Session.Add("TipoTramite", tipo_tramite);
                }

                divAlertCorrecto.Visible = false;
                divAlertError.Visible = false;

                pnlFormulario.Visible = false;
                Llenar_cbo_tramite(tipo_tramite);

                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
            }
        }

        protected void CargaFormulario(object sender, EventArgs e)
        {
            
            var no_formulario = Convert.ToInt32(cbo_tramite.SelectedValue);
            Session.Add("no_formulario", no_formulario);
            

            if (no_formulario > 0)
            {
                pnl_seccion_1.Controls.Clear();
                pnl_seccion_2.Controls.Clear();
                pnl_seccion_3.Controls.Clear();


                LlenarFormulario(no_formulario);
                pnlFormulario.Visible = true;
                //ViewState["controlsadded"] = null;
            }
            else
            {
                pnlFormulario.Visible = false;
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            divAlertCorrecto.Visible = false;
            divAlertError.Visible = false;

            DataTable dt = new DataTable();
            dt = ObtengoControlesConValores();

            objCEFormulario.Dt_Campos = dt;
            objCEFormulario.ID_Usuario_Solicita = (int)Session["UsuarioID"];
            objCEFormulario.No_Formulario = (int)Session["no_formulario"];

            //if (objCNFormulario.InsertDatosFormularioBorrador(objCEFormulario))
            //{
            //    MensajeCorrectoPrincipal.Text = "Se almacenaron los campos.";

            //    if (GuardarAnexos())
            //    {
            //        MensajeCorrectoPrincipal.Text += " Se han almacenado los documentos anexos.";
            //    }
            //    else
            //    {
            //        ErrorMessagePrincipal.Text = "Ha ocurrido un error al cargar archivos anexos.";
            //        divAlertError.Visible = true;
            //    }


            //    divAlertCorrecto.Visible = true;
                
            //}
            //else
            //{
            //    divAlertError.Visible = true;
            //    ErrorMessagePrincipal.Text = "Ha ocurrido un error al almacenar el formulario.";
            //}


            if (GuardarAnexos())
            {
                MensajeCorrectoPrincipal.Text += " Se han almacenado los documentos anexos.";
            }
            else
            {
                ErrorMessagePrincipal.Text = "Ha ocurrido un error al cargar archivos anexos.";
                divAlertError.Visible = true;
            }

        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {

        }

        protected void gvAnexos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {

        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            if (ViewState["controlsadded"] != null)
            {
                if (Session["no_formulario"] != null)
                {
                    var no_formulario = (int)Session["no_formulario"];
                    if (no_formulario > 0)
                    {
                        LlenarFormulario(no_formulario);
                        pnlFormulario.Visible = true;
                    }
                    else
                    {
                        pnlFormulario.Visible = false;
                    }
                }

            }
        }

        #endregion

        #region Funciones

        protected void Llenar_cbo_tramite(int tipo_tramite)
        {
            var tb = new DataTable();
            
            tb = objCNFormulario.SelectFormularios(tipo_tramite);

            if (tb.Rows.Count > 0)
            {



                cbo_tramite.DataTextField = tb.Columns["nombre"].ToString();
                cbo_tramite.DataValueField = tb.Columns["no_formulario"].ToString();
                cbo_tramite.DataSource = tb;                
                cbo_tramite.DataBind();

                cbo_tramite.Items.Insert(0, new ListItem("Seleccione Formulario", "0"));
            }
        }

        protected void LlenarFormulario(int no_formulario)
        {
            objCEFormulario.Nombre_Tabla = "M_Campos_Formulario";

            //var no_formulario = 1;
            objCEFormulario.No_Formulario = no_formulario;
          
            var dt_total_campos = new DataTable();
            var cantidad_s1 = 0;
            var cantidad_s2 = 0;
            var cantidad_s3 = 0;
            dt_total_campos = objCNFormulario.SelectCantidadCamposFormulario(no_formulario);



            foreach (DataRow rowc in dt_total_campos.Rows)
            {
                switch (rowc["seccion"].ToString())
                {
                    case "1":
                        if (rowc["total"] != null)
                        {
                            cantidad_s1 = (int)rowc["total"];
                        }
                        
                        break;
                    case "2":
                        if (rowc["total"] != null)
                        {
                            cantidad_s2 = (int)rowc["total"];
                        }

                        
                        break;
                    case "3":
                        if (rowc["total"] != null)
                        {
                            cantidad_s3 = (int)rowc["total"];
                        }                        
                        break;
                }
            }

            var dt = new DataTable();
            dt = objCNFormulario.SelectCamposFormulario(objCEFormulario);

            int cont = 1;
            int cs1 = 1;
            int cs2 = 1;
            int cs3 = 1;

            /*Creo datatable de adjuntos*/
            
            var dt_anexos = new DataTable();
            dt_anexos.Columns.Add("correlativo_campo");
            dt_anexos.Columns.Add("Etiqueta");

            gvAnexos.DataSource = dt_anexos;
            gvAnexos.DataBind();

            foreach (DataRow row in dt.Rows)
            {/*Recorro los campos del formulario a construir*/
                
                switch (row["seccion"].ToString())
                {
                    case "1":
                        ConstruirControlesCiclo(pnl_seccion_1, row, cantidad_s1, ref cont, ref cs1);
                        break;
                    case "2":
                        ConstruirControlesCiclo(pnl_seccion_2, row, cantidad_s2, ref cont, ref cs2);
                        break;
                    case "3":
                        if (row["tipo_control"].ToString() == "3")
                        {//Si es documento
                            Llenar_gvAnexos(dt_anexos, row);
                        }
                        else
                        {
                            ConstruirControlesCiclo(pnl_seccion_3, row, cantidad_s3, ref cont, ref cs3);
                        }
                        
                        
                        break;                    
                }

                
            }
            Session.Add("PNL1", pnl_seccion_1);
            
            int cantidad = pnl_seccion_1.Controls.Count;
            int otra;

        }

        protected void ConstruirControlesCiclo(Panel pnl_contenedor, DataRow row, int cantidad_controles,ref int cont, ref int x)
        {
            if (cantidad_controles >= 6)
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
        }

        protected void ConstruirControles(Panel pnl_contenedor, DataRow row, int total_campos)
        {
            /*Agrego Label*/
            var label = new Label();
            label.Text = row["etiqueta"].ToString();

            label.CssClass = "control-label col-xs-2";
            pnl_contenedor.Controls.Add(label);

            if (total_campos >= 6)
            {
                pnl_contenedor.Controls.Add(new LiteralControl("<div class='col-xs-4'>"));
            }
            else
            {
                pnl_contenedor.Controls.Add(new LiteralControl("<div class='col-xs-10'>"));
            }

            int no_control = 0;
            no_control = 10000+(int)row["correlativo_campo"];
            var identificacion = string.Empty;
            identificacion = row["nombre_control"].ToString() + "_" + no_control.ToString();

            switch (row["tipo_control"].ToString())
            {
                case "1":
                    //Si es textbox
                    TextBox MiTexBox = new TextBox();
                    MiTexBox.ID = identificacion;
                    MiTexBox.Text = "";
                    MiTexBox.CssClass = "form-control";
                    MiTexBox.ToolTip = row["descripcion"].ToString();
                    MiTexBox.ViewStateMode = ViewStateMode.Disabled;
                    
                    switch (row["modo_texto"].ToString())
                    {
                        case "Number":
                            //panel.Controls.Add(new LiteralControl("<div class='col-xs-2'>"));
                            MiTexBox.TextMode = TextBoxMode.Number;
                            break;
                        case "SingleLine":
                            //panel.Controls.Add(new LiteralControl("<div class='col-xs-10'>"));
                            MiTexBox.TextMode = TextBoxMode.SingleLine;
                            break;
                        case "Multiline":
                            //panel.Controls.Add(new LiteralControl("<div class='col-xs-10'>"));
                            MiTexBox.TextMode = TextBoxMode.MultiLine;
                            break;
                        case "Email":
                            //panel.Controls.Add(new LiteralControl("<div class='col-xs-10'>"));
                            MiTexBox.TextMode = TextBoxMode.Email;
                            break;
                    }

                    pnl_contenedor.Controls.Add(MiTexBox);
                    ViewState["controlsadded"] = true;                                        
                    //EnsureChildControls();
                    break;

                case "2":
                    
                    //Si es dropdowlist
                    DropDownList MiCombo = new DropDownList();
                    MiCombo.ID = identificacion;
                    MiCombo.CssClass = "form-control";
                    MiCombo.ToolTip = row["descripcion"].ToString();
                    //Metodo para llenar el combo
                    pnl_contenedor.Controls.Add(MiCombo);
                    EnsureChildControls();
                    break;

                case "3":
                    //Si es Adjunto
                    
                    break;
                case "4":
                    //Si es checkbox
                    CheckBox MiCheckbox = new CheckBox();
                    MiCheckbox.ID = identificacion;
                    MiCheckbox.CssClass = row["descripcion"].ToString();
                    pnl_contenedor.Controls.Add(MiCheckbox);
                    EnsureChildControls();
                    break;
                
            }

            pnl_contenedor.Controls.Add(new LiteralControl("</div>"));
            
        }
       
        protected void Llenar_gvAnexos(DataTable dt_anexos, DataRow row)
        {
            DataRow row_Anexos = dt_anexos.NewRow();
            row_Anexos["correlativo_campo"] = row["correlativo_campo"];
            row_Anexos["Etiqueta"] = row["Etiqueta"];

            dt_anexos.Rows.Add(row_Anexos);

            gvAnexos.DataSource = dt_anexos;
            gvAnexos.DataBind();
        }

        protected DataTable ObtengoControlesConValores()
        {
            /*Contruyo datatable*/
            DataTable dt_controles = new DataTable();
            dt_controles.Columns.Add("correlativo_campo");
            dt_controles.Columns.Add("nombre_control");
            dt_controles.Columns.Add("valor");

            
            int correlativo_campo = 0;
            string nombre_control = string.Empty;
            int numero_control = 0;
            string valor = string.Empty;

            foreach (Control c in pnl_seccion_1.Controls)
            {
                string tipo = Controls.GetType().ToString();

                if (c is TextBox)
                {
                    TextBox tbx;
                    tbx = (TextBox)c;
                                        
                    nombre_control = tbx.ID.Remove(tbx.ID.Length - 6);                    
                    numero_control = Convert.ToInt32(tbx.ID.Substring(tbx.ID.Length - 5, 5));
                    correlativo_campo = numero_control - 10000;
                    valor = tbx.Text;

                    DataRow row = dt_controles.NewRow();
                    row["correlativo_campo"] = correlativo_campo;
                    row["nombre_control"] = nombre_control;
                    row["valor"] = valor;
                    dt_controles.Rows.Add(row);
                }
                else if(c is DropDownList)
                {
                    DropDownList ddl;
                    ddl = (DropDownList)c;

                    nombre_control = ddl.ID.Remove(ddl.ID.Length-6);
                    numero_control = Convert.ToInt32(ddl.ID.Substring(ddl.ID.Length - 5, 5));
                    correlativo_campo = numero_control - 10000;
                    valor = ddl.SelectedValue;

                    DataRow row = dt_controles.NewRow();
                    row["correlativo_campo"] = correlativo_campo;
                    row["nombre_control"] = nombre_control;
                    row["valor"] = valor;
                    dt_controles.Rows.Add(row);

                }
                else if (c is CheckBox)
                {
                    CheckBox chk;
                    chk = (CheckBox)c;

                    nombre_control = chk.ID.Remove(chk.ID.Length - 6);
                    numero_control = Convert.ToInt32(chk.ID.Substring(chk.ID.Length - 5, 5));
                    correlativo_campo = numero_control - 10000;
                    valor = chk.Checked.ToString();

                    DataRow row = dt_controles.NewRow();
                    row["correlativo_campo"] = correlativo_campo;
                    row["nombre_control"] = nombre_control;
                    row["valor"] = valor;
                    dt_controles.Rows.Add(row);
                }

            }

            foreach (Control c in pnl_seccion_2.Controls)
            {
                string tipo = Controls.GetType().ToString();

                if (c is TextBox)
                {
                    TextBox tbx;
                    tbx = (TextBox)c;

                    nombre_control = tbx.ID.Remove(tbx.ID.Length - 6);
                    numero_control = Convert.ToInt32(tbx.ID.Substring(tbx.ID.Length - 5, 5));
                    correlativo_campo = numero_control - 10000;
                    valor = tbx.Text;

                    DataRow row = dt_controles.NewRow();
                    row["correlativo_campo"] = correlativo_campo;
                    row["nombre_control"] = nombre_control;
                    row["valor"] = valor;
                    dt_controles.Rows.Add(row);
                }
                else if (c is DropDownList)
                {
                    DropDownList ddl;
                    ddl = (DropDownList)c;

                    nombre_control = ddl.ID.Remove(ddl.ID.Length - 6);
                    numero_control = Convert.ToInt32(ddl.ID.Substring(ddl.ID.Length - 5, 5));
                    correlativo_campo = numero_control - 10000;
                    valor = ddl.SelectedValue;

                    DataRow row = dt_controles.NewRow();
                    row["correlativo_campo"] = correlativo_campo;
                    row["nombre_control"] = nombre_control;
                    row["valor"] = valor;
                    dt_controles.Rows.Add(row);

                }
                else if (c is CheckBox)
                {
                    CheckBox chk;
                    chk = (CheckBox)c;

                    nombre_control = chk.ID.Remove(chk.ID.Length - 6);
                    numero_control = Convert.ToInt32(chk.ID.Substring(chk.ID.Length - 5, 5));
                    correlativo_campo = numero_control - 10000;
                    valor = chk.Checked.ToString();

                    DataRow row = dt_controles.NewRow();
                    row["correlativo_campo"] = correlativo_campo;
                    row["nombre_control"] = nombre_control;
                    row["valor"] = valor;
                    dt_controles.Rows.Add(row);
                }

            }

            foreach (Control c in pnl_seccion_3.Controls)
            {
                string tipo = Controls.GetType().ToString();

                if (c is TextBox)
                {
                    TextBox tbx;
                    tbx = (TextBox)c;

                    nombre_control = tbx.ID.Remove(tbx.ID.Length - 6);
                    numero_control = Convert.ToInt32(tbx.ID.Substring(tbx.ID.Length - 5, 5));
                    correlativo_campo = numero_control - 10000;
                    valor = tbx.Text;

                    DataRow row = dt_controles.NewRow();
                    row["correlativo_campo"] = correlativo_campo;
                    row["nombre_control"] = nombre_control;
                    row["valor"] = valor;
                    dt_controles.Rows.Add(row);
                }
                else if (c is DropDownList)
                {
                    DropDownList ddl;
                    ddl = (DropDownList)c;

                    nombre_control = ddl.ID.Remove(ddl.ID.Length - 6);
                    numero_control = Convert.ToInt32(ddl.ID.Substring(ddl.ID.Length - 5, 5));
                    correlativo_campo = numero_control - 10000;
                    valor = ddl.SelectedValue;

                    DataRow row = dt_controles.NewRow();
                    row["correlativo_campo"] = correlativo_campo;
                    row["nombre_control"] = nombre_control;
                    row["valor"] = valor;
                    dt_controles.Rows.Add(row);

                }
                else if (c is CheckBox)
                {
                    CheckBox chk;
                    chk = (CheckBox)c;

                    nombre_control = chk.ID.Remove(chk.ID.Length - 6);
                    numero_control = Convert.ToInt32(chk.ID.Substring(chk.ID.Length - 5, 5));
                    correlativo_campo = numero_control - 10000;
                    valor = chk.Checked.ToString();

                    DataRow row = dt_controles.NewRow();
                    row["correlativo_campo"] = correlativo_campo;
                    row["nombre_control"] = nombre_control;
                    row["valor"] = valor;
                    dt_controles.Rows.Add(row);
                }
                else if (c is GridView)
                {
                    string entragrid = "0";
                }


            }

            return dt_controles;
        }

        protected bool GuardarAnexos()
        {
            var respuesta = false;

            foreach (GridViewRow row in gvAnexos.Rows)
            {
                int correlativo_campo = Convert.ToInt32(row.Cells[0].Text);                
                string nombre_campo = row.Cells[1].Text;

                int tipo_tramite = 0;
                if (Request.QueryString["cmd"] != null)
                {//Si se envia de query string se reasigna el valor      
                    tipo_tramite = Convert.ToInt32(Request.QueryString["cmd"]);
                    Session.Add("TipoTramite", tipo_tramite);
                }

                FileUpload fu = (FileUpload)row.Cells[2].FindControl("flup");

                if (fu.HasFile)
                {
                    string archivo = Path.GetFileName(fu.PostedFile.FileName);
                }

                if (fu.PostedFile.FileName != "")
                {
                    string archivo = Path.GetFileName(fu.PostedFile.FileName);
                }

            }


            //for (int i = 0; i < gvAnexos.Rows.Count; i++)
            //{
            //    int correlativo_campo = Convert.ToInt32(gvAnexos.Rows[i].Cells[0].Text);
            //    string nombre_campo = gvAnexos.Rows[i].Cells[1].Text;
            //    FileUpload FileUpload = gvAnexos.Rows[i].Cells[2].FindControl("flup") as FileUpload;

            //    if (FileUpload.HasFile)
            //    {
            //        string archivo = Path.GetFileName(FileUpload.PostedFile.FileName);
            //    }

            //    if (FileUpload.PostedFile.FileName != "")
            //    {
            //        string archivo = Path.GetFileName(FileUpload.PostedFile.FileName);
            //    }

            //}

            return respuesta;
        }

        protected Boolean GuardarDocumento(int id_solicitud, string tipo_tramite, ref int correlativo, int tipo_requisito, FileUpload FileUpload_Anexo)
        {
            var respuesta = false;
            correlativo++;
            string carpeta = Path.Combine(Request.PhysicalApplicationPath, "doctos");
            string prefijo = id_solicitud.ToString() + "_" + tipo_tramite + "_" + tipo_requisito.ToString() + "_" + correlativo.ToString();

            if (FileUpload_Anexo.PostedFile.FileName == "")
            {

                ErrorMessagePrincipal.Text += "No ha seleccionado ningun archivo.";
            }
            else
            {
                string extension = Path.GetExtension(FileUpload_Anexo.PostedFile.FileName);

                switch (extension.ToLower())
                {
                    case ".pdf":
                        break;
                    case ".jpg":
                        break;
                    case ".png":
                        break;
                    default:
                        ErrorMessagePrincipal.Text += "Extension no valida.";
                        return false;
                }

                try
                {
                    string archivo = Path.GetFileName(FileUpload_Anexo.PostedFile.FileName);
                    string carpeta_final = Path.Combine(carpeta, prefijo /*+ archivo*/+ extension);
                    FileUpload_Anexo.PostedFile.SaveAs(carpeta_final);
                    //Archivo copiado correctamente
                    respuesta = true;

                    //Guardarficha de documento
                    respuesta = GuardarFichaDocumentoAnexo(id_solicitud, tipo_tramite , archivo, prefijo /*+ archivo*/+ extension, carpeta_final);


                }
                catch (Exception ex)
                {

                    ErrorMessagePrincipal.Text += "Error: " + ex.Message;
                    divAlertError.Visible = true;
                }
            }

            return respuesta;
        }

        protected Boolean GuardarFichaDocumentoAnexo(int id_solicitud, string tipo_tramite, string DocumentoOriginal, string DocumentoSistema, string Path)
        {
            var respuesta = false;
            //objCEVerificacion.Estado = Session["STDEX"].ToString();
            //objCEVerificacion.ID_Solicitud = id_solicitud;
            //objCEVerificacion.TipoSolicitud = cmd;
            //objCEVerificacion.IdRequisito = getTipoRequisito();

            //objCEVerificacion.OficioSAT_Check = getOficioSATCheck();
            //objCEVerificacion.IDPrefijoSAT = getIdPrefijoSAT();

            //objCEVerificacion.PrefijoSAT = getPrefijoSAT();
            //objCEVerificacion.numeroOficioSAT = getNumeroOficioSAT();
            //objCEVerificacion.anioOficioSAT = getAnioOficioSAT();
            //objCEVerificacion.numeroReferencia = getNumeroReferencia();

            //objCEVerificacion.ObservacionesAnexo = getObservacionesAnexo();

            //objCEVerificacion.NombreDocumentoOriginal = DocumentoOriginal;
            //objCEVerificacion.NombreDocumentoSistema = DocumentoSistema;
            //objCEVerificacion.Path = Path;

            //respuesta = objCNVerificacion.InsertDocumentoAnexo(objCEVerificacion);

            return respuesta;
        }


        #endregion

        protected void gvAnexos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowid = Convert.ToInt32(gvAnexos.DataKeys[e.RowIndex].Value);

            FileUpload fileupload = gvAnexos.Rows[e.RowIndex].FindControl("flup") as FileUpload;

            if (fileupload.HasFile)
            {

            }

            gvAnexos.EditIndex = -1;
            

        }

    }
}