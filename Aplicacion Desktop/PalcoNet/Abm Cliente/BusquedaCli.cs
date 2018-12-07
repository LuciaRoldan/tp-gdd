using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Registro_de_Usuario;
using PalcoNet.Dominio;

namespace PalcoNet.Abm_Cliente
{
    public partial class BusquedaCli : MiForm
    {
        public BusquedaCli(MiForm formAnterior) : base(formAnterior)
        {
            InitializeComponent();
        }

        public bool verificarCampos() {
            string errores = "";
            int dni;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxNombre.Text)
                || !string.IsNullOrWhiteSpace(textBoxApellido.Text)
                || !string.IsNullOrWhiteSpace(textBoxDni.Text)
                || !string.IsNullOrWhiteSpace(textBoxMail.Text);

            if (!camposCompletos) {
                errores += "Se debe completar al menos un campo para realizar la búsqueda.";
            } else {
                if (!int.TryParse(textBoxDni.Text, out dni)) { errores += "El DNI debe ser un valor numérico."; }
            }

            if (errores != "") { 
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cliente cliente = new Cliente();
            new RegistroDeCliente(cliente, this).Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Aca hay que agarrar los valores, buscar y pasarle los resultados a la ventana
            if (this.verificarCampos())
            {
                Cliente cliente = new Cliente();
                cliente.Nombre = textBoxNombre.Text;
                cliente.Apellido = textBoxApellido.Text;
                cliente.Apellido = textBoxMail.Text;
                cliente.NumeroDeDocumento = Int32.Parse(textBoxDni.Text);
                List<Cliente> resultados = new List<Cliente>();
                
                //Aca hay que buscar en la base y obtener una lista de clientes que cumplan con los criterios de busqueda

                var bindingList = new BindingList<Cliente>(resultados);
                var source = new BindingSource(bindingList, null);
                dataGridResultados.DataSource = source;
            }
            
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            Cliente cliente = (Cliente)dataGridResultados.CurrentRow.DataBoundItem;
            this.Hide();
            new ModificarCli(cliente, this).Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxNombre.Text = "";
            textBoxApellido.Text = "";
            textBoxMail.Text = "";
            textBoxDni.Text = "";
        }

    }
}
