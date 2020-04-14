//#define SCAN2READ
//#define READ2SCAN
//#define READ2ERASE
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

namespace RFIDApplication
{

    public partial class R2000UartDemo : Form
    {
        private Reader.ReaderMethod reader;

        private ReaderSetting m_curSetting = new ReaderSetting();
        private InventoryBuffer m_curInventoryBuffer = new InventoryBuffer();
        private OperateTagBuffer m_curOperateTagBuffer = new OperateTagBuffer();
        Symmetric_Encrypted symmetric = new Symmetric_Encrypted();
        //private OperateTagISO18000Buffer m_curOperateTagISO18000Buffer = new OperateTagISO18000Buffer();

        //private const string settingPath = @"c:\TEMP\Setting.dat";
        private const int ACCESSCodeMAXLENGTH = 2; //4 wrods

        //Before inventory, you need to set working antenna to identify whether the inventory operation is executing.
        private bool m_bInventory = false;
        //Identify whether reckon the command execution time, and the current inventory command needs to reckon time.
        //private bool m_bReckonTime = false;
        //Real time inventory locking operation.
        private bool m_bLockTab = false;
        //Whether display the serial monitoring data.
        private bool m_bDisplayLog = false;
        //Record the number of ISO18000 tag written loop time.
        //private int m_nLoopTimes = 0;
        //Record the number of ISO18000 tag's written characters.
        //private int m_nBytes = 0;
        //Record the number of ISO18000 tag have been written loop time.
        //private int m_nLoopedTimes = 0;
        //Real time inventory times.
        private int m_nTotal = 0;
        //Frequency of list updating.
        private int m_nRealRate = 20;
        //Record quick poll antenna parameter.
        private byte[] m_btAryData = new byte[18];
        private byte[] m_btAryData_4 = new byte[10];
        //Record the total number of quick poll times.
        private int m_nSwitchTotal = 0;
        private int m_nSwitchTime = 0;
        private int m_nReceiveFlag = 0;

        private volatile bool m_nPhaseOpened = false;
        private volatile bool m_nSessionPhaseOpened = false;
        public R2000UartDemo()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            lvRealList.SmallImageList = sortImageList;


            this.columnHeader37.ImageIndex = 0;
            this.columnHeader38.ImageIndex = 0;

            this.refreshLvListView();
            this.columnHeader37.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
        }

        private void R2000UartDemo_Load(object sender, EventArgs e)
        {
            bool bfoundSettingFile = false;
            reader = new Reader.ReaderMethod();

            reader.AnalyCallback = AnalyData;
   
            gbRS232.Enabled = true;
            gbTcpIp.Enabled = false;
            SetFormEnable(false);
            rdbRS232.Checked = true;
            antType1.Checked = true;
            
            cmbComPort.Items.AddRange(SerialPort.GetPortNames());
            cmbComPort.SelectedIndex = 1;

            //load LabelFormat":
            byte[] labelFormat = Properties.Resources.LabelFormat;
            RFIDTagInfo.loadLabelFormat(labelFormat);
            WriteLog(lrtxtLog,"read label format " + RFIDTagInfo.readLabelFormat(),0);

            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            using (StreamReader sr = new StreamReader(path + "\\Setting.dat"))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    string[] param = line.Split(',');
                    switch (param[0])
                    {
                        case "ComPort":
                            {
                                for (int j = 0; j < cmbComPort.Items.Count; j++)
                                {
                                    if (param[1] == cmbComPort.Items[j].ToString())
                                    {
                                        cmbComPort.SelectedIndex = j;
                                        bfoundSettingFile = true;
                                        break;
                                    }
                                }
                            }
                            break;
                            /*
                        case "TonerVolumeCount":
                            {
                                if (param[1] != "")
                                {
                                    RFIDTagInfo.loadVolumeFile(param[1]);
                                }
                                break;
                            }*/
                    }
                }
            }
           
            cmbBaudrate.SelectedIndex = 1;
            ipIpServer.IpAddressStr = "192.168.0.178";
            txtTcpPort.Text = "4001";
            m_session_sl.SelectedIndex = 0;
                        
            //rdbInventoryRealTag_CheckedChanged(sender, e);
            cmbSession.SelectedIndex = 0;
            cmbTarget.SelectedIndex = 0;
            cmbReturnLossFreq.SelectedIndex = 33;
            if (cbUserDefineFreq.Checked == true)
            {
                groupBox21.Enabled = false;
                groupBox23.Enabled = true;

            }
            else
            {
                groupBox21.Enabled = true;
                groupBox23.Enabled = false;
            };

            //ListView settings
            this.lvRealList.ListViewItemSorter = new ListViewColumnSorter();
            ListViewHelper lvRealHelper = new ListViewHelper();
            lvRealHelper.addSortColumn(0);
            lvRealHelper.addSortColumn(1);
            this.lvRealList.ColumnClick += new ColumnClickEventHandler(lvRealHelper.ListView_ColumnClick);

            //this.lvBufferList.ListViewItemSorter = new ListViewColumnSorter();
            //ListViewHelper lvBufferHelper = new ListViewHelper();
            //lvBufferHelper.addSortColumn(0);
            //lvBufferHelper.addSortColumn(3);
            //this.lvBufferList.ColumnClick += new ColumnClickEventHandler(lvBufferHelper.ListView_ColumnClick);
            this.m_real_phase_value.SelectedIndex = 0;

            if(bfoundSettingFile)
            {
#if true
                btnConnectRs232.Enabled = true;
                btnDisconnectRs232.Enabled = false;
                btnConnectRs232_Click(sender, e);// auto start                                                 
                btnGetFirmwareVersion_Click(sender, e);//Get “Firmware Version” – read RFID current firmware version
                //Read “Read GPIO” – get GPIO status
                btnReadGpioValue_Click(sender, e);
                //Get “Current Ant” – get current antenna
                btnGetWorkAntenna_Click(sender, e);
                //Get “RF Output Power” – read current RF power setting
                btnGetOutputPower_Click(sender, e);
                //Get “RF Spectrum Setup: -read RF frequencies
                btnGetFrequencyRegion_Click(sender, e);
                //btRealTimeInventory_Click(sender, e);// auto start

                reader.WriteGpioValue(m_curSetting.btReadId, 0x03, 0);
                reader.WriteGpioValue(m_curSetting.btReadId, 0x04, 0);
                Thread.Sleep(20);
#else
                string strException = string.Empty;
                int nRet = reader.OpenCom(cmbComPort.Text, Convert.ToInt32(cmbBaudrate.Text), out strException);
                if (nRet != 0)
                {
                    string strLog = "Connection failed, failure cause: " + strException; 
                    WriteLog(lrtxtLog,strLog, 1);
                    return;
                }
                else
                {
                    string strLog = "Connect " + cmbComPort.Text + "@" + cmbBaudrate.Text;
                    WriteLog(lrtxtLog,strLog, 0);
                }            
                SetFormEnable(true);
#endif
                initScanTag();
            }
        }

        private delegate void SwitchTagUnSafe(TabControl tab, int index);
        public void SwitchTag(TabControl tab, int selectIndex)
        {
            if (this.InvokeRequired)
            {
                SwitchTagUnSafe InvokeWriteLog = new SwitchTagUnSafe(SwitchTag);
                this.Invoke(InvokeWriteLog, new object[] { tab, selectIndex });
            }
            else
            {
                tab.SelectedIndex = selectIndex;
            }
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
            totalTime.Enabled = true;

            RFIDTagInfo.label = "";
            RFIDTagInfo.tagInfo = "";
            //txtAccessEpcMatch.Text = "";
            RFIDTagInfo.setToScan();
            //btMemBank = 3;
            
            SwitchTag(tabCtrMain, 1);
            SwitchTag(tabEpcTest, 0);
        }

        private void refreshLvListView()
        {
            if (this.m_session_q_cb.Checked)
            {
                this.columnHeader37.Width = 53;
                this.columnHeader38.Width = 400;
                this.columnHeader39.Width = 61;
                this.columnHeader40.Width = 211;
                this.columnHeader41.Width = 89;
                this.columnHeader412.Width = 65;
                this.columnHeader42.Width = 117;
            }
            else
            {
                this.columnHeader37.Width = 56;
                this.columnHeader38.Width = 428;
                this.columnHeader39.Width = 65;
                this.columnHeader40.Width = 226;
                this.columnHeader41.Width = 96;
                this.columnHeader412.Width = 0;
                this.columnHeader42.Width = 125;
            }
        }

        private void AnalyData(Reader.MessageTran msgTran)
        {
            m_nReceiveFlag = 0;
            if (msgTran.PacketType != 0xA0)
            {
                return;
            }
            switch(msgTran.Cmd)
            {
                case 0x69:
                    ProcessSetProfile(msgTran);
                    break;
                case 0x6A:
                    ProcessGetProfile(msgTran);
                    break;
                case 0x71:
                    ProcessSetUartBaudrate(msgTran);
                    break;
                case 0x72:
                    ProcessGetFirmwareVersion(msgTran);
                    break;
                case 0x73:
                    ProcessSetReadAddress(msgTran);
                    break;
                case 0x74:
                    ProcessSetWorkAntenna(msgTran);
                    break;
                case 0x75:
                    ProcessGetWorkAntenna(msgTran);
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
                case 0x79:
                    ProcessGetFrequencyRegion(msgTran);
                    break;
                case 0x7A:
                    ProcessSetBeeperMode(msgTran);
                    break;
                case 0x7B:
                    ProcessGetReaderTemperature(msgTran);
                    break;
                case 0x7C:
                    ProcessSetDrmMode(msgTran);
                    break;
                case 0x7D:
                    ProcessGetDrmMode(msgTran);
                    break;
                case 0x7E:
                    ProcessGetImpedanceMatch(msgTran);
                    break;
                case 0x60:
                    ProcessReadGpioValue(msgTran);
                    break;
                case 0x61:
                    ProcessWriteGpioValue(msgTran);
                    break;
                case 0x62:
                    ProcessSetAntDetector(msgTran);
                    break;
                case 0x63:
                    ProcessGetAntDetector(msgTran);
                    break;
                case 0x67:
                    ProcessSetReaderIdentifier(msgTran);
                    break;
                case 0x68:
                    ProcessGetReaderIdentifier(msgTran);
                    break;
                              
                case 0x80:
                    //ProcessInventory(msgTran);
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
                    ProcessGetAccessEpcMatch(msgTran);
                    break;

                case 0x89:
                case 0x8B:
                    ProcessInventoryReal(msgTran);
                    break;
                case 0x8A:
                    ProcessFastSwitch(msgTran);
                    break;
                case 0x8D:
                    ProcessSetMonzaStatus(msgTran);
                    break;
                case 0x8E:
                    ProcessGetMonzaStatus(msgTran);
                    break;
#if false
                case 0x90:
                    //ProcessGetInventoryBuffer(msgTran);
                    break;
                case 0x91:
                    //ProcessGetAndResetInventoryBuffer(msgTran);
                    break;
                case 0x92:
                    //ProcessGetInventoryBufferTagCount(msgTran);
                    break;
                case 0x93:
                    //ProcessResetInventoryBuffer(msgTran);
                    break;
                case 0x98:
                    ProcessTagMask(msgTran);
                    break;

                case 0xb0:
                    //ProcessInventoryISO18000(msgTran);
                    break;
                case 0xb1:
                    //ProcessReadTagISO18000(msgTran);
                    break;
                case 0xb2:
                    //ProcessWriteTagISO18000(msgTran);
                    break;
                case 0xb3:
                    //ProcessLockTagISO18000(msgTran);
                    break;
                case 0xb4:
                    //ProcessQueryISO18000(msgTran);
                    break;
#endif
                case 0xE1:
                    ProcessUntraceable(msgTran);
                    break;
                default:
                    break;
            }
        }

        private void ProcessUntraceable(Reader.MessageTran msgTran)
        {
            string strCmd = "Set Untraceable";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + "F" + strErrorCode;

                WriteLog(lrtxtLog,strLog, 0);
            }
            else
            {
                int nLen = msgTran.AryData.Length;
                int nEpcLen = Convert.ToInt32(msgTran.AryData[2]) - 4;

                if (msgTran.AryData[nLen - 3] != 0x10)
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[nLen - 3]);
                    string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                    WriteLog(lrtxtLog,strLog, 1);
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

                RefreshUntraceable(0xE1);
                //WriteLog(lrtxtLog,strCmd);
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

        private delegate void RefreshiUntraceableUnsaft(byte btCmd);
        private void RefreshUntraceable(byte btCmd)
        {
            if (this.InvokeRequired)
            {
                RefreshiUntraceableUnsaft InvokeRefresh = new RefreshiUntraceableUnsaft(RefreshUntraceable);
                this.Invoke(InvokeRefresh, new object[] { btCmd });
            }
            else
            {
                switch (btCmd)
                {
                    case 0xE1:
                        {
                            int nCount = ltvOperate.Items.Count;
                            int nLength = m_curOperateTagBuffer.dtTagTable.Rows.Count;

                            DataRow row = m_curOperateTagBuffer.dtTagTable.Rows[nLength - 1];

                            ListViewItem item = new ListViewItem();
                            item.Text = (nCount + 1).ToString();
                            item.SubItems.Add(row[0].ToString());
                            item.SubItems.Add(row[1].ToString());
                            item.SubItems.Add(row[2].ToString());
                            item.SubItems.Add(row[3].ToString());
                            item.SubItems.Add(row[4].ToString());
                            item.SubItems.Add(row[5].ToString());
                            item.SubItems.Add(row[6].ToString());

                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private delegate void RefreshOpTagUnsafe(byte btCmd);
        private void RefreshOpTag(byte btCmd)
        {
            if (this.InvokeRequired)
            {
                RefreshOpTagUnsafe InvokeRefresh = new RefreshOpTagUnsafe(RefreshOpTag);
                this.Invoke(InvokeRefresh, new object[] { btCmd });
            }
            else
            {
                switch(btCmd)
                {
                    case 0x81:
                    case 0x82:
                    case 0x83:
                    case 0x84:
                        {
                            int nCount = ltvOperate.Items.Count;
                            int nLength = m_curOperateTagBuffer.dtTagTable.Rows.Count;

                            DataRow row = m_curOperateTagBuffer.dtTagTable.Rows[nLength - 1];

                            ListViewItem item = new ListViewItem();
                            item.Text = (nCount + 1).ToString();
                            item.SubItems.Add(row[0].ToString());
                            item.SubItems.Add(row[1].ToString());
                            item.SubItems.Add(row[2].ToString());
                            item.SubItems.Add(row[3].ToString());
                            item.SubItems.Add(row[4].ToString());
                            item.SubItems.Add(row[5].ToString());
                            item.SubItems.Add(row[6].ToString());

                            ltvOperate.Items.Add(item);
                        }
                        break;
                    case 0x86:
                        {
                            txtAccessEpcMatch.Text = m_curOperateTagBuffer.strAccessEpcMatch;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private delegate void RefreshInventoryRealUnsafe(byte btCmd);
        private void RefreshInventoryReal(byte btCmd)
        {
            if (this.InvokeRequired)
            {
                RefreshInventoryRealUnsafe InvokeRefresh = new RefreshInventoryRealUnsafe(RefreshInventoryReal);
                this.Invoke(InvokeRefresh, new object[] { btCmd });
            }
            else
            {
                switch(btCmd)
                {
                    case 0x89:
                    case 0x8B:
                        {
                            int nTagCount = m_curInventoryBuffer.dtTagTable.Rows.Count;
                            int nTotalRead = m_nTotal;// m_curInventoryBuffer.dtTagDetailTable.Rows.Count;
                            TimeSpan ts = m_curInventoryBuffer.dtEndInventory - m_curInventoryBuffer.dtStartInventory;
                            int nTotalTime = ts.Minutes * 60 * 1000 + ts.Seconds * 1000 + ts.Milliseconds;
                            int nCaculatedReadRate = 0;
                            int nCommandDuation = 0;

                            if (m_curInventoryBuffer.nReadRate == 0) //读写器没有返回速度前软件测速度
                            {
                                if (nTotalTime > 0)
                                {
                                    nCaculatedReadRate = (nTotalRead * 1000 / nTotalTime);
                                }
                            }
                            else
                            {
                                nCommandDuation = m_curInventoryBuffer.nDataCount * 1000 / m_curInventoryBuffer.nReadRate;
                                nCaculatedReadRate = m_curInventoryBuffer.nReadRate;
                            }

                            //列表用变量
                            int nEpcCount = 0;
                            int nEpcLength = m_curInventoryBuffer.dtTagTable.Rows.Count;
                                                       
                            ledReal1.Text = nTagCount.ToString();
                            ledReal2.Text = nCaculatedReadRate.ToString();
                            
                            ledReal5.Text = nTotalTime.ToString();
                            ledReal3.Text = nTotalRead.ToString();
                            ledReal4.Text = nCommandDuation.ToString();  //实际的命令执行时间
                            tbRealMaxRssi.Text = (m_curInventoryBuffer.nMaxRSSI - 129).ToString() + "dBm";
                            tbRealMinRssi.Text = (m_curInventoryBuffer.nMinRSSI - 129).ToString() + "dBm";
                            lbRealTagCount.Text = "标签EPC号列表（不重复）： " + nTagCount.ToString() + "个";

                            nEpcCount = lvRealList.Items.Count;
                            resetLEDstatus();

                            if (nEpcCount < nEpcLength)
                            {
                                DataRow row = m_curInventoryBuffer.dtTagTable.Rows[nEpcLength - 1];

                                ListViewItem item = new ListViewItem();
                                item.Text = (nEpcCount + 1).ToString();
                                item.SubItems.Add(row[2].ToString()); //EPC serial number
                                item.SubItems.Add(row[0].ToString());
                                //item.SubItems.Add(row[5].ToString());
                                //if (antType1.Checked)
                                {
                                    item.SubItems.Add(row[7].ToString());
                                }
                                item.SubItems.Add((Convert.ToInt32(row[4]) - 129).ToString() + "dBm");

                                item.SubItems.Add(row[15].ToString());
                                item.SubItems.Add(row[6].ToString());

                                //set Item backagroud color.
                                //item.BackColor = Color.Red;

                                lvRealList.Items.Add(item);

                                //do not location the scrolling bar in the bottom.
                                //lvRealList.Items[nEpcCount].EnsureVisible();

                            }
                            //更新列表中读取的次数
                            if (m_nTotal % m_nRealRate == 1)
                            {
                                int nIndex = 0;
                                foreach (DataRow row in m_curInventoryBuffer.dtTagTable.Rows)
                                {
                                    ListViewItem item;
                                    item = lvRealList.Items[nIndex];
                                    //item.SubItems[3].Text = row[5].ToString();
                                    if (antType1.Checked) {
                                        item.SubItems[3].Text = (row[7].ToString());
                                    }
                                    item.SubItems[4].Text = (Convert.ToInt32(row[4]) - 129).ToString() + "dBm";

                                    if (m_nSessionPhaseOpened)
                                    {
                                        item.SubItems[5].Text = row[15].ToString();
                                        item.SubItems[6].Text = row[6].ToString();
                                    }
                                    else
                                    {
                                        item.SubItems[6].Text = row[6].ToString();
                                    }
                                    nIndex++;
#if SCAN2READ
                                    try
                                    {

                                        string labelRead = row[2].ToString(); 
                                        string strLabel = RFIDTagInfo.HEXToASCII(labelRead);
                                        bool bIsLabel = RFIDTagInfo.verifyData(strLabel, false, false);                                                                     

                                        if (!bIsLabel)
                                        {
                                            WriteLog(lrtxtLog,"Read tag wrong format skip " + labelRead);                                           
                                            continue;
                                        }
                                        if (RFIDTagInfo.bIsScan())
                                        {//scan a tag, read it now
                                         //verify is same as label format
                                            ulong uWordCnt = 0;                                                                                        
                                            byte btWordCnt;
                                            //btMemBank = 3;
                                            rdbUser.Checked = true;

                                            RFIDTagInfo.label = RFIDTagInfo.readEPCLabel(labelRead, out uWordCnt);
                                            if (uWordCnt <= 99999999999999)
                                                btWordCnt = 22;
                                            else
                                                btWordCnt = 32;

                                            //byte btWordAddr = Convert.ToByte(txtWordAddr.Text);                                         
                                            string[] tmpAccess = CCommondMethod.StringToStringArray(symmetric.readAccessCode(), 2);
                                            byte[] btAryPwd = CCommondMethod.StringArrayToByteArray(tmpAccess, tmpAccess.Length);
                                            string[] reslut = CCommondMethod.StringToStringArray(labelRead.ToUpper(), 2);
                                            byte[] btAryEpc = CCommondMethod.StringArrayToByteArray(reslut, reslut.Length);

                                            m_curOperateTagBuffer.strAccessEpcMatch = labelRead; //cmbSetAccessEpcMatch.Text;
                                            txtAccessEpcMatch.Text = cmbSetAccessEpcMatch.Text = labelRead;
                                            ckAccessEpcMatch.Checked = true;
                                                                                        
                                            if (m_curInventoryBuffer.bLoopInventory)
                                            {// auto stop tag scanning
                                                m_bInventory = false;
                                                m_curInventoryBuffer.bLoopInventory = false;
                                                btRealTimeInventory.BackColor = Color.WhiteSmoke;
                                                btRealTimeInventory.ForeColor = Color.DarkBlue;
                                                btRealTimeInventory.Text = "Inventory";
                                                timerInventory.Enabled = false;
                                                totalTime.Enabled = false;                                            
                                            }

                                            tabCtrMain.SelectedIndex = 1;
                                            tabEpcTest.SelectedIndex = 1;

                                            reader.SetAccessEpcMatch(m_curSetting.btReadId, 0x00, Convert.ToByte(btAryEpc.Length), btAryEpc);
                                            m_curOperateTagBuffer.dtTagTable.Clear();
                                            ltvOperate.Items.Clear();
                                            lvRealList.Items.Clear();

                                            Thread.Sleep(20);
                                            //1, 3, 0, 22, btAryPwd = {80, 48, 67, 75} = {0x50, 0x30, 0x43, 0x4B} = {P0CK}
                                            reader.ReadTag(m_curSetting.btReadId, 3, 0, btWordCnt, btAryPwd);
                                            readTagRetry = 0;
                                            RFIDTagInfo.setToReadWrite();
                                            Thread.Sleep(20);
                                        }
                                        else
                                        {
                                            WriteLog(lrtxtLog,"Reading tag skip scan " + labelRead);
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        //MessageBox.Show(ex.Message);
                                        WriteLog(lrtxtLog,"Reading tag format not support Exception " + ex.Message);
                                        setVerifiedLEDStatus(0, 1);
                                    }
#endif
                                }
                            }                            
                        }
                        break;

                   
                    case 0x00:
                    case 0x01:
                        {
                            m_bLockTab = false;

                            TimeSpan ts = m_curInventoryBuffer.dtEndInventory - m_curInventoryBuffer.dtStartInventory;
                            int nTotalTime = ts.Minutes * 60 * 1000 + ts.Seconds * 1000 + ts.Milliseconds;

                            ledReal5.Text = nTotalTime.ToString();

                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private delegate void RefreshReadSettingUnsafe(byte btCmd);
        private void RefreshReadSetting(byte btCmd)
        {
            if (this.InvokeRequired)
            {
                RefreshReadSettingUnsafe InvokeRefresh = new RefreshReadSettingUnsafe(RefreshReadSetting);
                this.Invoke(InvokeRefresh, new object[] { btCmd });
            }
            else
            {
                htxtReadId.Text = string.Format("{0:X2}", m_curSetting.btReadId);
                switch(btCmd)
                {
                    case 0x6A:

                        break;
                    case 0x68:
                        htbGetIdentifier.Text = m_curSetting.btReaderIdentifier;

                        break;
                    case 0x72:
                        {
                            txtFirmwareVersion.Text = m_curSetting.btMajor.ToString() + "." + m_curSetting.btMinor.ToString();
                        }
                        break;
                    case 0x75:
                        {
                            cmbWorkAnt.SelectedIndex = m_curSetting.btWorkAntenna;
                        }
                        break;
                    case 0x77:
                        {
                            if (antType1.Checked)
                            {
                                if (m_curSetting.btOutputPower != 0 && m_curSetting.btOutputPowers == null)
                                {
                                    textBox1.Text = m_curSetting.btOutputPower.ToString();
                                    m_curSetting.btOutputPower = 0;
                                    m_curSetting.btOutputPowers = null;
                                }
                                else if (m_curSetting.btOutputPowers != null)
                                {
                                    textBox1.Text = m_curSetting.btOutputPowers[0].ToString();
                                    m_curSetting.btOutputPower = 0;
                                    m_curSetting.btOutputPowers = null;
                                }
                            }
                            
                        }
                        break;
                    case 0x97:
                        {
                            
                        }
                        break;
                    case 0x79:
                        {
                            switch(m_curSetting.btRegion)
                            {
                                case 0x01:
                                    {
                                        cbUserDefineFreq.Checked = false;
                                        textStartFreq.Text = "";
                                        TextFreqInterval.Text = "";
                                        textFreqQuantity.Text = "";
                                        rdbRegionFcc.Checked = true;
                                        cmbFrequencyStart.SelectedIndex = Convert.ToInt32(m_curSetting.btFrequencyStart) - 7;
                                        cmbFrequencyEnd.SelectedIndex = Convert.ToInt32(m_curSetting.btFrequencyEnd) - 7;
                                    }
                                    break;
                                case 0x02:
                                    {
                                        cbUserDefineFreq.Checked = false;
                                        textStartFreq.Text = "";
                                        TextFreqInterval.Text = "";
                                        textFreqQuantity.Text = "";
                                        rdbRegionEtsi.Checked = true;
                                        cmbFrequencyStart.SelectedIndex = Convert.ToInt32(m_curSetting.btFrequencyStart);
                                        cmbFrequencyEnd.SelectedIndex = Convert.ToInt32(m_curSetting.btFrequencyEnd);
                                    }
                                    break;
                                case 0x03:
                                    {
                                        cbUserDefineFreq.Checked = false;
                                        textStartFreq.Text = "";
                                        TextFreqInterval.Text = "";
                                        textFreqQuantity.Text = "";
                                        rdbRegionChn.Checked = true;
                                        cmbFrequencyStart.SelectedIndex = Convert.ToInt32(m_curSetting.btFrequencyStart) - 43;
                                        cmbFrequencyEnd.SelectedIndex = Convert.ToInt32(m_curSetting.btFrequencyEnd) - 43;
                                    }
                                    break;
                                case 0x04:
                                    {
                                        cbUserDefineFreq.Checked = true;
                                        rdbRegionChn.Checked = false;
                                        rdbRegionEtsi.Checked = false;
                                        rdbRegionFcc.Checked = false;
                                        cmbFrequencyStart.SelectedIndex = -1;
                                        cmbFrequencyEnd.SelectedIndex = -1;
                                        textStartFreq.Text = m_curSetting.nUserDefineStartFrequency.ToString();
                                        TextFreqInterval.Text = Convert.ToString(m_curSetting.btUserDefineFrequencyInterval * 10);
                                        textFreqQuantity.Text = m_curSetting.btUserDefineChannelQuantity.ToString();
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case 0x7B:
                        {
                            string strTemperature = string.Empty;
                            if (m_curSetting.btPlusMinus == 0x0)
                            {
                                strTemperature = "-" + m_curSetting.btTemperature.ToString() + "℃";
                            }
                            else
                            {
                                strTemperature = m_curSetting.btTemperature.ToString() + "℃";
                            }
                            txtReaderTemperature.Text = strTemperature;
                        }
                        break;
                    case 0x7D:
                        {
                            /*
                            if (m_curSetting.btDrmMode == 0x00)
                            {
                                rdbDrmModeClose.Checked = true;
                            }
                            else
                            {
                                rdbDrmModeOpen.Checked = true;
                            }
                             * */
                        }
                        break;
                    case 0x7E:
                        {
                            textReturnLoss.Text = m_curSetting.btAntImpedance.ToString() + " dB";
                        }
                        break;

                    
                    case 0x8E:
                        {

                        }
                        break;
                    case 0x60:
                        {
                            if (m_curSetting.btGpio1Value == 0x00)
                            {
                                rdbGpio1Low.Checked = true;
                            }
                            else
                            {
                                rdbGpio1High.Checked = true;
                            }

                            if (m_curSetting.btGpio2Value == 0x00)
                            {
                                rdbGpio2Low.Checked = true;
                            }
                            else
                            {
                                rdbGpio2High.Checked = true;
                            }
                        }
                        break;
                    case 0x63:
                        {
                        }
                        break;
                    case 0x98:
                        //getMaskInitStatus();
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
                if ( m_curInventoryBuffer.nIndexAntenna < m_curInventoryBuffer.lAntenna.Count - 1 || m_curInventoryBuffer.nCommond == 0)
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
                                reader.CustomizedInventoryV2(m_curSetting.btReadId,m_curInventoryBuffer.CustomizeSessionParameters.ToArray());
                            }
                            else //实时盘存
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
                //校验是否循环盘存
                else if (m_curInventoryBuffer.bLoopInventory)
                {//enter here
                    m_curInventoryBuffer.nIndexAntenna = 0;
                    m_curInventoryBuffer.nCommond = 0;

                    byte btWorkAntenna = 0; // m_curInventoryBuffer.lAntenna[m_curInventoryBuffer.nIndexAntenna];
                    reader.SetWorkAntenna(m_curSetting.btReadId, btWorkAntenna);//need to use
                    m_curSetting.btWorkAntenna = btWorkAntenna;
                }
            }
            //Thread.Sleep(50);
        }

        private delegate void RunLoopFastSwitchUnsafe();
        private void RunLoopFastSwitch()
        {
            if (this.InvokeRequired)
            {
                RunLoopFastSwitchUnsafe InvokeRunLoopFastSwitch = new RunLoopFastSwitchUnsafe(RunLoopFastSwitch);
                this.Invoke(InvokeRunLoopFastSwitch, new object[] { });
            }
        }
                
        private void rdbRS232_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbRS232.Checked)
            {
                gbRS232.Enabled = true;
                btnDisconnectRs232.Enabled = false;

                //设置按钮字体颜色
                btnConnectRs232.ForeColor = Color.Indigo;
                SetButtonBold(btnConnectRs232);
                if (btnConnectTcp.Font.Bold)
                {
                    SetButtonBold(btnConnectTcp);
                }                
                
                gbTcpIp.Enabled = false;
            }
        }

        private void rdbTcpIp_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbTcpIp.Checked)
            {
                gbTcpIp.Enabled = true;
                btnDisconnectTcp.Enabled = false;

                //设置按钮字体颜色
                btnConnectTcp.ForeColor = Color.Indigo;
                if (btnConnectRs232.Font.Bold)
                {
                    SetButtonBold(btnConnectRs232);
                }                
                SetButtonBold(btnConnectTcp);
                
                gbRS232.Enabled = false;
            }
        }

        private void SetButtonBold(Button btnBold)
        {
            Font oldFont = btnBold.Font;
            Font newFont = new Font(oldFont, oldFont.Style ^ FontStyle.Bold);
            btnBold.Font = newFont;
        }

        private void SetRadioButtonBold(CheckBox ckBold)
        {
            Font oldFont = ckBold.Font;
            Font newFont = new Font(oldFont, oldFont.Style ^ FontStyle.Bold);
            ckBold.Font = newFont;
        }

        private void SetFormEnable(bool bIsEnable)
        {
            gbConnectType.Enabled = (!bIsEnable);
            gbCmdReaderAddress.Enabled = bIsEnable;
            gbCmdVersion.Enabled = bIsEnable;
            gbCmdBaudrate.Enabled = bIsEnable;
            gbCmdTemperature.Enabled = bIsEnable;
            gbCmdOutputPower.Enabled = bIsEnable;
            gbCmdAntenna.Enabled = bIsEnable;
            //gbCmdDrm.Enabled = bIsEnable;
            gbCmdRegion.Enabled = bIsEnable;
            gbCmdBeeper.Enabled = bIsEnable;
            gbCmdReadGpio.Enabled = bIsEnable;
            
            gbReturnLoss.Enabled = bIsEnable;         

            btnResetReader.Enabled = bIsEnable;
            gbCmdOperateTag.Enabled = bIsEnable;        

            tabEpcTest.Enabled = bIsEnable;
            lbChangeBaudrate.Enabled = bIsEnable;
            cmbSetBaudrate.Enabled = bIsEnable;
            btnSetUartBaudrate.Enabled = bIsEnable;
            btReaderSetupRefresh.Enabled = bIsEnable;

            btRfSetup.Enabled = bIsEnable;
        }

        private void btnConnectRs232_Click(object sender, EventArgs e)
        {
            string strException = string.Empty;
            string strComPort = cmbComPort.Text;
            int nBaudrate=Convert.ToInt32(cmbBaudrate.Text);

            int nRet = reader.OpenCom(strComPort, nBaudrate, out strException);
            if (nRet != 0)
            {
                string strLog = "Connection failed, failure cause: " + strException; 
                WriteLog(lrtxtLog,strLog, 1);
                return;
            }
            else
            {
                string strLog = "Connect " + strComPort + "@" + nBaudrate.ToString();
                WriteLog(lrtxtLog,strLog, 0);
            }
            
            SetFormEnable(true);
                        
            btnConnectRs232.Enabled = false;
            btnDisconnectRs232.Enabled = true;

            btnConnectRs232.ForeColor = Color.Black;
            btnDisconnectRs232.ForeColor = Color.Indigo;
            SetButtonBold(btnConnectRs232);
            SetButtonBold(btnDisconnectRs232);
        }

        private void btnDisconnectRs232_Click(object sender, EventArgs e)
        {
            reader.CloseCom();

            SetFormEnable(false);
            btnConnectRs232.Enabled = true;
            btnDisconnectRs232.Enabled = false;

            btnConnectRs232.ForeColor = Color.Indigo;
            btnDisconnectRs232.ForeColor = Color.Black;
            SetButtonBold(btnConnectRs232);
            SetButtonBold(btnDisconnectRs232);
        }

        private void btnConnectTcp_Click(object sender, EventArgs e)
        {
            try
            {
                string strException = string.Empty;
                IPAddress ipAddress = IPAddress.Parse(ipIpServer.IpAddressStr);
                int nPort = Convert.ToInt32(txtTcpPort.Text);

                int nRet = reader.ConnectServer(ipAddress,nPort,out strException);
                if (nRet != 0)
                {
                    string strLog = "Connection failed, failure cause: " + strException;
                    WriteLog(lrtxtLog,strLog, 1);

                    return;
                }
                else
                {
                    string strLog = "Connect " + ipIpServer.IpAddressStr + "@" + nPort.ToString();
                    WriteLog(lrtxtLog,strLog, 0);
                }

                SetFormEnable(true);
                btnConnectTcp.Enabled = false;
                btnDisconnectTcp.Enabled = true;

                btnConnectTcp.ForeColor = Color.Black;
                btnDisconnectTcp.ForeColor = Color.Indigo;
                SetButtonBold(btnConnectTcp);
                SetButtonBold(btnDisconnectTcp);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnDisconnectTcp_Click(object sender, EventArgs e)
        {
            reader.SignOut();

            SetFormEnable(false);
            btnConnectTcp.Enabled = true;
            btnDisconnectTcp.Enabled = false;

            btnConnectTcp.ForeColor = Color.Indigo;
            btnDisconnectTcp.ForeColor = Color.Black;
            SetButtonBold(btnConnectTcp);
            SetButtonBold(btnDisconnectTcp);
        }

        private void btnResetReader_Click(object sender, EventArgs e)
        {
            int nRet = reader.Reset(m_curSetting.btReadId);
            if (nRet != 0)
            {
                string strLog = "Reset reader fails";
                WriteLog(lrtxtLog,strLog, 1);
            }
            else
            {
                string strLog = "Reset reader";
                m_curSetting.btReadId = (byte)0xFF;
                WriteLog(lrtxtLog,strLog, 0);
            }
        }

        private void btnSetReadAddress_Click(object sender, EventArgs e)
        {
            try
            {
                if (htxtReadId.Text.Length != 0)
                {
                    string strTemp = htxtReadId.Text.Trim();
                    reader.SetReaderAddress(m_curSetting.btReadId, Convert.ToByte(strTemp, 16));
                    m_curSetting.btReadId = Convert.ToByte(strTemp, 16);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void ProcessSetReadAddress(Reader.MessageTran msgTran)
        {
            string strCmd = "Set reader's address";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    WriteLog(lrtxtLog,strCmd, 0);

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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnGetFirmwareVersion_Click(object sender, EventArgs e)
        {
            reader.GetFirmwareVersion(m_curSetting.btReadId);
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

                RefreshReadSetting(msgTran.Cmd);
                //WriteLog(lrtxtLog,strCmd);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnSetUartBaudrate_Click(object sender, EventArgs e)
        {
            if (cmbSetBaudrate.SelectedIndex != -1)
            {
                reader.SetUartBaudrate(m_curSetting.btReadId, cmbSetBaudrate.SelectedIndex + 3);
                m_curSetting.btIndexBaudrate = Convert.ToByte(cmbSetBaudrate.SelectedIndex);
            }            
        }

        private void ProcessSetUartBaudrate(Reader.MessageTran msgTran)
        {
            string strCmd = "Set Baudrate";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    WriteLog(lrtxtLog,strCmd, 0);

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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnGetReaderTemperature_Click(object sender, EventArgs e)
        {
            reader.GetReaderTemperature(m_curSetting.btReadId);
        }

        private void ProcessGetReaderTemperature(Reader.MessageTran msgTran)
        {
            string strCmd = "Get reader internal temperature";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 2)
            {
                m_curSetting.btReadId = msgTran.ReadId;
                m_curSetting.btPlusMinus = msgTran.AryData[0];
                m_curSetting.btTemperature = msgTran.AryData[1];

                RefreshReadSetting(msgTran.Cmd);
                WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnGetOutputPower_Click(object sender, EventArgs e)
        {
            //WriteLog(lrtxtLog,"btnGetOutputPower");
            if (antType1.Checked)
            {               
                reader.GetOutputPowerFour(m_curSetting.btReadId);
            }
        }

        private void ProcessGetOutputPower(Reader.MessageTran msgTran)
        {
            string strCmd = "Get RF Output Power";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                m_curSetting.btReadId = msgTran.ReadId;
                m_curSetting.btOutputPower = msgTran.AryData[0];

                RefreshReadSetting(0x77);
                WriteLog(lrtxtLog,strCmd,0);
                return;
            }
            else if (msgTran.AryData.Length == 8)
            {
                m_curSetting.btReadId = msgTran.ReadId;
                m_curSetting.btOutputPowers = msgTran.AryData;

                RefreshReadSetting(0x97);
                WriteLog(lrtxtLog,strCmd,0);
                return;
            }
             else if (msgTran.AryData.Length == 4) 
            {
                m_curSetting.btReadId = msgTran.ReadId;
                m_curSetting.btOutputPowers = msgTran.AryData;

                RefreshReadSetting(0x77);
                WriteLog(lrtxtLog,strCmd,0);
                return;
            }
            else
            {
                strErrorCode = "Unknown Error";
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnSetOutputPower_Click(object sender, EventArgs e)
        {
            try
            {
                if (antType1.Checked)
                {
                    if (textBox1.Text.Length != 0)
                    {
                        byte[] OutputPower = new byte[1];
                        OutputPower[0] = Convert.ToByte(textBox1.Text);
                        //m_curSetting.btOutputPower = Convert.ToByte(txtOutputPower.Text);
                        reader.SetOutputPower(m_curSetting.btReadId, OutputPower);
                        // m_curSetting.btOutputPower = Convert.ToByte(txtOutputPower.Text);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    WriteLog(lrtxtLog,strCmd,0);

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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnGetWorkAntenna_Click(object sender, EventArgs e)
        {
            reader.GetWorkAntenna(m_curSetting.btReadId);
        }

        private void ProcessGetWorkAntenna(Reader.MessageTran msgTran)
        {
            string strCmd = "Get working antenna";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x00 || msgTran.AryData[0] == 0x01 || msgTran.AryData[0] == 0x02 || msgTran.AryData[0] == 0x03
                    || msgTran.AryData[0] == 0x04 || msgTran.AryData[0] == 0x05 || msgTran.AryData[0] == 0x06 || msgTran.AryData[0] == 0x07)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    m_curSetting.btWorkAntenna = msgTran.AryData[0];

                    RefreshReadSetting(0x75);
                    WriteLog(lrtxtLog,strCmd, 0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnSetWorkAntenna_Click(object sender, EventArgs e)
        {
            m_bInventory = false;
            byte btWorkAntenna = 0xFF;
            if (cmbWorkAnt.SelectedIndex != -1)
            {
                btWorkAntenna = (byte)cmbWorkAnt.SelectedIndex;
                reader.SetWorkAntenna(m_curSetting.btReadId, btWorkAntenna);
                m_curSetting.btWorkAntenna = btWorkAntenna;
            }
        }

        private void ProcessSetWorkAntenna(Reader.MessageTran msgTran)
        {
            int intCurrentAnt = 0;
            intCurrentAnt = m_curSetting.btWorkAntenna + 1;
            string strCmd = "Set working antenna successfully, Current Ant: Ant" + intCurrentAnt.ToString();
         
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    //WriteLog(lrtxtLog,strCmd);

                    if (m_bInventory)
                    {
                        RunLoopInventroy();
                    }
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
            WriteLog(lrtxtLog,strLog,1);

            if (m_bInventory)
            {
                m_curInventoryBuffer.nCommond = 1;
                m_curInventoryBuffer.dtEndInventory = DateTime.Now;
                RunLoopInventroy();
            }
        }

        private void btnGetDrmMode_Click(object sender, EventArgs e)
        {
            reader.GetDrmMode(m_curSetting.btReadId);
        }

        private void ProcessGetDrmMode(Reader.MessageTran msgTran)
        {
            string strCmd = "Get DRM Status";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x00 || msgTran.AryData[0] == 0x01)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    m_curSetting.btDrmMode = msgTran.AryData[0];

                    RefreshReadSetting(0x7D);
                    WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnSetDrmMode_Click(object sender, EventArgs e)
        {
            byte btDrmMode = 0xFF;
            /*
            if (rdbDrmModeClose.Checked)
            {
                btDrmMode = 0x00;
            }
            else if (rdbDrmModeOpen.Checked)
            {
                btDrmMode = 0x01;
            }
            else
            {
                return;
            }
             */

            reader.SetDrmMode(m_curSetting.btReadId, btDrmMode);
            m_curSetting.btDrmMode = btDrmMode;
        }

        private void ProcessSetDrmMode(Reader.MessageTran msgTran)
        {
            string strCmd = "Set DRM Status";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    WriteLog(lrtxtLog,strCmd,0);

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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void rdbRegionFcc_CheckedChanged(object sender, EventArgs e)
        {
            cmbFrequencyStart.SelectedIndex = -1;
            cmbFrequencyEnd.SelectedIndex = -1;
            cmbFrequencyStart.Items.Clear();
            cmbFrequencyEnd.Items.Clear();

            float nStart = 902.00f;
            for (int nloop = 0; nloop < 53; nloop++)
            {
                string strTemp = nStart.ToString("0.00");
                cmbFrequencyStart.Items.Add(strTemp);
                cmbFrequencyEnd.Items.Add(strTemp);

                nStart += 0.5f;
            }
        }

        private void rdbRegionEtsi_CheckedChanged(object sender, EventArgs e)
        {
            cmbFrequencyStart.SelectedIndex = -1;
            cmbFrequencyEnd.SelectedIndex = -1;
            cmbFrequencyStart.Items.Clear();
            cmbFrequencyEnd.Items.Clear();

            float nStart = 865.00f;
            for (int nloop = 0; nloop < 7; nloop++)
            {
                string strTemp = nStart.ToString("0.00");
                cmbFrequencyStart.Items.Add(strTemp);
                cmbFrequencyEnd.Items.Add(strTemp);

                nStart += 0.5f;
            }
        }

        private void rdbRegionChn_CheckedChanged(object sender, EventArgs e)
        {
            cmbFrequencyStart.SelectedIndex = -1;
            cmbFrequencyEnd.SelectedIndex = -1;
            cmbFrequencyStart.Items.Clear();
            cmbFrequencyEnd.Items.Clear();

            float nStart = 920.00f;
            for (int nloop = 0; nloop < 11; nloop++)
            {
                string strTemp = nStart.ToString("0.00");
                cmbFrequencyStart.Items.Add(strTemp);
                cmbFrequencyEnd.Items.Add(strTemp);

                nStart += 0.5f;
            }
        }

        private string GetFreqString(byte btFreq)
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

        private void btnGetFrequencyRegion_Click(object sender, EventArgs e)
        {
            WriteLog(lrtxtLog,"btnGetFrequencyRegion_Click", 0);
            reader.GetFrequencyRegion(m_curSetting.btReadId);
        }

        private void ProcessGetFrequencyRegion(Reader.MessageTran msgTran)
        {
            string strCmd = "Query RF frequency spectrum    ";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 3)
            {
                m_curSetting.btReadId = msgTran.ReadId;
                m_curSetting.btRegion = msgTran.AryData[0];
                m_curSetting.btFrequencyStart = msgTran.AryData[1];
                m_curSetting.btFrequencyEnd = msgTran.AryData[2];

                RefreshReadSetting(0x79);
                WriteLog(lrtxtLog,strCmd,0);
                return;
            }
            else if (msgTran.AryData.Length == 6)
            {
                m_curSetting.btReadId = msgTran.ReadId;
                m_curSetting.btRegion = msgTran.AryData[0];
                m_curSetting.btUserDefineFrequencyInterval = msgTran.AryData[1];
                m_curSetting.btUserDefineChannelQuantity = msgTran.AryData[2];
                m_curSetting.nUserDefineStartFrequency = msgTran.AryData[3] * 256 * 256 + msgTran.AryData[4] * 256 + msgTran.AryData[5];
                RefreshReadSetting(0x79);
                WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnSetFrequencyRegion_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbUserDefineFreq.Checked == true)
                {
                    int nStartFrequency = Convert.ToInt32(textStartFreq.Text);
                    int nFrequencyInterval = Convert.ToInt32(TextFreqInterval.Text);
                    nFrequencyInterval = nFrequencyInterval / 10;
                    byte btChannelQuantity = Convert.ToByte(textFreqQuantity.Text);
                    reader.SetUserDefineFrequency(m_curSetting.btReadId, nStartFrequency, (byte)nFrequencyInterval, btChannelQuantity);
                    m_curSetting.btRegion = 4;
                    m_curSetting.nUserDefineStartFrequency = nStartFrequency;
                    m_curSetting.btUserDefineFrequencyInterval = (byte)nFrequencyInterval;
                    m_curSetting.btUserDefineChannelQuantity = btChannelQuantity;
                }
                else
                {
                    byte btRegion = 0x00;
                    byte btStartFreq = 0x00;
                    byte btEndFreq = 0x00;

                    int nStartIndex = cmbFrequencyStart.SelectedIndex;
                    int nEndIndex = cmbFrequencyEnd.SelectedIndex;
                    if (nEndIndex < nStartIndex)
                    {
                        WriteLog(lrtxtLog,"Spectral range that does not meet specifications, please refer to the Serial Protocol", 1);
                        return;
                    }

                    if (rdbRegionFcc.Checked)
                    {
                        btRegion = 0x01;
                        btStartFreq = Convert.ToByte(nStartIndex + 7);
                        btEndFreq = Convert.ToByte(nEndIndex + 7);
                    }
                    else if (rdbRegionEtsi.Checked)
                    {
                        btRegion = 0x02;
                        btStartFreq = Convert.ToByte(nStartIndex);
                        btEndFreq = Convert.ToByte(nEndIndex);
                    }
                    else if (rdbRegionChn.Checked)
                    {
                        btRegion = 0x03;
                        btStartFreq = Convert.ToByte(nStartIndex + 43);
                        btEndFreq = Convert.ToByte(nEndIndex + 43);
                    }
                    else
                    {
                        return;
                    }

                    reader.SetFrequencyRegion(m_curSetting.btReadId, btRegion, btStartFreq, btEndFreq);
                    m_curSetting.btRegion = btRegion;
                    m_curSetting.btFrequencyStart = btStartFreq;
                    m_curSetting.btFrequencyEnd = btEndFreq;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                    WriteLog(lrtxtLog,strCmd,0);

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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnSetBeeperMode_Click(object sender, EventArgs e)
        {
            byte btBeeperMode = 0xFF;

            if (rdbBeeperModeSlient.Checked)
            {
                btBeeperMode = 0x00;
            }
            else if (rdbBeeperModeInventory.Checked)
            {
                btBeeperMode = 0x01;
            }
            else if (rdbBeeperModeTag.Checked)
            {
                btBeeperMode = 0x02;
            }
            else
            {
                return;
            }

            reader.SetBeeperMode(m_curSetting.btReadId, btBeeperMode);
            m_curSetting.btBeeperMode = btBeeperMode;
        }

        private void ProcessSetBeeperMode(Reader.MessageTran msgTran)
        {
            string strCmd = "Set reader's buzzer hehavior";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnReadGpioValue_Click(object sender, EventArgs e)
        {
            reader.ReadGpioValue(m_curSetting.btReadId);
        }

        private void ProcessReadGpioValue(Reader.MessageTran msgTran)
        {
            string strCmd = "Get GPIO status";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 2)
            {
                m_curSetting.btReadId = msgTran.ReadId;
                m_curSetting.btGpio1Value = msgTran.AryData[0];
                m_curSetting.btGpio2Value = msgTran.AryData[1];

                RefreshReadSetting(0x60);
                WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnWriteGpio3Value_Click(object sender, EventArgs e)
        {
            byte btGpioValue = 0xFF;
            if (rdbGpio3Low.Checked)
            {
                btGpioValue = 0x00;
            }
            else if (rdbGpio3High.Checked)
            {
                btGpioValue = 0x01;
            }
            else
            {
                return;
            }
            reader.WriteGpioValue(m_curSetting.btReadId, 0x03, btGpioValue);
            m_curSetting.btGpio3Value = btGpioValue;
        }

        private void btnWriteGpio4Value_Click(object sender, EventArgs e)
        {
            byte btGpioValue = 0xFF;

            if (rdbGpio4Low.Checked)
            {
                btGpioValue = 0x00;
            }
            else if (rdbGpio4High.Checked)
            {
                btGpioValue = 0x01;
            }
            else
            {
                return;
            }
            reader.WriteGpioValue(m_curSetting.btReadId, 0x04, btGpioValue);
            m_curSetting.btGpio4Value = btGpioValue;
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
                    WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void btnGetAntDetector_Click(object sender, EventArgs e)
        {
            reader.GetAntDetector(m_curSetting.btReadId);
        }

        private void ProcessGetAntDetector(Reader.MessageTran msgTran)
        {
            string strCmd = "Get antenna detector threshold value";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                m_curSetting.btReadId = msgTran.ReadId;
                m_curSetting.btAntDetector = msgTran.AryData[0];
                
                RefreshReadSetting(0x63);
                WriteLog(lrtxtLog,strCmd,0);
                return;
            }
            else
            {
                strErrorCode = "Unknown Error";
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog,strLog,1);
        }

        private void ProcessGetMonzaStatus(Reader.MessageTran msgTran)
        {
            string strCmd = "Get current Impinj FastTID setting";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x00 || msgTran.AryData[0] == 0x8D)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    m_curSetting.btMonzaStatus = msgTran.AryData[0];

                    RefreshReadSetting(0x8E);
                    WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void ProcessSetMonzaStatus(Reader.MessageTran msgTran)
        {
            string strCmd = "Set Impinj FastTID function";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    m_curSetting.btAntDetector = msgTran.AryData[0];
                    WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void ProcessSetProfile(Reader.MessageTran msgTran)
        {
            string strCmd = "Set RF link profile";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    m_curSetting.btLinkProfile = msgTran.AryData[0];
                    WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void ProcessGetProfile(Reader.MessageTran msgTran)
        {
            string strCmd = "Get RF link profile";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if ((msgTran.AryData[0] >= 0xd0) && (msgTran.AryData[0] <= 0xd3))
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    m_curSetting.btLinkProfile = msgTran.AryData[0];

                    RefreshReadSetting(0x6A);
                    WriteLog(lrtxtLog,strCmd,0);
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
            WriteLog(lrtxtLog,strLog,1);
        }



        private void ProcessGetReaderIdentifier(Reader.MessageTran msgTran)
        {
            string strCmd = "Get Reader Identifier";
            string strErrorCode = string.Empty;
            short i;
            string readerIdentifier = "";
            
            if (msgTran.AryData.Length == 12)
            {
                m_curSetting.btReadId = msgTran.ReadId;
                for (i = 0; i < 12; i ++)
                {
                    readerIdentifier = readerIdentifier + string.Format("{0:X2}", msgTran.AryData[i]) + " ";

                    
                }
                m_curSetting.btReaderIdentifier = readerIdentifier;
                RefreshReadSetting(0x68);
                WriteLog(lrtxtLog,strCmd,0);
                return;
            }
            else
            {
                strErrorCode = "Unknown Error";
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog,strLog,1);
        }

        private void ProcessGetImpedanceMatch(Reader.MessageTran msgTran)
        {
            string strCmd = "Measure Impedance of Antenna Port Match";
            string strErrorCode = string.Empty;
                  
            
            if (msgTran.AryData.Length == 1)
            {
                m_curSetting.btReadId = msgTran.ReadId;

                m_curSetting.btAntImpedance = msgTran.AryData[0];
                RefreshReadSetting(0x7E);
                WriteLog(lrtxtLog,strCmd,0);
                return;
            }
            else
            {
                strErrorCode = "Unknown Error";
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog,strLog,1);
        }

        

        private void ProcessSetReaderIdentifier(Reader.MessageTran msgTran)
        {
            string strCmd = "Set Reader Identifier";
            string strErrorCode = string.Empty;
            
            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    WriteLog(lrtxtLog,strCmd,0);
                    return;
                }
            }
            else
            {
                strErrorCode = "Unknown Error";
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog,strLog,1);
        }

        private void ProcessSetAntDetector(Reader.MessageTran msgTran)
        {
            string strCmd = "Set antenna detector threshold value";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    m_curSetting.btReadId = msgTran.ReadId;
                    WriteLog(lrtxtLog,strCmd,0);

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
            WriteLog(lrtxtLog,strLog,1);
        }

        private void ProcessFastSwitch(Reader.MessageTran msgTran)
        {
            string strCmd = "Real time inventory with fast ant switch";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                WriteLog(lrtxtLog,strLog, 1);
                //RefreshFastSwitch(0x8A);
                //RunLoopFastSwitch();
            }
            else if (msgTran.AryData.Length == 2)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[1]);
                WriteLog(lrtxtLog,"Return ant NO : " + (msgTran.AryData[0] + 1), 0);
                string strLog = strCmd + "Failure, failure cause: " + strErrorCode + "--" + "Antenna" + (msgTran.AryData[0] + 1);

                WriteLog(lrtxtLog,strLog, 1);
            }

            else if (msgTran.AryData.Length == 7)
            {
                m_nSwitchTotal = Convert.ToInt32(msgTran.AryData[0]) * 255 * 255  + Convert.ToInt32(msgTran.AryData[1]) * 255  + Convert.ToInt32(msgTran.AryData[2]);
                m_nSwitchTime = Convert.ToInt32(msgTran.AryData[3]) * 255 * 255 * 255 + Convert.ToInt32(msgTran.AryData[4]) * 255 * 255 + Convert.ToInt32(msgTran.AryData[5]) * 255 + Convert.ToInt32(msgTran.AryData[6]);

                m_curInventoryBuffer.nDataCount = m_nSwitchTotal;
                m_curInventoryBuffer.nCommandDuration = m_nSwitchTime;
                WriteLog(lrtxtLog,strCmd,0);
                //RefreshFastSwitch(0x00);
                //RunLoopFastSwitch();
            }

            /*else if (msgTran.AryData.Length == 8)
            {
                
                m_nSwitchTotal = Convert.ToInt32(msgTran.AryData[0]) * 255 * 255 * 255 + Convert.ToInt32(msgTran.AryData[1]) * 255 * 255 + Convert.ToInt32(msgTran.AryData[2]) * 255 + Convert.ToInt32(msgTran.AryData[3]);
                m_nSwitchTime = Convert.ToInt32(msgTran.AryData[4]) * 255 * 255 * 255 + Convert.ToInt32(msgTran.AryData[5]) * 255 * 255 + Convert.ToInt32(msgTran.AryData[6]) * 255 + Convert.ToInt32(msgTran.AryData[7]);

                m_curInventoryBuffer.nDataCount = m_nSwitchTotal;
                m_curInventoryBuffer.nCommandDuration = m_nSwitchTime;
                WriteLog(lrtxtLog,strCmd);
                RefreshFastSwitch(0x02);
                RunLoopFastSwitch();
            }*/
            else
            {
                m_nTotal++;
                int nLength = msgTran.AryData.Length;

                int nEpcLength = nLength - 4;
                if (m_nPhaseOpened)
                {
                    nEpcLength = nLength - 6;
                }

                //Add inventory list
                string strEPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 3, nEpcLength);
                string strPC = CCommondMethod.ByteArrayToString(msgTran.AryData, 1, 2);
                string strRSSI = string.Empty;

                if (m_nPhaseOpened)
                {
                    SetMaxMinRSSI(Convert.ToInt32(msgTran.AryData[nLength - 3] & 0x7F));
                    strRSSI = (msgTran.AryData[nLength - 3] & 0x7F).ToString();
                }
                else
                {
                    SetMaxMinRSSI(Convert.ToInt32(msgTran.AryData[nLength - 1] & 0x7F));
                    strRSSI = (msgTran.AryData[nLength - 1] & 0x7F).ToString();
                }
                
                byte btTemp = msgTran.AryData[0];
                byte btAntId = (byte)((btTemp & 0x03) + 1);
                string strPhase = string.Empty;
                if (m_nPhaseOpened)
                {
                    if ((msgTran.AryData[nLength - 3] & 0x80) != 0) btAntId += 4;
                    strPhase = CCommondMethod.ByteArrayToString(msgTran.AryData,nLength - 2,2);
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
                    switch (btAntId)
                    {
                        case 0x01:
                            {
                                row1[7] = "1";
                            }
                            break;
                        case 0x02:
                            {
                                row1[8] = "1";
                            }
                            break;
                        case 0x03:
                            {
                                row1[9] = "1";
                            }
                            break;
                        case 0x04:
                            {
                                row1[10] = "1";
                            }
                            break;
                        case 0x05:
                            {
                                row1[11] = "1";
                            }
                            break;
                        case 0x06:
                            {
                                row1[12] = "1";
                            }
                            break;
                        case 0x07:
                            {
                                row1[13] = "1";
                            }
                            break;
                        case 0x08:
                            {
                                row1[14] = "1";
                            }
                            break;
                        default:
                            break;
                    }

                    m_curInventoryBuffer.dtTagTable.Rows.Add(row1);
                    m_curInventoryBuffer.dtTagTable.AcceptChanges();
                }
                else
                {
                    foreach (DataRow dr in drs)
                    {
                        dr.BeginEdit();
                        int nTemp = 0;

                        dr[4] = strRSSI;
                        //dr[5] = (Convert.ToInt32(dr[5]) + 1).ToString();
                        nTemp = Convert.ToInt32(dr[5]);
                        dr[5] = (nTemp + 1).ToString();
                        dr[6] = strFreq;

                        dr[15] = strPhase;

                        switch (btAntId)
                        {
                            case 0x01:
                                {
                                    //dr[7] = (Convert.ToInt32(dr[7]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[7]);
                                    dr[7] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x02:
                                {
                                    //dr[8] = (Convert.ToInt32(dr[8]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[8]);
                                    dr[8] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x03:
                                {
                                    //dr[9] = (Convert.ToInt32(dr[9]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[9]);
                                    dr[9] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x04:
                                {
                                    //dr[10] = (Convert.ToInt32(dr[10]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[10]);
                                    dr[10] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x05:
                                {
                                    //dr[7] = (Convert.ToInt32(dr[7]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[11]);
                                    dr[11] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x06:
                                {
                                    //dr[8] = (Convert.ToInt32(dr[8]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[12]);
                                    dr[12] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x07:
                                {
                                    //dr[9] = (Convert.ToInt32(dr[9]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[13]);
                                    dr[13] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x08:
                                {
                                    //dr[10] = (Convert.ToInt32(dr[10]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[14]);
                                    dr[14] = (nTemp + 1).ToString();
                                }
                                break;
                            default:
                                break;
                        }

                        dr.EndEdit();
                    }
                    m_curInventoryBuffer.dtTagTable.AcceptChanges();
                }
                m_curInventoryBuffer.dtEndInventory = DateTime.Now;
                //RefreshFastSwitch(0x00);
            }

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
                WriteLog(lrtxtLog,strLog,1);

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
                Thread.Sleep(10);
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
                    SetMaxMinRSSI(Convert.ToInt32(msgTran.AryData[nLength - 3] & 0x7F));
                    strRSSI = (msgTran.AryData[nLength - 3] & 0x7F).ToString();
                }
                else
                {
                    SetMaxMinRSSI(Convert.ToInt32(msgTran.AryData[nLength - 1] & 0x7F));
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
                    switch (btAntId)
                    {
                        case 0x01:
                            {
                                row1[7] = "1";
                            }
                            break;
                        case 0x02:
                            {
                                row1[8] = "1";
                            }
                            break;
                        case 0x03:
                            {
                                row1[9] = "1";
                            }
                            break;
                        case 0x04:
                            {
                                row1[10] = "1";
                            }
                            break;
                        case 0x05:
                            {
                                row1[11] = "1";
                            }
                            break;
                        case 0x06:
                            {
                                row1[12] = "1";
                            }
                            break;
                        case 0x07:
                            {
                                row1[13] = "1";
                            }
                            break;
                        case 0x08:
                            {
                                row1[14] = "1";
                            }
                            break;
                        default:
                            break;
                    }

                    m_curInventoryBuffer.dtTagTable.Rows.Add(row1);
                    m_curInventoryBuffer.dtTagTable.AcceptChanges();
                }
                else
                {
                    foreach (DataRow dr in drs)
                    {
                        dr.BeginEdit();
                        int nTemp = 0;

                        dr[4] = strRSSI;
                        //dr[5] = (Convert.ToInt32(dr[5]) + 1).ToString();
                        nTemp = Convert.ToInt32(dr[5]);
                        dr[5] = (nTemp + 1).ToString();
                        dr[6] = strFreq;
                        dr[15] = strPhase;
                        switch (btAntId)
                        {
                            case 0x01:
                                {
                                    //dr[7] = (Convert.ToInt32(dr[7]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[7]);
                                    dr[7] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x02:
                                {
                                    //dr[8] = (Convert.ToInt32(dr[8]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[8]);
                                    dr[8] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x03:
                                {
                                    //dr[9] = (Convert.ToInt32(dr[9]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[9]);
                                    dr[9] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x04:
                                {
                                    //dr[10] = (Convert.ToInt32(dr[10]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[10]);
                                    dr[10] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x05:
                                {
                                    //dr[7] = (Convert.ToInt32(dr[7]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[11]);
                                    dr[11] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x06:
                                {
                                    //dr[8] = (Convert.ToInt32(dr[8]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[12]);
                                    dr[12] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x07:
                                {
                                    //dr[9] = (Convert.ToInt32(dr[9]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[13]);
                                    dr[13] = (nTemp + 1).ToString();
                                }
                                break;
                            case 0x08:
                                {
                                    //dr[10] = (Convert.ToInt32(dr[10]) + 1).ToString();
                                    nTemp = Convert.ToInt32(dr[14]);
                                    dr[14] = (nTemp + 1).ToString();
                                }
                                break;
                            default:
                                break;
                        }

                        dr.EndEdit();
                    }
                    m_curInventoryBuffer.dtTagTable.AcceptChanges();
                }

                m_curInventoryBuffer.dtEndInventory = DateTime.Now;
                RefreshInventoryReal(0x89);
                //Thread.Sleep(200);
            }
        }       

        private void btnGetInventoryBuffer_Click(object sender, EventArgs e)
        {
            m_curInventoryBuffer.dtTagTable.Rows.Clear();
            
            reader.GetInventoryBuffer(m_curSetting.btReadId);
        }

        private void SetMaxMinRSSI(int nRSSI)
        {
            if (m_curInventoryBuffer.nMaxRSSI < nRSSI)
            {
                m_curInventoryBuffer.nMaxRSSI = nRSSI;
            }

            if (m_curInventoryBuffer.nMinRSSI == 0)
            {
                m_curInventoryBuffer.nMinRSSI = nRSSI;
            }
            else if (m_curInventoryBuffer.nMinRSSI > nRSSI)
            {
                m_curInventoryBuffer.nMinRSSI = nRSSI;
            }
        }

        private void cbAccessEpcMatch_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAccessEpcMatch.Checked)
            {
                reader.GetAccessEpcMatch(m_curSetting.btReadId);
            }
            else
            {
                m_curOperateTagBuffer.strAccessEpcMatch = "";
                txtAccessEpcMatch.Text = "";
                reader.CancelAccessEpcMatch(m_curSetting.btReadId, 0x01);
            }
        }

        private void ProcessGetAccessEpcMatch(Reader.MessageTran msgTran)
        {
            string strCmd = "Get selected tag";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x01)
                {
                    WriteLog(lrtxtLog,"Unselected Tag", 1);
                    return;
                }
                else
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                }
            }
            else
            {
                if (msgTran.AryData[0] == 0x00)
                {
                    m_curOperateTagBuffer.strAccessEpcMatch = CCommondMethod.ByteArrayToString(msgTran.AryData, 2, Convert.ToInt32(msgTran.AryData[1]));
                    
                    RefreshOpTag(0x86);
                    WriteLog(lrtxtLog,strCmd, 0);
                    return;
                }
                else
                {
                    strErrorCode = "Unknown Error";
                }
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

            WriteLog(lrtxtLog,strLog, 1);
        }

        private void btnSetAccessEpcMatch_Click(object sender, EventArgs e)
        {
            string[] reslut = CCommondMethod.StringToStringArray(cmbSetAccessEpcMatch.Text.ToUpper(), 2);

            if (reslut == null)
            {
                WriteLog(lrtxtLog,"Please select EPC number", 1);
                return;
            }

            byte[] btAryEpc = CCommondMethod.StringArrayToByteArray(reslut, reslut.Length);

            m_curOperateTagBuffer.strAccessEpcMatch = cmbSetAccessEpcMatch.Text;
            txtAccessEpcMatch.Text = cmbSetAccessEpcMatch.Text;
            ckAccessEpcMatch.Checked = true;
            reader.SetAccessEpcMatch(m_curSetting.btReadId, 0x00, Convert.ToByte(btAryEpc.Length), btAryEpc);
        }

        private void ProcessSetAccessEpcMatch(Reader.MessageTran msgTran)
        {
            string strCmd = "Select/Deselect Tag";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == 0x10)
                {
                    WriteLog(lrtxtLog,strCmd, 0);
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
            WriteLog(lrtxtLog,strLog, 1);
        }

        
        private void btnReadTag_Click(object sender, EventArgs e)
        {
            try
            {
                byte btMemBank = findMemBank();
                byte btWordAddr = 0x00;
                byte btWordCnt = 0x00;
                readTagRetry = 0;

                if (txtWordAddr.Text.Length != 0)
                {
                    btWordAddr = Convert.ToByte(txtWordAddr.Text);
                }
                else
                {
                    WriteLog(lrtxtLog,"Please select the start Add of tag", 1);
                    return;
                }

                if (txtWordCnt.Text.Length != 0)
                {
                    btWordCnt = Convert.ToByte(txtWordCnt.Text);
                }
                else
                {
                    WriteLog(lrtxtLog,"Please select the Length", 1);
                    return;
                }

                string[] reslut = CCommondMethod.StringToStringArray(htxtReadAndWritePwd.Text.ToUpper(), 2);
                if (reslut != null && reslut.GetLength(0) != 4)
                {
                    WriteLog(lrtxtLog,"Password must be null or 4 bytes", 1);
                    return;
                }
                byte[] btAryPwd = null;
                if (reslut != null)
                {
                    btAryPwd = CCommondMethod.StringArrayToByteArray(reslut, reslut.Length);
                }

                m_curOperateTagBuffer.dtTagTable.Clear();
                ltvOperate.Items.Clear();
                reader.ReadTag(m_curSetting.btReadId, btMemBank, btWordAddr, btWordCnt, btAryPwd);
                WriteLog(lrtxtLog,"Read Tag", 0);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //private static byte btGPIO3 = 0; //Red
        //private static byte btGPIO4 = 0; //Green
        private static short displayGreenCnt = 5;
        private static short displayRedCnt = 5;
        private static short displayIdleGreenCnt = 0;
        private static short displayIdleRedCnt = 0;
        private const short displayMAXCnt = 20;
        private const short displayIdelMAXCnt = 200;
        private void resetLEDstatus()
        {
            if (m_curSetting.btGpio3Value != 0)
            {
                if(displayIdleGreenCnt++ > displayIdelMAXCnt)
                {//reset light when it is zero
                    reader.WriteGpioValue(m_curSetting.btReadId, 0x04, 0);
                    m_curSetting.btGpio3Value = 0;
                    displayIdleGreenCnt = 0;
                    Thread.Sleep(20);
                }
            }   
            else if(displayIdleGreenCnt-- > -3)
            {
                displayIdleGreenCnt--;
                reader.WriteGpioValue(m_curSetting.btReadId, 0x04, 0);
                m_curSetting.btGpio3Value = 0;
                Thread.Sleep(20);
            }
            else if (displayIdleGreenCnt <= -50)
            {
                displayIdleGreenCnt = 0;
            }

            if (m_curSetting.btGpio4Value != 0)
            {
                if(displayIdleRedCnt++ > displayIdelMAXCnt)
                {//reset light when it is zero
                    reader.WriteGpioValue(m_curSetting.btReadId, 0x03, 0); //bGpio3RedOn                
                    m_curSetting.btGpio4Value = 0;
                    displayIdleRedCnt = 0;
                    Thread.Sleep(20);
                }
            }
            else if (displayIdleRedCnt-- > -3)
            {
                displayIdleRedCnt--;
                reader.WriteGpioValue(m_curSetting.btReadId, 0x03, 0);
                m_curSetting.btGpio4Value = 0;
                Thread.Sleep(20);
            }
            else if (displayIdleRedCnt <= -50)
            {
                displayIdleRedCnt = 0;
            }
        }

        private void setVerifiedLEDStatus(byte bGpio4GreenOn, byte bGpio3RedOn)
        {//GPIO 4 green, GPIO 3 red
#if false
            if (bGpio3RedOn == 1)
            {//red on, green must be off
                bGpio4GreenOn = 0;
            }

            reader.WriteGpioValue(m_curSetting.btReadId, 0x04, bGpio4GreenOn);
            m_curSetting.btGpio4Value = bGpio4GreenOn;
            WriteLog(lrtxtLog,"LED Green set " + bGpio4GreenOn, 0);
            Thread.Sleep(20);
            
            /*if (bGpio3RedOn == 1)
            {//red on
                displayRedCnt = 0;
            }
            else if (bGpio3RedOn == 0 && m_curSetting.btGpio3Value == 1 &&
                        displayRedCnt++ < displayMAXCnt)
            {//red to off
                return;
            }*/
            reader.WriteGpioValue(m_curSetting.btReadId, 0x03, bGpio3RedOn);
            m_curSetting.btGpio3Value = bGpio3RedOn;
            WriteLog(lrtxtLog,"LED Red set " + bGpio3RedOn,0);
            Thread.Sleep(20);  
#endif            
        }

        private bool verifyTag(string label)
        {
            //bool bVerified = false;
            WriteLog(lrtxtLog,"Load Key " + symmetric.readKey(),0);

            string decryptMsg = symmetric.DecryptFromHEX(label);
            WriteLog(lrtxtLog,"Decrypt data " + decryptMsg + ", Size " + decryptMsg.Length,0);
            RFIDTagInfo.tagInfo = decryptMsg;

            //bVerified = symmetric.verifyLabel(decryptMsg, bRead2Erase);
            WriteLog(lrtxtLog, "Verify label read " + label, 0);
            return RFIDTagInfo.verifyData(decryptMsg, true, bRead2Erase);
        }

        static int readTagRetry = 0;
        static bool bRead2Erase = false;
        static bool bVerify = false;
        private void ProcessReadTag(Reader.MessageTran msgTran)
        {
            string strCmd = "Read Tag";
            string strErrorCode = string.Empty;
            
            try
            {
                byte btMemBank = findMemBank();
                byte btWordAddr = Convert.ToByte(txtWordAddr.Text);
                byte btWordCnt = Convert.ToByte(txtWordCnt.Text);

                //WriteLog(lrtxtLog,"Read Tag, AryData.Length " + msgTran.AryData.Length, 0);
                if (msgTran.AryData.Length == 1)
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                    string strLog = strCmd + " Failure, read tag failure cause1: " + strErrorCode;
                                        
                    if (readTagRetry++ < rwTagRetryMAX)
                    {
                        string[] reslut = CCommondMethod.StringToStringArray(htxtReadAndWritePwd.Text.ToUpper(), 2);
                        byte[] btAryPwd = CCommondMethod.StringArrayToByteArray(reslut, reslut.Length);
                        reader.ReadTag(m_curSetting.btReadId, btMemBank, btWordAddr, 22, btAryPwd);
                        Thread.Sleep(30);
                        //setVerifiedLEDStatus(false, true); //red on
                        WriteLog(lrtxtLog,"Read Tag retry " + readTagRetry, 1);
                    }
                    else
                    {// read tag failed
                        setVerifiedLEDStatus(0, 1); //red on                        
#if READ2SCAN
                        WriteLog(lrtxtLog,strLog + " retry " + readTagRetry);
                        initScanTag();
#endif
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

                    DataRow row = m_curOperateTagBuffer.dtTagTable.NewRow();
                    row[0] = strPC;
                    row[1] = strCRC;
                    row[2] = strEPC;
                    row[3] = strData;
                    row[4] = nDataLen.ToString();
                    row[5] = strAntId;
                    row[6] = strReadCount;

                    m_curOperateTagBuffer.dtTagTable.Rows.Add(row);
                    m_curOperateTagBuffer.dtTagTable.AcceptChanges();
                    RefreshOpTag(0x81);
                    WriteLog(lrtxtLog,strCmd,0);
                    ulong uWordCnt = 0;                    
                    RFIDTagInfo.label = RFIDTagInfo.readEPCLabel(strEPC, out uWordCnt);
                    if (uWordCnt <= 99999999999999)
                        btWordCnt = 22;
                    else
                        btWordCnt = 32;
                    
                    if (btMemBank == 0x03 && btWordAddr == 0 && btWordCnt >= 22)
                    {//read data section
                        try
                        {
                            bVerify = verifyTag(strData);
                            if (bVerify)
                            {
                                if (!bRead2Erase)
                                {//update tag to zero                                    
                                 //string [] zeroData;
#if READ2ERASE
#if TRUE
                                    btMemBank = 3; //1 for EPC, 3 for user                                    
                                    string zeroTmp = "";
                                    for(int i=0; i< btWordCnt*2; i++)
                                    {
                                        zeroTmp += "00 ";
                                    }
#else
                                    //btMemBank = 3; //1 for EPC, 3 for user                                    
                                    //string zeroTmp = "00 00 00 00 00 00 00 00 00 00 00 00";
                                    btMemBank = 3;
                                    string zeroTmp = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ";
#endif
#else
                                    btMemBank = 3; //1 for EPC, 3 for user
                                    string zeroTmp = "62 4a 49 72 57 49 62 6e 32 31 74 4d 39 44 51 37 74 47 73 2b 79 4c 64 6e 4d 54 4f 4c 71 2f 61 6f 43 56 51 48 67 69 70 64 44 2b 30 3d ";
#endif
                                    if (zeroTmp.StartsWith(" "))
                                        zeroTmp = zeroTmp.Substring(1);

                                    byte btCmd = findCmd();
                                    string[] result = CCommondMethod.StringToStringArray(symmetric.readAccessCode(), 2);// htxtReadAndWritePwd.Text.ToUpper(), 2);
                                    byte[] btAryPwd = CCommondMethod.StringArrayToByteArray(result, result.Length);
                                    result =  CCommondMethod.StringToStringArray(zeroTmp.ToUpper(), 2);
                                    byte[] btAryWriteData = CCommondMethod.StringArrayToByteArray(result, result.Length);
                                    btWordCnt = Convert.ToByte(result.Length / 2 + result.Length % 2);

                                    //1, access, 3, 0, 22, data, 148
                                    strHEXdata = zeroTmp;
                                    /*if (btMemBank == 1)
                                        btWordAddr = 2;
                                    else
                                        btWordAddr = 0;*/
#if READ2ERASE
                                    reader.WriteTag(m_curSetting.btReadId, btAryPwd, btMemBank, btWordAddr, btWordCnt, btAryWriteData, btCmd);

#endif
                                    bRead2Erase = true;
                                }
                            }
                            else
                            {//red on                                                           
                                setVerifiedLEDStatus(0, 1); //red on      
                                WriteLog(lrtxtLog,"verified failed, data " + strData, 1);
#if READ2SCAN
                                //need to move to scan next tag here
                                initScanTag();
#endif
                            }
                        }
                        catch (Exception exp)
                        {
                            setVerifiedLEDStatus(0, 1); //red on      
                            WriteLog(lrtxtLog,strCmd + " got error " + exp.Message, 1);
#if READ2SCAN
                            //Got exception need to move to scan next tag here
                            initScanTag();
#endif
                        }

                    }                    
                }

            }
            catch(Exception exp)
            {
                setVerifiedLEDStatus(0, 1); //red on      
                WriteLog(lrtxtLog,strCmd + " got error " + exp.Message, 1);               
                //need to move to scan next tag here
            }          
        }

        private byte findCmd()
        {
            byte btCmd = 0x00;
            if (radioButtonWrite.Checked)
            {
                btCmd = 0x82;
            }

            if (radioButtonBWrite.Checked)
            {
                btCmd = 0x94;
            }
            return btCmd;
        }

        private byte findMemBank()
        {
           byte byteMemBank = 0xFF;
            if (rdbReserved.Checked)
            {
                byteMemBank = 0x00;
            }
            else if (rdbEpc.Checked)
            {
                byteMemBank = 0x01;
            }
            else if (rdbTid.Checked)
            {
                byteMemBank = 0x02;
            }
            else if (rdbUser.Checked)
            {
                byteMemBank = 0x03;
            }
            else
            {
                WriteLog(lrtxtLog,"Please select the area of tag", 1);
                return byteMemBank;
            }
            return byteMemBank;
        }

        private void btnWriteTag_Click(object sender, EventArgs e)
        {
            try
            {
                if(rdbReserved.Checked && htxtWriteData.Text.Length > 12)
                {
                    MessageBox.Show("Write Wrong section");
                    return;
                }
                byte btMemBank = findMemBank();
                byte btWordAddr = 0x00;
                byte btWordCnt = 0x00;
                byte btCmd = findCmd();
                writeTagRetry = 0;

                if (txtWordAddr.Text.Length != 0)
                {
                    btWordAddr = Convert.ToByte(txtWordAddr.Text);
                }
                else
                {
                    WriteLog(lrtxtLog,"Pleader select the start Add of tag", 1);
                    return;
                }

                if (txtWordCnt.Text.Length != 0)
                {
                    btWordCnt = Convert.ToByte(txtWordCnt.Text);
                }
                else
                {
                    WriteLog(lrtxtLog,"Invalid input characters word cnt failed", 1);
                    return;
                }

                string[] result = CCommondMethod.StringToStringArray(htxtReadAndWritePwd.Text.ToUpper(), 2);
                if (result == null)
                {
                    WriteLog(lrtxtLog,"Invalid input characters, input access pwd", 1);
                    return;
                }
                else if (result.GetLength(0) < 4)
                {
                    WriteLog(lrtxtLog,"Enter at least 4 bytes", 1);
                    return;
                }
                
                byte[] btAryPwd = CCommondMethod.StringArrayToByteArray(result, result.Length);
                result = CCommondMethod.StringToStringArray(htxtWriteData.Text.ToUpper(), 2);
                if (result == null)
                {
                    WriteLog(lrtxtLog,"Invalid input characters error on hex write data", 1);
                    return;
                }

                byte[] btAryWriteData = CCommondMethod.StringArrayToByteArray(result, result.Length);
                btWordCnt = Convert.ToByte(result.Length / 2 + result.Length % 2);
                txtWordCnt.Text = btWordCnt.ToString();

                m_curOperateTagBuffer.dtTagTable.Clear();
                ltvOperate.Items.Clear();

                reader.WriteTag(m_curSetting.btReadId, btAryPwd, btMemBank, btWordAddr, btWordCnt, btAryWriteData,btCmd);//1, access, 3, 0, 22, data, 148
                WriteLog(lrtxtLog,"Write Tag", 0);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        //private static byte btMemBank;
        private string strHEXdata = "";
        private void writeTag()
        {
            byte btMemBank = findMemBank();
            byte btWordAddr = Convert.ToByte(txtWordAddr.Text);
            byte btWordCnt = Convert.ToByte(txtWordCnt.Text);
            byte btCmd = findCmd();
            string[] result = CCommondMethod.StringToStringArray(symmetric.readAccessCode(), 2);// htxtReadAndWritePwd.Text.ToUpper(), 2);
            byte[] btAryPwd = CCommondMethod.StringArrayToByteArray(result, result.Length);
            
            if (strHEXdata == "")
                strHEXdata = htxtWriteData.Text;

            result = CCommondMethod.StringToStringArray(strHEXdata.ToUpper(), 2);
            byte[] btAryWriteData = CCommondMethod.StringArrayToByteArray(result, result.Length);
            btWordCnt = Convert.ToByte(result.Length / 2 + result.Length % 2);
            /*
            if (btMemBank == 1) 
                btWordAddr = 2;
            else
                btWordAddr = 0;*/
            reader.WriteTag(m_curSetting.btReadId, btAryPwd, btMemBank, btWordAddr, btWordCnt, btAryWriteData, btCmd);
            //WriteLog(lrtxtLog,"Write tag");            
        }

        private int WriteTagCount = 0;
        private const int rwTagRetryMAX = 5;
        static int writeTagRetry = 0;
        private string ellapsed;

        private void ProcessWriteTag(Reader.MessageTran msgTran)
        {
            string strCmd = "Write Tag";
            string strErrorCode = string.Empty;
            
            if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + " Failure, failure cause1: " + strErrorCode;

                WriteLog(lrtxtLog,strLog, 1);
                if(writeTagRetry++ < rwTagRetryMAX)
                {                   
                    writeTag();
                    //Thread.Sleep(10);
                    WriteLog(lrtxtLog,"Write Tag retry " + writeTagRetry, 1);                    
                }
                else if(!bRead2Erase)
                {
                    setVerifiedLEDStatus(0, 1); //red on      
                    WriteLog(lrtxtLog,"Erase tag failed", 1);
#if READ2SCAN
                    initScanTag();
#endif
                }
                else
                {//write tag failed
                    setVerifiedLEDStatus(0, 1); //red on 
                    writeTagRetry = 0;
#if READ2SCAN
                    initScanTag();
#endif
                }
            }
            else
            {
                int nLen = msgTran.AryData.Length;
                int nEpcLen = Convert.ToInt32(msgTran.AryData[2])  - 4;
                
                if (msgTran.AryData[nLen - 3] != 0x10)
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[nLen - 3]);
                    string strLog = strCmd + " Failure, failure cause2: " + strErrorCode;

                    WriteLog(lrtxtLog,strLog,1);
#if READ2SCAN
                    initScanTag();
#endif
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

                RefreshOpTag(0x82);
                WriteLog(lrtxtLog,strCmd, 0);

                if (bRead2Erase && bVerify)
                {
                    bVerify = false;
                    bRead2Erase = false;
                    //RFIDTagInfo.addVolumeToFile(RFIDTagInfo.tagInfo);
                    //WriteLog(lrtxtLog,"Get Volume " + RFIDTagInfo.readVolume());
                    //RFIDTagInfo.addLog(RFIDTagInfo.label, RFIDTagInfo.tagInfo);

                    setVerifiedLEDStatus(1, 0); //green on
#if READ2SCAN
                    //return to scan
                    initScanTag();
#endif
                }                    

                if (WriteTagCount == (msgTran.AryData[0] * 256 + msgTran.AryData[1]))
                {
                    WriteTagCount = 0;
                }
            }
        }

        private void btnLockTag_Click(object sender, EventArgs e)
        {
            byte btMemBank = 0x00;
            byte btLockType = 0x00;

            if (rdbAccessPwd.Checked)
            {
                btMemBank = 0x04; //access password
            }
            else if (rdbKillPwd.Checked)
            {
                btMemBank = 0x05;
            }
            else if (rdbEpcMermory.Checked)
            {
                btMemBank = 0x03;
            }
            else if (rdbTidMemory.Checked)
            {
                btMemBank = 0x02;
            }
            else if (rdbUserMemory.Checked)
            {
                btMemBank = 0x01;
            }
            else
            {
                WriteLog(lrtxtLog,"Please select the protected area",1);
                return;
            }

            if (rdbFree.Checked)
            {
                btLockType = 0x00; //open
            }
            else if (rdbFreeEver.Checked)
            {
                btLockType = 0x02; 
            }
            else if (rdbLock.Checked)
            {
                btLockType = 0x01; //lock
            }
            else if (rdbLockEver.Checked)
            {
                btLockType = 0x03;
            }
            else if (rdbLockEverR6.Checked)
            {
                btLockType = 0x06;
            }
            else
            {
                WriteLog(lrtxtLog,"Please select the type of protection",1);
                return;
            }

            string[] reslut = CCommondMethod.StringToStringArray(htxtLockPwd.Text.ToUpper(), 2);

            if (reslut == null)
            {
                WriteLog(lrtxtLog,"Invalid input characters",1);
                return;
            }
            else if (reslut.GetLength(0) < 4)
            {
                WriteLog(lrtxtLog,"Enter at least 4 bytes",1);
                return;
            }

            byte[] btAryPwd = CCommondMethod.StringArrayToByteArray(reslut, reslut.Length);

            m_curOperateTagBuffer.dtTagTable.Clear();
            ltvOperate.Items.Clear();
            reader.LockTag(m_curSetting.btReadId, btAryPwd, btMemBank, btLockType);
        }

        private void ProcessLockTag(Reader.MessageTran msgTran)
        {
            string strCmd = "Lock Tag";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                WriteLog(lrtxtLog,strLog,1);
            }
            else
            {
                int nLen = msgTran.AryData.Length;
                int nEpcLen = Convert.ToInt32(msgTran.AryData[2]) - 4;

                if (msgTran.AryData[nLen - 3] != 0x10)
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[nLen - 3]);
                    string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                    WriteLog(lrtxtLog,strLog,1);
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

                RefreshOpTag(0x83);
                WriteLog(lrtxtLog,strCmd,0);
            }
        }

        private void btnKillTag_Click(object sender, EventArgs e)
        {
            string[] reslut = CCommondMethod.StringToStringArray(htxtKillPwd.Text.ToUpper(), 2);

            if (reslut == null)
            {
                WriteLog(lrtxtLog,"Invalid input characters",1);
                return;
            }
            else if (reslut.GetLength(0) < 4)
            {
                WriteLog(lrtxtLog,"Enter at least 4 bytes",1);
                return;
            }

            byte[] btAryPwd = CCommondMethod.StringArrayToByteArray(reslut, reslut.Length);

            m_curOperateTagBuffer.dtTagTable.Clear();
            ltvOperate.Items.Clear();
            reader.KillTag(m_curSetting.btReadId, btAryPwd);
        }

        private void ProcessKillTag(Reader.MessageTran msgTran)
        {
            string strCmd = "Kill Tag";
            string strErrorCode = string.Empty;

            if (msgTran.AryData.Length == 1)
            {
                strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[0]);
                string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                WriteLog(lrtxtLog,strLog,1);
            }
            else
            {
                int nLen = msgTran.AryData.Length;
                int nEpcLen = Convert.ToInt32(msgTran.AryData[2]) - 4;

                if (msgTran.AryData[nLen - 3] != 0x10)
                {
                    strErrorCode = CCommondMethod.FormatErrorCode(msgTran.AryData[nLen - 3]);
                    string strLog = strCmd + "Failure, failure cause: " + strErrorCode;

                    WriteLog(lrtxtLog,strLog,1);
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

                RefreshOpTag(0x84);
                WriteLog(lrtxtLog,strCmd,0);
            }
        }
  
        private string cleanString(string newStr)
        {
            string tempStr = newStr.Replace('\r', ' ');
            return tempStr.Replace('\n', ' ');
        }


        private void lrtxtLog_DoubleClick(object sender, EventArgs e)
        {
            lrtxtLog.Text = "";
        }

        private void txtTcpPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)ConsoleKey.Backspace)
            {
                e.Handled = false;
            }
        }

        private void txtOutputPower_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)ConsoleKey.Backspace)
            {
                e.Handled = false;
            }
        }

        private void txtChannel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)ConsoleKey.Backspace)
            {
                e.Handled = false;
            }
        }

        private void txtWordAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)ConsoleKey.Backspace)
            {
                e.Handled = false;
            }
        }

        private void txtWordCnt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)ConsoleKey.Backspace)
            {
                e.Handled = false;
            }
        }

        private void cmbSetAccessEpcMatch_DropDown(object sender, EventArgs e)
        {
            cmbSetAccessEpcMatch.Items.Clear();
            DataRow[] drs = m_curInventoryBuffer.dtTagTable.Select();
            foreach (DataRow row in drs)
            {
                cmbSetAccessEpcMatch.Items.Add(row[2].ToString());
            }
        }

        
        private void btnClearInventoryRealResult_Click(object sender, EventArgs e)
        {
            m_curInventoryBuffer.ClearInventoryRealResult();           
            lvRealList.Items.Clear();
            //ltvInventoryTag.Items.Clear();
        }

        private void ltvInventoryEpc_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ltvInventoryTag.Items.Clear();
            DataRow[] drs;

            if (lvRealList.SelectedItems.Count == 0)
            {
                drs = m_curInventoryBuffer.dtTagDetailTable.Select();
                //ShowListView(ltvInventoryTag, drs);
            }
            else
            {
                foreach (ListViewItem itemEpc in lvRealList.SelectedItems)
                {
                    //ListViewItem itemEpc = ltvInventoryEpc.Items[nIndex];
                    string strEpc = itemEpc.SubItems[1].Text;

                    drs = m_curInventoryBuffer.dtTagDetailTable.Select(string.Format("COLEPC = '{0}'", strEpc));
                    //ShowListView(ltvInventoryTag, drs);
                }
            }
        }

        private void ShowListView(ListView ltvListView, DataRow[] drSelect)
        {
            //ltvListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            int nItemCount = ltvListView.Items.Count;
            int nIndex = 1;

            foreach (DataRow row in drSelect)
            {
                ListViewItem item = new ListViewItem();
                item.Text = (nItemCount + nIndex).ToString();
                item.SubItems.Add(row[0].ToString());

                string strTemp = (Convert.ToInt32(row[1].ToString()) - 129).ToString() + "dBm";
                item.SubItems.Add(strTemp);
                byte byTemp = Convert.ToByte(row[1]);
                if (byTemp > 0x50)
                {
                    item.BackColor = Color.PowderBlue;
                }
                else if (byTemp < 0x30)
                {
                    item.BackColor = Color.LemonChiffon;
                }

                item.SubItems.Add(row[2].ToString());
                item.SubItems.Add(row[3].ToString());

                ltvListView.Items.Add(item);
                ltvListView.Items[nIndex - 1].EnsureVisible();
                nIndex++;
            }
        }

        private void ckDisplayLog_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDisplayLog.Checked)
            {
                m_bDisplayLog = true;
            }
            else
            {
                m_bDisplayLog = false;
            }
        }

       
        private void btRealTimeInventory_Click(object sender, EventArgs e)
        {
            try
            {
                m_curInventoryBuffer.ClearInventoryPar();

                if (textRealRound.Text.Length == 0) //1
                {
                    WriteLog(lrtxtLog,"Please enter the number of cycles",1);
                    return;
                }
                m_curInventoryBuffer.btRepeat = Convert.ToByte(textRealRound.Text); //1
                m_curInventoryBuffer.bLoopCustomizedSession = false;
                //m_curInventoryBuffer.lAntenna.Add(0x00);

                //默认循环发送命令                
                if (m_curInventoryBuffer.bLoopInventory) //default = false
                {
                    m_bInventory = false;
                    m_curInventoryBuffer.bLoopInventory = false;
                    btRealTimeInventory.BackColor = Color.WhiteSmoke;
                    btRealTimeInventory.ForeColor = Color.DarkBlue;
                    btRealTimeInventory.Text = "Inventory";
                    timerInventory.Enabled = false;
                    totalTime.Enabled = false;
                    return;
                }
                else
                {
                    m_bInventory = true;
                    m_curInventoryBuffer.bLoopInventory = true;
                    btRealTimeInventory.BackColor = Color.DarkBlue;
                    btRealTimeInventory.ForeColor = Color.White;
                    btRealTimeInventory.Text = "Stop";                    
                }

                m_curInventoryBuffer.bLoopInventoryReal = true;               
                m_curInventoryBuffer.ClearInventoryRealResult();
                lvRealList.Items.Clear();
                //lvRealList.Items.Clear();
                tbRealMaxRssi.Text = "0";
                tbRealMinRssi.Text = "0";
                m_nTotal = 0;

                reader.SetWorkAntenna(m_curSetting.btReadId, 0x00);
                m_curSetting.btWorkAntenna = 0x00;
                timerInventory.Enabled = true;
                totalTime.Enabled = true;                                         
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }          
        }

        private void btRealFresh_Click(object sender, EventArgs e)
        {
            m_curInventoryBuffer.ClearInventoryRealResult();
            
            lvRealList.Items.Clear();
            //lvRealList.Items.Clear();
            ledReal1.Text = "0";
            ledReal2.Text = "0";
            ledReal3.Text = "0";
            ledReal4.Text = "0";
            ledReal5.Text = "0";
            tbRealMaxRssi.Text = "0";
            tbRealMinRssi.Text = "0";
            textRealRound.Text = "1";
            cbRealWorkant1.Checked = true;
            lbRealTagCount.Text = "Tag List：";
        }

        private void btQueryBuffer_Click(object sender, EventArgs e)
        {
            reader.GetInventoryBufferTagCount(m_curSetting.btReadId);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtFirmwareVersion.Text = "";
            htxtReadId.Text = "";
            htbSetIdentifier.Text = "";
            txtReaderTemperature.Text = "";
            //txtOutputPower.Text = "";
            textBox1.Text = "";
            htbGetIdentifier.Text = "";
        }

        private void btGetMonzaStatus_Click(object sender, EventArgs e)
        {
            reader.GetMonzaStatus(m_curSetting.btReadId);
        }

        private void btGetIdentifier_Click(object sender, EventArgs e)
        {
            reader.GetReaderIdentifier(m_curSetting.btReadId);
        }

        private void btSetIdentifier_Click(object sender, EventArgs e)
        {
            try
            {
                string strTemp = htbSetIdentifier.Text.Trim();
                string[] result = CCommondMethod.StringToStringArray(strTemp.ToUpper(), 2);

                if (result == null)
                {
                    WriteLog(lrtxtLog,"Invalid input characters",1);
                    return;
                }
                else if (result.GetLength(0) != 12)
                {
                    WriteLog(lrtxtLog,"Please enter 12 bytes",1);
                    return;
                }
                byte[] readerIdentifier = CCommondMethod.StringArrayToByteArray(result, 12);
                reader.SetReaderIdentifier(m_curSetting.btReadId, readerIdentifier);
                //m_curSetting.btReadId = Convert.ToByte(strTemp, 16);                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btReaderSetupRefresh_Click(object sender, EventArgs e)
        {
            htxtReadId.Text = "";
            htbGetIdentifier.Text = "";
            htbSetIdentifier.Text = "";
            txtFirmwareVersion.Text = "";
            txtReaderTemperature.Text = "";
            rdbGpio1High.Checked = false;
            rdbGpio1Low.Checked = false;
            rdbGpio2High.Checked = false;
            rdbGpio2Low.Checked = false;
            rdbGpio3High.Checked = false;
            rdbGpio3Low.Checked = false;
            rdbGpio4High.Checked = false;
            rdbGpio4Low.Checked = false;

            rdbBeeperModeSlient.Checked = false;
            rdbBeeperModeInventory.Checked = false;
            rdbBeeperModeTag.Checked = false;

            cmbSetBaudrate.SelectedIndex = -1;
        }

        private void btRfSetup_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

            cmbFrequencyStart.SelectedIndex = -1;
            cmbFrequencyEnd.SelectedIndex = -1;

            rdbRegionFcc.Checked = false;
            rdbRegionEtsi.Checked = false;
            rdbRegionChn.Checked = false;

            textReturnLoss.Text = "";
            cmbWorkAnt.SelectedIndex = -1;
            textStartFreq.Text = "";
            TextFreqInterval.Text = "";
            textFreqQuantity.Text = "";

        }
        private void cbRealSession_CheckedChanged(object sender, EventArgs e)
        {
            /*
            if (cbRealSession.Checked == true)
            {
                label97.Enabled = true;
                label98.Enabled = true;
                cmbSession.Enabled = true;
                cmbTarget.Enabled = true;
            }
            else
            {
                label97.Enabled = false;
                label98.Enabled = false;
                cmbSession.Enabled = false;
                cmbTarget.Enabled = false;

                m_session_sl_cb.Checked = false;
            } */
        }

        private void btReturnLoss_Click(object sender, EventArgs e)
        {
            if (cmbReturnLossFreq.SelectedIndex != -1)
            {
                reader.MeasureReturnLoss(m_curSetting.btReadId, Convert.ToByte(cmbReturnLossFreq.SelectedIndex));
            }
        }

        private void cbUserDefineFreq_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUserDefineFreq.Checked == true)
            {
                groupBox21.Enabled = false;
                groupBox23.Enabled = true;

            }
            else
            {
                groupBox21.Enabled = true;
                groupBox23.Enabled = false;
            }
        }

        private void tabCtrMain_Click(object sender, EventArgs e)
        {
            if ((m_curSetting.btRegion < 1) || (m_curSetting.btRegion > 4)) 
            {
                reader.GetFrequencyRegion(m_curSetting.btReadId);
                Thread.Sleep(5);

            }
        }

        private void timerInventory_Tick(object sender, EventArgs e)
        {
            m_nReceiveFlag++;
            if (m_nReceiveFlag >=5)
            {
                RunLoopInventroy();
                m_nReceiveFlag = 0;
            }
        }

        private void totalTimeDisplay(object sender, EventArgs e)
        {
            TimeSpan sp = DateTime.Now - m_curInventoryBuffer.dtStartInventory;
            int totalTime = sp.Minutes * 60 * 1000 + sp.Seconds * 1000 + sp.Milliseconds;
            ledReal5.Text = totalTime.ToString();
            RefreshInventoryReal(0x00);
        }

        private void ProcessTagMask(Reader.MessageTran msgTran)
        {
            string strCmd = "Operate Mask";
            string strErrorCode = string.Empty;
            if (msgTran.AryData.Length == 1)
            {
                if (msgTran.AryData[0] == (byte)0x10)
                {
                    //WriteLog(lrtxtLog,"Command execute success！", 0);
                    return;
                }
                else if (msgTran.AryData[1] == (byte)0x41)
                {
                    strErrorCode = "Invaild parameter";
                }
                else
                {
                    strErrorCode = "Unknown Error";
                }
            }
            else
            {
                if (msgTran.AryData.Length > 7)
                {
                    m_curSetting.btsGetTagMask = msgTran.AryData;
                    RefreshReadSetting(msgTran.Cmd);
                    //WriteLog(lrtxtLog,"Get tag mask sucess");
                    return;
                }
            }

            string strLog = strCmd + "Failure, failure cause: " + strErrorCode;
            WriteLog(lrtxtLog,strLog,1);
        }
         
        private void button8_Click(object sender, EventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            Encoder encoder = Encoding.UTF8.GetEncoder();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            const string wordSeparater = ",";// "\t";
            const string endOfLine = "\n"; // "\r\n";
            if (txt_format_rb.Checked)
            {
                saveFileDialog1.Filter = "csv files (*.csv)|*.csv";//"txt files (*.txt)|*.txt";
                saveFileDialog1.Title = "Save an CSV File";//"Save an text File";
                saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.
                if (saveFileDialog1.FileName != "")
                {
                    // Saves the Image via a FileStream created by the OpenFile method.
                    System.IO.FileStream fs =
                       (System.IO.FileStream)saveFileDialog1.OpenFile();
                    // Saves the Image in the appropriate ImageFormat based upon the
                    // File type selected in the dialog box.
                    // NOTE that the FilterIndex property is one-based.
                    //String strHead = "---------------------------------------------------------------------------------------------------------------------------\r\n";

                    DataTable table = ListViewToDataTable(lvRealList);
                    String title = String.Empty;
                    foreach (ColumnHeader header in lvRealList.Columns)
                    {
                        title += header.Text + wordSeparater; //"\t";
                    }
                    title += endOfLine; //"\r\n";
                    byte[] byteTitile = System.Text.Encoding.UTF8.GetBytes(title);
                    fs.Write(byteTitile, 0, byteTitile.Length);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];
                        String strData = String.Empty;
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            if (j != table.Columns.Count - 1)
                            {
                                strData += row[j].ToString() + wordSeparater; //"\t";
                            }
                            else
                            {
                                strData += row[j].ToString() + wordSeparater + endOfLine; // "\t\r\n";
                            }
                        }
                        Char[] charData = strData.ToString().ToArray();
                        Byte[] byData = new byte[charData.Length];
                        encoder.GetBytes(charData, 0, charData.Length, byData, 0, true);
                        fs.Write(byData, 0, byData.Length);
                    }
                    fs.Close();
                    WriteLog(lrtxtLog,"Export data success！",0);
                }
            }
        }


        //save tag as excel

        public DataTable ListViewToDataTable(ListView listView)
        {
            DataTable table = new DataTable();

            foreach (ColumnHeader header in listView.Columns)
            {
                table.Columns.Add(header.Text,typeof(string));
            }

            foreach (ListViewItem item in listView.Items)
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    //MessageBox.Show(item.SubItems[i].Text);
                    row[i] = item.SubItems[i].Text;
                }

                table.Rows.Add(row);
            }    
            return table;
        }


        //////////////////////////////////////////////////////////////////////////
        public void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
            }
        }
        //save tag as execel
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {

            }
            else
            {
                try
                {

                    int tmp = Convert.ToInt16(textBox1.Text);
                    if (tmp > 33 || tmp < 0)
                    {
                        WriteLog(lrtxtLog,"Parameter exception!",1);
                        textBox1.Text = "";
                        return;
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    textBox1.Text = "";
                }
            }            
        }  

        private void antType1_CheckedChanged(object sender, EventArgs e)
        {
            if (antType1.Checked)
            {
                columnHeader40.Text = "Identification Count";

                //set work ant
                this.cmbWorkAnt.Items.Clear();
                this.cmbWorkAnt.Items.AddRange(new object[] {"ANT 1"});
                this.cmbWorkAnt.SelectedIndex = 0;

                cbRealWorkant1.Enabled = false;
            }
        }

        private void m_session_q_cb_CheckedChanged(object sender, EventArgs e)
        {
            this.refreshLvListView();
            if (m_session_q_cb.Checked)
            {
                m_session_sl_cb.Checked = true;
                
                /*
                m_session_start_q.Enabled = true;
                m_session_min_q.Enabled = true;
                m_session_max_q.Enabled = true;
                m_min_q_content.Enabled = true;
                m_start_q_content.Enabled = true;
                m_max_q_content.Enabled = true;
                 * */
                if (this.m_real_phase_value.SelectedIndex == 1) 
                {
                    m_nSessionPhaseOpened = true;
                }
                else if (this.m_real_phase_value.SelectedIndex == 0)
                {
                    m_nSessionPhaseOpened = false;
                }
                
                this.m_real_phase_value.Enabled = true;
            }
            else
            {
                /*
                m_session_start_q.Enabled = false;
                m_session_min_q.Enabled = false;
                m_session_max_q.Enabled = false;
                m_min_q_content.Enabled = false;
                m_start_q_content.Enabled = false;
                m_max_q_content.Enabled = false;
                 * */
                m_nSessionPhaseOpened = false;
                this.m_real_phase_value.Enabled = false;
            }
        }

        private void m_session_sl_cb_CheckedChanged(object sender, EventArgs e)
        {
            if (m_session_sl_cb.Checked)
            {
                sessionInventoryrb.Checked = true;
                //cbRealSession.Checked = true;
                m_session_sl.Enabled = true;
                m_sl_content.Enabled = true;
            }
            else
            {
                m_session_q_cb.Checked = false;
                m_session_sl.Enabled = false;
                m_sl_content.Enabled = false;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void autoInventoryrb_CheckedChanged(object sender, EventArgs e)
        {
            if(autoInventoryrb.Checked)
            {
                label97.Enabled = false;
                label98.Enabled = false;
                cmbSession.Enabled = false;
                cmbTarget.Enabled = false;

                m_session_sl_cb.Checked = false;
            }
        }

        private void sessionInventoryrb_CheckedChanged(object sender, EventArgs e)
        {
            if(sessionInventoryrb.Checked)
            {
                label97.Enabled = true;
                label98.Enabled = true;
                cmbSession.Enabled = true;
                cmbTarget.Enabled = true;
            }
        }

        private void rdbReserved_CheckedChanged(object sender, EventArgs e)
        {
            if(htxtReadAndWritePwd.Text == "")
            {
                byte[] accessCode = Properties.Resources.AccessCode;
                htxtReadAndWritePwd.Text = RFIDTagInfo.ASCIIToHex(Encoding.ASCII.GetString(accessCode)).ToUpper();                
            }
            if (rdbReserved.Checked)
            {
                txtWordAddr.Text = "0";
                txtWordCnt.Text = "2";
            }
        }

        private void rdbEpc_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEpc.Checked)
            {
                txtWordAddr.Text = "2";
                txtWordCnt.Text = "6";
            }
        }

        private void rdbUser_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbUser.Checked)
            {
                txtWordAddr.Text = "0";
                txtWordCnt.Text = "22";
            }
        }

        private void btEPC2Hex_Click(object sender, EventArgs e)
        {
            if (plainEPCTextBox.Text != "")
            {
#if true
                //mix with 2 ASCII and 10 digits
                string tmpWord = plainEPCTextBox.Text.Substring(0, 2).ToUpper();
                string serialNumber = plainEPCTextBox.Text.Substring(2);
                string EPClabel = RFIDTagInfo.ASCIIToHex(tmpWord);
                ulong uNumber;
                try
                {
                    if(!ulong.TryParse(serialNumber, out uNumber))
                    {
                        uNumber = 0;
                    }
                }
                catch(Exception exp)
                {
                    uNumber = 0;
                }
                string EPSnumber = uNumber.ToString("D20");
                htxtWriteData.Text = EPClabel + RFIDTagInfo.AddHexSpace(EPSnumber);
#else
                //use serial number, support 12 digits
                htxtWriteData.Text = AddHexSpace(Int32.Parse(plainEPCTextBox.Text).ToString("X24"));
                //use ASCII, support 6 words
                //htxtWriteData.Text = symmetric.ASCIIToHex(plainEPCTextBox.Text);
#endif
                labelHEXSize.Text = "HEX Count: " + htxtWriteData.Text.Length/3;
                rdbEpc.Checked = true;
                txtWordAddr.Text = "2";
                txtWordCnt.Text = "6";
            }
        }

        private void EncryptedToHEX_Click(object sender, EventArgs e)
        {
            //load Access Code           
            byte[] accessCode = Properties.Resources.AccessCode;
            string strCode = RFIDTagInfo.ASCIIToHex(Encoding.ASCII.GetString(accessCode)).ToUpper();
            symmetric.loadAccessCode(strCode);

            if (htxtReadAndWritePwd.Text == "")
                htxtReadAndWritePwd.Text = strCode;

            //load key
            byte[] encryptKey = Properties.Resources.SymmetricKey;
            symmetric.loadKey(encryptKey);
            //keyFilePathTextBox.Text = Encoding.ASCII.GetString(encryptKey);

            if (symmetric.readKey() == null)
            {
                WriteLog(lrtxtLog,"Please select key file first, Warning",1);
                return;
            }

            if(plainTextBox.Text == "")
            {
                WriteLog(lrtxtLog,"Please input data first, Warning",1);
                return;
            }

            //read key            
            WriteLog(lrtxtLog, "Read Key " + symmetric.readKey() + ", size " + symmetric.readKey().Length,0);
            //read EPS SN number
            string strEPC = RFIDTagInfo.HEXToASCII(txtAccessEpcMatch.Text.Substring(0,6));
            string strnum = txtAccessEpcMatch.Text.Substring(6).Replace(" ", String.Empty);
            string encrypString = "";
            ulong num = 0;
#if true
            if(ulong.TryParse(strnum, out num))
            {
                encrypString = strEPC + num.ToString() + RFIDTagInfo.serialSep + plainTextBox.Text;
            }
            else
            {
                encrypString = strEPC + "0" + RFIDTagInfo.serialSep + plainTextBox.Text;
                WriteLog(lrtxtLog,"Error, exceed RFID tag ulong",1);
            }
#else
            encrypString = plainTextBox.Text;
#endif
            // Encrypt string = EPC serial + data  
            symmetric.encryptedtext = symmetric.Encrypt(encrypString);//, symmetric.aes.Key);
            string strEncrypted = Convert.ToBase64String(symmetric.encryptedtext);
            WriteLog(lrtxtLog,"Encrypt data " + strEncrypted + 
                               ", size " + symmetric.encryptedtext.Length,0);

            hexTbReserve.Text = RFIDTagInfo.ASCIIToHex(strEncrypted.Substring(0,4));
            htxtWriteData.Text = RFIDTagInfo.ASCIIToHex(strEncrypted.Substring(4));
            labelHEXSize.Text = "HEX Count: " + htxtWriteData.Text.Length/3;

            if((htxtWriteData.Text.Length / 3 ) > 64)
            {
                WriteLog(lrtxtLog,"Error, exceed RFID tag user section", 1);
                labelHEXSize.ForeColor = Color.Red;
                htxtWriteData.ForeColor = Color.Red;
            }

            rdbUser.Checked = true;
            txtWordAddr.Text = "0";
            txtWordCnt.Text = "20";
        }
        
        private void btDecrypt_Click(object sender, EventArgs e)
        {
            //WriteLog(lrtxtLog,"Read Key " + symmetric.readKey() + ", size " + symmetric.readKey().Length, 0);
            if (symmetric.readKey() != "")
            {
                string decryptMsg = symmetric.DecryptFromHEX(hexTbReserve.Text + htxtWriteData.Text);
                WriteLog(lrtxtLog, "Decrypt data " + decryptMsg + ", Size " + decryptMsg.Length, 0);
            }
        }

        private void cmbSetAccessEpcMatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] reslut = CCommondMethod.StringToStringArray(cmbSetAccessEpcMatch.Text.ToUpper(), 2);

            if (reslut == null)
            {
                WriteLog(lrtxtLog,"Please select EPC number",1);
                return;
            }

            byte[] btAryEpc = CCommondMethod.StringArrayToByteArray(reslut, reslut.Length);

            m_curOperateTagBuffer.strAccessEpcMatch = cmbSetAccessEpcMatch.Text;
            txtAccessEpcMatch.Text = cmbSetAccessEpcMatch.Text;
            ckAccessEpcMatch.Checked = true;
            reader.SetAccessEpcMatch(m_curSetting.btReadId, 0x00, Convert.ToByte(btAryEpc.Length), btAryEpc);

            if(plainTextBox.Text != "")
            {
                EncryptedToHEX_Click(sender, e);
            }
        }

        private void btWriteReserver_Click(object sender, EventArgs e)
        {
            try
            {
                byte btMemBank = findMemBank();
                byte btWordAddr = 0x00;
                byte btWordCnt = 0x00;
                byte btCmd = findCmd();
                writeTagRetry = 0;

                if (!rdbReserved.Checked)
                {
                    MessageBox.Show("Write wrong memory bank, error return!");
                    return;
                }

                if (txtWordAddr.Text.Length != 0)
                {
                    btWordAddr = Convert.ToByte(txtWordAddr.Text);
                }
                else
                {
                    WriteLog(lrtxtLog, "Pleader select the start Add of tag", 1);
                    return;
                }

                if (txtWordCnt.Text.Length != 0)
                {
                    btWordCnt = Convert.ToByte(txtWordCnt.Text);
                }
                else
                {
                    WriteLog(lrtxtLog, "Invalid input characters word cnt failed", 1);
                    return;
                }

                string[] result = CCommondMethod.StringToStringArray(htxtReadAndWritePwd.Text.ToUpper(), 2);
                if (result == null)
                {
                    WriteLog(lrtxtLog, "Invalid input characters, input access pwd", 1);
                    return;
                }
                else if (result.GetLength(0) < 4)
                {
                    WriteLog(lrtxtLog, "Enter at least 4 bytes", 1);
                    return;
                }

                byte[] btAryPwd = CCommondMethod.StringArrayToByteArray(result, result.Length);
                result = CCommondMethod.StringToStringArray(hexTbReserve.Text.ToUpper(), 2);
                if (result == null)
                {
                    WriteLog(lrtxtLog, "Invalid input characters error on hex write data", 1);
                    return;
                }

                byte[] btAryWriteData = CCommondMethod.StringArrayToByteArray(result, result.Length);
                btWordCnt = Convert.ToByte(result.Length / 2 + result.Length % 2);

                reader.WriteTag(m_curSetting.btReadId, btAryPwd, btMemBank, btWordAddr, btWordCnt, btAryWriteData, btCmd);//1, access, 3, 0, 22, data, 148
                WriteLog(lrtxtLog, "Write Tag", 0);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
