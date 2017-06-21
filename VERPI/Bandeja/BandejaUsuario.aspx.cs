using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Capa_Negocio.Bandeja;
using Capa_Negocio.General;

namespace VERPI.Bandeja
{
    public partial class BandejaUsuario : System.Web.UI.Page
    {
        CNBandejaUsuario objCNBandeja = new CNBandejaUsuario();
        CNFormularios objCNFormulario = new CNFormularios();

        #region Eventos del Formulario

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAlertError.Visible = false;
                divAlertCorrecto.Visible = false;

                if (Session["UsuarioID"] != null)
                {
                    Llenar_gvBorradores((int)Session["UsuarioID"]);
                    Llenar_CantidadBorradores((int)Session["UsuarioID"]);

                }
            }
        }

        protected void gvBorradores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {            
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = gvBorradores.Rows[index];
                int no_PreIngreso = Convert.ToInt32(row.Cells[0].Text);
                int cmd = Convert.ToInt32(row.Cells[1].Text);
                int no_formulario = Convert.ToInt32(row.Cells[2].Text);

                Session.Add("NoPreIngreso", no_PreIngreso);
                Session.Add("cmd", cmd);
                Session.Add("no_formulario", no_formulario);

                switch (e.CommandName)
                {
                    case "modificar":
                        Response.Redirect("~/PreIngresos/Marcas/PreIngreso.aspx?cmd="+cmd.ToString()+"&nf="+no_formulario.ToString()+"&np="+no_PreIngreso.ToString());
                        break;
                    case "eliminar":
                        EliminarFormulario(no_PreIngreso);
                        Llenar_gvBorradores((int)Session["UsuarioID"]);
                        Llenar_CantidadBorradores((int)Session["UsuarioID"]);
                        break;
                }

            }

        }

        protected void gvBorradores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBorradores.PageIndex = e.NewPageIndex;
            Llenar_gvBorradores((int)Session["UsuarioID"]);
        }

        #endregion

        #region Funciones

        protected void Llenar_gvBorradores(int id_usuario)
        {
            var dt = new DataTable();
            dt = objCNBandeja.SelectFormulariosBorrador((int)Session["UsuarioID"]);

            gvBorradores.DataSource = dt;
            gvBorradores.DataBind();
        }

        protected void Llenar_CantidadBorradores(int id_usuario)
        {
            lblCantidadBandeja.Text = objCNBandeja.SelectCantidadBorradores(id_usuario).ToString();
        }

        protected void EliminarFormulario(int no_PreIngreso)
        {
            if (objCNFormulario.EliminaFormulario(no_PreIngreso))
            {
                MensajeCorrectoPrincipal.Text = "Se ha eliminado formulario correctamente.";
                divAlertCorrecto.Visible = true;
            }
            else
            {
                ErrorMessagePrincipal.Text = "Ha ocurrido un error al tratar de eliminar formulario.";
                divAlertError.Visible = true;
            }
        }

        #endregion

    }
}