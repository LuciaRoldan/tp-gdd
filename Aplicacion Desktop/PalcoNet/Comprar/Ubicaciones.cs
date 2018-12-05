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

namespace PalcoNet.Comprar
{
    public partial class Ubicaciones : MiForm
    {
        Compra compra;

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

            foreach (Ubicacion u in this.UbicacionesDisponibles)
            {
                this.checkedListBoxUbicaciones.Items.Add(u);
            }
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
            new MedioPago().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que ir gurdando una lista de todas las entradas
            //Tambien podria salir un cartelito de que las cosas salieron bien

            if (this.numericUpDownCantidad.Value > 0 && this.checkedListBoxUbicaciones.SelectedIndices.Count > 0) {
                Ubicacion ubicacion = new Ubicacion();
                ubicacion.TipoAsiento = this.UbicacionesDisponibles[this.checkedListBoxUbicaciones.SelectedIndices[0]].TipoAsiento;
                ubicacion.Precio = this.UbicacionesDisponibles[this.checkedListBoxUbicaciones.SelectedIndices[0]].Precio;
                ubicacion.Numerada = this.UbicacionesDisponibles[this.checkedListBoxUbicaciones.SelectedIndices[0]].Numerada;
                if (ubicacion.Numerada)
                { 
                    //Alguna idea de como mostrar los asientos disponibles para elegir fila y asiento???????
                }
            }

        }

        private void checkedListBoxUbicaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBoxUbicaciones.CheckedItems.Count > 1)
            {
                Int32 checkedItemIndex = checkedListBoxUbicaciones.CheckedIndices[0];
                checkedListBoxUbicaciones.ItemCheck -= checkedListBoxUbicaciones_ItemCheck;
                checkedListBoxUbicaciones.SetItemChecked(checkedItemIndex, false);
                checkedListBoxUbicaciones.ItemCheck += checkedListBoxUbicaciones_ItemCheck;
            }
            this.numericUpDownCantidad.Value = 0;
        }

         private void checkedListBoxUbicaciones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                for (int ix = 0; ix < checkedListBoxUbicaciones.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBoxUbicaciones.SetItemChecked(ix, false);
        }
            
    }
}
