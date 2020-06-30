
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronPython.Hosting;

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

            string sourceFilePath = Path.GetDirectoryName(new Uri(this.GetType().Assembly.GetName().CodeBase).LocalPath);
            if (File.Exists(sourceFilePath + @"\reference\database.txt"))
            {
                using (StreamReader sr = new StreamReader(sourceFilePath + @"\reference\database.txt"))
                {
                    string currentLine;
                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        string[] dataRow = currentLine.Split(',');
                        if(dataRow.Length == 2)
                        {
                            url = dataRow[0];
                            dbName = dataRow[1];
                        }                       
                    }
                }
            }
        }

        public void disableComPort()
        {
            btLogin.Visible = false;
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            if(tbUserName.Text == null || tbUserName.Text == "")
            {
                MessageBox.Show("Username empty, try again!", "Warning");
                return;
            }

            if (tbPassword.Text == null || tbPassword.Text == "")
            {
                MessageBox.Show("Password empty, try again!", "Warning");
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
                if (!btLogin.Visible)
                {
                    btLogin.Visible = true;
                    textBoxNote.Visible = false;
                    pictureBoxLogin.Visible = false;                  
                }
            }
            else
            {
                desktopApp.checkComPort();
                btLogin.Visible = false;              
                textBoxNote.Show();
                pictureBoxLogin.Visible = true;                
            }
        }

        private void tbUserName_MouseClick(object sender, MouseEventArgs e)
        {
            if (tbUserName.Text == "Username")
                tbUserName.Text = tbUserName.Text.Replace("Username", "smuroyan@packsmartinc.com");  //smuroyan@packsmartinc.com          
        }

        private void tbPassword_MouseClick(object sender, MouseEventArgs e)
        {
            if (tbPassword.Text == "Password")
                tbPassword.Text = tbPassword.Text.Replace("Password", "Qwerty123"); //Qwerty123
        }
   
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBoxHelp_Click(object sender, EventArgs e)
        {
            // Initializes the variables to pass to the MessageBox.Show method.
            string message = "For support, please send email to dyeh@packsmartinc.com";
            string caption = "Support";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                //Closes the parent form.
                //this.Close();
            }
        }

        private void tbPassword_KeyUp(object sender, KeyEventArgs e)
        {          
            if (e.KeyCode == Keys.Enter)
            {
                if((tbUserName.Text != "") && (tbPassword.Text != ""))
                {
                    if (desktopApp.xmlLogin(url, dbName, tbUserName.Text, tbPassword.Text))
                    {
                        this.Hide();
                        desktopApp.Show();
                    }
                }
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
