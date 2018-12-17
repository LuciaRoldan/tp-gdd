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

namespace PalcoNet.Abm_Grado
{
    public partial class GradoDeLaPublicacion : MiForm
    {
        List<Publicacion> publicaciones = new List<Publicacion>();
        Publicacion pubSelecc;
        String gradoSelecc;
        Servidor servidor = Servidor.getInstance();
        Sesion sesion = Sesion.getInstance();

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

        public GradoDeLaPublicacion(MiForm anterior) : base(anterior)
        {
            InitializeComponent();
            if (sesion.esEmpresa()) {
                Empresa empresa = Sesion.getInstance().traerEmpresa();
                
                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.getPublicacionesDeUsuario_sp '" + sesion.usuario.NombreUsuario + "'");
                List<Publicacion> publicaciones = new List<Publicacion>();
                //19-67139304-09
                while (reader.Read())
                {
                    Publicacion publicacion = new Publicacion();
                    publicacion.Id = Convert.ToInt16(reader["id_publicacion"]);
                    publicacion.Descripcion = reader["descripcion"].ToString();
                    publicacion.GradoDePublicacion = reader["grado"].ToString();
                    checkedListBoxPublicaciones.Items.Add(publicacion.Descripcion);
                    this.Publicaciones.Add(publicacion);
                }
                reader.Close();
            } else {
                MessageBox.Show("Se encuentra loggeado como " + sesion.rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad.", "Advertencia", MessageBoxButtons.OK);
                buttonAceptar.Enabled = false;
            }

            
             
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.PubSelecc.GradoDePublicacion = this.GradoSelecc;
            //Aca hay que hacer un update en la base de la publicacion seleccionada
            //Estaria bueno que salga un cartelito de que salio todo ok

            servidor.realizarQuery("EXEC MATE_LAVADO.actualizarGradoPublicacion_sp '" + this.pubSelecc.Id + "', '" + this.pubSelecc.GradoDePublicacion + "'");
            MessageBox.Show("El grado de la publicacion se ha modificado con éxito", "Grado Publicación", MessageBoxButtons.OK);
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
