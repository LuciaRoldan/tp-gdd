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

namespace PalcoNet.Editar_Publicacion
{
    public partial class Editar_publicacion : MiForm
    {
        List<Publicacion> publicaciones = new List<Publicacion>();
        Publicacion publicacionElegida;

        public Publicacion PublicacionElegida
        {
            get { return publicacionElegida; }
            set { publicacionElegida = value; }
        }

        public List<Publicacion> Publicaciones
        {
            get { return publicaciones; }
            set { publicaciones = value; }
        }

        public Editar_publicacion(MiForm anterior, Empresa empresa)
        {
            InitializeComponent();

            //Aca hay que buscar en la base todas las publicaciones de la empresa y guardarlas en las lista de arriba
            foreach (Publicacion p in this.Publicaciones) {
                comboBoxPublicaciones.Items.Add(p.Descripcion);
            }
        }

        public bool VerificarCamposUbicacion()
        {
            return checkBoxNumerado.Checked ? !string.IsNullOrWhiteSpace(textBoxFilas.Text) : true
                && !string.IsNullOrWhiteSpace(textBoxCantidad.Text)
                && !string.IsNullOrWhiteSpace(textBoxPrecio.Text)
                && comboBoxTipo.SelectedItem != null;
        }

        public bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(textBoxDescripcion.Text)
                && !string.IsNullOrWhiteSpace(textBoxDireccion.Text)
                && comboBoxEstado.SelectedIndex > -1
                && comboBoxRubro.SelectedIndex > -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Hace que se completen los campos de abajo con la informacion de la publicacion seleccionada
            if (comboBoxPublicaciones.SelectedIndex != null) {
                this.publicacionElegida = this.Publicaciones[comboBoxPublicaciones.SelectedIndex];
                textBoxDescripcion.Text = publicacionElegida.Descripcion;
                textBoxDireccion.Text = publicacionElegida.Direccion;
                comboBoxRubro.Text = publicacionElegida.Rubro;
                comboBoxEstado.Text = publicacionElegida.EstadoDePublicacion;
                this.actualizarFechas();
                this.actualizarUbicaciones();
            } else {
                MessageBox.Show("Se debe sleccionar alguna publicación", "Error", MessageBoxButtons.OK);
            }
        }

        public void actualizarFechas() {
            var bindingList = new BindingList<DateTime>(this.PublicacionElegida.Fechas);
            var source = new BindingSource(bindingList, null);
            dataGridViewFechas.DataSource = source;
        }

        public void actualizarUbicaciones(){
            var bindingList = new BindingList<Ubicacion>(this.PublicacionElegida.Ubicaciones);
            var source = new BindingSource(bindingList, null);
            dataGridViewUbicaciones.DataSource = source;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Agrega una publicacion a la lista de abajo

            if (this.VerificarCamposUbicacion())
            {
                Ubicacion ubicacion = new Ubicacion();

                ubicacion.CantidadAsientos = Int32.Parse(textBoxCantidad.Text);
                ubicacion.Precio = Int32.Parse(textBoxPrecio.Text);
                ubicacion.TipoAsiento = comboBoxTipo.Text;
                ubicacion.Numerada = checkBoxNumerado.Checked;
                if (checkBoxNumerado.Checked) { ubicacion.CantidadFilas = Int32.Parse(textBoxFilas.Text); }

                this.PublicacionElegida.Ubicaciones.Add(ubicacion);
                this.actualizarUbicaciones();

            }
            else
            {
                string mensaje = "Los siguientes campos deben ser completados:";
                if (string.IsNullOrWhiteSpace(textBoxCantidad.Text)) { mensaje = mensaje + "\n Cantidad de Asientos"; }
                if (string.IsNullOrWhiteSpace(textBoxPrecio.Text)) { mensaje = mensaje + "\n Precio"; }
                if (comboBoxTipo.SelectedIndex <= -1) { mensaje = mensaje + "\n Tipo de Asiento"; }
                if (checkBoxNumerado.Checked ? string.IsNullOrWhiteSpace(textBoxFilas.Text) : false) { mensaje = mensaje + "\n Cantidad de Filas"; }

                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Agrega una fecha a la lista de abajo
            //Habria que verificar que la fecha sea valida

            DateTime fecha = dateTimePickerFecha.Value.Date + dateTimePickerFecha.Value.TimeOfDay;
            this.PublicacionElegida.Fechas.Add(fecha);
            this.actualizarFechas();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Hace los cambios que haya que hacer en la base
            //Podria salir un cartelito que diga que los cambios se guardaron correctamente

            if (this.VerificarCampos())
            {
                this.PublicacionElegida.Descripcion = textBoxDescripcion.Text;
                this.PublicacionElegida.Direccion = textBoxDireccion.Text;
                this.PublicacionElegida.EstadoDePublicacion = comboBoxEstado.Text;
                this.PublicacionElegida.Rubro = comboBoxRubro.Text;

                //Actualizar fechas y ubicaciones
                List<DateTime> fechas = new List<DateTime>();
                foreach (DataGridViewRow dr in dataGridViewFechas.Rows){
                    DateTime fecha = (DateTime) dr.DataBoundItem;
                    fechas.Add(fecha);
                }

                List<Ubicacion> ubicaciones = new List<Ubicacion>();
                foreach (DataGridViewRow dr in dataGridViewUbicaciones.Rows)
                {
                    Ubicacion ubicacion = (Ubicacion)dr.DataBoundItem;
                    ubicaciones.Add(ubicacion);
                }

                this.PublicacionElegida.Fechas = fechas;
                this.PublicacionElegida.Ubicaciones = ubicaciones;

                //Hay que actualizar en la base los cambios en la publicacion elegida, inclyendo en sus fechas y sus ubicaciones
                //3 SP uno de lo basico de publicacion, uno de las fechas y uno de las ubicaciones, en ese orden.
            }
            else
            {
                string mensaje = "Los siguientes campos deben ser completados:";
                if (string.IsNullOrWhiteSpace(textBoxDescripcion.Text)) { mensaje = mensaje + "\n Descripción"; }
                if (string.IsNullOrWhiteSpace(textBoxDireccion.Text)) { mensaje = mensaje + "\n Dirección"; }
                if (comboBoxEstado.SelectedIndex <= -1) { mensaje = mensaje + "\n Estado"; }
                if (comboBoxRubro.SelectedIndex <= -1) { mensaje = mensaje + "\n Rubro"; }

                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxDescripcion_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBoxRubro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
