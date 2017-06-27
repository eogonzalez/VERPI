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

        #region Eventos del formulario

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
            if (e.CommandName != "Page")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvExpedientes.Rows[index];
                int id_expediente = Convert.ToInt32(row.Cells[0].Text);
                int id_preingreso = Convert.ToInt32(row.Cells[2].Text);
                var cmd = row.Cells[1].Text;
                Session.Add("IDExpediente", id_expediente);

                switch (e.CommandName)
                {
                    case "Asignar":
                        //objCEExpedientes.ID_Expediente = id_expediente;
                        //objCEExpedientes.ID_Usuario_DACE = (int)Session["UsuarioID"];

                        ///*Obtener primer estado*/
                        //var dt = new DataTable();

                        //dt = objCNBandeja.SelectEstadoMinimo("VO");
                        //var row_ex = dt.Rows[0];

                        //objCEExpedientes.Sigla_Estado = row_ex["descripcion"].ToString();
                        //objCEExpedientes.Estado_Principal = (int)row_ex["codigo_estado"];
                        ////objCEExpedientes.Estado_Alterno 
                        //objCEExpedientes.Dias_Maximos = (int)row_ex["dias_max"];
                        //objCEExpedientes.Dias_Minimos = (int)row_ex["dias_min"];

                        //AutoAsignarExpedientes(objCEExpedientes);
                        //Llenar_gvBandeja();

                        break;

                    case "AsignarFuncionario":

                        break;
                    default:
                        break;
                }

            }
        }

        protected void gvExpedientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvExpedientes.PageIndex = e.NewPageIndex;
            LLenar_gvExpedientes();
        }

        #endregion

        #region Funciones

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

        #endregion

    }
}