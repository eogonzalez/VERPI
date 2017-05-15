using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Capa_Entidad.General;
using Capa_Negocio.General;

namespace VERPI.Administracion
{
    public partial class TipoPagos : System.Web.UI.Page
    {
        CEMantenimientosDinamicos objCEMant = new CEMantenimientosDinamicos();
        CNMantenimientosDinamicos objCNMant = new CNMantenimientosDinamicos();
       
        #region Eventos del formulario
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EstablecerValoresGenerales();

                Llenar_gvTipoPagos();
                btnGuardar.Attributes.Add("onclick", "this.value='Procesando Espere...';this.disabled=true;" + ClientScript.GetPostBackEventReference(btnGuardar, ""));
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int id_tipoPago = 0;
            if (Session["IDTipoPago"] != null)
            {
                id_tipoPago = (int)Session["IDTipoPago"];
            }

            switch (btnGuardar.CommandName)
            {
                case "Guardar":
                    if (GuardarTipoPago())
                    {
                        Llenar_gvTipoPagos();
                        LimpiarPanel();
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al guardar Tipo de Pago.";
                    }
                    break;
                case "Editar":
                    if (ActualizarTipoPago(id_tipoPago))
                    {
                        Llenar_gvTipoPagos();
                        LimpiarPanel();
                        btnGuardar.Text = "Guardar";
                        btnGuardar.CommandName = "Guardar";
                    }
                    else
                    {
                        lkBtn_viewPanel_ModalPopupExtender.Show();
                        ErrorMessage.Text = "Ha ocurrido un error al actualizar Tipo de Pago.";
                    }
                    break;
                
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            LimpiarPanel();
            btnGuardar.Text = "Guardar";
            btnGuardar.CommandName = "Guardar";
        }

        protected void gvTipoPagos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = gvTipoPagos.Rows[index];
            int id_tipopago = Convert.ToInt32(row.Cells[0].Text);

            Session.Add("IDTipoPago", id_tipopago);

            switch (e.CommandName)
            {
                case "modificar":
                    MostrarDatos(id_tipopago);
                    this.lkBtn_viewPanel_ModalPopupExtender.Show();
                    break;

                case "eliminar":
                    EliminarTipoPago(id_tipopago);
                    Llenar_gvTipoPagos();
                    break;
                
            }
        }

        #endregion

        #region Funciones

        protected void Llenar_gvTipoPagos()
        {
            var tbl = new DataTable();
            tbl = objCNMant.SelectMantenimientoGridView(objCEMant);
            gvTipoPagos.DataSource = tbl;
            gvTipoPagos.DataBind();
        }

        protected bool GuardarTipoPago()
        {
            var respuesta = false;
            EstablecerValoresGenerales();

            objCEMant.Nombre_Mant_Valor = getNombre();
            objCEMant.Descripcion_Mant_Valor = getDescripcion();

            respuesta = objCNMant.InsertMantenimiento(objCEMant);

            return respuesta;
        }

        protected bool ActualizarTipoPago(int id_tipoPago)
        {
            var respuesta = false;
            EstablecerValoresGenerales();
            objCEMant.Nombre_Mant_Valor = getNombre();
            objCEMant.Descripcion_Mant_Valor = getDescripcion();
            objCEMant.ID_Mant_Valor = id_tipoPago;

            respuesta = objCNMant.UpdateMantenimientoRegistro(objCEMant);

            return respuesta;
        }

        protected void LimpiarPanel()
        {
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
        }

        protected void MostrarDatos(int id_tipopago)
        {
            btnGuardar.Text = "Editar";
            btnGuardar.CommandName = "Editar";

            EstablecerValoresGenerales();
            objCEMant.ID_Mant_Valor = id_tipopago;

            var tbl = new DataTable();
            tbl = objCNMant.SelectMantenimientoRegistro(objCEMant);
            var row = tbl.Rows[0];
            
            txtNombre.Text = row[objCEMant.Nombre_Mant].ToString();
            txtDescripcion.Text = row[objCEMant.Descripcon_Mant].ToString();


        }

        protected void EliminarTipoPago(int id_tipopago)
        {
            EstablecerValoresGenerales();
            objCEMant.ID_Mant_Valor = id_tipopago;
            objCNMant.DeleteMantenimiento(objCEMant);
        }

        protected void EstablecerValoresGenerales()
        {
            objCEMant.TBL_Mant = "G_TipoPagos";
            objCEMant.ID_Mant = "Id_TipoPago";
            objCEMant.Nombre_Mant = "Nombre";
            objCEMant.Descripcon_Mant = "Descripcion";
        }
        #endregion

        #region Obtener Valores del Formulario

        protected string getNombre()
        {
            return txtNombre.Text;
        }

        protected string getDescripcion()
        {
            return txtDescripcion.Text;
        }

        #endregion
    }
}