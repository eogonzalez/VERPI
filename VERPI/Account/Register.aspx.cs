using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using VERPI.Models;
using Capa_Entidad.General;
using Capa_Negocio.General;
using System.Net.Mail;
using System.Net.Mime;

namespace VERPI.Account
{
    public partial class Register : Page
    {
        CNLogin objCNLogin = new CNLogin();
        CEUsuario objCEUsuario = new CEUsuario();
        CNUsuario objCNUsuario = new CNUsuario();

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            //Obtengo datos del usuario para creacion
            objCEUsuario.CE_Nombres = NombreUsuario.Text;
            objCEUsuario.CE_Apellidos = ApellidoUsuario.Text;
            objCEUsuario.CE_CUI = CuiUsuario.Text;
            objCEUsuario.CE_Telefono = txtNumero.Text;            
            objCEUsuario.CE_Direccion = txtDireccion.Text;
            objCEUsuario.CE_Correo = Email.Text;
            objCEUsuario.CE_Password = Password.Text;
            objCEUsuario.CE_Estado = "C";
            string contraseñaConfirma = ConfirmPassword.Text;

            //Valido que las contraseñas son iguales
            if (objCEUsuario.CE_Password == contraseñaConfirma)
            {
                //Valido si usuario ya ha sido creado
                if (objCNLogin.AutenticarRegistro(objCEUsuario.CE_Correo))
                {
                    ErrorMessage.Text = "Ya existe un Usuario registrado con esta direccion de correo.";
                }
                else
                {

                    if (objCNUsuario.RegistrarUsuario(objCEUsuario))
                    {
                        Random rnd = new Random();
                        string code = Convert.ToString(rnd.Next(DateTime.Now.Day, DateTime.Now.Month + 100));

                        if (objCNLogin.InsertCodigoRecuperacion(Email.Text, code))
                        {
                            //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, Email.Text, Request);                            
                            string absoluteDir = IdentityHelper.GetUserConfirmationRedirectUrl(code, Email.Text, Request);
                            string relativePath = absoluteDir.Replace("/Account", "/verpi/Account");
                            string callbackUrl = relativePath;

                            string mensaje = "Apreciable usuario " + objCEUsuario.CE_Nombres + " " + objCEUsuario.CE_Apellidos + " Bienvenido al Sistema de Ventanilla Electrónica del Registro de la Propiedad Intelectual -VERPI- </br> " +
                                "Hemos recibido su solicitud de registro al sistema.</br> " +
                                "Puede acceder al Sistema con los datos registrados.</br>" +
                                 "Confirmando el registro haciendo clic <a href=\"" + callbackUrl + "\">aquí</a>.";

                            //Enviar correo
                            
                            if (EnvioMensajeRegistro(mensaje, objCEUsuario.CE_Correo))
                            {
                                //Muestra a usuario pantalla que ha sido registrado y que revise su correo
                                Response.Redirect("RegisterDone.aspx");                                
                            }
                            else
                            {
                                ErrorMessage.Text = "Mensaje no enviado!";
                            }
                        }
                        else
                        {
                            ErrorMessage.Text = "No se ha podido generar codigos de confirmarcion de registro en este momento.";
                        }

                    }
                    else
                    {
                        ErrorMessage.Text = "Ha ocurrido un error al registrar usuario";
                    }

                }
            }
            else
            {
                ErrorMessage.Text = "Contraseña no coincide, verifique.";
            }
        }

        protected Boolean EnvioMensajeRegistro(string mensaje, string correo)
        {
            Boolean Enviado = false;
            try
            {

                Correo Cr = new Correo();
                MailMessage mnsj = new MailMessage();

                mnsj.Subject = "Registro -VERPI-";

                mnsj.To.Add(new MailAddress(correo));

                mnsj.From = new MailAddress("alertas.verpi@gmail.com", "Alertas VERPI");

                /* Si deseamos Adjuntar algún archivo*/
                //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));

                //mnsj.Body = "Apreciable usuario " + nombre + " " + apellido + " Bienvenido al Sistema de Ventanilla Electrónica del Registro de la Propiedad Intelectual -VERPI- \n\n " +
                //    "Hemos recibido su solicitud de registro al sistema.\n\n" +
                //    "Puede acceder al Sistema con los datos registrados. \n\n" +
                //    "Nota: Favor de no responder este correo.";

                

                // Construir body alternativo de tipo HTML.
                string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                body += "</HEAD><BODY><DIV><FONT face=Arial color=#020202 size=2>" +mensaje;
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

        protected void Page_Load(object sender, EventArgs e)
        {
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
        }
    }
}