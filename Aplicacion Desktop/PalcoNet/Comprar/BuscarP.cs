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
    public partial class BuscarP : MiForm
    {
        Cliente cliente;
        List<string> categorias;

        public List<string> Categorias
        {
            get { return categorias; }
            set { categorias = value; }
        }

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        public BuscarP(Cliente cliente, MiForm anterior) : base(anterior)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.cerrarAnteriores();
        }

        public bool verificarCampos() {
            return !string.IsNullOrWhiteSpace(this.textBoxDescripcion.Text) 
                || this.checkedListBoxCategorias.SelectedIndices.Count > 0
                || ((this.dateTimePickerDesde.Value != null) && (this.dateTimePickerHasta.Value != null));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Hay que hacer la query aca y obtener la lista para pasarla abajo
            if (this.verificarCampos()) {
                string descripcion = null; 
                List<string> categoriasSelecc = new List<string>();
                DateTime? desde = null;
                DateTime? hasta = null;
                if (!string.IsNullOrWhiteSpace(this.textBoxDescripcion.Text)) { descripcion = this.textBoxDescripcion.Text; }
                if (this.checkedListBoxCategorias.SelectedIndices.Count > 0) {
                    foreach (int index in checkedListBoxCategorias.SelectedIndices) { categoriasSelecc.Add(this.Categorias[index]); }
                }
                if (this.dateTimePickerDesde.Value != null && this.dateTimePickerHasta.Value != null) {
                    desde = this.dateTimePickerDesde.Value;
                    hasta = this.dateTimePickerHasta.Value;
                }

                //Aca hay que hacer la query y traer los resultados ordenados de los primeron no se cuantos por pagina

                List<Publicacion> resultados = new List<Publicacion>();

                if (!resultados.Any()){
                    MessageBox.Show("No se encontraron resultados.", "Error", MessageBoxButtons.OK);
                } else {
                    new Seleccion(descripcion, desde, hasta, categoriasSelecc, this).Show();
                    this.Hide();
                }
            }
        }
    }
}
