using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalcoNet.Dominio;
using System.Configuration;

namespace PalcoNet
{
    class Sesion
    {
        public Rol rol { get; set; }
        public Usuario usuario { get; set; }
        public DateTime fecha = DateTime.ParseExact(ConfigurationManager.AppSettings["horarioSistema"], "yyyy-dd-MM HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

        public static Sesion sesion { get; set; }

        public Empresa empresaDefault()
        {
            Empresa empresa = new Empresa();
            empresa.NombreUsuario = "19-67139304-09";
            return empresa;

        }

        public Rol rolEmpresa()
        {
            Rol rol = new Rol();
            rol.Nombre = "Empresa";
            return rol;
        }

        public String rolCliente()
        {
            return "Cliente";
        }


        public static Sesion getInstance()
        {
            if (sesion == null)
            {
                sesion = new Sesion();

            }
            return sesion;
        }
    }
}
