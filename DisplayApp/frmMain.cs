using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DisplayApp
{
	public partial class frmMain : Form
    {
        int counter = 0;
        DateTime date, pBegin, pEnd;
        public static Settings mySettings = new Settings();
        public Boolean noError = true;
        
        public frmMain()
        {
            InitializeComponent();
			if (!mySettings.LoadSettings()) 
            { 
                noError = false;
                var result = System.Windows.Forms.MessageBox.Show("Hiba a beállítások betöltése közben!" + "\n" + "Check Error.log for more information!", "Hiba", System.Windows.Forms.MessageBoxButtons.OK);
                if(result == DialogResult.OK) { System.Environment.Exit(1); }
            }
            lblLine.Text = "SOR-" + (mySettings.LineNo + 1).ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (frmSettings frm = new frmSettings())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    lblLine.Text = "SOR-" + (mySettings.LineNo + 1).ToString();
                    UpdateStats();
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            

        }

        private int dbQuantity()
        {//elkészűlt darabszám lekérése a jelenlegi műszakból
            MySqlConnection conn = new MySqlConnection(mySettings.ConnectionString);
            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) as db FROM `products` WHERE `FinishDate` >= '" + pBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and `FinishDate` < '" + pEnd.ToString("yyyy-MM-dd HH:mm:ss") + "';";
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
                return -1;
            }
        }
        private void dbDate()
        {//dátum és idő frissítése a form-on, és az aktuális idő frissítése a date változóban.
            MySqlConnection conn = new MySqlConnection(mySettings.ConnectionString);
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
                label3.Text = date.ToShortDateString();
                label4.Text = date.ToLongTimeString();
            }
            catch (Exception ex)
            {
                LogException(ex);                
            }
        }

        private int timedplanCalc(int TimeCorrection)
        {// időtől függő cél-darabszám számítása (szünetek figyelembevétele)
            try
            {
                double seconds = (date - pBegin).TotalSeconds;
                if (seconds > 13200)
                    seconds -= ((TimeCorrection / 3) * 120);

                if (seconds > 18000)
                    seconds -= ((TimeCorrection / 3) * 60);

                double calcQty = seconds / mySettings.TactTime;
                return (int)calcQty;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return -1;
            }
        }

        private int PlanedQtyCalc(int TimeCorrection)
        {//cél darabszám kiszámítása
            try
            {
                ShiftCalc();
                double seconds = (pEnd - pBegin).TotalSeconds - TimeCorrection * 60;
                double calcQty = seconds / mySettings.TactTime;
                return (int)calcQty;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return -1;
            }
        }

        private int efficiencyCalc()
        {//hatékonyság kiszámítása
            int OkProductQty, prodqtyplan;

            OkProductQty = dbQuantity();
            prodqtyplan = timedplanCalc(30);
            if(OkProductQty != -1) { lblActQuantity.Text = OkProductQty.ToString(); }
            if (prodqtyplan != -1) { lblTimedPlan.Text = prodqtyplan.ToString(); }

            if (prodqtyplan > 0)
            {
                var temp = ((float)OkProductQty / (float)prodqtyplan) * 100;
                return (int)temp;
            }
            return 0;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            ShiftCalc();
            counter++;
            if (counter >= 10) //hány másodpercenként ellenőrizze/frissítse a statisztikát
            {

                UpdateStats();
                counter = 0;
            }
        }

        private void UpdateStats()
        {
            try
            {
                var planquan = PlanedQtyCalc(30);
                if(planquan != -1) { lblPlanQuantity.Text = planquan.ToString(); }
                lblEffic.Text = efficiencyCalc().ToString() + "%";
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            
        }
        private void ShiftCalc()
        {
			if (noError)
			{
                try
                {
                    dbDate();
                    if (date != null && !date.Equals("0001.01.01 0:00:00"))
                    {//megmondja melyik műszakban vagyunk, beállítja a műszak kezdő és végző időpontját
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
                }
                catch (Exception ex)
                {
                    noError = false;
                    LogException(ex);
                    var result=MessageBox.Show("Database connection error!" + "\n" + "Check Error.log for more information!", "Error", MessageBoxButtons.RetryCancel);
                    if(result == DialogResult.Retry) { noError = true; }
                    if(result == DialogResult.Cancel) { System.Environment.Exit(1); }

                }
            }

        }
        
        //private void addMSG(string msg)
        //{//debug helper
        //    string seged = kisConsole.Text;
        //    string newLine = Environment.NewLine;
        //    seged = msg + newLine + seged;
        //    kisConsole.Text = seged;
        //}

        //Exception Logging
        public static void LogException(Exception ex)
        {
			try
			{
                File.AppendAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/Error.log"
                ,ex.Message + "\n" +"-----------------------------------------"+"\n");
			}
			catch(Exception)
			{
                System.Environment.Exit(1);
            }
            

        }

        public static void LogException(string msg, Exception ex)
        {
			try
			{
                File.AppendAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+ @"/Error.log"
                ,msg + "\n" + ex.Message + "\n" +"-----------------------------------------"+"\n");
			}
			catch(Exception)
			{
                System.Environment.Exit(1);
            }

        }
    }
}
