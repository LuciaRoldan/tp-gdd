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

namespace PalcoNet.Abm_Rubro
{
    public partial class AbmRubro : MiForm
    {
        Servidor servidor = Servidor.getInstance();
        Publicacion publicacion = new Publicacion();
        public AbmRubro(MiForm formAnterior) : base(formAnterior)
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Acá se buscaría una publicación particular, a la cual luego se le modificaría el rubro/categoría
            //No nos extendimos en el resto de funcionalidad ya que no era un requerimiento del trabajo
            publicacion.Id = Convert.ToInt16(textBox3.Text);

            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.getRubrosDePublicacion_sp " + publicacion.Id);

            while (reader.Read())
            {
                textBox1.Text += (reader["id_rubro"].ToString());
                textBox2.Text += (reader["descripcion"].ToString());
            }
            reader.Close();
        }
    }
}
