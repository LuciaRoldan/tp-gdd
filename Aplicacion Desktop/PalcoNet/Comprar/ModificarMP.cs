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

namespace PalcoNet.Comprar
{
    public partial class ModificarMP : MiForm
    {
        Servidor servidor = Servidor.getInstance();
        Tarjeta tarjetaVieja = new Tarjeta();
        MedioPago anterior;

        public MedioPago Anterior1
        {
            get { return anterior; }
            set { anterior = value; }
        }

        //se trae la tarjeta que ha sido seleccionada para modificar
        public ModificarMP(MedioPago anterior, Tarjeta tarjeta) : base(anterior)
        {
            InitializeComponent();

            tarjetaVieja = tarjeta;
            this.Anterior1 = anterior;

            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.getDatosTarjeta_sp " + tarjeta.Id);

            textBoxNumero.Text = "*******" + tarjetaVieja.NumeroDeTarjeta.ToString();
            Console.WriteLine("EL ID DE TARJETA ES: " + tarjetaVieja.Id);

            while (reader.Read())
            {
                Console.WriteLine("EL TITULAR ES: " + reader["titular"].ToString()); 
                textBox1.Text = reader["titular"].ToString();
                tarjeta.Titular = reader["titular"].ToString();
            }
            reader.Close();

        }

        private void textBoxNumero_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos())
            {
                Tarjeta tarjetaModificada = new Tarjeta();
                
                tarjetaModificada.NumeroDeTarjeta = long.Parse(textBoxNumero.Text);
                tarjetaModificada.Titular = textBox1.Text;

                String query = tarjetaVieja.Id + ", '" + tarjetaModificada.NumeroDeTarjeta + "', '" + tarjetaModificada.Titular + "'";

                servidor.realizarQuery("EXEC MATE_LAVADO.modificarTarjeta_sp " + query);

                this.Anterior1.actualizar(tarjetaModificada);

                MessageBox.Show("Los cambios se realizaron exitosamente.", "Modificar medio de pago", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            anterior.resetearComboBox();
            this.Anterior1.borrar(tarjetaVieja);
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public bool verificarCampos()
        {
            string error = "";
            long i;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxNumero.Text) && !string.IsNullOrWhiteSpace(textBox1.Text);

            if (!camposCompletos)
            {
                error += "Se debe ingresar un número de tarjeta y el nombre del titular";
            }
            else
            {
                if (!long.TryParse(textBoxNumero.Text, out i)) { error += "El número de tarjeta debe ser un valor numérico."; }
            }

            if (error != "")
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }
    }
}
