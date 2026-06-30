using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Database
{
    using MYSQLCOMMAND = MySql.Data.MySqlClient.MySqlCommand;
    using MYSQLREADER = MySql.Data.MySqlClient.MySqlDataReader;
    using MYSQLCONNECTION = MySql.Data.MySqlClient.MySqlConnection;
    public class Connection
    {
        private static string MySqlUsername, MySqlPassword, MySqlDatabase, MySqlHost;
        public static string ConnectionString;
        public static void CreateConnection(string user, string password, string database, string host)
        {
            MySqlUsername = user;
            MySqlHost = host;
            MySqlPassword = password;//
            MySqlDatabase = database;
            ConnectionString = "Server=" + host + ";Port=3306;Database=" + database + ";Uid=" + user + ";Password=" + password + ";SslMode=None;Persist Security Info=True;Pooling=true; Min Pool Size = 1;  Max Pool Size = 5;";         
        }
        public static bool OpenConnection()
        {
            try
            {
                MySqlConnection.Open();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {             
                switch (ex.Number)
                {
                    case 0:
                        Program.MyApp.ShowBox("Cannot connect to server.  Contact administrator");
                        break;                 
                    case 1045:
                        Program.MyApp.ShowBox("Invalid username/password, please try again");
                        break;
                    default:
                        Program.MyApp.ShowBox(ex.ToString());
                        break;
                }
                return false;
            }
        }
        public static MYSQLCONNECTION MySqlConnection
        {
            get
            {
                MYSQLCONNECTION conn = new MYSQLCONNECTION();

                conn.ConnectionString = ConnectionString;
                return conn;
            }
        }
    }
}
