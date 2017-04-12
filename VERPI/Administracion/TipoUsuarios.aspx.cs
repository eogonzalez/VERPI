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
    public partial class TipoUsuarios : System.Web.UI.Page
    {
        CNTipoUsuarios objCapaNegocio = new CNTipoUsuarios();
        CETipoUsuarios objetoEntidad = new CETipoUsuarios();

        #region Eventos del Formulario
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //btnGuardar.Enabled = false;
                //gvTipoUsuario.Columns[1].Visible = false ;
                LLenar_gvTipoUsuario();
                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
            }
        }

        protected void LLenar_gvTipoUsuario()
        {
            DataTable tbl = new DataTable();
            //objCapaNegocio = new CNTipoUsuarios();

            tbl = objCapaNegocio.SelectTipoUsuarios().Tables[0];

            gvTipoUsuario.DataSource = tbl;
            gvTipoUsuario.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /*
             * Declaro la variable que obtiene 
             * el tipo de perfil seleccionado de las variables de session
            */

            int id_tipousuario = 0;

            if (Session["IDTipoUsuario"] != null)
            {
                id_tipousuario = Convert.ToInt32(Session["IDTipoUsuario"].ToString());
            }


            switch (btnGuardar.CommandName)
            {
                case "Editar":

                    if (UpdateTipoUsuario(id_tipousuario))
                    {
                        LLenar_gvTipoUsuario();
                        LimpiarTipoUsuario();
                        btnGuardar.CommandName = "Guardar";
                        btnGuardar.Text = "Guardar";
                    }
                    else
                    {
                        ErrorMessage.Text = "Ha ocurrido un error al Editar Perfil.";
                        lkBtn_testModalPopupExtender.Show();
                    }

                    break;
                case "Guardar":

                    if (GuardarTipoUsuario())
                    {
                        //Mensaje Se Guardo con exito
                        LLenar_gvTipoUsuario();
                        LimpiarTipoUsuario();
                    }
                    else
                    {

                        ErrorMessage.Text = "Ha ocurrido un error al Guardar Perfil.";
                        lkBtn_testModalPopupExtender.Show();
                    }

                    break;
                default:
                    break;
            }

        }

        protected void gvTipoUsuario_RowCommand(Object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = gvTipoUsuario.Rows[index];
            int id_tipousuario = Convert.ToInt32(row.Cells[0].Text);

            /*
             * Agrego a la variable de sesion el perfil seleccionado
             * para las acciones de editar o eliminar
             */

            Session.Add("IDTipoUsuario", id_tipousuario);

            switch (e.CommandName)
            {
                case "permisos":
                    Response.Redirect("~/Administracion/PermisosPerfiles.aspx?id=" + id_tipousuario.ToString());
                    break;
                case "modificar":
                    Mostrardatos(id_tipousuario);
                    lkBtn_testModalPopupExtender.Show();
                    break;
                case "eliminar":
                    EliminaTipoUsuario(id_tipousuario);
                    LLenar_gvTipoUsuario();
                    break;
                default:
                    break;
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            LimpiarTipoUsuario();
            btnGuardar.CommandName = "Guardar";
            btnGuardar.Text = "Guardar";
        }

        #endregion

        #region Funciones
        protected Boolean GuardarTipoUsuario()
        {
            bool respuesta = false;
            objCapaNegocio = new CNTipoUsuarios();
            objetoEntidad = new CETipoUsuarios();

            objetoEntidad.Nombre = txtNombre.Text;
            objetoEntidad.Descripcion = txtDescripcion.Text;
            objetoEntidad.TipoPermiso = ddlTipoPermiso.SelectedValue;
            objetoEntidad.ID_UsuarioAutoriza = Convert.ToInt32(Session["UsuarioID"].ToString());

            respuesta = objCapaNegocio.InsertTipoUsuarios(objetoEntidad);

            return respuesta;
        }

        protected Boolean UpdateTipoUsuario(int id_tipousuario)
        {
            Boolean respuesta = false;
            objetoEntidad = new CETipoUsuarios();
            objCapaNegocio = new CNTipoUsuarios();

            objetoEntidad.Nombre = txtNombre.Text;
            objetoEntidad.Descripcion = txtDescripcion.Text;
            objetoEntidad.TipoPermiso = ddlTipoPermiso.SelectedValue;
            objetoEntidad.ID_TipoUsuario = id_tipousuario;
            objetoEntidad.ID_UsuarioAutoriza = Convert.ToInt32(Session["UsuarioID"].ToString());

            respuesta = objCapaNegocio.UpdateTipoUsuario(objetoEntidad);

            return respuesta;
        }

        protected Boolean EliminaTipoUsuario(int id_tipousuario)
        {
            var respuesta = false;
            objetoEntidad = new CETipoUsuarios();
            objetoEntidad.ID_TipoUsuario = id_tipousuario;
            objetoEntidad.ID_UsuarioAutoriza = Convert.ToInt32(Session["UsuarioID"].ToString());

            respuesta = objCapaNegocio.DeleteTipoUsuario(objetoEntidad);

            return respuesta;
        }

        protected void LimpiarTipoUsuario()
        {
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
        }

        protected void Mostrardatos(int id_tipousuario)
        {

            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            var tbl = new DataTable();
            tbl = objCapaNegocio.SelectTipoUsuario(id_tipousuario);
            var row = tbl.Rows[0];

            txtNombre.Text = row["nombre"].ToString();
            txtDescripcion.Text = row["descripcion"].ToString();
            ddlTipoPermiso.SelectedValue = row["tipo_permiso"].ToString();

            //lkBtn_nuevo_ModalPopupExtender.Show();
            this.lkBtn_testModalPopupExtender.Show();

        }

        #endregion
        





    }
}