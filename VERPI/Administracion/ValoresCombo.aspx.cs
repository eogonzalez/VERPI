using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Capa_Negocio.Administracion;
using Capa_Entidad.Administracion;
using System.Data;

namespace VERPI.Administracion
{
    public partial class ValoresCombo : System.Web.UI.Page
    {
        CNValoresCombo objCNValores = new CNValoresCombo();
        CEValoresCombo objCEValores = new CEValoresCombo();

        #region Eventos del Formulario

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var correlativo_campo = 0;
                if (Request.QueryString["cc"] != null)
                {
                    correlativo_campo = Convert.ToInt32(Request.QueryString["cc"].ToString());
                    Session.Add("corrCampo", correlativo_campo);
                }

                Llenar_gvValoresCombo(correlativo_campo);

                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
            }
        }

        protected void gvValoresCombo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = gvValoresCombo.Rows[index];
            int id_valor_combo = Convert.ToInt32(row.Cells[0].Text);

            Session.Add("idValorCbo", id_valor_combo);

            switch (e.CommandName)
            {
                case "modificar":
                    MostrarDatos(id_valor_combo);
                    lkBtn_viewPanel_ModalPopupExtender.Show();
                    break;
                case "eliminar":
                    EliminarDatos(id_valor_combo);
                    Llenar_gvValoresCombo((int)Session["corrCampo"]);
                    break;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int correlativo_campo = 0;
            if (Session["corrCampo"] != null)
            {
                correlativo_campo = (int)Session["corrCampo"];
            }

            int id_valor_combo = 0;
            if (Session["idValorCbo"] != null)
            {
                id_valor_combo = (int)Session["idValorCbo"];

            }

            switch (btnGuardar.CommandName)
            {
                case "Guardar":
                    if (GuardarValor())
                    {
                        Llenar_gvValoresCombo(correlativo_campo);
                        LimpiarPanel();
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al guardar Valor de Combo.";
                    }
                    break;
                case "Editar":
                    if (ActualizarValorCombo(id_valor_combo))
                    {
                        Llenar_gvValoresCombo(correlativo_campo);
                        LimpiarPanel();
                        btnGuardar.Text = "Guardar";
                        btnGuardar.CommandName = "Guardar";
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al actualizar Valor del Combo";
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

        #endregion

        #region Funciones
        
        protected void Llenar_gvValoresCombo(int correlativo_campo)
        {
            var dt = new DataTable();
            dt = objCNValores.SelectValoresCombo(correlativo_campo);

            gvValoresCombo.DataSource = dt;
            gvValoresCombo.DataBind();
        }

        protected void MostrarDatos(int id_valor_combo)
        {
            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            var dt = new DataTable();
            dt = objCNValores.SelectValorCombo(id_valor_combo);
            var row = dt.Rows[0];

            txtTexto.Text = row["texto"].ToString();
            txtValor.Text = row["valor"].ToString();


        }

        protected void EliminarDatos(int id_valor_combo)
        {
            objCNValores.DeleteValorCombo(id_valor_combo);
        }

        protected bool GuardarValor()
        {
            var respuesta = false;

            objCEValores.Correlativo_Campo = (int)Session["corrCampo"];
            objCEValores.Texto = txtTexto.Text;
            objCEValores.Valor = Convert.ToInt32(txtValor.Text);

            respuesta = objCNValores.InsertValorCombo(objCEValores);

            return respuesta;
        }

        protected void LimpiarPanel()
        {
            txtTexto.Text = string.Empty;
            txtValor.Text = string.Empty;
        }

        protected bool ActualizarValorCombo(int id_valor_combo)
        {
            var respuesta = false;

            objCEValores.ID_Valor_Combo = id_valor_combo;
            objCEValores.Texto = txtTexto.Text;
            objCEValores.Valor = Convert.ToInt32(txtValor.Text);

            respuesta = objCNValores.UpdateValorCombo(objCEValores);

            return respuesta;
        }

        #endregion
    }
}