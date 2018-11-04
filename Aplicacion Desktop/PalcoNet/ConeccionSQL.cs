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
    class Server
    {

        string servidor = ConfigurationSettings.AppSettings["server"];
        string db = ConfigurationSettings.AppSettings["database"];
        string user = ConfigurationSettings.AppSettings["username"];
        string password = ConfigurationSettings.AppSettings["password"];


        public static Server server;
        private SqlConnection connection;
        private SqlDataReader reader;

    }
}