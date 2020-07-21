using PrintLabel.Helpers;
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

namespace PrintLabel
{
    public partial class FrmMain : Form
    {
        private Form activeForm = null;
        //string connectionString = "Data Source=10.50.4.4;Initial Catalog=VACP4DB;User ID=sa; Password=ACVN1234~!;";
        string connectionString = "Data Source=DESKTOP-P8OGV6L\\SQLEXPRESS;Initial Catalog=VACP4DB;User ID=sa; Password=123456;";
        LoadBatchcodeFromWO loadBatchcodeFromWO = new LoadBatchcodeFromWO();
        public FrmMain()
        {
            InitializeComponent();
        }

        private void openActiveForm(Form frm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = frm;
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            frm.TopMost = true;
            frm.FormBorderStyle = FormBorderStyle.None;
            pnMain.Controls.Add(frm);
            pnMain.Tag = frm;
            frm.BringToFront();
            frm.Show();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbProduct.SelectedItem;
            String product = drv["Product"].ToString().Trim();
            DataTable dt = loadBatchcodeFromWO.LoadBatchcode(dpDate.Value.ToString("MM/dd/yyyy"), product);
            Printer printer = new Printer();
            printer.PrintAll(product, dt);
        }

        private void dpDate_ValueChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbProduct.SelectedItem;
            String product = drv["Product"].ToString().Trim();
            DataTable dt = loadBatchcodeFromWO.LoadBatchcode(dpDate.Value.ToString("MM/dd/yyyy"), product);
            dvData.DataSource = dt;
            lblTotal.Text = $"Total: {dt.Rows.Count} labels";
        }

        private void LoadProduct()
        {
            string queryString = $"SELECT DISTINCT(PRODUCT) FROM BATCHCODE WHERE CONVERT(DATE, CREATEDATE, 103) = '{dpDate.Value.ToString("MM/dd/yyyy")}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(queryString, connectionString);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        cbProduct.DataSource = dt;
                        cbProduct.DisplayMember = "Product";
                        cbProduct.ValueMember = "Product";
                    }
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
        }

        private void cbProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbProduct.SelectedItem;
            String product = drv["Product"].ToString().Trim();
            DataTable dt = loadBatchcodeFromWO.LoadBatchcode(dpDate.Value.ToString("MM/dd/yyyy"), product);
            dvData.DataSource = dt;
            lblTotal.Text = $"Total: {dt.Rows.Count} labels";
        }

        private void currentTime_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
        }

        private void btnPrintError_Click(object sender, EventArgs e)
        {
            FrmPrintError frm = new FrmPrintError();
            frm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            FrmLogin frm = new FrmLogin();
            frm.Show();
            this.Hide();
        }
    }
}
