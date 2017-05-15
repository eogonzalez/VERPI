﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Capa_Entidad.General;
using Capa_Negocio.General;
using System.Data;
using System.Web.UI.HtmlControls;

namespace VERPI.PreIngresos.Marcas
{
    public partial class PreIngreso : System.Web.UI.Page
    {
        CEFormularios objCEFormulario = new CEFormularios();
        CNFormularios objCNFormulario = new CNFormularios();

        #region Eventos del formulario

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LlenarFormulario();
            }
        }

        #endregion


        #region Funciones

        protected void LlenarFormulario()
        {
            objCEFormulario.Nombre_Tabla = "M_Campos_Formulario";

            var no_formulario = 1;
            objCEFormulario.No_Formulario = no_formulario;
          
            var dt_total_campos = new DataTable();
            var cantidad_s1 = 0;
            var cantidad_s2 = 0;
            var cantidad_s3 = 0;
            dt_total_campos = objCNFormulario.SelectCantidadCamposFormulario(no_formulario);
            foreach (DataRow rowc in dt_total_campos.Rows)
            {
                switch (rowc["seccion"].ToString())
                {
                    case "1":
                        if (rowc["total"] != null)
                        {
                            cantidad_s1 = (int)rowc["total"];
                        }
                        
                        break;
                    case "2":
                        if (rowc["total"] != null)
                        {
                            cantidad_s2 = (int)rowc["total"];
                        }

                        
                        break;
                    case "3":
                        if (rowc["total"] != null)
                        {
                            cantidad_s3 = (int)rowc["total"];
                        }                        
                        break;
                }
            }

            var dt = new DataTable();
            dt = objCNFormulario.SelectCamposFormulario(objCEFormulario);

            int cont = 1;
            foreach (DataRow row in dt.Rows)
            {/*Recorro los campos del formulario a construir*/
                
                switch (row["seccion"].ToString())
                {
                    case "1":
                        ConstruirControlesCiclo(pnl_seccion_1, row, cantidad_s1, ref cont);
                        break;
                    case "2":
                        ConstruirControlesCiclo(pnl_seccion_2, row, cantidad_s2, ref cont);
                        break;
                    case "3":
                        ConstruirControlesCiclo(pnl_seccion_3, row, cantidad_s3, ref cont);
                        break;                    
                }

                
            }

        }

        protected void ConstruirControlesCiclo(HtmlGenericControl panel, DataRow row, int cantidad_controles,ref int cont)
        {
            if (cantidad_controles >= 6)
            {
                //Agrego Primer div
                if (cont == 1)
                {
                    panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                }

                //Creo controles
                ConstruirControles(panel, row, cantidad_controles);

                if (cont == 2)
                {
                    panel.Controls.Add(new LiteralControl("</div>"));
                    cont = 0;
                }

                cont++;

            }
            else {
                panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                //Si es la seccion 1, se agregan campos en encabezado del formulario
                ConstruirControles(panel, row, cantidad_controles);
                panel.Controls.Add(new LiteralControl("</div>"));
            }
        }

        protected void ConstruirControles(HtmlGenericControl panel, DataRow row, int total_campos)
        {
            /*Agrego Label*/
            var label = new Label();
            label.Text = row["etiqueta"].ToString();

            label.CssClass = "control-label col-xs-2";
            panel.Controls.Add(label);

            if (total_campos >= 6)
            {
                panel.Controls.Add(new LiteralControl("<div class='col-xs-4'>"));
            }
            else
            {
                panel.Controls.Add(new LiteralControl("<div class='col-xs-10'>"));
            }


            switch (row["tipo_control"].ToString())
            {
                case "1":
                    //Si es textbox
                    TextBox MiTexBox = new TextBox();
                    MiTexBox.ID = row["nombre_control"].ToString();
                    MiTexBox.CssClass = "form-control";
                    MiTexBox.ToolTip = row["descripcion"].ToString();
                   
                    switch (row["modo_texto"].ToString())
                    {
                        case "Number":
                            //panel.Controls.Add(new LiteralControl("<div class='col-xs-2'>"));
                            MiTexBox.TextMode = TextBoxMode.Number;
                            break;
                        case "SingleLine":
                            //panel.Controls.Add(new LiteralControl("<div class='col-xs-10'>"));
                            MiTexBox.TextMode = TextBoxMode.SingleLine;
                            break;
                        case "Multiline":
                            //panel.Controls.Add(new LiteralControl("<div class='col-xs-10'>"));
                            MiTexBox.TextMode = TextBoxMode.MultiLine;
                            break;
                        case "Email":
                            //panel.Controls.Add(new LiteralControl("<div class='col-xs-10'>"));
                            MiTexBox.TextMode = TextBoxMode.Email;
                            break;
                    }
                    
                    panel.Controls.Add(MiTexBox);
                    break;

                case "2":
                    
                    //Si es dropdowlist
                    DropDownList MiCombo = new DropDownList();
                    MiCombo.ID = row["nombre_control"].ToString();
                    MiCombo.CssClass = "form-control";
                    MiCombo.ToolTip = row["descripcion"].ToString();
                    //Metodo para llenar el combo
                    panel.Controls.Add(MiCombo);
                    break;

                case "3":
                    //Si es Date
                    
                    break;
                
            }

            panel.Controls.Add(new LiteralControl("</div>"));

        }

        #endregion
    }
}