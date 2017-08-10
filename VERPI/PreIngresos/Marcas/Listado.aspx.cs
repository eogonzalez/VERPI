using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Capa_Negocio.PreIngresos;
using Capa_Entidad.PreIngresos;

namespace VERPI.PreIngresos.Marcas
{
    public partial class Listado : System.Web.UI.Page
    {
        CNListado objCNListadoG = new CNListado();
        CEListado objCEListadoG = new CEListado();
                
        #region Eventos del formulario
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tipo_lista = 0;
                int no_preingreso = 0;
                if (Request.QueryString["tl"] != null)
                {
                    tipo_lista = Convert.ToInt32(Request.QueryString["tl"]);
                    Session.Add("TipoLista", tipo_lista);

                    if (Request.QueryString["np"] != null)
                    {
                        no_preingreso = Convert.ToInt32(Request.QueryString["np"]);
                        Session.Add("noPreIngreso", no_preingreso);

                        Llenar_gvListaGenerica(tipo_lista, no_preingreso);
                    }
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int tipo_listado = 0;
            if (Session["TipoLista"] != null)
            {
                tipo_listado = (int)Session["TipoLista"];
            }

            int no_preingreso = 0;
            if (Session["noPreIngreso"] != null)
            {
                no_preingreso = (int)Session["noPreIngreso"];
            }

            int correlativo_elemento = 0;
            if (Session["CorrelativoLista"] != null)
            {
                correlativo_elemento = (int)Session["CorrelativoLista"];
            }

            switch (btnGuardar.CommandName)
            {
                case "Guardar":
                    if (GuardarElementoLista())
                    {
                        Llenar_gvListaGenerica(tipo_listado, no_preingreso);
                        LimpiarPanel();
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al guardar elimento de la lista.";
                    }
                    break;

                case "Editar":
                    if (ActualizarElementoLista(correlativo_elemento))
                    {
                        Llenar_gvListaGenerica(tipo_listado, no_preingreso);
                        LimpiarPanel();
                        btnGuardar.Text = "Guardar";
                        btnGuardar.CommandName = "Guardar";
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al actualizar elemento de la lista";
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

        protected void gvListaGenerica_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = gvListaGenerica.Rows[index];
                int correlativo_elemento = Convert.ToInt32(row.Cells[0].Text);

                Session.Add("CorrelativoLista", correlativo_elemento);

                switch (e.CommandName)
                {
                    case "modificar":
                        MostrarDatos(correlativo_elemento);
                        this.lkBtn_viewPanel_ModalPopupExtender.Show();
                        break;
                    case "eliminar":
                        EliminarElementoLista(correlativo_elemento);
                        Llenar_gvListaGenerica((int)Session["TipoLista"], (int)Session["noPreIngreso"]);
                        break;
                }
            }
        }

        protected void gvListaGenerica_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListaGenerica.PageIndex = e.NewPageIndex;
            Llenar_gvListaGenerica((int)Session["TipoLista"], (int)Session["noPreIngreso"]);
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PreIngresos/Marcas/PreIngreso.aspx?cmd=" + Session["cmd"].ToString() + "&nf=" + Session["no_formulario"].ToString() + "&np=" + Session["noPreIngreso"].ToString());
        }

        #endregion

        #region Funciones

        protected void Llenar_gvListaGenerica(int tipo_lista, int no_preingreso)
        {
            var tbl = new DataTable();
            tbl = objCNListadoG.SelectListadoGridView(tipo_lista, no_preingreso);
            gvListaGenerica.DataSource = tbl;
            gvListaGenerica.DataBind();
        }

        protected bool GuardarElementoLista()
        {
            var respuesta = false;

            objCEListadoG.No_PreIngreso = (int)Session["noPreIngreso"];
            objCEListadoG.Tipo_Lista = (int)Session["TipoLista"];
            objCEListadoG.Nombre = getNombre();
            objCEListadoG.Direccion = getDireccion();
            objCEListadoG.Email = getCorreo();
            objCEListadoG.Telefono = getTelefono();

            respuesta = objCNListadoG.InsertElementoLista(objCEListadoG);

            return respuesta;
        }

        protected   bool ActualizarElementoLista(int correlativo_lista)
        {
            var respuesta = false;

            objCEListadoG.Correlativo_Lista = correlativo_lista;            
            objCEListadoG.Nombre = getNombre();
            objCEListadoG.Direccion = getDireccion();
            objCEListadoG.Email = getCorreo();
            objCEListadoG.Telefono = getTelefono();

            respuesta = objCNListadoG.UpdateMantenimientoRegistro(objCEListadoG);

            return respuesta;
        }

        protected void LimpiarPanel()
        {
            txtNombre.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtTelefono.Text = string.Empty;
        }

        protected void MostrarDatos(int correlativo_elemento)
        {
            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            objCEListadoG.Correlativo_Lista = correlativo_elemento;

            var tbl = new DataTable();
            tbl = objCNListadoG.SelectElementoLista(objCEListadoG);
            var row = tbl.Rows[0];

            txtNombre.Text = row["nombre"].ToString();
            txtDireccion.Text = row["direccion"].ToString();
            txtCorreo.Text = row["email"].ToString();
            txtTelefono.Text = row["telefono"].ToString();
        }

        protected void EliminarElementoLista(int correlativo_elemento)
        {
            objCEListadoG.Correlativo_Lista = correlativo_elemento;
            objCNListadoG.DeleteElementoLista(objCEListadoG);
        }

        #endregion

        #region Obtener valores del formulario

        protected string getNombre()
        {
            return txtNombre.Text;
        }

        protected string getDireccion()
        {
            return txtDireccion.Text;
        }

        protected string getCorreo()
        {
            return txtCorreo.Text;
        }

        protected string getTelefono()
        {
            return txtTelefono.Text;
        }



        #endregion

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PreIngresos/Marcas/PreIngreso.aspx?cmd=" + Session["cmd"].ToString() + "&nf=" + Session["no_formulario"].ToString() + "&np=" + Session["noPreIngreso"].ToString());
        }
    }
}