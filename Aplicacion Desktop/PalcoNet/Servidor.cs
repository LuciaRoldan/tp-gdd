using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace PalcoNet
{
    public class Servidor
    {
        string servidor = ConfigurationManager.AppSettings["server"];
        string db = ConfigurationManager.AppSettings["database"];
        string user = ConfigurationManager.AppSettings["username"];
        string password = ConfigurationManager.AppSettings["password"];

        public static Servidor server;
        private SqlConnection connection;
        private SqlDataReader reader;

        //Trae una instancia del servidor
        public static Servidor getInstance()
        {
            if (server == null)
            {
                server = new Servidor();
                server.conectar();
            }
            return server;
        }

        //Ejecuta una query y devuelve un reader
        public SqlDataReader query(string query)
        {
            SqlCommand command = new SqlCommand(query, this.connection);
            this.reader = command.ExecuteReader();
            return this.reader;
        }

        //Cierra el reader
        public void closeReader()
        {
            this.reader.Close();
        }

        //Ejecuta una query que no retorna nada
        public void realizarQuery(string query)
        {
            this.query(query);
            this.closeReader();
        }

        //Conecta con la base de datos
        private void conectar()
        {
            try
            {
                this.connection = new SqlConnection("Data Source=" + servidor +
                    ";Initial Catalog=" + db + ";Integrated Security=False;User ID=" + user + ";Password=" + password + ";MultipleActiveResultSets=True");
                this.connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}