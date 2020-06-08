using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuthorizationForms
{
    public partial class AuthorizationForm : Form
    {
        Symmetric_Encrypted symmetric = new Symmetric_Encrypted();
        const int dayAllowed = 1;

        public AuthorizationForm()
        {
            InitializeComponent();
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            //verify each field
            //1. date
            if (RFIDTagInfo.checkAuthDate(tbDate.Text, dayAllowed) != 1)
            {
                MessageBox.Show("Abort, Expire date input not corrected, check again (mmddyy)!",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //2. verify volume
            int inkVolume = 0;
            if(!int.TryParse(tbInkVolume.Text, out inkVolume))
            {
                MessageBox.Show("Abort, ink volume input not corrected, check again (ml)!",
                               "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //3. serial number
            UInt64 serialNum = 0;
            if((tbTagID.Text.Substring(0,2) != "PS") ||
                (!UInt64.TryParse(tbTagID.Text.Substring(2), out serialNum)))
            {
                MessageBox.Show("Abort, RFID tag ID input not corrected, check again PS with max 19 numbers!",
                              "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //load key
            byte[] encryptKey = Properties.Resources.SymmetricKey;
            symmetric.loadKey(encryptKey);

            string data = tbDate.Text + "," + tbTagID.Text + "," + 
                          tbInkVolume.Text + "," + tbDongleID.Text;
            symmetric.encryptedtext = symmetric.Encrypt(data);
            tbEncryptedData.Text = Convert.ToBase64String(symmetric.encryptedtext);
        }

        private void btVerify_Click(object sender, EventArgs e)
        {
            checkAuthFile(tbEncryptedData.Text);
        }

        public void checkAuthFile(string line)
        {//read from encrypted log file and compare
            string strAuthData = "";
            try
            {
                strAuthData = symmetric.DecryptData(Convert.FromBase64String(line));
                if (strAuthData != "")
                {
                    string[] authDataArry = strAuthData.Split(',');                   
                    if ((authDataArry[0] == tbDate.Text) &&
                        (authDataArry[1] == tbTagID.Text) &&
                        (authDataArry[2] == tbInkVolume.Text) &&
                        (authDataArry[3] == tbDongleID.Text))
                    {
                        MessageBox.Show("Verified successful!");
                        return;
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Read Authorization data got exception: " + exp.Message);
            }
            MessageBox.Show("Read Authorization data failed! ");
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = @"C:\ProgramData";
            saveFileDialog1.FileName = "inkAuthorization.dat";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream fileStream = saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fileStream);
                sw.WriteLine(tbEncryptedData.Text);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
