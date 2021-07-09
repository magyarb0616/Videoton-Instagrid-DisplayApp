using System;
using System.Windows.Forms;
using static DisplayApp.frmMain;

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
                cmbLine.SelectedIndex = mySettings.LineNo;
            }
            catch (Exception)
            {

                throw;
            }
            cmbLine.Enabled = true;
            mtbTactIdo.Text = mySettings.TactTime.ToString("#0.0");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mySettings.LineNo = cmbLine.SelectedIndex;
            mySettings.TactTime = float.Parse(mtbTactIdo.Text);
            mySettings.SaveSettings();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
