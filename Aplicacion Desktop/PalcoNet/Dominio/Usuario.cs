using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    public class Usuario
    {
        string nombreUsuario;
        string contrasenia;
        string calle;
        string ciudad;
        string localidad;
        int numeroDeCalle;
        int piso;
        string depto;
        string codigoPostal;
        
        public int Piso
        {
            get { return piso; }
            set { piso = value; }
        }
        
        public string Calle
        {
            get { return calle; }
            set { calle = value; }
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
        
        public int NumeroDeCalle
        {
            get { return numeroDeCalle; }
            set { numeroDeCalle = value; }
        }

        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }

        public string Contrasenia
        {
            get { return contrasenia; }
            set { contrasenia = value; }
        }
        public string Departamento
        {
            get { return depto; }
            set { depto = value; }
        }
        public string CodigoPostal
        {
            get { return codigoPostal; }
            set { codigoPostal = value; }
        }
    }
}
