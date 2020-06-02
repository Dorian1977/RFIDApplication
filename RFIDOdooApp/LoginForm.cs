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
        RFID_Desktop_App desktopApp = null;


        string url = "https://packsmart-rfid-1142387.dev.odoo.com";
        string dbName = "packsmart-rfid-1142387";
        //string userName = "smuroyan@packsmartinc.com";
        //string pwd = "Qwerty123";
        
        public LoginForm()
        {
            desktopApp = new RFID_Desktop_App();
            InitializeComponent();
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

        private void btConnect_Click(object sender, EventArgs e)
        {
            desktopApp.checkComPort();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (desktopApp.iComPortStatus == 1)
            {
                btConnect.BackColor = Color.LightGreen;
                btLogin.Enabled = true;
                timer1.Enabled = false;
            }
            else
            {
                btConnect.BackColor = SystemColors.Control;
                btLogin.Enabled = false;
            }
        }
    }
}
