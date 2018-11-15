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
        Cliente cliente;

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
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
            comboBoxDocumento.SelectedIndex = comboBoxDocumento.FindStringExact(cliente.TipoDocumento);
            dateTimePickerNacimiento.Value = cliente.FechaDeNacimiento;
        }

        public bool clienteNoFueModifiado(Cliente clienteModificado) {
           /* return cliente.Nombre.Equals(clienteModificado.Nombre)
                && cliente.Apellido.Equals(clienteModificado.Apellido)
                && cliente.Mail.Equals(clienteModificado.Mail)
                && cliente.Telefono.Equals(clienteModificado.Telefono)
                && cliente.NumeroDeDocumento.Equals(clienteModificado.NumeroDeDocumento)
                && cliente.Cuil.Equals(clienteModificado.Cuil)
                && cliente.TipoDocumento.Equals(clienteModificado.TipoDocumento)
                && cliente.FechaDeNacimiento.Equals(clienteModificado.FechaDeNacimiento);*/
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cliente clienteModificado = new Cliente();
            clienteModificado.Apellido = textBoxApellido.Text;
            clienteModificado.Nombre = textBoxNombre.Text;
            clienteModificado.Mail = textBoxMail.Text;
            clienteModificado.Telefono = Int32.Parse(textBoxTelefono.Text);
            clienteModificado.NumeroDeDocumento = Int32.Parse(textBoxDocumento.Text);
            clienteModificado.Cuil = Int32.Parse(textBoxCuil.Text);
            clienteModificado.TipoDocumento = comboBoxDocumento.SelectedText;
            clienteModificado.FechaDeNacimiento = dateTimePickerNacimiento.Value;

            if (!this.clienteNoFueModifiado(clienteModificado))
            {
                //Aca hay que hacer el update en la base
            }

            new BusquedaCli(this.Anterior.Anterior.Anterior).Show();
            this.Close();
        }

        private void comboBoxDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
