namespace RFIDApplication
{
    partial class RFID_Desktop_App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RFID_Desktop_App));
            this.tabCtrMain = new System.Windows.Forms.TabControl();
            this.pageEpcTest = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listViewEPCTag = new System.Windows.Forms.ListView();
            this.RFIDTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Counts = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ckClearOperationRec = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.timerInventory = new System.Windows.Forms.Timer(this.components);
            this.lrtxtLog = new CustomControl.LogRichTextBox();
            this.tabCtrMain.SuspendLayout();
            this.pageEpcTest.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCtrMain
            // 
            this.tabCtrMain.Controls.Add(this.pageEpcTest);
            this.tabCtrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabCtrMain.Location = new System.Drawing.Point(0, 0);
            this.tabCtrMain.Name = "tabCtrMain";
            this.tabCtrMain.SelectedIndex = 0;
            this.tabCtrMain.Size = new System.Drawing.Size(338, 230);
            this.tabCtrMain.TabIndex = 1;
            // 
            // pageEpcTest
            // 
            this.pageEpcTest.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pageEpcTest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageEpcTest.Controls.Add(this.groupBox4);
            this.pageEpcTest.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold);
            this.pageEpcTest.ForeColor = System.Drawing.SystemColors.Desktop;
            this.pageEpcTest.Location = new System.Drawing.Point(4, 22);
            this.pageEpcTest.Name = "pageEpcTest";
            this.pageEpcTest.Size = new System.Drawing.Size(330, 204);
            this.pageEpcTest.TabIndex = 5;
            this.pageEpcTest.Text = "18000-6C Tag Access";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listViewEPCTag);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Font = new System.Drawing.Font("SimSun", 12F);
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(328, 202);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Scan EPC Tag";
            // 
            // listViewEPCTag
            // 
            this.listViewEPCTag.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RFIDTag,
            this.Counts});
            this.listViewEPCTag.Dock = System.Windows.Forms.DockStyle.Top;
            this.listViewEPCTag.Font = new System.Drawing.Font("SimSun", 9F);
            this.listViewEPCTag.FullRowSelect = true;
            this.listViewEPCTag.GridLines = true;
            this.listViewEPCTag.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewEPCTag.Location = new System.Drawing.Point(3, 22);
            this.listViewEPCTag.MultiSelect = false;
            this.listViewEPCTag.Name = "listViewEPCTag";
            this.listViewEPCTag.Size = new System.Drawing.Size(322, 177);
            this.listViewEPCTag.TabIndex = 10;
            this.listViewEPCTag.UseCompatibleStateImageBehavior = false;
            this.listViewEPCTag.View = System.Windows.Forms.View.Details;
            // 
            // RFIDTag
            // 
            this.RFIDTag.Text = "RFIDTag";
            this.RFIDTag.Width = 250;
            // 
            // Counts
            // 
            this.Counts.Text = "Counts";
            this.Counts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ckClearOperationRec
            // 
            this.ckClearOperationRec.AutoSize = true;
            this.ckClearOperationRec.Checked = true;
            this.ckClearOperationRec.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckClearOperationRec.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ckClearOperationRec.Location = new System.Drawing.Point(117, 233);
            this.ckClearOperationRec.Name = "ckClearOperationRec";
            this.ckClearOperationRec.Size = new System.Drawing.Size(75, 17);
            this.ckClearOperationRec.TabIndex = 21;
            this.ckClearOperationRec.Text = "Auto Clear";
            this.ckClearOperationRec.UseVisualStyleBackColor = true;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label35.Location = new System.Drawing.Point(5, 233);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(91, 13);
            this.label35.TabIndex = 19;
            this.label35.Text = "Operation History:";
            // 
            // timerInventory
            // 
            this.timerInventory.Enabled = true;
            this.timerInventory.Interval = 200;
            this.timerInventory.Tick += new System.EventHandler(this.timerInventory_Tick);
            // 
            // lrtxtLog
            // 
            this.lrtxtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lrtxtLog.Location = new System.Drawing.Point(0, 251);
            this.lrtxtLog.Name = "lrtxtLog";
            this.lrtxtLog.Size = new System.Drawing.Size(338, 123);
            this.lrtxtLog.TabIndex = 18;
            this.lrtxtLog.Text = "";
            // 
            // RFID_Desktop_App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 374);
            this.Controls.Add(this.ckClearOperationRec);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.lrtxtLog);
            this.Controls.Add(this.tabCtrMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RFID_Desktop_App";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.RFID_Desktop_Load);
            this.tabCtrMain.ResumeLayout(false);
            this.pageEpcTest.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabCtrMain;
        private System.Windows.Forms.CheckBox ckClearOperationRec;
        private System.Windows.Forms.Label label35;
        private CustomControl.LogRichTextBox lrtxtLog;
        private System.Windows.Forms.Timer timerInventory;
        private System.Windows.Forms.TabPage pageEpcTest;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView listViewEPCTag;
        private System.Windows.Forms.ColumnHeader RFIDTag;
        private System.Windows.Forms.ColumnHeader Counts;
    }
}

