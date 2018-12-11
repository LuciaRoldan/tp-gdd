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

namespace PalcoNet.Comprar
{
    public partial class MedioPago : MiForm
    {
        Compra compra;

        public Compra Compra
        {
            get { return compra; }
            set { compra = value; }
        }

        List<Tarjeta> tarjetas = new List<Tarjeta>();

        internal List<Tarjeta> Tarjetas
        {
            get { return tarjetas; }
            set { tarjetas = value; }
        }

        public MedioPago(MiForm anterior, Compra compra) : base(anterior)
        {
            if (Sesion.getInstance().rol.Nombre == "Cliente")
            {
                Cliente cliente = Sesion.getInstance().traerCliente();
                this.Compra = compra;
                InitializeComponent();

                //Aca hay que traer todas las tarjetas del cliente y guardarlas en la lista de arriba
                Servidor servidor = Servidor.getInstance();
                SqlDataReader reader = servidor.query("exec getMediosDePago_sp " + compra.Publicacion.Id);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Tarjeta tarjeta = new Tarjeta();
                        tarjeta.NumeroDeTarjeta = int.Parse(reader["digitos"].ToString());
                        if (tarjeta.NumeroDeTarjeta != 0)
                        {
                            comboBoxTarjeta.Items.Add("*******" + tarjeta.NumeroDeTarjeta);
                            this.tarjetas.Add(tarjeta);
                        }
                    }
                }
                else {
                    NuevoMP nuevo = new NuevoMP(this);
                    nuevo.Show();
                    if (nuevo.Tarjeta != null)
                    {
                        this.comboBoxTarjeta.Items.Add(nuevo.Tarjeta);
                        nuevo.Close();
                    }
                }
            }
            else {
                Tarjeta tarjeta = new Tarjeta();
                tarjeta.NumeroDeTarjeta = 0;
                this.Compra.MedioDePago = tarjeta;
                new FinalizarCompra(this, this.Compra).Show();
                this.Hide();
            }
        }

        public void actualizar(Tarjeta tarjeta) {
            Console.WriteLine("*********************************");
            this.comboBoxTarjeta.Items.Add("*******" + (tarjeta.NumeroDeTarjeta % 10000));
            this.Tarjetas.Add(tarjeta);
        }

        public bool verificarCampos() {
            string error = "";
            int i;
            bool camposCompletos = comboBoxTarjeta.SelectedIndex > -1 && !string.IsNullOrWhiteSpace(textBoxCodigo.Text);

            if (!camposCompletos){
                error += "Se debe seleccionar una tarjeta y proveer de un código de seguridad";
            } else {
                if (!int.TryParse(textBoxCodigo.Text, out i)) { error += "El código de seguridad debe ser un valor numérico."; }
            }

            if (error != "")
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos()) {
                int NumeroDeTarjeta = this.Tarjetas[this.comboBoxTarjeta.SelectedIndex].NumeroDeTarjeta;
                string codigoSeguridad = this.textBoxCodigo.Text;

                //Aca hay que verificar que la tarjeta exista en la base y que los datos coincidan
                if (/*verificacion es correcta*/true)
                {
                    this.Compra.MedioDePago = this.Tarjetas[this.comboBoxTarjeta.SelectedIndex];
                    new FinalizarCompra(this, this.Compra).Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Los datos ingresados para el medio de pago con invalidos.", "Error", MessageBoxButtons.OK);
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NuevoMP nuevo = new NuevoMP(this);
            nuevo.Show();
        }

        private void textBoxCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTarjeta_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
