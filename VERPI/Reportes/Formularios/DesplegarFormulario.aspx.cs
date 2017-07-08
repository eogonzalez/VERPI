using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using Capa_Negocio.General;
using Capa_Negocio.Administracion;

namespace VERPI.Reportes.Formularios
{
    public partial class DesplegarFormulario : System.Web.UI.Page
    {
        Capa_Negocio.General.CNFormularios objCNFormulario = new Capa_Negocio.General.CNFormularios();
        Capa_Negocio.Administracion.CNFormularios objCNForm = new Capa_Negocio.Administracion.CNFormularios();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int no_formulario = 0;
                if (Request.QueryString["nf"] != null)
                {
                    no_formulario = Convert.ToInt32(Request.QueryString["nf"]);
                    Session.Add("no_formulario", no_formulario);
                }

                int noPreingreso = 0;
                if (Request.QueryString["np"] != null)
                {
                    noPreingreso = Convert.ToInt32(Request.QueryString["np"]);
                    Session.Add("noPreIngreso", noPreingreso);

                }



                //CrystalReportSource1.Report.FileName = "~/CrystalReport1.rpt";
                var tbl = new DataTable();
                tbl = objCNForm.SelectFormulario(no_formulario);
                if (tbl.Rows.Count > 0)
                {
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

                        DataTable dt = new DataTable();
                        dt = objCNFormulario.SelectValoresFormulario(noPreingreso);

                        foreach (DataRow row in dt.Rows)
                        {
                            string ID_Control = row["nombre_control"].ToString();
                            string valor = row["valor"].ToString();

                            try
                            {
                                reporte.DataDefinition.FormulaFields[ID_Control].Text = "'" + valor + "'";
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