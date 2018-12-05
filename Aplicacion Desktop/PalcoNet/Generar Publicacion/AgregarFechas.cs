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

namespace PalcoNet.Generar_Publicacion
{
    public partial class AgregarFechas : MiForm
    {
        Publicacion publicacion;
        Empresa empresa;
        List<DateTime> fechas = new List<DateTime>();

        public List<DateTime> Fechas
        {
            get { return fechas; }
            set { fechas = value; }
        }

        public Empresa Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        }

        internal Publicacion Publicacion
        {
            get { return publicacion; }
            set { publicacion = value; }
        }


        public AgregarFechas(MiForm anterior, Publicacion publicacion, Empresa empresa) : base(anterior)
        {
            this.Empresa = empresa;
            this.Publicacion = publicacion;
            InitializeComponent();
        }

        private void actualizarFechas() {
            var bindingList = new BindingList<DateTime>(this.Fechas);
            var source = new BindingSource(bindingList, null);
            dataGridView.DataSource = source;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Hace que se guarde la fecha en la lista de abajo
            DateTime fecha = dateTimePickerFecha.Value.Date + dateTimePickerHora.Value.TimeOfDay;
            //Hay que verificar que las fechas no sean anteriore a hoy
            this.Fechas.Add(fecha);
            this.Publicacion.Fechas.Add(fecha);
            this.actualizarFechas();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Se le agrega una lista de fechas al objeto de la publicacion
            if (this.Fechas.Count > 0){
                new CrearPublicacionUbicaciones(this, this.Publicacion, this.Empresa).Show();
                this.Hide();
            }
            else {
                MessageBox.Show("Se debe ingresar al menos una fecha", "Error", MessageBoxButtons.OK);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }
    }
}
