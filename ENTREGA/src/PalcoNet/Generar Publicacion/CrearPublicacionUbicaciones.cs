using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Dominio;

namespace PalcoNet.Generar_Publicacion
{
    public partial class CrearPublicacionUbicaciones : MiForm
    {
        Servidor servidor = Servidor.getInstance();
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

        public CrearPublicacionUbicaciones(MiForm anterior, Publicacion publicacion) : base(anterior)
        {
            this.Publicacion = publicacion;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Se agrega a la lista de abajo cada ubicación
            //verificar campos
            if (this.VerificarCampos())
            {
                Ubicacion ubicacion = new Ubicacion();

                ubicacion.CantidadAsientos = Int32.Parse(textBoxCantidad.Text);
                ubicacion.Precio = decimal.Parse(textBoxPrecio.Text);
                ubicacion.TipoAsiento = textBoxTipo.Text;
                ubicacion.Numerada = checkBoxNumerado.Checked;
                if (checkBoxNumerado.Checked) { ubicacion.CantidadFilas = Int32.Parse(textBoxFilas.Text); }

                this.Ubicaciones.Add(ubicacion);
                this.Publicacion.Ubicaciones.Add(ubicacion);
                this.actualizarUbicaciones();

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
            string errores = "";
            int x;
            decimal y;
            if (checkBoxNumerado.Checked ? !int.TryParse(textBoxFilas.Text, out x) : false) { errores += "El campo Cantidad de Filas debe contener un valor numérico.\n"; }
            if(!int.TryParse(textBoxCantidad.Text, out x)) {errores += "El campo Cantidad de Asientos debe contener un valor numérico.\n" ;}
            if(!decimal.TryParse(textBoxPrecio.Text, out y)){errores += "El campo Precio debe contener un valor numérico.\n"; }
            if(string.IsNullOrWhiteSpace(textBoxTipo.Text)) {errores += "Se debe completar con un Tipo de Asiento.\n"; }
           
            if (errores != "") { 
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        //Agregamos todas las ubicaciones a la publicacion
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.Ubicaciones.Count() > 0) {
                new Finalizar_publicacion(this, this.Publicacion).Show();
                this.Hide();
            } else {
                MessageBox.Show("Se debe ingresar al menos una Ubicación.", "Error", MessageBoxButtons.OK);
            }
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
