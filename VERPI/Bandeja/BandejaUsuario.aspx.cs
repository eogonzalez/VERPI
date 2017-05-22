using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Capa_Negocio.Bandeja;

namespace VERPI.Bandeja
{
    public partial class BandejaUsuario : System.Web.UI.Page
    {
        CNBandejaUsuario objCNBandeja = new CNBandejaUsuario();

        #region Eventos del Formulario

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAlertError.Visible = false;

                if (Session["UsuarioID"] != null)
                {
                    Llenar_gvBorradores(Convert.ToInt32(Session["UsuarioID"].ToString()));
                    Llenar_CantidadBorradores(Convert.ToInt32(Session["UsuarioID"].ToString()));

                }
            }
        }

        protected void gvBorradores_RowCommand(object sender, GridViewCommandEventArgs e)
        {

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
        #endregion
    }
}