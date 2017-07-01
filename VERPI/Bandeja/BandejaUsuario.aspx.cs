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
                
                /*
                Valores por defecto de los filtros
                */
                Session.Add("FiltroEstado", 0);
                Session.Add("FiltroTipoTramite", 0);
                Session.Add("FechaInicial", string.Empty);
                Session.Add("FechaFinal", string.Empty);

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
                string estado = row.Cells[5].Text;

                Session.Add("NoPreIngreso", no_PreIngreso);
                Session.Add("cmd", cmd);
                Session.Add("no_formulario", no_formulario);
                //Session.Add("Estado", estado);

                switch (e.CommandName)
                {
                    case "modificar":
                        Response.Redirect("~/PreIngresos/Marcas/PreIngreso.aspx?cmd="+cmd.ToString()+"&nf="+no_formulario.ToString()+"&np="+no_PreIngreso.ToString());
                        break;
                    case "eliminar":
                        if (estado == "Enviado")
                        {
                            divAlertError.Visible = true;
                            ErrorMessagePrincipal.Text = "No es posible eliminar el formulario, ya ha sido enviado.";
                        }
                        else if (estado == "Borrador")
                        {
                            EliminarFormulario(no_PreIngreso);
                            Llenar_gvBorradores((int)Session["UsuarioID"]);
                            Llenar_CantidadBorradores((int)Session["UsuarioID"]);
                        }

                        break;
                }

            }

        }

        protected void gvBorradores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBorradores.PageIndex = e.NewPageIndex;
            Llenar_gvBorradores((int)Session["UsuarioID"]);
        }

        protected void cbo_estado_Filtro_SelectedIndexChanged(object sender, EventArgs e)
        {
            int valor = Convert.ToInt32(cbo_estado_Filtro.SelectedValue.ToString());
            Session["FiltroEstado"] = valor;
            Llenar_gvBorradores((int)Session["UsuarioID"]);
        }

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

            Llenar_gvBorradores((int)Session["UsuarioID"]);
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

            Llenar_gvBorradores((int)Session["UsuarioID"]);
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

            Llenar_gvBorradores((int)Session["UsuarioID"]);
        }

        protected void txtFechaInicial_TextChanged(object sender, EventArgs e)
        {
            string fecha_inicial = txtFechaInicial.Text;
            Session["FechaInicial"] = fecha_inicial;
            Llenar_gvBorradores((int)Session["UsuarioID"]);

        }

        protected void txtFechaFinal_TextChanged(object sender, EventArgs e)
        {
            string fecha_final = txtFechaFinal.Text;
            Session["FechaFinal"] = fecha_final;
            Llenar_gvBorradores((int)Session["UsuarioID"]);
        }

        #endregion

        #region Funciones

        protected void Llenar_gvBorradores(int id_usuario)
        {
            var dt = new DataTable();
            dt = objCNBandeja.SelectFormulariosBorrador((int)Session["UsuarioID"], 
                (int)Session["FiltroEstado"], (int)Session["FiltroTipoTramite"], 
                Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString());

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