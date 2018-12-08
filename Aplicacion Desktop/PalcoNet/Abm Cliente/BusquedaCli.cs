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
        
        public BusquedaCli(MiForm formAnterior) : base(formAnterior)
           
        {
            InitializeComponent();
            
        }

        Servidor servidor = Servidor.getInstance();
        List<Cliente> clientesEncontrados = new List<Cliente>();

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
                if (!int.TryParse(textBoxDni.Text, out dni) && textBoxDni.Text != " ") { errores += "El DNI debe ser un valor numérico."; }
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
                cliente.Mail = textBoxMail.Text;
                cliente.NumeroDeDocumento = Int32.Parse(textBoxDni.Text);
                List<Cliente> resultados = new List<Cliente>();

                String query = cliente.Nombre + "', '" + cliente.Apellido + "', '" + cliente.NumeroDeDocumento + 
                                "', '" + cliente.Mail + "'";

                SqlDataReader reader = servidor.query("EXEC dbo.buscarUsuarioPorCriterio_sp '" + query );
                
                var bindingList = new BindingList<Cliente>(resultados);
                var source = new BindingSource(bindingList, null);
                dataGridResultados.DataSource = source;

                while (reader.Read())
                {
                    Cliente clienteEnc = new Cliente();
                    cliente.Nombre = reader["nombre"].ToString();
                    cliente.Apellido = reader["apellido"].ToString();
                   // cliente.Cuil = Convert.ToInt32(reader["cuil"]); NO SE QUE HACER SI SON NULOS
                    cliente.Mail = reader["mail"].ToString();
                    // cliente.Telefono = Convert.ToInt32(reader["telefono"]); NO SE QUE HACER SI SON NULOS
                    cliente.NumeroDeDocumento = Convert.ToInt32(reader["documento"]);
                    cliente.TipoDocumento = reader["tipo_documento"].ToString();
                    cliente.Calle = reader["calle"].ToString();
                    cliente.NumeroDeCalle = Convert.ToInt32(reader["numero_calle"]);
                    cliente.Piso = Convert.ToInt16(reader["piso"]);
                    cliente.CodigoPostal = reader["codigo_postal"].ToString();
                    cliente.NombreUsuario = reader["username"].ToString();
                    cliente.Contrasenia = reader["password"].ToString();
                    cliente.FechaDeNacimiento = (DateTime) reader["fecha_nacimiento"];
                    cliente.FechaDeCreacion = (DateTime) reader["fecha_creacion"];

                    clientesEncontrados.Add(clienteEnc);
                    bindingList.Add(clienteEnc);

  /*                  dataGridResultados.Rows.Add(cliente.Nombre, cliente.Apellido, cliente.Cuil, cliente.Mail);
                    dataGridResultados.Rows.Add(reader["nombre"].ToString(), reader["apellido"], reader["cuil"].ToString(),
                        reader["mail"].ToString(), reader["telefono"].ToString(), reader["documento"].ToString(), 
                        reader["tipo_documento"].ToString(), reader["calle"].ToString(), reader["numero_calle"].ToString(),
                        reader["piso"].ToString(), reader["codigo_postal"].ToString(), reader["username"].ToString(),
                        reader["password"].ToString(), reader["fecha_nacimiento"].ToString(), reader["fecha_creacion"].ToString());
  */                   
                }
                reader.Close();
                //Aca hay que buscar en la base y obtener una lista de clientes que cumplan con los criterios de busqueda

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

        private void dataGridResultados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
