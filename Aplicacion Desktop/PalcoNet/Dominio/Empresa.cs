using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class Empresa : Usuario
    {
        string razonSocial;
        string mail;
        int cuit;
        DateTime fechaDeCreacion;
        string calle;
        int piso;
        string departamento;
        int numeroDeCalle;

        string codigoPostal;

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

        public string Calle
        {
            get { return calle; }
            set { calle = value; }
        }

        public int NumeroDeCalle
        {
            get { return numeroDeCalle; }
            set { numeroDeCalle = value; }
        }

        public int Piso
        {
            get { return piso; }
            set { piso = value; }
        }

        public string Departamento
        {
            get { return departamento; }
            set { departamento = value; }
        }

        public string CodigoPostal
        {
            get { return codigoPostal; }
            set { codigoPostal = value; }
        }
    }
}
