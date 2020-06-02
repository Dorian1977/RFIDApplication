//#define enableWriteControl
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;
using IronPython.Hosting;

namespace RFIDApplication
{
    public partial class RFIDTagIDForm : Form
    {
        LoginForm loginForm = null;
        public dynamic xmlRpc = null;
        private Reader.ReaderMethod reader;
        private ReaderSetting m_curSetting = new ReaderSetting();
        private InventoryBuffer m_curInventoryBuffer = new InventoryBuffer();
        private OperateTagBuffer m_curOperateTagBuffer = new OperateTagBuffer();
        Symmetric_Encrypted symmetric = new Symmetric_Encrypted();

        public int iComPortStatus = 0;

        //Before inventory, you need to set working antenna to identify whether the inventory operation is executing.
        private bool m_bInventory = false;
        //Identify whether reckon the command execution time, and the current inventory command needs to reckon time.
        //private bool m_bReckonTime = false;
        //Real time inventory locking operation.
        private bool m_bLockTab = false;
        //Whether display the serial monitoring data.
        private bool m_bDisplayLog = false;
        const short rwTagDelay = 300; //read/write RFID tag delay
        private int m_nTotal = 0;
        //Frequency of list updating.
        private int m_nRealRate = 4;
        //Record quick poll antenna parameter.
        private byte[] m_btAryData = new byte[18];
        private byte[] m_btAryData_4 = new byte[10];
        //Record the total number of quick poll times.
        //private int m_nSwitchTotal = 0;
        //private int m_nSwitchTime = 0;
        private int m_nReceiveFlag_200ms = 0;
        private int m_nReceiveFlag_1s = 0;

        private volatile bool m_nPhaseOpened = false;
        private volatile bool m_nSessionPhaseOpened = false;
        List<RFIDTagData> tagLists = new List<RFIDTagData>();

        int readTagRetry = 0;
        bool bVerify = false;

        byte btMemBank = 0;
        byte btWordAddr = 0;
        byte btWordCnt = 0;
        string strHEXdata = "";
        string xmlEPCTag = "";
        
        static bool bReaderListUpdate = false;
        private string sourceFilePath = "";
        private XmlRPCTagStatus xmlRpcStatus;
        enum XmlRPCTagStatus
        {
            Ready,
            ReadID,
            WriteID,
            WriteData
        }

        public RFIDTagIDForm(LoginForm login)
        {
            loginForm = login;
            reader = new Reader.ReaderMethod();
            reader.AnalyCallback = AnalyData;
            sourceFilePath = Path.GetDirectoryName(new Uri(this.GetType().Assembly.GetName().CodeBase).LocalPath);

            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void RFIDTagIDForm_Load(object sender, EventArgs e)
        {
            byte[] labelFormat = Properties.Resources.LabelFormat;
            RFIDTagInfo.loadLabelFormat(labelFormat);
            
            if(checkBoxShowDetail.Checked)
            {
                splitContainer1.SplitterDistance = 300;
            }
            else
            {
                splitContainer1.SplitterDistance = 0;
            }

            checkComPort();
            if (iComPortStatus == 1)
            {
                reader.GetFirmwareVersion(m_curSetting.btReadId);                        
                initScanTag();
            }
        }

        public bool xmlLogin(string url, string dbName, string usr, string pwd)
        {
            try
            {
                var engine = Python.CreateEngine();
                ICollection<string> Paths = engine.GetSearchPaths();
                Paths.Add(@"C:\Program Files\IronPython 2.7\Lib");
                engine.SetSearchPaths(Paths);
                                
                dynamic py = engine.ExecuteFile(sourceFilePath + @"\reference\xml_rpc.py");
                xmlRpc = py.XmlRpc();
                xmlRpc.isRFIDConnected(true);
                xmlRpc.login(url, dbName, usr, pwd);
                xmlRpcStatus = XmlRPCTagStatus.Ready;
#if enableWriteControl
                if (!xmlRpc.getUserWriteTagPrivilege())
                {
                    tabCtrMain.TabPages.Remove(pageEpcID);
                }
#endif
            }
            catch (Exception exp) {
                MessageBox.Show("Login to Odoo failed " + exp.Message);
                return false;
            }
            return true;
        }

        public void checkPort(string comPort)
        {
            string strException = string.Empty;

            int nRet = reader.OpenCom(comPort, Convert.ToInt32("115200"), out strException);
            if (nRet != 0)
            {
                WriteLog(lrtxtLog, "Connection failed, failure cause: " + strException, 1);
                return;
            }
            else
            {
                WriteLog(lrtxtLog, "Connect " + comPort + "@" + "115200", 0);
            }
            Thread.Sleep(rwTagDelay);
            reader.resetCom();
            Thread.Sleep(rwTagDelay);

            reader.GetFirmwareVersion(m_curSetting.btReadId);
        }

        public void checkComPort()
        {
            //load serial port
            string[] serialPort = SerialPort.GetPortNames();
            int i = serialPort.Length - 1;

            do
            {
                try
                {
                    if (iComPortStatus != 1)
                    {
                        string comPort = serialPort[i];
                        checkPort(comPort);
                        Thread.Sleep(rwTagDelay * 3);
                    }
                }
                catch (Exception exp) { WriteLog(lrtxtLog, "Error, can't connect to any Com Port", 1); }
            }
            while ((iComPortStatus != 1) && (i-- > 0));

            if (iComPortStatus == 1)
            {
                DateTime dateTimeNow = DateTime.Now;
                WriteLog(lrtxtLog, "Message start: " + dateTimeNow.ToString(), 0);
                WriteLog(lrtxtLog, "Com port found, Load setting", 0);
                loadSetting();
            }
            else
            {
                WriteLog(lrtxtLog, "Error, can't connect to any Com Port", 1);
            }
        }

        public void loadSetting()
        { //R2000UartDemo_Load //C:\ProgramData
            //load Access Code
            byte[] accessCode = Properties.Resources.AccessCode;
            string strCode = RFIDTagInfo.ASCIIToHex(Encoding.ASCII.GetString(accessCode)).ToUpper();
            symmetric.loadAccessCode(strCode);
            //Trace.WriteLine("Read Access code " + strCode);

            //load key
            byte[] encryptKey = Properties.Resources.SymmetricKey;
            symmetric.loadKey(encryptKey);

            //load LabelFormat":
            byte[] labelFormat = Properties.Resources.LabelFormat;
            RFIDTagInfo.loadLabelFormat(labelFormat);

            //init scan tag buffer
            m_curInventoryBuffer.ClearInventoryPar();
            m_curInventoryBuffer.btRepeat = 1; //1
            m_curInventoryBuffer.bLoopCustomizedSession = false;

            m_bInventory = true;
            m_curInventoryBuffer.bLoopInventory = true;
            m_curInventoryBuffer.bLoopInventoryReal = true;
            m_curInventoryBuffer.ClearInventoryResult();

            tagLists.Clear();
            m_nTotal = 0;
            reader.SetWorkAntenna(m_curSetting.btReadId, 0x00);
            m_curSetting.btWorkAntenna = 0x00;
            return;
        }

        private void initScanTag()
        {
            m_curInventoryBuffer.ClearInventoryPar();
            m_curInventoryBuffer.btRepeat = 1; //1
            m_curInventoryBuffer.bLoopCustomizedSession = false;

            m_bInventory = true;
            m_curInventoryBuffer.bLoopInventory = true;
            m_curInventoryBuffer.bLoopInventoryReal = true;
            m_curInventoryBuffer.ClearInventoryRealResult();

            //lvRealList.Items.Clear();
            //tbRealMaxRssi.Text = "0";
            //tbRealMinRssi.Text = "0";

            m_nTotal = 0;
            reader.SetWorkAntenna(m_curSetting.btReadId, 0x00);
            m_curSetting.btWorkAntenna = 0x00;
            timerInventory.Enabled = true;

            RFIDTagInfo.label = "";
            RFIDTagInfo.tagInfo = "";
            RFIDTagInfo.reserverData = "";
            
        }

        private void AnalyData(Reader.MessageTran msgTran)
        {
            m_nReceiveFlag_200ms = 0;
            if (msgTran.PacketType != 0xA0)
            {
                return;
            }
            switch (msgTran.Cmd)
            {
                case 0x72:
                    ProcessGetFirmwareVersion(msgTran);
                    break;
                case 0x61:
                    ProcessWriteGpioValue(msgTran);
                    break;
                case 0x81:
                    ProcessReadTag(msgTran);
                    break;
                case 0x82:
                case 0x94:
                    ProcessWriteTag(msgTran);
                    break;
                case 0x83:
                    ProcessLockTag(msgTran);
                    break;
                case 0x84:
                    ProcessKillTag(msgTran);
                    break;
                case 0x85:
                    ProcessSetAccessEpcMatch(msgTran);
                    break;
                case 0x86:
                    //ProcessGetAccessEpcMatch(msgTran);
                    break;

                case 0x89:
                case 0x8B:
                    ProcessInventoryReal(msgTran); //need
                    break;

                default:
                    break;
            }
        }

        private delegate void WriteLogUnSafe(CustomControl.LogRichTextBox logRichTxt, string strLog, int nType);
        public void WriteLog(CustomControl.LogRichTextBox logRichTxt, string strLog, int nType)
        {
            if (this.InvokeRequired)
            {
                WriteLogUnSafe InvokeWriteLog = new WriteLogUnSafe(WriteLog);
                this.Invoke(InvokeWriteLog, new object[] { logRichTxt, strLog, nType });
            }
            else
            {
                if (nType == 0)
                {
                    logRichTxt.AppendTextEx(strLog, Color.Indigo);
                }
                else
                {
                    logRichTxt.AppendTextEx(strLog, Color.Red);
                }

                if (ckClearOperationRec.Checked)
                {
                    if (logRichTxt.Lines.Length > 50)
                    {
                        logRichTxt.Clear();
                    }
                }

                logRichTxt.Select(logRichTxt.TextLength, 0);
                logRichTxt.ScrollToCaret();
            }
        }

        private delegate void RefreshInventoryRealUnsafe(byte btCmd);
        private void RefreshInventoryReal(byte btCmd)
        {//update Tag info
            if (this.InvokeRequired)
            {
                RefreshInventoryRealUnsafe InvokeRefresh = new RefreshInventoryRealUnsafe(RefreshInventoryReal);
                this.Invoke(InvokeRefresh, new object[] { btCmd });
            }
            else
            {
                switch (btCmd)
                {//scanning tag
                    case 0x89:
                    case 0x8B:
                        {
                            //int nTagCount = m_curInventoryBuffer.dtTagTable.Rows.Count;
                            int nTotalRead = m_nTotal;// m_curInventoryBuffer.dtTagDetailTable.Rows.Count;
                            TimeSpan ts = m_curInventoryBuffer.dtEndInventory - m_curInventoryBuffer.dtStartInventory;
                            int nTotalTime = ts.Minutes * 60 * 1000 + ts.Seconds * 1000 + ts.Milliseconds;


                            if (m_nTotal % m_nRealRate == 1)
                            {//update item info here
                                //string strEPC = "";
                                foreach (DataRow row in m_curInventoryBuffer.dtTagTable.Rows)
                                {
                                    //row[0] PC
                                    //row[2] EPC serial number
                                    //row[4] RSSI (value - 129)dBm
                                    //row[6] frequency
                                    //row[7] identification count
                                    int index = -1;
                                    if (tagLists.Count > 0)
                                    {//update tag info
                                        index = findTagIndex(row[2].ToString().Trim());                                       
                                    }
                                    if (index != -1)
                                    {
                                        tagLists[index].readCount = Convert.ToInt32(row[7]);
                                        tagLists[index].rssi = Convert.ToInt32(row[4]);
                                    }
                                    else
                                    {
                                        RFIDTagData tagData = new RFIDTagData();

                                        tagData.EPC_ID = row[2].ToString();
                                        RFIDTagInfo.readEPCLabel(tagData.EPC_ID, out tagData.EPC_PS_Num);

                                        if (tagData.EPC_ID.StartsWith(" "))
                                            tagData.EPC_ID = tagData.EPC_ID.Remove(0, 1) + " ";

                                        tagData.readCount = Convert.ToInt32(row[7]);
                                        tagData.rssi = Convert.ToInt32(row[4]);

                                        if(tagData.EPC_PS_Num > 0)
                                        {
                                            tagData.tagStatus = RFIDTagData.TagStatus.IDUpdated;
                                        }
                                        else
                                        {
                                            tagData.tagStatus = RFIDTagData.TagStatus.IDNotUpdate;
                                        }                                      
                                        tagLists.Add(tagData);

                                        checkTagStatus(tagData.EPC_ID);

                                        bool bExisted = false;                                  
                                        foreach (ListViewItem item in listViewEPCTag.Items)
                                        {
                                            if (item.SubItems[0].Text == tagData.EPC_ID)
                                            {//existed 
                                                bExisted = true;
                                                break;
                                            }
                                        }
                                        if (!bExisted)
                                        {
                                            ListViewItem epcTag = new ListViewItem(new[] {  tagData.EPC_ID,
                                                                                        tagData.readCount.ToString() });
                                            listViewEPCTag.Items.Add(epcTag);
                                        }
                                    }

                                    //update count info
                                    foreach (ListViewItem item in listViewEPCTag.Items)
                                    {
                                        for (int j = 0; j < tagLists.Count; j++)
                                        {
                                            if (item.SubItems[0].Text == tagLists[j].EPC_ID)
                                            {
                                                if (tagLists[j].readCount > 0)
                                                {
                                                    listViewEPCTag.BeginUpdate();
                                                    if ((tabCtrMain.SelectedTab == pageEpcID) &&
                                                        ((tagLists[j].tagStatus == RFIDTagData.TagStatus.AccessCodeUpdated) ||
                                                         (tagLists[j].tagStatus == RFIDTagData.TagStatus.DataNotUpdate)))
                                                    {
                                                        item.ForeColor = Color.Green;
                                                        tBEPCTagVerify.ForeColor = SystemColors.Control;
                                                        tBEPCTagVerify.BackColor = SystemColors.Control;
                                                        tBAccessCodeVerify.ForeColor = SystemColors.Control;
                                                        tBAccessCodeVerify.BackColor = SystemColors.Control;
                                                        tBDataVerify.ForeColor = SystemColors.Control;
                                                        tBDataVerify.BackColor = SystemColors.Control;
                                                    }
                                                    else if((tabCtrMain.SelectedTab == pageData) &&
                                                            (tagLists[j].tagStatus == RFIDTagData.TagStatus.IDUpdated))
                                                    {
                                                        selectTag(tagLists[j].EPC_ID);
                                                        checkTagStatus(tagLists[j].EPC_ID);
                                                        Thread.Sleep(rwTagDelay * 5);
                                                    }
                                                    item.SubItems[1].Text = tagLists[j].readCount.ToString();
                                                    listViewEPCTag.EndUpdate();
                                                }
                                            }
                                        }
                                    }
                                }
                                bReaderListUpdate = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private delegate void RunLoopInventoryUnsafe();
        private void RunLoopInventroy()
        {
            if (this.InvokeRequired)
            {
                RunLoopInventoryUnsafe InvokeRunLoopInventory = new RunLoopInventoryUnsafe(RunLoopInventroy);
                this.Invoke(InvokeRunLoopInventory, new object[] { });
            }
            else
            {
                //校验盘存是否所有天线均完成
                if (m_curInventoryBuffer.nIndexAntenna < m_curInventoryBuffer.lAntenna.Count - 1 || m_curInventoryBuffer.nCommond == 0)
                {
                    if (m_curInventoryBuffer.nCommond == 0)
                    {
                        m_curInventoryBuffer.nCommond = 1;

                        if (m_curInventoryBuffer.bLoopInventoryReal)
                        {
                            //m_bLockTab = true;
                            //btnInventory.Enabled = false;
                            if (m_curInventoryBuffer.bLoopCustomizedSession)//自定义Session和Inventoried Flag 
                            {
                                //reader.CustomizedInventory(m_curSetting.btReadId, m_curInventoryBuffer.btSession, m_curInventoryBuffer.btTarget, m_curInventoryBuffer.btRepeat); 
                                reader.CustomizedInventoryV2(m_curSetting.btReadId, m_curInventoryBuffer.CustomizeSessionParameters.ToArray());
                            }
                            else
                            {//enter here
                                reader.InventoryReal(m_curSetting.btReadId, m_curInventoryBuffer.btRepeat);

                            }
                        }
                        else
                        {
                            if (m_curInventoryBuffer.bLoopInventory)
                                reader.Inventory(m_curSetting.btReadId, m_curInventoryBuffer.btRepeat);
                        }
                    }
                    else
                    {
                        m_curInventoryBuffer.nCommond = 0;
                        m_curInventoryBuffer.nIndexAntenna++;

                        byte btWorkAntenna = 0; //m_curInventoryBuffer.lAntenna[m_curInventoryBuffer.nIndexAntenna];
                        reader.SetWorkAntenna(m_curSetting.btReadId, btWorkAntenna); //need to use
                        m_curSetting.btWorkAntenna = btWorkAntenna;
                    }
                }
                else if (m_curInventoryBuffer.bLoopInventory)
                {//enter here
                    m_curInventoryBuffer.nIndexAntenna = 0;
                    m_curInventoryBuffer.nCommond = 0;

                    byte btWorkAntenna = 0; // m_curInventoryBuffer.lAntenna[m_curInventoryBuffer.nIndexAntenna];
                    reader.SetWorkAntenna(m_curSetting.btReadId, btWorkAntenna);//need to use
                    m_curSetting.btWorkAntenna = btWorkAntenna;
                }
            }
            Thread.Sleep(20);
        }

        string GetFreqString(byte btFreq)
        {
            string strFreq = string.Empty;

            if (m_curSetting.btRegion == 4)
            {
                float nExtraFrequency = btFreq * m_curSetting.btUserDefineFrequencyInterval * 10;
                float nstartFrequency = ((float)m_curSetting.nUserDefineStartFrequency) / 1000;
                float nStart = nstartFrequency + nExtraFrequency / 1000;
                string strTemp = nStart.ToString("0.000");
                return strTemp;
            }
            else
            {
                if (btFreq < 0x07)
                {
                    float nStart = 865.00f + Convert.ToInt32(btFreq) * 0.5f;
                    string strTemp = nStart.ToString("0.00");
                    return strTemp;
                }
                else
                {
                    float nStart = 902.00f + (Convert.ToInt32(btFreq) - 7) * 0.5f;
                    string strTemp = nStart.ToString("0.00");
                    return strTemp;
                }
            }
        }

        private void ProcessGetFirmwareVersion(Reader.MessageTran msgTran)
        {
            string strCmd = "Get Reader's firmware version";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 2)
            {
                m_curSetting.btMajor = msgTran.AryData[0];
                m_curSetting.btMinor = msgTran.AryData[1];
                m_curSetting.btReadId = msgTran.ReadId;

                iComPortStatus = 1;
                WriteLog(lrtxtLog, strCmd + m_curSetting.btMajor + "." + m_curSetting.btMinor, 0);
                return;
            }
            else if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
            }
            else
            {
                strErrorCode = "Unknown Error";
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog, strLog, 1);
        }
#if false
        private void ProcessSetUartBaudrate(Reader.MessageTran msgTran)
        {
            string strCmd = "Set Baudrate";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    WriteLog(lrtxtLog, strCmd, 0);

                    return;
                }
                else
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                }
            }
            else
            {
                strErrorCode = "Unknown Error";
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog, strLog, 1);
        }
#endif
        private void ProcessWriteGpioValue(Reader.MessageTran msgTran)
        {
            string strCmd = "Set GPIO status";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    //WriteLog(lrtxtLog,strCmd,0);
                    return;
                }
                else
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                }
            }
            else
            {
                strErrorCode = "Unknown Error";
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog, strLog, 1);
        }

        //private static bool bLEDOn = false;
        private void ProcessInventoryReal(Reader.MessageTran msgTran)
        {
            string strCmd = "";
            if (msgTran.Cmd == 0x89)
            {
                strCmd = "Real time inventory scan";
            }
            if (msgTran.Cmd == 0x8B)
            {
                strCmd = "User define Session and Inventoried Flag inventory";
            }
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + " Failure, failure cause: " + strErrorCode;
                WriteLog(lrtxtLog, strLog, 1);

                m_curInventoryBuffer.dtEndInventory = DateTime.Now;

                RefreshInventoryReal(0x89);
                if (m_bInventory)
                {
                    RunLoopInventroy();
                }
            }
            else if (msgTran.AryData.Length == 7)
            {
                m_curInventoryBuffer.nReadRate = Convert.ToInt32(msgTran.AryData[1]) * 256 + Convert.ToInt32(msgTran.AryData[2]);
                m_curInventoryBuffer.nDataCount = Convert.ToInt32(msgTran.AryData[3]) * 256 * 256 * 256 + Convert.ToInt32(msgTran.AryData[4]) * 256 * 256 + Convert.ToInt32(msgTran.AryData[5]) * 256 + Convert.ToInt32(msgTran.AryData[6]);

                m_curInventoryBuffer.dtEndInventory = DateTime.Now;

                RefreshInventoryReal(0x89);

                if (m_bInventory)
                {
                    RunLoopInventroy();
                }
                Thread.Sleep(20);
                //WriteLog(lrtxtLog,strCmd);
            }
            else
            {
                m_nTotal++;
                int nLength = msgTran.AryData.Length;

                int nEpcLength = nLength - 4;
                if (m_nSessionPhaseOpened)
                {
                    nEpcLength = nLength - 6;
                }

                //Add inventory list
                string strEPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 3, nEpcLength);
                string strPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 1, 2);
                string strRSSI = string.Empty;

                if (m_nSessionPhaseOpened)
                {
                    //SetMaxMinRSSI(Convert.ToInt32(msgTran.AryData[nLength - 3] & 0x7F));
                    strRSSI = (msgTran.AryData[nLength - 3] & 0x7F).ToString();
                }
                else
                {
                    //SetMaxMinRSSI(Convert.ToInt32(msgTran.AryData[nLength - 1] & 0x7F));
                    strRSSI = (msgTran.AryData[nLength - 1] & 0x7F).ToString();
                }

                byte btTemp = msgTran.AryData[0];
                byte btAntId = (byte)((btTemp & 0x03) + 1);
                string strPhase = string.Empty;
                if (m_nSessionPhaseOpened)
                {
                    if ((msgTran.AryData[nLength - 3] & 0x80) != 0) btAntId += 4;
                    strPhase = CCommondMethod.ByteArrayToString(msgTran.AryData, nLength - 2, 2);
                }
                else
                {
                    if ((msgTran.AryData[nLength - 1] & 0x80) != 0) btAntId += 4;
                }

                m_curInventoryBuffer.nCurrentAnt = (int)btAntId;
                string strAntId = btAntId.ToString();
                byte btFreq = (byte)(btTemp >> 2);

                string strFreq = GetFreqString(btFreq);

                DataRow[] drs = m_curInventoryBuffer.dtTagTable.Select(string.Format("COLEPC = '{0}'", strEPC));
                if (drs.Length == 0)
                {
                    DataRow row1 = m_curInventoryBuffer.dtTagTable.NewRow();
                    row1[0] = strPC;
                    row1[2] = strEPC;
                    row1[4] = strRSSI;
                    row1[5] = "1";
                    row1[6] = strFreq;
                    row1[7] = "0";
                    row1[8] = "0";
                    row1[9] = "0";
                    row1[10] = "0";
                    row1[11] = "0";
                    row1[12] = "0";
                    row1[13] = "0";
                    row1[14] = "0";
                    row1[15] = strPhase;
                    m_curInventoryBuffer.dtTagTable.Rows.Add(row1);
                    m_curInventoryBuffer.dtTagTable.AcceptChanges();
                }
                else
                {
                    foreach (DataRow dr in drs)
                    {
                        dr.BeginEdit();
                        dr[4] = strRSSI;
                        dr[5] = (Convert.ToInt32(dr[5]) + 1).ToString();
                        dr[6] = strFreq;
                        dr[7] = (Convert.ToInt32(dr[7]) + 1).ToString();
                        dr[15] = strPhase;
                        dr.EndEdit();
                    }
                    m_curInventoryBuffer.dtTagTable.AcceptChanges();
                }
                m_curInventoryBuffer.dtEndInventory = DateTime.Now;

                RefreshInventoryReal(0x89);
                Thread.Sleep(20);
            }
        }


        private bool verifyTag(string label)
        {
            string decryptMsg = symmetric.DecryptFromHEX(label);
            //WriteLog(lrtxtLog, "Decrypt data " + decryptMsg + ", Size " + decryptMsg.Length, 0);
            RFIDTagInfo.tagInfo = decryptMsg;
            
            //WriteLog(lrtxtLog, "Verify label read " + label, 0);
            return RFIDTagInfo.verifyData(decryptMsg);
        }

        private string lookupTable(ulong labelNum, string data)
        {
            string data1 = data;
            if (labelNum < 99999999999999)
            {
                if (data.Length > 120)
                {
                    data1 = data.Substring(0, 120);
                }
            }
            return data1;
        }

        private string lookupTable(string label, string data)
        {
            ulong labelNum = 0;
            string data1 = data;

            RFIDTagInfo.readEPCLabel(label, out labelNum);

            if (labelNum < 99999999999999)
            {
                if (data.Length > 120)
                {
                    data1 = data.Substring(0, 120);
                }
            }
            return data1;
        }

        private int findTagIndex(string RFIDTagID)
        {
            int index = -1;
            for (int i = 0; i < tagLists.Count; i++)
            {
                if (tagLists[i].EPC_ID.Trim().Contains(RFIDTagID))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private delegate void ProcessReadTagUnsafe(Reader.MessageTran msgTran);
        private void ProcessReadTag(Reader.MessageTran msgTran)
        {
            if (this.InvokeRequired)
            {
                ProcessReadTagUnsafe InvokeRefresh = new ProcessReadTagUnsafe(ProcessReadTag);
                this.Invoke(InvokeRefresh, new object[] { msgTran });
            }
            else
            {
                string strCmd = "Read Tag";
                string strErrorCode = string.Empty;

                try
                {
                    //WriteLog(lrtxtLog,"Read Tag, AryData.Length " + msgTran.AryData.Length, 0);
                    if (msgTran.AryData.Length == 1)
                    {
                        strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                        string strLog = strCmd + " Failure, read tag failure cause1: " + strErrorCode;

                        if (readTagRetry++ < rwTagRetryMAX)
                        {
                            byte[] btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
                            if (RFIDTagInfo.accessCode == null)
                            {
                                Array.Clear(btAryPwd, 0, btAryPwd.Length);
                            }
                            RFIDTagInfo.accessCode = btAryPwd;
                            if (btWordCnt == 2) btWordCnt = 4;
                            reader.ReadTag(m_curSetting.btReadId, btMemBank, btWordAddr, btWordCnt, btAryPwd);
                            Thread.Sleep(rwTagDelay);
                            //setVerifiedLEDStatus(false, true); //red on
                            WriteLog(lrtxtLog, "Read Tag retry " + readTagRetry, 1);
                        }
                        else
                        {// read tag failed
                            //setVerifiedLEDStatus(0, 1); //red on
                            readTagRetry = 0;
                        }
                    }
                    else
                    {
                        int nLen = msgTran.AryData.Length;
                        int nDataLen = Convert.ToInt32(msgTran.AryData[nLen - 3]);
                        int nEpcLen = Convert.ToInt32(msgTran.AryData[2]) - nDataLen - 4;

                        string strPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 3, 2);
                        string strEPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 5, nEpcLen);
                        string strCRC = CCommondMethod.ByteArrayToString(msgTran.AryData, 5 + nEpcLen, 2);
                        string strData = CCommondMethod.ByteArrayToString(msgTran.AryData, 7 + nEpcLen, nDataLen);

                        byte byTemp = msgTran.AryData[nLen - 2];
                        byte byAntId = (byte)((byTemp & 0x03) + 1);
                        string strAntId = byAntId.ToString();
                        string strReadCount = msgTran.AryData[nLen - 1].ToString();

                        ulong uWordCnt = 0;
                        RFIDTagInfo.label = RFIDTagInfo.readEPCLabel(strEPC, out uWordCnt);
                        strData = lookupTable(uWordCnt, strData);
                        if (strData == null || strData == "")
                            return;

                        if (RFIDTagInfo.verifyData(RFIDTagInfo.label))
                        {//verify tag
                            if (tBEPCTagVerify.ForeColor != Color.Green)
                            {
                                tBEPCTagVerify.ForeColor = Color.Green;
                                tBEPCTagVerify.BackColor = SystemColors.Control;                               
                            }
                        }
                        else
                        {
                            if (tBEPCTagVerify.ForeColor != Color.Red)
                            {
                                tBEPCTagVerify.ForeColor = Color.Red;
                                tBEPCTagVerify.BackColor = SystemColors.Control;                           
                            }
                        }
                        

                        int index = findTagIndex(strEPC.Trim());
                        strData = lookupTable(strEPC, strData);

                        if (strData.Length / 3 == 4 || strData.Length / 3 == 8)
                        {
                            if (RFIDTagInfo.bAccessCode(symmetric.readAccessCode(), strData))
                            {//verify access code       
                                RFIDTagInfo.reserverData = strData;

                                if (tabCtrMain.SelectedTab == pageEpcID)
                                {
                                    tBAccessCodeVerify.ForeColor = Color.Green;
                                    tBAccessCodeVerify.BackColor = SystemColors.Control;
                                }

                                //update tag status for ready or locked                                
                                if ((index != -1) && (tagLists[index].tagStatus != RFIDTagData.TagStatus.DataUpdated))
                                {
                                    tagLists[index].tagStatus = RFIDTagData.TagStatus.AccessCodeUpdated;

                                    btMemBank = 3;
                                    btWordAddr = 0;
                                    btWordCnt = 30;
                                    reader.ReadTag(m_curSetting.btReadId, btMemBank, btWordAddr, btWordCnt, RFIDTagInfo.accessCode);
                                    Thread.Sleep(rwTagDelay * 2);
                                    WriteLog(lrtxtLog, "*** Verified Access Code Successful ***" + tagLists[index].EPC_ID, 0);
                                    //labelUpdateStatus.Text = "Reading Tag, accesscode is corrected";
                                }
                            }
                            else
                            {
                                if (tabCtrMain.SelectedTab == pageEpcID)
                                {
                                    tBAccessCodeVerify.ForeColor = Color.Red;
                                    tBAccessCodeVerify.BackColor = SystemColors.Control;
                                }

                                if (index != -1 && 
                                   ((tagLists[index].tagStatus != RFIDTagData.TagStatus.IDNotUpdate) ||
                                    (tagLists[index].tagStatus != RFIDTagData.TagStatus.DataUpdated)))
                                {
                                    tagLists[index].tagStatus = RFIDTagData.TagStatus.AccessCodeNotUpdate;
                                    WriteLog(lrtxtLog, "*** Verified Access Code Failed ***" + tagLists[index].EPC_ID, 1);
                                    labelUpdateStatus.Text = "Tag access code incorrected, retry!";
                                }                              
                            }
                        }
                        else if (strData.Length / 3 >= 40)
                        {
                            string reserveData = "";
                            if (RFIDTagInfo.reserverData != null && RFIDTagInfo.reserverData != "")
                            {
                                reserveData = RFIDTagInfo.reserverData.Substring(0, 12);
                            }
                            else
                            {
                                WriteLog(lrtxtLog, "Add reserve data first!! ", 1);
                                return;
                            }

                            bVerify = verifyTag(reserveData.ToUpper() + strData);
                            if (bVerify)
                            {
                                foreach (ListViewItem item in listViewEPCTag.Items)
                                {
                                    if (item.SubItems[0].Text.Trim().Contains(strEPC.Trim()))
                                    {
                                        item.ForeColor = Color.LightGray;
                                        //item.BackColor = SystemColors.Control;
                                        break;
                                    }
                                }
                                //udpate tag status for done or content updated
                                if (index != -1)
                                {
                                    tagLists[index].tagStatus = RFIDTagData.TagStatus.DataUpdated;
                                    WriteLog(lrtxtLog, "*** Verified Data Successful ***" + tagLists[index].EPC_ID, 0);
                                    if (xmlRpcStatus == XmlRPCTagStatus.WriteData)
                                    {
                                        xmlRpc.writeRFIDTagSuccessful(true);
                                        labelUpdateStatus.Text = "Update Successful, Move to next RFID Tag";
                                        tBDataVerify.ForeColor = Color.Green;
                                        tBDataVerify.BackColor = SystemColors.Control;
                                    }
                                }
                            }
                            else if ((index != -1) &&
                                     (tagLists[index].tagStatus != RFIDTagData.TagStatus.DataUpdated))
                            {
                                tagLists[index].tagStatus = RFIDTagData.TagStatus.DataNotUpdate;
                                WriteLog(lrtxtLog, "verified failed, retry now " + tagLists[index].EPC_ID, 1);
                                if (xmlRpcStatus == XmlRPCTagStatus.WriteData)
                                {
                                    xmlRpc.writeRFIDTagSuccessful(false);
                                    labelUpdateStatus.Text = "Tag Data is incorrcted, retry!";
                                    tBDataVerify.ForeColor = Color.Red;
                                    tBDataVerify.BackColor = SystemColors.Control;
                                }
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    //setVerifiedLEDStatus(0, 1); //red on      
                    WriteLog(lrtxtLog, strCmd + " got error " + exp.Message, 1);
                    //need to move to scan next tag here
                }
            }
        }

        private void writeTag()
        {
            byte[] btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);            
            byte[] btAryWriteData = CCommondMethod.String2ByteArray(strHEXdata.ToUpper(), 2, out btWordCnt);
            reader.WriteTag(m_curSetting.btReadId, btAryPwd, btMemBank, btWordAddr, btWordCnt, btAryWriteData, 0x94);
        }

        private int WriteTagCount = 0;
        private const int rwTagRetryMAX = 5;
        static int writeTagRetry = 0;
        //private string ellapsed;
        private void ProcessWriteTag(Reader.MessageTran msgTran)
        {
            string strCmd = "Write Tag";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + " Failure, failure cause1: " + strErrorCode;

                WriteLog(lrtxtLog, strLog, 1);
                if (writeTagRetry++ < rwTagRetryMAX)
                {
                    writeTag();
                    Thread.Sleep(rwTagDelay);
                    WriteLog(lrtxtLog, "Write Tag retry " + writeTagRetry, 1);
                }
                else
                {//write tag failed
                    //setVerifiedLEDStatus(0, 1); //red on 
                    writeTagRetry = 0;
                }
            }
            else
            {
                int nLen = msgTran.AryData.Length;
                int nEpcLen = Convert.ToInt32(msgTran.AryData[2]) - 4;

                if (msgTran.AryData[nLen - 3] != 0x10)
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[nLen - 3]);
                    string strLog = strCmd + " Failure, failure cause2: " + strErrorCode;
                    WriteLog(lrtxtLog, strLog, 1);
                    return;
                }
                WriteTagCount++;

                string strPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 3, 2);
                string strEPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 5, nEpcLen);
                string strCRC = CCommondMethod.ByteArrayToString(msgTran.AryData, 5 + nEpcLen, 2);
                string strData = string.Empty;

                byte byTemp = msgTran.AryData[nLen - 2];
                byte byAntId = (byte)((byTemp & 0x03) + 1);
                string strAntId = byAntId.ToString();
                string strReadCount = msgTran.AryData[nLen - 1].ToString();

                DataRow row = m_curOperateTagBuffer.dtTagTable.NewRow();
                row[0] = strPC;
                row[1] = strCRC;
                row[2] = strEPC;
                row[3] = strData;
                row[4] = string.Empty;
                row[5] = strAntId;
                row[6] = strReadCount;

                m_curOperateTagBuffer.dtTagTable.Rows.Add(row);
                m_curOperateTagBuffer.dtTagTable.AcceptChanges();

                //RefreshOpTag(0x82);
                WriteLog(lrtxtLog, strCmd, 0);

                if (bVerify)
                {
                    bVerify = false;
                }

                if (WriteTagCount == (msgTran.AryData[0] * 256 + msgTran.AryData[1]))
                {
                    WriteTagCount = 0;
                }
            }
        }
        
        private void ProcessSetAccessEpcMatch(Reader.MessageTran msgTran)
        {
            string strCmd = "Select/Deselect Tag";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    WriteLog(lrtxtLog, strCmd, 0);
                    return;
                }
                else
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                }
            }
            else
            {
                strErrorCode = "Unknown Error";
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog, strLog, 1);
        }

        private void ProcessLockTag(Reader.MessageTran msgTran)
        {
            string strCmd = "Lock Tag";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                WriteLog(lrtxtLog, strLog, 1);
            }
            else
            {
                int nLen = msgTran.AryData.Length;
                int nEpcLen = Convert.ToInt32(msgTran.AryData[2]) - 4;

                if (msgTran.AryData[nLen - 3] != 0x10)
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[nLen - 3]);
                    string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                    WriteLog(lrtxtLog, strLog, 1);
                    return;
                }

                string strPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 3, 2);
                string strEPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 5, nEpcLen);
                string strCRC = CCommondMethod.ByteArrayToString(msgTran.AryData, 5 + nEpcLen, 2);
                string strData = string.Empty;

                byte byTemp = msgTran.AryData[nLen - 2];
                byte byAntId = (byte)((byTemp & 0x03) + 1);
                string strAntId = byAntId.ToString();

                string strReadCount = msgTran.AryData[nLen - 1].ToString();

                DataRow row = m_curOperateTagBuffer.dtTagTable.NewRow();
                row[0] = strPC;
                row[1] = strCRC;
                row[2] = strEPC;
                row[3] = strData;
                row[4] = string.Empty;
                row[5] = strAntId;
                row[6] = strReadCount;

                m_curOperateTagBuffer.dtTagTable.Rows.Add(row);
                m_curOperateTagBuffer.dtTagTable.AcceptChanges();

                //RefreshOpTag(0x83);
                WriteLog(lrtxtLog, strCmd, 0);
            }
        }

        private void ProcessKillTag(Reader.MessageTran msgTran)
        {
            string strCmd = "Kill Tag";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                WriteLog(lrtxtLog, strLog, 1);
            }
            else
            {
                int nLen = msgTran.AryData.Length;
                int nEpcLen = Convert.ToInt32(msgTran.AryData[2]) - 4;

                if (msgTran.AryData[nLen - 3] != 0x10)
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[nLen - 3]);
                    string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                    WriteLog(lrtxtLog, strLog, 1);
                    return;
                }

                string strPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 3, 2);
                string strEPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 5, nEpcLen);
                string strCRC = CCommondMethod.ByteArrayToString(msgTran.AryData, 5 + nEpcLen, 2);
                string strData = string.Empty;

                byte byTemp = msgTran.AryData[nLen - 2];
                byte byAntId = (byte)((byTemp & 0x03) + 1);
                string strAntId = byAntId.ToString();

                string strReadCount = msgTran.AryData[nLen - 1].ToString();

                DataRow row = m_curOperateTagBuffer.dtTagTable.NewRow();
                row[0] = strPC;
                row[1] = strCRC;
                row[2] = strEPC;
                row[3] = strData;
                row[4] = string.Empty;
                row[5] = strAntId;
                row[6] = strReadCount;

                m_curOperateTagBuffer.dtTagTable.Rows.Add(row);
                m_curOperateTagBuffer.dtTagTable.AcceptChanges();

                //RefreshOpTag(0x84);
                WriteLog(lrtxtLog, strCmd, 0);
            }
        }

        private void timerInventory_Tick(object sender, EventArgs e)
        {
            m_nReceiveFlag_200ms++;
            if (m_nReceiveFlag_200ms >= 5)
            {//1s
                RunLoopInventroy();
                m_nReceiveFlag_200ms = 0;

                if(m_nReceiveFlag_1s++ > 60)
                {//1 minute, reflash table
                    listViewEPCTag.Items.Clear();
                    listViewEPCTag.Refresh();
                    tagLists.Clear();
                    initScanTag();
                    m_nReceiveFlag_1s = 0;
                }
                try
                {
                    if (bReaderListUpdate && xmlRpc != null)
                    {
                        bReaderListUpdate = false;
                        if (xmlRpc.readyToUpdateTag())
                        {
                            if (tabCtrMain.SelectedTab == pageData)
                            {
                                udpateTagData();
                            }
                            else
                            {
                                udpateTagID();

                                if (checkBoxUpdateData.Checked == true)
                                {
                                    udpateTagData();
                                }
                            }
                        }  
                    }
                    else if(xmlRpc == null)
                    {//check tag status only
                        labelUpdateStatus.Text = "Cloud can't connect, check the connection!";

                        int maxCountIndex = findTag(true);
                        if (maxCountIndex < 0) return;
                        checkTagStatus(tagLists[maxCountIndex].EPC_ID);
                        Thread.Sleep(rwTagDelay * 5);
                    }
                }
                catch (Exception exp) { }
            }
        }

        private int findTag(bool bUpdateTagID)
        {
            int readCount = 0;
            int readRSSI = 60;
            int maxCountIndex = -1;

            for (int i = 0; i < tagLists.Count; i++)
            {
                if ((readCount < tagLists[i].readCount) &&                   
                    (readRSSI < tagLists[i].rssi) && (tagLists[i].rssi > 60))
                {
                    if (bUpdateTagID &&
                       ((tagLists[i].tagStatus == RFIDTagData.TagStatus.IDNotUpdate) ||
                        (tagLists[i].tagStatus == RFIDTagData.TagStatus.IDUpdated) ||
                        (tagLists[i].tagStatus == RFIDTagData.TagStatus.AccessCodeNotUpdate)))
                    {
                        readCount = tagLists[i].readCount;
                        readRSSI = tagLists[i].rssi;
                        maxCountIndex = i;
                    }
                    else if(!bUpdateTagID &&                          
                           ((tagLists[i].tagStatus == RFIDTagData.TagStatus.AccessCodeUpdated) ||
                            (tagLists[i].tagStatus == RFIDTagData.TagStatus.DataNotUpdate)))
                    {
                        readCount = tagLists[i].readCount;
                        readRSSI = tagLists[i].rssi;
                        maxCountIndex = i;
                    }
                }
            }
            return maxCountIndex;
        }

        int rfidID = 12;
        private void udpateTagID()
        {
#if enableWriteControl
            rfidID = xmlRpc.getRFIDNumber();
            if (rfidID == 0)
            {
                WriteLog(lrtxtLog, "Can't read Tag ID from cloud ", 1);
                return;
            }
#endif
            textBoxEPCTagID.Text = rfidID.ToString();                                
            if (tagLists.Count > 0)
            {
                int maxCountIndex = findTag(true);
                if (maxCountIndex < 0) return;
                
                switch(tagLists[maxCountIndex].tagStatus)
                {
                    case RFIDTagData.TagStatus.IDNotUpdate:
                        {
                            string tagID = "";
                            selectTag(tagLists[maxCountIndex].EPC_ID);
                            writeTagID(rfidID.ToString(), out tagID);
                            xmlRpcStatus = XmlRPCTagStatus.WriteID;

                            foreach (ListViewItem item in listViewEPCTag.Items)
                            {
                                if (item.SubItems[0].Text == tagLists[maxCountIndex].EPC_ID)
                                {
                                    item.Remove();
                                    break;
                                }
                            }
                            tagLists.RemoveAt(maxCountIndex);
                            tBEPCTagVerify.ForeColor = SystemColors.Control;
                            tBEPCTagVerify.BackColor = SystemColors.Control;
                            tBAccessCodeVerify.ForeColor = SystemColors.Control;
                            tBAccessCodeVerify.BackColor = SystemColors.Control;
                            tBDataVerify.ForeColor = SystemColors.Control;
                            tBDataVerify.BackColor = SystemColors.Control;
                            initScanTag();
                        }break;
                    case RFIDTagData.TagStatus.IDUpdated:
                        {
                            selectTag(tagLists[maxCountIndex].EPC_ID);
                            checkTagStatus(tagLists[maxCountIndex].EPC_ID);
                        }
                        break;
                    case RFIDTagData.TagStatus.AccessCodeNotUpdate:
                        {
                            selectTag(tagLists[maxCountIndex].EPC_ID);
                            btAccessCode_Click(null, null);
                            checkTagStatus(tagLists[maxCountIndex].EPC_ID);
                            Thread.Sleep(rwTagDelay);
                            if (xmlRpcStatus == XmlRPCTagStatus.WriteID)
                            {
#if enableWriteControl
                        xmlRpc.getNextRFIDNumber(rfidID);
#else
                                rfidID++;
#endif
                                xmlRpcStatus = XmlRPCTagStatus.Ready;
                                initScanTag();
                            }
                        }
                        break;
                }
            }
        }

        private void udpateTagData()
        {//1. get RFID tag with top hit count
            if (tagLists.Count > 0)
            {
                int maxCountIndex = findTag(false);
                if (maxCountIndex < 0) return;

                ulong rCount = 0;
                RFIDTagInfo.label = RFIDTagInfo.readEPCLabel(tagLists[maxCountIndex].EPC_ID, out rCount);
                string rfidTag = RFIDTagInfo.label.Substring(0, 2) + rCount.ToString();
                xmlRpc.readRFIDTagID(rfidTag);
                xmlRpcStatus = XmlRPCTagStatus.ReadID;
                WriteLog(lrtxtLog, "Send Tag: " + RFIDTagInfo.label, 0);
            }

            string tagData = xmlRpc.updateTag();
            if (tagData != "" && RFIDTagInfo.verifyData(tagData))
            {//Todo: need the manufacturing data
                string strRFIDTag = "";
                int dateInDigit = 0;
                if (int.TryParse(DateTime.Now.ToString("MMyy"), out dateInDigit))
                {
                    string[] tagDataList = tagData.Split(RFIDTagInfo.serialSep);

                    //Todo: lookup table to convert ink type to detail info                       
                    using (StreamReader sr = new StreamReader(sourceFilePath + @"\reference\lookUpTable.csv"))
                    {
                        string currentLine;
                        // currentLine will be null when the StreamReader reaches the end of file
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            string[] dataRow = currentLine.Split(',');
                            if (dataRow[0] == tagDataList[1])
                            {//got same ink type
                                strRFIDTag = tagData.Replace(dataRow[0], dataRow[1].Trim() + dataRow[2].Trim());
                                break;
                            }
                        }
                    }
                    if (strRFIDTag != "")
                    {//add expire date
                        strRFIDTag += (dateInDigit + 1).ToString("D4");
                        xmlRpcStatus = XmlRPCTagStatus.WriteData;
                        writeTagData(strRFIDTag);
                        //tbUserData.Text = tagData;
                        WriteLog(lrtxtLog, "Write Tag data: " + tagData + " => " + strRFIDTag, 0);
                        initScanTag();
                    }
                    else
                    {
                        WriteLog(lrtxtLog, "Convert tag data failed" + tagData, 1);
                    }
                }
            }
        }

        private void writeTagData(string data)
        {
            if (data.Length < 17)
            {//need to be at less 120 
                int missNum = 17 - data.Length;
                data += new string(RFIDTagInfo.serialSep, missNum);
            }

            symmetric.encryptedtext = symmetric.Encrypt(data);
            string strEncrypted = Convert.ToBase64String(symmetric.encryptedtext);
            WriteLog(lrtxtLog, "Encrypt data " + strEncrypted +
                               ", size " + symmetric.encryptedtext.Length, 0);

            string EPClabel = RFIDTagInfo.ASCIIToHex(data.Substring(0, 2).ToUpper());
            string[] EPCList = data.Split(RFIDTagInfo.serialSep);
            ulong uNumber;
            try
            {
                if (!ulong.TryParse(EPCList[0].Substring(2), out uNumber))
                {
                    uNumber = 0;
                }
            }
            catch (Exception exp) { uNumber = 0; }

            if (uNumber == 0)
            {
                WriteLog(lrtxtLog, "Tag format not corrected, error", 1);
                return;
            }

            string EPSnumber = uNumber.ToString("D20");
            string RFIDTagID = EPClabel + RFIDTagInfo.AddHexSpace(EPSnumber);

            selectTag(RFIDTagID);

            string reserveData = RFIDTagInfo.ASCIIToHex(strEncrypted.Substring(0, 4));
            string userData = RFIDTagInfo.ASCIIToHex(strEncrypted.Substring(4));
            byte[] btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);

            //1. write reserve section
            strHEXdata = reserveData;
            byte[] byteData = CCommondMethod.String2ByteArray(strHEXdata.ToUpper(), 2, out btWordCnt);
            
            btMemBank = 0;
            btWordAddr = 0;
            reader.WriteTag(m_curSetting.btReadId, btAryPwd, btMemBank, btWordAddr, btWordCnt, byteData, 0x94);
            Thread.Sleep(rwTagDelay * 2);

            //2. write data section
            strHEXdata = userData;
            byteData = CCommondMethod.String2ByteArray(userData.ToUpper(), 2, out btWordCnt);
            btMemBank = 3;
            btWordAddr = 0;
            reader.WriteTag(m_curSetting.btReadId, btAryPwd, btMemBank, btWordAddr, btWordCnt, byteData, 0x94);
            Thread.Sleep(rwTagDelay * 2);

            //read back to verify reserve data first
            reader.ReadTag(m_curSetting.btReadId, 0x00, 0, 4, btAryPwd);
            Thread.Sleep(rwTagDelay * 2);

            reader.ReadTag(m_curSetting.btReadId, 0x03, 0, btWordCnt, btAryPwd);
            Thread.Sleep(rwTagDelay * 2);
        }

        private void writeTagID(string serialNumber, out string tagID)
        {//1. original tag
         //2. pack smart tag, start with 50 30
            writeTagRetry = 0;

            string[] result = { "00", "00", "00", "00" };
            byte[] btAryPwd = { 0, 0, 0, 0 };
            byte[] btAryWriteData = null;
            
            if (RFIDTagInfo.bAccessCode(symmetric.readAccessCode(), RFIDTagInfo.reserverData))
            {
                btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
            }
            string EPClabel = RFIDTagInfo.readLabelFormat().Substring(0, 2).ToUpper();
            ulong uNumber;
            try
            {
                if (!ulong.TryParse(serialNumber, out uNumber))
                {
                    uNumber = 0;
                }
            }
            catch (Exception exp)  { uNumber = 0; }
            if (uNumber == 0)
            {
                WriteLog(lrtxtLog, "Input Tag out of range " + serialNumber, 1);
                tagID = "";
                return;
            }
            string EPSnumber = uNumber.ToString("D20");

            //mix with 2 ASCII and 10 digits
            strHEXdata = tagID = RFIDTagInfo.ASCIIToHex(EPClabel) + RFIDTagInfo.AddHexSpace(EPSnumber);
            btAryWriteData = CCommondMethod.String2ByteArray(tagID, 2, out btWordCnt);

            btMemBank = 1;
            btWordAddr = 2;
            //btWordCnt = 6;
            //1, access, 1, 2, 6, data, 148
            reader.WriteTag(m_curSetting.btReadId, btAryPwd, btMemBank, btWordAddr, btWordCnt, btAryWriteData, 0x94);
            WriteLog(lrtxtLog, "Write Tag ID: " + tagID, 0);
            Thread.Sleep(rwTagDelay);

            selectTag(tagID);
            Thread.Sleep(rwTagDelay);
        }

        private void btEPCTag_Click(object sender, EventArgs e)
        {//update Tag, 2 kinds of tag id
            string tagID = ""; // to be write
            writeTagID(textBoxEPCTagID.Text, out tagID);

            btAccessCode_Click(sender, e);
            Thread.Sleep(rwTagDelay);

            checkTagStatus(tagID);
            Thread.Sleep(rwTagDelay);
            
            if (listViewEPCTag.SelectedItems.Count > 0)
            {
                for (int i = 0; i < tagLists.Count; i++)
                {
                    if (tagLists[i].EPC_ID == listViewEPCTag.SelectedItems[0].Text)
                    {
                        tagLists.RemoveAt(i);
                        break;
                    }
                }
                listViewEPCTag.SelectedItems[0].Remove();
            }
            tBEPCTagVerify.ForeColor = SystemColors.Control;
            tBEPCTagVerify.BackColor = SystemColors.Control;
            tBAccessCodeVerify.ForeColor = SystemColors.Control;
            tBAccessCodeVerify.BackColor = SystemColors.Control;
            tBDataVerify.ForeColor = SystemColors.Control;
            tBDataVerify.BackColor = SystemColors.Control;
            initScanTag();
        }

        private void btAccessCode_Click(object sender, EventArgs e)
        {//write access code, it can be empty or update before
            writeTagRetry = 0;

            string[] result = { "00", "00", "00", "00" };
            byte[] byteAryPwd = { 0, 0, 0, 0 };
            byte[] byteAryWriteData = null;

            byte btMemBank = 0;
            byte btWordAddr = 2;
            byte btWordCnt = 2;

            //load Access Code
            string strCode = RFIDTagInfo.ASCIIToHex(Encoding.ASCII.GetString(Properties.Resources.AccessCode)).ToUpper();
            symmetric.loadAccessCode(strCode);

            if (RFIDTagInfo.bAccessCode(symmetric.readAccessCode(), RFIDTagInfo.reserverData))
            {
                result = CCommondMethod.String2StringArray(symmetric.readAccessCode(), 2);
            }
            byteAryPwd = CCommondMethod.StringArray2ByteArray(result, result.Length);            
            byteAryWriteData = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
            
            //1, access, 0, 2, 2, data, 148
            //btWordAddr = 2;
            reader.WriteTag(m_curSetting.btReadId, byteAryPwd, btMemBank, btWordAddr, btWordCnt, byteAryWriteData, 0x94);
            Thread.Sleep(rwTagDelay);

            //lock tag
            btMemBank = 4;
            reader.LockTag(m_curSetting.btReadId, byteAryWriteData, btMemBank, 0x01);
            Thread.Sleep(rwTagDelay);

            btWordAddr = 0;
            btWordCnt = 4;
            reader.ReadTag(m_curSetting.btReadId, btMemBank, btWordAddr, btWordCnt, byteAryWriteData);
            Thread.Sleep(rwTagDelay);
        }

        private void listViewEPCTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabCtrMain.SelectedTab == pageEpcID)
                {
                    String text = listViewEPCTag.SelectedItems[0].Text;
                    tBEPCTagVerify.ForeColor = SystemColors.Control;
                    tBEPCTagVerify.BackColor = SystemColors.Control;
                    tBAccessCodeVerify.ForeColor = SystemColors.Control;
                    tBAccessCodeVerify.BackColor = SystemColors.Control;
                    tBDataVerify.ForeColor = SystemColors.Control;
                    tBDataVerify.BackColor = SystemColors.Control;
                    selectTag(text);
                    Thread.Sleep(rwTagDelay);

                    checkTagStatus(text);
                    Thread.Sleep(rwTagDelay);
                }
            }
            catch (Exception exp) { return; }
        }

        private void selectTag(string tagID)
        {
            byte[] btAryEpc = CCommondMethod.String2ByteArray(tagID.ToUpper(), 2, out btWordCnt);
            reader.SetAccessEpcMatch(m_curSetting.btReadId, 0x00, Convert.ToByte(btAryEpc.Length), btAryEpc);
            Thread.Sleep(rwTagDelay);
        }
                
        private void checkTagStatus(string tagID)
        {
            try
            {
                tbSelectTag.Text = tagID;             
                int index = findTagIndex(tagID.Trim());
                if (index == -1)
                    return;
                if ((tagLists[index].tagStatus == RFIDTagData.TagStatus.IDNotUpdate) ||
                   (tagLists[index].tagStatus == RFIDTagData.TagStatus.IDUpdated))
                {
                    RFIDTagInfo.accessCode = null;
                }
                else
                {
                    RFIDTagInfo.accessCode = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
                }
                btMemBank = 0;
                btWordAddr = 0;
                reader.ReadTag(m_curSetting.btReadId, btMemBank, 0, btWordCnt, RFIDTagInfo.accessCode);
                Thread.Sleep(rwTagDelay);
            }
            catch (Exception exp) { WriteLog(lrtxtLog, "Read Tag got exception: " + exp.Message, 1); }
        }

        private void tabCtrMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabCtrMain.SelectedTab == pageData)
            {//clear the listview row background color
                foreach (ListViewItem item in listViewEPCTag.Items)
                {
                    if (item.ForeColor != SystemColors.Control)
                    {
                        item.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void checkBoxShowDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowDetail.Checked)
            {
                splitContainer1.SplitterDistance = 300;
            }
            else
            {
                splitContainer1.SplitterDistance = 0;
            }
        }
    }
}
