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
using System.Data.SqlClient;

namespace PalcoNet.Abm_Cliente
{
    public partial class ModificarCli : MiForm
    {
        bool fueModificado = false;
        Cliente clienteViejo;
        Servidor servidor = Servidor.getInstance();
        SqlDataReader readerCliente;
        String calle;
        Int32 numeroCalle;
        Int32 piso;
        String depto;
        String codigoPostal;
        MiForm formAnt;

        public bool FueModificado
        {
            get { return fueModificado; }
            set { fueModificado = value; }
        }

        //con los datos que obtuvimos de la busqueda completamos todos los campos para que la persona 
        //pueda modificar el que desea
        public ModificarCli(Cliente cliente, MiForm formAnterior) : base(formAnterior)
        {
            formAnt = formAnterior;
            InitializeComponent();
            comboBoxDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
            textBoxNombre.Text += cliente.Nombre;
            textBoxApellido.Text += cliente.Apellido;
            textBoxMail.Text += cliente.Mail;
            var telefono = cliente.Telefono;
            textBoxTelefono.Text += telefono == 0 ? null : telefono.ToString();
            var documento = cliente.NumeroDeDocumento;
            textBoxDocumento.Text += documento == 0 ? null : documento.ToString();
            var cuil = cliente.Cuil;
            textBoxCuil.Text += cuil == 0 ? null : cuil.ToString();
            textBoxCiudad.Text = cliente.Ciudad;
            textBoxLocalidad.Text = cliente.Localidad;
            comboBoxDocumento.Text = cliente.TipoDocumento;
            dateTimePickerNacimiento.Value = cliente.FechaDeNacimiento;
            clienteViejo = cliente;

            Servidor servidor = Servidor.getInstance();
            readerCliente = servidor.query("EXEC MATE_LAVADO.obtenerDatosAdicionalesCliente '" + cliente.Id + "'");

            readerCliente.Read();

            calle = readerCliente["calle"].ToString();
            var nro = readerCliente["numero_calle"];
            if (!(nro is DBNull)) numeroCalle = Convert.ToInt32(nro);
            var _piso = readerCliente["piso"];
            if (!(_piso is DBNull)) piso = Convert.ToInt32(piso);
            depto = readerCliente["depto"].ToString();
            codigoPostal = readerCliente["codigo_postal"].ToString();

            readerCliente.Close();

            textBoxCalle.Text += calle;
            textBoxNumeroCalle.Text += numeroCalle == 0 ? null : numeroCalle.ToString();
            textBoxPiso.Text += piso == 0 ? null : piso.ToString();
            textBoxDepto.Text += depto;
            textBoxCodigoPostal.Text += codigoPostal;
            
        }
        //verificamos que ninguno quede vacio
        public bool verificarCampos() {
            string errores = "";
            int numero;
            long num;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxNombre.Text)
                && !string.IsNullOrWhiteSpace(textBoxApellido.Text)
                && !string.IsNullOrWhiteSpace(textBoxMail.Text)
                && !string.IsNullOrWhiteSpace(textBoxDocumento.Text)
                && !string.IsNullOrWhiteSpace(textBoxCuil.Text)
                && !string.IsNullOrWhiteSpace(textBoxLocalidad.Text)
                && comboBoxDocumento.SelectedIndex > -1
                && dateTimePickerNacimiento.Value != null;

            if (!camposCompletos)
            {
                if (!string.IsNullOrWhiteSpace(textBoxNombre.Text)) {errores += "El campo Nombre no puede estar vacio.\n"; }
                if (!string.IsNullOrWhiteSpace(textBoxApellido.Text)) {errores += "El campo Apellido no puede estar vacio.\n"; }
                if (!string.IsNullOrWhiteSpace(textBoxMail.Text)) {errores += "El campo Mail no puede estar vacio.\n"; }
                if (!string.IsNullOrWhiteSpace(textBoxDocumento.Text)) {errores += "El campo Documento no puede estar vacio.\n"; }
                if (!string.IsNullOrWhiteSpace(textBoxCuil.Text)) {errores += "El campo CUIL no puede estar vacio.\n"; }
                if (!string.IsNullOrWhiteSpace(textBoxLocalidad.Text)) {errores += "El campo Localidad no puede estar vacio.\n"; }
                if (comboBoxDocumento.SelectedIndex > -1) {errores += "El campo Tipo de Documento no puede estar vacio.\n"; }
                if (dateTimePickerNacimiento.Value > Sesion.getInstance().fecha) { errores += "La Fecha de Nacimiento no puede ser posterior a hoy.\n"; }
            }
            else
            {
                if (!int.TryParse(textBoxDocumento.Text, out numero)) { errores += "El DNI debe ser un valor numérico. \n"; }
                if (!string.IsNullOrWhiteSpace(textBoxCuil.Text)) {if (!long.TryParse(textBoxCuil.Text, out num)) { errores += "El CUIL debe ser un valor numérico. \n"; }}
                if (!string.IsNullOrWhiteSpace(textBoxTelefono.Text)) { if (!long.TryParse(textBoxTelefono.Text, out num)) { errores += "El teléfono debe ser un valor numérico. \n"; } }
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
                if (!string.IsNullOrWhiteSpace(textBoxTelefono.Text)) { clienteModificado.Telefono = long.Parse(textBoxTelefono.Text); }
                clienteModificado.NumeroDeDocumento = Int32.Parse(textBoxDocumento.Text);
                if (!string.IsNullOrWhiteSpace(textBoxCuil.Text)) { clienteModificado.Cuil = long.Parse(textBoxCuil.Text); }
                clienteModificado.TipoDocumento = comboBoxDocumento.Text;
                clienteModificado.FechaDeNacimiento = dateTimePickerNacimiento.Value;
                //Aca hay que hacer el update en la base
                //sp que le paso el cuil (validamos que el nuevo cuil no exista)del cliente que es unico 
                //para que busque el viejo y todos los datos nuevos para ser actualizados
                calle = textBoxCalle.Text;
                numeroCalle = Convert.ToInt32(textBoxNumeroCalle.Text);
                if (!string.IsNullOrWhiteSpace(textBoxPiso.Text)) { piso = Int32.Parse(textBoxPiso.Text); }
                if (!string.IsNullOrWhiteSpace(textBoxDepto.Text)) { depto = textBoxDepto.Text; }
                codigoPostal = textBoxCodigoPostal.Text;
                clienteModificado.Ciudad = textBoxCiudad.Text;
                clienteModificado.Localidad = textBoxLocalidad.Text;

                String query = clienteViejo.Id + ", '" + clienteModificado.Nombre + "', '" + clienteModificado.Apellido
                                + "', '" + clienteModificado.Mail + "', '" + comboBoxDocumento.Text + "', " + clienteModificado.NumeroDeDocumento + ", " + clienteModificado.Cuil
                                + ", " + clienteModificado.Telefono + ", '" + clienteModificado.FechaDeNacimiento + "', '"
                                + calle + "', " + numeroCalle + ", " + piso + ", '" + depto + "', '" + codigoPostal + "', '" + clienteModificado.Ciudad + "', '" + clienteModificado.Localidad + "'";

                servidor.realizarQuery("EXEC MATE_LAVADO.modificarCliente_sp " + query);
                MessageBox.Show("Los cambios se realizaron exitosamente.", "Modificar cliente", MessageBoxButtons.OK);

                this.Anterior.Show();
                this.Close();

              
                //formAnt.Show();
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

        private void ModificarCli_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
