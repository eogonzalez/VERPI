﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.General
{
    public class CEUsuario
    {
        public int ID_Usuario { get; set; }

        public string CE_Nombres { get; set; }

        public string CE_Apellidos { get; set; }

        public string CE_CUI { get; set; }

        public string CE_Telefono { get; set; }

        public string CE_Direccion { get; set; }

        public string CE_Correo { get; set; }

        public string CE_Password { get; set; }

        public int ID_TipoUsuario { get; set; }

        public int ID_UsuarioAutoriza { get; set; }

        public string CE_Estado { get; set; }
    }
}
