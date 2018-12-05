using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet
{
    class Sesion
    {
        public string rol { get; set; }
        public string usuario { get; set; }
        public DateTime fecha;

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
