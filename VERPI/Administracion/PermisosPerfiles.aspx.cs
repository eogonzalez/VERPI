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
    public partial class PermisosPerfiles : System.Web.UI.Page
    {
        CNPermisosPerfiles objCNPermisosPerfiles = new CNPermisosPerfiles();
        CEPermisosPerfiles objCEPermisosPerfiles = new CEPermisosPerfiles();

        #region Eventos del formulario

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id_usuarioPermiso = 0;

                if (Request.QueryString["id"] != null)
                {
                    id_usuarioPermiso = Convert.ToInt32(Request.QueryString["id"].ToString());
                    cboPerfil.SelectedValue = id_usuarioPermiso.ToString();
                    cboPerfil.Enabled = false;
                    Llenar_gvPermisosPerfiles(id_usuarioPermiso);
                }
                else
                {
                    Llenar_gvPermisosPerfiles();
                }

                Llenar_combos();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int id_permisoPerfil = 0;
            if (Session["IDPermisoPerfil"] != null)
            {
                id_permisoPerfil = (Int32)Session["IDPermisoPerfil"];
            }

            switch (btnGuardar.CommandName)
            {
                case "Guardar":
                    if (GuardarPermisoPerfil())
                    {
                        LimpiarPanel();
                        int id_usuarioPermiso = 0;

                        if (Request.QueryString["id"] != null)
                        {
                            id_usuarioPermiso = Convert.ToInt32(Request.QueryString["id"].ToString());
                            Llenar_gvPermisosPerfiles(id_usuarioPermiso);
                        }
                        else
                        {
                            
                            Llenar_gvPermisosPerfiles();
                        }

                    }
                    else
                    {
                        ErrorMessage.Text = "Ha Ocurrido un error al guardar permiso.";
                    }
                    break;
                case "Editar":
                    if (ActualizarPermisoPerfil(id_permisoPerfil))
                    {
                        int id_usuarioPermiso = 0;
                        LimpiarPanel();
                        btnGuardar.Text = "Guardar";
                        btnGuardar.CommandName = "Guardar";
                        
                        if (Request.QueryString["id"] != null)
                        {
                            id_usuarioPermiso = Convert.ToInt32(Request.QueryString["id"].ToString());
                            Llenar_gvPermisosPerfiles(id_usuarioPermiso);
                        }
                        else
                        {
                            cboMenu.Enabled = true;
                            
                            Llenar_gvPermisosPerfiles();
                        }                                                
                    }
                    else
                    {
                        ErrorMessage.Text = "Ha Ocurrido un error al actualizar permiso.";
                    }
                    break;
                default:
                    break;
            }


        }

        protected void gvPermisosPerfiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = gvPermisosPerfiles.Rows[index];
            int id_permisoPerfil = Convert.ToInt32(row.Cells[0].Text);

            Session.Add("IDPermisoPerfil", id_permisoPerfil);

            int id_usuarioPermiso = 0;

            if (Request.QueryString["id"] != null)
            {
                id_usuarioPermiso = Convert.ToInt32(Request.QueryString["id"].ToString());
            }

            switch (e.CommandName)
            {   
                case "modificar":
                    cboMenu.Enabled = false;
                    cboPerfil.Enabled = false;
                    MostrarDatos(id_permisoPerfil);
                    lkBtn_viewPanel_ModalPopupExtender.Show();
                    break;
                case "eliminar":
                    EliminarPermiso(id_permisoPerfil);
                    Llenar_gvPermisosPerfiles(id_usuarioPermiso);
                    break;
                default:
                    break;
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            cboMenu.Enabled = true;
            LimpiarPanel();
            btnGuardar.Text = "Guardar";
            btnGuardar.CommandName = "Guardar";
        }

        #endregion

        #region Funciones

        protected void Llenar_gvPermisosPerfiles(int id_usuarioPermiso = 0)
        {
            var tbl = new DataSet();

            tbl = objCNPermisosPerfiles.SelectPermisosPerfiles(id_usuarioPermiso);

            gvPermisosPerfiles.DataSource = tbl;
            gvPermisosPerfiles.DataBind();
        }

        protected void Llenar_combos()
        {
            var dt = objCNPermisosPerfiles.SelectCombosPermisosPerfiles();
            DataTable dtPerfil = dt.Tables[0];
            DataTable dtMenu = dt.Tables[1];

            if (dtPerfil.Rows.Count > 0)
            {
                cboPerfil.DataTextField = dtPerfil.Columns["nombre"].ToString();
                cboPerfil.DataValueField = dtPerfil.Columns["id_tipousuario"].ToString();
                cboPerfil.DataSource = dtPerfil;
                cboPerfil.DataBind();
            }

            if (dtMenu.Rows.Count > 0)
            {
                cboMenu.DataTextField = dtMenu.Columns["nombre"].ToString();
                cboMenu.DataValueField = dtMenu.Columns["id_opcion"].ToString();
                cboMenu.DataSource = dtMenu;
                cboMenu.DataBind();
            }
        }

        protected Boolean GuardarPermisoPerfil()
        {
            objCEPermisosPerfiles.ID_TipoUsuario = getIdTipoUsuario();
            objCEPermisosPerfiles.ID_Opcion = getIdOpcion();
            objCEPermisosPerfiles.ID_UsuarioAutoriza = getUsuarioAutoriza();
            objCEPermisosPerfiles.Acceder = getAcceder();
            objCEPermisosPerfiles.Insertar = getInsertar();
            objCEPermisosPerfiles.Editar = getEditar();
            objCEPermisosPerfiles.Borrar = getBorrar();
            objCEPermisosPerfiles.Aprobar = getAprobar();
            objCEPermisosPerfiles.Rechazar = getRechazar();

            return objCNPermisosPerfiles.InsertPermisosPerfiles(objCEPermisosPerfiles);
        }

        protected void MostrarDatos(int id_permisoPerfil)
        {
            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            var tbl = new DataTable();
            tbl = objCNPermisosPerfiles.SelectPermisoPerfil(id_permisoPerfil);
            var row = tbl.Rows[0];

            cboPerfil.SelectedValue = row["id_tipousuario"].ToString();
            cboMenu.SelectedValue = row["id_opcion"].ToString();

            cb_insertar.Checked = (Boolean)row["insertar"];
            cb_acceder.Checked = (Boolean)row["acceder"];
            cb_editar.Checked = (Boolean)row["editar"];
            cb_borrar.Checked = (Boolean)row["borrar"];
            cb_aprobar.Checked = (Boolean)row["aprobar"];
            cb_rechazar.Checked = (Boolean)row["rechazar"];
        }

        protected void EliminarPermiso(int id_permisoPerfil)
        {
            objCEPermisosPerfiles.ID_PermisoPerfil = id_permisoPerfil;
            objCEPermisosPerfiles.ID_UsuarioAutoriza = getUsuarioAutoriza();

            objCNPermisosPerfiles.DeletePermisoPerfil(objCEPermisosPerfiles);
        }

        protected Boolean ActualizarPermisoPerfil(int id_permisoPerfil)
        {
            objCEPermisosPerfiles.Insertar = getInsertar();
            objCEPermisosPerfiles.Acceder = getAcceder();
            objCEPermisosPerfiles.Editar = getEditar();
            objCEPermisosPerfiles.Borrar = getBorrar();
            objCEPermisosPerfiles.Aprobar = getAprobar();
            objCEPermisosPerfiles.Rechazar = getRechazar();
            objCEPermisosPerfiles.ID_PermisoPerfil = id_permisoPerfil;

            return objCNPermisosPerfiles.UpdatePermisoPerfil(objCEPermisosPerfiles);
        }

        protected void LimpiarPanel()
        {
            cb_acceder.Checked = false;
            cb_aprobar.Checked = false;
            cb_borrar.Checked = false;
            cb_editar.Checked = false;
            cb_insertar.Checked = false;
            cb_rechazar.Checked = false;
        }

        #endregion

        #region Funciones para obtener valores del formulario
        protected int getUsuarioAutoriza()
        {
            return Convert.ToInt32(Session["UsuarioID"].ToString());
        }

        protected int getIdTipoUsuario()
        {
            return Convert.ToInt32(cboPerfil.SelectedValue.ToString());
        }

        protected int getIdOpcion()
        {
            return Convert.ToInt32(cboMenu.SelectedValue.ToString());
        }

        protected Boolean getAcceder()
        {
            return cb_acceder.Checked;
        }

        protected Boolean getInsertar()
        {
            return cb_insertar.Checked;
        }

        protected Boolean getEditar()
        {
            return cb_editar.Checked;
        }

        protected Boolean getBorrar()
        {
            return cb_borrar.Checked;
        }

        protected Boolean getAprobar()
        {
            return cb_aprobar.Checked;
        }

        protected Boolean getRechazar()
        {
            return cb_rechazar.Checked;
        }
        #endregion
               
    }
}