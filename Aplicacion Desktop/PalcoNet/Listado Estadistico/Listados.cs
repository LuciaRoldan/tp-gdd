using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Dominio;

namespace PalcoNet.Listado_Estadistico
{
    public partial class Listados : MiForm
    {

        int anio;

        public int Anio
        {
            get { return anio; }
            set { anio = value; }
        }
        string trimestre;

        public string Trimestre
        {
            get { return trimestre; }
            set { trimestre = value; }
        }

        DateTime fechaInicio;

        public DateTime FechaInicio
        {
            get { return fechaInicio; }
            set { fechaInicio = value; }
        }
        DateTime fechaFin;

        public DateTime FechaFin
        {
            get { return fechaFin; }
            set { fechaFin = value; }
        }





        public Listados()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime inicio = armarFechaInicio();
            DateTime fin = armarFechaFin();

            //aca le mando a la BD una consulta con las fechas de inicio y fin y me
            //devuelve una lista de los clientes con sus compras, de mayor a menor ordenado
            //por cantidad de compras. Agrupando las publicaciones por empresa.
            //Nombre - Apellido - Usuario - Empresa - CantidadCompras

            List<CompraCliente> comprasClientes = new List<CompraCliente>();

            new ClientesMuchasCompras(comprasClientes, this).Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime inicio = armarFechaInicio();
            DateTime fin = armarFechaFin();

            new EmpresaLocalidades(inicio, fin, this).Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            DateTime inicio = armarFechaInicio();
            DateTime fin = armarFechaFin();

            List<Cliente> clientesOrdenadosPorPuntos = new List<Cliente>();
            //aca consulta a la BD por la lista de todos los clientes ordenados
            //por la cantidad de puntos vencidos DESC
            //a esa consulta le voy a pasar dos fechas: inicio y fin

            new ClientesPuntos(clientesOrdenadosPorPuntos, this).Show();
            this.Hide();
        }

        private void anioCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            anio = Int32.Parse(anioCombobox.SelectedValue.ToString());
        }


        private void trimestreCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            trimestre = trimestreCombobox.SelectedValue.ToString();
        }

        private DateTime armarFechaInicio()
        {
            switch (trimestre)
            {
                case "Enero-Marzo": return new DateTime(anio, 1, 1);
                case "Abril-Junio": return new DateTime(anio, 4, 1);
                case "Julio-Septiembre": return new DateTime(anio, 7, 1);
                case "Octubre-Diciembre": return new DateTime(anio, 10, 1);
                default: return new DateTime();
            }
        }

        private DateTime armarFechaFin()
        {
            switch (trimestre)
            {
                case "Enero-Marzo": return new DateTime(anio, 3, 31);
                case "Abril-Junio": return new DateTime(anio, 6, 30);
                case "Julio-Septiembre": return new DateTime(anio, 9, 31);
                case "Octubre-Diciembre": return new DateTime(anio, 12, 31);
                default: return new DateTime();
            }
        }
    }
}
