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
        string servidor = ConfigurationSettings.AppSettings["server"];
        string db = ConfigurationSettings.AppSettings["database"];
        string user = ConfigurationSettings.AppSettings["username"];
        string password = ConfigurationSettings.AppSettings["password"];
       

        public static Servidor server;
        private SqlConnection connection;
        private SqlDataReader reader;

        /// <summary>Get the instance for the server</summary>
        public static Servidor getInstance()
        {
            if (server == null)
            {
                server = new Servidor();
                server.conectar();
            }
            return server;
        }

        /// <summary>Execute a query with a return value (functions or selects)</summary>
        /// <param name="query">Query to be executed</param>
        /// <returns>Returns the reader with the results of the query</returns>
        public SqlDataReader query(string query)
        {
            SqlCommand command = new SqlCommand(query, this.connection);
            this.reader = command.ExecuteReader();
            return this.reader;
        }

        /// <summary>Close the reader opened with the previous method (if the reader is not closed it will fail in the next execution)</summary>
        public void closeReader()
        {
            this.reader.Close();
        }

        /// <summary>Execute a query without a return value (procedures, updates, drops, etc)</summary>
        public void realizarQuery(string query)
        {
            this.query(query);
            this.closeReader();
        }

        /// <summary>Connects to the database</summary>
        private void conectar()
        {
            try
            {
                this.connection = new SqlConnection("Data Source=" + servidor +
                    ";Initial Catalog=" + db + ";Integrated Security=False;User ID=" + user + ";Password=" + password+";MultipleActiveResultSets=True");
                this.connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
    }
}
