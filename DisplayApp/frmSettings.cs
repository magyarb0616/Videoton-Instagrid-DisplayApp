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
            cmbLine.SelectedIndex = Properties.Settings.Default.lineNu;
            cmbLine.Enabled = true;
            mtbTactIdo.Text = Properties.Settings.Default.taktIdo.ToString("#0.0");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.taktIdo = float.Parse(mtbTactIdo.Text);
            Properties.Settings.Default.lineNu = (sbyte)cmbLine.SelectedIndex;
            Properties.Settings.Default.Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //this.Close();
        }
    }
}
