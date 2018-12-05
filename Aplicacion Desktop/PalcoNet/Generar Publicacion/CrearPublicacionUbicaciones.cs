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
    public partial class CrearPublicacionUbicaciones : MiForm
    {
        Empresa empresa;
        Publicacion publicacion;
        List<Ubicacion> ubicaciones = new List<Ubicacion>();

        public List<Ubicacion> Ubicaciones
        {
            get { return ubicaciones; }
            set { ubicaciones = value; }
        }

        public Publicacion Publicacion
        {
            get { return publicacion; }
            set { publicacion = value; }
        }

        public Empresa Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        }

        public CrearPublicacionUbicaciones(MiForm anterior, Publicacion publicacion, Empresa empresa) : base(anterior)
        {
            this.Publicacion = publicacion;
            this.Empresa = empresa;
            InitializeComponent();
            //Aca habria que buscar todos los rubros disponibles que hay que
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Se agrega a la lista de abajo
            //verificar campos
            if (this.VerificarCampos())
            {
                Ubicacion ubicacion = new Ubicacion();

                ubicacion.CantidadAsientos = Int32.Parse(textBoxCantidad.Text);
                ubicacion.Precio = Int32.Parse(textBoxPrecio.Text);
                ubicacion.TipoAsiento = comboBoxTipo.Text;
                ubicacion.Numerada = checkBoxNumerado.Checked;
                if (checkBoxNumerado.Checked) { ubicacion.CantidadFilas = Int32.Parse(textBoxFilas.Text); }

                this.Ubicaciones.Add(ubicacion);
                this.Publicacion.Ubicaciones.Add(ubicacion);
                this.actualizarUbicaciones();

            } else {
                string mensaje = "Los siguientes campos deben ser completados:";
                if (string.IsNullOrWhiteSpace(textBoxCantidad.Text)) { mensaje = mensaje + "\n Cantidad de Asientos"; }
                if (string.IsNullOrWhiteSpace(textBoxPrecio.Text)) { mensaje = mensaje + "\n Precio"; }
                if (comboBoxTipo.SelectedIndex <= -1) { mensaje = mensaje + "\n Tipo de Asiento"; }
                if (checkBoxNumerado.Checked ? string.IsNullOrWhiteSpace(textBoxFilas.Text) : false) { mensaje = mensaje + "\n Cantidad de Filas"; }

                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK);
            }

        }

        private void actualizarUbicaciones()
        {
            var bindingList = new BindingList<Ubicacion>(this.Ubicaciones);
            var source = new BindingSource(bindingList, null);
            dataGridView.DataSource = source;
        }

        public bool VerificarCampos()
        {
            return checkBoxNumerado.Checked ? !string.IsNullOrWhiteSpace(textBoxFilas.Text) : true
                && !string.IsNullOrWhiteSpace(textBoxCantidad.Text)
                && !string.IsNullOrWhiteSpace(textBoxPrecio.Text)
                && comboBoxTipo.SelectedItem != null;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            new Finalizar_publicacion(this, this.Empresa, this.Publicacion).Show();
            this.Hide(); 
        }

        private void radioButtonNumerados_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxNumerado_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNumerado.Checked)
            {
                textBoxFilas.Enabled = true;
            }
            else
            {
                textBoxFilas.Enabled = false;
            }
        }
    }
}
