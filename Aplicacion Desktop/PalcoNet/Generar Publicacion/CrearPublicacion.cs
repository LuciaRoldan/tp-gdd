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

        public CrearPublicacion(MiForm anterior) : base(anterior)
        {
            InitializeComponent();
            if (Sesion.getInstance().rol.Nombre == "Empresa")
            {
                Console.WriteLine("2");
                
                Empresa empresa = Sesion.getInstance().traerEmpresa();
                
                SqlDataReader reader = servidor.query("EXEC dbo.getRubros_sp");
                
                while (reader.Read())
                {
                    comboBoxRubro.Items.Add(reader["descripcion"].ToString());
                }
                reader.Close();
                //Aca habria que cargar los rubros existentes de la base y ponerlos en el combo box
            }
            else {
                MessageBox.Show("Se encuentra loggeado como " + Sesion.getInstance().rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad." +
                "Podrá simular el proceso de generación de publicacion pero no generarla.", "Advertencia", MessageBoxButtons.OK);
            }
        }

        public bool verificarCampos() {
            string errores = "";
            if(string.IsNullOrWhiteSpace(textBoxDescripcion.Text)) {errores += "El campo Descripción no puede estar vacío.\n"; }
            if(string.IsNullOrWhiteSpace(textBoxDireccion.Text)) {errores += "El campo Dirección no puede estar vacío.\n"; }
            if(comboBoxEstado.SelectedIndex < 0) {errores += "Se debe seleccionar un Estado.\n"; }
            if(comboBoxGrado.SelectedIndex < 0) {errores += "Se debe seleccionar un Grado.\n"; }
            if(comboBoxRubro.SelectedIndex < 0) {errores += "Se debe seleccionar un Rubro.\n"; }

            if (errores != "")
            {
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
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
                new AgregarFechas(this, publicacion).Show();
                this.Hide();
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
