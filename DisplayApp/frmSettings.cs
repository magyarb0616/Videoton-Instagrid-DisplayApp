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
using DisplayApp;
using System.Configuration;
using System.Collections.Specialized;

namespace DisplayApp
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            cmbLine.Items.Clear();
            cmbLine.Items.Add("AUTO-1");
            cmbLine.Items.Add("AUTO-2");
            try
            {
               // cmbLine.SelectedIndex = int.Parse(ConfigurationManager.AppSettings.Get("lineNo"));
            }
            catch (Exception)
            {

                throw;
            }
            cmbLine.Enabled = true;
            //mtbTactIdo.Text = float.Parse(ConfigurationManager.AppSettings.Get("tactTime")).ToString("#0.0");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
