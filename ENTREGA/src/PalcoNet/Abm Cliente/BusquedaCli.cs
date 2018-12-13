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
using PalcoNet.Registro_de_Usuario;
using PalcoNet.Dominio;

namespace PalcoNet.Abm_Cliente
{
    public partial class BusquedaCli : MiForm
    {

        public BusquedaCli(MiForm formAnterior)
            : base(formAnterior)
        {
            InitializeComponent();

        }

        Servidor servidor = Servidor.getInstance();
        List<Cliente> clientesEncontrados = new List<Cliente>();

        //verificamos que la persona complete al menos un campo
        public bool verificarCampos()
        {
            string errores = "";
            int dni;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxNombre.Text)
                || !string.IsNullOrWhiteSpace(textBoxApellido.Text)
                || !string.IsNullOrWhiteSpace(textBoxDni.Text)
                || !string.IsNullOrWhiteSpace(textBoxMail.Text);

            if (!camposCompletos)
            {
                errores += "Se debe completar al menos un campo para realizar la búsqueda.";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(textBoxDni.Text) ? !int.TryParse(textBoxDni.Text, out dni) : false) { errores += "El DNI debe ser un valor numérico."; }
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
            this.cerrarAnteriores();
        }

        //Este botón es para que el usuario pueda registrar un nuevo cliente
        private void button1_Click(object sender, EventArgs e)
        {
            Cliente cliente = new Cliente();
            new RegistroDeCliente(cliente, this).Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Aca agarramos los valores insertados por el usuario y se los pasamos al stored procedure que los buscará en la base
            if (this.verificarCampos())
            {
                Cliente cliente = new Cliente();
                cliente.Nombre = textBoxNombre.Text;
                cliente.Apellido = textBoxApellido.Text;
                cliente.Mail = textBoxMail.Text;
                if (!string.IsNullOrWhiteSpace(textBoxDni.Text)) { cliente.NumeroDeDocumento = Int32.Parse(textBoxDni.Text); }
                List<Cliente> resultados = new List<Cliente>();

                String query = cliente.Nombre + "', '" + cliente.Apellido + "', " + (cliente.NumeroDeDocumento == 0 ? "''" : cliente.NumeroDeDocumento.ToString()) +
                                ", '" + cliente.Mail + "'";

                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarUsuarioPorCriterio_sp '" + query);
                Console.WriteLine(query);
                while (reader.Read())
                {
                    Cliente clienteEnc = new Cliente();
                    clienteEnc.Nombre = reader["nombre"].ToString();
                    clienteEnc.Apellido = reader["apellido"].ToString();
                    Console.WriteLine(reader["nombre"]);
                    clienteEnc.Cuil = long.Parse(reader["cuil"].ToString());
                    clienteEnc.Mail = reader["mail"].ToString();
                    clienteEnc.Telefono = long.Parse(reader["telefono"].ToString());
                    clienteEnc.NumeroDeDocumento = Convert.ToInt32(reader["documento"]);
                    clienteEnc.TipoDocumento = reader["tipo_documento"].ToString();
                    clienteEnc.Calle = reader["calle"].ToString();
                    clienteEnc.NumeroDeCalle = Convert.ToInt32(reader["numero_calle"]);
                    clienteEnc.FechaDeNacimiento = (DateTime)reader["fecha_nacimiento"];
                    clienteEnc.FechaDeCreacion = (DateTime)reader["fecha_creacion"];
                    clienteEnc.CodigoPostal = reader["codigo_postal"].ToString();
                    clienteEnc.Id = int.Parse(reader["id_cliente"].ToString());
                    clienteEnc.Departamento = reader["depto"].ToString();

                    clientesEncontrados.Add(clienteEnc);
                    resultados.Add(clienteEnc);

                }
                reader.Close();

                var bindingList = new BindingList<Cliente>(resultados);
                var source = new BindingSource(bindingList, null);
                dataGridResultados.DataSource = source;
                //Aca hay que buscar en la base y obtener una lista de clientes que cumplan con los criterios de busqueda
                //y mostramos por pantalla los resultados de la busqueda

            }
        }

    //Si se quiere modificar el cliente encontrado se selecciona y se accede por este botón 
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

        private void dataGridResultados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}