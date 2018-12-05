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

namespace PalcoNet
{
    public partial class SeleccionarFuncionalidad : MiForm
    {
        public SeleccionarFuncionalidad()
        {
            InitializeComponent();
            Servidor servidor = Servidor.getInstance();
            SqlDataReader reader = servidor.query("EXEC dbo.getFuncionalidadesDeUsuario_sp '" + Sesion.sesion.usuario + "'");

            while (reader.Read())
            {
                Console.WriteLine("ACA ESCRIBE " + reader["nombre"].ToString());
                comboBox1.Items.Add(reader["nombre"].ToString());
            }
            reader.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new LogIn().Show();
            this.Hide();
        }
    }
}
