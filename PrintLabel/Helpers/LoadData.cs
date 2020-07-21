using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintLabel.Helpers
{
    class LoadData
    {
        //string connectionString = "Data Source=10.50.4.4;Initial Catalog=VACP4DB;User ID=sa; Password=ACVN1234~!;";
        string connectionString = "Data Source=DESKTOP-P8OGV6L\\SQLEXPRESS;Initial Catalog=VACP4DB;User ID=sa; Password=123456;";
        public DataTable FillDataTable(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }
    }
}
