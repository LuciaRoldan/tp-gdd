using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
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
        //Verificamos que todos los campos esten completos
        public bool VerificarCampos(){
            string error = "";
            long x;
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text)) {error += "El Nombre no puede estar vacío\n"; }
            if (string.IsNullOrWhiteSpace(textBoxApellido.Text)) {error += "El Apellido no puede estar vacío\n"; }
            if (string.IsNullOrWhiteSpace(textBoxDocumento.Text)) {error += "El Número de Documento no puede estar vacío\n"; }
            if (!long.TryParse(textBoxDocumento.Text, out x)) { error += "El campo 'Número de Documento' debe ser numérico\n"; }
            if (comboBoxDocumento.SelectedItem == null) {error += "El Tipo de Documento no puede estar vacío\n"; }
            if (string.IsNullOrWhiteSpace(textBoxCuil.Text)) {error += "El CUIL no puede estar vacío\n"; }
            if (!long.TryParse(textBoxCuil.Text, out x)) { error += "El campo 'CUIL' debe ser numérico\n"; }
            if (string.IsNullOrWhiteSpace(textBoxMail.Text)) {error += "El Mail no puede estar vacío\n"; }
            if (!long.TryParse(textBoxTelefono.Text, out x)) { error += "El campo 'Teléfono' debe ser numérico\n"; }
            if (Sesion.getInstance().fecha < dateTimePickerNacimiento.Value ) { error += "La fecha de nacimiento no es válida\n"; }

            if (error != "")
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void volver_Click_1(object sender, EventArgs e)
        {
            this.Close();
            if (this.Anterior == null) { new LogIn().Show(); } else { this.Anterior.Show(); }
        }

        private void siguiente_Click(object sender, EventArgs e)
        {
            if (this.VerificarCampos())
            {
                //Creamos el objeto cliete con todos los datos que ingreso el usuario
                cliente.FechaDeCreacion = Sesion.getInstance().fecha;
                cliente.Apellido = textBoxApellido.Text;
                cliente.Nombre = textBoxNombre.Text;
                cliente.Mail = textBoxMail.Text;
                cliente.NumeroDeDocumento = Int32.Parse(textBoxDocumento.Text);
                cliente.Cuil = long.Parse(textBoxCuil.Text);
                cliente.TipoDocumento = comboBoxDocumento.SelectedText;
                if (!string.IsNullOrWhiteSpace(textBoxTelefono.Text)) { Cliente.Telefono = long.Parse(textBoxTelefono.Text); }
                if (dateTimePickerNacimiento.Value != null) { Cliente.FechaDeNacimiento = dateTimePickerNacimiento.Value; }

                //Encriptamos la contraseña
                if (string.IsNullOrWhiteSpace(cliente.NombreUsuario) || cliente.DebeCambiarContraseña) {
                    cliente.NombreUsuario = textBoxDocumento.Text;
                   
                    StringBuilder Sb = new StringBuilder();
                    using (SHA256 hash = SHA256Managed.Create())
                    {
                        Encoding enc = Encoding.UTF8;
                        Byte[] result = hash.ComputeHash(enc.GetBytes(textBoxDocumento.Text.ToString()));

                        foreach (Byte b in result)
                            Sb.Append(b.ToString("x2"));

                        Console.WriteLine("EL HASH ES:" + Sb.ToString());
                    }
                    cliente.Contrasenia = Sb.ToString();
                    cliente.DebeCambiarContraseña = true;
                }
                
                new RegistroDomicilio(this, cliente).Show();
                this.Hide();
            }
            
        }

        private void comboBoxDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerNacimiento_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNombre_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
