#define DESKTOP
#define enableWriteControl
#define Connect2Odoo
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.Ports;
using IronPython.Hosting;
using System.Media;

namespace RFIDApplication
{
    public partial class RFIDTagIDForm : Form
    {
        public class RFIDReader
        {
#if DESKTOP
            public static int nNormalStartFrequency = 921000; //Start Freq: kHz
            public static byte nFrequencyInterval = 10;//5;    //5, Freq Sapce: per 10KHz
            public static byte btChannelQuantity = 50;//30;     //5, Quentity
            public static byte[] OutputPower = { 10 }; //26-18, 10
            public static int NormalRSSI_MIN = 80;
            public static int RSSI_MIN = 80;
#else
            public static int nNormalStartFrequency = 13850000; //Start Freq: kHz
            public static byte nFrequencyInterval = 5;    //5, Freq Sapce: per 10KHz
            public static byte btChannelQuantity = 30;     //5, Quentity
            public static byte[] OutputPower = { 18 }; //26-18, 10
             public static int NormalRSSI_MIN = 60;
            public static int RSSI_MIN = 80;
#endif
        }

        LoginForm loginForm = null;
        public dynamic xmlRpc = null;
        private Reader.ReaderMethod reader;
        private ReaderSetting m_curSetting = new ReaderSetting();
        private InventoryBuffer m_curInventoryBuffer = new InventoryBuffer();
        private OperateTagBuffer m_curOperateTagBuffer = new OperateTagBuffer();
        Symmetric_Encrypted symmetric = new Symmetric_Encrypted();

        public const int TAGRESET = 3;
        public int iComPortStatus = 0;
        private const int QC_Write_Test = 20;
        //Before inventory, you need to set working antenna to identify whether the inventory operation is executing.
        private bool m_bInventory = false;
        const short rwTagDelay = 50; //read/write RFID tag delay, default is 300
        private int m_nTotal = 0;
        //Frequency of list updating.
        private int m_nRealRate = 5;
        //Record quick poll antenna parameter.
        private byte[] m_btAryData = new byte[18];
        private byte[] m_btAryData_4 = new byte[10];
        private int m_nReceiveFlag_300ms = 0;
        //private int m_nReceiveFlag_1s = 0;

        int readTagRetry = 0;
        int writeTagRetry = 0;
        int WriteTagCount = 0;
        const int rwTagRetryMAX = 5;
       
        //private volatile bool m_nPhaseOpened = false;
        private volatile bool m_nSessionPhaseOpened = false;
        List<RFIDTagData> tagLists = new List<RFIDTagData>();
        bool bFormMoveflag = false;

        bool bVerify = false;        
        string strHEXdata = "";    
        
        static bool bReaderListUpdate = false;
        private string sourceFilePath = "";
      
        enum tagSelect
        {
            writeID,
            writeData,
            Test
        }

        public RFIDTagIDForm(LoginForm login)
        {
            reader = new Reader.ReaderMethod();
            reader.AnalyCallback = AnalyData;
            sourceFilePath = Path.GetDirectoryName(new Uri(this.GetType().Assembly.GetName().CodeBase).LocalPath);

            loginForm = login;

            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void RFIDTagIDForm_Load(object sender, EventArgs e)
        {
            byte[] labelFormat = Properties.Resources.LabelFormat;
            RFIDTagInfo.loadLabelFormat(labelFormat);
            tbQCTestCnt.Text = "Waiting...";//FIDTagInfo.writeTestReceiveCount + " out of " + QC_Write_Test;
            if (xmlRpc != null)
            {
                tbLoginID.Text = "Welcome, " + xmlRpc.getUserName();
            }
#if DEBUG
            btShowDebug1.Text = "Hide log";
            splitContainer2.Panel2Collapsed = false;
            this.Height = 551;

            checkBoxShowDetail.Checked = true;
            splitContainer1.SplitterDistance = (int)Math.Ceiling(this.Width * 0.623);
#else
            btShowDebug1.Text = "Show log";
            splitContainer2.Panel2Collapsed = true;            
            this.Height = 369;
#endif
            if (checkBoxShowDetail.Checked)
            {
                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
            }

            checkComPort();
            if (iComPortStatus == 1)
            {
                reader.GetFirmwareVersion(m_curSetting.btReadId);                        
                initScanTag(true);
            }
        }

        private void timerInventory_Tick(object sender, EventArgs e)
        {
            if (m_nReceiveFlag_300ms++ >= 1)
            {//900ms
                RunLoopInventroy();
                m_nReceiveFlag_300ms = 0;

                try
                {
                    if (/*bReaderListUpdate &&*/ xmlRpc != null)
                    {
                        bReaderListUpdate = false;
                        if (xmlRpc.readyToUpdateTag())
                        {
                            if (tabCtrMain.SelectedTab == pageData)
                            {
                                if (XMLRPC.xmlRpcStatus == XMLRPC.XmlRPCTagStatus.OdooSuccessful)
                                {
                                    //checkOdooStatus();
                                    return;
                                }
                                else if (XMLRPC.xmlRpcStatus == XMLRPC.XmlRPCTagStatus.Update2Odoo)
                                {
                                    checkOdooStatus();
                                }
                                else
                                {
                                    xmlRpcUpdateTagData(-1);
                                }
                                return;
                            }
                            else if (tabCtrMain.SelectedTab == pageEpcID)
                            {
                                xmlRpcUpdateTagID();
                                return;
                            }
                        }

                        //check tag status only
                        //tbDataUpdateStatus.Text = "Cloud can't connect, check the connection!";
                        int index = findTag(tagSelect.Test);
                        if (index < 0) return;

                        selectTag(tagLists[index].EPC_ID);
                        Thread.Sleep(rwTagDelay);
                        if (tabCtrMain.SelectedTab == pageQC)
                        {                            
                            QCPage(index);
                        }
                        else
                        {
                            checkTagStatus(tagLists[index].EPC_ID);
                        }
                    }
                }
                catch (Exception exp) { }
            }
        }

        public void checkPort(string comPort)
        {
            string strException = string.Empty;

            int nRet = reader.OpenCom(comPort, Convert.ToInt32("115200"), out strException);
            if (nRet != 0)
            {
                //WriteLog(lrtxtLog, "Connection failed, failure cause: " + strException, 1);
                return;
            }
            else
            {
#if DEBUG
                //WriteLog(lrtxtLog, "Connect " + comPort + "@" + "115200", 0);
#endif
            }
            Thread.Sleep(rwTagDelay);
            reader.resetCom();
            Thread.Sleep(rwTagDelay);

            reader.GetFirmwareVersion(m_curSetting.btReadId);
            Thread.Sleep(rwTagDelay);
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
                catch (Exception exp) { /*WriteLog(lrtxtLog, "Error, can't connect to any Com Port", 1);*/ }
            }
            while ((iComPortStatus != 1) && (i-- > 0));

            if (iComPortStatus == 1)
            {
#if DEBUG
               //DateTime dateTimeNow = DateTime.Now;
               //WriteLog(lrtxtLog, "Message start: " + dateTimeNow.ToString(), 0);
               // WriteLog(lrtxtLog, "Com port found, Load setting", 0);
#endif
                loadSetting();
            }
            else
            {
                //WriteLog(lrtxtLog, "Error, can't connect to any Com Port", 1);
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
                        
            listViewEPCTag.Items.Clear();
            tagLists.Clear();
            m_nTotal = 0;
            reader.SetWorkAntenna(m_curSetting.btReadId, 0x00);
            m_curSetting.btWorkAntenna = 0x00;
        }
        private void initScanTag(bool bUpdateSetting)
        {
            m_curInventoryBuffer.ClearInventoryPar();
            m_curInventoryBuffer.btRepeat = 1; //1
            m_curInventoryBuffer.bLoopCustomizedSession = false;

            m_bInventory = true;
            m_curInventoryBuffer.bLoopInventory = true;
            m_curInventoryBuffer.bLoopInventoryReal = true;
            m_curInventoryBuffer.ClearInventoryRealResult();

            if (bUpdateSetting)
            {
                reader.SetOutputPower(m_curSetting.btReadId, RFIDReader.OutputPower);
                Thread.Sleep(rwTagDelay*2);
#if false
                if (tabCtrMain.TabPages.Count == 3 && tabCtrMain.SelectedTab == pageQC)
                {
                    reader.SetUserDefineFrequency(m_curSetting.btReadId,
                                                    RFIDReader.nAdvStartFrequency,
                                                    RFIDReader.nFrequencyInterval,
                                                    RFIDReader.btChannelQuantity);
                    m_curSetting.btRegion = 4;
                    m_curSetting.nUserDefineStartFrequency = RFIDReader.nAdvStartFrequency;
                    m_curSetting.btUserDefineFrequencyInterval = RFIDReader.nFrequencyInterval;
                    m_curSetting.btUserDefineChannelQuantity = RFIDReader.btChannelQuantity;
                    RFIDReader.RSSI_MIN = RFIDReader.AdvRSSI_MIN;
                }
                else
#endif
                {
                    reader.SetUserDefineFrequency(m_curSetting.btReadId,
                                    RFIDReader.nNormalStartFrequency,
                                    RFIDReader.nFrequencyInterval,
                                    RFIDReader.btChannelQuantity);

                    m_curSetting.btRegion = 4;
                    m_curSetting.nUserDefineStartFrequency = RFIDReader.nNormalStartFrequency;
                    m_curSetting.btUserDefineFrequencyInterval = RFIDReader.nFrequencyInterval;
                    m_curSetting.btUserDefineChannelQuantity = RFIDReader.btChannelQuantity;
                    RFIDReader.RSSI_MIN = RFIDReader.NormalRSSI_MIN;
                }
                Thread.Sleep(rwTagDelay * 2);

                m_nTotal = 0;
                reader.SetWorkAntenna(m_curSetting.btReadId, 0x00);
                Thread.Sleep(rwTagDelay * 2);
                m_curSetting.btWorkAntenna = 0x00;
            }
            timerInventory.Enabled = true;
        }

        private TextBox SetStatusResult(TextBox tb, TagQC.TagStatus tStatus)
        {
            switch (tStatus)
            {
                case TagQC.TagStatus.Default:
                    {
                        tb.Text = TagQC.TagResult.DEFAULT;
                        tb.ForeColor = Color.Green;
                        tb.BackColor = Color.White;
                    }
                    break;
                case TagQC.TagStatus.Pass:
                    {
                        tb.Text = TagQC.TagResult.PASS;
                        tb.ForeColor = Color.Green;
                        tb.BackColor = Color.White;
                    }
                    break;
                case TagQC.TagStatus.Fail:
                    {
                        tb.Text = TagQC.TagResult.FAIL;
                        tb.ForeColor = Color.Red;
                        tb.BackColor = Color.White;
                    }
                    break;
            }
            tb.Refresh();
            return tb;
        }

        private void QCPage(int iTagIndex)
        {//for existing tag, check tag access code, data
         //for new tag, check the size of ID, size of access code, size of data

            byte btWordCnt = 0;
            byte[] btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
            byte[] byteZeroData = new byte[4] { 0, 0, 0, 0 };
            if (tbQCTagStatus.Text == TagQC.TagAccessCodeText.DEFAULT)
            {
                if (tbQCTagID.Text == TagQC.TagIDText.EMPTY)
                {
                    reader.ReadTag(m_curSetting.btReadId, 0, 0, RFIDTagInfo.RESERVESIZE, byteZeroData);
                    RFIDTagInfo.accessCode = byteZeroData;
                    Thread.Sleep(rwTagDelay*2);
                }
                else
                {
                    checkTagStatus(tagLists[iTagIndex].EPC_ID);
                }
            }
            else if (tbQCTagData.Text == TagQC.TagDataText.DEFAULT)
            {
                reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, byteZeroData);
                Thread.Sleep(rwTagDelay * 2);
            }
            else if (tbQCTagData.Text == TagQC.TagDataText.EMPTY ||
                    tbQCTagData.Text == TagQC.TagDataText.USED ||
                    tbQCTagData.Text == TagQC.TagDataText.DATASIZE)
            {//test read write  
                if (tagLists[iTagIndex].writeTestCount == 0) //reset
                {
                    RFIDTagInfo.writeTestReceiveCount = 0;
                    RFIDTagInfo.writeTestRssiTotal = 0;
                    timerInventory.Interval = 30;
                }
                if (tagLists[iTagIndex].writeTestCount <= QC_Write_Test)
                {
                    XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.Test;
                    m_bInventory = false;
                    m_curInventoryBuffer.bLoopInventory = false;
                    m_curInventoryBuffer.bLoopInventoryReal = false;

                    if (RFIDTagInfo.reserverData == "")
                    {
                        if (tbQCTagStatus.Text == TagQC.TagAccessCodeText.LOCKED && tbAccessCodeResult.Text == TagQC.TagResult.PASS)
                            reader.WriteTag(m_curSetting.btReadId, btAryPwd, 0, 0, 4, byteZeroData, 0x94);
                        else
                            reader.WriteTag(m_curSetting.btReadId, byteZeroData, 0, 0, 4, byteZeroData, 0x94);
                    }
                    else
                    {
                        byte[] byteData = CCommondMethod.String2ByteArray(RFIDTagInfo.reserverData.ToUpper(), 2, out btWordCnt);
                        reader.WriteTag(m_curSetting.btReadId, btAryPwd, 0, 0, btWordCnt, byteData, 0x94);
                    }
                    //WriteLog(lrtxtLog, "Write test, count " + tagLists[iTagIndex].writeTestCount.ToString(), 0);
                    Thread.Sleep(rwTagDelay * 3);
                    tagLists[iTagIndex].writeReceiveCount = RFIDTagInfo.writeTestReceiveCount;
                    RFIDTagInfo.writeTestRssiTotal += tagLists[iTagIndex].rssi;
                    tbQCTestCnt.Text = tagLists[iTagIndex].writeReceiveCount + " out of " + tagLists[iTagIndex].writeTestCount++;
                }
                else if (tagLists[iTagIndex].writeReceiveCount >= (tagLists[iTagIndex].writeTestCount / 4 * 3) &&
                         tbWriteTestResult.Text == TagQC.TagResult.PASS ||
                         tbWriteTestResult.Text == TagQC.TagResult.FAIL)
                {
                    if(!m_curInventoryBuffer.bLoopInventory)
                    {
                        timerInventory.Interval = 200;
                        initScanTag(false);
                    }
                    return;
                }
                else if (tbWriteTestResult.Text != TagQC.TagResult.PASS ||
                        tbWriteTestResult.Text != TagQC.TagResult.FAIL)
                {
                    if (tagLists[iTagIndex].writeReceiveCount >= (tagLists[iTagIndex].writeTestCount / 4 * 3))
                    {
                        tbWriteTestResult = SetStatusResult(tbWriteTestResult, TagQC.TagStatus.Pass);
                        RFIDTagInfo.playSound(true);
                    }
                    else
                    {
                        tbWriteTestResult = SetStatusResult(tbWriteTestResult, TagQC.TagStatus.Fail);
                        RFIDTagInfo.playSound(false);
                    }
                    timerInventory.Interval = 200;
                    initScanTag(false);
                    XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.Ready;
                }
            }
            else if (tbQCTagData.Text.Contains(tbQCTagID.Text))
            {
                if (tbQCTestCnt.Text == TagQC.TagDataText.DEFAULT)
                {
                    tbQCTestCnt.Text = "Skip write test";
                }
            }
        }
        private void ProcessSetOutputPower(Reader.MessageTran msgTran)
        {
            string strCmd = "Set RF Output Power";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    //WriteLog(lrtxtLog, strCmd, 0);
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
        private void ProcessGetOutputPower(Reader.MessageTran msgTran)
        {
            string strCmd = "Get RF Output Power";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                m_curSetting.btReadId = msgTran.ReadId;
                m_curSetting.btOutputPower = msgTran.AryData[0];

                if (msgTran.AryData[0] != RFIDReader.OutputPower[0])
                {
                    reader.SetOutputPower(m_curSetting.btReadId, RFIDReader.OutputPower);
                }
                return;
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog, strLog, 1);
        }

        private void ProcessSetFrequencyRegion(Reader.MessageTran msgTran)
        {
            string strCmd = "Set RF frequency spectrum";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
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
            //string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            //WriteLog(lrtxtLog, strLog, 1);
        }

        private void AnalyData(Reader.MessageTran msgTran)
        {
            m_nReceiveFlag_300ms = 0;
            if (msgTran.PacketType != 0xA0)
            {
                return;
            }
            switch (msgTran.Cmd)
            {
                case 0x61:
                    ProcessWriteGpioValue(msgTran);
                    break;
                case 0x72:
                    ProcessGetFirmwareVersion(msgTran);
                    break;
                case 0x76:
                    ProcessSetOutputPower(msgTran);
                    break;
                case 0x97:
                case 0x77:
                    ProcessGetOutputPower(msgTran);
                    break;
                case 0x78:
                    ProcessSetFrequencyRegion(msgTran);
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
                    logRichTxt.AppendTextEx(strLog, Color.White);//Color.Indigo);
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

        private void resetStatusColor()
        {
            tbEPCTagDetected.Visible = false;
            tBAccessCodeVerify.Visible = false;
            tBAccessCodeVerify.ForeColor = Color.Green;            
            tbDataVerify.Visible = false;
            tbOdooStatus.Visible = false;
            tbOdooStatus.ForeColor = Color.Green;

            XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.Ready;
            tbQCTagID.Text = TagQC.TagIDText.DEFAULT;
            tbIDResult = SetStatusResult(tbIDResult, TagQC.TagStatus.Default);

            tbQCTagStatus.Text = TagQC.TagAccessCodeText.DEFAULT;
            tbAccessCodeResult = SetStatusResult(tbAccessCodeResult, TagQC.TagStatus.Default);

            tbQCTagData.Text = TagQC.TagDataText.DEFAULT;
            tbDataResult = SetStatusResult(tbDataResult, TagQC.TagStatus.Default);

            tbQCTestCnt.Text = "Waiting..."; // RFIDTagInfo.writeTestReceiveCount + " out of " + QC_Write_Test;
            tbWriteTestResult = SetStatusResult(tbWriteTestResult, TagQC.TagStatus.Default);
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
                            //TimeSpan ts = m_curInventoryBuffer.dtEndInventory - m_curInventoryBuffer.dtStartInventory;
                            //int nTotalTime = ts.Minutes * 60 * 1000 + ts.Seconds * 1000 + ts.Milliseconds;

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
                                        if (tagLists[index].readCount == Convert.ToInt32(row[7]))
                                            tagLists[index].notUpdateCount++;
                                        else if (tagLists[index].notUpdateCount > 0)                                        
                                            tagLists[index].notUpdateCount--;
                                        
                                        tagLists[index].readCount = Convert.ToInt32(row[7]);
                                        tagLists[index].rssi = Convert.ToInt32(row[4]);

                                        if (!tbEPCTagDetected.Visible)
                                        {
                                            tbEPCTagDetected.Visible = true;
                                            //tbDataUpdateStatus.Text = "Tag ID format not corrected, move to next Tag";
                                        }
                                    }
                                    else
                                    {
                                        RFIDTagData tagData = new RFIDTagData();
                                        
                                        tagData.EPC_ID = row[2].ToString();
                                        RFIDTagInfo.readEPCLabel(tagData.EPC_ID, out tagData.EPC_PS_Num);

                                        if (tagData.EPC_ID.StartsWith(" "))
                                            tagData.EPC_ID = tagData.EPC_ID.Remove(0, 1) + " ";

                                        selectTag(tagData.EPC_ID);
                                        tagData.readCount = Convert.ToInt32(row[7]);
                                        tagData.rssi = Convert.ToInt32(row[4]);

                                        if (tagData.EPC_PS_Num > 0)
                                        {                                           
                                            tagData.tagStatus = RFIDTagData.TagStatus.IDUpdated;
                                            resetStatusColor();

                                            if (tabCtrMain.SelectedTab == pageEpcID)
                                            {
                                                tbEPCTagDetected.Visible = true;

                                                if (rfidID > 0 && tagData.EPC_PS_Num == (ulong)rfidID)
                                                {
                                                    xmlRpc.getNextRFIDNumber(rfidID);

                                                    tBAccessCodeVerify.Text = "Write success";
                                                    tBAccessCodeVerify.Visible = true;
                                                    tBAccessCodeVerify.ForeColor = Color.Green;
                                                    tBAccessCodeVerify.BackColor = Color.White;
                                                    RFIDTagInfo.playSound(true);
                                                }
                                                else
                                                {
                                                    tBAccessCodeVerify.Visible = true;
                                                    tBAccessCodeVerify.Text = "Tag ID has already updated";
                                                    tBAccessCodeVerify.ForeColor = Color.Red;
                                                    tBAccessCodeVerify.BackColor = Color.White;
                                                    RFIDTagInfo.playSound(false);
                                                }
                                            }
                                            else if (tabCtrMain.SelectedTab == pageQC)
                                            {
                                                tbQCTagID.Text = "PS"+ tagData.EPC_PS_Num.ToString();                                               
                                                tbIDResult = SetStatusResult(tbIDResult, TagQC.TagStatus.Pass);
                                            }
                                        }
                                        else
                                        {                                            
                                            tagData.tagStatus = RFIDTagData.TagStatus.IDNotUpdate;
                                            resetStatusColor();

                                            if (tabCtrMain.SelectedTab == pageEpcID)
                                            {
                                                tbEPCTagDetected.Visible = true;
                                            }
                                            else if (tabCtrMain.SelectedTab == pageQC)
                                            {
                                                tbQCTagID.Text = TagQC.TagIDText.EMPTY;//"Tag ID format not corrected";
                                                tbIDResult = SetStatusResult(tbIDResult, TagQC.TagStatus.Pass);
                                            }

                                            if (tabCtrMain.SelectedTab != pageEpcID)
                                            {
                                                WriteLog(lrtxtLog, "Tag ID format not corrected", 1);
                                            }
                                        }                       
                                        tagLists.Add(tagData);
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
                                            //epcTag.ForeColor = Color.White;
                                            epcTag.SubItems.Add("Color");
                                            epcTag.SubItems[1].BackColor = Color.FromArgb(25, 69, 88, 136);
                                            epcTag.SubItems[1].ForeColor = Color.White;
                                            epcTag.UseItemStyleForSubItems = false;
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
                                                    switch (tagLists[j].tagStatus)
                                                    {                                                       
                                                        case RFIDTagData.TagStatus.IDUpdated:
                                                            {
                                                                checkTagStatus(tagLists[j].EPC_ID);
                                                            }
                                                            break;
                                                    }
                                                    item.SubItems[1].Text = tagLists[j].readCount.ToString();
                                                    listViewEPCTag.EndUpdate();
                                                }
                                            }
                                        }
                                    }
                                }
                                for(int i=0; i<tagLists.Count; i++)
                                {
                                    if(tagLists[i].notUpdateCount > TAGRESET)
                                    {
                                        m_curInventoryBuffer.removeInventoryItem(2, tagLists[i].EPC_ID);
                                        foreach (ListViewItem item in listViewEPCTag.Items)
                                        {
                                            if (item.SubItems[0].Text.Trim().Contains(tagLists[i].EPC_ID.Trim()))
                                            {
                                                item.Remove();
                                                break;
                                            }
                                        }
                                        tagLists.RemoveAt(i);
                                    }
                                }
                                bReaderListUpdate = true;
                                Thread.Sleep(50);
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
                //WriteLog(lrtxtLog, strCmd + m_curSetting.btMajor + "." + m_curSetting.btMinor, 0);
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

        private bool verifyTag(string label, int index)
        {
            string decryptMsg = symmetric.DecryptFromHEX(label);
#if DEBUG
            //WriteLog(lrtxtLog, "Verify data read " + decryptMsg, 0);
#endif
            if( RFIDTagInfo.verifyData(decryptMsg, sourceFilePath))
            {
                tagLists[index].tagInfo = decryptMsg;
                return true;
            }
            return false;
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
                byte btWordCnt = 0;
                try
                {
                    //WriteLog(lrtxtLog,"Read Tag, AryData.Length " + msgTran.AryData.Length, 0);
                    if (msgTran.AryData.Length == 1)
                    {
                        if (tagLists.Count == 0)
                            return; 

                        strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                        string strLog = strCmd + " Failure, read tag failure cause1: " + strErrorCode;
                        if (readTagRetry++ < rwTagRetryMAX)
                        {
                            byte[] btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
                            if (!RFIDTagInfo.checkZeroAccessCode(RFIDTagInfo.accessCode))
                            {
                                Array.Clear(btAryPwd, 0, btAryPwd.Length);
                            }
                            RFIDTagInfo.accessCode = btAryPwd;

                            switch(XMLRPC.xmlRpcStatus)
                            {
                                case XMLRPC.XmlRPCTagStatus.WriteIDOk: //read ID
                                    reader.ReadTag(m_curSetting.btReadId, 1, 2, 6, btAryPwd);
                                    break;
                                case XMLRPC.XmlRPCTagStatus.Ready:
                                case XMLRPC.XmlRPCTagStatus.WriteAccessOk:
                                    reader.ReadTag(m_curSetting.btReadId, 0, 0, RFIDTagInfo.RESERVESIZE, btAryPwd);
                                    break;
                                case XMLRPC.XmlRPCTagStatus.writeUserData:
                                    reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, btAryPwd);
                                    break;
                            }                           
                            Thread.Sleep((readTagRetry % rwTagRetryMAX) * 2);
#if DEBUG
                            //WriteLog(lrtxtLog, "Read Tag retry " + readTagRetry, 1);
#endif
                            if (RFIDTagInfo.currentTagID != "")
                            {
                                for (int i = 0; i < tagLists.Count; i++)
                                {
                                    if (RFIDTagInfo.currentTagID == tagLists[i].EPC_ID)
                                    {
                                        tagLists[i].notUpdateCount++;
                                    }
                                }
                            }
                        }
                        else 
                        {// read tag failed
                            WriteLog(lrtxtLog, "Read Tag retry" + readTagRetry + " (" + rwTagRetryMAX + ") " + readTagRetry, 1);
                            readTagRetry = 0;
                            Thread.Sleep(rwTagDelay*2);
                            if (RFIDTagInfo.currentTagID != "")
                            {
                                for (int i = 0; i < tagLists.Count; i++)
                                {
                                    if (RFIDTagInfo.currentTagID == tagLists[i].EPC_ID)
                                    {
                                        m_curInventoryBuffer.removeInventoryItem(2, tagLists[i].EPC_ID);
                                        foreach (ListViewItem item in listViewEPCTag.Items)
                                        {
                                            if (item.SubItems[0].Text.Trim().Contains(tagLists[i].EPC_ID.Trim()))
                                            {
                                                item.Remove();
                                                break;
                                            }
                                        }
                                        tagLists.RemoveAt(i);
                                    }
                                }
                            }
                            timerInventory.Enabled = false;
                            timerInventory.Interval = 200;
                            initScanTag(false);
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
                        int index = findTagIndex(strEPC.Trim());
                        if (index == -1)
                            return;

                        tagLists[index].label = RFIDTagInfo.readEPCLabel(strEPC, out uWordCnt);
                        if (RFIDTagInfo.currentTagID != tagLists[index].EPC_ID)
                        {
                            if (tabCtrMain.SelectedTab != pageEpcID)
                                resetStatusColor();

                            RFIDTagInfo.currentTagID = tagLists[index].EPC_ID;
                        }
                        byte[] byteZeroData = new byte[4] { 0, 0, 0, 0 };
                        if (tabCtrMain.SelectedTab == pageQC)
                        {
                            if (tbQCTagID.Text == TagQC.TagIDText.DEFAULT)
                            {
                                if (tagLists[index].label != "" && uWordCnt > 0)                                    
                                    tbQCTagID.Text = tagLists[index].label.Substring(0, 2) + uWordCnt.ToString();                                    
                                else                                    
                                    tbQCTagID.Text = TagQC.TagIDText.EMPTY;
                                    
                                tbIDResult.Text = TagQC.TagResult.PASS;
                            }
                            if(tbQCTagStatus.Text == TagQC.TagAccessCodeText.DEFAULT)
                            {
                                byte[] btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);                                    
                                    
                                if (tbQCTagID.Text == TagQC.TagIDText.EMPTY)                                    
                                    RFIDTagInfo.accessCode = byteZeroData;                           
                                else                                    
                                    RFIDTagInfo.accessCode = btAryPwd;
                                    
                                reader.ReadTag(m_curSetting.btReadId, 0, 0, RFIDTagInfo.RESERVESIZE, RFIDTagInfo.accessCode);
                                Thread.Sleep(rwTagDelay);
                            }
                        }                                            
                        
                        if (!tbEPCTagDetected.Visible)
                            tbEPCTagDetected.Visible = true;                       

                        if (strData.Length / 3 == 4 || strData.Length / 3 == 8)
                        {
                            switch (tagLists[index].tagStatus)
                            {
                                //case RFIDTagData.TagStatus.AccessCodeNotUpdate:
                                case RFIDTagData.TagStatus.DataUpdated:
                                case RFIDTagData.TagStatus.DataErased:
                                    return;
                            }

                            if (RFIDTagInfo.bAccessCode(symmetric.readAccessCode(), strData))
                            {//verify access code                               
                                RFIDTagInfo.reserverData = strData;                               

                                //update tag status for ready or locked                                
                                if ((tagLists[index].tagStatus != RFIDTagData.TagStatus.DataNotUpdate) &&
                                    (tagLists[index].tagStatus != RFIDTagData.TagStatus.DataUpdated) &&
                                    (tagLists[index].tagStatus != RFIDTagData.TagStatus.DataErased))
                                {
                                    if (tagLists[index].tagStatus != RFIDTagData.TagStatus.AccessCodeUpdated)
                                    {
                                        WriteLog(lrtxtLog, "Verified Access Code Successful", 0);
                                    }

                                    if (!tbDataVerify.Visible)
                                    {
                                        tbDataVerify.Visible = true;
                                    }

                                    tagLists[index].tagStatus = RFIDTagData.TagStatus.AccessCodeUpdated;
                                    if (tabCtrMain.SelectedTab == pageEpcID)
                                    {
                                        reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, null);
                                        Thread.Sleep(rwTagDelay*2);
                                    }
                                    else if (tabCtrMain.SelectedTab == pageData)
                                    {//in data page, read TID
                                        reader.ReadTag(m_curSetting.btReadId, 2, 0, 12, null);
                                        Thread.Sleep(rwTagDelay);
                                    }
                                    else if (tabCtrMain.SelectedTab == pageQC)
                                    {
                                        tbQCTagStatus.Text = TagQC.TagAccessCodeText.LOCKED; // "TAG Locked";
                                        tbAccessCodeResult = SetStatusResult(tbAccessCodeResult, TagQC.TagStatus.Pass);

                                        reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, null);
                                        Thread.Sleep(rwTagDelay*2);
                                    }
                                }
                                else if((RFIDTagInfo.reserverData.Substring(0, 12).Trim() != "00 00 00 00") &&
                                        (tagLists[index].tagStatus != RFIDTagData.TagStatus.DataErased))
                                {
                                    reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, RFIDTagInfo.accessCode);
                                    Thread.Sleep(rwTagDelay*2);
                                }
                            }
                            else
                            {
                                RFIDTagInfo.reserverData = "";
                                // ID update, but access code not update
                                tagLists[index].tagStatus = RFIDTagData.TagStatus.AccessCodeNotUpdate;
                                                               
                                if (tabCtrMain.SelectedTab == pageQC)
                                {                                   
                                    if(tbQCTagID.Text != TagQC.TagIDText.EMPTY)
                                    {// ID is serialize, not lock
                                        tbQCTagStatus.Text = TagQC.TagAccessCodeText.NONLOCK; // "TAG is not lock";
                                        tbAccessCodeResult = SetStatusResult(tbAccessCodeResult, TagQC.TagStatus.Fail);
                                        if (strData == null || strData.Trim().StartsWith("00 00 00 00"))
                                        {
                                            tbQCTagData.Text = TagQC.TagDataText.EMPTY; // "Data verification failed";
                                            tbDataResult = SetStatusResult(tbDataResult, TagQC.TagStatus.Pass);
                                        }
                                        else
                                        {
                                            XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.writeUserData;
                                            reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, RFIDTagInfo.accessCode);
                                            Thread.Sleep(rwTagDelay*2);
                                        }
                                    }
                                    else
                                    {//check raw tag
                                        //reserve section is require 4 words / 8 bytes
                                        tbQCTagStatus.Text = TagQC.TagAccessCodeText.LOCKSIZE; // "Reserve section requires 8 bytes";
                                        if (strData != null && strData.Length / 3 == 8)
                                            tbAccessCodeResult = SetStatusResult(tbAccessCodeResult, TagQC.TagStatus.Pass);                                         
                                        else
                                            tbAccessCodeResult = SetStatusResult(tbAccessCodeResult, TagQC.TagStatus.Fail);
                                        
                                        tbDataResult.Refresh();
                                        XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.writeUserData;
                                        reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, RFIDTagInfo.accessCode);
                                        Thread.Sleep(rwTagDelay*2);
                                    }                                   
                                }
                                else
                                {
                                    WriteLog(lrtxtLog, "*** Verified Access Code Failed ***", 1);
                                }
                            }
                        }
                        else if(strData.Length / 3 == 24)
                        {//read TID
                            tagLists[index].tagStatus = RFIDTagData.TagStatus.TIDUpdated;
                            tagLists[index].tagTIDInfo = strData.Trim();
                            reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, RFIDTagInfo.accessCode);
                            Thread.Sleep(rwTagDelay*2);
                        }
                        else if (strData.Length / 3 >= 40)
                        {
                            string reserveData = "";
                            ListViewItem item1 = null;
                            if (((tagLists[index].tagStatus == RFIDTagData.TagStatus.DataUpdated) ||
                                (tagLists[index].tagStatus == RFIDTagData.TagStatus.DataErased)) &&
                                (XMLRPC.xmlRpcStatus != XMLRPC.XmlRPCTagStatus.WriteDataDone))
                            {//don't need to verify again
                                return;
                            }
                            if (RFIDTagInfo.reserverData != null && RFIDTagInfo.reserverData != "")
                            {
                                reserveData = RFIDTagInfo.reserverData.Substring(0, 12).Trim();
                            }
                            else
                            {
                                if (tabCtrMain.SelectedTab == pageQC)
                                {
                                    if (tbQCTagID.Text == TagQC.TagIDText.EMPTY)
                                    {// reserve data existed
                                        tbQCTagData.Text = TagQC.TagDataText.DATASIZE; // "Data section requires 60 bytes";
                                        if (strData.Length / 3 == 64)
                                            tbDataResult = SetStatusResult(tbDataResult, TagQC.TagStatus.Pass);
                                        else
                                            tbDataResult = SetStatusResult(tbDataResult, TagQC.TagStatus.Fail);

                                        tbDataResult.Refresh();
                                        XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.Test;
                                        return;
                                    }
                                    if (tbQCTagStatus.Text == TagQC.TagAccessCodeText.DEFAULT)
                                    {
                                        reader.ReadTag(m_curSetting.btReadId, 0, 0, RFIDTagInfo.RESERVESIZE, RFIDTagInfo.accessCode);
                                        Thread.Sleep(rwTagDelay);
                                        return;
                                    }
                                    if(tbQCTagData.Text == TagQC.TagDataText.DEFAULT)
                                    {
                                        tbQCTagData.Text = "Read data failed!";
                                        tbDataResult = SetStatusResult(tbDataResult, TagQC.TagStatus.Fail);
                                    }
                                }                                                              
                            }

                            if (reserveData == "00 00 00 00")
                            {//no reserve data existed
                                if (!strData.Trim().StartsWith("00 00 00 00"))
                                {
                                    tagLists[index].tagStatus = RFIDTagData.TagStatus.DataErased;
                                    if (tabCtrMain.SelectedTab == pageData || tabCtrMain.SelectedTab == pageQC)
                                    {
                                        if (!tbOdooStatus.Visible)
                                        {
                                            tbOdooStatus.Visible = true;
                                            tbOdooStatus.Text = "Error, tag has already authenticated";
                                            tbOdooStatus.ForeColor = Color.Red;
                                            tbOdooStatus.BackColor = Color.White;
                                            RFIDTagInfo.playSound(false);
                                        }
                                        tbQCTagData.Text = TagQC.TagDataText.USED; // "Tag has already authenticated";
                                        tbDataResult = SetStatusResult(tbDataResult, TagQC.TagStatus.Fail);
                                    }
                                }
                                bVerify = false;
                            }
                            else
                            {
                                bVerify = verifyTag(reserveData.ToUpper() + strData, index);
                            }
                            if (bVerify)
                            {/*
                                foreach (ListViewItem item in listViewEPCTag.Items)
                                {
                                    if (item.SubItems[0].Text.Trim().Contains(strEPC.Trim()))
                                    {
                                        item1 = item;
                                        item.ForeColor = Color.WhiteSmoke;
                                        break;
                                    }
                                }*/   
                                tagLists[index].tagStatus = RFIDTagData.TagStatus.DataUpdated;

                                if(tabCtrMain.SelectedTab == pageEpcID)
                                {
                                    return;
                                }
                                else if (tabCtrMain.SelectedTab == pageQC)
                                {
                                    tbQCTagData.Text = tagLists[index].tagInfo;
                                    string [] tagIDData = tbQCTagData.Text.Split(RFIDTagInfo.serialSep);
                                    if(tagIDData[0].Substring(2) == tagLists[index].EPC_PS_Num.ToString())
                                        tbDataResult = SetStatusResult(tbDataResult, TagQC.TagStatus.Pass);
                                    else
                                        tbDataResult = SetStatusResult(tbDataResult, TagQC.TagStatus.Fail);
                                    tbDataResult.Refresh();

                                    tbQCTestCnt.Text = "Skip write test";
                                    tbWriteTestResult = SetStatusResult(tbWriteTestResult, TagQC.TagStatus.Pass);
                                    return;
                                }

                                if ((XMLRPC.xmlRpcStatus != XMLRPC.XmlRPCTagStatus.OdooSuccessful) &&
                                    (XMLRPC.xmlRpcStatus == XMLRPC.XmlRPCTagStatus.WriteDataDone) &&
                                    (tagLists[index].OdooTagInfo != ""))
                                {
#if Connect2Odoo
                                    XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.Update2Odoo;
                                    int result = xmlRpc.writeRFIDTag(tagLists[index].OdooTagInfo, tagLists[index].tagTIDInfo);
                                    if (result == 0)
                                    {                                 
                                        WriteLog(lrtxtLog,"Verified Data Successful " + tagLists[index].EPC_ID, 0);
                                        //WriteLog(lrtxtLog, "*** Data Read ***" + RFIDTagInfo.tagInfo, 0);

                                        WriteLog(lrtxtLog, "*** Click \"Update\" on WebPage Now ***", 0);
                                        ////tbDataUpdateStatus.Text = "Click RECORD PRODUCTION on WebPage Now";
                                        /// XMLRPC.xmlRpcStatus = XMLRPC.XmlRPCTagStatus.OdooSuccessful;
                                        //udpate tag status for done or content updated 
#if true
                                        if (tbOdooStatus.ForeColor != Color.Green || tbOdooStatus.Visible == false)
                                        {
                                            tbOdooStatus.Visible = true;
                                            tbOdooStatus.Text = "Tag " + tagLists[index].OdooTagInfo +
                                                                " Activated. Press \"UPDATE\" ->  \"RECORD PRODCUTION\" on Odoo";
                                            tbOdooStatus.ForeColor = Color.Green;
                                            tbOdooStatus.BackColor = Color.White;
                                            RFIDTagInfo.playSound(true);
                                        }
#endif
                                        //RFIDTagInfo.playSound(true);

                                    }
                                    else if (result == -1)
                                    {//duplicate tag
                                       XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.OdooFail;
                                        WriteLog(lrtxtLog, "*** Get Exception from Odoo ***" + tagLists[index].EPC_ID, 1);
                                        WriteLog(lrtxtLog, "*** Serial number not accecpted, try again ***", 1);
                                        //tbDataUpdateStatus.Text = "Get Exception from Odoo, try again";
                                        //item1.ForeColor = Color.Red;
                                        if (!tbOdooStatus.Visible)
                                        {
                                            tbOdooStatus.Visible = true;
                                            tbOdooStatus.Text = "Error, duplicate tag";
                                            tbOdooStatus.ForeColor = Color.Red;
                                            tbOdooStatus.BackColor = Color.White;
                                            RFIDTagInfo.playSound(false);
                                        }
                                    }
                                    else {//press record production                            
                                        if (!tbOdooStatus.Visible)
                                        {
                                            //xmlRpcStatus = XMLRPC.XmlRPCTagStatus.OdooFail;
                                            WriteLog(lrtxtLog, "*** Get Exception from Odoo ***" + tagLists[index].EPC_ID, 1);
                                            WriteLog(lrtxtLog, "*** Press RECORD PRODUCTION ***", 1);
                                            //tbDataUpdateStatus.Text = "Get Exception from Odoo, try again";
                                            //item1.ForeColor = Color.Red;
                                            tbOdooStatus.Visible = true;
                                            tbOdooStatus.Text = "Error, Click \"RECORD PRODUCTION\" on WebPage Now";
                                            tbOdooStatus.ForeColor = Color.Red;
                                            tbOdooStatus.BackColor = Color.White;
                                            RFIDTagInfo.playSound(false);
                                        }
                                    }
#else
                                    if(true)
#endif
                                }
                                else
                                {
                                    //tbDataUpdateStatus.Text = "Can't overwrite existed data, Move to next RFID Tag";
                                }                               
                            }
                            else if ((tagLists[index].tagStatus != RFIDTagData.TagStatus.DataUpdated) &&
                                     (tagLists[index].tagStatus != RFIDTagData.TagStatus.DataNotUpdate) &&
                                     (tagLists[index].tagStatus != RFIDTagData.TagStatus.DataErased))
                            {
                                tagLists[index].tagStatus = RFIDTagData.TagStatus.DataNotUpdate;

                                if (tabCtrMain.SelectedTab == pageQC)
                                {
                                    tbQCTagData.Text = TagQC.TagDataText.EMPTY; // "Data verification failed";
                                    tbDataResult = SetStatusResult(tbDataResult, TagQC.TagStatus.Fail);
                                }
                                else if (tabCtrMain.SelectedTab == pageEpcID)
                                {
                                    WriteLog(lrtxtLog, "Data is empty ", 0);
                                }
                                else if(tabCtrMain.SelectedTab == pageData)
                                { 
                                    WriteLog(lrtxtLog, "verified data failed, retry now " + tagLists[index].EPC_ID, 1);
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
                    tbOdooStatus.Visible = true;
                    tbOdooStatus.Text = "Error, write tag failed";
                    tbOdooStatus.ForeColor = Color.Red;
                    tbOdooStatus.BackColor = Color.White;
                    RFIDTagInfo.playSound(false);

                }
            }
        }
        
        //private string ellapsed;
        private void ProcessWriteTag(Reader.MessageTran msgTran)
        {
            string strCmd = "Write Tag";
            string strErrorCode = string.Empty;
            byte btWordCnt = 0;

            if (msgTran.AryData.Length == 1)
            {
                if (XMLRPC.xmlRpcStatus== XMLRPC.XmlRPCTagStatus.WriteDataDone)
                   XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.writeUserData;

                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + " Failure, failure cause1: " + strErrorCode;

                WriteLog(lrtxtLog, strLog, 1);
                if (writeTagRetry++ < rwTagRetryMAX)
                {                    
                    byte[] btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
                    byte[] btAryWriteData = CCommondMethod.String2ByteArray(strHEXdata.ToUpper(), 2, out btWordCnt);

                    switch(XMLRPC.xmlRpcStatus)
                    {
                        case XMLRPC.XmlRPCTagStatus.WriteAccessCode:
                            reader.WriteTag(m_curSetting.btReadId, btAryPwd, 0, 2, btWordCnt, btAryWriteData, 0x94);
                            break;
                        case XMLRPC.XmlRPCTagStatus.WriteID:
                            reader.WriteTag(m_curSetting.btReadId, btAryPwd, 1, 2, btWordCnt, btAryWriteData, 0x94);
                            break;
                        case XMLRPC.XmlRPCTagStatus.writeUserData:
                            reader.WriteTag(m_curSetting.btReadId, btAryPwd, 3, 0, btWordCnt, btAryWriteData, 0x94);
                            break;
                    }
                    WriteLog(lrtxtLog, " Write Tag retry " + writeTagRetry, 1);                    
                    Thread.Sleep(rwTagDelay * (writeTagRetry % 3 + 2));
                }
                else
                {//write tag failed
                    //setVerifiedLEDStatus(0, 1); //red on 
                    writeTagRetry = 0;
                    timerInventory.Enabled = false;
                    timerInventory.Interval = 200;
                    initScanTag(false);
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
                //WriteLog(lrtxtLog, strCmd + " read back: " + strData + " count " +
                //         (msgTran.AryData[0] * 256 + msgTran.AryData[1]), 0);

                if (bVerify)
                {
                    bVerify = false;
                }

                if (WriteTagCount == (msgTran.AryData[0] * 256 + msgTran.AryData[1]))
                {
                    WriteTagCount = 0;
                }
                byte[] btAryPwd = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
                byte[] byteData = CCommondMethod.String2ByteArray(strHEXdata.ToUpper(), 2, out btWordCnt);
#if DEBUG
                //WriteLog(lrtxtLog, " Write Tag state " + xmlRpcStatus, 0);
#endif
                switch (XMLRPC.xmlRpcStatus)
                {
                    case XMLRPC.XmlRPCTagStatus.WriteIDOk:
                        return;
                    case XMLRPC.XmlRPCTagStatus.WriteID:
                        {
                           XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.WriteIDOk;
                            reader.ReadTag(m_curSetting.btReadId, 0, 0, RFIDTagInfo.RESERVESIZE, btAryPwd);
                            Thread.Sleep(rwTagDelay);
                        }
                        break;
                    case XMLRPC.XmlRPCTagStatus.WriteAccessCode:
                        {
                           XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.WriteAccessOk;
                            reader.ReadTag(m_curSetting.btReadId, 0, 0, RFIDTagInfo.RESERVESIZE, btAryPwd);
                            Thread.Sleep(rwTagDelay);
                        }
                        break;
                    case XMLRPC.XmlRPCTagStatus.WriteReserveData:
                        {
                           XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.writeUserData;
                            reader.WriteTag(m_curSetting.btReadId, btAryPwd, 3, 0, btWordCnt, byteData, 0x94);
                            Thread.Sleep(rwTagDelay * 3);
                        }
                        break;
                    case XMLRPC.XmlRPCTagStatus.writeUserData:
                        if (RFIDTagInfo.reserverData.Trim().StartsWith("00 00 00 00"))
                        {
                           XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.WriteDataDone;
                            reader.ReadTag(m_curSetting.btReadId, 0, 0, RFIDTagInfo.RESERVESIZE, btAryPwd);
                        }
                        else
                        {
                            reader.ReadTag(m_curSetting.btReadId, 3, 0, RFIDTagInfo.DATASIZE, btAryPwd);
                            //WriteLog(lrtxtLog, " Write -> Read Tag data section", 0);
                        }
                        Thread.Sleep(rwTagDelay*2);
                        break;
                    case XMLRPC.XmlRPCTagStatus.Test:
                        RFIDTagInfo.writeTestReceiveCount++;
                        //WriteLog(lrtxtLog, "Receive write test" + RFIDTagInfo.writeTestReceiveCount, 0);
                        break;
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
                    //WriteLog(lrtxtLog, strCmd, 0);
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
#if DEBUG
                //RefreshOpTag(0x83);
                //WriteLog(lrtxtLog, strCmd, 0);
#endif
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
#if DEBUG
                //RefreshOpTag(0x84);
                WriteLog(lrtxtLog, strCmd, 0);
#endif
            }
        }

        private delegate int findTagUnSafe(tagSelect selectTag);
        private int findTag(tagSelect selectTag)
        {
            if (this.InvokeRequired)
            {
                findTagUnSafe InvokeFindTag = new findTagUnSafe(findTag);
                this.Invoke(InvokeFindTag, new object[] { selectTag });
            }
            int readCount = 0;
            int readRSSI = 60;
            int maxCountIndex = -1;

            for (int i = 0; i < tagLists.Count; i++)
            {
                if ((readCount < (tagLists[i].readCount - tagLists[i].notUpdateCount*2)) &&
                    /*(readRSSI < tagLists[i].rssi) &&*/ (tagLists[i].rssi >= RFIDReader.RSSI_MIN))
                {
#if DEBUG
                    //WriteLog(lrtxtLog, "Get tag " + tagLists[i].EPC_ID + ", rssi " + tagLists[i].rssi, 0);
#endif
                    if (tagLists[i].tagStatus == RFIDTagData.TagStatus.IDUpdated ||
                        tagLists[i].tagStatus == RFIDTagData.TagStatus.IDNotUpdate)
                       XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.Ready;

                    switch(tagLists[i].tagStatus)
                    {
                        case RFIDTagData.TagStatus.IDUpdated:
                        case RFIDTagData.TagStatus.IDNotUpdate:                       
                        case RFIDTagData.TagStatus.AccessCodeNotUpdate:
                            {
                                if (selectTag == tagSelect.writeID ||
                                    selectTag == tagSelect.Test)
                                {
                                    readCount = tagLists[i].readCount;
                                    readRSSI = tagLists[i].rssi;
                                    maxCountIndex = i;

                                    if (tagLists[i].tagStatus == RFIDTagData.TagStatus.AccessCodeNotUpdate &&
                                        !tbDataVerify.Visible)
                                    {
                                        tbDataVerify.Visible = true;
                                    }
                                }
                            }break;                        
                        case RFIDTagData.TagStatus.AccessCodeUpdated:
                        case RFIDTagData.TagStatus.TIDUpdated:
                        case RFIDTagData.TagStatus.DataNotUpdate:
                            {
                                if (selectTag == tagSelect.writeData ||
                                    selectTag == tagSelect.Test)
                                {
                                    readCount = tagLists[i].readCount;
                                    readRSSI = tagLists[i].rssi;
                                    maxCountIndex = i;

                                    if (!tbDataVerify.Visible)
                                    {
                                        tbDataVerify.Visible = true;
                                    }
                                }
                            }break;
                        case RFIDTagData.TagStatus.DataUpdated:
                            {
                                if(tabCtrMain.SelectedTab == pageEpcID)
                                {
                                    if (!tbDataVerify.Visible)
                                    {
                                        tbDataVerify.Visible = true;
                                    }
                                }
                                else if(XMLRPC.xmlRpcStatus== XMLRPC.XmlRPCTagStatus.Ready ||
                                       XMLRPC.xmlRpcStatus== XMLRPC.XmlRPCTagStatus.writeUserData ||
                                       XMLRPC.xmlRpcStatus== XMLRPC.XmlRPCTagStatus.WriteDataDone ||
                                       XMLRPC.xmlRpcStatus== XMLRPC.XmlRPCTagStatus.Update2Odoo)
                                {
                                    readCount = tagLists[i].readCount;
                                    readRSSI = tagLists[i].rssi;
                                    maxCountIndex = i;
                                }
                                break;
                            }
                    }
                }
            }                     
            return maxCountIndex;
        }
        
        private void setAccessCode(int index)
        {//write access code, it can be empty or update before
            writeTagRetry = 0;

            string[] result = { "00", "00", "00", "00" };
            byte[] byteAryPwd = { 0, 0, 0, 0 };
            byte[] byteAryWriteData = null;
            
            byte btWordCnt = 2;                     
            //load Access Code
            symmetric.loadAccessCode(RFIDTagInfo.ASCIIToHex(Encoding.ASCII.GetString(Properties.Resources.AccessCode)).ToUpper());

            if (RFIDTagInfo.bAccessCode(symmetric.readAccessCode(), RFIDTagInfo.reserverData))
            {
                result = CCommondMethod.String2StringArray(symmetric.readAccessCode(), 2);
            }
            byteAryPwd = CCommondMethod.StringArray2ByteArray(result, result.Length);            
            byteAryWriteData = CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
#if DEBUG
            //1, access, 0, 2, 2, data, 148
            //WriteLog(lrtxtLog, " Write Tag Access code 0 2" + btWordCnt, 0);
#endif
            reader.WriteTag(m_curSetting.btReadId, byteAryPwd, 0, 2, 2, byteAryWriteData, 0x94);
            RFIDTagInfo.reserverData = symmetric.readAccessCode();
            Thread.Sleep(rwTagDelay*3);

            //lock tag
            reader.LockTag(m_curSetting.btReadId, byteAryWriteData, 4, 0x01);
            Thread.Sleep(rwTagDelay*2);
            /*
            if (sender != null)
            {
                btWordAddr = 0;
                btWordCnt = 4;
                reader.ReadTag(m_curSetting.btReadId, btMemBank, btWordAddr, btWordCnt, byteAryWriteData);
                Thread.Sleep(rwTagDelay);
            }
            */
        }

        private void listViewEPCTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabCtrMain.SelectedTab == pageEpcID)
                {
                    String text = listViewEPCTag.SelectedItems[0].Text;
                    resetStatusColor();
                    checkTagStatus(text);
                }
            }
            catch (Exception exp) { return; }
        }

        private void selectTag(string tagID)
        {
            byte wCnt = 0;
            byte[] btAryEpc = CCommondMethod.String2ByteArray(tagID.ToUpper(), 2, out wCnt);
            reader.SetAccessEpcMatch(m_curSetting.btReadId, 0x00, Convert.ToByte(btAryEpc.Length), btAryEpc);
#if DEBUG
            //WriteLog(lrtxtLog, "Select Tag: " + tagID, 0);
#endif
            Thread.Sleep(rwTagDelay);            
        }
                
        private void checkTagStatus(string tagID)
        {
            byte btWordCnt = 0;
            try
            {
                //tbSelectTag.Text = tagID;
                int index = findTagIndex(tagID.Trim());
                if (index == -1)
                    return;

                if (tabCtrMain.SelectedTab != pageQC)
                {
                    switch (tagLists[index].tagStatus)
                    {
                        case RFIDTagData.TagStatus.IDNotUpdate:
                        case RFIDTagData.TagStatus.DataUpdated:
                        case RFIDTagData.TagStatus.DataErased:
                            //no need to read status
                            return;
                        default:
                            RFIDTagInfo.accessCode =
                                CCommondMethod.String2ByteArray(symmetric.readAccessCode(), 2, out btWordCnt);
                            break;
                    }
                }
                selectTag(tagID);
                reader.ReadTag(m_curSetting.btReadId, 0, 0, RFIDTagInfo.RESERVESIZE, RFIDTagInfo.accessCode);
#if DEBUG
                //WriteLog(lrtxtLog, "Read Tag access code: " + tagID, 0);
#endif
                Thread.Sleep(rwTagDelay*2);               
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
                        item.ForeColor = Color.White;
                    }
                }
                if (splitContainer2.Panel2Collapsed == true)
                {
                    btShowDebug.Text = "Show log";
                    splitContainer2.Panel2Collapsed = true;
                    //splitContainer2.Panel2.Hide();                
                    this.Height = 369;
                }
                else
                {
                    btShowDebug.Text = "Hide log";
                    //splitContainer2.Panel2.Show();
                    splitContainer2.Panel2Collapsed = false;
                    this.Height = 551;
                }
            }
            else if(tabCtrMain.SelectedTab == pageEpcID)
            {
                if (splitContainer2.Panel2Collapsed == true)
                {//ID
                    btShowDebug1.Text = "Show log";
                    splitContainer2.Panel2Collapsed = true;
                    //splitContainer2.Panel2.Hide();
                    this.Height = 369;
                }
                else
                {
                    btShowDebug1.Text = "Hide log";
                    splitContainer2.Panel2Collapsed = false;
                    //splitContainer2.Panel2.Show();
                    this.Height = 551;
                }
            }           
            else
            {//QC
                checkBoxShowDetail.Checked = true;
                ckClearOperationRec.Checked = false;
                splitContainer2.Panel2Collapsed = false;
                btShowDebugQC.Text = "Hide log";
                this.Height = 551;
            }
            XMLRPC.xmlRpcStatus= XMLRPC.XmlRPCTagStatus.Ready;

            tagLists.Clear();
            resetStatusColor();
            initScanTag(true);
        }
        
        private void checkBoxShowDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowDetail.Checked)
            {
                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
            }
        }

        private void RFIDTagIDForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(loginForm != null)
            {
                loginForm.Close();                
            }
            timerInventory.Enabled = false;
            reader.resetCom();
            reader.CloseCom();
        }
        
        private void btShowDebug_Click(object sender, EventArgs e)
        {
            if (splitContainer2.Panel2Collapsed == false)
            {
                btShowDebug.Text = "Show log";
                splitContainer2.Panel2Collapsed = true;
                //splitContainer2.Panel2.Hide();                
                this.Height = 369;
            }
            else
            {
                btShowDebug.Text = "Hide log";
                //splitContainer2.Panel2.Show();
                splitContainer2.Panel2Collapsed = false;
                this.Height = 551;
            }
        }

        private void btShowDebug1_Click(object sender, EventArgs e)
        {
            if (splitContainer2.Panel2Collapsed == false)
            {//ID
                btShowDebug1.Text = "Show log";
                splitContainer2.Panel2Collapsed = true;
                //splitContainer2.Panel2.Hide();
                this.Height = 369;
            }
            else
            {
                btShowDebug1.Text = "Hide log";
                splitContainer2.Panel2Collapsed = false;
                //splitContainer2.Panel2.Show();
                this.Height = 551;
            }
        }
        private void btShowDebugQC_Click(object sender, EventArgs e)
        {
            if (splitContainer2.Panel2Collapsed == false)
            {
                btShowDebugQC.Text = "Show log";
                splitContainer2.Panel2Collapsed = true;
                //splitContainer2.Panel2.Hide();
                this.Height = 369;
            }
            else
            {
                btShowDebugQC.Text = "Hide log";
                splitContainer2.Panel2Collapsed = false;
                //splitContainer2.Panel2.Show();
                this.Height = 551;
            }
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

        private void listViewEPCTag_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            FontFamily fontFamily = new FontFamily("Arial");
            Font font = new Font(
               fontFamily,
               12,
               FontStyle.Regular,
               GraphicsUnit.Pixel);

            System.Drawing.Drawing2D.LinearGradientBrush GradientBrush = 
                new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, 
                Color.FromArgb(25, 69, 88, 136), 
                Color.FromArgb(125, 69, 88, 136), 270);
            e.Graphics.FillRectangle(GradientBrush, e.Bounds);
            e.Graphics.DrawLine(SystemPens.ControlLightLight, e.Bounds.X, (e.Bounds.Y + 20), e.Bounds.Right, (e.Bounds.Y + 20));
            //e.Graphics.DrawLine(SystemPens.ControlLightLight, (e.Bounds.X + 1), e.Bounds.Bottom, e.Bounds.Right, e.Bounds.Bottom);
            //e.Graphics.DrawLine(SystemPens.ControlLightLight, e.Bounds.Right, (e.Bounds.Y + 1), e.Bounds.Right, e.Bounds.Bottom);
            /*e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.White), 2), 
                e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2);*/
            e.Graphics.DrawString(e.Header.Text, font, new SolidBrush(Color.White), e.Bounds);
        }

        private void listViewEPCTag_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listViewEPCTag_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.Item.ForeColor = Color.White;
            e.DrawDefault = true;
        }
        
        private void btClose_Click(object sender, EventArgs e)
        {

            this.Close();
            loginForm.Close();
        }

        private void btHelp_Click(object sender, EventArgs e)
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
        
        private void splitContainer2_Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            bFormMoveflag = true;
        }

        private void splitContainer2_Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (bFormMoveflag == true)
            {
                this.Location = Cursor.Position;
            }
        }

        private void splitContainer2_Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            bFormMoveflag = false;
        }

    }

}
