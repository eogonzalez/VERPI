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
    public partial class Menu : System.Web.UI.Page
    {
        CNMenu objCNMenu = new CNMenu();
        CEMenu objCEMenu = new CEMenu();

        #region Eventos del formulario
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Llenar_gvMenu();

                //Valores por defecto si es nuevo
                txtOrden.Text = "0";
                cb_obligatorio.Checked = false;
                cb_obligatorio.Visible = true;
                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int id_menuOpcion = 0;

            if (Session["IDMenuOpcion"] != null)
            {
                id_menuOpcion = (Int32)Session["IDMenuOpcion"];
            }

            switch (btnGuardar.CommandName)
            {
                case "Editar":
                    if (ActualizarMenuOpcion(id_menuOpcion))
                    {
                        Llenar_gvMenu();
                        LimpiarPanel();
                        btnGuardar.Text = "Guardar";
                        btnGuardar.CommandName = "Guardar";
                    }
                    else
                    {
                        this.lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha Ocurrido un Error al actualizar Opcion";
                    }
                    break;
                case "Guardar":
                    if (GuardarMenu())
                    {
                        Llenar_gvMenu();
                        LimpiarPanel();
                    }
                    else
                    {
                        this.lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha Ocurrido un Error al guardar Opcion";
                    }
                    break;
                default:
                    break;
            }


        }

        protected void gvMenu_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = gvMenu.Rows[index];
            int id_menu = Convert.ToInt32(row.Cells[0].Text);

            /*
             * Agrego a la variable de sesion el id de menu seleccionado
             * para las acciones de editar o eliminar
             */

            Session.Add("IDMenuOpcion", id_menu);

            switch (e.CommandName)
            {
                case "submenu":
                    Response.Redirect("~/Administracion/MenuOpcion.aspx?id_om=" + id_menu.ToString());
                    break;

                case "modificar":
                    MostrarDatos(id_menu);
                    this.lkBtn_viewPanel_ModalPopupExtender.Show();
                    break;

                case "eliminar":
                    EliminaMenuOpcion(id_menu);
                    Llenar_gvMenu();
                    break;

                default:
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
        protected void Llenar_gvMenu()
        {
            var tbl = new DataTable();

            tbl = objCNMenu.SelectMenu();

            gvMenu.DataSource = tbl;
            gvMenu.DataBind();
        }

        protected Boolean GuardarMenu()
        {
            objCEMenu.Nombre = txtNombreOpcion.Text;
            objCEMenu.Descripcion = getDescripcion();
            objCEMenu.URL = getURL();
            objCEMenu.Comando = getComando();
            objCEMenu.Orden = getOrden();
            objCEMenu.Obligatorio = getObligatorio();
            objCEMenu.Visible = getVisible();
            objCEMenu.Login = getConLogin();
            objCEMenu.Id_Padre = 0;
            objCEMenu.ID_UsuarioAutoriza = Convert.ToInt32(Session["UsuarioID"].ToString());

            return objCNMenu.SaveMenu(objCEMenu);
        }

        protected Boolean ActualizarMenuOpcion(int id_opcion)
        {
            var respuesta = false;

            objCEMenu.ID_MenuOpcion = id_opcion;
            objCEMenu.Nombre = txtNombreOpcion.Text;
            objCEMenu.Descripcion = getDescripcion();
            objCEMenu.URL = getURL();
            objCEMenu.Comando = getComando();
            objCEMenu.Orden = getOrden();
            objCEMenu.Obligatorio = getObligatorio();
            objCEMenu.Visible = getVisible();
            objCEMenu.Login = getConLogin();
            objCEMenu.Id_Padre = 0;
            objCEMenu.ID_UsuarioAutoriza = Convert.ToInt32(Session["UsuarioID"].ToString());

            respuesta = objCNMenu.UpdateMenuOpcion(objCEMenu);

            return respuesta;
        }

        protected Boolean EliminaMenuOpcion(int id_opcion)
        {
            var respuesta = false;

            objCEMenu = new CEMenu();
            objCEMenu.ID_MenuOpcion = id_opcion;
            objCEMenu.ID_UsuarioAutoriza = Convert.ToInt32(Session["UsuarioID"].ToString());

            respuesta = objCNMenu.DeleteMenuOpcion(objCEMenu);
            return respuesta;
        }

        protected void MostrarDatos(int id_menu)
        {
            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            var tbl = new DataTable();
            tbl = objCNMenu.SelectOpcionMenu(id_menu);
            var row = tbl.Rows[0];

            txtNombreOpcion.Text = row["nombre"].ToString();
            txtDescripcionOpcion.Text = row["descripcion"].ToString();
            txtURL.Text = row["url"].ToString();
            txtComando.Text = row["comando"].ToString();
            txtOrden.Text = row["orden"].ToString();
            cb_visible.Checked = (Boolean)row["visible"];
            cb_obligatorio.Checked = (Boolean)row["obligatorio"];
            cb_login.Checked = (Boolean)row["login"];
        }

        protected void LimpiarPanel()
        {
            txtNombreOpcion.Text = "";
            txtDescripcionOpcion.Text = "";
            txtURL.Text = "";
            txtComando.Text = "";
            txtOrden.Text = "";
            cb_visible.Checked = false;
            cb_obligatorio.Checked = false;
            cb_login.Checked = false;
        }
        #endregion

        #region Funiones para obtener valores del formulario
        protected string getNombre()
        {
            return txtNombreOpcion.Text;
        }

        protected string getDescripcion()
        {
            return txtDescripcionOpcion.Text;
        }

        protected string getURL()
        {
            return txtURL.Text;
        }

        protected string getComando()
        {
            return txtComando.Text;
        }

        protected int getOrden()
        {
            return Convert.ToInt32(txtOrden.Text);
        }

        protected Boolean getObligatorio()
        {
            return cb_obligatorio.Checked;
        }

        protected Boolean getVisible()
        {
            return cb_visible.Checked;
        }

        protected Boolean getConLogin()
        {
            return cb_login.Checked;
        }
        #endregion
        
    }
}