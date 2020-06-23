using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDApplication
{
    public partial class LoginForm : Form
    {
        RFIDTagIDForm desktopApp = null;

        string url = "https://packsmart-rfid-1204018.dev.odoo.com";
        string dbName = "packsmart-rfid-1204018";

        //string userName = "smuroyan@packsmartinc.com";
        //string pwd = "Qwerty123";

        public LoginForm()
        {
            desktopApp = new RFIDTagIDForm(this);
            desktopApp.checkComPort();
            InitializeComponent();
        }

        public void disableComPort()
        {
            btLogin.Enabled = false;            
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            if(tbUserName.Text == null || tbUserName.Text == "")
            {
                MessageBox.Show("Username empty, try again!");
                return;
            }

            if (tbPassword.Text == null || tbPassword.Text == "")
            {
                MessageBox.Show("Password empty, try again!");
                return;
            }

            if(desktopApp.xmlLogin(url, dbName, tbUserName.Text, tbPassword.Text))
            {
                this.Hide();                                
                desktopApp.Show();                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (desktopApp.iComPortStatus == 1)
            {
                btLogin.Enabled = true;
                labelNotes.Text = "Notes: RFID connected successful, Login now";
            }
            else
            {
                desktopApp.checkComPort();
                btLogin.Enabled = false;
                labelNotes.Text = "Note: Connect to RFID Reader first";
            }
        }
    }
}
