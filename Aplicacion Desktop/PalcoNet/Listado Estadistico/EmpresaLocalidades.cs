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

namespace PalcoNet.Listado_Estadistico
{
    public partial class EmpresaLocalidades : MiForm
    {

        DateTime fechaInicio;
        DateTime fechaFin;
        Servidor servidor = Servidor.getInstance();

        public EmpresaLocalidades(DateTime inicio, DateTime fin, MiForm formAnterior) : base(formAnterior) 
        {
            InitializeComponent();
            fechaInicio = inicio;
            fechaFin = fin;
            comboBox1.Text = "Bajo";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Busca las empresas segun el grado que se seleccione y las mustra en la tabla

            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.top5EmpresasLocalidadesNoVendidas_sp '" + comboBox1.Text.ToString() + "', '" + this.fechaInicio.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + this.fechaFin.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
            List<CantidadLocalidadesEmpresa> localidadesEmpresa = new List<CantidadLocalidadesEmpresa>();
            Console.WriteLine("EXEC MATE_LAVADO.top5EmpresasLocalidadesNoVendidas_sp '" + comboBox1.Text.ToString() + "', '" + this.fechaInicio.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + this.fechaFin.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
            while (reader.Read())
            {
                CantidadLocalidadesEmpresa localidad = new CantidadLocalidadesEmpresa();
                localidad.Empresa = reader["Razon social"].ToString();
                localidad.Cuit = Int64.Parse(reader["cuit"].ToString());
                localidad.LocalidadesNoVendidas = Convert.ToInt32(reader["Ubicaciones no vendidas"]);

                localidadesEmpresa.Add(localidad);
            }
            reader.Close();

            var bindingList = new BindingList<CantidadLocalidadesEmpresa>(localidadesEmpresa);
            var source = new BindingSource(bindingList, null);
            localidadesEmpresaGrid.DataSource = source;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide(); 
        }

        private void localidadesEmpresaGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
