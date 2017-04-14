﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.General;
using System.Data;
using Capa_Entidad.General;

namespace Capa_Negocio.General
{
    public class CNUsuario
    {
        Usuario objCDUsuario = new Usuario();

        public DataTable SelectUsuarios()
        {
            return objCDUsuario.SelectUsuarios();
        }

        public DataTable SelectComboPerfiles()
        {
            return objCDUsuario.SelectComboPerfiles();
        }

        public int ConsultaUsuarioId(string correo)
        {
            return objCDUsuario.ConsultaUsuarioId(correo);
        }

        public bool GuardarUsuario(CEUsuario objCEUsuario)
        {
            return objCDUsuario.GuardarUsuario(objCEUsuario);
        }

        public DataTable SelectUsuario(int id_usuario)
        {
            return objCDUsuario.SelectUsuario(id_usuario);
        }
    }



 


}