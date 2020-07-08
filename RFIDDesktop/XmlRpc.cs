#define enableWriteControl
#define Connect2Odoo
using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDApplication
{
    public class XMLRPC
    {
        public static XmlRPCTagStatus xmlRpcStatus = XmlRPCTagStatus.Ready;
        public enum XmlRPCTagStatus
        {
            Ready,
            ReadID,
            WriteID,
            WriteIDOk,
            WriteAccessCode,
            WriteAccessOk,
            WriteReserveData,
            writeUserData,
            WriteDataDone,
            OdooSuccessful,
            OdooFail,
            Test
        }
    }
    public partial class RFIDTagIDForm
    {

        public int xmlLogin(string url, string dbName, string usr, string pwd, out List<string> productList)
        {
            List<string> products = new List<string>();
            int iLogin = 0;
            try
            {
                var engine = Python.CreateEngine();
                ICollection<string> Paths = engine.GetSearchPaths();
                //Paths.Add(@"C:\Program Files\IronPython 2.7\Lib");
                Paths.Add(sourceFilePath + @"\reference\Lib");
                engine.SetSearchPaths(Paths);
                dynamic py = engine.ExecuteFile(sourceFilePath + @"\reference\xml_rpc.py");

                xmlRpc = py.XmlRpc();
                xmlRpc.isRFIDConnected(true);
                iLogin = xmlRpc.login(url, dbName, usr, pwd);
                               
                XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.Ready;
#if true
                var readProductList = xmlRpc.readWorkOrderList();
                if(readProductList.Count == 0)
                {
                    MessageBox.Show("No work order planned!");
                    productList = products;
                    iLogin = -1;
                }
                else
                {                    
                    for(int i=0;i< readProductList.Count; i++)
                    {
                        products.Add(readProductList[i][1]);
                    }
                }
                if(iLogin == 0)
                {
                    tabCtrMain.TabPages.Remove(pageEpcID);
                }
#else
                var productionId = xmlRpc.readProductId();
                var productId = xmlRpc.readProductId();

                Type pID = productionId.GetType();
                if (productionId.GetType() == typeof(string) && pID == typeof(string))
                {
                    MessageBox.Show("Can't find work order, or work order is pause ", "Warning");
                    //tabCtrMain.TabPages.Remove(pageData);
                    return false;
                }
                else if (pID == typeof(int))
                {
                    MessageBox.Show("Read Error: " + productionId +
                                    " work orders are currerntly in progress!. Please pause all but one workorder before proceeding.", "Warning");
                    return false;
                }
                else if (pID == typeof(IronPython.Runtime.List))
                {
                    if (productId.Count > 0 && productionId.Count > 0)
                    {
                        richtbWorkOrderInfo.Text = productionId[1];
                        richTextBoxProductID.Text = productId[1];
                        if (!bLogin)
                        {
                            tabCtrMain.TabPages.Remove(pageEpcID);
                        }
                    }
                }
#endif
            }
            catch (Exception exp)
            {
                MessageBox.Show("No work order planned, got error " + exp.Message, "Warning");
                //tabCtrMain.TabPages.Remove(pageData);
                productList = products;
                iLogin = -1;
            }
            productList = products;
            return iLogin;
        }

        void getWorkOrder(out List<string> productList)
        {
            List<string> products = new List<string>();
            var readProductList = xmlRpc.readWorkOrderList();            
            for (int i = 0; i < readProductList.Count; i++)
            {
                products.Add(readProductList[i][1]);
            }
            productList = products;
        }
#if false
        void checkOdooStatus()
        {
            int tagIndex = findTag(tagSelect.writeData);
            if (tagIndex < 0) return;
            try
            {
                var final_lot_id = xmlRpc.readFinalLotID(); //{'final_lot_id': [554, 'PS1109=S2-0058'], 'id': 27}
                if (final_lot_id != null && final_lot_id != "")
                {
                    if (tagLists[tagIndex].OdooTagInfo.Contains(final_lot_id))
                    {
                        XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.OdooSuccessful;
                        //udpate tag status for done or content updated 
                        tbOdooStatus.Text = "Tag " + tagLists[tagIndex].OdooTagInfo + " done, Move to next tag";
                        //if (tbOdooStatus.ForeColor != Color.Green || tbOdooStatus.Visible == false)
                        {
                            //tbOdooStatus.Visible = true;
                            //tbOdooStatus.Text = "Tag " + tagLists[tagIndex].OdooTagInfo +
                            //                   " Activated. Press \"UPDATE\" \\- \\>  \"RECORD PRODCUTION\" on Odoo";
                            //tbOdooStatus.ForeColor = Color.Green;
                            //tbOdooStatus.BackColor = Color.White;
                            //RFIDTagInfo.playSound(true);
                        }
                        //tbDataUpdateStatus.Text += "Update Successful, Move to next RFID Tag";   
                        return;
                    }
                }
            }
            catch (Exception exp) { }
            // Initializes the variables to pass to the MessageBox.Show method.
            /*string message = "*** Click \"Update\" on WebPage Now ***";
            string caption = "Notice";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            timerInventory.Enabled = false;
            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                timerInventory.Enabled = true;
                initScanTag(false);
            }*/
        }
#endif
        int rfidID = 0;
        private void xmlRpcUpdateTagID()
        {
            if (tagLists.Count > 0)
            {
                int tagIndex = findTag(tagSelect.writeID);
                if (tagIndex < 0) return;
                               
                selectTag(tagLists[tagIndex].EPC_ID);
                switch (tagLists[tagIndex].tagStatus)
                {
                    case RFIDTagData.TagStatus.IDNotUpdate:
                        {
#if enableWriteControl
                            if (XMLRPC.xmlRpcStatus != XMLRPC.XmlRPCTagStatus.WriteID)
                            {
                                rfidID = xmlRpc.getRFIDNumber();
                                
                                if (rfidID == 0)
                                {
                                    WriteLog(lrtxtLog, "Can't read Tag ID from cloud ", 1);
                                    return;
                                }
                               XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.WriteIDOk;
                            }
#else
                            rfidID++;
#endif
                            //textBoxEPCTagID.Text = rfidID.ToString();
                            string tagID = "";
                            //resetStatusColor();
                            if (RFIDTagInfo.reserverData == "")
                            {
                                selectTag(tagLists[tagIndex].EPC_ID);
                                setAccessCode(tagIndex);
                            }

                            writeTagID(rfidID.ToString(), tagIndex, out tagID);
                            XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.Ready;
                            m_curInventoryBuffer.removeInventoryItem(2, tagLists[tagIndex].EPC_ID);
                            foreach (ListViewItem item in listViewEPCTag.Items)
                            {
                                if (item.SubItems[0].Text == tagLists[tagIndex].EPC_ID)
                                {
                                    item.Remove();
                                    break;
                                }
                            }
                            tagLists.RemoveAt(tagIndex);
                            //initScanTag();
                        }
                        break;
                    case RFIDTagData.TagStatus.IDUpdated:
                        {
                            XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.Ready;
                            checkTagStatus(tagLists[tagIndex].EPC_ID);
                        }
                        break;
                    case RFIDTagData.TagStatus.AccessCodeNotUpdate:
                        {
                            RFIDTagInfo.reserverData = "";
                            //selectTag(tagLists[maxCountIndex].EPC_ID);
                            setAccessCode(tagIndex);
                            checkTagStatus(tagLists[tagIndex].EPC_ID);
                            XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.WriteAccessCode;
                        }
                        break;
                }
            }
        }

        private void writeTagID(string serialNumber, int index, out string tagID)
        {//1. original tag
         //2. pack smart tag, start with 50 30
            writeTagRetry = 0;
            byte btWordCnt = 0;

            string[] result = { "00", "00", "00", "00" };
            byte[] btAryPwd = { 0, 0, 0, 0 };

            if (RFIDTagInfo.bAccessCode(symmetric.readAccessCode(), RFIDTagInfo.reserverData))
            {
                btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
            }
            string EPClabel = RFIDTagInfo.readLabelFormat().Substring(0, 2).ToUpper();
            ulong uNumber = 0;
            string EPSnumber = "";
            try
            {
                if (ulong.TryParse(serialNumber, out uNumber))
                {
                    EPSnumber = uNumber.ToString("D20");
                }
            }
            catch (Exception exp)
            {
                WriteLog(lrtxtLog, "Input Tag out of range " + serialNumber, 1);
                tagID = "";
                return;
            }
            //mix with 2 ASCII and 10 digits
            tagID = RFIDTagInfo.ASCIIToHex(EPClabel) + RFIDTagInfo.AddHexSpace(EPSnumber);
            RFIDTagInfo.WriteData.ID = CCommondMethod.String2ByteArray(tagID, 2, out btWordCnt);

            //1, access, 1, 2, 6, data, 148
            reader.WriteTag(m_curSetting.btReadId, btAryPwd, 1, 2, btWordCnt, RFIDTagInfo.WriteData.ID, 0x94);
#if DEBUG
            WriteLog(lrtxtLog, "Write Tag ID: " + tagID, 0);
#endif
            Thread.Sleep(rwTagDelay * 3);
        }
#if false
        private void btEPCTag_Click(object sender, EventArgs e)
        {//update Tag, 2 kinds of tag id
            string tagID = ""; // to be write
            if(tbSelectTag.Text == "")
            {
                MessageBox.Show("Error, Scan Tag first!, Abort!");
                return;
            }

            if(textBoxEPCTagID.Text == "")
            {
                MessageBox.Show("Error, Input Tag ID in number format first!, Abort!");
                return;
            }
            selectTag(tbSelectTag.Text);
            RFIDTagInfo.reserverData = "";
            btAccessCode_Click(sender, e);
            writeTagID(textBoxEPCTagID.Text, out tagID);

            if (listViewEPCTag.SelectedItems.Count > 0)
            {
                for (int i = 0; i < tagLists.Count; i++)
                {
                    if (tagLists[i].EPC_ID == listViewEPCTag.SelectedItems[0].Text)
                    {
                        tagLists.RemoveAt(i);
                        //break;
                    }
                    else
                    {
                        tagLists[i].readCount = 0;
                    }
                }
                listViewEPCTag.SelectedItems[0].Remove();
            }
            resetStatusColor();
            //initScanTag();
        }
#endif
        bool bShowWorkOrderError = false;
        private void xmlRpcUpdateTagData()
        {//1. get RFID tag with top hit count
            int tagIndex = 0;
            if (tagLists.Count == 0)
                return;

            tagIndex = findTag(tagSelect.writeData);
            if (tagIndex < 0)
            {
                return;
            }

            ulong rCount = 0;
            selectTag(tagLists[tagIndex].EPC_ID);
            tagLists[tagIndex].label = RFIDTagInfo.readEPCLabel(tagLists[tagIndex].EPC_ID, out rCount);
            string rfidTag = tagLists[tagIndex].label.Substring(0, 2) + rCount.ToString();
#if Connect2Odoo
            if (productInfo != "")
                tagLists[tagIndex].OdooTagInfo = rfidTag + RFIDTagInfo.serialSep + productInfo; //xmlRpc.readRFIDTagID(rfidTag);     
            else return;
#endif

            switch (XMLRPC.xmlRpcStatus)
            {
                //case XMLRPC.XmlRPCTagStatus.writeUserData:
                //    return;
                case XMLRPC.XmlRPCTagStatus.WriteDataDone:
                    {
                        reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, RFIDTagInfo.accessCode);
                        Thread.Sleep(rwTagDelay * 2);
                    }
                    break;
            }
#if DEBUG
            WriteLog(lrtxtLog, "Write data state " + XMLRPC.xmlRpcStatus, 0);
#endif
#if DEBUG
            //WriteLog(lrtxtLog, "Send Tag: " + tagLists[tagIndex].label, 0);
#endif
#if Connect2Odoo
            string tagOdooData = tagLists[tagIndex].OdooTagInfo;//xmlRpc.updateTag();
#else
            tagOdooData = rfidTag + "=S2-0067";
#endif
            if (tagOdooData == "" && !bShowWorkOrderError)
            {
                bShowWorkOrderError = true;
                string message = "*** Cannot find Work Order in Odoo! ***" +
                                 "Please make sure the Work Order is being processed and not paused.";
                string caption = "Warning";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                timerInventory.Enabled = false;
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    timerInventory.Enabled = true;
                }
                return;
            }
            else if (tagOdooData != "" && RFIDTagInfo.verifyData(tagOdooData, sourceFilePath))
            {//Todo: need the manufacturing data
                string strRFIDTag = "";
                int dateInDigit = 0;
                if (int.TryParse(DateTime.Now.ToString("MMyy"), out dateInDigit))
                {
                    string[] tagDataList = tagOdooData.Split(RFIDTagInfo.serialSep);

                    //Todo: lookup table to convert ink type to detail info                       
                    using (StreamReader sr = new StreamReader(sourceFilePath + @"\reference\lookUpTable.csv"))
                    {
                        string currentLine;
                        // currentLine will be null when the StreamReader reaches the end of file
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            string[] dataRow = currentLine.Split(',');
                            if (dataRow[0] == tagDataList[1])
                            {//convert ink type from ASCII to digit
                                strRFIDTag = tagOdooData.Replace(dataRow[0], dataRow[1].Trim() + dataRow[2].Trim());
                                //add expire date
                                strRFIDTag += (dateInDigit + 1).ToString("D4") + "00"; //add supplier
                                break;
                            }
                        }
                    }
                    if (strRFIDTag != "")
                    {
                        writeTagData(strRFIDTag, tagIndex);
                    }
                }
            }
        }

        private void writeTagData(string data, int tagIndex)
        {
            byte btWordCnt = 0;
            if (data.Length < 17)
            {//need to be at less 120 
                int missNum = 17 - data.Length;
                data += new string(RFIDTagInfo.serialSep, missNum);
            }

            symmetric.encryptedtext = symmetric.Encrypt(data);
            string strEncrypted = Convert.ToBase64String(symmetric.encryptedtext);            
            string reserveData = RFIDTagInfo.ASCIIToHex(strEncrypted.Substring(0, 4));
            string userData = RFIDTagInfo.ASCIIToHex(strEncrypted.Substring(4));

            RFIDTagInfo.WriteData.data = CCommondMethod.String2ByteArray(userData.ToUpper() + "00 00 00 00 ", 2, out btWordCnt);
            RFIDTagInfo.WriteData.reserve = CCommondMethod.String2ByteArray(reserveData.ToUpper(), 2, out btWordCnt);

            if (!RFIDTagInfo.reserverData.Trim().StartsWith(reserveData.ToUpper().Trim()))
            {//1. check reserve section
                XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.WriteReserveData;
                reader.WriteTag(m_curSetting.btReadId, RFIDTagInfo.accessCode, 0, 0, btWordCnt, RFIDTagInfo.WriteData.reserve, 0x94);
                Thread.Sleep(rwTagDelay * 3);
#if DEBUG
                WriteLog(lrtxtLog, "Write reserve ", 0);
#endif
            }
            else if(tagLists[tagIndex].tagTIDInfo == "")
            {
                reader.ReadTag(m_curSetting.btReadId, 2, 0, 12, null);
                Thread.Sleep(rwTagDelay);
#if DEBUG
                WriteLog(lrtxtLog, "Read TID ", 0);
#endif
            }
            else if (tagLists[tagIndex].tagInfo != data && XMLRPC.xmlRpcStatus != XMLRPC.XmlRPCTagStatus.WriteDataDone)
            {//check tag data section
                XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.writeUserData;
                byte size = (byte)(RFIDTagInfo.WriteData.data.Length / 2);
                reader.WriteTag(m_curSetting.btReadId, RFIDTagInfo.accessCode, 3, 0, size, RFIDTagInfo.WriteData.data, 0x94);
                Thread.Sleep(rwTagDelay * 5);
#if DEBUG
                WriteLog(lrtxtLog, "Write data ", 0);
#endif
            }
            /*else if ((RFIDTagInfo.bAccessCode(symmetric.readAccessCode(), RFIDTagInfo.reserverData)) &&
                     (tagLists[tagIndex].tagInfo != tagLists[tagIndex].OdooTagInfo) &&
                     XML
            {

            }*/
            else if(tagLists[tagIndex].tagInfo == data)
            {
                XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.WriteDataDone;
                reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, RFIDTagInfo.accessCode);
                Thread.Sleep(rwTagDelay * 2);
#if DEBUG
                WriteLog(lrtxtLog, "read data ", 0);
#endif
            }
        }
    }
}
