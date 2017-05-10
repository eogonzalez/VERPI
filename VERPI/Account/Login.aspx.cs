using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using VERPI.Models;
using System.Web.Security;
using Capa_Negocio.General;
using System.Data;

namespace VERPI.Account
{
    public partial class Login : Page
    {
        CNLogin cnLogin = new CNLogin();
        CNUsuario objCNUsuario = new CNUsuario();

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Habilite esta opción una vez tenga la confirmación de la cuenta habilitada para la funcionalidad de restablecimiento de contraseña
            ForgotPasswordHyperLink.NavigateUrl = "Forgot";

            if (!IsPostBack)
            {
                int i = 0;
                if (Session["UsuarioID"] != null && int.TryParse(Session["UsuarioID"].ToString(), out i))
                {
                    if (i > 0)
                    {
                        Response.Redirect("~/Default");
                    }

                }
            }
            

            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            //var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            //if (!String.IsNullOrEmpty(returnUrl))
            //{
            //    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            //}
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string strCorreo = string.Empty;
                string strContraseña = string.Empty;
                

                strCorreo = txtCorreo.Text;
                strContraseña = Password.Text;

                if (cnLogin.AutorizaLogin(strCorreo))
                {//Si el usuario esta autorizado
                    if (cnLogin.AutenticarLogin(strCorreo, strContraseña))
                    {//Si las credenciales son correctas
                        int idUsuario = objCNUsuario.ConsultaUsuarioId(strCorreo);

                        //Se almacena cuando el usuario ingresa al sistema
                        cnLogin.Seguridad(idUsuario, DateTime.Now, Convert.ToString(Request.ServerVariables["REMOTE_ADDR"]));

                        //Obtengo datos de usuario para variables de session
                        var tbl = new DataTable();
                        tbl = objCNUsuario.SelectDatosUsuario(idUsuario);
                        DataRow row = tbl.Rows[0];

                        Session["UsuarioID"] = idUsuario;
                        //Session.Add("CorreoUsuarioLogin", txtCorreo.Text);
                        Session.Add("CorreoUsuarioLogin", row["correo"].ToString());
                        Session.Add("NombresUsuarioLogin", row["nombres"].ToString());
                        Session.Add("ApellidosUsuarioLogin", row["apellidos"].ToString());


                        FormsAuthentication.RedirectFromLoginPage(strCorreo, RememberMe.Checked);
                    }
                    else
                    {//Si las credenciales son incorrectas
                        FailureText.Text = "Usuario o contraseña incorrecta.";
                        ErrorMessage.Visible = true;
                    }
                }
                else
                {//Si el usuario no esta autorizado o no existe
                    FailureText.Text = "Usuario no existe o aun no esta autorizado para ingresar.";
                    ErrorMessage.Visible = true;
                }
            }
        }
    }
}