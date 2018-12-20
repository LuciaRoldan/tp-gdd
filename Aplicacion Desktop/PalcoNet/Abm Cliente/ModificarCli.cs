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
        Cliente clienteViejo;
        Servidor servidor = Servidor.getInstance();
        SqlDataReader readerCliente;
        String calle;
        Int32 numeroCalle;
        Int32 piso;
        String depto;
        String codigoPostal;
        MiForm formAnt;

        //con los datos que obtuvimos de la busqueda completamos todos los campos para que la persona 
        //pueda modificar el que desea
        public ModificarCli(Cliente cliente, MiForm formAnterior) : base(formAnterior)
        {
            formAnt = formAnterior;
            InitializeComponent();
            dateTimePickerNacimiento.MaxDate = Sesion.getInstance().fecha;
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

            textBoxNumeroCalle.Text += clienteViejo.NumeroDeCalle == 0 ? null : clienteViejo.NumeroDeCalle.ToString();
            textBoxPiso.Text += clienteViejo.Piso == 0 ? null : clienteViejo.Piso.ToString();
            textBoxCalle.Text += readerCliente["calle"].ToString();
            textBoxDepto.Text += readerCliente["depto"].ToString();
            textBoxCodigoPostal.Text += readerCliente["codigo_postal"].ToString();
            readerCliente.Close();

            if (cliente.Habilitado)
            {
                buttonD.Enabled = true;
                buttonH.Enabled = false;
            }
            else
            {
                buttonD.Enabled = false;
                buttonH.Enabled = true;
            }

            button1.Enabled = false;
        }
        //verificamos que ninguno quede vacio
        public bool verificarCampos()
        {
            string errores = "";
            int numero;
            long num;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxNombre.Text)
                && !string.IsNullOrWhiteSpace(textBoxApellido.Text)
                && !string.IsNullOrWhiteSpace(textBoxTelefono.Text)
                && !string.IsNullOrWhiteSpace(textBoxMail.Text)
                && !string.IsNullOrWhiteSpace(textBoxCuil.Text)
                && !string.IsNullOrWhiteSpace(textBoxNumeroCalle.Text)
                && !string.IsNullOrWhiteSpace(textBoxCalle.Text)
                && !string.IsNullOrWhiteSpace(textBoxLocalidad.Text)
                && !string.IsNullOrWhiteSpace(textBoxDocumento.Text)
                && comboBoxDocumento.SelectedIndex > -1
                && dateTimePickerNacimiento.Value != null;

            if (!string.IsNullOrWhiteSpace(textBoxNombre.Text)) { errores += "El campo Nombre no puede estar vacio.\n"; }
            if (!string.IsNullOrWhiteSpace(textBoxApellido.Text)) { errores += "El campo Apellido no puede estar vacio.\n"; }
            if (!string.IsNullOrWhiteSpace(textBoxMail.Text)) { errores += "El campo Mail no puede estar vacio.\n"; }
            if (!string.IsNullOrWhiteSpace(textBoxDocumento.Text)) { errores += "El campo Documento no puede estar vacio.\n"; }
            if (!string.IsNullOrWhiteSpace(textBoxCuil.Text)) { errores += "El campo CUIL no puede estar vacio.\n"; }
            if (!string.IsNullOrWhiteSpace(textBoxLocalidad.Text)) { errores += "El campo Localidad no puede estar vacio.\n"; }
            if (comboBoxDocumento.SelectedIndex > -1) { errores += "El campo Tipo de Documento no puede estar vacio.\n"; }
            if (dateTimePickerNacimiento.Value > Sesion.getInstance().fecha) { errores += "La Fecha de Nacimiento no puede ser posterior a hoy.\n"; }
            if (!int.TryParse(textBoxDocumento.Text, out numero)) { errores += "El DNI debe ser un valor numérico. \n"; }
            if (!string.IsNullOrWhiteSpace(textBoxCodigoPostal.Text)) { if (!long.TryParse(textBoxCuil.Text, out num)) { errores += "El CUIL debe ser un valor numérico. \n"; } }
            if (!long.TryParse(textBoxTelefono.Text, out num)) { errores += "El teléfono debe ser un valor numérico. \n"; }
            if (!string.IsNullOrWhiteSpace(textBoxPiso.Text) && !int.TryParse(textBoxPiso.Text, out numero)) { errores += "El Piso debe ser un valor numérico. \n"; }
            if (!int.TryParse(textBoxNumeroCalle.Text, out numero)) { errores += "El Numero de la Calle debe ser un valor numérico. \n"; }
            if (!string.IsNullOrWhiteSpace(textBoxCodigoPostal.Text) && !int.TryParse(textBoxCodigoPostal.Text, out numero)) { errores += "El Codigo Postal debe ser un valor numérico. \n"; }
            if (dateTimePickerNacimiento.Value < dateTimePickerNacimiento.MinDate) { errores += "La fecha de nacimiento no puede ser anterior al 1900. \n"; }
            if (Sesion.getInstance().fecha < dateTimePickerNacimiento.Value) { errores += "La fecha de nacimiento no puede ser posterior a hoy. \n"; }

                if (Sesion.getInstance().fecha < dateTimePickerNacimiento.Value) { errores += "La fecha de nacimiento no puede ser posterior a hoy. \n"; }

                //Si posee DNI, valido DNI y CUIL
                if (comboBoxDocumento.Text == "DNI")
                {

                    if (int.TryParse(textBoxDocumento.Text, out numero))
                    {
                        //Verificamos que el documento tenga el largo que corresponde

                        if (!(Int32.Parse(textBoxDocumento.Text) > 9999999 & Int32.Parse(textBoxDocumento.Text) < 100000000))
                        { errores += "El documento debe poseer 8 digitos. \n"; }
                    }

                    if (long.TryParse(textBoxCuil.Text, out num))
                    {
                        //Verificamos que el CUIL tenga el largo que corresponde
                        Console.Write(long.Parse(textBoxCuil.Text));
                        if (!(long.Parse(textBoxCuil.Text) > 9999999999 & long.Parse(textBoxCuil.Text) < 100000000000))
                        { errores += "El CUIL debe poseer 11 digitos. \n";
                        }else
                        {
                            //Verificamos que el CUIL sea valido
                            Servidor servidor = Servidor.getInstance();
                            string query = "'" + Int64.Parse(textBoxCuil.Text) + "', '" + Int64.Parse(textBoxDocumento.Text) + "'";
                            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.cuilEsValido_sp " + query);
                            while (reader.Read())
                            {
                                if (!bool.Parse(reader["valido"].ToString()))
                                {
                                    errores += "Ingrese un CUIL válido. \n";
                                }
                            }
                        }

                    }
            }
                if (errores != "")
                {
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
            if (this.verificarCampos()){
                Cliente clienteModificado = new Cliente();
                clienteModificado.Apellido = textBoxApellido.Text;
                clienteModificado.Nombre = textBoxNombre.Text;
                clienteModificado.Mail = textBoxMail.Text;
                clienteModificado.Telefono = long.Parse(textBoxTelefono.Text);
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
                //this.cerrarAnteriores();
                new SeleccionarFuncionalidad().Show();
                this.Close();

              
                //formAnt.Show();
            }
        }

        private void comboBoxDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxNombre_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxApellido_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxTelefono_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxMail_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxCuil_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxDocumento_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void dateTimePickerNacimiento_ValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void ModificarCli_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPiso_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxNumeroCalle_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            servidor.realizarQuery("exec habilitarUsuario_sp " + this.clienteViejo.IdUsuario);
            MessageBox.Show("El cliente fue habilitado", "Editar Cliente", MessageBoxButtons.OK);
            buttonD.Enabled = true;
            buttonH.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            servidor.realizarQuery("exec deshabilitarUsuario_sp " + this.clienteViejo.IdUsuario);
            MessageBox.Show("El cliente fue deshabilitado", "Editar Cliente", MessageBoxButtons.OK);
            buttonD.Enabled = false;
            buttonH.Enabled = true;
        }

        private void textBoxCalle_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxCiudad_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxLocalidad_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxDepto_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void textBoxCodigoPostal_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
