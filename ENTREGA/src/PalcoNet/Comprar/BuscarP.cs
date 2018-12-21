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
            //Primero se verifica si el usuario es un cliente, si no lo es se le muestra un mensaje de advertencia
            //Luego se traen los rubros existentes desde la base de datos.
            InitializeComponent();
            this.Cliente = sesion.traerCliente();

            if (this.Cliente == null) {
                MessageBox.Show("Se encuentra loggeado como " + sesion.rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad. " +
                   "Podrá simular el proceso de compra pero no comprar.", "Advertencia", MessageBoxButtons.OK);
            }

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
                       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        public bool verificarCampos() {
            //Verifica que al menos uno de los criterios de busqueda este completo y genera un mensaje de error si es necesario.
            string error = "";
            bool camposCompletos = !string.IsNullOrWhiteSpace(this.textBoxDescripcion.Text)
                || this.checkedListBoxCategorias.SelectedIndices.Count > 0
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
            //Adelanta el offset y realiza una buscqueda de las siguientes 10 publicaciones, en caso de no encontrar publicaciones
            // o de que el offset sea negativo se envia un mensaje de error al usuario y se corrige el valor del offset sumando o restando 
            // segun sea el caso.
            if (paraAdelante) { this.Offset++; } else { this.Offset--; }
            List<Publicacion> encontradas = new List<Publicacion>();

            if (this.Offset >= 0)
            {
                string descripcion = "";
                List<string> categoriasSelecc = new List<string>();
                DateTime? desde = null;
                DateTime? hasta = null;
                if (!string.IsNullOrWhiteSpace(this.textBoxDescripcion.Text)) { descripcion = this.textBoxDescripcion.Text; }
                if (this.checkedListBoxCategorias.SelectedIndices.Count > 0)
                {
                    for (int i = 0; i < checkedListBoxCategorias.Items.Count; i++)
                    {
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

                String query = (descripcion == "" ? "null" : "'" + descripcion + "' ") + ", " + (categorias == "" ? "null" : " '" + categorias + "' ")
                    + (checkBox1.Checked ? (", '" + desde.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "', '"
                    + hasta.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "', ") : ", null, null, ") + this.Offset * 10;
                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarPublicacionesPorCriterio_sp " + query);

                while (reader.Read())
                {
                    Publicacion publicacion = new Publicacion();
                    publicacion.Id = Convert.ToInt16(reader["id"]);
                    publicacion.Descripcion = reader["descripcion"].ToString();
                    publicacion.Direccion = reader["direccion"].ToString();
                    publicacion.Rubro = reader["rubro"].ToString();
                    publicacion.GradoDePublicacion = reader["grado"].ToString();
                    encontradas.Add(publicacion);
                }
                reader.Close();
            }
            if (encontradas.Count() == 0)
            {
                if (paraAdelante) { this.Offset--; } else { this.Offset++; }
                MessageBox.Show("No existen más resultados", "Advertencia", MessageBoxButtons.OK);
            }
            else
            {
                //las publicaciones encontradas se cargaran en el data grid para que el usuario pueda seleccionar la que desea

                this.Publicaciones = encontradas;
                var bindingList = new BindingList<Publicacion> (this.Publicaciones);
                var source = new BindingSource(bindingList, null);
                dataGridViewResultados.DataSource = source;
                if (this.Publicaciones.Count >= 1) { dataGridViewResultados.Rows[0].Selected = true; }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Cuando se realiza una busqueda nueva se limpia la tabla y si la verificacion de campos es correcta se lee
            this.limpiarTabla();
            if (this.verificarCampos()) {
                this.Offset = -1;
                this.leer(true); 
            }
        }

        private void limpiarTabla() {
            //Deja la tabla de resultados vacia
            this.Publicaciones.Clear();
            var bindingList = new BindingList<Publicacion>(this.Publicaciones);
            var source = new BindingSource(bindingList, null);
            dataGridViewResultados.DataSource = source;
        }

        private void dataGridViewResultados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Se lee una pagina para atras (bool paraAdelante = false)
            this.leer(false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Se lee una pagina para atras (bool paraAdelante = true)
            this.leer(true);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Verifica que se haya seleccionado una fila y genera una compra que le pasa a la siguiente pantalla para elegir la fecha
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
            //Limpia los criterios de busqueda y la tabla
            for (int ix = 0; ix < checkedListBoxCategorias.Items.Count; ++ix)
                checkedListBoxCategorias.SetItemChecked(ix, false);
            textBoxDescripcion.Text = "";
            checkBox1.Checked = false;
            dateTimePickerDesde.Value = DateTimePicker.MinimumDateTime;
            dateTimePickerHasta.Value = DateTimePicker.MinimumDateTime;
            dateTimePickerDesde.Enabled = false;
            dateTimePickerHasta.Enabled = false;
            this.limpiarTabla();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Verifica si se marco o desmarco el check box de filtrar por fecha y activa o desactiva los dateTimePickers
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
