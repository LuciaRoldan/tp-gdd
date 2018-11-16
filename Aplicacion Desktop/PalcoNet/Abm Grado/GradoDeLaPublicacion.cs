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

namespace PalcoNet.Abm_Grado
{
    public partial class GradoDeLaPublicacion : MiForm
    {
        List<Publicacion> publicaciones = new List<Publicacion>();
        Publicacion pubSelecc;
        String gradoSelecc;

        public String GradoSelecc
        {
            get { return gradoSelecc; }
            set { gradoSelecc = value; }
        }

        internal Publicacion PubSelecc
        {
            get { return pubSelecc; }
            set { pubSelecc = value; }
        }

        internal List<Publicacion> Publicaciones
        {
            get { return publicaciones; }
            set { publicaciones = value; }
        }

        public GradoDeLaPublicacion(Empresa empresa, MiForm anterior) : base(anterior)
        {
            //Aca habria que buscar las publicaciones en la base y asegurarnos que tengan su grado
            //Hay que guardar las publicaciones de la empresa y guardarlas en publicaciones
            //Se puede cambiar el grado de cualquier publicacion o solo de las que no tienen uno seleccionado?

            InitializeComponent();
            Publicacion publicacion = new Publicacion();
            publicacion.Descripcion = "Mi grado es medio";
            publicacion.GradoDePublicacion = "Medio";
            Publicaciones.Add(publicacion);
            Publicacion publicacion2 = new Publicacion();
            publicacion2.Descripcion = "Mi grado es bajo";
            publicacion2.GradoDePublicacion = "Bajo";
            Publicaciones.Add(publicacion2);
            foreach(Publicacion pub in this.Publicaciones){
                checkedListBoxPublicaciones.Items.Add(pub.Descripcion);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new SeleccionarFuncionalidad().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.PubSelecc.GradoDePublicacion = this.GradoSelecc;
            //Aca hay que hacer un update en la base de la publicacion seleccionada
            Console.WriteLine(this.GradoSelecc + this.PubSelecc.Descripcion);

        }

        private void checkedListBoxPublicaciones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                for (int ix = 0; ix < checkedListBoxPublicaciones.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBoxPublicaciones.SetItemChecked(ix, false);
        }

        private void checkedListBoxPublicaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBoxGrado.Enabled = true;

            if (checkedListBoxPublicaciones.CheckedItems.Count > 1)
            {
                Int32 checkedItemIndex = checkedListBoxPublicaciones.CheckedIndices[0];
                checkedListBoxPublicaciones.ItemCheck -= checkedListBoxPublicaciones_ItemCheck;
                checkedListBoxPublicaciones.SetItemChecked(checkedItemIndex, false);
                checkedListBoxPublicaciones.ItemCheck += checkedListBoxPublicaciones_ItemCheck;
            }

            int[] indexes = checkedListBoxPublicaciones.CheckedIndices.Cast<int>().ToArray();
            this.PubSelecc = this.Publicaciones[indexes[0]];
            string grado = this.PubSelecc.GradoDePublicacion;
            switch(grado){
                case "Alto":
                    this.GradoSelecc = "Alto";
                    checkedListBoxGrado.SetItemChecked(1, false);
                    checkedListBoxGrado.SetItemChecked(2, false);
                    checkedListBoxGrado.SetItemChecked(0, true);
                    break;
                case "Medio":
                    this.GradoSelecc = "Medio";
                    checkedListBoxGrado.SetItemChecked(1, true);
                    checkedListBoxGrado.SetItemChecked(0, false);
                    checkedListBoxGrado.SetItemChecked(2, false);
                    break;
                case "Bajo":
                    this.GradoSelecc = "Bajo";
                    checkedListBoxGrado.SetItemChecked(2, true);
                    checkedListBoxGrado.SetItemChecked(1, false);
                    checkedListBoxGrado.SetItemChecked(0, false);
                    break;
                default:
                    break;
            }

        }

        private void checkedListBoxGrado_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                for (int ix = 0; ix < checkedListBoxGrado.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBoxGrado.SetItemChecked(ix, false);
        }

        private void checkedListBoxGrado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBoxGrado.CheckedItems.Count > 1)
            {
                Int32 checkedItemIndex = checkedListBoxGrado.CheckedIndices[0];
                checkedListBoxGrado.ItemCheck -= checkedListBoxGrado_ItemCheck;
                checkedListBoxGrado.SetItemChecked(checkedItemIndex, false);
                checkedListBoxGrado.ItemCheck += checkedListBoxGrado_ItemCheck;
            }

            int[] indexes = checkedListBoxGrado.CheckedIndices.Cast<int>().ToArray();
            Console.WriteLine(indexes[0]);
            switch (indexes[0]) { 
                case 0:
                    this.GradoSelecc = "Alto";
                    break;
                case 1:
                    this.GradoSelecc = "Medio";
                    break;
                case 2:
                    this.GradoSelecc = "Bajo";
                    break;
            }
        }
    }
}
