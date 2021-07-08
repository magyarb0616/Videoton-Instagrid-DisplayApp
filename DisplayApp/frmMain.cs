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
        string msgConsole = "";
        string server = ConfigurationManager.AppSettings.Get("server");
        string database = ConfigurationManager.AppSettings.Get("database");
        string user = ConfigurationManager.AppSettings.Get("user");
        string password = ConfigurationManager.AppSettings.Get("password");
        string port = ConfigurationManager.AppSettings.Get("port");
        DateTime date, pBegin, pEnd;
        
        public frmMain()
        {
            InitializeComponent();
            addMSG("Started!");
            dbDate();
            //label2.Text = "AUTO-" + ((int.Parse(ConfigurationManager.AppSettings.Get("lineNo"))) + 1).ToString();
            //label2.Text = "AUTO-" + ConfigurationManager.AppSettings.Get("lineNo");
            ShiftCalc();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (frmSettings frm = new frmSettings())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    label2.Text = "AUTO-" + ((int.Parse(ConfigurationManager.AppSettings.Get("lineNo"))) + 1).ToString();
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            

        }

        private int dbQuantity(DateTime begin,DateTime end)
        {
            String connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", server, port, user, password, database);
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) as db FROM `products` WHERE `FinishDate` >= '" + begin.ToString("yyyy-MM-dd HH:mm:ss") + "' and `FinishDate` < '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                int adat = 0;
                while (rdr.Read())
                {
                    adat = int.Parse(rdr[0].ToString());
                }
                rdr.Close();
                conn.Close();
                return adat;
            }
            catch (Exception ex)
            {
                LogException(ex);
                addMSG("DB quantity: "+ex.Message);
                return -1;
            }
        }
        private void dbDate()
        {
            String connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", server, port, user, password, database);
            MySqlConnection conn = new MySqlConnection(connectionString);
 
            try
            {
                conn.Open();
                string sql = "SELECT NOW();";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                string adat = null;
                while (rdr.Read())
                {
                    adat = rdr[0].ToString();
                }
                rdr.Close();
                conn.Close();
                date = DateTime.Parse(adat);
                //string[] datefrag = date.split('.');
                //label3.text = datefrag[0]+ "." + datefrag[1]+ "." + datefrag[2];
                //label4.text = datefrag[3];
                label3.Text = date.ToShortDateString();
                label4.Text = date.ToLongTimeString();
            }
            catch (Exception ex)
            {
                LogException(ex);
                addMSG("DB date:"+ex.Message);
                
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            dbDate();
            addMSG(dbQuantity(pBegin, pEnd).ToString());

        }

        private void addMSG(string msg)
        {
            string seged = kisConsole.Text;
            string newLine = Environment.NewLine;
            seged = seged + newLine + msg;
            kisConsole.Text = seged;
        }

        private void ShiftCalc()
        {
            dbDate();
            if (date!= null)
            {
                if (date.TimeOfDay < TimeSpan.Parse("06:00:00") || date.TimeOfDay > TimeSpan.Parse("21:59:59"))
                {
                    pBegin = date.AddDays(-1).Date.Add(TimeSpan.Parse("22:00:00"));
                    pEnd = date.Date.Add(TimeSpan.Parse("05:59:59"));
                }
                else if (date.TimeOfDay < TimeSpan.Parse("14:00:00"))
                {
                    pBegin = date.Date.Add(TimeSpan.Parse("06:00:00"));
                    pEnd = date.Date.Add(TimeSpan.Parse("13:59:59"));
                }
                else
                {
                    pBegin = date.Date.Add(TimeSpan.Parse("14:00:00"));
                    pEnd = date.Date.Add(TimeSpan.Parse("21:59:59"));
                }
            }
            
            addMSG("pBegin=" + pBegin + ", pEnd=" + pEnd);
        }
    }
}
