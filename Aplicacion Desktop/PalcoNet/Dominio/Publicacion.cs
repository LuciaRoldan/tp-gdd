﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    public class Publicacion
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        string gradoDePublicacion;
        string rubro;
        string descripcion;
        string estadoDePublicacion;
        DateTime fechaDeInicio;
        int cantidadDeAsientos;
        DateTime fechaDeEvento;
        string direccion;
        List<Ubicacion> ubicaciones = new List<Ubicacion>();
        List<DateTime> fechas = new List<DateTime>();

        public List<DateTime> Fechas
        {
            get { return fechas; }
            set { fechas = value; }
        }

        internal List<Ubicacion> Ubicaciones
        {
            get { return ubicaciones; }
            set { ubicaciones = value; }
        }

        public string GradoDePublicacion
        {
            get { return gradoDePublicacion; }
            set { gradoDePublicacion = value; }
        }

        public string Rubro
        {
            get { return rubro; }
            set { rubro = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public string EstadoDePublicacion
        {
            get { return estadoDePublicacion; }
            set { estadoDePublicacion = value; }
        }

        public DateTime FechaDeInicio
        {
            get { return fechaDeInicio; }
            set { fechaDeInicio = value; }
        }

        public DateTime FechaDeEvento
        {
            get { return fechaDeEvento; }
            set { fechaDeEvento = value; }
        }

        public int CantidadDeAsientos
        {
            get { return cantidadDeAsientos; }
            set { cantidadDeAsientos = value; }
        }

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }
    }
}
