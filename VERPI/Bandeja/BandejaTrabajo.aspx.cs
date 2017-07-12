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

                /*
                Valores por defecto de los filtros
                */
                Session.Add("FiltroEstado", 0);
                Session.Add("FiltroTipoTramite", 0);
                Session.Add("FechaInicial", string.Empty);
                Session.Add("FechaFinal", string.Empty);


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

        //protected void cbo_estado_Filtro_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int valor = Convert.ToInt32(cbo_estado_Filtro.SelectedValue.ToString());
        //    Session["FiltroEstado"] = valor;
        //    LLenar_gvExpedientes();
        //}

        protected void chk_Marcas_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Marcas.Checked)
            {
                Session["FiltroTipoTramite"] = 1;

                chk_Derechos.Enabled = false;
                chk_Patentes.Enabled = false;
            }
            else
            {
                Session["FiltroTipoTramite"] = 0;
                chk_Derechos.Enabled = true;
                chk_Patentes.Enabled = true;
            }

            LLenar_gvExpedientes();
        }

        protected void chk_Patentes_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Patentes.Checked)
            {
                Session["FiltroTipoTramite"] = 2;

                chk_Marcas.Enabled = false;
                chk_Derechos.Enabled = false;
            }
            else
            {
                Session["FiltroTipoTramite"] = 0;
                chk_Marcas.Enabled = true;
                chk_Derechos.Enabled = true;
            }

            LLenar_gvExpedientes();
        }

        protected void chk_Derechos_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Derechos.Checked)
            {
                Session["FiltroTipoTramite"] = 3;

                chk_Marcas.Enabled = false;
                chk_Patentes.Enabled = false;
            }
            else
            {
                Session["FiltroTipoTramite"] = 0;
                chk_Marcas.Enabled = true;
                chk_Patentes.Enabled = true;
            }

            LLenar_gvExpedientes();
        }

        protected void txtFechaInicial_TextChanged(object sender, EventArgs e)
        {
            string fecha_inicial = txtFechaInicial.Text;
            Session["FechaInicial"] = fecha_inicial;
            LLenar_gvExpedientes();
        }

        protected void txtFechaFinal_TextChanged(object sender, EventArgs e)
        {
            string fecha_final = txtFechaFinal.Text;
            Session["FechaFinal"] = fecha_final;
            LLenar_gvExpedientes();
        }

        #endregion

        #region Funciones

        protected void LLenar_gvExpedientes()
        {
            var dt = new DataTable();
            dt = objCNBandeja.SelectFormularios((int)Session["FiltroTipoTramite"],
                Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString());

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