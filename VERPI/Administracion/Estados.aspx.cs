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
                cbo_estadoAnterior.Enabled = false;
                cbo_estadoSiguiente.Enabled = false;
                cbo_Formulario.Enabled = false;

                divAlertError.Visible = false;

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
                        cbo_tipoSolicitud.Enabled = false;
                        cbo_estadoAnterior.Enabled = false;
                        cbo_estadoSiguiente.Enabled = false;
                        cbo_Formulario.Enabled = false;
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
                        divAlertError.Visible = true;
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessagePrincipal.Text = "Ha ocurrido un error al actualizar estado.";
                    }

                    break;
                case "Guardar":
                    int tipoTramite = Convert.ToInt32(getTipoSolicitud());
                    int noFormulario = Convert.ToInt32(getNoFormulario());

                    if (objCNEstados.ExisteEstado(tipoTramite, noFormulario))
                    {//Si ya existe estado
                        if (Convert.ToInt32(getEstadoAnterior()) > 0)
                        {//Si selecciono estado anterior
                            //Verifico si codigo anterior existe
                            int codigoEstadoAnterior = objCNEstados.SelectCodigoEstado(Convert.ToInt32(getEstadoAnterior()));

                            if (GuardarEstado())
                            {
                                LimpiarPanel();
                                Llenar_gvEstados();
                            }
                            else
                            {
                                divAlertError.Visible = true;
                                lkBtn_viewPanel_ModalPopupExtender.Show();
                                if (Session["MsgError"] != null)
                                {
                                    ErrorMessagePrincipal.Text = Session["MsgError"].ToString();
                                }
                                
                            }                         
                        }
                        else
                        {
                            //Se muestra error ya que debe de seleccionar estado anterior
                            divAlertError.Visible = true;
                            lkBtn_viewPanel_ModalPopupExtender.Show();
                            ErrorMessagePrincipal.Text = "Debe seleccionar estado anterior.";
                        }
                    }
                    else
                    {//Si es el primer estado
                        if (GuardarEstado())
                        {
                            LimpiarPanel();
                            Llenar_gvEstados();
                        }
                        else
                        {
                            divAlertError.Visible = true;
                            lkBtn_viewPanel_ModalPopupExtender.Show();
                            ErrorMessagePrincipal.Text = "Ha ocurrido un error al guardar Estado.";
                        }
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

        protected void cbo_tipoSolicitud_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Llenar Combo de Formularios
            Llenar_cboFormularios(Convert.ToInt32(getTipoSolicitud().ToString()));
        }

        protected void cbo_Formulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Lleno Combo de Estados
            int noTipoSolicitud = Convert.ToInt32(getTipoSolicitud().ToString());
            int noFormulario = Convert.ToInt32(getNoFormulario().ToString());
            Llenar_cboEstados(noTipoSolicitud, noFormulario);
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

            int tipoTramite = Convert.ToInt32(getTipoSolicitud());
            Llenar_cboFormularios(tipoTramite);

            cbo_Formulario.SelectedValue = row["no_formulario"].ToString();

            Llenar_cboEstados(tipoTramite, Convert.ToInt32(row["no_formulario"].ToString()));

            cbo_estadoAnterior.SelectedValue = row["id_estadoAnterior"].ToString();
            cbo_estadoSiguiente.SelectedValue = row["id_estadoSiguiente"].ToString();
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
            objCEEstados.Descripcion = getNombreEstado();
            objCEEstados.Dias_Max = getDiasMaximos();
            objCEEstados.Dias_Min = getDiasMinimos();

            respuesta = objCNEstados.UpdateEstado(objCEEstados);
            return respuesta;
        }

        protected void LimpiarPanel()
        {
            //txtCodigo.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtDiasMax.Text = string.Empty;
            txtDiasMin.Text = string.Empty;

            cbo_tipoSolicitud.SelectedValue = "0";
            cbo_Formulario.SelectedValue = "0";

            cbo_estadoAnterior.SelectedValue = "0";
            cbo_estadoSiguiente.SelectedValue = "0";

            cbo_estadoAnterior.Enabled = false;
            cbo_estadoSiguiente.Enabled = false;
            cbo_Formulario.Enabled = false;
            divAlertError.Visible = false;
            cbo_tipoSolicitud.Enabled = true;
        }

        protected Boolean GuardarEstado()
        {
            var respuesta = false;
            int noFormulario = Convert.ToInt32(getNoFormulario());
            int tipoTramite = Convert.ToInt32(getTipoSolicitud());

            objCEEstados.NoFormulario = noFormulario;
            objCEEstados.TipoTramite = getTipoSolicitud();
            //objCEEstados.Codigo_Estado = getCodigoEstado();
            
            objCEEstados.Descripcion = getNombreEstado();
            objCEEstados.Dias_Max = getDiasMaximos();
            objCEEstados.Dias_Min = getDiasMinimos();

            if (objCNEstados.ExisteEstado(tipoTramite, noFormulario))
            {//Si existe Estado

                int codigoEstadoAnterior = objCNEstados.SelectCodigoEstado(Convert.ToInt32(getEstadoAnterior()));
                objCEEstados.ID_EstadoAnterior = Convert.ToInt32(getEstadoAnterior());
                objCEEstados.EstadoAnterior = codigoEstadoAnterior;

                int codigoEstadoSiguiente = 0;

                if (Convert.ToInt32(getEstadoSiguiente()) > 0)
                {//Si selecciono estado siguiente
                    codigoEstadoSiguiente = objCNEstados.SelectCodigoEstado(Convert.ToInt32(getEstadoSiguiente()));
                    objCEEstados.ID_EstadoSiguiente = Convert.ToInt32(getEstadoSiguiente());                   
                    objCEEstados.EstadoSiguiente = codigoEstadoSiguiente;

                    if (codigoEstadoAnterior < codigoEstadoSiguiente)
                    {//Si estado anterior es mayor que estado siguiente
                        objCEEstados.Codigo_Estado = (codigoEstadoAnterior + codigoEstadoSiguiente) / 2;                                                 
                    }
                    else
                    {
                        //Error el estado siguiente debe de ser mayor 
                        //divAlertError.Visible = true;
                        Session.Add("MsgError"," El estado Siguiente debe de ser mayor. ");
                        //lkBtn_viewPanel_ModalPopupExtender.Show();
                    }

                }
                else
                {
                    objCEEstados.Codigo_Estado = codigoEstadoAnterior + 100;
                }
                
            }
            else
            {//Es el primer Estado
                objCEEstados.Codigo_Estado = 100;
                //objCEEstados.EstadoAnterior = 0;
                //objCEEstados.EstadoSiguiente = 0;
            }


            if (!objCNEstados.ExisteCodigoEstado(tipoTramite, noFormulario, objCEEstados.Codigo_Estado))
            {
                respuesta = objCNEstados.InsertEstado(objCEEstados);
            }
            else
            {
                //divAlertError.Visible = true;
                //lkBtn_viewPanel_ModalPopupExtender.Show();
                //ErrorMessagePrincipal.Text = "Ya existe un estado siguiente para el estado anterior seleccionado.";
                if (Session["MsgError"] != null)
                {
                    Session["MsgError"] += "Ya existe un estado siguiente para el estado anterior seleccionado.";
                }
                else
                {
                    Session.Add("MsgError", "Ya existe un estado siguiente para el estado anterior seleccionado.");
                }
                
            }


            return respuesta;
        }

        protected void Llenar_cboEstados(int tipoTramite, int noFormulario)
        {
            var tb = new DataTable();

            tb = objCNEstados.SelectEstadosTipoTramite(tipoTramite, noFormulario);

            if (tb.Rows.Count > 0)
            {

                cbo_estadoAnterior.DataTextField = tb.Columns["descripcion"].ToString();
                cbo_estadoAnterior.DataValueField = tb.Columns["id_estado"].ToString();
                cbo_estadoAnterior.DataSource = tb;
                cbo_estadoAnterior.DataBind();

                cbo_estadoAnterior.Items.Insert(0, new ListItem("Seleccione Estado", "0"));

                cbo_estadoSiguiente.DataTextField = tb.Columns["descripcion"].ToString();
                cbo_estadoSiguiente.DataValueField = tb.Columns["id_estado"].ToString();
                cbo_estadoSiguiente.DataSource = tb;
                cbo_estadoSiguiente.DataBind();

                cbo_estadoSiguiente.Items.Insert(0, new ListItem("Seleccione Estado", "0"));

                cbo_estadoAnterior.Enabled = true;
                cbo_estadoSiguiente.Enabled = true;
            }
            else
            {
                cbo_estadoAnterior.Enabled = false;
                cbo_estadoSiguiente.Enabled = false;

                cbo_estadoAnterior.Items.Insert(0, new ListItem("Seleccione Estado", "0"));
                cbo_estadoSiguiente.Items.Insert(0, new ListItem("Seleccione Estado", "0"));
            }

            lkBtn_viewPanel_ModalPopupExtender.Show();
        }

        protected void Llenar_cboFormularios(int tipoTramite)
        {
            var tb = new DataTable();

            tb = objCNEstados.SelectFormularios(tipoTramite);

            if (tb.Rows.Count > 0)
            {

                cbo_Formulario.DataTextField = tb.Columns["nombre"].ToString();
                cbo_Formulario.DataValueField = tb.Columns["no_formulario"].ToString();
                cbo_Formulario.DataSource = tb;
                cbo_Formulario.DataBind();

                cbo_Formulario.Items.Insert(0, new ListItem("Seleccione Formulario", "0"));
                cbo_Formulario.Enabled = true;
            }
            else
            {
                cbo_Formulario.Enabled = false;
                cbo_Formulario.Items.Insert(0, new ListItem("Seleccione Formulario", "0"));                
            }

            lkBtn_viewPanel_ModalPopupExtender.Show();
        }

        #endregion

        #region Obtener datos del formulario

        protected string getNoFormulario()
        {
            return cbo_Formulario.SelectedValue.ToString();
        }

        protected string getTipoSolicitud()
        {
            return cbo_tipoSolicitud.SelectedValue.ToString();
        }

        protected string getEstadoAnterior()
        {
            return cbo_estadoAnterior.SelectedValue.ToString();
        }

        protected string getEstadoSiguiente()
        {
            return cbo_estadoSiguiente.SelectedValue.ToString();
        }

        //protected int getCodigoEstado()
        //{
        //    return Convert.ToInt32(txtCodigo.Text);
        //}

        protected string getNombreEstado()
        {
            return txtNombre.Text;
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