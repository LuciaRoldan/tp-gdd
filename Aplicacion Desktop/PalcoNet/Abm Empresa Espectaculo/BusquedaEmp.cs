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
using PalcoNet.Registro_de_Usuario;
using PalcoNet.Dominio;

namespace PalcoNet.Abm_Empresa_Espectaculo
{
    public partial class BusquedaEmp : MiForm
    {
        Servidor servidor = Servidor.getInstance();
        Sesion sesion = Sesion.getInstance();

        public BusquedaEmp(MiForm formAnterior) : base(formAnterior)
        {
            InitializeComponent();
        }

        //verificamos que la persona complete al menos un campo
        public bool verificarCampos(){
            string errores = "";
            long cuit;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBox1.Text)
                || !string.IsNullOrWhiteSpace(textBox2.Text)
                || !string.IsNullOrWhiteSpace(textBox4.Text);

            if (!camposCompletos) {
                errores += "Se debe completar al menos un campo para realizar la búsqueda.";
            } else {
                if (!string.IsNullOrWhiteSpace(textBox1.Text) ? !long.TryParse(textBox1.Text, out cuit) : false) { errores += "El CUIT debe ser un valor numérico."; }
            }

            if (errores != "") { 
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Aca agarramos los valores insertados por el usuario y se los pasamos al stored procedure que los buscará en la base
            if (this.verificarCampos())
            {
                Empresa empresa = new Empresa();
                if (!string.IsNullOrWhiteSpace(textBox1.Text)) { empresa.Cuit = Int64.Parse(textBox1.Text); }
                empresa.Mail = textBox2.Text;
                empresa.RazonSocial = textBox4.Text;
                List<Empresa> resultados = new List<Empresa>();

                String query = (empresa.Cuit == 0 ? "" : empresa.Cuit.ToString()) + "', '" + empresa.RazonSocial + "', '" + empresa.Mail + "'";

                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarEmpresaPorCriterio_sp '" + query );
                Console.WriteLine(query);
                while (reader.Read())
                {
                    Empresa empresaEnc = new Empresa();
                    empresaEnc.RazonSocial = reader["razon_social"].ToString();
                    empresaEnc.Mail = reader["mail"].ToString();
                    empresaEnc.Cuit = Convert.ToInt64(reader["cuit"]);
                    empresaEnc.FechaDeCreacion = (DateTime)reader["fecha_creacion"];
                    empresaEnc.Calle = reader["calle"].ToString();
                    empresaEnc.NumeroDeCalle = Convert.ToInt32(reader["numero_calle"]);
                    empresaEnc.Piso = Convert.ToInt32(reader["piso"]);
                    empresaEnc.Departamento = reader["depto"].ToString();

                    resultados.Add(empresaEnc);
                }
                reader.Close();

                var bindingList = new BindingList<Empresa>(resultados);
                var source = new BindingSource(bindingList, null);
                dataGridViewResultados.DataSource = source;

                //Aca hay que buscar en la base y obtener una lista de empresas que cumplan con los criterios de busqueda
                //y mostramos por pantalla los resultados de la busqueda

            }
        }

        //Este botón es para que el usuario pueda registrar una nueva empresa
        private void button1_Click(object sender, EventArgs e)
        { 
            Empresa cliente = new Empresa();
            new RegistroDeEmpresa(cliente, this).Show();
            this.Hide(); 
        }

        private void BusquedaEmp_Load(object sender, EventArgs e)
        {

        }

        //Si se quiere modificar la empresa encontrada se selecciona y se accede por este botón 
        private void buttonModificar_Click(object sender, EventArgs e)
        {
            Empresa empresa = (Empresa)dataGridViewResultados.CurrentRow.DataBoundItem;
            this.Hide();
            new ModificarEmp(empresa, this).Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
        }

        private void dataGridViewResultados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
