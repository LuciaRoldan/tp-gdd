using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    public class Cliente : Usuario
    {
        string nombre;
        string apellido;
        char[] tipoDocumento = new char[3];
        int numeroDeDocumento;
        int cuil;
        string mail;
        int telefono;
        DateTime fechaDeCreacion;
        DateTime fechaDeNacimiento;
        string codigoPostal;
        int puntos;


        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }
        }

        public char[] TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }

        public int NumeroDeDocumento
        {
            get { return numeroDeDocumento; }
            set { numeroDeDocumento = value; }
        }

        public int Cuil
        {
            get { return cuil; }
            set { cuil = value; }
        }

        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        public int Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        public DateTime FechaDeCreacion
        {
            get { return fechaDeCreacion; }
            set { fechaDeCreacion = value; }
        }

        public DateTime FechaDeNacimiento
        {
            get { return fechaDeNacimiento; }
            set { fechaDeNacimiento = value; }
        }

        public string CodigoPostal
        {
            get { return codigoPostal; }
            set { codigoPostal = value; }
        }

        public int Puntos
        {
            get { return puntos; }
            set { puntos = value; }
        }
        
    }
}
