using System.Windows.Forms;
namespace RFIDApplication
{
    partial class R2000UartDemo
    {
        /// <summary>
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // add ms
        public static long wasteTime = 0;

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(R2000UartDemo));
            this.tabCtrMain = new System.Windows.Forms.TabControl();
            this.PagReaderSetting = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.antType1 = new System.Windows.Forms.RadioButton();
            this.btReaderSetupRefresh = new System.Windows.Forms.Button();
            this.gbCmdReadGpio = new System.Windows.Forms.GroupBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label33 = new System.Windows.Forms.Label();
            this.rdbGpio3High = new System.Windows.Forms.RadioButton();
            this.rdbGpio3Low = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label32 = new System.Windows.Forms.Label();
            this.rdbGpio4High = new System.Windows.Forms.RadioButton();
            this.rdbGpio4Low = new System.Windows.Forms.RadioButton();
            this.btnWriteGpio4Value = new System.Windows.Forms.Button();
            this.btnWriteGpio3Value = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label30 = new System.Windows.Forms.Label();
            this.rdbGpio1High = new System.Windows.Forms.RadioButton();
            this.rdbGpio1Low = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label31 = new System.Windows.Forms.Label();
            this.rdbGpio2High = new System.Windows.Forms.RadioButton();
            this.rdbGpio2Low = new System.Windows.Forms.RadioButton();
            this.btnReadGpioValue = new System.Windows.Forms.Button();
            this.gbCmdBeeper = new System.Windows.Forms.GroupBox();
            this.btnSetBeeperMode = new System.Windows.Forms.Button();
            this.rdbBeeperModeTag = new System.Windows.Forms.RadioButton();
            this.rdbBeeperModeInventory = new System.Windows.Forms.RadioButton();
            this.rdbBeeperModeSlient = new System.Windows.Forms.RadioButton();
            this.gbCmdTemperature = new System.Windows.Forms.GroupBox();
            this.btnGetReaderTemperature = new System.Windows.Forms.Button();
            this.txtReaderTemperature = new System.Windows.Forms.TextBox();
            this.gbCmdVersion = new System.Windows.Forms.GroupBox();
            this.btnGetFirmwareVersion = new System.Windows.Forms.Button();
            this.txtFirmwareVersion = new System.Windows.Forms.TextBox();
            this.btnResetReader = new System.Windows.Forms.Button();
            this.gbCmdBaudrate = new System.Windows.Forms.GroupBox();
            this.htbGetIdentifier = new CustomControl.HexTextBox();
            this.htbSetIdentifier = new CustomControl.HexTextBox();
            this.btSetIdentifier = new System.Windows.Forms.Button();
            this.btGetIdentifier = new System.Windows.Forms.Button();
            this.gbCmdReaderAddress = new System.Windows.Forms.GroupBox();
            this.htxtReadId = new CustomControl.HexTextBox();
            this.btnSetReadAddress = new System.Windows.Forms.Button();
            this.gbTcpIp = new System.Windows.Forms.GroupBox();
            this.btnDisconnectTcp = new System.Windows.Forms.Button();
            this.txtTcpPort = new System.Windows.Forms.TextBox();
            this.btnConnectTcp = new System.Windows.Forms.Button();
            this.ipIpServer = new CustomControl.IpAddressTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbRS232 = new System.Windows.Forms.GroupBox();
            this.btnSetUartBaudrate = new System.Windows.Forms.Button();
            this.btnDisconnectRs232 = new System.Windows.Forms.Button();
            this.cmbSetBaudrate = new System.Windows.Forms.ComboBox();
            this.lbChangeBaudrate = new System.Windows.Forms.Label();
            this.btnConnectRs232 = new System.Windows.Forms.Button();
            this.cmbBaudrate = new System.Windows.Forms.ComboBox();
            this.cmbComPort = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbConnectType = new System.Windows.Forms.GroupBox();
            this.rdbTcpIp = new System.Windows.Forms.RadioButton();
            this.rdbRS232 = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gbReturnLoss = new System.Windows.Forms.GroupBox();
            this.label110 = new System.Windows.Forms.Label();
            this.label109 = new System.Windows.Forms.Label();
            this.cmbReturnLossFreq = new System.Windows.Forms.ComboBox();
            this.label108 = new System.Windows.Forms.Label();
            this.textReturnLoss = new System.Windows.Forms.TextBox();
            this.btReturnLoss = new System.Windows.Forms.Button();
            this.btRfSetup = new System.Windows.Forms.Button();
            this.gbCmdAntenna = new System.Windows.Forms.GroupBox();
            this.label107 = new System.Windows.Forms.Label();
            this.cmbWorkAnt = new System.Windows.Forms.ComboBox();
            this.btnGetWorkAntenna = new System.Windows.Forms.Button();
            this.btnSetWorkAntenna = new System.Windows.Forms.Button();
            this.gbCmdRegion = new System.Windows.Forms.GroupBox();
            this.cbUserDefineFreq = new System.Windows.Forms.CheckBox();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.label106 = new System.Windows.Forms.Label();
            this.label105 = new System.Windows.Forms.Label();
            this.label104 = new System.Windows.Forms.Label();
            this.label103 = new System.Windows.Forms.Label();
            this.label86 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.textFreqQuantity = new System.Windows.Forms.TextBox();
            this.TextFreqInterval = new System.Windows.Forms.TextBox();
            this.textStartFreq = new System.Windows.Forms.TextBox();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.cmbFrequencyEnd = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbFrequencyStart = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.rdbRegionChn = new System.Windows.Forms.RadioButton();
            this.rdbRegionEtsi = new System.Windows.Forms.RadioButton();
            this.rdbRegionFcc = new System.Windows.Forms.RadioButton();
            this.btnGetFrequencyRegion = new System.Windows.Forms.Button();
            this.btnSetFrequencyRegion = new System.Windows.Forms.Button();
            this.gbCmdOutputPower = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnGetOutputPower = new System.Windows.Forms.Button();
            this.btnSetOutputPower = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.pageEpcTest = new System.Windows.Forms.TabPage();
            this.tabEpcTest = new System.Windows.Forms.TabControl();
            this.pageRealMode = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label125 = new System.Windows.Forms.Label();
            this.m_real_phase_value = new System.Windows.Forms.ComboBox();
            this.btRealTimeInventory = new System.Windows.Forms.Button();
            this.label84 = new System.Windows.Forms.Label();
            this.textRealRound = new System.Windows.Forms.TextBox();
            this.sessionInventoryrb = new System.Windows.Forms.RadioButton();
            this.autoInventoryrb = new System.Windows.Forms.RadioButton();
            this.m_session_q_cb = new System.Windows.Forms.CheckBox();
            this.m_session_sl_cb = new System.Windows.Forms.CheckBox();
            this.m_session_sl = new System.Windows.Forms.ComboBox();
            this.m_sl_content = new System.Windows.Forms.Label();
            this.cmbTarget = new System.Windows.Forms.ComboBox();
            this.label98 = new System.Windows.Forms.Label();
            this.cmbSession = new System.Windows.Forms.ComboBox();
            this.label97 = new System.Windows.Forms.Label();
            this.txt_format_rb = new System.Windows.Forms.RadioButton();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.cbRealWorkant1 = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tbRealMinRssi = new System.Windows.Forms.TextBox();
            this.tbRealMaxRssi = new System.Windows.Forms.TextBox();
            this.btRealFresh = new System.Windows.Forms.Button();
            this.label70 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.lbRealTagCount = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ledReal3 = new LxControl.LxLedControl();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.ledReal5 = new LxControl.LxLedControl();
            this.ledReal2 = new LxControl.LxLedControl();
            this.ledReal4 = new LxControl.LxLedControl();
            this.label53 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.ledReal1 = new LxControl.LxLedControl();
            this.lvRealList = new System.Windows.Forms.ListView();
            this.columnHeader37 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader38 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader39 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader40 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader41 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader412 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader42 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pageAcessTag = new System.Windows.Forms.TabPage();
            this.ltvOperate = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbCmdOperateTag = new System.Windows.Forms.GroupBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.btnKillTag = new System.Windows.Forms.Button();
            this.htxtKillPwd = new CustomControl.HexTextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.htxtLockPwd = new CustomControl.HexTextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.rdbUserMemory = new System.Windows.Forms.RadioButton();
            this.rdbTidMemory = new System.Windows.Forms.RadioButton();
            this.rdbEpcMermory = new System.Windows.Forms.RadioButton();
            this.rdbKillPwd = new System.Windows.Forms.RadioButton();
            this.rdbAccessPwd = new System.Windows.Forms.RadioButton();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.rdbLockEverR6 = new System.Windows.Forms.RadioButton();
            this.rdbLockEver = new System.Windows.Forms.RadioButton();
            this.rdbFreeEver = new System.Windows.Forms.RadioButton();
            this.rdbLock = new System.Windows.Forms.RadioButton();
            this.rdbFree = new System.Windows.Forms.RadioButton();
            this.btnLockTag = new System.Windows.Forms.Button();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.labelHEXSize = new System.Windows.Forms.Label();
            this.plainTextBox = new System.Windows.Forms.TextBox();
            this.btEPC2Hex = new System.Windows.Forms.Button();
            this.btDecrypt = new System.Windows.Forms.Button();
            this.accessCodeTextBox = new CustomControl.HexTextBox();
            this.accessCodeFileBrower = new System.Windows.Forms.Button();
            this.label129 = new System.Windows.Forms.Label();
            this.plainEPCTextBox = new System.Windows.Forms.TextBox();
            this.label128 = new System.Windows.Forms.Label();
            this.EncryptedToHEX = new System.Windows.Forms.Button();
            this.keyFilePathTextBox = new CustomControl.HexTextBox();
            this.keyFileBrower = new System.Windows.Forms.Button();
            this.label127 = new System.Windows.Forms.Label();
            this.label126 = new System.Windows.Forms.Label();
            this.radioButtonBWrite = new System.Windows.Forms.RadioButton();
            this.radioButtonWrite = new System.Windows.Forms.RadioButton();
            this.htxtWriteData = new CustomControl.HexTextBox();
            this.txtWordCnt = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.btnWriteTag = new System.Windows.Forms.Button();
            this.btnReadTag = new System.Windows.Forms.Button();
            this.txtWordAddr = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.htxtReadAndWritePwd = new CustomControl.HexTextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.rdbUser = new System.Windows.Forms.RadioButton();
            this.rdbTid = new System.Windows.Forms.RadioButton();
            this.rdbEpc = new System.Windows.Forms.RadioButton();
            this.rdbReserved = new System.Windows.Forms.RadioButton();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.btnSetAccessEpcMatch = new System.Windows.Forms.Button();
            this.cmbSetAccessEpcMatch = new System.Windows.Forms.ComboBox();
            this.txtAccessEpcMatch = new System.Windows.Forms.TextBox();
            this.ckAccessEpcMatch = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.ckDisplayLog = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label76 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.comboBox9 = new System.Windows.Forms.ComboBox();
            this.lxLedControl9 = new LxControl.LxLedControl();
            this.lxLedControl10 = new LxControl.LxLedControl();
            this.lxLedControl11 = new LxControl.LxLedControl();
            this.lxLedControl12 = new LxControl.LxLedControl();
            this.label79 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.lxLedControl13 = new LxControl.LxLedControl();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader43 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader44 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader45 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader46 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader47 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader48 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBox10 = new System.Windows.Forms.ComboBox();
            this.label87 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.label89 = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.label91 = new System.Windows.Forms.Label();
            this.ckClearOperationRec = new System.Windows.Forms.CheckBox();
            this.timerInventory = new System.Windows.Forms.Timer(this.components);
            this.totalTime = new System.Windows.Forms.Timer(this.components);
            this.sortImageList = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lrtxtLog = new CustomControl.LogRichTextBox();
            this.lxLedControl14 = new LxControl.LxLedControl();
            this.lxLedControl15 = new LxControl.LxLedControl();
            this.lxLedControl16 = new LxControl.LxLedControl();
            this.lxLedControl17 = new LxControl.LxLedControl();
            this.lxLedControl18 = new LxControl.LxLedControl();
            this.tabCtrMain.SuspendLayout();
            this.PagReaderSetting.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.gbCmdReadGpio.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.gbCmdBeeper.SuspendLayout();
            this.gbCmdTemperature.SuspendLayout();
            this.gbCmdVersion.SuspendLayout();
            this.gbCmdBaudrate.SuspendLayout();
            this.gbCmdReaderAddress.SuspendLayout();
            this.gbTcpIp.SuspendLayout();
            this.gbRS232.SuspendLayout();
            this.gbConnectType.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gbReturnLoss.SuspendLayout();
            this.gbCmdAntenna.SuspendLayout();
            this.gbCmdRegion.SuspendLayout();
            this.groupBox23.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.gbCmdOutputPower.SuspendLayout();
            this.pageEpcTest.SuspendLayout();
            this.tabEpcTest.SuspendLayout();
            this.pageRealMode.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal1)).BeginInit();
            this.pageAcessTag.SuspendLayout();
            this.gbCmdOperateTag.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl18)).BeginInit();
            this.SuspendLayout();
            // 
            // tabCtrMain
            // 
            this.tabCtrMain.Controls.Add(this.PagReaderSetting);
            this.tabCtrMain.Controls.Add(this.pageEpcTest);
            resources.ApplyResources(this.tabCtrMain, "tabCtrMain");
            this.tabCtrMain.Name = "tabCtrMain";
            this.tabCtrMain.SelectedIndex = 0;
            this.tabCtrMain.Click += new System.EventHandler(this.tabCtrMain_Click);
            // 
            // PagReaderSetting
            // 
            this.PagReaderSetting.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PagReaderSetting.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.PagReaderSetting, "PagReaderSetting");
            this.PagReaderSetting.Name = "PagReaderSetting";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.groupBox24);
            this.tabPage1.Controls.Add(this.btReaderSetupRefresh);
            this.tabPage1.Controls.Add(this.gbCmdReadGpio);
            this.tabPage1.Controls.Add(this.gbCmdBeeper);
            this.tabPage1.Controls.Add(this.gbCmdTemperature);
            this.tabPage1.Controls.Add(this.gbCmdVersion);
            this.tabPage1.Controls.Add(this.btnResetReader);
            this.tabPage1.Controls.Add(this.gbCmdBaudrate);
            this.tabPage1.Controls.Add(this.gbCmdReaderAddress);
            this.tabPage1.Controls.Add(this.gbTcpIp);
            this.tabPage1.Controls.Add(this.gbRS232);
            this.tabPage1.Controls.Add(this.gbConnectType);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.antType1);
            resources.ApplyResources(this.groupBox24, "groupBox24");
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.TabStop = false;
            // 
            // antType1
            // 
            resources.ApplyResources(this.antType1, "antType1");
            this.antType1.Checked = true;
            this.antType1.Name = "antType1";
            this.antType1.TabStop = true;
            this.antType1.UseVisualStyleBackColor = true;
            this.antType1.CheckedChanged += new System.EventHandler(this.antType1_CheckedChanged);
            // 
            // btReaderSetupRefresh
            // 
            resources.ApplyResources(this.btReaderSetupRefresh, "btReaderSetupRefresh");
            this.btReaderSetupRefresh.Name = "btReaderSetupRefresh";
            this.btReaderSetupRefresh.UseVisualStyleBackColor = true;
            this.btReaderSetupRefresh.Click += new System.EventHandler(this.btReaderSetupRefresh_Click);
            // 
            // gbCmdReadGpio
            // 
            this.gbCmdReadGpio.Controls.Add(this.groupBox11);
            this.gbCmdReadGpio.Controls.Add(this.groupBox10);
            this.gbCmdReadGpio.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbCmdReadGpio, "gbCmdReadGpio");
            this.gbCmdReadGpio.Name = "gbCmdReadGpio";
            this.gbCmdReadGpio.TabStop = false;
            // 
            // groupBox11
            // 
            this.groupBox11.BackColor = System.Drawing.Color.Transparent;
            this.groupBox11.Controls.Add(this.groupBox6);
            this.groupBox11.Controls.Add(this.groupBox7);
            this.groupBox11.Controls.Add(this.btnWriteGpio4Value);
            this.groupBox11.Controls.Add(this.btnWriteGpio3Value);
            this.groupBox11.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label33);
            this.groupBox6.Controls.Add(this.rdbGpio3High);
            this.groupBox6.Controls.Add(this.rdbGpio3Low);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // rdbGpio3High
            // 
            resources.ApplyResources(this.rdbGpio3High, "rdbGpio3High");
            this.rdbGpio3High.Name = "rdbGpio3High";
            this.rdbGpio3High.TabStop = true;
            this.rdbGpio3High.UseVisualStyleBackColor = true;
            // 
            // rdbGpio3Low
            // 
            resources.ApplyResources(this.rdbGpio3Low, "rdbGpio3Low");
            this.rdbGpio3Low.Name = "rdbGpio3Low";
            this.rdbGpio3Low.TabStop = true;
            this.rdbGpio3Low.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label32);
            this.groupBox7.Controls.Add(this.rdbGpio4High);
            this.groupBox7.Controls.Add(this.rdbGpio4Low);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.Name = "label32";
            // 
            // rdbGpio4High
            // 
            resources.ApplyResources(this.rdbGpio4High, "rdbGpio4High");
            this.rdbGpio4High.Name = "rdbGpio4High";
            this.rdbGpio4High.TabStop = true;
            this.rdbGpio4High.UseVisualStyleBackColor = true;
            // 
            // rdbGpio4Low
            // 
            resources.ApplyResources(this.rdbGpio4Low, "rdbGpio4Low");
            this.rdbGpio4Low.Name = "rdbGpio4Low";
            this.rdbGpio4Low.TabStop = true;
            this.rdbGpio4Low.UseVisualStyleBackColor = true;
            // 
            // btnWriteGpio4Value
            // 
            resources.ApplyResources(this.btnWriteGpio4Value, "btnWriteGpio4Value");
            this.btnWriteGpio4Value.Name = "btnWriteGpio4Value";
            this.btnWriteGpio4Value.UseVisualStyleBackColor = true;
            this.btnWriteGpio4Value.Click += new System.EventHandler(this.btnWriteGpio4Value_Click);
            // 
            // btnWriteGpio3Value
            // 
            resources.ApplyResources(this.btnWriteGpio3Value, "btnWriteGpio3Value");
            this.btnWriteGpio3Value.Name = "btnWriteGpio3Value";
            this.btnWriteGpio3Value.UseVisualStyleBackColor = true;
            this.btnWriteGpio3Value.Click += new System.EventHandler(this.btnWriteGpio3Value_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.groupBox4);
            this.groupBox10.Controls.Add(this.groupBox5);
            this.groupBox10.Controls.Add(this.btnReadGpioValue);
            this.groupBox10.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.groupBox10, "groupBox10");
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label30);
            this.groupBox4.Controls.Add(this.rdbGpio1High);
            this.groupBox4.Controls.Add(this.rdbGpio1Low);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // rdbGpio1High
            // 
            resources.ApplyResources(this.rdbGpio1High, "rdbGpio1High");
            this.rdbGpio1High.Name = "rdbGpio1High";
            this.rdbGpio1High.TabStop = true;
            this.rdbGpio1High.UseVisualStyleBackColor = true;
            // 
            // rdbGpio1Low
            // 
            resources.ApplyResources(this.rdbGpio1Low, "rdbGpio1Low");
            this.rdbGpio1Low.Name = "rdbGpio1Low";
            this.rdbGpio1Low.TabStop = true;
            this.rdbGpio1Low.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label31);
            this.groupBox5.Controls.Add(this.rdbGpio2High);
            this.groupBox5.Controls.Add(this.rdbGpio2Low);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // rdbGpio2High
            // 
            resources.ApplyResources(this.rdbGpio2High, "rdbGpio2High");
            this.rdbGpio2High.Name = "rdbGpio2High";
            this.rdbGpio2High.TabStop = true;
            this.rdbGpio2High.UseVisualStyleBackColor = true;
            // 
            // rdbGpio2Low
            // 
            resources.ApplyResources(this.rdbGpio2Low, "rdbGpio2Low");
            this.rdbGpio2Low.Name = "rdbGpio2Low";
            this.rdbGpio2Low.TabStop = true;
            this.rdbGpio2Low.UseVisualStyleBackColor = true;
            // 
            // btnReadGpioValue
            // 
            resources.ApplyResources(this.btnReadGpioValue, "btnReadGpioValue");
            this.btnReadGpioValue.Name = "btnReadGpioValue";
            this.btnReadGpioValue.UseVisualStyleBackColor = true;
            this.btnReadGpioValue.Click += new System.EventHandler(this.btnReadGpioValue_Click);
            // 
            // gbCmdBeeper
            // 
            this.gbCmdBeeper.Controls.Add(this.btnSetBeeperMode);
            this.gbCmdBeeper.Controls.Add(this.rdbBeeperModeTag);
            this.gbCmdBeeper.Controls.Add(this.rdbBeeperModeInventory);
            this.gbCmdBeeper.Controls.Add(this.rdbBeeperModeSlient);
            this.gbCmdBeeper.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbCmdBeeper, "gbCmdBeeper");
            this.gbCmdBeeper.Name = "gbCmdBeeper";
            this.gbCmdBeeper.TabStop = false;
            // 
            // btnSetBeeperMode
            // 
            resources.ApplyResources(this.btnSetBeeperMode, "btnSetBeeperMode");
            this.btnSetBeeperMode.Name = "btnSetBeeperMode";
            this.btnSetBeeperMode.UseVisualStyleBackColor = true;
            this.btnSetBeeperMode.Click += new System.EventHandler(this.btnSetBeeperMode_Click);
            // 
            // rdbBeeperModeTag
            // 
            resources.ApplyResources(this.rdbBeeperModeTag, "rdbBeeperModeTag");
            this.rdbBeeperModeTag.Name = "rdbBeeperModeTag";
            this.rdbBeeperModeTag.TabStop = true;
            this.rdbBeeperModeTag.UseVisualStyleBackColor = true;
            // 
            // rdbBeeperModeInventory
            // 
            resources.ApplyResources(this.rdbBeeperModeInventory, "rdbBeeperModeInventory");
            this.rdbBeeperModeInventory.Name = "rdbBeeperModeInventory";
            this.rdbBeeperModeInventory.TabStop = true;
            this.rdbBeeperModeInventory.UseVisualStyleBackColor = true;
            // 
            // rdbBeeperModeSlient
            // 
            resources.ApplyResources(this.rdbBeeperModeSlient, "rdbBeeperModeSlient");
            this.rdbBeeperModeSlient.Name = "rdbBeeperModeSlient";
            this.rdbBeeperModeSlient.TabStop = true;
            this.rdbBeeperModeSlient.UseVisualStyleBackColor = true;
            // 
            // gbCmdTemperature
            // 
            this.gbCmdTemperature.Controls.Add(this.btnGetReaderTemperature);
            this.gbCmdTemperature.Controls.Add(this.txtReaderTemperature);
            this.gbCmdTemperature.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbCmdTemperature, "gbCmdTemperature");
            this.gbCmdTemperature.Name = "gbCmdTemperature";
            this.gbCmdTemperature.TabStop = false;
            // 
            // btnGetReaderTemperature
            // 
            resources.ApplyResources(this.btnGetReaderTemperature, "btnGetReaderTemperature");
            this.btnGetReaderTemperature.Name = "btnGetReaderTemperature";
            this.btnGetReaderTemperature.UseVisualStyleBackColor = true;
            this.btnGetReaderTemperature.Click += new System.EventHandler(this.btnGetReaderTemperature_Click);
            // 
            // txtReaderTemperature
            // 
            resources.ApplyResources(this.txtReaderTemperature, "txtReaderTemperature");
            this.txtReaderTemperature.Name = "txtReaderTemperature";
            this.txtReaderTemperature.ReadOnly = true;
            // 
            // gbCmdVersion
            // 
            this.gbCmdVersion.Controls.Add(this.btnGetFirmwareVersion);
            this.gbCmdVersion.Controls.Add(this.txtFirmwareVersion);
            this.gbCmdVersion.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbCmdVersion, "gbCmdVersion");
            this.gbCmdVersion.Name = "gbCmdVersion";
            this.gbCmdVersion.TabStop = false;
            // 
            // btnGetFirmwareVersion
            // 
            resources.ApplyResources(this.btnGetFirmwareVersion, "btnGetFirmwareVersion");
            this.btnGetFirmwareVersion.Name = "btnGetFirmwareVersion";
            this.btnGetFirmwareVersion.UseVisualStyleBackColor = true;
            this.btnGetFirmwareVersion.Click += new System.EventHandler(this.btnGetFirmwareVersion_Click);
            // 
            // txtFirmwareVersion
            // 
            resources.ApplyResources(this.txtFirmwareVersion, "txtFirmwareVersion");
            this.txtFirmwareVersion.Name = "txtFirmwareVersion";
            this.txtFirmwareVersion.ReadOnly = true;
            // 
            // btnResetReader
            // 
            resources.ApplyResources(this.btnResetReader, "btnResetReader");
            this.btnResetReader.Name = "btnResetReader";
            this.btnResetReader.UseVisualStyleBackColor = true;
            this.btnResetReader.Click += new System.EventHandler(this.btnResetReader_Click);
            // 
            // gbCmdBaudrate
            // 
            this.gbCmdBaudrate.Controls.Add(this.htbGetIdentifier);
            this.gbCmdBaudrate.Controls.Add(this.htbSetIdentifier);
            this.gbCmdBaudrate.Controls.Add(this.btSetIdentifier);
            this.gbCmdBaudrate.Controls.Add(this.btGetIdentifier);
            this.gbCmdBaudrate.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbCmdBaudrate, "gbCmdBaudrate");
            this.gbCmdBaudrate.Name = "gbCmdBaudrate";
            this.gbCmdBaudrate.TabStop = false;
            // 
            // htbGetIdentifier
            // 
            resources.ApplyResources(this.htbGetIdentifier, "htbGetIdentifier");
            this.htbGetIdentifier.Name = "htbGetIdentifier";
            this.htbGetIdentifier.ReadOnly = true;
            // 
            // htbSetIdentifier
            // 
            resources.ApplyResources(this.htbSetIdentifier, "htbSetIdentifier");
            this.htbSetIdentifier.Name = "htbSetIdentifier";
            // 
            // btSetIdentifier
            // 
            resources.ApplyResources(this.btSetIdentifier, "btSetIdentifier");
            this.btSetIdentifier.Name = "btSetIdentifier";
            this.btSetIdentifier.UseVisualStyleBackColor = true;
            this.btSetIdentifier.Click += new System.EventHandler(this.btSetIdentifier_Click);
            // 
            // btGetIdentifier
            // 
            resources.ApplyResources(this.btGetIdentifier, "btGetIdentifier");
            this.btGetIdentifier.Name = "btGetIdentifier";
            this.btGetIdentifier.UseVisualStyleBackColor = true;
            this.btGetIdentifier.Click += new System.EventHandler(this.btGetIdentifier_Click);
            // 
            // gbCmdReaderAddress
            // 
            this.gbCmdReaderAddress.Controls.Add(this.htxtReadId);
            this.gbCmdReaderAddress.Controls.Add(this.btnSetReadAddress);
            this.gbCmdReaderAddress.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbCmdReaderAddress, "gbCmdReaderAddress");
            this.gbCmdReaderAddress.Name = "gbCmdReaderAddress";
            this.gbCmdReaderAddress.TabStop = false;
            // 
            // htxtReadId
            // 
            resources.ApplyResources(this.htxtReadId, "htxtReadId");
            this.htxtReadId.Name = "htxtReadId";
            // 
            // btnSetReadAddress
            // 
            resources.ApplyResources(this.btnSetReadAddress, "btnSetReadAddress");
            this.btnSetReadAddress.Name = "btnSetReadAddress";
            this.btnSetReadAddress.UseVisualStyleBackColor = true;
            this.btnSetReadAddress.Click += new System.EventHandler(this.btnSetReadAddress_Click);
            // 
            // gbTcpIp
            // 
            this.gbTcpIp.Controls.Add(this.btnDisconnectTcp);
            this.gbTcpIp.Controls.Add(this.txtTcpPort);
            this.gbTcpIp.Controls.Add(this.btnConnectTcp);
            this.gbTcpIp.Controls.Add(this.ipIpServer);
            this.gbTcpIp.Controls.Add(this.label4);
            this.gbTcpIp.Controls.Add(this.label3);
            resources.ApplyResources(this.gbTcpIp, "gbTcpIp");
            this.gbTcpIp.Name = "gbTcpIp";
            this.gbTcpIp.TabStop = false;
            // 
            // btnDisconnectTcp
            // 
            resources.ApplyResources(this.btnDisconnectTcp, "btnDisconnectTcp");
            this.btnDisconnectTcp.Name = "btnDisconnectTcp";
            this.btnDisconnectTcp.UseVisualStyleBackColor = true;
            this.btnDisconnectTcp.Click += new System.EventHandler(this.btnDisconnectTcp_Click);
            // 
            // txtTcpPort
            // 
            resources.ApplyResources(this.txtTcpPort, "txtTcpPort");
            this.txtTcpPort.Name = "txtTcpPort";
            // 
            // btnConnectTcp
            // 
            resources.ApplyResources(this.btnConnectTcp, "btnConnectTcp");
            this.btnConnectTcp.Name = "btnConnectTcp";
            this.btnConnectTcp.UseVisualStyleBackColor = true;
            this.btnConnectTcp.Click += new System.EventHandler(this.btnConnectTcp_Click);
            // 
            // ipIpServer
            // 
            this.ipIpServer.IpAddressStr = "";
            resources.ApplyResources(this.ipIpServer, "ipIpServer");
            this.ipIpServer.Name = "ipIpServer";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // gbRS232
            // 
            this.gbRS232.Controls.Add(this.btnSetUartBaudrate);
            this.gbRS232.Controls.Add(this.btnDisconnectRs232);
            this.gbRS232.Controls.Add(this.cmbSetBaudrate);
            this.gbRS232.Controls.Add(this.lbChangeBaudrate);
            this.gbRS232.Controls.Add(this.btnConnectRs232);
            this.gbRS232.Controls.Add(this.cmbBaudrate);
            this.gbRS232.Controls.Add(this.cmbComPort);
            this.gbRS232.Controls.Add(this.label2);
            this.gbRS232.Controls.Add(this.label1);
            resources.ApplyResources(this.gbRS232, "gbRS232");
            this.gbRS232.Name = "gbRS232";
            this.gbRS232.TabStop = false;
            // 
            // btnSetUartBaudrate
            // 
            resources.ApplyResources(this.btnSetUartBaudrate, "btnSetUartBaudrate");
            this.btnSetUartBaudrate.Name = "btnSetUartBaudrate";
            this.btnSetUartBaudrate.UseVisualStyleBackColor = true;
            this.btnSetUartBaudrate.Click += new System.EventHandler(this.btnSetUartBaudrate_Click);
            // 
            // btnDisconnectRs232
            // 
            resources.ApplyResources(this.btnDisconnectRs232, "btnDisconnectRs232");
            this.btnDisconnectRs232.Name = "btnDisconnectRs232";
            this.btnDisconnectRs232.UseVisualStyleBackColor = true;
            this.btnDisconnectRs232.Click += new System.EventHandler(this.btnDisconnectRs232_Click);
            // 
            // cmbSetBaudrate
            // 
            this.cmbSetBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSetBaudrate.FormattingEnabled = true;
            this.cmbSetBaudrate.Items.AddRange(new object[] {
            resources.GetString("cmbSetBaudrate.Items"),
            resources.GetString("cmbSetBaudrate.Items1")});
            resources.ApplyResources(this.cmbSetBaudrate, "cmbSetBaudrate");
            this.cmbSetBaudrate.Name = "cmbSetBaudrate";
            // 
            // lbChangeBaudrate
            // 
            resources.ApplyResources(this.lbChangeBaudrate, "lbChangeBaudrate");
            this.lbChangeBaudrate.Name = "lbChangeBaudrate";
            // 
            // btnConnectRs232
            // 
            resources.ApplyResources(this.btnConnectRs232, "btnConnectRs232");
            this.btnConnectRs232.Name = "btnConnectRs232";
            this.btnConnectRs232.UseVisualStyleBackColor = true;
            this.btnConnectRs232.Click += new System.EventHandler(this.btnConnectRs232_Click);
            // 
            // cmbBaudrate
            // 
            this.cmbBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBaudrate.FormattingEnabled = true;
            this.cmbBaudrate.Items.AddRange(new object[] {
            resources.GetString("cmbBaudrate.Items"),
            resources.GetString("cmbBaudrate.Items1")});
            resources.ApplyResources(this.cmbBaudrate, "cmbBaudrate");
            this.cmbBaudrate.Name = "cmbBaudrate";
            // 
            // cmbComPort
            // 
            this.cmbComPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComPort.FormattingEnabled = true;
            resources.ApplyResources(this.cmbComPort, "cmbComPort");
            this.cmbComPort.Name = "cmbComPort";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // gbConnectType
            // 
            this.gbConnectType.Controls.Add(this.rdbTcpIp);
            this.gbConnectType.Controls.Add(this.rdbRS232);
            resources.ApplyResources(this.gbConnectType, "gbConnectType");
            this.gbConnectType.Name = "gbConnectType";
            this.gbConnectType.TabStop = false;
            // 
            // rdbTcpIp
            // 
            resources.ApplyResources(this.rdbTcpIp, "rdbTcpIp");
            this.rdbTcpIp.Name = "rdbTcpIp";
            this.rdbTcpIp.TabStop = true;
            this.rdbTcpIp.UseVisualStyleBackColor = true;
            this.rdbTcpIp.CheckedChanged += new System.EventHandler(this.rdbTcpIp_CheckedChanged);
            // 
            // rdbRS232
            // 
            resources.ApplyResources(this.rdbRS232, "rdbRS232");
            this.rdbRS232.Name = "rdbRS232";
            this.rdbRS232.TabStop = true;
            this.rdbRS232.UseVisualStyleBackColor = true;
            this.rdbRS232.CheckedChanged += new System.EventHandler(this.rdbRS232_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.gbReturnLoss);
            this.tabPage2.Controls.Add(this.btRfSetup);
            this.tabPage2.Controls.Add(this.gbCmdAntenna);
            this.tabPage2.Controls.Add(this.gbCmdRegion);
            this.tabPage2.Controls.Add(this.gbCmdOutputPower);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            // 
            // gbReturnLoss
            // 
            this.gbReturnLoss.BackColor = System.Drawing.Color.Transparent;
            this.gbReturnLoss.Controls.Add(this.label110);
            this.gbReturnLoss.Controls.Add(this.label109);
            this.gbReturnLoss.Controls.Add(this.cmbReturnLossFreq);
            this.gbReturnLoss.Controls.Add(this.label108);
            this.gbReturnLoss.Controls.Add(this.textReturnLoss);
            this.gbReturnLoss.Controls.Add(this.btReturnLoss);
            this.gbReturnLoss.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbReturnLoss, "gbReturnLoss");
            this.gbReturnLoss.Name = "gbReturnLoss";
            this.gbReturnLoss.TabStop = false;
            // 
            // label110
            // 
            resources.ApplyResources(this.label110, "label110");
            this.label110.Name = "label110";
            // 
            // label109
            // 
            resources.ApplyResources(this.label109, "label109");
            this.label109.Name = "label109";
            // 
            // cmbReturnLossFreq
            // 
            this.cmbReturnLossFreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReturnLossFreq.FormattingEnabled = true;
            this.cmbReturnLossFreq.Items.AddRange(new object[] {
            resources.GetString("cmbReturnLossFreq.Items"),
            resources.GetString("cmbReturnLossFreq.Items1"),
            resources.GetString("cmbReturnLossFreq.Items2"),
            resources.GetString("cmbReturnLossFreq.Items3"),
            resources.GetString("cmbReturnLossFreq.Items4"),
            resources.GetString("cmbReturnLossFreq.Items5"),
            resources.GetString("cmbReturnLossFreq.Items6"),
            resources.GetString("cmbReturnLossFreq.Items7"),
            resources.GetString("cmbReturnLossFreq.Items8"),
            resources.GetString("cmbReturnLossFreq.Items9"),
            resources.GetString("cmbReturnLossFreq.Items10"),
            resources.GetString("cmbReturnLossFreq.Items11"),
            resources.GetString("cmbReturnLossFreq.Items12"),
            resources.GetString("cmbReturnLossFreq.Items13"),
            resources.GetString("cmbReturnLossFreq.Items14"),
            resources.GetString("cmbReturnLossFreq.Items15"),
            resources.GetString("cmbReturnLossFreq.Items16"),
            resources.GetString("cmbReturnLossFreq.Items17"),
            resources.GetString("cmbReturnLossFreq.Items18"),
            resources.GetString("cmbReturnLossFreq.Items19"),
            resources.GetString("cmbReturnLossFreq.Items20"),
            resources.GetString("cmbReturnLossFreq.Items21"),
            resources.GetString("cmbReturnLossFreq.Items22"),
            resources.GetString("cmbReturnLossFreq.Items23"),
            resources.GetString("cmbReturnLossFreq.Items24"),
            resources.GetString("cmbReturnLossFreq.Items25"),
            resources.GetString("cmbReturnLossFreq.Items26"),
            resources.GetString("cmbReturnLossFreq.Items27"),
            resources.GetString("cmbReturnLossFreq.Items28"),
            resources.GetString("cmbReturnLossFreq.Items29"),
            resources.GetString("cmbReturnLossFreq.Items30"),
            resources.GetString("cmbReturnLossFreq.Items31"),
            resources.GetString("cmbReturnLossFreq.Items32"),
            resources.GetString("cmbReturnLossFreq.Items33"),
            resources.GetString("cmbReturnLossFreq.Items34"),
            resources.GetString("cmbReturnLossFreq.Items35"),
            resources.GetString("cmbReturnLossFreq.Items36"),
            resources.GetString("cmbReturnLossFreq.Items37"),
            resources.GetString("cmbReturnLossFreq.Items38"),
            resources.GetString("cmbReturnLossFreq.Items39"),
            resources.GetString("cmbReturnLossFreq.Items40"),
            resources.GetString("cmbReturnLossFreq.Items41"),
            resources.GetString("cmbReturnLossFreq.Items42"),
            resources.GetString("cmbReturnLossFreq.Items43"),
            resources.GetString("cmbReturnLossFreq.Items44"),
            resources.GetString("cmbReturnLossFreq.Items45"),
            resources.GetString("cmbReturnLossFreq.Items46"),
            resources.GetString("cmbReturnLossFreq.Items47"),
            resources.GetString("cmbReturnLossFreq.Items48"),
            resources.GetString("cmbReturnLossFreq.Items49"),
            resources.GetString("cmbReturnLossFreq.Items50"),
            resources.GetString("cmbReturnLossFreq.Items51"),
            resources.GetString("cmbReturnLossFreq.Items52"),
            resources.GetString("cmbReturnLossFreq.Items53"),
            resources.GetString("cmbReturnLossFreq.Items54"),
            resources.GetString("cmbReturnLossFreq.Items55"),
            resources.GetString("cmbReturnLossFreq.Items56"),
            resources.GetString("cmbReturnLossFreq.Items57"),
            resources.GetString("cmbReturnLossFreq.Items58"),
            resources.GetString("cmbReturnLossFreq.Items59")});
            resources.ApplyResources(this.cmbReturnLossFreq, "cmbReturnLossFreq");
            this.cmbReturnLossFreq.Name = "cmbReturnLossFreq";
            // 
            // label108
            // 
            resources.ApplyResources(this.label108, "label108");
            this.label108.Name = "label108";
            // 
            // textReturnLoss
            // 
            resources.ApplyResources(this.textReturnLoss, "textReturnLoss");
            this.textReturnLoss.Name = "textReturnLoss";
            this.textReturnLoss.ReadOnly = true;
            // 
            // btReturnLoss
            // 
            resources.ApplyResources(this.btReturnLoss, "btReturnLoss");
            this.btReturnLoss.Name = "btReturnLoss";
            this.btReturnLoss.UseVisualStyleBackColor = true;
            this.btReturnLoss.Click += new System.EventHandler(this.btReturnLoss_Click);
            // 
            // btRfSetup
            // 
            resources.ApplyResources(this.btRfSetup, "btRfSetup");
            this.btRfSetup.Name = "btRfSetup";
            this.btRfSetup.UseVisualStyleBackColor = true;
            this.btRfSetup.Click += new System.EventHandler(this.btRfSetup_Click);
            // 
            // gbCmdAntenna
            // 
            this.gbCmdAntenna.Controls.Add(this.label107);
            this.gbCmdAntenna.Controls.Add(this.cmbWorkAnt);
            this.gbCmdAntenna.Controls.Add(this.btnGetWorkAntenna);
            this.gbCmdAntenna.Controls.Add(this.btnSetWorkAntenna);
            this.gbCmdAntenna.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbCmdAntenna, "gbCmdAntenna");
            this.gbCmdAntenna.Name = "gbCmdAntenna";
            this.gbCmdAntenna.TabStop = false;
            // 
            // label107
            // 
            resources.ApplyResources(this.label107, "label107");
            this.label107.Name = "label107";
            // 
            // cmbWorkAnt
            // 
            this.cmbWorkAnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWorkAnt.FormattingEnabled = true;
            this.cmbWorkAnt.Items.AddRange(new object[] {
            resources.GetString("cmbWorkAnt.Items"),
            resources.GetString("cmbWorkAnt.Items1"),
            resources.GetString("cmbWorkAnt.Items2"),
            resources.GetString("cmbWorkAnt.Items3")});
            resources.ApplyResources(this.cmbWorkAnt, "cmbWorkAnt");
            this.cmbWorkAnt.Name = "cmbWorkAnt";
            // 
            // btnGetWorkAntenna
            // 
            resources.ApplyResources(this.btnGetWorkAntenna, "btnGetWorkAntenna");
            this.btnGetWorkAntenna.Name = "btnGetWorkAntenna";
            this.btnGetWorkAntenna.UseVisualStyleBackColor = true;
            this.btnGetWorkAntenna.Click += new System.EventHandler(this.btnGetWorkAntenna_Click);
            // 
            // btnSetWorkAntenna
            // 
            resources.ApplyResources(this.btnSetWorkAntenna, "btnSetWorkAntenna");
            this.btnSetWorkAntenna.Name = "btnSetWorkAntenna";
            this.btnSetWorkAntenna.UseVisualStyleBackColor = true;
            this.btnSetWorkAntenna.Click += new System.EventHandler(this.btnSetWorkAntenna_Click);
            // 
            // gbCmdRegion
            // 
            this.gbCmdRegion.Controls.Add(this.cbUserDefineFreq);
            this.gbCmdRegion.Controls.Add(this.groupBox23);
            this.gbCmdRegion.Controls.Add(this.groupBox21);
            this.gbCmdRegion.Controls.Add(this.btnGetFrequencyRegion);
            this.gbCmdRegion.Controls.Add(this.btnSetFrequencyRegion);
            this.gbCmdRegion.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbCmdRegion, "gbCmdRegion");
            this.gbCmdRegion.Name = "gbCmdRegion";
            this.gbCmdRegion.TabStop = false;
            // 
            // cbUserDefineFreq
            // 
            resources.ApplyResources(this.cbUserDefineFreq, "cbUserDefineFreq");
            this.cbUserDefineFreq.Name = "cbUserDefineFreq";
            this.cbUserDefineFreq.UseVisualStyleBackColor = true;
            this.cbUserDefineFreq.CheckedChanged += new System.EventHandler(this.cbUserDefineFreq_CheckedChanged);
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.label106);
            this.groupBox23.Controls.Add(this.label105);
            this.groupBox23.Controls.Add(this.label104);
            this.groupBox23.Controls.Add(this.label103);
            this.groupBox23.Controls.Add(this.label86);
            this.groupBox23.Controls.Add(this.label75);
            this.groupBox23.Controls.Add(this.textFreqQuantity);
            this.groupBox23.Controls.Add(this.TextFreqInterval);
            this.groupBox23.Controls.Add(this.textStartFreq);
            this.groupBox23.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.groupBox23, "groupBox23");
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.TabStop = false;
            // 
            // label106
            // 
            resources.ApplyResources(this.label106, "label106");
            this.label106.Name = "label106";
            // 
            // label105
            // 
            resources.ApplyResources(this.label105, "label105");
            this.label105.Name = "label105";
            // 
            // label104
            // 
            resources.ApplyResources(this.label104, "label104");
            this.label104.Name = "label104";
            // 
            // label103
            // 
            resources.ApplyResources(this.label103, "label103");
            this.label103.Name = "label103";
            // 
            // label86
            // 
            resources.ApplyResources(this.label86, "label86");
            this.label86.Name = "label86";
            // 
            // label75
            // 
            resources.ApplyResources(this.label75, "label75");
            this.label75.Name = "label75";
            // 
            // textFreqQuantity
            // 
            resources.ApplyResources(this.textFreqQuantity, "textFreqQuantity");
            this.textFreqQuantity.Name = "textFreqQuantity";
            // 
            // TextFreqInterval
            // 
            resources.ApplyResources(this.TextFreqInterval, "TextFreqInterval");
            this.TextFreqInterval.Name = "TextFreqInterval";
            // 
            // textStartFreq
            // 
            resources.ApplyResources(this.textStartFreq, "textStartFreq");
            this.textStartFreq.Name = "textStartFreq";
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.label37);
            this.groupBox21.Controls.Add(this.label36);
            this.groupBox21.Controls.Add(this.cmbFrequencyEnd);
            this.groupBox21.Controls.Add(this.label13);
            this.groupBox21.Controls.Add(this.cmbFrequencyStart);
            this.groupBox21.Controls.Add(this.label12);
            this.groupBox21.Controls.Add(this.rdbRegionChn);
            this.groupBox21.Controls.Add(this.rdbRegionEtsi);
            this.groupBox21.Controls.Add(this.rdbRegionFcc);
            this.groupBox21.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.groupBox21, "groupBox21");
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.TabStop = false;
            // 
            // label37
            // 
            resources.ApplyResources(this.label37, "label37");
            this.label37.Name = "label37";
            // 
            // label36
            // 
            resources.ApplyResources(this.label36, "label36");
            this.label36.Name = "label36";
            // 
            // cmbFrequencyEnd
            // 
            this.cmbFrequencyEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrequencyEnd.FormattingEnabled = true;
            resources.ApplyResources(this.cmbFrequencyEnd, "cmbFrequencyEnd");
            this.cmbFrequencyEnd.Name = "cmbFrequencyEnd";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // cmbFrequencyStart
            // 
            this.cmbFrequencyStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrequencyStart.FormattingEnabled = true;
            resources.ApplyResources(this.cmbFrequencyStart, "cmbFrequencyStart");
            this.cmbFrequencyStart.Name = "cmbFrequencyStart";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // rdbRegionChn
            // 
            resources.ApplyResources(this.rdbRegionChn, "rdbRegionChn");
            this.rdbRegionChn.Name = "rdbRegionChn";
            this.rdbRegionChn.TabStop = true;
            this.rdbRegionChn.UseVisualStyleBackColor = true;
            this.rdbRegionChn.CheckedChanged += new System.EventHandler(this.rdbRegionChn_CheckedChanged);
            // 
            // rdbRegionEtsi
            // 
            resources.ApplyResources(this.rdbRegionEtsi, "rdbRegionEtsi");
            this.rdbRegionEtsi.Name = "rdbRegionEtsi";
            this.rdbRegionEtsi.TabStop = true;
            this.rdbRegionEtsi.UseVisualStyleBackColor = true;
            this.rdbRegionEtsi.CheckedChanged += new System.EventHandler(this.rdbRegionEtsi_CheckedChanged);
            // 
            // rdbRegionFcc
            // 
            resources.ApplyResources(this.rdbRegionFcc, "rdbRegionFcc");
            this.rdbRegionFcc.Name = "rdbRegionFcc";
            this.rdbRegionFcc.TabStop = true;
            this.rdbRegionFcc.UseVisualStyleBackColor = true;
            this.rdbRegionFcc.CheckedChanged += new System.EventHandler(this.rdbRegionFcc_CheckedChanged);
            // 
            // btnGetFrequencyRegion
            // 
            resources.ApplyResources(this.btnGetFrequencyRegion, "btnGetFrequencyRegion");
            this.btnGetFrequencyRegion.Name = "btnGetFrequencyRegion";
            this.btnGetFrequencyRegion.UseVisualStyleBackColor = true;
            this.btnGetFrequencyRegion.Click += new System.EventHandler(this.btnGetFrequencyRegion_Click);
            // 
            // btnSetFrequencyRegion
            // 
            resources.ApplyResources(this.btnSetFrequencyRegion, "btnSetFrequencyRegion");
            this.btnSetFrequencyRegion.Name = "btnSetFrequencyRegion";
            this.btnSetFrequencyRegion.UseVisualStyleBackColor = true;
            this.btnSetFrequencyRegion.Click += new System.EventHandler(this.btnSetFrequencyRegion_Click);
            // 
            // gbCmdOutputPower
            // 
            this.gbCmdOutputPower.Controls.Add(this.label18);
            this.gbCmdOutputPower.Controls.Add(this.textBox1);
            this.gbCmdOutputPower.Controls.Add(this.label15);
            this.gbCmdOutputPower.Controls.Add(this.btnGetOutputPower);
            this.gbCmdOutputPower.Controls.Add(this.btnSetOutputPower);
            this.gbCmdOutputPower.Controls.Add(this.label9);
            this.gbCmdOutputPower.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.gbCmdOutputPower, "gbCmdOutputPower");
            this.gbCmdOutputPower.Name = "gbCmdOutputPower";
            this.gbCmdOutputPower.TabStop = false;
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // btnGetOutputPower
            // 
            resources.ApplyResources(this.btnGetOutputPower, "btnGetOutputPower");
            this.btnGetOutputPower.Name = "btnGetOutputPower";
            this.btnGetOutputPower.UseVisualStyleBackColor = true;
            this.btnGetOutputPower.Click += new System.EventHandler(this.btnGetOutputPower_Click);
            // 
            // btnSetOutputPower
            // 
            resources.ApplyResources(this.btnSetOutputPower, "btnSetOutputPower");
            this.btnSetOutputPower.Name = "btnSetOutputPower";
            this.btnSetOutputPower.UseVisualStyleBackColor = true;
            this.btnSetOutputPower.Click += new System.EventHandler(this.btnSetOutputPower_Click);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // pageEpcTest
            // 
            this.pageEpcTest.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pageEpcTest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageEpcTest.Controls.Add(this.tabEpcTest);
            resources.ApplyResources(this.pageEpcTest, "pageEpcTest");
            this.pageEpcTest.ForeColor = System.Drawing.SystemColors.Desktop;
            this.pageEpcTest.Name = "pageEpcTest";
            // 
            // tabEpcTest
            // 
            this.tabEpcTest.Controls.Add(this.pageRealMode);
            this.tabEpcTest.Controls.Add(this.pageAcessTag);
            resources.ApplyResources(this.tabEpcTest, "tabEpcTest");
            this.tabEpcTest.Name = "tabEpcTest";
            this.tabEpcTest.SelectedIndex = 0;
            this.tabEpcTest.TabStop = false;
            // 
            // pageRealMode
            // 
            this.pageRealMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pageRealMode.Controls.Add(this.tableLayoutPanel1);
            this.pageRealMode.Controls.Add(this.txt_format_rb);
            this.pageRealMode.Controls.Add(this.btnSaveFile);
            this.pageRealMode.Controls.Add(this.groupBox20);
            this.pageRealMode.Controls.Add(this.tbRealMinRssi);
            this.pageRealMode.Controls.Add(this.tbRealMaxRssi);
            this.pageRealMode.Controls.Add(this.btRealFresh);
            this.pageRealMode.Controls.Add(this.label70);
            this.pageRealMode.Controls.Add(this.label74);
            this.pageRealMode.Controls.Add(this.lbRealTagCount);
            this.pageRealMode.Controls.Add(this.groupBox1);
            this.pageRealMode.Controls.Add(this.lvRealList);
            resources.ApplyResources(this.pageRealMode, "pageRealMode");
            this.pageRealMode.Name = "pageRealMode";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label125);
            this.panel5.Controls.Add(this.m_real_phase_value);
            this.panel5.Controls.Add(this.btRealTimeInventory);
            this.panel5.Controls.Add(this.label84);
            this.panel5.Controls.Add(this.textRealRound);
            this.panel5.Controls.Add(this.sessionInventoryrb);
            this.panel5.Controls.Add(this.autoInventoryrb);
            this.panel5.Controls.Add(this.m_session_q_cb);
            this.panel5.Controls.Add(this.m_session_sl_cb);
            this.panel5.Controls.Add(this.m_session_sl);
            this.panel5.Controls.Add(this.m_sl_content);
            this.panel5.Controls.Add(this.cmbTarget);
            this.panel5.Controls.Add(this.label98);
            this.panel5.Controls.Add(this.cmbSession);
            this.panel5.Controls.Add(this.label97);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // label125
            // 
            resources.ApplyResources(this.label125, "label125");
            this.label125.Name = "label125";
            // 
            // m_real_phase_value
            // 
            this.m_real_phase_value.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.m_real_phase_value, "m_real_phase_value");
            this.m_real_phase_value.FormattingEnabled = true;
            this.m_real_phase_value.Items.AddRange(new object[] {
            resources.GetString("m_real_phase_value.Items"),
            resources.GetString("m_real_phase_value.Items1")});
            this.m_real_phase_value.Name = "m_real_phase_value";
            // 
            // btRealTimeInventory
            // 
            resources.ApplyResources(this.btRealTimeInventory, "btRealTimeInventory");
            this.btRealTimeInventory.ForeColor = System.Drawing.Color.DarkBlue;
            this.btRealTimeInventory.Name = "btRealTimeInventory";
            this.btRealTimeInventory.UseVisualStyleBackColor = true;
            this.btRealTimeInventory.Click += new System.EventHandler(this.btRealTimeInventory_Click);
            // 
            // label84
            // 
            resources.ApplyResources(this.label84, "label84");
            this.label84.Name = "label84";
            // 
            // textRealRound
            // 
            resources.ApplyResources(this.textRealRound, "textRealRound");
            this.textRealRound.Name = "textRealRound";
            // 
            // sessionInventoryrb
            // 
            resources.ApplyResources(this.sessionInventoryrb, "sessionInventoryrb");
            this.sessionInventoryrb.Name = "sessionInventoryrb";
            this.sessionInventoryrb.TabStop = true;
            this.sessionInventoryrb.UseVisualStyleBackColor = true;
            this.sessionInventoryrb.CheckedChanged += new System.EventHandler(this.sessionInventoryrb_CheckedChanged);
            // 
            // autoInventoryrb
            // 
            resources.ApplyResources(this.autoInventoryrb, "autoInventoryrb");
            this.autoInventoryrb.Checked = true;
            this.autoInventoryrb.Name = "autoInventoryrb";
            this.autoInventoryrb.TabStop = true;
            this.autoInventoryrb.UseVisualStyleBackColor = true;
            this.autoInventoryrb.CheckedChanged += new System.EventHandler(this.autoInventoryrb_CheckedChanged);
            // 
            // m_session_q_cb
            // 
            resources.ApplyResources(this.m_session_q_cb, "m_session_q_cb");
            this.m_session_q_cb.Name = "m_session_q_cb";
            this.m_session_q_cb.UseVisualStyleBackColor = true;
            this.m_session_q_cb.CheckedChanged += new System.EventHandler(this.m_session_q_cb_CheckedChanged);
            // 
            // m_session_sl_cb
            // 
            resources.ApplyResources(this.m_session_sl_cb, "m_session_sl_cb");
            this.m_session_sl_cb.Name = "m_session_sl_cb";
            this.m_session_sl_cb.UseVisualStyleBackColor = true;
            this.m_session_sl_cb.CheckedChanged += new System.EventHandler(this.m_session_sl_cb_CheckedChanged);
            // 
            // m_session_sl
            // 
            this.m_session_sl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.m_session_sl, "m_session_sl");
            this.m_session_sl.FormattingEnabled = true;
            this.m_session_sl.Items.AddRange(new object[] {
            resources.GetString("m_session_sl.Items"),
            resources.GetString("m_session_sl.Items1"),
            resources.GetString("m_session_sl.Items2"),
            resources.GetString("m_session_sl.Items3")});
            this.m_session_sl.Name = "m_session_sl";
            // 
            // m_sl_content
            // 
            resources.ApplyResources(this.m_sl_content, "m_sl_content");
            this.m_sl_content.Name = "m_sl_content";
            // 
            // cmbTarget
            // 
            this.cmbTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbTarget, "cmbTarget");
            this.cmbTarget.FormattingEnabled = true;
            this.cmbTarget.Items.AddRange(new object[] {
            resources.GetString("cmbTarget.Items"),
            resources.GetString("cmbTarget.Items1")});
            this.cmbTarget.Name = "cmbTarget";
            // 
            // label98
            // 
            resources.ApplyResources(this.label98, "label98");
            this.label98.Name = "label98";
            // 
            // cmbSession
            // 
            this.cmbSession.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbSession, "cmbSession");
            this.cmbSession.FormattingEnabled = true;
            this.cmbSession.Items.AddRange(new object[] {
            resources.GetString("cmbSession.Items"),
            resources.GetString("cmbSession.Items1"),
            resources.GetString("cmbSession.Items2"),
            resources.GetString("cmbSession.Items3"),
            resources.GetString("cmbSession.Items4")});
            this.cmbSession.Name = "cmbSession";
            // 
            // label97
            // 
            resources.ApplyResources(this.label97, "label97");
            this.label97.Name = "label97";
            // 
            // txt_format_rb
            // 
            resources.ApplyResources(this.txt_format_rb, "txt_format_rb");
            this.txt_format_rb.Checked = true;
            this.txt_format_rb.Name = "txt_format_rb";
            this.txt_format_rb.TabStop = true;
            this.txt_format_rb.UseVisualStyleBackColor = true;
            // 
            // btnSaveFile
            // 
            resources.ApplyResources(this.btnSaveFile, "btnSaveFile");
            this.btnSaveFile.ForeColor = System.Drawing.SystemColors.Desktop;
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.button8_Click);
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.cbRealWorkant1);
            this.groupBox20.Controls.Add(this.label19);
            resources.ApplyResources(this.groupBox20, "groupBox20");
            this.groupBox20.ForeColor = System.Drawing.Color.Black;
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.TabStop = false;
            // 
            // cbRealWorkant1
            // 
            resources.ApplyResources(this.cbRealWorkant1, "cbRealWorkant1");
            this.cbRealWorkant1.Checked = true;
            this.cbRealWorkant1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRealWorkant1.Name = "cbRealWorkant1";
            this.cbRealWorkant1.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // tbRealMinRssi
            // 
            resources.ApplyResources(this.tbRealMinRssi, "tbRealMinRssi");
            this.tbRealMinRssi.Name = "tbRealMinRssi";
            this.tbRealMinRssi.ReadOnly = true;
            // 
            // tbRealMaxRssi
            // 
            resources.ApplyResources(this.tbRealMaxRssi, "tbRealMaxRssi");
            this.tbRealMaxRssi.Name = "tbRealMaxRssi";
            this.tbRealMaxRssi.ReadOnly = true;
            // 
            // btRealFresh
            // 
            resources.ApplyResources(this.btRealFresh, "btRealFresh");
            this.btRealFresh.ForeColor = System.Drawing.SystemColors.Desktop;
            this.btRealFresh.Name = "btRealFresh";
            this.btRealFresh.UseVisualStyleBackColor = true;
            this.btRealFresh.Click += new System.EventHandler(this.btRealFresh_Click);
            // 
            // label70
            // 
            resources.ApplyResources(this.label70, "label70");
            this.label70.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label70.Name = "label70";
            // 
            // label74
            // 
            resources.ApplyResources(this.label74, "label74");
            this.label74.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label74.Name = "label74";
            // 
            // lbRealTagCount
            // 
            resources.ApplyResources(this.lbRealTagCount, "lbRealTagCount");
            this.lbRealTagCount.ForeColor = System.Drawing.SystemColors.Desktop;
            this.lbRealTagCount.Name = "lbRealTagCount";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ledReal3);
            this.groupBox1.Controls.Add(this.comboBox6);
            this.groupBox1.Controls.Add(this.ledReal5);
            this.groupBox1.Controls.Add(this.ledReal2);
            this.groupBox1.Controls.Add(this.ledReal4);
            this.groupBox1.Controls.Add(this.label53);
            this.groupBox1.Controls.Add(this.label66);
            this.groupBox1.Controls.Add(this.label67);
            this.groupBox1.Controls.Add(this.label68);
            this.groupBox1.Controls.Add(this.label69);
            this.groupBox1.Controls.Add(this.ledReal1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // ledReal3
            // 
            this.ledReal3.BackColor = System.Drawing.Color.Transparent;
            this.ledReal3.BackColor_1 = System.Drawing.Color.Transparent;
            this.ledReal3.BackColor_2 = System.Drawing.Color.DarkRed;
            this.ledReal3.BevelRate = 0.1F;
            this.ledReal3.BorderColor = System.Drawing.Color.Lavender;
            this.ledReal3.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.ledReal3.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.ledReal3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ledReal3.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.ledReal3, "ledReal3");
            this.ledReal3.Name = "ledReal3";
            this.ledReal3.RoundCorner = true;
            this.ledReal3.SegmentIntervalRatio = 50;
            this.ledReal3.ShowHighlight = true;
            this.ledReal3.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            this.ledReal3.TotalCharCount = 10;
            // 
            // comboBox6
            // 
            this.comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox6.ForeColor = System.Drawing.SystemColors.InfoText;
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Items.AddRange(new object[] {
            resources.GetString("comboBox6.Items"),
            resources.GetString("comboBox6.Items1"),
            resources.GetString("comboBox6.Items2"),
            resources.GetString("comboBox6.Items3"),
            resources.GetString("comboBox6.Items4")});
            resources.ApplyResources(this.comboBox6, "comboBox6");
            this.comboBox6.Name = "comboBox6";
            // 
            // ledReal5
            // 
            this.ledReal5.BackColor = System.Drawing.Color.Transparent;
            this.ledReal5.BackColor_1 = System.Drawing.Color.Transparent;
            this.ledReal5.BackColor_2 = System.Drawing.Color.DarkRed;
            this.ledReal5.BevelRate = 0.1F;
            this.ledReal5.BorderColor = System.Drawing.Color.Lavender;
            this.ledReal5.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.ledReal5.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.ledReal5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ledReal5.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.ledReal5, "ledReal5");
            this.ledReal5.Name = "ledReal5";
            this.ledReal5.RoundCorner = true;
            this.ledReal5.SegmentIntervalRatio = 50;
            this.ledReal5.ShowHighlight = true;
            this.ledReal5.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            this.ledReal5.TotalCharCount = 10;
            // 
            // ledReal2
            // 
            this.ledReal2.BackColor = System.Drawing.Color.Transparent;
            this.ledReal2.BackColor_1 = System.Drawing.Color.Transparent;
            this.ledReal2.BackColor_2 = System.Drawing.Color.DarkRed;
            this.ledReal2.BevelRate = 0.1F;
            this.ledReal2.BorderColor = System.Drawing.Color.Lavender;
            this.ledReal2.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.ledReal2.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.ledReal2.ForeColor = System.Drawing.Color.Purple;
            this.ledReal2.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.ledReal2, "ledReal2");
            this.ledReal2.Name = "ledReal2";
            this.ledReal2.RoundCorner = true;
            this.ledReal2.SegmentIntervalRatio = 50;
            this.ledReal2.ShowHighlight = true;
            this.ledReal2.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            this.ledReal2.TotalCharCount = 6;
            // 
            // ledReal4
            // 
            this.ledReal4.BackColor = System.Drawing.Color.Transparent;
            this.ledReal4.BackColor_1 = System.Drawing.Color.Transparent;
            this.ledReal4.BackColor_2 = System.Drawing.Color.DarkRed;
            this.ledReal4.BevelRate = 0.1F;
            this.ledReal4.BorderColor = System.Drawing.Color.Lavender;
            this.ledReal4.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.ledReal4.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.ledReal4.ForeColor = System.Drawing.Color.Purple;
            this.ledReal4.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.ledReal4, "ledReal4");
            this.ledReal4.Name = "ledReal4";
            this.ledReal4.RoundCorner = true;
            this.ledReal4.SegmentIntervalRatio = 50;
            this.ledReal4.ShowHighlight = true;
            this.ledReal4.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            this.ledReal4.TotalCharCount = 6;
            // 
            // label53
            // 
            resources.ApplyResources(this.label53, "label53");
            this.label53.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label53.Name = "label53";
            // 
            // label66
            // 
            resources.ApplyResources(this.label66, "label66");
            this.label66.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label66.Name = "label66";
            // 
            // label67
            // 
            resources.ApplyResources(this.label67, "label67");
            this.label67.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label67.Name = "label67";
            // 
            // label68
            // 
            resources.ApplyResources(this.label68, "label68");
            this.label68.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label68.Name = "label68";
            // 
            // label69
            // 
            resources.ApplyResources(this.label69, "label69");
            this.label69.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label69.Name = "label69";
            // 
            // ledReal1
            // 
            this.ledReal1.BackColor = System.Drawing.Color.Transparent;
            this.ledReal1.BackColor_1 = System.Drawing.Color.Transparent;
            this.ledReal1.BackColor_2 = System.Drawing.Color.DarkRed;
            this.ledReal1.BevelRate = 0.1F;
            this.ledReal1.BorderColor = System.Drawing.Color.Lavender;
            this.ledReal1.BorderWidth = 3;
            this.ledReal1.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.ledReal1.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.ledReal1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ledReal1.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.ledReal1, "ledReal1");
            this.ledReal1.Name = "ledReal1";
            this.ledReal1.RoundCorner = true;
            this.ledReal1.SegmentIntervalRatio = 50;
            this.ledReal1.ShowHighlight = true;
            this.ledReal1.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            // 
            // lvRealList
            // 
            this.lvRealList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader37,
            this.columnHeader38,
            this.columnHeader39,
            this.columnHeader40,
            this.columnHeader41,
            this.columnHeader412,
            this.columnHeader42});
            resources.ApplyResources(this.lvRealList, "lvRealList");
            this.lvRealList.GridLines = true;
            this.lvRealList.HideSelection = false;
            this.lvRealList.Name = "lvRealList";
            this.lvRealList.UseCompatibleStateImageBehavior = false;
            this.lvRealList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader37
            // 
            resources.ApplyResources(this.columnHeader37, "columnHeader37");
            // 
            // columnHeader38
            // 
            resources.ApplyResources(this.columnHeader38, "columnHeader38");
            // 
            // columnHeader39
            // 
            resources.ApplyResources(this.columnHeader39, "columnHeader39");
            // 
            // columnHeader40
            // 
            resources.ApplyResources(this.columnHeader40, "columnHeader40");
            // 
            // columnHeader41
            // 
            resources.ApplyResources(this.columnHeader41, "columnHeader41");
            // 
            // columnHeader412
            // 
            resources.ApplyResources(this.columnHeader412, "columnHeader412");
            // 
            // columnHeader42
            // 
            resources.ApplyResources(this.columnHeader42, "columnHeader42");
            // 
            // pageAcessTag
            // 
            this.pageAcessTag.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pageAcessTag.Controls.Add(this.ltvOperate);
            this.pageAcessTag.Controls.Add(this.gbCmdOperateTag);
            resources.ApplyResources(this.pageAcessTag, "pageAcessTag");
            this.pageAcessTag.Name = "pageAcessTag";
            this.pageAcessTag.UseVisualStyleBackColor = true;
            // 
            // ltvOperate
            // 
            this.ltvOperate.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15});
            resources.ApplyResources(this.ltvOperate, "ltvOperate");
            this.ltvOperate.GridLines = true;
            this.ltvOperate.HideSelection = false;
            this.ltvOperate.Name = "ltvOperate";
            this.ltvOperate.UseCompatibleStateImageBehavior = false;
            this.ltvOperate.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader9
            // 
            resources.ApplyResources(this.columnHeader9, "columnHeader9");
            // 
            // columnHeader10
            // 
            resources.ApplyResources(this.columnHeader10, "columnHeader10");
            // 
            // columnHeader11
            // 
            resources.ApplyResources(this.columnHeader11, "columnHeader11");
            // 
            // columnHeader12
            // 
            resources.ApplyResources(this.columnHeader12, "columnHeader12");
            // 
            // columnHeader13
            // 
            resources.ApplyResources(this.columnHeader13, "columnHeader13");
            // 
            // columnHeader14
            // 
            resources.ApplyResources(this.columnHeader14, "columnHeader14");
            // 
            // columnHeader15
            // 
            resources.ApplyResources(this.columnHeader15, "columnHeader15");
            // 
            // gbCmdOperateTag
            // 
            this.gbCmdOperateTag.Controls.Add(this.groupBox16);
            this.gbCmdOperateTag.Controls.Add(this.groupBox15);
            this.gbCmdOperateTag.Controls.Add(this.groupBox14);
            this.gbCmdOperateTag.Controls.Add(this.groupBox13);
            resources.ApplyResources(this.gbCmdOperateTag, "gbCmdOperateTag");
            this.gbCmdOperateTag.Name = "gbCmdOperateTag";
            this.gbCmdOperateTag.TabStop = false;
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.btnKillTag);
            this.groupBox16.Controls.Add(this.htxtKillPwd);
            this.groupBox16.Controls.Add(this.label29);
            resources.ApplyResources(this.groupBox16, "groupBox16");
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.TabStop = false;
            // 
            // btnKillTag
            // 
            resources.ApplyResources(this.btnKillTag, "btnKillTag");
            this.btnKillTag.Name = "btnKillTag";
            this.btnKillTag.UseVisualStyleBackColor = true;
            this.btnKillTag.Click += new System.EventHandler(this.btnKillTag_Click);
            // 
            // htxtKillPwd
            // 
            resources.ApplyResources(this.htxtKillPwd, "htxtKillPwd");
            this.htxtKillPwd.Name = "htxtKillPwd";
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.htxtLockPwd);
            this.groupBox15.Controls.Add(this.label28);
            this.groupBox15.Controls.Add(this.groupBox19);
            this.groupBox15.Controls.Add(this.groupBox18);
            this.groupBox15.Controls.Add(this.btnLockTag);
            resources.ApplyResources(this.groupBox15, "groupBox15");
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.TabStop = false;
            // 
            // htxtLockPwd
            // 
            resources.ApplyResources(this.htxtLockPwd, "htxtLockPwd");
            this.htxtLockPwd.Name = "htxtLockPwd";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.rdbUserMemory);
            this.groupBox19.Controls.Add(this.rdbTidMemory);
            this.groupBox19.Controls.Add(this.rdbEpcMermory);
            this.groupBox19.Controls.Add(this.rdbKillPwd);
            this.groupBox19.Controls.Add(this.rdbAccessPwd);
            resources.ApplyResources(this.groupBox19, "groupBox19");
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.TabStop = false;
            // 
            // rdbUserMemory
            // 
            resources.ApplyResources(this.rdbUserMemory, "rdbUserMemory");
            this.rdbUserMemory.Name = "rdbUserMemory";
            this.rdbUserMemory.TabStop = true;
            this.rdbUserMemory.UseVisualStyleBackColor = true;
            // 
            // rdbTidMemory
            // 
            resources.ApplyResources(this.rdbTidMemory, "rdbTidMemory");
            this.rdbTidMemory.Name = "rdbTidMemory";
            this.rdbTidMemory.TabStop = true;
            this.rdbTidMemory.UseVisualStyleBackColor = true;
            // 
            // rdbEpcMermory
            // 
            resources.ApplyResources(this.rdbEpcMermory, "rdbEpcMermory");
            this.rdbEpcMermory.Name = "rdbEpcMermory";
            this.rdbEpcMermory.TabStop = true;
            this.rdbEpcMermory.UseVisualStyleBackColor = true;
            // 
            // rdbKillPwd
            // 
            resources.ApplyResources(this.rdbKillPwd, "rdbKillPwd");
            this.rdbKillPwd.Name = "rdbKillPwd";
            this.rdbKillPwd.TabStop = true;
            this.rdbKillPwd.UseVisualStyleBackColor = true;
            // 
            // rdbAccessPwd
            // 
            resources.ApplyResources(this.rdbAccessPwd, "rdbAccessPwd");
            this.rdbAccessPwd.Name = "rdbAccessPwd";
            this.rdbAccessPwd.TabStop = true;
            this.rdbAccessPwd.UseVisualStyleBackColor = true;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.rdbLockEverR6);
            this.groupBox18.Controls.Add(this.rdbLockEver);
            this.groupBox18.Controls.Add(this.rdbFreeEver);
            this.groupBox18.Controls.Add(this.rdbLock);
            this.groupBox18.Controls.Add(this.rdbFree);
            resources.ApplyResources(this.groupBox18, "groupBox18");
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.TabStop = false;
            // 
            // rdbLockEverR6
            // 
            resources.ApplyResources(this.rdbLockEverR6, "rdbLockEverR6");
            this.rdbLockEverR6.Name = "rdbLockEverR6";
            this.rdbLockEverR6.TabStop = true;
            this.rdbLockEverR6.UseVisualStyleBackColor = true;
            // 
            // rdbLockEver
            // 
            resources.ApplyResources(this.rdbLockEver, "rdbLockEver");
            this.rdbLockEver.Name = "rdbLockEver";
            this.rdbLockEver.TabStop = true;
            this.rdbLockEver.UseVisualStyleBackColor = true;
            // 
            // rdbFreeEver
            // 
            resources.ApplyResources(this.rdbFreeEver, "rdbFreeEver");
            this.rdbFreeEver.Name = "rdbFreeEver";
            this.rdbFreeEver.TabStop = true;
            this.rdbFreeEver.UseVisualStyleBackColor = true;
            // 
            // rdbLock
            // 
            resources.ApplyResources(this.rdbLock, "rdbLock");
            this.rdbLock.Name = "rdbLock";
            this.rdbLock.TabStop = true;
            this.rdbLock.UseVisualStyleBackColor = true;
            // 
            // rdbFree
            // 
            resources.ApplyResources(this.rdbFree, "rdbFree");
            this.rdbFree.Name = "rdbFree";
            this.rdbFree.TabStop = true;
            this.rdbFree.UseVisualStyleBackColor = true;
            // 
            // btnLockTag
            // 
            resources.ApplyResources(this.btnLockTag, "btnLockTag");
            this.btnLockTag.Name = "btnLockTag";
            this.btnLockTag.UseVisualStyleBackColor = true;
            this.btnLockTag.Click += new System.EventHandler(this.btnLockTag_Click);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.labelHEXSize);
            this.groupBox14.Controls.Add(this.plainTextBox);
            this.groupBox14.Controls.Add(this.btEPC2Hex);
            this.groupBox14.Controls.Add(this.btDecrypt);
            this.groupBox14.Controls.Add(this.accessCodeTextBox);
            this.groupBox14.Controls.Add(this.accessCodeFileBrower);
            this.groupBox14.Controls.Add(this.label129);
            this.groupBox14.Controls.Add(this.plainEPCTextBox);
            this.groupBox14.Controls.Add(this.label128);
            this.groupBox14.Controls.Add(this.EncryptedToHEX);
            this.groupBox14.Controls.Add(this.keyFilePathTextBox);
            this.groupBox14.Controls.Add(this.keyFileBrower);
            this.groupBox14.Controls.Add(this.label127);
            this.groupBox14.Controls.Add(this.label126);
            this.groupBox14.Controls.Add(this.radioButtonBWrite);
            this.groupBox14.Controls.Add(this.radioButtonWrite);
            this.groupBox14.Controls.Add(this.htxtWriteData);
            this.groupBox14.Controls.Add(this.txtWordCnt);
            this.groupBox14.Controls.Add(this.label27);
            this.groupBox14.Controls.Add(this.btnWriteTag);
            this.groupBox14.Controls.Add(this.btnReadTag);
            this.groupBox14.Controls.Add(this.txtWordAddr);
            this.groupBox14.Controls.Add(this.label26);
            this.groupBox14.Controls.Add(this.htxtReadAndWritePwd);
            this.groupBox14.Controls.Add(this.label25);
            this.groupBox14.Controls.Add(this.groupBox17);
            this.groupBox14.Controls.Add(this.label24);
            resources.ApplyResources(this.groupBox14, "groupBox14");
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.TabStop = false;
            // 
            // labelHEXSize
            // 
            resources.ApplyResources(this.labelHEXSize, "labelHEXSize");
            this.labelHEXSize.Name = "labelHEXSize";
            // 
            // plainTextBox
            // 
            resources.ApplyResources(this.plainTextBox, "plainTextBox");
            this.plainTextBox.Name = "plainTextBox";
            // 
            // btEPC2Hex
            // 
            resources.ApplyResources(this.btEPC2Hex, "btEPC2Hex");
            this.btEPC2Hex.Name = "btEPC2Hex";
            this.btEPC2Hex.UseVisualStyleBackColor = true;
            this.btEPC2Hex.Click += new System.EventHandler(this.btEPC2Hex_Click);
            // 
            // btDecrypt
            // 
            resources.ApplyResources(this.btDecrypt, "btDecrypt");
            this.btDecrypt.Name = "btDecrypt";
            this.btDecrypt.UseVisualStyleBackColor = true;
            this.btDecrypt.Click += new System.EventHandler(this.btDecrypt_Click);
            // 
            // accessCodeTextBox
            // 
            resources.ApplyResources(this.accessCodeTextBox, "accessCodeTextBox");
            this.accessCodeTextBox.Name = "accessCodeTextBox";
            // 
            // accessCodeFileBrower
            // 
            resources.ApplyResources(this.accessCodeFileBrower, "accessCodeFileBrower");
            this.accessCodeFileBrower.Name = "accessCodeFileBrower";
            this.accessCodeFileBrower.UseVisualStyleBackColor = true;
            this.accessCodeFileBrower.Click += new System.EventHandler(this.accessCodeFileBrower_Click);
            // 
            // label129
            // 
            resources.ApplyResources(this.label129, "label129");
            this.label129.Name = "label129";
            // 
            // plainEPCTextBox
            // 
            resources.ApplyResources(this.plainEPCTextBox, "plainEPCTextBox");
            this.plainEPCTextBox.Name = "plainEPCTextBox";
            // 
            // label128
            // 
            resources.ApplyResources(this.label128, "label128");
            this.label128.Name = "label128";
            // 
            // EncryptedToHEX
            // 
            resources.ApplyResources(this.EncryptedToHEX, "EncryptedToHEX");
            this.EncryptedToHEX.Name = "EncryptedToHEX";
            this.EncryptedToHEX.UseVisualStyleBackColor = true;
            this.EncryptedToHEX.Click += new System.EventHandler(this.EncryptedToHEX_Click);
            // 
            // keyFilePathTextBox
            // 
            resources.ApplyResources(this.keyFilePathTextBox, "keyFilePathTextBox");
            this.keyFilePathTextBox.Name = "keyFilePathTextBox";
            // 
            // keyFileBrower
            // 
            resources.ApplyResources(this.keyFileBrower, "keyFileBrower");
            this.keyFileBrower.Name = "keyFileBrower";
            this.keyFileBrower.UseVisualStyleBackColor = true;
            this.keyFileBrower.Click += new System.EventHandler(this.keyFileBrower_Click);
            // 
            // label127
            // 
            resources.ApplyResources(this.label127, "label127");
            this.label127.Name = "label127";
            // 
            // label126
            // 
            resources.ApplyResources(this.label126, "label126");
            this.label126.Name = "label126";
            // 
            // radioButtonBWrite
            // 
            resources.ApplyResources(this.radioButtonBWrite, "radioButtonBWrite");
            this.radioButtonBWrite.Name = "radioButtonBWrite";
            this.radioButtonBWrite.UseVisualStyleBackColor = true;
            // 
            // radioButtonWrite
            // 
            resources.ApplyResources(this.radioButtonWrite, "radioButtonWrite");
            this.radioButtonWrite.Checked = true;
            this.radioButtonWrite.Name = "radioButtonWrite";
            this.radioButtonWrite.TabStop = true;
            this.radioButtonWrite.UseVisualStyleBackColor = true;
            // 
            // htxtWriteData
            // 
            resources.ApplyResources(this.htxtWriteData, "htxtWriteData");
            this.htxtWriteData.Name = "htxtWriteData";
            // 
            // txtWordCnt
            // 
            resources.ApplyResources(this.txtWordCnt, "txtWordCnt");
            this.txtWordCnt.Name = "txtWordCnt";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // btnWriteTag
            // 
            resources.ApplyResources(this.btnWriteTag, "btnWriteTag");
            this.btnWriteTag.Name = "btnWriteTag";
            this.btnWriteTag.UseVisualStyleBackColor = true;
            this.btnWriteTag.Click += new System.EventHandler(this.btnWriteTag_Click);
            // 
            // btnReadTag
            // 
            resources.ApplyResources(this.btnReadTag, "btnReadTag");
            this.btnReadTag.Name = "btnReadTag";
            this.btnReadTag.UseVisualStyleBackColor = true;
            this.btnReadTag.Click += new System.EventHandler(this.btnReadTag_Click);
            // 
            // txtWordAddr
            // 
            resources.ApplyResources(this.txtWordAddr, "txtWordAddr");
            this.txtWordAddr.Name = "txtWordAddr";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // htxtReadAndWritePwd
            // 
            resources.ApplyResources(this.htxtReadAndWritePwd, "htxtReadAndWritePwd");
            this.htxtReadAndWritePwd.Name = "htxtReadAndWritePwd";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.rdbUser);
            this.groupBox17.Controls.Add(this.rdbTid);
            this.groupBox17.Controls.Add(this.rdbEpc);
            this.groupBox17.Controls.Add(this.rdbReserved);
            resources.ApplyResources(this.groupBox17, "groupBox17");
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.TabStop = false;
            // 
            // rdbUser
            // 
            resources.ApplyResources(this.rdbUser, "rdbUser");
            this.rdbUser.Name = "rdbUser";
            this.rdbUser.TabStop = true;
            this.rdbUser.UseVisualStyleBackColor = true;
            this.rdbUser.CheckedChanged += new System.EventHandler(this.rdbUser_CheckedChanged);
            // 
            // rdbTid
            // 
            resources.ApplyResources(this.rdbTid, "rdbTid");
            this.rdbTid.Name = "rdbTid";
            this.rdbTid.TabStop = true;
            this.rdbTid.UseVisualStyleBackColor = true;
            // 
            // rdbEpc
            // 
            resources.ApplyResources(this.rdbEpc, "rdbEpc");
            this.rdbEpc.Name = "rdbEpc";
            this.rdbEpc.TabStop = true;
            this.rdbEpc.UseVisualStyleBackColor = true;
            this.rdbEpc.CheckedChanged += new System.EventHandler(this.rdbEpc_CheckedChanged);
            // 
            // rdbReserved
            // 
            resources.ApplyResources(this.rdbReserved, "rdbReserved");
            this.rdbReserved.Name = "rdbReserved";
            this.rdbReserved.TabStop = true;
            this.rdbReserved.UseVisualStyleBackColor = true;
            this.rdbReserved.CheckedChanged += new System.EventHandler(this.rdbReserved_CheckedChanged);
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label23);
            this.groupBox13.Controls.Add(this.btnSetAccessEpcMatch);
            this.groupBox13.Controls.Add(this.cmbSetAccessEpcMatch);
            this.groupBox13.Controls.Add(this.txtAccessEpcMatch);
            this.groupBox13.Controls.Add(this.ckAccessEpcMatch);
            resources.ApplyResources(this.groupBox13, "groupBox13");
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.TabStop = false;
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // btnSetAccessEpcMatch
            // 
            resources.ApplyResources(this.btnSetAccessEpcMatch, "btnSetAccessEpcMatch");
            this.btnSetAccessEpcMatch.Name = "btnSetAccessEpcMatch";
            this.btnSetAccessEpcMatch.UseVisualStyleBackColor = true;
            this.btnSetAccessEpcMatch.Click += new System.EventHandler(this.btnSetAccessEpcMatch_Click);
            // 
            // cmbSetAccessEpcMatch
            // 
            this.cmbSetAccessEpcMatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSetAccessEpcMatch.FormattingEnabled = true;
            resources.ApplyResources(this.cmbSetAccessEpcMatch, "cmbSetAccessEpcMatch");
            this.cmbSetAccessEpcMatch.Name = "cmbSetAccessEpcMatch";
            this.cmbSetAccessEpcMatch.DropDown += new System.EventHandler(this.cmbSetAccessEpcMatch_DropDown);
            this.cmbSetAccessEpcMatch.SelectedIndexChanged += new System.EventHandler(this.cmbSetAccessEpcMatch_SelectedIndexChanged);
            // 
            // txtAccessEpcMatch
            // 
            resources.ApplyResources(this.txtAccessEpcMatch, "txtAccessEpcMatch");
            this.txtAccessEpcMatch.Name = "txtAccessEpcMatch";
            this.txtAccessEpcMatch.ReadOnly = true;
            // 
            // ckAccessEpcMatch
            // 
            resources.ApplyResources(this.ckAccessEpcMatch, "ckAccessEpcMatch");
            this.ckAccessEpcMatch.Name = "ckAccessEpcMatch";
            this.ckAccessEpcMatch.UseVisualStyleBackColor = true;
            this.ckAccessEpcMatch.CheckedChanged += new System.EventHandler(this.cbAccessEpcMatch_CheckedChanged);
            // 
            // label35
            // 
            resources.ApplyResources(this.label35, "label35");
            this.label35.Name = "label35";
            // 
            // ckDisplayLog
            // 
            resources.ApplyResources(this.ckDisplayLog, "ckDisplayLog");
            this.ckDisplayLog.ForeColor = System.Drawing.Color.Indigo;
            this.ckDisplayLog.Name = "ckDisplayLog";
            this.ckDisplayLog.UseVisualStyleBackColor = true;
            this.ckDisplayLog.CheckedChanged += new System.EventHandler(this.ckDisplayLog_CheckedChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.panel6, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel7, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button4);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // button4
            // 
            this.button4.ForeColor = System.Drawing.SystemColors.Desktop;
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.checkBox5);
            this.panel7.Controls.Add(this.checkBox6);
            this.panel7.Controls.Add(this.checkBox7);
            this.panel7.Controls.Add(this.checkBox8);
            resources.ApplyResources(this.panel7, "panel7");
            this.panel7.Name = "panel7";
            // 
            // checkBox5
            // 
            resources.ApplyResources(this.checkBox5, "checkBox5");
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            resources.ApplyResources(this.checkBox6, "checkBox6");
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            resources.ApplyResources(this.checkBox7, "checkBox7");
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            resources.ApplyResources(this.checkBox8, "checkBox8");
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            resources.ApplyResources(this.textBox5, "textBox5");
            this.textBox5.Name = "textBox5";
            // 
            // textBox6
            // 
            resources.ApplyResources(this.textBox6, "textBox6");
            this.textBox6.Name = "textBox6";
            // 
            // button5
            // 
            this.button5.ForeColor = System.Drawing.SystemColors.Desktop;
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label76
            // 
            resources.ApplyResources(this.label76, "label76");
            this.label76.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label76.Name = "label76";
            // 
            // label77
            // 
            resources.ApplyResources(this.label77, "label77");
            this.label77.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label77.Name = "label77";
            // 
            // label78
            // 
            resources.ApplyResources(this.label78, "label78");
            this.label78.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label78.Name = "label78";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.comboBox9);
            this.groupBox8.Controls.Add(this.lxLedControl9);
            this.groupBox8.Controls.Add(this.lxLedControl10);
            this.groupBox8.Controls.Add(this.lxLedControl11);
            this.groupBox8.Controls.Add(this.lxLedControl12);
            this.groupBox8.Controls.Add(this.label79);
            this.groupBox8.Controls.Add(this.label80);
            this.groupBox8.Controls.Add(this.label81);
            this.groupBox8.Controls.Add(this.label82);
            this.groupBox8.Controls.Add(this.label83);
            this.groupBox8.Controls.Add(this.lxLedControl13);
            this.groupBox8.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // comboBox9
            // 
            this.comboBox9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox9.ForeColor = System.Drawing.SystemColors.InfoText;
            this.comboBox9.FormattingEnabled = true;
            this.comboBox9.Items.AddRange(new object[] {
            resources.GetString("comboBox9.Items"),
            resources.GetString("comboBox9.Items1"),
            resources.GetString("comboBox9.Items2"),
            resources.GetString("comboBox9.Items3"),
            resources.GetString("comboBox9.Items4")});
            resources.ApplyResources(this.comboBox9, "comboBox9");
            this.comboBox9.Name = "comboBox9";
            // 
            // lxLedControl9
            // 
            this.lxLedControl9.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl9.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl9.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl9.BevelRate = 0.1F;
            this.lxLedControl9.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl9.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl9.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl9.ForeColor = System.Drawing.Color.Black;
            this.lxLedControl9.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl9, "lxLedControl9");
            this.lxLedControl9.Name = "lxLedControl9";
            this.lxLedControl9.RoundCorner = true;
            this.lxLedControl9.SegmentIntervalRatio = 50;
            this.lxLedControl9.ShowHighlight = true;
            this.lxLedControl9.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            this.lxLedControl9.TotalCharCount = 10;
            // 
            // lxLedControl10
            // 
            this.lxLedControl10.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl10.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl10.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl10.BevelRate = 0.1F;
            this.lxLedControl10.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl10.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl10.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl10.ForeColor = System.Drawing.Color.Black;
            this.lxLedControl10.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl10, "lxLedControl10");
            this.lxLedControl10.Name = "lxLedControl10";
            this.lxLedControl10.RoundCorner = true;
            this.lxLedControl10.SegmentIntervalRatio = 50;
            this.lxLedControl10.ShowHighlight = true;
            this.lxLedControl10.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            // 
            // lxLedControl11
            // 
            this.lxLedControl11.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl11.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl11.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl11.BevelRate = 0.1F;
            this.lxLedControl11.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl11.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl11.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl11.ForeColor = System.Drawing.Color.Black;
            this.lxLedControl11.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl11, "lxLedControl11");
            this.lxLedControl11.Name = "lxLedControl11";
            this.lxLedControl11.RoundCorner = true;
            this.lxLedControl11.SegmentIntervalRatio = 50;
            this.lxLedControl11.ShowHighlight = true;
            this.lxLedControl11.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            // 
            // lxLedControl12
            // 
            this.lxLedControl12.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl12.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl12.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl12.BevelRate = 0.1F;
            this.lxLedControl12.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl12.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl12.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl12.ForeColor = System.Drawing.Color.Black;
            this.lxLedControl12.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl12, "lxLedControl12");
            this.lxLedControl12.Name = "lxLedControl12";
            this.lxLedControl12.RoundCorner = true;
            this.lxLedControl12.SegmentIntervalRatio = 50;
            this.lxLedControl12.ShowHighlight = true;
            this.lxLedControl12.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            // 
            // label79
            // 
            resources.ApplyResources(this.label79, "label79");
            this.label79.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label79.Name = "label79";
            // 
            // label80
            // 
            resources.ApplyResources(this.label80, "label80");
            this.label80.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label80.Name = "label80";
            // 
            // label81
            // 
            resources.ApplyResources(this.label81, "label81");
            this.label81.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label81.Name = "label81";
            // 
            // label82
            // 
            resources.ApplyResources(this.label82, "label82");
            this.label82.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label82.Name = "label82";
            // 
            // label83
            // 
            resources.ApplyResources(this.label83, "label83");
            this.label83.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label83.Name = "label83";
            // 
            // lxLedControl13
            // 
            this.lxLedControl13.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl13.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl13.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl13.BevelRate = 0.1F;
            this.lxLedControl13.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl13.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl13.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl13.ForeColor = System.Drawing.Color.Purple;
            this.lxLedControl13.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl13, "lxLedControl13");
            this.lxLedControl13.Name = "lxLedControl13";
            this.lxLedControl13.RoundCorner = true;
            this.lxLedControl13.SegmentIntervalRatio = 50;
            this.lxLedControl13.ShowHighlight = true;
            this.lxLedControl13.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader43,
            this.columnHeader44,
            this.columnHeader45,
            this.columnHeader46,
            this.columnHeader47,
            this.columnHeader48});
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.HideSelection = false;
            this.listView1.Name = "listView1";
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader43
            // 
            resources.ApplyResources(this.columnHeader43, "columnHeader43");
            // 
            // columnHeader44
            // 
            resources.ApplyResources(this.columnHeader44, "columnHeader44");
            // 
            // columnHeader45
            // 
            resources.ApplyResources(this.columnHeader45, "columnHeader45");
            // 
            // columnHeader46
            // 
            resources.ApplyResources(this.columnHeader46, "columnHeader46");
            // 
            // columnHeader47
            // 
            resources.ApplyResources(this.columnHeader47, "columnHeader47");
            // 
            // columnHeader48
            // 
            resources.ApplyResources(this.columnHeader48, "columnHeader48");
            // 
            // comboBox10
            // 
            this.comboBox10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox10.ForeColor = System.Drawing.SystemColors.InfoText;
            this.comboBox10.FormattingEnabled = true;
            this.comboBox10.Items.AddRange(new object[] {
            resources.GetString("comboBox10.Items"),
            resources.GetString("comboBox10.Items1"),
            resources.GetString("comboBox10.Items2"),
            resources.GetString("comboBox10.Items3"),
            resources.GetString("comboBox10.Items4")});
            resources.ApplyResources(this.comboBox10, "comboBox10");
            this.comboBox10.Name = "comboBox10";
            // 
            // label87
            // 
            resources.ApplyResources(this.label87, "label87");
            this.label87.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label87.Name = "label87";
            // 
            // label88
            // 
            resources.ApplyResources(this.label88, "label88");
            this.label88.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label88.Name = "label88";
            // 
            // label89
            // 
            resources.ApplyResources(this.label89, "label89");
            this.label89.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label89.Name = "label89";
            // 
            // label90
            // 
            resources.ApplyResources(this.label90, "label90");
            this.label90.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label90.Name = "label90";
            // 
            // label91
            // 
            resources.ApplyResources(this.label91, "label91");
            this.label91.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label91.Name = "label91";
            // 
            // ckClearOperationRec
            // 
            resources.ApplyResources(this.ckClearOperationRec, "ckClearOperationRec");
            this.ckClearOperationRec.Checked = true;
            this.ckClearOperationRec.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckClearOperationRec.Name = "ckClearOperationRec";
            this.ckClearOperationRec.UseVisualStyleBackColor = true;
            // 
            // timerInventory
            // 
            this.timerInventory.Interval = 500;
            this.timerInventory.Tick += new System.EventHandler(this.timerInventory_Tick);
            // 
            // totalTime
            // 
            this.totalTime.Interval = 50;
            this.totalTime.Tick += new System.EventHandler(this.totalTimeDisplay);
            // 
            // sortImageList
            // 
            this.sortImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("sortImageList.ImageStream")));
            this.sortImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.sortImageList.Images.SetKeyName(0, "sort.png");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "\"dat\"";
            this.openFileDialog1.FileName = "openFileDialog1";
            resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
            this.openFileDialog1.InitialDirectory = "@\"C:\\\"";
            // 
            // lrtxtLog
            // 
            resources.ApplyResources(this.lrtxtLog, "lrtxtLog");
            this.lrtxtLog.Name = "lrtxtLog";
            this.lrtxtLog.DoubleClick += new System.EventHandler(this.lrtxtLog_DoubleClick);
            // 
            // lxLedControl14
            // 
            this.lxLedControl14.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl14.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl14.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl14.BevelRate = 0.1F;
            this.lxLedControl14.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl14.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl14.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl14.ForeColor = System.Drawing.Color.Black;
            this.lxLedControl14.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl14, "lxLedControl14");
            this.lxLedControl14.Name = "lxLedControl14";
            this.lxLedControl14.RoundCorner = true;
            this.lxLedControl14.SegmentIntervalRatio = 50;
            this.lxLedControl14.ShowHighlight = true;
            this.lxLedControl14.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            this.lxLedControl14.TotalCharCount = 10;
            // 
            // lxLedControl15
            // 
            this.lxLedControl15.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl15.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl15.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl15.BevelRate = 0.1F;
            this.lxLedControl15.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl15.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl15.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl15.ForeColor = System.Drawing.Color.Black;
            this.lxLedControl15.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl15, "lxLedControl15");
            this.lxLedControl15.Name = "lxLedControl15";
            this.lxLedControl15.RoundCorner = true;
            this.lxLedControl15.SegmentIntervalRatio = 50;
            this.lxLedControl15.ShowHighlight = true;
            this.lxLedControl15.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            // 
            // lxLedControl16
            // 
            this.lxLedControl16.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl16.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl16.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl16.BevelRate = 0.1F;
            this.lxLedControl16.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl16.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl16.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl16.ForeColor = System.Drawing.Color.Black;
            this.lxLedControl16.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl16, "lxLedControl16");
            this.lxLedControl16.Name = "lxLedControl16";
            this.lxLedControl16.RoundCorner = true;
            this.lxLedControl16.SegmentIntervalRatio = 50;
            this.lxLedControl16.ShowHighlight = true;
            this.lxLedControl16.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            // 
            // lxLedControl17
            // 
            this.lxLedControl17.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl17.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl17.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl17.BevelRate = 0.1F;
            this.lxLedControl17.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl17.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl17.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl17.ForeColor = System.Drawing.Color.Black;
            this.lxLedControl17.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl17, "lxLedControl17");
            this.lxLedControl17.Name = "lxLedControl17";
            this.lxLedControl17.RoundCorner = true;
            this.lxLedControl17.SegmentIntervalRatio = 50;
            this.lxLedControl17.ShowHighlight = true;
            this.lxLedControl17.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            // 
            // lxLedControl18
            // 
            this.lxLedControl18.BackColor = System.Drawing.Color.Transparent;
            this.lxLedControl18.BackColor_1 = System.Drawing.Color.Transparent;
            this.lxLedControl18.BackColor_2 = System.Drawing.Color.DarkRed;
            this.lxLedControl18.BevelRate = 0.1F;
            this.lxLedControl18.BorderColor = System.Drawing.Color.Lavender;
            this.lxLedControl18.FadedColor = System.Drawing.SystemColors.ControlLight;
            this.lxLedControl18.FocusedBorderColor = System.Drawing.Color.LightCoral;
            this.lxLedControl18.ForeColor = System.Drawing.Color.Purple;
            this.lxLedControl18.HighlightOpaque = ((byte)(20));
            resources.ApplyResources(this.lxLedControl18, "lxLedControl18");
            this.lxLedControl18.Name = "lxLedControl18";
            this.lxLedControl18.RoundCorner = true;
            this.lxLedControl18.SegmentIntervalRatio = 50;
            this.lxLedControl18.ShowHighlight = true;
            this.lxLedControl18.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            // 
            // R2000UartDemo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.ckClearOperationRec);
            this.Controls.Add(this.ckDisplayLog);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.lrtxtLog);
            this.Controls.Add(this.tabCtrMain);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "R2000UartDemo";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.R2000UartDemo_Load);
            this.tabCtrMain.ResumeLayout(false);
            this.PagReaderSetting.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            this.gbCmdReadGpio.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.gbCmdBeeper.ResumeLayout(false);
            this.gbCmdBeeper.PerformLayout();
            this.gbCmdTemperature.ResumeLayout(false);
            this.gbCmdTemperature.PerformLayout();
            this.gbCmdVersion.ResumeLayout(false);
            this.gbCmdVersion.PerformLayout();
            this.gbCmdBaudrate.ResumeLayout(false);
            this.gbCmdBaudrate.PerformLayout();
            this.gbCmdReaderAddress.ResumeLayout(false);
            this.gbCmdReaderAddress.PerformLayout();
            this.gbTcpIp.ResumeLayout(false);
            this.gbTcpIp.PerformLayout();
            this.gbRS232.ResumeLayout(false);
            this.gbRS232.PerformLayout();
            this.gbConnectType.ResumeLayout(false);
            this.gbConnectType.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.gbReturnLoss.ResumeLayout(false);
            this.gbReturnLoss.PerformLayout();
            this.gbCmdAntenna.ResumeLayout(false);
            this.gbCmdAntenna.PerformLayout();
            this.gbCmdRegion.ResumeLayout(false);
            this.gbCmdRegion.PerformLayout();
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.gbCmdOutputPower.ResumeLayout(false);
            this.gbCmdOutputPower.PerformLayout();
            this.pageEpcTest.ResumeLayout(false);
            this.tabEpcTest.ResumeLayout(false);
            this.pageRealMode.ResumeLayout(false);
            this.pageRealMode.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledReal1)).EndInit();
            this.pageAcessTag.ResumeLayout(false);
            this.gbCmdOperateTag.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl18)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabCtrMain;
        private System.Windows.Forms.TabPage PagReaderSetting;
        private CustomControl.LogRichTextBox lrtxtLog;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.CheckBox ckDisplayLog;
        private System.Windows.Forms.TabPage pageEpcTest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.ComboBox comboBox9;
        private LxControl.LxLedControl lxLedControl9;
        private LxControl.LxLedControl lxLedControl10;
        private LxControl.LxLedControl lxLedControl11;
        private LxControl.LxLedControl lxLedControl12;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label83;
        private LxControl.LxLedControl lxLedControl13;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader43;
        private System.Windows.Forms.ColumnHeader columnHeader44;
        private System.Windows.Forms.ColumnHeader columnHeader45;
        private System.Windows.Forms.ColumnHeader columnHeader46;
        private System.Windows.Forms.ColumnHeader columnHeader47;
        private System.Windows.Forms.ColumnHeader columnHeader48;
        private System.Windows.Forms.ComboBox comboBox10;
        private LxControl.LxLedControl lxLedControl14;
        private LxControl.LxLedControl lxLedControl15;
        private LxControl.LxLedControl lxLedControl16;
        private LxControl.LxLedControl lxLedControl17;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.Label label91;
        private LxControl.LxLedControl lxLedControl18;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox gbCmdReadGpio;
        private System.Windows.Forms.Button btnWriteGpio4Value;
        private System.Windows.Forms.Button btnWriteGpio3Value;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.RadioButton rdbGpio4High;
        private System.Windows.Forms.RadioButton rdbGpio4Low;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.RadioButton rdbGpio3High;
        private System.Windows.Forms.RadioButton rdbGpio3Low;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.RadioButton rdbGpio2High;
        private System.Windows.Forms.RadioButton rdbGpio2Low;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.RadioButton rdbGpio1High;
        private System.Windows.Forms.RadioButton rdbGpio1Low;
        private System.Windows.Forms.Button btnReadGpioValue;
        private System.Windows.Forms.GroupBox gbCmdBeeper;
        private System.Windows.Forms.Button btnSetBeeperMode;
        private System.Windows.Forms.RadioButton rdbBeeperModeTag;
        private System.Windows.Forms.RadioButton rdbBeeperModeInventory;
        private System.Windows.Forms.RadioButton rdbBeeperModeSlient;
        private System.Windows.Forms.GroupBox gbCmdTemperature;
        private System.Windows.Forms.Button btnGetReaderTemperature;
        private System.Windows.Forms.TextBox txtReaderTemperature;
        private System.Windows.Forms.GroupBox gbCmdVersion;
        private System.Windows.Forms.Button btnGetFirmwareVersion;
        private System.Windows.Forms.TextBox txtFirmwareVersion;
        private System.Windows.Forms.Button btnResetReader;
        public System.Windows.Forms.GroupBox gbCmdBaudrate;
        private CustomControl.HexTextBox htbGetIdentifier;
        private CustomControl.HexTextBox htbSetIdentifier;
        private System.Windows.Forms.Button btSetIdentifier;
        private System.Windows.Forms.Button btGetIdentifier;
        private System.Windows.Forms.GroupBox gbCmdReaderAddress;
        private CustomControl.HexTextBox htxtReadId;
        private System.Windows.Forms.Button btnSetReadAddress;
        private System.Windows.Forms.GroupBox gbTcpIp;
        private System.Windows.Forms.Button btnDisconnectTcp;
        private System.Windows.Forms.TextBox txtTcpPort;
        private System.Windows.Forms.Button btnConnectTcp;
        private CustomControl.IpAddressTextBox ipIpServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbRS232;
        private System.Windows.Forms.Button btnSetUartBaudrate;
        private System.Windows.Forms.Button btnDisconnectRs232;
        private System.Windows.Forms.ComboBox cmbSetBaudrate;
        private System.Windows.Forms.Label lbChangeBaudrate;
        private System.Windows.Forms.Button btnConnectRs232;
        private System.Windows.Forms.ComboBox cmbBaudrate;
        private System.Windows.Forms.ComboBox cmbComPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbConnectType;
        private System.Windows.Forms.RadioButton rdbTcpIp;
        private System.Windows.Forms.RadioButton rdbRS232;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox gbCmdAntenna;
        private System.Windows.Forms.Button btnGetWorkAntenna;
        private System.Windows.Forms.Button btnSetWorkAntenna;
        private System.Windows.Forms.GroupBox gbCmdRegion;
        private System.Windows.Forms.Button btnGetFrequencyRegion;
        private System.Windows.Forms.Button btnSetFrequencyRegion;
        private System.Windows.Forms.GroupBox gbCmdOutputPower;
        private System.Windows.Forms.Button btnGetOutputPower;
        private System.Windows.Forms.Button btnSetOutputPower;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btReaderSetupRefresh;
        private System.Windows.Forms.Button btRfSetup;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.TextBox textFreqQuantity;
        private System.Windows.Forms.TextBox TextFreqInterval;
        private System.Windows.Forms.TextBox textStartFreq;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.ComboBox cmbFrequencyEnd;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbFrequencyStart;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton rdbRegionChn;
        private System.Windows.Forms.RadioButton rdbRegionEtsi;
        private System.Windows.Forms.RadioButton rdbRegionFcc;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.Label label105;
        private System.Windows.Forms.Label label104;
        private System.Windows.Forms.Label label103;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.GroupBox gbReturnLoss;
        private System.Windows.Forms.Label label108;
        private System.Windows.Forms.TextBox textReturnLoss;
        private System.Windows.Forms.Button btReturnLoss;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.ComboBox cmbWorkAnt;
        private System.Windows.Forms.Label label110;
        private System.Windows.Forms.Label label109;
        private System.Windows.Forms.ComboBox cmbReturnLossFreq;
        private System.Windows.Forms.CheckBox ckClearOperationRec;
        private System.Windows.Forms.CheckBox cbUserDefineFreq;
        private System.Windows.Forms.Timer timerInventory;

        private System.Windows.Forms.Timer totalTime;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.RadioButton antType1;
        private ImageList sortImageList;
        private TabControl tabEpcTest;
        private TabPage pageRealMode;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel5;
        private Label label125;
        private ComboBox m_real_phase_value;
        private Button btRealTimeInventory;
        private Label label84;
        private TextBox textRealRound;
        private RadioButton sessionInventoryrb;
        private RadioButton autoInventoryrb;
        private CheckBox m_session_q_cb;
        private CheckBox m_session_sl_cb;
        private ComboBox m_session_sl;
        private Label m_sl_content;
        private ComboBox cmbTarget;
        private Label label98;
        private ComboBox cmbSession;
        private Label label97;
        private RadioButton txt_format_rb;
        private Button btnSaveFile;
        private GroupBox groupBox20;
        private CheckBox cbRealWorkant1;
        private Label label19;
        private TextBox tbRealMinRssi;
        private TextBox tbRealMaxRssi;
        private Button btRealFresh;
        private Label label70;
        private Label label74;
        private Label lbRealTagCount;
        private GroupBox groupBox1;
        private LxControl.LxLedControl ledReal3;
        private ComboBox comboBox6;
        private LxControl.LxLedControl ledReal5;
        private LxControl.LxLedControl ledReal2;
        private LxControl.LxLedControl ledReal4;
        private Label label53;
        private Label label66;
        private Label label67;
        private Label label68;
        private Label label69;
        private LxControl.LxLedControl ledReal1;
        private ListView lvRealList;
        private ColumnHeader columnHeader37;
        private ColumnHeader columnHeader38;
        private ColumnHeader columnHeader39;
        private ColumnHeader columnHeader40;
        private ColumnHeader columnHeader41;
        private ColumnHeader columnHeader412;
        private ColumnHeader columnHeader42;
        private TabPage pageAcessTag;
        private ListView ltvOperate;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private ColumnHeader columnHeader13;
        private ColumnHeader columnHeader14;
        private ColumnHeader columnHeader15;
        private GroupBox gbCmdOperateTag;
        private GroupBox groupBox16;
        private Button btnKillTag;
        private CustomControl.HexTextBox htxtKillPwd;
        private Label label29;
        private GroupBox groupBox15;
        private CustomControl.HexTextBox htxtLockPwd;
        private Label label28;
        private GroupBox groupBox19;
        private RadioButton rdbUserMemory;
        private RadioButton rdbTidMemory;
        private RadioButton rdbEpcMermory;
        private RadioButton rdbKillPwd;
        private RadioButton rdbAccessPwd;
        private GroupBox groupBox18;
        private RadioButton rdbLockEver;
        private RadioButton rdbFreeEver;
        private RadioButton rdbLock;
        private RadioButton rdbFree;
        private Button btnLockTag;
        private GroupBox groupBox14;
        private RadioButton radioButtonBWrite;
        private RadioButton radioButtonWrite;
        private CustomControl.HexTextBox htxtWriteData;
        private TextBox txtWordCnt;
        private Label label27;
        private Button btnWriteTag;
        private Button btnReadTag;
        private TextBox txtWordAddr;
        private Label label26;
        private CustomControl.HexTextBox htxtReadAndWritePwd;
        private Label label25;
        private GroupBox groupBox17;
        private RadioButton rdbUser;
        private RadioButton rdbTid;
        private RadioButton rdbEpc;
        private RadioButton rdbReserved;
        private Label label24;
        private GroupBox groupBox13;
        private Label label23;
        private ComboBox cmbSetAccessEpcMatch;
        private TextBox txtAccessEpcMatch;
        private CheckBox ckAccessEpcMatch;
        private RadioButton rdbLockEverR6;
        private Button keyFileBrower;
        private Label label127;
        private Label label126;
        private OpenFileDialog openFileDialog1;
        private Button EncryptedToHEX;
        private Label label128;
        private CustomControl.HexTextBox keyFilePathTextBox;
        private TextBox plainEPCTextBox;
        private CustomControl.HexTextBox accessCodeTextBox;
        private Button accessCodeFileBrower;
        private Label label129;
        private Button btDecrypt;
        private Button btEPC2Hex;
        private TextBox plainTextBox;
        private Label labelHEXSize;
        private Button btnSetAccessEpcMatch;
    }
}

