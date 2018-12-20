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
        List<DateTime> fechas = new List<DateTime>();

        public List<DateTime> Fechas
        {
            get { return fechas; }
            set { fechas = value; }
        }

        internal Publicacion Publicacion
        {
            get { return publicacion; }
            set { publicacion = value; }
        }


        public AgregarFechas(MiForm anterior, Publicacion publicacion) : base(anterior)
        {
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
            //Verificamos que la fecha no sea anterior a hoy
            if (fecha > this.fechaMaximaActual() && !this.Publicacion.Fechas.Contains(fecha)){
                this.Fechas.Add(fecha);
                this.Publicacion.Fechas.Add(fecha);
                this.actualizarFechas();
            }
            else{
                MessageBox.Show("La fecha no puede ser anterior a la actual ni ser menor a la última fecha agregada.", "Error", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Se le agrega una lista de fechas al objeto de la publicacion
            if (this.Fechas.Count > 0){
                new CrearPublicacionUbicaciones(this, this.Publicacion).Show();
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

        private void RemoverFechaAnteBorrado(object sender, DataGridViewRowCancelEventArgs e)
        {
            this.publicacion.Fechas.Remove((DateTime) e.Row.DataBoundItem);
            this.actualizarFechas();            
        }

        private DateTime fechaMaximaActual()
        {
            if (this.fechas.Count == 0) { return Sesion.getInstance().fecha; }
            return this.fechas.Max();
        }

    }
}
