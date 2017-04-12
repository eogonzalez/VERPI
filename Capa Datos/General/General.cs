using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;

namespace Capa_Datos.General
{
    public class General
    {
        public string EncodePassword(string originalPassword)
        {
            string respuesta = string.Empty;

            //Clave que se utilizara para encriptar el usuario y la contrasenia
            string clave = "7f9facc418f74439c5e9709832;0ab8a5:OCOdN5Wl,q8SLIQz8i|8agmu¬s13Q7ZXyno/";
            
            //Se instancia el objeto sh512 para posteriormente usuarlo para calcular la matriz de bytes especificada
            SHA512 sha512 = new SHA512CryptoServiceProvider();

            //Se crea un arreglo para convertir el usuario, la contrasenia y la calve a una secuencia de bytes.
            Byte[] inputBytes = (new UnicodeEncoding()).GetBytes(originalPassword+clave);

            Byte[] hash = sha512.ComputeHash(inputBytes);

            respuesta = Convert.ToBase64String(hash);

            return respuesta;
        }

        
        
    }
}
