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

        //se trae la tarjeta que ha sido seleccionada para modificar
        public ModificarMP(MiForm anterior, Tarjeta tarjeta) : base(anterior)
        {
            InitializeComponent();

            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.getDatosTarjeta_sp " + tarjeta.NumeroDeTarjeta);

            textBoxNumero.Text = "*******" + tarjeta.NumeroDeTarjeta.ToString();
            tarjetaVieja = tarjeta;

            while (reader.Read())
            {
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
                MessageBox.Show("Los cambios se realizaron exitosamente.", "Modificar cliente", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

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
