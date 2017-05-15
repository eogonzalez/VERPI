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
    public partial class Formularios : System.Web.UI.Page
    {
        CNFormularios objCNFormularios = new CNFormularios();
        CEFormularios objCEFormulario = new CEFormularios();

        #region Eventos del formulario

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Llenar_gvFormularios();
                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
            }
        }

        protected void gvFormularios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = gvFormularios.Rows[index];
            int no_formulario = Convert.ToInt32(row.Cells[0].Text);

            Session.Add("NoFormulario", no_formulario);

            switch (e.CommandName)
            {
                case "modificar":
                    MostrarDatos(no_formulario);
                    this.lkBtn_viewPanel_ModalPopupExtender.Show();
                    break;

                case "eliminar":
                    EliminarDatos(no_formulario);
                    Llenar_gvFormularios();
                    break;

                case "campos":
                    Response.Redirect("~/Administracion/CamposFormularios.aspx?nf="+no_formulario.ToString());
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

            switch (btnGuardar.CommandName)
            {
                case "Guardar":
                    if (GuardarFormulario())
                    {
                        Llenar_gvFormularios();
                        LimpiarPanel();
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al guardar Formulario.";
                    }
                    break;
                case "Editar":
                    if (ActualizarFormulario(no_formulario))
                    {
                        Llenar_gvFormularios();
                        LimpiarPanel();
                        btnGuardar.Text = "Guardar";
                        btnGuardar.CommandName = "Guardar";
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al actualizar Formulario.";
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

        protected void Llenar_gvFormularios()
        {
            var tbl = new DataTable();
            tbl = objCNFormularios.SelectFormularios();
            gvFormularios.DataSource = tbl;
            gvFormularios.DataBind();
        }

        protected void LimpiarPanel()
        {
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
        }

        protected bool GuardarFormulario()
        {
            var respuesta = false;

            objCEFormulario.TipoTramite = getTipoTramite();
            objCEFormulario.NombreFormulario = getNombreFormulario();
            objCEFormulario.Descripcion = getDescripcion();

            respuesta = objCNFormularios.InsertFormulario(objCEFormulario);

            return respuesta;
        }

        protected bool ActualizarFormulario(int no_formulario)
        {
            var respuesta = false;
            objCEFormulario.No_Formulario = no_formulario;
            objCEFormulario.TipoTramite = getTipoTramite();
            objCEFormulario.NombreFormulario = getNombreFormulario();
            objCEFormulario.Descripcion = getDescripcion();

            respuesta = objCNFormularios.UpdateFormulario(objCEFormulario);
            return respuesta;
        }

        protected void MostrarDatos(int no_formulario)
        {
            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            var tbl = new DataTable();
            tbl = objCNFormularios.SelectFormulario(no_formulario);
            var row = tbl.Rows[0];

            cbo_tipo_tramite.SelectedValue = row["tipo_tramite"].ToString();
            txtNombre.Text = row["nombre"].ToString();
            txtDescripcion.Text = row["descripcion_formulario"].ToString();

        }

        protected void EliminarDatos(int no_formulario)
        {
            objCNFormularios.DeleteFormulario(no_formulario);
        }

        #endregion

        #region Obtener valores del formulario

        protected int getTipoTramite()
        {
            return Convert.ToInt32(cbo_tipo_tramite.SelectedValue);
        }

        protected string getNombreFormulario()
        {
            return txtNombre.Text;
        }

        protected string getDescripcion()
        {
            return txtDescripcion.Text;
        }

        #endregion
    }
}