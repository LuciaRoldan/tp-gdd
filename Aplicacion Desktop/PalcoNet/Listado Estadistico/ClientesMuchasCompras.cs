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
using PalcoNet.Dominio;

namespace PalcoNet.Listado_Estadistico
{
    public partial class ClientesMuchasCompras : MiForm
    {
        Servidor servidor = Servidor.getInstance();
        DateTime empieza;
        DateTime termina;

        public ClientesMuchasCompras(DateTime inicio, DateTime fin, MiForm formAnterior) : base(formAnterior)
        {
            InitializeComponent();
            empieza = inicio;
            termina = fin;

            //Se traen todas las razones sociales de las empresas existentes y se las agrega al combobox
            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.traerTodasRazonesSociales_sp");

            while (reader.Read())
            {
                Empresa empresa = new Empresa();
                empresa.RazonSocial = reader["razon_social"].ToString();
                comboBox1.Items.Add(empresa.RazonSocial);

            }
            reader.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Se busca en la base los clientes y se los muestra en la tabla
            if (comboBox1.SelectedIndex > -1)
            {
                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.top5ClienteComprasParaUnaEmpresa_sp '" + comboBox1.Text.ToString() + "', '" + empieza.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + termina.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
                List<CompraCliente> comprasClientes = new List<CompraCliente>();

                while (reader.Read())
                {
                    Console.WriteLine("LLEGA HASTA ACA");
                    CompraCliente compra = new CompraCliente();
                    compra.Nombre = reader["nombre"].ToString();
                    compra.Apellido = reader["apellido"].ToString();
                    compra.Empresa = reader["razon_social"].ToString();
                    compra.CantidadCompras = Convert.ToInt32(reader["Cantidad de compras"]);

                    comprasClientes.Add(compra);

                }
                reader.Close();

                var bindingList = new BindingList<CompraCliente>(comprasClientes);
                var source = new BindingSource(bindingList, null);
                clientesComprasGrid.DataSource = source;
            }
        }

        private void ClientesMuchasCompras_Load(object sender, EventArgs e)
        {

        }
    }
}
