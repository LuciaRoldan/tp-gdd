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
            return !string.IsNullOrWhiteSpace(textBoxNombre.Text)
                || !string.IsNullOrWhiteSpace(textBoxApellido.Text)
                || !string.IsNullOrWhiteSpace(textBoxDni.Text)
                || !string.IsNullOrWhiteSpace(textBoxMail.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Close(); 
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
                if (!string.IsNullOrWhiteSpace(textBoxNombre.Text)) { cliente.Nombre = textBoxNombre.Text; }
                if (!string.IsNullOrWhiteSpace(textBoxApellido.Text)) { cliente.Apellido = textBoxApellido.Text; }
                if (!string.IsNullOrWhiteSpace(textBoxMail.Text)) { cliente.NumeroDeDocumento = Int32.Parse(textBoxDni.Text); }
                List<Cliente> resultados = new List<Cliente>();
                
                //Aca hay que buscar en la base y obtener una lista de clientes que cumplan con los criterios de busqueda

                List<Cliente> lista = new List<Cliente>();
                Cliente clienteP = new Cliente();
                clienteP.Nombre = "Luis";
                clienteP.Apellido = "Lucena";
                clienteP.TipoDocumento = "DNI";
                clienteP.FechaDeNacimiento = new DateTime(2012, 05, 28);
                lista.Add(clienteP);

                new ResultadoBusquedaCli(lista, this).Show();
                this.Hide();
            }
            
        }
    }
}
