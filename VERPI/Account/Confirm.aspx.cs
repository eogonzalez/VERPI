using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using VERPI.Models;
using Capa_Negocio.General;

namespace VERPI.Account
{
    public partial class Confirm : Page
    {

        CNLogin objCNLogin = new CNLogin();
        CNUsuario objCNUsuario = new CNUsuario();
           
        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);
            string userId = IdentityHelper.GetUserIdFromRequest(Request);

            if (code != null && userId != null)
            {

                if (objCNLogin.ValidoCodigoRecuperacion(userId, code) && objCNUsuario.UpdateRegistro(userId))
                {
                    successPanel.Visible = true;
                    return;
                }

            }
            successPanel.Visible = false;
            errorPanel.Visible = true;
        }
    }
}