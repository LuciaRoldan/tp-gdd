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

namespace PalcoNet.Registro_de_Usuario
{
    public partial class RegistroDomicilio : MiForm
    {
        Usuario usuario;

        public Usuario Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public RegistroDomicilio(MiForm formAnterior, Usuario usuario) : base(formAnterior)
        {
            this.Usuario = usuario;
            InitializeComponent();
        }

        private bool camposCompletos()
        {
            return !string.IsNullOrWhiteSpace(textBoxCalle.Text)
                && !string.IsNullOrWhiteSpace(textBoxNro.Text)
                && !string.IsNullOrWhiteSpace(textBoxCiudad.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            this.Anterior.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.camposCompletos()) {
                this.Usuario.Calle = textBoxCalle.Text;
                this.Usuario.NumeroDeCalle = Int32.Parse(textBoxNro.Text);
                this.Usuario.Ciudad = textBoxCiudad.Text;
                if (!string.IsNullOrWhiteSpace(textBoxLocalidad.Text)) { this.Usuario.Localidad = textBoxLocalidad.Text; }
                if (!string.IsNullOrWhiteSpace(textBoxPiso.Text)) { this.Usuario.Piso = Int32.Parse(textBoxPiso.Text); }
                this.Hide();
                this.cerrarAnteriores();
            }
            
        }

        private void textBoxCiudad_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
