namespace RFIDApplication
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.label1 = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btLogin = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBoxLogin = new System.Windows.Forms.PictureBox();
            this.btClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogin)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Roboto Medium", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(113, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "RFID Workstation";
            // 
            // tbUserName
            // 
            this.tbUserName.AcceptsTab = true;
            this.tbUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            this.tbUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUserName.Font = new System.Drawing.Font("Roboto Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUserName.ForeColor = System.Drawing.SystemColors.Control;
            this.tbUserName.Location = new System.Drawing.Point(434, 117);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbUserName.Size = new System.Drawing.Size(215, 16);
            this.tbUserName.TabIndex = 3;
            this.tbUserName.Text = "Username";
            this.tbUserName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbUserName_MouseClick);
            // 
            // tbPassword
            // 
            this.tbPassword.AcceptsTab = true;
            this.tbPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            this.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPassword.Font = new System.Drawing.Font("Roboto Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPassword.ForeColor = System.Drawing.Color.White;
            this.tbPassword.Location = new System.Drawing.Point(434, 161);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(215, 16);
            this.tbPassword.TabIndex = 4;
            this.tbPassword.Text = "Password";
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbPassword_MouseClick);
            this.tbPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbPassword_KeyUp);
            // 
            // btLogin
            // 
            this.btLogin.BackColor = System.Drawing.Color.Transparent;
            this.btLogin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            this.btLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLogin.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLogin.ForeColor = System.Drawing.Color.Transparent;
            this.btLogin.Location = new System.Drawing.Point(490, 221);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(103, 40);
            this.btLogin.TabIndex = 5;
            this.btLogin.Text = "LOGIN";
            this.btLogin.UseVisualStyleBackColor = false;
            this.btLogin.Visible = false;
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(45, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(301, 248);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBoxNote
            // 
            this.textBoxNote.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            this.textBoxNote.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxNote.Font = new System.Drawing.Font("Roboto Medium", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.textBoxNote.Location = new System.Drawing.Point(401, 297);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(280, 15);
            this.textBoxNote.TabIndex = 9;
            this.textBoxNote.Text = "Please connect to RFID Reader to PC.";
            this.textBoxNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxNote.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(430, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(223, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "____________________________________";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(430, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(223, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "____________________________________";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Roboto Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(241, 369);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(227, 14);
            this.label4.TabIndex = 13;
            this.label4.Text = "2020 Pack-Smart Inc. All Rights Reserved";
            // 
            // pictureBoxLogin
            // 
            this.pictureBoxLogin.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogin.Image")));
            this.pictureBoxLogin.Location = new System.Drawing.Point(402, 167);
            this.pictureBoxLogin.Name = "pictureBoxLogin";
            this.pictureBoxLogin.Size = new System.Drawing.Size(279, 144);
            this.pictureBoxLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogin.TabIndex = 14;
            this.pictureBoxLogin.TabStop = false;
            this.pictureBoxLogin.Visible = false;
            // 
            // btClose
            // 
            this.btClose.FlatAppearance.BorderSize = 0;
            this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClose.Font = new System.Drawing.Font("Garamond", 16F);
            this.btClose.ForeColor = System.Drawing.Color.White;
            this.btClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btClose.Location = new System.Drawing.Point(690, 12);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(28, 27);
            this.btClose.TabIndex = 20;
            this.btClose.Text = "×";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(88)))), ((int)(((byte)(136)))));
            this.ClientSize = new System.Drawing.Size(730, 396);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.btLogin);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBoxLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBoxLogin;
        private System.Windows.Forms.Button btClose;
    }
}