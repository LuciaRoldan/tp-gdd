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
        long cuit;
        DateTime fechaDeCreacion;
        int telefono;
        long id;
        string ciudad;
        string localidad;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

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

        public string Ciudad
        {
            get { return ciudad; }
            set { ciudad = value; }
        }

        public string Localidad
        {
            get { return localidad; }
            set { localidad = value; }
        }

        public long Cuit
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
