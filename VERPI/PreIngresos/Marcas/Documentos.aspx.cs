using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Capa_Negocio.General;
using Capa_Entidad.General;

namespace VERPI.PreIngresos.Marcas
{
    public partial class Documentos : System.Web.UI.Page
    {
        CNFormularios objCNFormulario = new CNFormularios();
        CEFormularios objCEFormulario = new CEFormularios();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LlenarPanel();
            }
        }


        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);



            if (ViewState["controlsadded"] != null)
            {
               
                    
                        LlenarPanel();


                

            }

        }

        protected void LlenarPanel()
        {
            var dt_total_campos = new DataTable();
            var cantidad_s1 = 0;
            var cantidad_s2 = 0;
            var cantidad_s3 = 0;
            int no_formulario = 1;

            objCEFormulario.Nombre_Tabla = "M_Campos_Formulario";
            objCEFormulario.No_Formulario = no_formulario;

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
            int cs3 = 1;

            foreach (DataRow row in dt.Rows)
            {/*Recorro los campos del formulario a construir*/

                switch (row["seccion"].ToString())
                {

                    case "4":

                        if (row["tipo_control"].ToString() == "3")
                        {//Si es documento
                            ConstruirControlesCiclo(pnl_seccion_adjuntos, row, cantidad_s3, ref cont, ref cs3);
                        }
                        else
                        {
                            //ConstruirControlesCiclo(pnl_seccion_3, row, cantidad_s3, ref cont, ref cs3);
                        }


                        break;
                }


            }
        }

        protected void ConstruirControlesCiclo(Panel pnl_contenedor, DataRow row, int cantidad_controles, ref int cont, ref int x)
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

            ViewState["controlsadded"] = true;
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
            no_control = 10000 + (int)row["correlativo_campo"];
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


                    break;

                case "2":

                    //Si es dropdowlist
                    DropDownList MiCombo = new DropDownList();
                    MiCombo.ID = identificacion;
                    MiCombo.CssClass = "form-control";
                    MiCombo.ToolTip = row["descripcion"].ToString();
                    //Metodo para llenar el combo
                    pnl_contenedor.Controls.Add(MiCombo);
                    //ViewState["controlsadded"] = true;
                    break;

                case "3":

                    //Si es Adjunto
                    FileUpload MiFileUpload = new FileUpload();
                    MiFileUpload.ID = identificacion;

                    //MiFileUpload.ToolTip = row["descripcion"].ToString();
                    MiFileUpload.EnableViewState = true;
                    //MiFileUpload.ViewStateMode = ViewStateMode.Enabled;

                    pnl_contenedor.Controls.Add(MiFileUpload);
                    //ViewState["controlsadded"] = true;

                    break;
                case "4":
                    //Si es checkbox
                    CheckBox MiCheckbox = new CheckBox();
                    MiCheckbox.ID = identificacion;
                    MiCheckbox.CssClass = row["descripcion"].ToString();
                    pnl_contenedor.Controls.Add(MiCheckbox);
                    //ViewState["controlsadded"] = true;
                    break;

            }
            ViewState["controlsadded"] = true;
            pnl_contenedor.Controls.Add(new LiteralControl("</div>"));

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /*Construyo datatable*/
            DataTable dt_controles = new DataTable();
            dt_controles.Columns.Add("correlativo_campo");
            dt_controles.Columns.Add("nombre_control");
            dt_controles.Columns.Add("valor");
            ObtengoControlesConValores(pnl_seccion_adjuntos, ref dt_controles);
        }

        protected void ObtengoControlesConValores(Panel pnl_contenedor, ref DataTable dt_controles)
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
                if (c is FileUpload)
                {
                    FileUpload flup;
                    flup = (FileUpload)c;

                    string archivo = string.Empty;
                    archivo = flup.FileName;

                    if (flup.HasFile)
                    {
                        divAlertCorrecto.Visible = true;
                        MensajeCorrectoPrincipal.Text = archivo;
                    }

                }

            }

            //return dt_controles;
        }
    }
}