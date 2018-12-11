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
    public partial class Ubicaciones : MiForm
    {
        Compra compra;
        Servidor servidor = Servidor.getInstance();
        Sesion sesion = Sesion.getInstance();

        internal Compra Compra
        {
            get { return compra; }
            set { compra = value; }
        }

        List<Ubicacion> ubicacionesDisponibles = new List<Ubicacion>();

        internal List<Ubicacion> UbicacionesDisponibles
        {
            get { return ubicacionesDisponibles; }
            set { ubicacionesDisponibles = value; }
        }

        public Ubicaciones(Compra compra, MiForm anterior) : base(anterior)
        {
            this.Compra = compra;
            InitializeComponent();

            //Aca hay que traer de la base una lista de las ubicaciones disponibles de this.Compra.Publicacion y guardarlo en ubicacionesDisponibles

            SqlDataReader reader = servidor.query("EXEC dbo.buscarUbicacionessPorPublicacion_sp " + compra.Publicacion.Id);

            while (reader.Read())
            {
                Ubicacion u = new Ubicacion();
                u.Numerada = bool.Parse(reader["sin_numerar"].ToString());
                u.CantidadAsientos = int.Parse(reader["asientos"].ToString());
                if (u.Numerada) { u.CantidadFilas = int.Parse(reader["filas"].ToString()); }
                u.Precio = decimal.Parse(reader["precio"].ToString());
                u.Id = int.Parse(reader["id_ubicacion"].ToString());
                u.TipoAsiento = reader["descripcion"].ToString();
                comboBoxUbicaciones.Items.Add(u.TipoAsiento + ", $" + u.Precio.ToString());
                this.UbicacionesDisponibles.Add(u);
            }
            reader.Close();

            foreach (Ubicacion u in this.UbicacionesDisponibles)
            {
                comboBoxUbicaciones.Items.Add(u.TipoAsiento);
            }
            this.Compra.Ubicaciones = new List<Ubicacion>();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new MedioPago(this, this.Compra).Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que ir gurdando una lista de todas las entradas
            //Tambien podria salir un cartelito de que las cosas salieron bien

            if (this.numericUpDownCantidad.Value > 0 && this.comboBoxUbicaciones.SelectedIndex > -1)
            {
                Ubicacion ubicacion = new Ubicacion();
                ubicacion.TipoAsiento = this.UbicacionesDisponibles[this.comboBoxUbicaciones.SelectedIndex].TipoAsiento;
                ubicacion.Precio = this.UbicacionesDisponibles[this.comboBoxUbicaciones.SelectedIndex].Precio;
                ubicacion.Numerada = this.UbicacionesDisponibles[this.comboBoxUbicaciones.SelectedIndex].Numerada;
                ubicacion.Id = this.UbicacionesDisponibles[this.comboBoxUbicaciones.SelectedIndex].Id;
                ubicacion.CantidadAsientos = (int) this.numericUpDownCantidad.Value;
                if (ubicacion.Numerada)
                {
                    List<Asiento> asientos = new List<Asiento>();
                    for (int i = 0; i < this.numericUpDownCantidad.Value; i++)
                    {
                        SeleccionAsiento seleccion = new SeleccionAsiento(ubicacion, compra);
                        seleccion.Show();
                        asientos.Add(seleccion.Asiento);
                        seleccion.Close();
                    }
                    ubicacion.Asientos = asientos;
                }
                this.Compra.Ubicaciones.Add(ubicacion);
            }

        }

        private void checkedListBoxUbicaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

         private void checkedListBoxUbicaciones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

         private void numericUpDownCantidad_ValueChanged(object sender, EventArgs e)
         {
             if (this.comboBoxUbicaciones.SelectedIndex > -1)
             {
                 if (numericUpDownCantidad.Value > this.UbicacionesDisponibles[this.comboBoxUbicaciones.SelectedIndex].CantidadAsientos) { numericUpDownCantidad.Value--; }
             }
         }

         private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
         {

         }
            
    }
}
