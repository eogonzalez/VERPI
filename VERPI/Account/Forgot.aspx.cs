using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using VERPI.Models;
using Capa_Negocio.General;
//using WebMatrix.WebData;
using System.Net.Mail;
using System.Net.Mime;

namespace VERPI.Account
{
    public partial class ForgotPassword : Page
    {
        CNLogin objCNLogin = new CNLogin();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Forgot(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validar la dirección de correo electrónico del usuario
                if (objCNLogin.AutorizaLogin(Email.Text))
                {
                    // Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
                    // Enviar correo electrónico con el código y la dirección de redireccionamiento a la página de restablecimiento de contraseña
                    //string code = manager.GeneratePasswordResetToken(user.Id);

                    //string code = WebSecurity.GeneratePasswordResetToken("Eder");
                    //string code = Email.Text;
                    Random rnd = new Random();
                    
                    string code = Convert.ToString(rnd.Next(DateTime.Now.Day, DateTime.Now.Month + 100));
                    if (objCNLogin.InsertCodigoRecuperacion(Email.Text, code))
                    {
                        string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
                        EnvioMensaje("Restablecer contraseña", "Para restablecer la contraseña, haga clic <a href=\"" + callbackUrl + "\">aquí</a>.", Email.Text);

                        //manager.SendEmail(user.Id, "Restablecer contraseña", "Para restablecer la contraseña, haga clic <a href=\"" + callbackUrl + "\">aquí</a>.");
                        loginForm.Visible = false;
                        DisplayEmail.Visible = true;
                    }
                    else
                    {
                        FailureText.Text = "No se ha podido generar recuperacion de contraseña en este momento.";
                        ErrorMessage.Visible = true;
                        return;
                    }
                }
                else
                {
                    FailureText.Text = "El usuario no existe o no se ha confirmado.";
                    ErrorMessage.Visible = true;
                    return;
                }
            }
        }

        protected Boolean EnvioMensaje(string subject, string mensaje, string correo)
        {
            Boolean Enviado = false;
            try
            {

                Correo Cr = new Correo();
                MailMessage mnsj = new MailMessage();

                mnsj.Subject = subject;

                mnsj.To.Add(new MailAddress(correo));

                mnsj.From = new MailAddress("alertas.verpi@gmail.com", "Alertas VERPI");

                /* Si deseamos Adjuntar algún archivo*/
                //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));

                //mnsj.Body = mensaje+" Nota: Favor de no responder este correo.";

                // Construir body alternativo de tipo HTML.
                string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                body += "</HEAD><BODY><DIV><FONT face=Arial color=#020202 size=2>" + mensaje;
                body += "</font></div></br><DIV><strong><FONT face=Arial color=#020202 size=2>";
                body += "Nota: Favor de no responder este correo.</FONT></strong></DIV></BODY></HTML>";
                

                ContentType mimeType = new ContentType("text/html");
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                mnsj.AlternateViews.Add(alternate);

                /* Enviar */
                Cr.EnviarCorreo(mnsj);
                Enviado = true;

                //MessageBox.Show("El Mail se ha Enviado Correctamente", "Listo!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
                Enviado = false;
            }

            return Enviado;
        }
    }
}