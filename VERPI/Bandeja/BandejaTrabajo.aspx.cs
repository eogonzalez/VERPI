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
    public partial class BandejaTrabajo : System.Web.UI.Page
    {
        CNBandejaTrabajo objCNBandeja = new CNBandejaTrabajo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAlertError.Visible = false;
                divAlertCorrecto.Visible = false;

                LLenar_gvExpedientes();
                Llenar_CantidadFormularios();
                
            }
        }

        protected void gvExpedientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvExpedientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void LLenar_gvExpedientes()
        {
            var dt = new DataTable();
            dt = objCNBandeja.SelectFormularios();
            gvExpedientes.DataSource = dt;
            gvExpedientes.DataBind();
        }

        protected void Llenar_CantidadFormularios()
        {
            lblCantidadBandeja.Text = objCNBandeja.SelectCantidadFormularios().ToString();
        }
    }
}