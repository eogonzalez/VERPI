using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.General;
using Capa_Entidad.General;
using System.Data;

namespace Capa_Negocio.General
{
    public class CNLogin
    {
        Login objCDLogin = new Login();        

        public Boolean AutenticarLogin(string correo, string contraseña)
        {
           return objCDLogin.AutenticarLogin(correo, contraseña);
        }

        public Boolean AutenticarRegistro(string correo)
        {
            return objCDLogin.AutenticarRegistro(correo);
        }

        public Boolean RegistrarUsuario(CEUsuario objCEUsuario)
        {
            return objCDLogin.RegistrarUsuario(objCEUsuario);
        }

        public DataSet SelectSolicitudRegistroUsuarios()
        {
            return objCDLogin.SelectSolicitudRegistroUsuarios();
        }

        public Boolean AutorizaLogin(string correo)
        {
            return objCDLogin.AutorizaLogin(correo);
        }

        public Boolean Seguridad(int id_usuario, DateTime fecha_acceso, string dir_ip)
        {
            return objCDLogin.Seguridad(id_usuario, fecha_acceso, dir_ip);
        }

        public int ConsultaUsuarioId(string correo)
        {
            return objCDLogin.ConsultaUsuarioId(correo);
        }

        public DataTable SelectComboPerfiles()
        {
            return objCDLogin.SelectComboPerfiles();
        }

        public Boolean InsertAutorizacionPermisoUsuario(int id_usuario, int id_tipousuario, int id_usuarioAutoriza)
        {
            return objCDLogin.InsertAutorizacionPermisoUsuario(id_usuario, id_tipousuario, id_usuarioAutoriza);
        }

        public Boolean UpdateRechazoPermisoUsuario(int id_usuarioAutoriza, int id_usuario)
        {
            return objCDLogin.UpdateRechazoPermisoUsuario(id_usuarioAutoriza, id_usuario);
        }

        public DataTable SelectComboDepartamentos()
        {
            return objCDLogin.SelectComboDepartamentos();
        }

        public DataTable SelectDatosUsuario(int idUsuario)
        {
            return objCDLogin.SelectDatosUsuario(idUsuario);
        }
    }
}
