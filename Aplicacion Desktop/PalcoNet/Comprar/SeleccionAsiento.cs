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
        HashSet<Char> filas = new HashSet<Char>();
        List<Int32> asientos = new List<Int32>();
        Ubicaciones formUbicaciones;

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


        public SeleccionAsiento(Ubicaciones formUbicaciones, Ubicacion ubicacion, Compra compra)
        {
            InitializeComponent();

            this.compra = compra;
            this.ubicacion = ubicacion;
            this.formUbicaciones = formUbicaciones;

            Servidor servidor = Servidor.getInstance();
            //Aca hay que buscar en la base todas las filas que tienen asientos disponibles para la publicacion seleccionada

            //SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.filasDisponiblesSegunEspectaculo_sp " + this.compra.Espectaculo.Id + ", " + this.ubicacion.Precio);

            List<Char> lista = new List<Char>();
            Console.Write("una fila");
            Console.Write(formUbicaciones.AsientosDisponibles[0].Fila);
            lista = formUbicaciones.AsientosDisponibles.Select(a => a.Fila).ToList();
            this.filas = new HashSet<Char>(lista);
                        
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

            List<Asiento> asientosSeleccionados = formUbicaciones.AsientosDisponibles.Where(a => a.Fila == filaSeleccionada).ToList();
            List<Int32> numeroAsientosSeleccionados = asientosSeleccionados.Select(a => a.Asiento1).ToList();
            this.asientos = numeroAsientosSeleccionados;
            //Aca hay que buscar en la base todas los asientos disponibles para la fila seleccionada de la publicacion seleccionada

            //SqlDataReader reader = Servidor.getInstance().query("EXEC MATE_LAVADO.asientosDisponiblesSegunEspectaculoYFila_sp " + this.compra.Espectaculo.Id + ", '" + filaSeleccionada + "', " + this.ubicacion.Precio);


            foreach (Int32 a in numeroAsientosSeleccionados)
            {
                comboBoxAsiento.Items.Add(a);
                Console.Write(a.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBoxAsiento.SelectedIndex > -1 && comboBoxFila.SelectedIndex > -1){
                //this.Asiento = new Asiento();
                //this.Asiento.Fila = this.filas.ElementAt(comboBoxFila.SelectedIndex);
                //this.Asiento.Asiento1 = this.asientos.ElementAt(comboBoxAsiento.SelectedIndex);
                Console.Write("la fila selecc es");
                Console.Write(this.filas.ElementAt(comboBoxFila.SelectedIndex));
                Console.Write("el asiento selecc es");
                Console.Write(this.asientos.ElementAt(comboBoxAsiento.SelectedIndex).ToString());
                

                this.formUbicaciones.asientoSeleccionado(this.filas.ElementAt(comboBoxFila.SelectedIndex), this.asientos.ElementAt(comboBoxAsiento.SelectedIndex));
                
                this.Hide();
            } else {
                MessageBox.Show("Se debe seleccionar un asiento", "Error", MessageBoxButtons.OK);
            }
        }
    }
}
