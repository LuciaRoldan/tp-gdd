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
        string tipoDocumento;
        long numeroDeDocumento;
        long cuil;
        string mail;
        long telefono;
        DateTime fechaDeCreacion;
        DateTime fechaDeNacimiento;
        int puntos;
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }


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

        public string TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }

        public long NumeroDeDocumento
        {
            get { return numeroDeDocumento; }
            set { numeroDeDocumento = value; }
        }

        public long Cuil
        {
            get { return cuil; }
            set { cuil = value; }
        }
        
        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        public long Telefono
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
        public int Puntos
        {
            get { return puntos; }
            set { puntos = value; }
        }
        
    }
}
