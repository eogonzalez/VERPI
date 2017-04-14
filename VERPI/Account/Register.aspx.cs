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

namespace VERPI.Account
{
    public partial class Register : Page
    {
        CNLogin objCNLogin = new CNLogin();
        CEUsuario objCEUsuario = new CEUsuario();

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

                    if (objCNLogin.RegistrarUsuario(objCEUsuario))
                    {
                        //Enviar correo
                        //Muestra a usuario pantalla que ha sido registrado y que revise su correo
                        if (EnvioMensajeRegistro(objCEUsuario.CE_Nombres, objCEUsuario.CE_Apellidos, objCEUsuario.CE_Correo))
                        {
                            Response.Redirect("RegisterDone.aspx");
                            //Server.Transfer("RegisterDone.aspx", false);
                        }
                        else
                        {
                            ErrorMessage.Text = "Mensaje no enviado!";
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

        protected Boolean EnvioMensajeRegistro(string nombre, string apellido, string correo)
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

                mnsj.Body = "Apreciable usuario " + nombre + " " + apellido + " Bienvenido al Sistema de Ventanilla Electrónica del Registro de la Propiedad Intelectual -VERPI- \n\n " +
                    "Hemos recibido su solicitud de registro al sistema.\n\n" +
                    "Puede acceder al Sistema con los datos registrados. \n\n" +
                    "Nota: Favor de no responder este correo.";

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