﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Sistema_de_Gestion_Expedientes.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["UsuarioID"] = 0;
            Session.Clear();
            ViewState.Clear();
            FormsAuthentication.SignOut();
            //Server.Transfer("~/Default.aspx", false);
            //Server.Transfer("~/", false);
            Response.Redirect("~/Index.html");
            //FormsAuthentication.RedirectToLoginPage();                        
        }
    }
}