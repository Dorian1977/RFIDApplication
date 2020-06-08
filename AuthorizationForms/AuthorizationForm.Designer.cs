namespace AuthorizationForms
{
    partial class AuthorizationForm
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
            this.tbDate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTagID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDongleID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btGenerate = new System.Windows.Forms.Button();
            this.btVerify = new System.Windows.Forms.Button();
            this.tbEncryptedData = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbInkVolume = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // tbDate
            // 
            this.tbDate.Location = new System.Drawing.Point(12, 29);
            this.tbDate.Name = "tbDate";
            this.tbDate.Size = new System.Drawing.Size(79, 20);
            this.tbDate.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Expire Date (mmddyy)";
            // 
            // tbTagID
            // 
            this.tbTagID.Location = new System.Drawing.Point(12, 79);
            this.tbTagID.Name = "tbTagID";
            this.tbTagID.Size = new System.Drawing.Size(171, 20);
            this.tbTagID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "TagID (PS#)";
            // 
            // tbDongleID
            // 
            this.tbDongleID.Location = new System.Drawing.Point(12, 131);
            this.tbDongleID.Name = "tbDongleID";
            this.tbDongleID.Size = new System.Drawing.Size(171, 20);
            this.tbDongleID.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "DongleID";
            // 
            // btGenerate
            // 
            this.btGenerate.Location = new System.Drawing.Point(16, 220);
            this.btGenerate.Name = "btGenerate";
            this.btGenerate.Size = new System.Drawing.Size(75, 23);
            this.btGenerate.TabIndex = 6;
            this.btGenerate.Text = "Generate";
            this.btGenerate.UseVisualStyleBackColor = true;
            this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
            // 
            // btVerify
            // 
            this.btVerify.Location = new System.Drawing.Point(97, 220);
            this.btVerify.Name = "btVerify";
            this.btVerify.Size = new System.Drawing.Size(75, 23);
            this.btVerify.TabIndex = 7;
            this.btVerify.Text = "Verify";
            this.btVerify.UseVisualStyleBackColor = true;
            this.btVerify.Click += new System.EventHandler(this.btVerify_Click);
            // 
            // tbEncryptedData
            // 
            this.tbEncryptedData.Location = new System.Drawing.Point(12, 194);
            this.tbEncryptedData.Name = "tbEncryptedData";
            this.tbEncryptedData.Size = new System.Drawing.Size(300, 20);
            this.tbEncryptedData.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Encrypted Data";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(152, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "ink volume (ml)";
            // 
            // tbInkVolume
            // 
            this.tbInkVolume.Location = new System.Drawing.Point(155, 29);
            this.tbInkVolume.Name = "tbInkVolume";
            this.tbInkVolume.Size = new System.Drawing.Size(82, 20);
            this.tbInkVolume.TabIndex = 11;
            this.tbInkVolume.Text = "1500";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(178, 220);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Save File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AuthorizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 268);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbInkVolume);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbEncryptedData);
            this.Controls.Add(this.btVerify);
            this.Controls.Add(this.btGenerate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDongleID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbTagID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDate);
            this.Name = "AuthorizationForm";
            this.Text = "AuthorizationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTagID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDongleID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btGenerate;
        private System.Windows.Forms.Button btVerify;
        private System.Windows.Forms.TextBox tbEncryptedData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbInkVolume;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

