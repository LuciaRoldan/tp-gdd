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
        Tarjeta tarjetaSeleccionada = new Tarjeta();

        internal List<Tarjeta> Tarjetas
        {
            get { return tarjetas; }
            set { tarjetas = value; }
        }

        public MedioPago(MiForm anterior, Compra compra) : base(anterior)
        {
            this.Compra = compra;
            InitializeComponent();
            Cliente cliente = Sesion.getInstance().traerCliente();
            
            if (cliente != null)
            {
                //Aca traemos todas las tarjetas del cliente y las guardamos en la lista para que el usuario pueda seleccionar la que desee
                this.updateMP();              
            }
            else 
            {
                //si no hay cliente no le permite ingresar una nueva tarjeta
                this.button3.Enabled = false;
                Tarjeta tarjeta = new Tarjeta();
                tarjeta.NumeroDeTarjeta = 0;
                tarjeta.Id = 0;
                comboBoxTarjeta.Items.Add("*******");
                this.tarjetas.Add(tarjeta);
            }
        }

        //actualiza la lista de tarjetas despues de agregar una o modificarla
        public void actualizar(Tarjeta tarjeta) {
            this.comboBoxTarjeta.Items.Add(tarjeta.NumeroDeTarjeta);
            this.Tarjetas.Add(tarjeta);
        }

        //función para borrar una tarjeta de la lista porque ha sido modificada
        public void borrar(Tarjeta tarjeta)
        {
            this.tarjetas.Remove(tarjeta);
        }

        //Función para validar que los campos esten completos y tengan el tipo que corresponda
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

        //Luego de verificar los campos, guardamos el medio de pago seleccionado y vamos a la proxima pantalla de finalizar la compra
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos()) {
                    
                long NumeroDeTarjeta = this.Tarjetas[this.comboBoxTarjeta.SelectedIndex].NumeroDeTarjeta;
                string codigoSeguridad = this.textBoxCodigo.Text;

                //La verificacion del codigo de seguridad no la hacemos ya que se haría con algún servicio porque no nos
                //parece correcto guardar esa información en la base
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

        internal void resetearComboBox()
        {
            comboBoxTarjeta.ResetText();
        }


        internal void updateMP()
        {
            tarjetas.Clear();
            
            Servidor servidor = Servidor.getInstance();
            SqlDataReader reader = servidor.query("exec MATE_LAVADO.getMediosDePago_sp " + Sesion.getInstance().traerCliente().Id);
            
            while (reader.Read())
            {
                Tarjeta tarjeta = new Tarjeta();
                tarjeta.NumeroDeTarjeta = long.Parse(reader["digitos"].ToString());
                tarjeta.Id = int.Parse(reader["id_medio_de_pago"].ToString());
                if (tarjeta.NumeroDeTarjeta != 0)
                {
                    comboBoxTarjeta.Items.Add(tarjeta.NumeroDeTarjeta);
                    this.tarjetas.Add(tarjeta);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }
    }
}
