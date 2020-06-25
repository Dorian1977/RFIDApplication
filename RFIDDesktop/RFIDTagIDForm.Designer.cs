using System.Windows.Forms;
namespace RFIDApplication
{
    partial class RFIDTagIDForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RFIDTagIDForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.checkBoxShowDetail = new System.Windows.Forms.CheckBox();
            this.ckClearOperationRec = new System.Windows.Forms.CheckBox();
            this.lrtxtLog = new CustomControl.LogRichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listViewEPCTag = new System.Windows.Forms.ListView();
            this.RFIDTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Counts = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabCtrMain = new System.Windows.Forms.TabControl();
            this.pageEpcID = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelID = new System.Windows.Forms.TableLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tbEPCTagDetected = new System.Windows.Forms.TextBox();
            this.tBAccessCodeVerify = new System.Windows.Forms.TextBox();
            this.btShowDebug1 = new System.Windows.Forms.Button();
            this.pictureBoxID = new System.Windows.Forms.PictureBox();
            this.pageData = new System.Windows.Forms.TabPage();
            this.btShowDebug = new System.Windows.Forms.Button();
            this.tableLayoutPanelDataTop = new System.Windows.Forms.TableLayoutPanel();
            this.labelPID = new System.Windows.Forms.Label();
            this.richTextBoxProductID = new System.Windows.Forms.RichTextBox();
            this.labelWOrder = new System.Windows.Forms.Label();
            this.richtbWorkOrderInfo = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanelData = new System.Windows.Forms.TableLayoutPanel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tbDataVerify = new System.Windows.Forms.TextBox();
            this.tbOdooStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBoxData = new System.Windows.Forms.PictureBox();
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
            this.timerInventory = new System.Windows.Forms.Timer(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btHelp = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.tbLoginID = new System.Windows.Forms.TextBox();
            this.lxLedControl14 = new LxControl.LxLedControl();
            this.lxLedControl15 = new LxControl.LxLedControl();
            this.lxLedControl16 = new LxControl.LxLedControl();
            this.lxLedControl17 = new LxControl.LxLedControl();
            this.lxLedControl18 = new LxControl.LxLedControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabCtrMain.SuspendLayout();
            this.pageEpcID.SuspendLayout();
            this.tableLayoutPanelID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxID)).BeginInit();
            this.pageData.SuspendLayout();
            this.tableLayoutPanelDataTop.SuspendLayout();
            this.tableLayoutPanelData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxData)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl18)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            // 
            // splitContainer3
            // 
            resources.ApplyResources(this.splitContainer3, "splitContainer3");
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.checkBoxShowDetail);
            this.splitContainer3.Panel1.Controls.Add(this.ckClearOperationRec);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.lrtxtLog);
            // 
            // checkBoxShowDetail
            // 
            resources.ApplyResources(this.checkBoxShowDetail, "checkBoxShowDetail");
            this.checkBoxShowDetail.ForeColor = System.Drawing.Color.White;
            this.checkBoxShowDetail.Name = "checkBoxShowDetail";
            this.checkBoxShowDetail.UseVisualStyleBackColor = true;
            this.checkBoxShowDetail.CheckedChanged += new System.EventHandler(this.checkBoxShowDetail_CheckedChanged);
            // 
            // ckClearOperationRec
            // 
            resources.ApplyResources(this.ckClearOperationRec, "ckClearOperationRec");
            this.ckClearOperationRec.Checked = true;
            this.ckClearOperationRec.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckClearOperationRec.ForeColor = System.Drawing.Color.White;
            this.ckClearOperationRec.Name = "ckClearOperationRec";
            this.ckClearOperationRec.UseVisualStyleBackColor = true;
            // 
            // lrtxtLog
            // 
            this.lrtxtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            this.lrtxtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.lrtxtLog, "lrtxtLog");
            this.lrtxtLog.ForeColor = System.Drawing.Color.White;
            this.lrtxtLog.Name = "lrtxtLog";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.listViewEPCTag);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // listViewEPCTag
            // 
            this.listViewEPCTag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            this.listViewEPCTag.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewEPCTag.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RFIDTag,
            this.Counts});
            resources.ApplyResources(this.listViewEPCTag, "listViewEPCTag");
            this.listViewEPCTag.ForeColor = System.Drawing.Color.White;
            this.listViewEPCTag.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewEPCTag.MultiSelect = false;
            this.listViewEPCTag.Name = "listViewEPCTag";
            this.listViewEPCTag.OwnerDraw = true;
            this.listViewEPCTag.Scrollable = false;
            this.listViewEPCTag.UseCompatibleStateImageBehavior = false;
            this.listViewEPCTag.View = System.Windows.Forms.View.Details;
            this.listViewEPCTag.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewEPCTag_DrawColumnHeader);
            this.listViewEPCTag.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewEPCTag_DrawItem);
            this.listViewEPCTag.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listViewEPCTag_DrawSubItem);
            this.listViewEPCTag.SelectedIndexChanged += new System.EventHandler(this.listViewEPCTag_SelectedIndexChanged);
            // 
            // RFIDTag
            // 
            resources.ApplyResources(this.RFIDTag, "RFIDTag");
            // 
            // Counts
            // 
            resources.ApplyResources(this.Counts, "Counts");
            // 
            // tabCtrMain
            // 
            this.tabCtrMain.Controls.Add(this.pageEpcID);
            this.tabCtrMain.Controls.Add(this.pageData);
            resources.ApplyResources(this.tabCtrMain, "tabCtrMain");
            this.tabCtrMain.Name = "tabCtrMain";
            this.tabCtrMain.SelectedIndex = 0;
            this.tabCtrMain.SelectedIndexChanged += new System.EventHandler(this.tabCtrMain_SelectedIndexChanged);
            // 
            // pageEpcID
            // 
            this.pageEpcID.BackColor = System.Drawing.Color.White;
            this.pageEpcID.Controls.Add(this.tableLayoutPanelID);
            this.pageEpcID.Controls.Add(this.btShowDebug1);
            this.pageEpcID.Controls.Add(this.pictureBoxID);
            resources.ApplyResources(this.pageEpcID, "pageEpcID");
            this.pageEpcID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            this.pageEpcID.Name = "pageEpcID";
            // 
            // tableLayoutPanelID
            // 
            resources.ApplyResources(this.tableLayoutPanelID, "tableLayoutPanelID");
            this.tableLayoutPanelID.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanelID.Controls.Add(this.tbEPCTagDetected, 0, 2);
            this.tableLayoutPanelID.Controls.Add(this.tBAccessCodeVerify, 0, 4);
            this.tableLayoutPanelID.Name = "tableLayoutPanelID";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            // 
            // tbEPCTagDetected
            // 
            this.tbEPCTagDetected.BackColor = System.Drawing.Color.White;
            this.tbEPCTagDetected.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbEPCTagDetected, "tbEPCTagDetected");
            this.tbEPCTagDetected.ForeColor = System.Drawing.Color.Black;
            this.tbEPCTagDetected.Name = "tbEPCTagDetected";
            this.tbEPCTagDetected.ReadOnly = true;
            // 
            // tBAccessCodeVerify
            // 
            this.tBAccessCodeVerify.BackColor = System.Drawing.Color.White;
            this.tBAccessCodeVerify.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tBAccessCodeVerify, "tBAccessCodeVerify");
            this.tBAccessCodeVerify.ForeColor = System.Drawing.Color.Green;
            this.tBAccessCodeVerify.Name = "tBAccessCodeVerify";
            this.tBAccessCodeVerify.ReadOnly = true;
            // 
            // btShowDebug1
            // 
            this.btShowDebug1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btShowDebug1.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btShowDebug1, "btShowDebug1");
            this.btShowDebug1.Name = "btShowDebug1";
            this.btShowDebug1.UseVisualStyleBackColor = true;
            this.btShowDebug1.Click += new System.EventHandler(this.btShowDebug1_Click);
            // 
            // pictureBoxID
            // 
            resources.ApplyResources(this.pictureBoxID, "pictureBoxID");
            this.pictureBoxID.Name = "pictureBoxID";
            this.pictureBoxID.TabStop = false;
            // 
            // pageData
            // 
            this.pageData.BackColor = System.Drawing.Color.White;
            this.pageData.Controls.Add(this.btShowDebug);
            this.pageData.Controls.Add(this.tableLayoutPanelDataTop);
            this.pageData.Controls.Add(this.tableLayoutPanelData);
            this.pageData.Controls.Add(this.label4);
            this.pageData.Controls.Add(this.pictureBoxData);
            this.pageData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            resources.ApplyResources(this.pageData, "pageData");
            this.pageData.Name = "pageData";
            // 
            // btShowDebug
            // 
            this.btShowDebug.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btShowDebug, "btShowDebug");
            this.btShowDebug.Name = "btShowDebug";
            this.btShowDebug.UseVisualStyleBackColor = true;
            this.btShowDebug.Click += new System.EventHandler(this.btShowDebug_Click);
            // 
            // tableLayoutPanelDataTop
            // 
            resources.ApplyResources(this.tableLayoutPanelDataTop, "tableLayoutPanelDataTop");
            this.tableLayoutPanelDataTop.Controls.Add(this.labelPID, 0, 1);
            this.tableLayoutPanelDataTop.Controls.Add(this.richTextBoxProductID, 1, 1);
            this.tableLayoutPanelDataTop.Controls.Add(this.labelWOrder, 0, 0);
            this.tableLayoutPanelDataTop.Controls.Add(this.richtbWorkOrderInfo, 1, 0);
            this.tableLayoutPanelDataTop.Name = "tableLayoutPanelDataTop";
            // 
            // labelPID
            // 
            resources.ApplyResources(this.labelPID, "labelPID");
            this.labelPID.Name = "labelPID";
            // 
            // richTextBoxProductID
            // 
            this.richTextBoxProductID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBoxProductID, "richTextBoxProductID");
            this.richTextBoxProductID.Name = "richTextBoxProductID";
            // 
            // labelWOrder
            // 
            resources.ApplyResources(this.labelWOrder, "labelWOrder");
            this.labelWOrder.Name = "labelWOrder";
            // 
            // richtbWorkOrderInfo
            // 
            this.richtbWorkOrderInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richtbWorkOrderInfo, "richtbWorkOrderInfo");
            this.richtbWorkOrderInfo.Name = "richtbWorkOrderInfo";
            // 
            // tableLayoutPanelData
            // 
            resources.ApplyResources(this.tableLayoutPanelData, "tableLayoutPanelData");
            this.tableLayoutPanelData.Controls.Add(this.textBox2, 0, 0);
            this.tableLayoutPanelData.Controls.Add(this.tbDataVerify, 0, 2);
            this.tableLayoutPanelData.Controls.Add(this.tbOdooStatus, 0, 4);
            this.tableLayoutPanelData.Name = "tableLayoutPanelData";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.ForeColor = System.Drawing.Color.Black;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            // 
            // tbDataVerify
            // 
            this.tbDataVerify.BackColor = System.Drawing.Color.White;
            this.tbDataVerify.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbDataVerify, "tbDataVerify");
            this.tbDataVerify.ForeColor = System.Drawing.Color.Black;
            this.tbDataVerify.Name = "tbDataVerify";
            this.tbDataVerify.ReadOnly = true;
            // 
            // tbOdooStatus
            // 
            this.tbOdooStatus.BackColor = System.Drawing.Color.White;
            this.tbOdooStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbOdooStatus, "tbOdooStatus");
            this.tbOdooStatus.ForeColor = System.Drawing.Color.Green;
            this.tbOdooStatus.Name = "tbOdooStatus";
            this.tbOdooStatus.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // pictureBoxData
            // 
            resources.ApplyResources(this.pictureBoxData, "pictureBoxData");
            this.pictureBoxData.Name = "pictureBoxData";
            this.pictureBoxData.TabStop = false;
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
            // timerInventory
            // 
            this.timerInventory.Interval = 200;
            this.timerInventory.Tick += new System.EventHandler(this.timerInventory_Tick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btHelp);
            this.splitContainer2.Panel1.Controls.Add(this.btClose);
            this.splitContainer2.Panel1.Controls.Add(this.tbLoginID);
            this.splitContainer2.Panel1.Controls.Add(this.tabCtrMain);
            this.splitContainer2.Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitContainer2_Panel1_MouseDown);
            this.splitContainer2.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitContainer2_Panel1_MouseMove);
            this.splitContainer2.Panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitContainer2_Panel1_MouseUp);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            // 
            // btHelp
            // 
            this.btHelp.BackColor = System.Drawing.Color.Transparent;
            this.btHelp.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btHelp, "btHelp");
            this.btHelp.ForeColor = System.Drawing.Color.Gold;
            this.btHelp.Name = "btHelp";
            this.btHelp.UseVisualStyleBackColor = false;
            this.btHelp.Click += new System.EventHandler(this.btHelp_Click);
            // 
            // btClose
            // 
            this.btClose.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btClose, "btClose");
            this.btClose.ForeColor = System.Drawing.Color.White;
            this.btClose.Name = "btClose";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // tbLoginID
            // 
            this.tbLoginID.AllowDrop = true;
            this.tbLoginID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            this.tbLoginID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbLoginID, "tbLoginID");
            this.tbLoginID.ForeColor = System.Drawing.Color.White;
            this.tbLoginID.Name = "tbLoginID";
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
            // RFIDTagIDForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.splitContainer2);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RFIDTagIDForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RFIDTagIDForm_FormClosing);
            this.Load += new System.EventHandler(this.RFIDTagIDForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabCtrMain.ResumeLayout(false);
            this.pageEpcID.ResumeLayout(false);
            this.tableLayoutPanelID.ResumeLayout(false);
            this.tableLayoutPanelID.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxID)).EndInit();
            this.pageData.ResumeLayout(false);
            this.pageData.PerformLayout();
            this.tableLayoutPanelDataTop.ResumeLayout(false);
            this.tableLayoutPanelDataTop.PerformLayout();
            this.tableLayoutPanelData.ResumeLayout(false);
            this.tableLayoutPanelData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxData)).EndInit();
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
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lxLedControl18)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCtrMain;
        private CustomControl.LogRichTextBox lrtxtLog;
        private System.Windows.Forms.TabPage pageEpcID;
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
        private System.Windows.Forms.CheckBox ckClearOperationRec;
        private System.Windows.Forms.Timer timerInventory;
        private TextBox tBAccessCodeVerify;
        private GroupBox groupBox4;
        private TextBox tbEPCTagDetected;
        private ListView listViewEPCTag;
        private ColumnHeader RFIDTag;
        private ColumnHeader Counts;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private TabPage pageData;
        private Label label4;
        private CheckBox checkBoxShowDetail;
        private Label labelWOrder;
        private RichTextBox richtbWorkOrderInfo;
        private TextBox textBox1;
        private PictureBox pictureBoxID;
        private TextBox tbOdooStatus;
        private TextBox tbDataVerify;
        private RichTextBox richTextBoxProductID;
        private Label labelPID;
        private TextBox textBox2;
        private PictureBox pictureBoxData;
        private TextBox tbLoginID;
        private Button btShowDebug;
        private Button btShowDebug1;
        private TableLayoutPanel tableLayoutPanelData;
        private TableLayoutPanel tableLayoutPanelID;
        private TableLayoutPanel tableLayoutPanelDataTop;
        private SplitContainer splitContainer3;
        private Button btClose;
        private Button btHelp;
    }
}

