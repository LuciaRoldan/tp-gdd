using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class Sesion
    {
        string rol;
        string usuario;
        DateTime fecha;
        Sesion sesion;

        public Sesion obtenerInstancia() {
            if (this.Sesion1 == null) {
                this.Sesion1 = new Sesion();
            }
            return sesion;
        }

        internal Sesion Sesion1
        {
            get { return sesion; }
            set { sesion = value; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public string Rol
        {
            get { return rol; }
            set { rol = value; }
        }
    }
}
