using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Capa_Negocio.Administracion;
using Capa_Entidad.Administracion;


namespace VERPI.Administracion
{
    public partial class CamposFormularios : System.Web.UI.Page
    {
        CNCamposFormularios objCNCampos = new CNCamposFormularios();
        CECamposFormularios objCECampos = new CECamposFormularios();

        #region Eventos del formulario
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cbo_formulario.Enabled = false;
                var no_formulario = 0;
                if (Request.QueryString["nf"] != null)
                {
                    no_formulario = Convert.ToInt32(Request.QueryString["nf"].ToString());
                    Session.Add("NoFormulario", no_formulario);
                }

                Llenar_cbo_formulario();
                Llenar_gvCamposFormulario(no_formulario);
                cbo_formulario.SelectedValue = no_formulario.ToString();

                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
            }
        }

        protected void gvCamposFormulario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = gvCamposFormulario.Rows[index];
            int correlativo_campo = Convert.ToInt32(row.Cells[0].Text);

            Session.Add("CorrelativoCampo", correlativo_campo);

            switch (e.CommandName)
            {
                case "modificar":
                    MostrarDatos(correlativo_campo);
                    this.lkBtn_viewPanel_ModalPopupExtender.Show();
                    break;

                case "eliminar":
                    EliminarDatos(correlativo_campo);
                    Llenar_gvCamposFormulario((int)Session["NoFormulario"]);
                    break;

                case "combo":
                    Response.Redirect("~/Administracion/ValoresCombo.aspx?cc=" + correlativo_campo.ToString());
                    break;

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int no_formulario = 0;
            if (Session["NoFormulario"] != null)
            {
                no_formulario = (int)Session["NoFormulario"];
            }

            int no_correlativo_Campo = 0;
            if (Session["CorrelativoCampo"] != null)
            {
                no_correlativo_Campo = (int)Session["CorrelativoCampo"];
            }


            switch (btnGuardar.CommandName)
            {
                case "Guardar":
                    if (GuardarCampo())
                    {
                        Llenar_gvCamposFormulario(no_formulario);
                        LimpiarPanel();
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al guardar Campo.";
                    }
                    break;
                case "Editar":
                    if (ActualizarCampo(no_correlativo_Campo))
                    {
                        Llenar_gvCamposFormulario(no_formulario);
                        LimpiarPanel();
                        btnGuardar.Text = "Guardar";
                        btnGuardar.CommandName = "Guardar";
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al actualizar Campo.";
                    }
                    break;

            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            LimpiarPanel();
            btnGuardar.Text = "Guardar";
            btnGuardar.CommandName = "Guardar";
        }

        protected void gvCamposFormulario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            if (e.Row.Cells[5] != null)
            {
                var valor = e.Row.Cells[5].Text;

                if (valor != "Combo")
                {
                    e.Row.Cells[7].Controls.Clear();
                }
            }

        }

        #endregion

        #region Funciones

        protected void Llenar_gvCamposFormulario(int no_formulario)
        {
            var dt = new DataTable();
            dt = objCNCampos.SelectCamposFormulario(no_formulario);

            gvCamposFormulario.DataSource = dt;
            gvCamposFormulario.DataBind();

        }

        protected void Llenar_cbo_formulario()
        {
            var dt = new DataTable();
            dt = objCNCampos.SelectFormularios();

            if (dt.Rows.Count > 0)
            {
                cbo_formulario.DataTextField = dt.Columns["nombre"].ToString();
                cbo_formulario.DataValueField = dt.Columns["no_formulario"].ToString();
                cbo_formulario.DataSource = dt;
                cbo_formulario.DataBind();
            }
        }

        protected void LimpiarPanel()
        {
            txtOrden.Text = string.Empty;
            txtEtiqueta.Text = string.Empty;
            txtNombreControl.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txt_nombre_campo_db.Text = string.Empty;
            check_obligatorio.Checked = false;
            txt_ExpresionRegular.Text = string.Empty;
        }

        protected bool GuardarCampo()
        {
            var respuesta = false;

            objCECampos.No_Formulario = getNoFormulario();
            objCECampos.Orden = getOrden();
            objCECampos.Seccion = getSeccion();
            objCECampos.Etiqueta = getEtiqueta();
            objCECampos.NombreControl = getNombreControl();
            objCECampos.TipoControl = getTipoControl();
            objCECampos.Descripcion = getDescripcion();
            objCECampos.ModoTexto = getModoTexto();
            objCECampos.CampoBaseDatosRPI = getCampoBaseDatosRPI();
            objCECampos.Obligatorio = getObligatorio();
            objCECampos.ExpresionRegular = getExpresionRegular();

            respuesta = objCNCampos.InsertCamposFormulario(objCECampos);

            return respuesta;
        }

        protected bool ActualizarCampo(int no_correlativo_campo)
        {
            var respuesta = false;

            objCECampos.No_Correlativo_Campo = no_correlativo_campo;
            objCECampos.No_Formulario = getNoFormulario();
            objCECampos.Orden = getOrden();
            objCECampos.Seccion = getSeccion();
            objCECampos.Etiqueta = getEtiqueta();
            objCECampos.NombreControl = getNombreControl();
            objCECampos.TipoControl = getTipoControl();
            objCECampos.Descripcion = getDescripcion();
            objCECampos.ModoTexto = getModoTexto();
            objCECampos.CampoBaseDatosRPI = getCampoBaseDatosRPI();
            objCECampos.Obligatorio = getObligatorio();
            objCECampos.ExpresionRegular = getExpresionRegular();

            respuesta = objCNCampos.UpdateCampoFormulario(objCECampos);
            
            return respuesta;
        }

        protected void MostrarDatos(int correlativo_campo)
        {
            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            var tbl = new DataTable();
            tbl = objCNCampos.SelectCampoFormulario(correlativo_campo);
            var row = tbl.Rows[0];

            cbo_formulario.SelectedValue = row["no_formulario"].ToString();
            txtOrden.Text = row["no_orden"].ToString();
            cbo_seccion.SelectedValue = row["seccion"].ToString();
            txtEtiqueta.Text = row["etiqueta"].ToString();
            txtNombreControl.Text = row["nombre_control"].ToString();
            cbo_tipoControl.SelectedValue = row["tipo_control"].ToString();
            txtDescripcion.Text = row["descripcion"].ToString();
            cbo_modo_texto.SelectedValue = row["modo_texto"].ToString();
            txt_nombre_campo_db.Text = row["nombre_campo_db"].ToString();
            check_obligatorio.Checked = (bool)row["obligatorio"];
            txt_ExpresionRegular.Text = row["expresion_regular"].ToString();
            
        }

        protected void EliminarDatos(int correlativo_campo)
        {
            objCNCampos.DeleteCampoFormulario(correlativo_campo);
        }

        #endregion

        #region Valores del formulario

        protected int getNoFormulario()
        {
            return Convert.ToInt32(cbo_formulario.SelectedValue.ToString());
        }

        protected int getOrden()
        {
            return Convert.ToInt32( txtOrden.Text);
        }

        protected int getSeccion()
        {
            return Convert.ToInt32(cbo_seccion.SelectedValue.ToString());
        }

        protected string getEtiqueta()
        {
            return txtEtiqueta.Text;
        }

        protected string getNombreControl()
        {
            return txtNombreControl.Text;
        }

        protected int getTipoControl()
        {
            return Convert.ToInt32(cbo_tipoControl.SelectedValue.ToString());
        }

        protected string getDescripcion()
        {
            return txtDescripcion.Text;
        }

        protected string getModoTexto()
        {
            return cbo_modo_texto.SelectedValue;
        }

        protected string getCampoBaseDatosRPI()
        {
            return txt_nombre_campo_db.Text;
        }

        protected bool getObligatorio()
        {
            return check_obligatorio.Checked;
        }

        protected string getExpresionRegular()
        {
            return txt_ExpresionRegular.Text;
        }


        #endregion


    }
}