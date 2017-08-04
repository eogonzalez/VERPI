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
using System.Runtime.InteropServices;

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
                    tipo_tramite = Convert.ToInt32(Request.QueryString["cmd"]);
                    Session.Add("TipoTramite", tipo_tramite);

                    Llenar_cbo_tramite(tipo_tramite);
                }

                ConfiguracionInicial();

                int no_formulario = 0;
                if (Request.QueryString["nf"] != null)
                {
                    no_formulario = Convert.ToInt32(Request.QueryString["nf"]);
                    Session.Add("no_formulario", no_formulario);
                }

                int noPreingreso = 0;
                if (Request.QueryString["np"] != null)
                {
                    noPreingreso = Convert.ToInt32(Request.QueryString["np"]);
                    Session.Add("noPreIngreso", noPreingreso);

                }

                if (no_formulario > 0 && noPreingreso > 0)
                {
                    btnGuardar.Visible = true;
                    btnAdjuntar.Visible = true;
                    btnEnviar.Visible = true;
                    btnSalir.Visible = true;
                    btnAdjuntar.Enabled = true;
                    btnEnviar.Enabled = true;

                    cbo_tramite.Enabled = false;
                    //Llenar_cbo_tramite(tipo_tramite);
                    cbo_tramite.SelectedValue = no_formulario.ToString();

                    pnl_seccion_1.Controls.Clear();
                    pnl_seccion_2.Controls.Clear();
                    pnl_seccion_3.Controls.Clear();

                    LlenarFormulario(no_formulario);
                    pnlFormulario.Visible = true;
                    LlenarValoresFormulario(noPreingreso);
                    ConfiguracionEstado();
                }

                               
                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
                btnEnviar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnEnviar, ""));
            }

        }

        protected void CargaFormulario(object sender, EventArgs e)
        {
            btnGuardar.Visible = true;
            btnAdjuntar.Visible = true;
            btnEnviar.Visible = true;
            btnSalir.Visible = true;
            divAlertClase.Visible = false;

            if (Session["noPreIngreso"] != null)
            {
                Session["noPreIngreso"] = null;
            }

            var no_formulario = Convert.ToInt32(cbo_tramite.SelectedValue);
            Session.Add("no_formulario", no_formulario);
            
            if (no_formulario > 0)
            {
                pnl_seccion_1.Controls.Clear();
                pnl_seccion_2.Controls.Clear();
                pnl_seccion_3.Controls.Clear();

                LlenarFormulario(no_formulario);
                pnlFormulario.Visible = true;
            }
            else
            {
                pnlFormulario.Visible = false;
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Session["noPreIngreso"] != null)
            {
                if ((int)Session["noPreIngreso"] > 0)
                {
                    ActualizarFormulario((int)Session["noPreIngreso"]);
                }                
            }
            else
            {
                GuardarFormulario();
            }

        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/#");
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
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

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {
            if (Session["noPreIngreso"] != null)
            {
                //Redirecciono a los documentos adjuntos
                Response.Redirect("~/PreIngresos/Marcas/Documentos.aspx?cmd="+Session["TipoTramite"].ToString() +"&np=" + Session["noPreIngreso"].ToString()+"&nf="+Session["no_formulario"].ToString());
            }
            else
            {
                divAlertCorrecto.Visible = false;

                ErrorMessagePrincipal.Text = "Debe de guardar primero el formulario para poder adjuntar documentos.";
                divAlertError.Visible = true;
            }
        }

        protected void btnIrBandeja_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Bandeja/BandejaUsuario");
        }

        #endregion

        #region Funciones

        protected void ConfiguracionInicial()
        {
            //liEncabezado.Disabled = true;
            //liDatos.Disabled = true;
            //liAnexos.Disabled = true;

            Session.Add("ValidoEnvio", true);
            divContrasenia.Visible = false;

            divAlertClase.Visible = false;

            divAlertCorrecto.Visible = false;
            divAlertError.Visible = false;
            btnAdjuntar.Enabled = false;
            btnEnviar.Enabled = false;

            btnGuardar.Visible = false;
            btnEnviar.Visible = false;
            btnAdjuntar.Visible = false;
            btnSalir.Visible = false;

            pnlFormulario.Visible = false;
        }

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

            /*Si no tiene controles oculto el tab*/
            if (cantidad_s1 == 0)
            {
                liEncabezado.Visible = false;
            }
            else if (cantidad_s2 == 0)
            {
                liDatos.Visible = false;
            }
            else if (cantidad_s3 == 0)
            {
                liAnexos.Visible = false;
            }

            Session.Add("Etiqueta", false);

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
                        ConstruirControlesCiclo(pnl_seccion_3, row, cantidad_s3, ref cont, ref cs3);
                        break;                    
                }

                
            }                                    

        }

        protected void ConstruirControlesCiclo(Panel pnl_contenedor, DataRow row, int cantidad_controles,ref int cont, ref int x)
        {
            if (cantidad_controles >= 6)
            {
                //Agrego Primer div
                if (cont == 1)
                {
                    if (row["tipo_control"].ToString() == "6")
                    {
                        pnl_contenedor.Controls.Add(new LiteralControl("<h4><span class='label label-info'>" + row["etiqueta"].ToString() + "</span></h4>"));
                        Session["Etiqueta"] = true;
                        cont = 0;
                    }
                    else if (row["tipo_control"].ToString() == "8")
                    {
                        pnl_contenedor.Controls.Add(new LiteralControl("<p class='alert alert-success'>" + "AYUDA: "+ row["etiqueta"].ToString() + "</p>"));
                        Session["Etiqueta"] = true;
                        cont = 0;
                    }
                    else
                    {
                        pnl_contenedor.Controls.Add(new LiteralControl("<div class='form-group'>"));
                    }                    
                }

                
                
                if (!(bool)Session["Etiqueta"])
                {
                    //Creo controles                                
                    ConstruirControles(pnl_contenedor, row, cantidad_controles);
                }
                else
                {
                    Session["Etiqueta"] = false;

                    if (cont == 1)
                    {
                        pnl_contenedor.Controls.Add(new LiteralControl("</div>"));
                    }
                    cont = 0;
                }


                if (cont == 2 || cantidad_controles == x)
                {
                    
                    if (row["tipo_control"].ToString() == "6")
                    {
                        pnl_contenedor.Controls.Add(new LiteralControl("</div>"));
                        pnl_contenedor.Controls.Add(new LiteralControl("<h4><span class='label label-info'>" + row["etiqueta"].ToString() + "</span></h4>"));
                    }
                    else if (row["tipo_control"].ToString() == "8")
                    {
                        pnl_contenedor.Controls.Add(new LiteralControl("</div>"));
                        pnl_contenedor.Controls.Add(new LiteralControl("<p class='alert alert-success'>" + "AYUDA: " + row["etiqueta"].ToString() + "</p>"));
                    }
                    else
                    {
                        
                        pnl_contenedor.Controls.Add(new LiteralControl("</div>"));
                    }
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
            bool agregoEtiqueta = true;
            if (row["tipo_control"].ToString() == "6" || row["tipo_control"].ToString() == "8")
            {
                agregoEtiqueta = false;
            }


            if (agregoEtiqueta)
            {
                /*Agrego Label*/
                var label = new Label();
                label.Text = row["etiqueta"].ToString();
                label.CssClass = "control-label col-xs-2";
                pnl_contenedor.Controls.Add(label);
            }

            if (total_campos >= 6)
            {
                if (!(bool)Session["Etiqueta"])
                {
                    pnl_contenedor.Controls.Add(new LiteralControl("<div class='col-xs-4'>"));
                }
                else
                {
                    Session["Etiqueta"] = false;
                }
            }
            else
            {
                pnl_contenedor.Controls.Add(new LiteralControl("<div class='col-xs-9'>"));
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
                            MiTexBox.TextMode = TextBoxMode.Number;
                            break;
                        case "SingleLine":
                            MiTexBox.TextMode = TextBoxMode.SingleLine;
                            break;
                        case "Multiline":
                            MiTexBox.TextMode = TextBoxMode.MultiLine;
                            break;
                        case "Email":
                            MiTexBox.TextMode = TextBoxMode.Email;
                            break;
                    }

                    pnl_contenedor.Controls.Add(MiTexBox);

                    //pnl_contenedor.Controls.Add(new LiteralControl("<button  id='"+no_control.ToString()+"' type='button' class='btn btn-danger' data-toggle='popover' title='Popover title' data-content='And heres some amazing content. It's very engaging.Right ? '>Click to toggle popover</button>"));

                    break;

                case "2":
                    
                    //Si es dropdowlist
                    DropDownList MiCombo = new DropDownList();
                    MiCombo.ID = identificacion;
                    MiCombo.CssClass = "form-control";
                    MiCombo.ToolTip = row["descripcion"].ToString();
                    //Metodo para llenar el combo
                    LlenarCbo(ref MiCombo, no_control-10000, 2);
                    pnl_contenedor.Controls.Add(MiCombo);
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
                    break;
                case "5":

                    //Si es dropdowlist
                    DropDownList MiComboPais = new DropDownList();
                    MiComboPais.ID = identificacion;
                    MiComboPais.CssClass = "form-control";
                    MiComboPais.ToolTip = row["descripcion"].ToString();
                    //Metodo para llenar el combo
                    LlenarCbo(ref MiComboPais, no_control - 10000, 5);
                    pnl_contenedor.Controls.Add(MiComboPais);
                    break;
                //case "6" y "8":
                //Las etiquetas se agregan de otra manera   
                case "7":
                    /* Si es clase de niza*/
                    DropDownList MiComboClase = new DropDownList();
                    MiComboClase.ID = identificacion;
                    MiComboClase.CssClass = "form-control";
                    MiComboClase.ToolTip = row["descripcion"].ToString();
                    MiComboClase.AutoPostBack = true;
                    MiComboClase.TextChanged += new System.EventHandler(LlenoTexto);
                    /*Metodo para llenar el combo*/
                    LlenarCbo(ref MiComboClase, no_control - 10000, 7);
                    pnl_contenedor.Controls.Add(MiComboClase);

                    break;

            }            
            pnl_contenedor.Controls.Add(new LiteralControl("</div>"));            
        }
       
        protected void ObtengoControlesConValores(Panel pnl_contenedor,ref DataTable dt_controles)
        {

            int correlativo_campo = 0;
            string nombre_control = string.Empty;
            int numero_control = 0;
            string valor = string.Empty;

            foreach (Control c in pnl_contenedor.Controls)
            {

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

        }

        protected void LlenarCbo(ref DropDownList combo, int correlativo_campo, int tipo)
        {

            var dt = new DataTable();
            dt = objCNFormulario.SelectDatosCombos(correlativo_campo, tipo);
            combo.DataTextField = dt.Columns["texto"].ToString();
            combo.DataValueField = dt.Columns["valor"].ToString();
            combo.DataSource = dt;
            combo.DataBind();
            combo.Items.Insert(0, new ListItem("Seleccione...", "0"));

        }

        protected void GuardarFormulario()
        {
            divAlertCorrecto.Visible = false;
            divAlertError.Visible = false;

            /*Construyo datatable*/
            DataTable dt_controles = new DataTable();
            dt_controles.Columns.Add("correlativo_campo");
            dt_controles.Columns.Add("nombre_control");
            dt_controles.Columns.Add("valor");

            ObtengoControlesConValores(pnl_seccion_1, ref dt_controles);
            ObtengoControlesConValores(pnl_seccion_2, ref dt_controles);
            ObtengoControlesConValores(pnl_seccion_3, ref dt_controles);

            /*Establezco Valores antes de enviar a guardar los valores*/
            objCEFormulario.Dt_Campos = dt_controles;
            objCEFormulario.ID_Usuario_Solicita = (int)Session["UsuarioID"];
            objCEFormulario.No_Formulario = (int)Session["no_formulario"];

            int NoPreingreso = objCNFormulario.InsertDatosFormularioBorrador(objCEFormulario);
            Session.Add("noPreIngreso", NoPreingreso);
            if (NoPreingreso > 0)
            {
                MensajeCorrectoPrincipal.Text = "Se almaceno su formulario correctamente, su formulario esta en la bandeja como borrador, ahora adjunte los documentos a la solicitud.";
                divAlertCorrecto.Visible = true;

                btnAdjuntar.Enabled = true;
                btnEnviar.Enabled = true;
            }
            else
            {
                divAlertError.Visible = true;
                ErrorMessagePrincipal.Text = "Ha ocurrido un error al almacenar el formulario.";
            }
        }

        protected void ActualizarFormulario(int noPreingreso)
        {
            divAlertCorrecto.Visible = false;
            divAlertError.Visible = false;

            /*Construyo datatable*/
            DataTable dt_controles = new DataTable();
            dt_controles.Columns.Add("correlativo_campo");
            dt_controles.Columns.Add("nombre_control");
            dt_controles.Columns.Add("valor");

            ObtengoControlesConValores(pnl_seccion_1, ref dt_controles);
            ObtengoControlesConValores(pnl_seccion_2, ref dt_controles);
            ObtengoControlesConValores(pnl_seccion_3, ref dt_controles);

            /*Establezco Valores antes de enviar a guardar los valores*/
            objCEFormulario.No_PreIngreso = noPreingreso;
            objCEFormulario.Dt_Campos = dt_controles;
            objCEFormulario.ID_Usuario_Solicita = (int)Session["UsuarioID"];
            objCEFormulario.No_Formulario = (int)Session["no_formulario"];
                        
            if (objCNFormulario.UpdateDatosFormularioBorrador(objCEFormulario))
            {
                MensajeCorrectoPrincipal.Text = "Se actualizo formulario correctamente.";
                divAlertCorrecto.Visible = true;               
            }
            else
            {
                divAlertError.Visible = true;
                ErrorMessagePrincipal.Text = "Ha ocurrido un error al actualizar el formulario.";
            }
        }

        protected void LlenarValoresFormulario(int noPreIngreso)
        {
            var dt_valoresControles = new DataTable();
            dt_valoresControles = objCNFormulario.SelectValoresFormulario(noPreIngreso);
            var estado = objCNFormulario.SelectEstadoPreIngreso(noPreIngreso);
            Session.Add("estado", estado);

            foreach (DataRow row in dt_valoresControles.Rows)
            {
                int correlativo = 10000+(int)row["correlativo_campo"];
                string ID_Control = row["nombre_control"]+"_"+correlativo.ToString();
                string valor = row["valor"].ToString();

                EstablecerValoresCampos(pnl_seccion_1, ID_Control, valor, estado);
                EstablecerValoresCampos(pnl_seccion_2, ID_Control, valor, estado);
                EstablecerValoresCampos(pnl_seccion_3, ID_Control, valor, estado);
            }
        }

        protected void EstablecerValoresCampos(Panel pnl_contenedor, string ID_Control, string Valor, string estado)
        {
            foreach (Control c in pnl_contenedor.Controls)
            {
                if (c is TextBox)
                {
                    TextBox MiTexBox = (TextBox)c;

                    if (MiTexBox.ID == ID_Control)
                    {
                        MiTexBox.Text = Valor;

                        if (estado == "E")
                        {
                            MiTexBox.Enabled = false;
                        }
                        
                        break;
                    }
                }
                else if (c is DropDownList)
                {
                    DropDownList MiCombo = (DropDownList)c;

                    if (MiCombo.ID == ID_Control)
                    {
                        MiCombo.SelectedValue = Valor;

                        if (estado == "E")
                        {
                            MiCombo.Enabled = false;
                        }
                        break;
                    }
                }
                else if (c is CheckBox)
                {
                    CheckBox MiCheck = (CheckBox)c;

                    if (MiCheck.ID == ID_Control)
                    {
                        MiCheck.Checked = Convert.ToBoolean(Valor);

                        if (estado == "E")
                        {
                            MiCheck.Enabled = false;
                        }

                        break;
                    }
                }
            }
        }

        protected bool ValidoCamposObligatorios()
        {
            /*Selecciono valores obligatorios del formulario*/
            var dt_obligatorios = objCNFormulario.SelectCamposObligatorios((int)Session["no_formulario"]);
            var dt_valoresControles = objCNFormulario.SelectValoresFormulario((int)Session["noPreIngreso"]);

            bool cumple = true;
            string nombre_campo = string.Empty;
            int x=1;

            if (dt_obligatorios.Rows.Count > 0)
            {//Si trae valores obligatorios
                                
                foreach (DataRow row in dt_obligatorios.Rows)
                {/*recorro campos obligatorios*/
                    int corr_obligatorio = (int)row["correlativo_campo"];
                    int tipo_control = Convert.ToInt32(row["tipo_control"].ToString());
                    
                    foreach (DataRow rowValor in dt_valoresControles.Rows)
                    {/*Busco si cumple en los datos del formulario*/
                        int corr_valor = (int)rowValor["correlativo_campo"];
                        if ( corr_obligatorio == corr_valor)
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
                                else if (tipo_control == 2 || tipo_control == 5)
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
                ErrorMessagePrincipal.Text = "Los siguientes campos son obligatorios, favor verifique: "+nombre_campo;
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
            if ( Session["estado"].ToString() == "E")
            {
                btnGuardar.Visible = false;
                btnEnviar.Visible = false;
                btnAdjuntar.Text = "Ir a documentos adjuntos ";
            }
        }

        void LlenoTexto(object sender, EventArgs e)
        {
            DropDownList miCbo = (DropDownList)sender;
            int valor = Convert.ToInt32(miCbo.SelectedValue.ToString());

            string textClase = objCNFormulario.SelectDescripcionClase(valor);
            divAlertClase.Visible = true;
            MensajeClase.Text = "AYUDA (Este texto es para indicar las mercancias, que define Niza para las clases tales como): "+textClase;
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