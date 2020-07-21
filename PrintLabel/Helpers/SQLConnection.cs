using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintLabel.Helpers
{
    public class SQLConnection
    {
        SqlConnection conn;
        Configuration config= ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public SQLConnection()
        {
            conn = new SqlConnection(GetConnectionString());
        }
        public bool isConnection
        {
            get
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                return true;
            }
        }

        public void SaveConfig(string server, string database, string username, string password)
        {
            config.AppSettings.Settings["server"].Value = server;
            config.AppSettings.Settings["database"].Value = database;
            config.AppSettings.Settings["username"].Value = username;
            config.AppSettings.Settings["password"].Value = password;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public string[] LoadConfig()
        {
            string[] res = new string[4];
            res[0] = config.AppSettings.Settings["server"].Value;
            res[1] = config.AppSettings.Settings["database"].Value;
            res[2] = config.AppSettings.Settings["username"].Value;
            res[3] = config.AppSettings.Settings["password"].Value;
            return res;
        }

        public string GetConnectionString()
        {
            string sServer = config.AppSettings.Settings["server"].Value;
            string sDatabase = config.AppSettings.Settings["database"].Value;
            string sUsername = config.AppSettings.Settings["username"].Value;
            string sPassword = config.AppSettings.Settings["password"].Value;
            string connectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2}; Password={3};", sServer, sDatabase, sUsername, sPassword);

            return connectionString;
        }
    }
}
