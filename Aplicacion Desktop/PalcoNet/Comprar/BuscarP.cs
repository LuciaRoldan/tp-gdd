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
        List<string> categorias = new List<string>();
        Sesion sesion = Sesion.getInstance();
        Servidor servidor = Servidor.getInstance();
        int offset = -1;
        List<Publicacion> publicaciones = new List<Publicacion>();

        public List<Publicacion> Publicaciones
        {
          get { return publicaciones; }
          set { publicaciones = value; }
        }

        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

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
            this.Cliente = sesion.traerCliente();

            if (this.Cliente != null) {

                dateTimePickerDesde.Enabled = false;
                dateTimePickerHasta.Enabled = false;

                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.getRubros_sp");

                while (reader.Read())
                {
                    checkedListBoxCategorias.Items.Add(reader["descripcion"].ToString());
                    this.Categorias.Add(reader["descripcion"].ToString());
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
                // || ((this.dateTimePickerDesde.Value != null) && (this.dateTimePickerHasta.Value != null));
                || this.checkBox1.Checked;

            if (!camposCompletos) {
                error += "Se debe completar al menos un campo para realizar la búsqueda.";
            } else {
                if (this.checkBox1.Checked)
                {
                    if (Sesion.getInstance().fecha > dateTimePickerDesde.Value || dateTimePickerDesde.Value > dateTimePickerHasta.Value) { error += "Las fechas no son correctas."; }
                }
            }

            if (error != "")
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void leer(bool paraAdelante)
        {

            if (paraAdelante) { this.Offset++; } else { this.Offset--; }

            if (this.Offset >= 0)
            {
                this.Publicaciones.Clear();
                string descripcion = "";
                List<string> categoriasSelecc = new List<string>();
                DateTime? desde = null;
                DateTime? hasta = null;
                if (!string.IsNullOrWhiteSpace(this.textBoxDescripcion.Text)) { descripcion = this.textBoxDescripcion.Text; }
                if (this.checkedListBoxCategorias.SelectedIndices.Count > 0)
                {
                    for (int i = 0; i < checkedListBoxCategorias.Items.Count; i++)
                    {
                        Console.WriteLine(this.Categorias[i]);
                        if (checkedListBoxCategorias.GetItemCheckState(i) == CheckState.Checked) { categoriasSelecc.Add(this.Categorias[i]); }
                    }
                }
                if (this.checkBox1.Checked)
                {
                    desde = this.dateTimePickerDesde.Value;
                    hasta = this.dateTimePickerHasta.Value;
                }
                String categorias = "";
                foreach (String s in categoriasSelecc)
                {
                    if (categorias == "") { categorias = categorias + "''" + s + "''"; }
                    else { categorias = categorias + ", ''" + s + "''"; }
                }

                String query = (descripcion == "" ? "null" : "'" + descripcion + "' ") + ", " + (categorias == "" ? "null" : " '" + categorias + "' ") + (checkBox1.Checked ? (", '" + desde.GetValueOrDefault() + "', '" + hasta.GetValueOrDefault() + "', ") : ", null, null, ") + this.Offset * 10;

                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarPublicacionesPorCriterio_sp " + query);
                List<Publicacion> resultados = new List<Publicacion>();

                while (reader.Read())
                {
                    Publicacion publicacion = new Publicacion();
                    publicacion.Id = Convert.ToInt16(reader["id"]);
                    publicacion.Descripcion = reader["descripcion"].ToString();
                    publicacion.Direccion = reader["direccion"].ToString();
                    publicacion.Rubro = reader["rubro"].ToString();
                    resultados.Add(publicacion);
                    this.Publicaciones.Add(publicacion);
                }
                reader.Close();
            }
            if (this.Publicaciones.Count() == 0)
            {
                if (paraAdelante) { this.Offset--; } else { this.Offset++; }
                dataGridViewResultados.DataSource = new BindingSource(new BindingList<Publicacion>(), null);
                MessageBox.Show("No existen más resultados", "Advertencia", MessageBoxButtons.OK);
            }
            else
            {
                var bindingList = new BindingList<Publicacion> (this.Publicaciones);
                var source = new BindingSource(bindingList, null);
                dataGridViewResultados.DataSource = source;

                /*dataGridViewResultados.Columns[0].HeaderText = "Descripcion";
                dataGridViewResultados.Columns[0].DataPropertyName = "Descripcion";
                dataGridViewResultados.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewResultados.Columns[1].HeaderText = "Rubro";
                dataGridViewResultados.Columns[1].DataPropertyName = "Rubro";
                dataGridViewResultados.Columns[2].HeaderText = "Direccion";
                dataGridViewResultados.Columns[2].DataPropertyName = "Direccion";
                for (int i = 3; i < dataGridViewResultados.Columns.Count; i++) { dataGridViewResultados.Columns[i].Visible = false; }*/
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Hay que hacer la query aca y obtener la lista para pasarla abajo
            if (this.verificarCampos()) {
                this.Offset = -1;
                this.leer(true); 
            }
        }

        private void dataGridViewResultados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los anteriores elementos en la base y reemplazar el contenido de la lista a la que esta bindeada la tabla
            this.leer(false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los siguientes elementos en la base y reemplazar el contenido de la lista a la que esta bindeada la tabla
            this.leer(true);
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
                new Espectaculos(this, compra).Show();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dateTimePickerDesde.Enabled = true;
                dateTimePickerHasta.Enabled = true;
            }
            else
            {
                dateTimePickerDesde.Enabled = false;
                dateTimePickerHasta.Enabled = false;
            }
        }
    }
}
