using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Capa_Negocio.General
{
    public class Correo
    {
        SmtpClient server = new SmtpClient("smtp.gmail.com", 587);

        public Correo()
        {
            /*
             * Autenticacion en el Servidor
             * Utilizaremos nuestra cuenta de correo
             *
             */

            server.Credentials = new System.Net.NetworkCredential("alertas.dace@gmail.com", "DACE87654321");
            server.EnableSsl = true;
        }

        public void EnviarCorreo(MailMessage mensaje)
        {
            server.Send(mensaje);
        }
    }
}
