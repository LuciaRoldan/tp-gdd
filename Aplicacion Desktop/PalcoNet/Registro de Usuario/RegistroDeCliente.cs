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
    public partial class RegistroDeCliente : MiForm
    {
        Cliente cliente;

        internal Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        public RegistroDeCliente(Cliente cliente, MiForm formAnterior) : base(formAnterior)
        {
            this.cliente = cliente;
            InitializeComponent();
            comboBoxDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public bool VerificarCampos(){
            return !string.IsNullOrWhiteSpace(textBoxNombre.Text)
                && !string.IsNullOrWhiteSpace(textBoxApellido.Text)
                && !string.IsNullOrWhiteSpace(textBoxDocumento.Text)
                && !string.IsNullOrWhiteSpace(textBoxCuil.Text)
                && !string.IsNullOrWhiteSpace(textBoxMail.Text)
                && comboBoxDocumento.SelectedItem != null;
        }

        private void volver_Click_1(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide();
        }

        private void siguiente_Click(object sender, EventArgs e)
        {
            if (this.VerificarCampos())
            {
                cliente.FechaDeCreacion = DateTime.Now;
                cliente.Apellido = textBoxApellido.Text;
                cliente.Nombre = textBoxNombre.Text;
                cliente.Mail = textBoxMail.Text;
                cliente.NumeroDeDocumento = Int32.Parse(textBoxDocumento.Text);
                cliente.Cuil = Int32.Parse(textBoxCuil.Text);
                cliente.TipoDocumento = comboBoxDocumento.SelectedText;
                if (!string.IsNullOrWhiteSpace(textBoxTelefono.Text)) { Cliente.Telefono = Int32.Parse(textBoxTelefono.Text); }
                if (dateTimePickerNacimiento.Value != null) { Cliente.FechaDeNacimiento = dateTimePickerNacimiento.Value; }

                //Capaz aca hay que encriptar la contrasenia
                if (string.IsNullOrWhiteSpace(cliente.NombreUsuario)) {
                    cliente.NombreUsuario = textBoxDocumento.Text;
                    cliente.Contrasenia = textBoxDocumento.Text;
                }
                
                new RegistroDomicilio(this, cliente).Show();
                this.Hide();
            }
            
        }

        private void comboBoxDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
