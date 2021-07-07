using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DisplayApp.Database;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Collections.Specialized;


namespace DisplayApp
{
    public partial class frmMain : Form
    {
        string server = ConfigurationManager.AppSettings.Get("server");
        string database = ConfigurationManager.AppSettings.Get("database");
        string user = ConfigurationManager.AppSettings.Get("user");
        string password = ConfigurationManager.AppSettings.Get("password");
        string port = ConfigurationManager.AppSettings.Get("port");

        public frmMain()
        {
            InitializeComponent();
            dbDate();
            label2.Text = "AUTO-"+(Properties.Settings.Default.lineNu+1).ToString();

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (frmSettings frm = new frmSettings())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    label2.Text = "AUTO-" + (Properties.Settings.Default.lineNu + 1).ToString();
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            

        }

        private void dbDate()
        {
            String connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", server, port, user, password, database);
            //String connStr = "";
            MySqlConnection conn = new MySqlConnection(connectionString);
 
            try
            {
                conn.Open();
                string sql = "SELECT NOW();";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                string date = null;
                while (rdr.Read())
                {
                    date = rdr[0].ToString();
                }
                rdr.Close();
                string[] dateFrag = date.Split('.');
                label3.Text = dateFrag[0]+ "." + dateFrag[1]+ "." + dateFrag[2];
                label4.Text = dateFrag[3];
            }
            catch (Exception ex)
            {
                LogException(ex);
                
            }
        }
    }
}
