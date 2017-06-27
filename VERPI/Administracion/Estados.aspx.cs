using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Capa_Entidad.Administracion;
using Capa_Negocio.Administracion;


namespace VERPI.Administracion
{
    public partial class Estados : System.Web.UI.Page
    {
        CNEstados objCNEstados = new CNEstados();
        CEEstados objCEEstados = new CEEstados();

        #region Eventos del formulario

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Llenar_gvEstados();

                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
            }
        }

        protected void gvEstados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEstados.PageIndex = e.NewPageIndex;
            Llenar_gvEstados();
        }

        protected void gvEstados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = gvEstados.Rows[index];
                int id_estado = Convert.ToInt32(row.Cells[0].Text);

                Session.Add("IdEstado", id_estado);

                switch (e.CommandName)
                {
                    case "modificar":
                        MostrarDatos(id_estado);
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        break;
                    case "eliminar":
                        EliminaEstado(id_estado);
                        Llenar_gvEstados();
                        break;
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int id_estado = 0;
            if (Session["IDEstado"] != null)
            {
                id_estado = (Int32)Session["IDEstado"];
            }

            switch (btnGuardar.CommandName)
            {
                case "Editar":
                    if (ActualizarEstado(id_estado))
                    {
                        Llenar_gvEstados();
                        LimpiarPanel();
                        btnGuardar.Text = "Guardar";
                        btnGuardar.CommandName = "Guardar";
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al actualizar estado.";
                    }

                    break;
                case "Guardar":
                    if (GuardarEstado())
                    {
                        LimpiarPanel();
                        Llenar_gvEstados();
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al guardar Estado.";
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

        protected void Llenar_gvEstados()
        {
            var tbl = new DataTable();
            tbl = objCNEstados.SelectEstadosTiempos();
            gvEstados.DataSource = tbl;
            gvEstados.DataBind();

        }

        protected void MostrarDatos(int id_estado)
        {
            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            var tbl = new DataTable();
            tbl = objCNEstados.SelectEstado(id_estado);
            var row = tbl.Rows[0];

            cbo_tipoSolicitud.SelectedValue = row["tipo_tramite"].ToString();
            txtCodigo.Text = row["codigo_estado"].ToString();
            txtNombre.Text = row["descripcion"].ToString();
            txtDiasMax.Text = row["dias_max"].ToString();
            txtDiasMin.Text = row["dias_min"].ToString();
        }

        protected Boolean EliminaEstado(int id_estado)
        {
            var respuesta = false;
            respuesta = objCNEstados.DeleleEstado(id_estado);
            return respuesta;
        }

        protected Boolean ActualizarEstado(int id_estado)
        {
            var respuesta = false;

            objCEEstados.ID_Estado = id_estado;
            objCEEstados.TipoTramite = getTipoSolicitud();
            objCEEstados.Codigo_Estado = getCodigoEstado();
            objCEEstados.Descripcion = getNombreEstado();
            objCEEstados.Dias_Max = getDiasMaximos();
            objCEEstados.Dias_Min = getDiasMinimos();

            respuesta = objCNEstados.UpdateEstado(objCEEstados);
            return respuesta;
        }

        protected void LimpiarPanel()
        {
            txtCodigo.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtDiasMax.Text = string.Empty;
            txtDiasMin.Text = string.Empty;
        }

        protected Boolean GuardarEstado()
        {
            var respuesta = false;

            objCEEstados.TipoTramite = getTipoSolicitud();
            objCEEstados.Codigo_Estado = getCodigoEstado();
            objCEEstados.Descripcion = getNombreEstado();
            objCEEstados.Dias_Max = getDiasMaximos();
            objCEEstados.Dias_Min = getDiasMinimos();

            respuesta = objCNEstados.InsertEstado(objCEEstados);
            return respuesta;
        }

        #endregion

        #region Obtener datos del formulario

        protected int getCodigoEstado()
        {
            return Convert.ToInt32(txtCodigo.Text);
        }

        protected string getNombreEstado()
        {
            return txtNombre.Text;
        }

        protected string getTipoSolicitud()
        {
            return cbo_tipoSolicitud.SelectedValue.ToString();
        }

        protected int getDiasMaximos()
        {
            return Convert.ToInt32(txtDiasMax.Text);
        }

        protected int getDiasMinimos()
        {
            return Convert.ToInt32(txtDiasMin.Text);
        }

        #endregion
    }
}