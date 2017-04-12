using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Capa_Datos.General
{
    public class Conexion
    {
        public SqlConnection Conectar()
        {
            SqlConnection conexion;
            
            conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

            return conexion;
        }
    }
}
