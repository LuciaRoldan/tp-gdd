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
    public partial class SeleccionAsiento : Form
    {
        Ubicacion ubicacion;
        Asiento asiento;
        Compra compra;
        List<char> filas = new List<char>();
        List<int> asientos = new List<int>();

        public Compra Compra
        {
            get { return compra; }
            set { compra = value; }
        }

        internal Asiento Asiento
        {
            get { return asiento; }
            set { asiento = value; }
        }

        public Ubicacion Ubicacion
        {
            get { return ubicacion; }
            set { ubicacion = value; }
        }


        public SeleccionAsiento(Ubicacion ubicacion, Compra compra)
        {
            InitializeComponent();

            this.compra = compra;
            this.ubicacion = ubicacion;

            Servidor servidor = Servidor.getInstance();
            //Aca hay que buscar en la base todas las filas que tienen asientos disponibles para la publicacion seleccionada

            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.filasDisponiblesSegunEspectaculo_sp " + this.compra.Espectaculo.Id + ", " + this.ubicacion.Precio);

            while (reader.Read())
            {
                this.filas.Add(Convert.ToChar(reader["fila"]));
            }
            reader.Close();
                        
            foreach (char f in filas) {
                comboBoxFila.Items.Add(f);
            }
        }

        private void comboBoxFila_SelectedIndexChanged(object sender, EventArgs e)
        {
            char filaSeleccionada = this.filas.ElementAt(comboBoxFila.SelectedIndex);
            this.asientos.Clear();
            comboBoxAsiento.Items.Clear();
            comboBoxAsiento.ResetText();
            //Aca hay que buscar en la base todas los asientos disponibles para la fila seleccionada de la publicacion seleccionada

            SqlDataReader reader = Servidor.getInstance().query("EXEC MATE_LAVADO.asientosDisponiblesSegunEspectaculoYFila_sp " + this.compra.Espectaculo.Id + ", '" + filaSeleccionada + "', " + this.ubicacion.Precio);

            while (reader.Read())
            {
                this.asientos.Add(Convert.ToInt32(reader["asiento"]));
            }
            reader.Close();

            foreach (int a in asientos)
            {
                comboBoxAsiento.Items.Add(a);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBoxAsiento.SelectedIndex > -1 && comboBoxFila.SelectedIndex > -1){
                this.Asiento = new Asiento();
                this.Asiento.Fila = this.filas.ElementAt(comboBoxFila.SelectedIndex);
                this.Asiento.Asiento1 = this.asientos.ElementAt(comboBoxAsiento.SelectedIndex);
                this.Hide();
            } else {
                MessageBox.Show("Se debe seleccionar un asiento", "Error", MessageBoxButtons.OK);
            }
        }
    }
}
