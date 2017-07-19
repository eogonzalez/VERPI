using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using Capa_Negocio.Administracion;
using Capa_Negocio.Reportes;

namespace VERPI.Reportes.Formularios
{
    public partial class DesplegarFormulario : System.Web.UI.Page
    {        
        CNFormularios objCNForm = new CNFormularios();
        CNDesplegarFormulario objCNDesplegar = new CNDesplegarFormulario();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int no_formulario = 0;
                if (Request.QueryString["nf"] != null)
                {/*Valido no_formulario*/
                    no_formulario = Convert.ToInt32(Request.QueryString["nf"]);
                    Session.Add("no_formulario", no_formulario);
                }

                int noPreingreso = 0;
                if (Request.QueryString["np"] != null)
                {/*valido no_preingreso o numero electronico*/
                    noPreingreso = Convert.ToInt32(Request.QueryString["np"]);
                    Session.Add("noPreIngreso", noPreingreso);
                }

                /*Obtengo datos del formulario*/
                var tbl = new DataTable();
                tbl = objCNForm.SelectFormulario(no_formulario);

                if (tbl.Rows.Count > 0)
                {
                    /*Obtengo ruta del formulario .rpt*/
                    var rowForm = tbl.Rows[0];

                    if (rowForm["path_reporte"] != null)
                    {
                        string pathdb = rowForm["path_reporte"].ToString();

                        string path = Server.MapPath(pathdb);
                        ReportDocument reporte = new ReportDocument();
                        reporte.Load(path);

                        /*Agrego valores iniciales*/
                        reporte.DataDefinition.FormulaFields["txt_correoelectronico_tramitador"].Text = "'" + Session["CorreoUsuarioLogin"].ToString() + "'";
                        reporte.DataDefinition.FormulaFields["txt_fecha_ingreso"].Text = "'" + DateTime.Now.ToString() + "'";
                        reporte.DataDefinition.FormulaFields["txt_no_electronico"].Text = "'" + noPreingreso.ToString() + "'";

                        /*Selecciono valores a llenar dentro del formulario*/
                        DataTable dt = new DataTable();
                        dt = objCNDesplegar.SelectValoresFormularioReporte(noPreingreso, no_formulario);

                        foreach (DataRow row in dt.Rows)
                        {
                            string ID_Control = row["nombre_control"].ToString();
                            string valor = row["valor"].ToString();

                            try
                            {
                                /*Obtener tipo de campo*/
                                int tipo_control = objCNDesplegar.SelectTipoCampo((int)row["correlativo_campo"]);

                                switch (tipo_control)
                                {
                                    case 1:
                                        /*Si es textbox*/
                                        reporte.DataDefinition.FormulaFields[ID_Control].Text = "'" + valor + "'";
                                        break;
                                    case 2:
                                        /*Si es combo*/
                                        string valor_combo = objCNDesplegar.SelectValorCombo((int)row["correlativo_campo"], valor);
                                        reporte.DataDefinition.FormulaFields[ID_Control].Text = "'" + valor_combo + "'";
                                        break;
                                    case 3:
                                        /*Si es adjunto*/
                                        if (valor == "True")
                                        {
                                            reporte.DataDefinition.FormulaFields[ID_Control].Text = "'x'"; 
                                        }
                                        break;
                                    case 4:
                                        /*Si es checkbox*/
                                        if (valor == "True")
                                        {
                                            reporte.DataDefinition.FormulaFields[ID_Control].Text = "'x'";
                                        }
                                        break;
                                    case 5:
                                        /*Si es combo de pais*/
                                        string valor_combo_pais = objCNDesplegar.SelectValorComboPais(Convert.ToInt32(valor));
                                        reporte.DataDefinition.FormulaFields[ID_Control].Text = "'" + valor_combo_pais + "'";
                                        break;
                                    case 7:
                                        string valor_combo_niza = objCNDesplegar.SelectValorComboNiza(Convert.ToInt32(valor));
                                        reporte.DataDefinition.FormulaFields[ID_Control].Text = "'" + valor_combo_niza + "'";
                                        break;
                                }

                                
                            }
                            catch (Exception)
                            {

                                //throw;
                            }

                        }

                        string saveFilePath = Server.MapPath("~/doctos");
                        string nombreArchivo = noPreingreso.ToString() + "_formulario.pdf";
                        string nombreDocto = saveFilePath+ "\\" + nombreArchivo;
                        
                        Session.Add("nombre_docto", nombreArchivo);
                        reporte.ExportToDisk(ExportFormatType.PortableDocFormat, nombreDocto);
                        CrystalReportViewer1.ReportSource = reporte;

                    }

                }

                //ParameterFieldDefinitions crFieldDefinitions;
                //ParameterFieldDefinition crFieldDefinition;

                //ParameterValues crParameterValue = new ParameterValues();
                //ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                //crParameterDiscreteValue.Value = "Ejemplo";
                //crFieldDefinitions = reporte.DataDefinition.ParameterFields;
                //crFieldDefinition = crFieldDefinitions["txt_ejemplo"];
                //crParameterValue = crFieldDefinition.CurrentValues;

                //crParameterValue.Clear();
                //crParameterValue.Add(crParameterDiscreteValue);
                //crFieldDefinition.ApplyCurrentValues(crParameterValue);

            }
        }

        protected void btnVerFormulario_Click(object sender, EventArgs e)
        {
            string pathArchivo = Session["nombre_docto"].ToString();
            Response.Redirect("~/doctos/"+pathArchivo);
        }
    }
}