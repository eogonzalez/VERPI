using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Capa_Negocio.General;
using Capa_Entidad.General;

namespace VERPI.Administracion
{
    public partial class Usuarios : System.Web.UI.Page
    {
        CNUsuario objCNUsuario = new CNUsuario();
        CEUsuario objCEUsuario = new CEUsuario();

        #region Eventos del formulario
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Llenar_gvUsuarios();
                Llenar_ddlTipoPermiso();
                cb_generarContrasenia.Visible = false;

                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
            }
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = gvUsuarios.Rows[index];
            int id_usuarioManager = Convert.ToInt32(row.Cells[0].Text);
            string correoUsuarioManager = row.Cells[4].Text;
            Session.Add("IDUsuarioManager", id_usuarioManager);
            Session.Add("CorreoUsuarioManager", correoUsuarioManager);

            switch (e.CommandName)
            {
                case "modificar":
                    MostrarUsuario(id_usuarioManager);
                    lkBtn_viewPanel_ModalPopupExtender.Show();
                    break;
                case "eliminar":
                    EliminarUsuario(id_usuarioManager);
                    Llenar_gvUsuarios();
                    break;
                case "generarpassword":
                    lkBtn_ModificarContraseña_ModalPopupExtender.Show();
                    break;
                default:
                    break;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            switch (btnGuardar.CommandName)
            {
                case "Guardar":
                    if (IsValid)
                    {
                        if (Password.Text == ConfirmPassword.Text)
                        {
                            if (GuardarUsuario())
                            {
                                Llenar_gvUsuarios();
                                LimpiarPanel();
                            }
                            else
                            {
                                lkBtn_viewPanel_ModalPopupExtender.Show();
                                ErrorMessagePanel.Text = "Ha ocurrido un error al guardar usuario.";
                            }
                        }
                        else
                        {
                            ErrorMessagePanel.Text = "Contraseña no coincide, verifique.";
                        }

                    }

                    break;
                case "Editar":
                    if (ActualizarUsuario((int)Session["IDUsuarioManager"]))
                    {
                        Llenar_gvUsuarios();
                        LimpiarPanel();
                        btnGuardar.Text = "Guardar";
                        btnGuardar.CommandName = "Guardar";

                        divPassword.Visible = true;
                        divConfirmPassword.Visible = true;
                        Email.Enabled = true;

                    }
                    else
                    {
                        ErrorMessagePanel.Text = "Ha ocurrido un error al actualizar datos.";
                    }
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

            divPassword.Visible = true;
            divConfirmPassword.Visible = true;
        }

        protected void cb_generarContrasenia_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_generarContrasenia.Checked)
            {
                Password.Enabled = true;
                ConfirmPassword.Enabled = true;
            }
            else
            {
                Password.Enabled = false;
                ConfirmPassword.Enabled = false;
            }
        }

        protected void btnModificarContraseña_Click(object sender, EventArgs e)
        {
            if (txtContraseña.Text == txtConfirmarContraseña.Text)
            {
                int id_usuarioManager = (int)Session["IDUsuarioManager"];
                string correoUsuarioManager = Session["CorreoUsuarioManager"].ToString();

                if (objCNUsuario.UpdateContraseña(id_usuarioManager, correoUsuarioManager, txtContraseña.Text))
                {
                    ErrorMessage.Text = "Se actualizo contraseña correctamente";
                }
                else
                {
                    ErrorPanelContraseña.Text = "Ha ocurrido un error al actualizar contraseña.";
                }
            }
            else
            {
                ErrorPanelContraseña.Text = "Contraseña no coincide, verifique.";
            }
        }

        #endregion

        #region Funciones

        protected void Llenar_gvUsuarios()
        {
            DataTable tbl = new DataTable();

            tbl = objCNUsuario.SelectUsuarios();

            gvUsuarios.DataSource = tbl;
            gvUsuarios.DataBind();

        }

        protected void Llenar_ddlTipoPermiso()
        {
            var dt = new DataTable();

            dt = objCNUsuario.SelectComboPerfiles();

            if (dt.Rows.Count > 0)
            {
                ddlTipoPermiso.DataTextField = dt.Columns["nombre"].ToString();
                ddlTipoPermiso.DataValueField = dt.Columns["id_tipousuario"].ToString();
                ddlTipoPermiso.DataSource = dt;
                ddlTipoPermiso.DataBind();
            }
        }

        protected bool GuardarUsuario()
        {
            var respuesta = false;
            objCEUsuario.CE_Nombres = getNombreUsuario();
            objCEUsuario.CE_Apellidos = getApellido();
            objCEUsuario.CE_CUI = getCUI();
            objCEUsuario.CE_Telefono = getNumero();
            objCEUsuario.CE_Direccion = getDirecion();
            objCEUsuario.CE_Correo = getCorreo();
            objCEUsuario.CE_Password = getPassword();
            objCEUsuario.ID_TipoUsuario = getId_TipoUsuario();
            objCEUsuario.ID_UsuarioAutoriza = (int)Session["UsuarioID"];
            objCEUsuario.CE_Estado = "A";

            respuesta = objCNUsuario.GuardarUsuario(objCEUsuario);

            return respuesta;
        }

        protected void LimpiarPanel()
        {
            NombreUsuario.Text = string.Empty;
            ApellidoUsuario.Text = string.Empty;
            CuiUsuario.Text = string.Empty;
            txtNumero.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            Email.Text = string.Empty;
            Password.Text = string.Empty;
            ConfirmPassword.Text = string.Empty;
        }

        protected void MostrarUsuario(int id_usuarioManager)
        {
            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            divPassword.Visible = false;
            divConfirmPassword.Visible = false;
            Email.Enabled = false;
            
            var tbl = new DataTable();
            tbl = objCNUsuario.SelectUsuario(id_usuarioManager);
            var row = tbl.Rows[0];

            NombreUsuario.Text = row["nombres"].ToString();
            ApellidoUsuario.Text = row["apellidos"].ToString();
            CuiUsuario.Text = row["cui"].ToString();
            txtNumero.Text = row["telefono"].ToString();
            txtDireccion.Text = row["direccion"].ToString();
            Email.Text = row["correo"].ToString();
            ddlTipoPermiso.SelectedValue = row["id_tipousuario"].ToString();
        }

        protected bool EliminarUsuario(int id_usuarioManager)
        {
            var respuesta = false;

            objCEUsuario.ID_Usuario = id_usuarioManager;
            objCEUsuario.ID_UsuarioAutoriza = Convert.ToInt32(Session["UsuarioID"].ToString());

            respuesta = objCNUsuario.DeleteUsuario(objCEUsuario);

            return respuesta;
        }

        protected bool ActualizarUsuario(int id_usuarioManager)
        {
            var respuesta = false;

            objCEUsuario.ID_Usuario = id_usuarioManager;
            objCEUsuario.CE_Nombres = getNombreUsuario();
            objCEUsuario.CE_Apellidos = getApellido();
            objCEUsuario.CE_CUI = getCUI();
            objCEUsuario.CE_Telefono = getNumero();
            objCEUsuario.CE_Direccion = getDirecion();            
            objCEUsuario.ID_TipoUsuario = getId_TipoUsuario();
            objCEUsuario.ID_UsuarioAutoriza = (int)Session["UsuarioID"];

            respuesta = objCNUsuario.UpdateUsuario(objCEUsuario);

            return respuesta;
        }

        protected void ModificarContraseña()
        {
            if (txtContraseña.Text == txtConfirmarContraseña.Text)
            {
                int id_usuarioManager = (int)Session["IDUsuarioManager"];
                string correoUsuarioManager = Session["CorreoUsuarioManager"].ToString();

                if (objCNUsuario.UpdateContraseña(id_usuarioManager, correoUsuarioManager, txtContraseña.Text))
                {
                    ErrorMessage.Text = "Se actualizo contraseña correctamente";
                }
                else
                {
                    ErrorPanelContraseña.Text = "Ha ocurrido un error al actualizar contraseña.";
                }
            }
            else
            {
                ErrorPanelContraseña.Text = "Contraseña no coincide, verifique.";
            }
        }

        #endregion

        #region Obtener Valores

        protected string getNombreUsuario()
        {
            return NombreUsuario.Text;
        }

        protected string getApellido()
        {
            return ApellidoUsuario.Text;
        }

        protected string getCUI()
        {
            return CuiUsuario.Text;
        }

        protected string getNumero()
        {
            return txtNumero.Text;
        }

        protected string getDirecion()
        {
            return txtDireccion.Text;
        }

        protected string getCorreo()
        {
            return Email.Text;
        }

        protected string getPassword()
        {
            return Password.Text;
        }

        protected string getPasswordConfirmacion()
        {
            return ConfirmPassword.Text;
        }

        protected int getId_TipoUsuario()
        {
            return Convert.ToInt32(ddlTipoPermiso.SelectedValue);
        }


        #endregion

    }
}