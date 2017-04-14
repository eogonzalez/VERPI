using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using VERPI.Models;
using Capa_Negocio.General;
using Capa_Entidad.General;

namespace VERPI.Account
{
    public partial class ResetPassword : Page
    {
        CNLogin objCNLogin = new CNLogin();
        CEUsuario objCEUsuario = new CEUsuario();

        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);
            if (code != null)
            {
                if (!objCNLogin.AutorizaLogin(Email.Text))
                {
                    ErrorMessage.Text = "El correo no tienen ningún usuario relacionado.";
                    return;
                }
                else
                {
                    if (objCNLogin.ValidoCodigoRecuperacion(Email.Text, code))
                    {

                        //Obtengo datos del usuario para actualizacion de password
                        objCEUsuario.CE_Correo = Email.Text;
                        objCEUsuario.CE_Password = Password.Text;
                        string contraseñaConfirma = ConfirmPassword.Text;

                        //Valido que las contraseñas son iguales
                        if (objCEUsuario.CE_Password == contraseñaConfirma)
                        {
                                if (objCNLogin.UpdateContraseña(objCEUsuario))
                                {
                                    Response.Redirect("~/Account/ResetPasswordConfirmation");
                                    return;
                                }
                                else
                                {
                                    ErrorMessage.Text = "Ha ocurrido un error al registrar usuario";
                                    return;
                                }
                        }
                        else
                        {
                            ErrorMessage.Text = "Contraseña no coincide, verifique.";
                            return;
                        }
                    }
                    else
                    {
                        ErrorMessage.Text = "La direccion de recuperacion ha expirado o no ha generado una recuperacion de contraseña correcta, favor genere un nuevo correo de recuperacion de contraseña.";
                        return;
                    }
                }
            }

            ErrorMessage.Text = "Se produjo un error";
        }
    }
}