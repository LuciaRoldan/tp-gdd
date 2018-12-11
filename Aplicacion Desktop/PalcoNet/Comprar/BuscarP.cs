using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        Sesion sesion = Sesion.getInstance();
        Servidor servidor = Servidor.getInstance();

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

        public BuscarP(MiForm anterior) : base(anterior)
        {
            InitializeComponent();
            if (sesion.rol.Nombre == "Cliente") {

                this.Cliente = sesion.traerCliente();

                SqlDataReader reader = servidor.query("EXEC dbo.getRubros_sp");

                while (reader.Read())
                {
                    checkedListBoxCategorias.Items.Add(reader["descripcion"].ToString());
                }
                reader.Close();

                dataGridViewResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            } else {
                MessageBox.Show("Se encuentra loggeado como " + sesion.rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad." + 
                    "Podrá simular el proceso de compra pero no comprar.", "Advertencia", MessageBoxButtons.OK);
            }
                       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        public bool verificarCampos() {
            string error = "";
            bool camposCompletos = !string.IsNullOrWhiteSpace(this.textBoxDescripcion.Text) 
                || this.checkedListBoxCategorias.SelectedIndices.Count > 0
                || ((this.dateTimePickerDesde.Value != null) && (this.dateTimePickerHasta.Value != null));

            if (!camposCompletos) {
                error += "Se debe completar al menos un campo para realizar la búsqueda.";
            } else {
                if (Sesion.getInstance().fecha > dateTimePickerDesde.Value || dateTimePickerDesde.Value > dateTimePickerHasta.Value) { error += "Las fechas no son correctas."; }
            }

            if (error != "")
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Hay que hacer la query aca y obtener la lista para pasarla abajo
            if (this.verificarCampos()) {
                string descripcion = ""; 
                List<string> categoriasSelecc = new List<string>();
                DateTime? desde = null;
                DateTime? hasta = null;
                if (!string.IsNullOrWhiteSpace(this.textBoxDescripcion.Text)) { descripcion = this.textBoxDescripcion.Text; }
                if (this.checkedListBoxCategorias.SelectedIndices.Count > 0) {
                    for (int i = 0; i < checkedListBoxCategorias.Items.Count; i++)
                    {
                        if (checkedListBoxCategorias.GetItemCheckState(i) == CheckState.Checked) { categoriasSelecc.Add(this.Categorias[i]); }
                    }
                }
                if (this.dateTimePickerDesde.Value != null && this.dateTimePickerHasta.Value != null) {
                    desde = this.dateTimePickerDesde.Value;
                    hasta = this.dateTimePickerHasta.Value;
                }
                String categorias = "";
                foreach (String s in categoriasSelecc)
                {
                    categorias = categorias + "', '" + s + "'";
                }

                String query = (descripcion == "" ? "null" : "'" + descripcion + "' ") + ", " + (categorias == "" ? "null" : " '" + categorias + "' ") + ", '" + desde + "', '" + hasta + "'";

               
                SqlDataReader reader = servidor.query("EXEC dbo.buscarPublicacionesPorCriterio_sp " + query);
                List<Publicacion> resultados = new List<Publicacion>();


                while (reader.Read())
                {
                    Publicacion publicacion = new Publicacion();
                    publicacion.Id = Convert.ToInt16(reader["id"]);
                    publicacion.Descripcion = reader["descripcion"].ToString();
                    publicacion.Direccion = reader["direccion"].ToString();
                    publicacion.Rubro = reader["rubro"].ToString();
                    resultados.Add(publicacion);
                }
                reader.Close();

                //Aca hay que hacer la query y traer los resultados ordenados de los primeron no se cuantos por pagina

                if (!resultados.Any()){
                    MessageBox.Show("No se encontraron resultados.", "Error", MessageBoxButtons.OK);
                } else {
                    //Mantener numero de pagina, no se como sera mejor manejarlo
                    

                    //dataGridViewResultados.AutoGenerateColumns = false;
                    
                    var bindingList = new BindingList<Publicacion>(resultados);
                    var source = new BindingSource(bindingList, null);
                    dataGridViewResultados.DataSource = source;

                    dataGridViewResultados.Columns[0].HeaderText = "Descripcion";
                    dataGridViewResultados.Columns[0].DataPropertyName = "Descripcion";
                    dataGridViewResultados.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewResultados.Columns[1].HeaderText = "Rubro";
                    dataGridViewResultados.Columns[1].DataPropertyName = "Rubro";
                    dataGridViewResultados.Columns[2].HeaderText = "Direccion";
                    dataGridViewResultados.Columns[2].DataPropertyName = "Direccion";
                    for (int i = 3; i < dataGridViewResultados.Columns.Count; i++) { dataGridViewResultados.Columns[i].Visible = false; }
                }
            }
        }

        private void dataGridViewResultados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los anteriores elementos en la base y reemplazar el contenido de la lista a la que esta bindeada la tabla
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los siguientes elementos en la base y reemplazar el contenido de la lista a la que esta bindeada la tabla
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridViewResultados.SelectedRows.Count > 0)
            {
                Publicacion publicacionSeleccionada = (Publicacion)dataGridViewResultados.CurrentRow.DataBoundItem;
                Compra compra = new Compra();
                compra.Publicacion = publicacionSeleccionada;
                new Espectaculo(this, compra).Show();
                this.Hide();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int ix = 0; ix < checkedListBoxCategorias.Items.Count; ++ix)
                checkedListBoxCategorias.SetItemChecked(ix, false);
            textBoxDescripcion.Text = "";
            dateTimePickerDesde.Value = DateTimePicker.MinimumDateTime;
            dateTimePickerHasta.Value = DateTimePicker.MinimumDateTime;
        }
    }
}
