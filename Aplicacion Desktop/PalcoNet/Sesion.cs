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
