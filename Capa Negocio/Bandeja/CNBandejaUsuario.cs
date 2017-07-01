using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos.Bandeja;
using System.Data;

namespace Capa_Negocio.Bandeja
{
    public class CNBandejaUsuario
    {
        BandejaUsuario objCDBandeja = new BandejaUsuario();

        public DataTable SelectFormulariosBorrador(int id_usuario_solicita, int estado = 0, int tipo_tramite = 0, string fecha_inicial = "", string fecha_final = "")
        {
            return objCDBandeja.SelectFormulariosBorrador(id_usuario_solicita, estado, tipo_tramite, fecha_inicial, fecha_final);
        }

        public int SelectCantidadBorradores(int id_usuario_solicita)
        {
            return objCDBandeja.SelectCantidadBorradores(id_usuario_solicita);
        }
    }
}
