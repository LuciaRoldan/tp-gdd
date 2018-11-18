using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    public class Empresa : Usuario
    {
        string razonSocial;
        string mail;
        int cuit;
        DateTime fechaDeCreacion;
        int telefono;

        public int Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        public string RazonSocial
        {
            get { return razonSocial; }
            set { razonSocial = value; }
        }

        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        public int Cuit
        {
            get { return cuit; }
            set { cuit = value; }
        }

        public DateTime FechaDeCreacion
        {
            get { return fechaDeCreacion; }
            set { fechaDeCreacion = value; }
        }
    }
}
