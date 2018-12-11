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

namespace PalcoNet.Abm_Cliente
{
    public partial class ModificarCli : MiForm
    {
        bool fueModificado = false;
        Cliente clienteViejo;
        Servidor servidor = Servidor.getInstance();

        public bool FueModificado
        {
            get { return fueModificado; }
            set { fueModificado = value; }
        }

        public ModificarCli(Cliente cliente, MiForm formAnterior) : base(formAnterior)
        {
            InitializeComponent();
            comboBoxDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
            textBoxNombre.Text += cliente.Nombre;
            textBoxApellido.Text += cliente.Apellido;
            textBoxMail.Text += cliente.Mail;
            textBoxTelefono.Text += cliente.Telefono;
            textBoxDocumento.Text += cliente.NumeroDeDocumento;
            textBoxCuil.Text += cliente.Cuil;
            comboBoxDocumento.Text = cliente.TipoDocumento;
            dateTimePickerNacimiento.Value = cliente.FechaDeNacimiento;
            clienteViejo = cliente;
        }

        public bool verificarCampos() {
            string errores = "";
            int numero;
            long num;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxNombre.Text)
                && !string.IsNullOrWhiteSpace(textBoxApellido.Text)
                && !string.IsNullOrWhiteSpace(textBoxTelefono.Text)
                && !string.IsNullOrWhiteSpace(textBoxMail.Text)
                && !string.IsNullOrWhiteSpace(textBoxCuil.Text)
                && !string.IsNullOrWhiteSpace(textBoxDocumento.Text)
                && comboBoxDocumento.SelectedIndex > -1
                && dateTimePickerNacimiento.Value != null;

            if (!camposCompletos)
            {
                errores += "Todos los campos deben estar completos.";
            }
            else
            {
                if (!int.TryParse(textBoxDocumento.Text, out numero)) { errores += "El DNI debe ser un valor numérico. \n"; }
                if (!long.TryParse(textBoxCuil.Text, out num)) { errores += "El CUIL debe ser un valor numérico. \n"; }
                if (!long.TryParse(textBoxTelefono.Text, out num)) { errores += "El teléfono debe ser un valor numérico. \n"; }
                if (Sesion.getInstance().fecha < dateTimePickerNacimiento.Value) { errores += "La fecha de nacimiento no puede ser posterior a hoy. \n"; }
            }

            if (errores != "") { 
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos() && this.fueModificado){
                Cliente clienteModificado = new Cliente();
                clienteModificado.Apellido = textBoxApellido.Text;
                clienteModificado.Nombre = textBoxNombre.Text;
                clienteModificado.Mail = textBoxMail.Text;
                clienteModificado.Telefono = long.Parse(textBoxTelefono.Text);
                clienteModificado.NumeroDeDocumento = Int32.Parse(textBoxDocumento.Text);
                clienteModificado.Cuil = long.Parse(textBoxCuil.Text);
                clienteModificado.TipoDocumento = comboBoxDocumento.Text;
                clienteModificado.FechaDeNacimiento = dateTimePickerNacimiento.Value;
                //Aca hay que hacer el update en la base
                //sp que le paso el cuil (validar que el nuevo cuil no exista)del cliente que es unico para que busque el viejo y todos los datos nuevos

                String query = clienteViejo.Id + ", '" + clienteModificado.Nombre + "', '" + clienteModificado.Apellido 
                                + "', '" + clienteModificado.Mail + "', " + clienteModificado.NumeroDeDocumento + ", " + clienteModificado.Cuil
                                + ", " + clienteModificado.Telefono + ", '" + clienteModificado.FechaDeNacimiento + "', '" + clienteModificado.TipoDocumento + "'";

                servidor.realizarQuery("EXEC modificarCliente_sp " + query);
                MessageBox.Show("Los cambios se realizaron exitosamente.", "Modificar cliente", MessageBoxButtons.OK);
                this.cerrarAnteriores();
            }
        }

        private void comboBoxDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fueModificado = true;
        }

        private void textBoxNombre_TextChanged(object sender, EventArgs e)
        {
            this.fueModificado = true;
        }

        private void textBoxApellido_TextChanged(object sender, EventArgs e)
        {
            this.fueModificado = true;
        }

        private void textBoxTelefono_TextChanged(object sender, EventArgs e)
        {
            this.fueModificado = true;
        }

        private void textBoxMail_TextChanged(object sender, EventArgs e)
        {
            this.fueModificado = true;
        }

        private void textBoxCuil_TextChanged(object sender, EventArgs e)
        {
            this.fueModificado = true;
        }

        private void textBoxDocumento_TextChanged(object sender, EventArgs e)
        {
            this.fueModificado = true;
        }

        private void dateTimePickerNacimiento_ValueChanged(object sender, EventArgs e)
        {
            this.fueModificado = true;
        }
    }
}
