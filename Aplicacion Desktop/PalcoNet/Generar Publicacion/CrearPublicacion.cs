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
    public partial class CrearPublicacion : MiForm
    {
        Empresa empresa;
        Servidor servidor = Servidor.getInstance();

        public Empresa Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        }

        public CrearPublicacion(MiForm anterior, Empresa empresa) : base(anterior)
        {
            this.Empresa = empresa;
            InitializeComponent();

            SqlDataReader reader = servidor.query("EXEC dbo.getRubros_sp");

            while (reader.Read())
            {
                comboBoxRubro.Items.Add(reader["descripcion"].ToString());
            }
            reader.Close();
            //Aca habria que cargar los rubros existentes de la base y ponerlos en el combo box
        }

        public bool verificarCampos() {
            return !string.IsNullOrWhiteSpace(textBoxDescripcion.Text)
                && !string.IsNullOrWhiteSpace(textBoxDireccion.Text)
                && comboBoxEstado.SelectedIndex > -1
                && comboBoxGrado.SelectedIndex > -1
                && comboBoxRubro.SelectedIndex > -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.cerrarAnteriores();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Se tiene que comenzar a crear el objeto de la publicacion con los datos
            if (this.verificarCampos())
            {
                Publicacion publicacion = new Publicacion();
                publicacion.Descripcion = textBoxDescripcion.Text;
                publicacion.Direccion = textBoxDireccion.Text;
                publicacion.GradoDePublicacion = comboBoxGrado.Text;
                publicacion.EstadoDePublicacion = comboBoxEstado.Text;
                publicacion.Rubro = comboBoxRubro.Text;
                new AgregarFechas(this, publicacion, this.Empresa).Show();
                this.Hide();
            }
            else{
                string mensaje = "Los siguientes campos deben ser completados:";
                if (string.IsNullOrWhiteSpace(textBoxDescripcion.Text)) { mensaje = mensaje + "\n Descripción"; }
                if (string.IsNullOrWhiteSpace(textBoxDireccion.Text)) { mensaje = mensaje + "\n Dirección"; }
                if (comboBoxEstado.SelectedIndex <= -1) { mensaje = mensaje + "\n Estado"; }
                if (comboBoxGrado.SelectedIndex <= -1) { mensaje = mensaje + "\n Grado"; }
                if (comboBoxRubro.SelectedIndex <= -1) { mensaje = mensaje + "\n Rubro"; }

                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK);
            }
            
        }

        private void comboBoxGrado_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBoxRubro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
