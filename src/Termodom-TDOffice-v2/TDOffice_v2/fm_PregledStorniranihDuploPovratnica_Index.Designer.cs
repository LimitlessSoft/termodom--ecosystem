namespace TDOffice_v2
{
    partial class fm_PregledStorniranihDuploPovratnica_Index
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
            this.magacini_clb = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.status_cmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tipStornoRacuna_cmb = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.suma_txt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // magacini_clb
            // 
            this.magacini_clb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.magacini_clb.FormattingEnabled = true;
            this.magacini_clb.Location = new System.Drawing.Point(12, 12);
            this.magacini_clb.Name = "magacini_clb";
            this.magacini_clb.Size = new System.Drawing.Size(589, 169);
            this.magacini_clb.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tipStornoRacuna_cmb);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.status_cmb);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.odDatuma_dtp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.doDatuma_dtp);
            this.groupBox1.Location = new System.Drawing.Point(607, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 169);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Do datuma:";
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odDatuma_dtp.Location = new System.Drawing.Point(77, 19);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.odDatuma_dtp.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Od datuma:";
            // 
            // doDatuma_dtp
            // 
            this.doDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.doDatuma_dtp.Location = new System.Drawing.Point(77, 45);
            this.doDatuma_dtp.Name = "doDatuma_dtp";
            this.doDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.doDatuma_dtp.TabIndex = 8;
            // 
            // status_cmb
            // 
            this.status_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.status_cmb.FormattingEnabled = true;
            this.status_cmb.Location = new System.Drawing.Point(12, 92);
            this.status_cmb.Name = "status_cmb";
            this.status_cmb.Size = new System.Drawing.Size(218, 21);
            this.status_cmb.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Status:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Tip storno racuna:";
            // 
            // tipStornoRacuna_cmb
            // 
            this.tipStornoRacuna_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tipStornoRacuna_cmb.FormattingEnabled = true;
            this.tipStornoRacuna_cmb.Location = new System.Drawing.Point(12, 135);
            this.tipStornoRacuna_cmb.Name = "tipStornoRacuna_cmb";
            this.tipStornoRacuna_cmb.Size = new System.Drawing.Size(218, 21);
            this.tipStornoRacuna_cmb.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(255, 119);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 39);
            this.button1.TabIndex = 15;
            this.button1.Text = "Prikazi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 187);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(986, 259);
            this.dataGridView1.TabIndex = 2;
            // 
            // suma_txt
            // 
            this.suma_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.suma_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.suma_txt.Location = new System.Drawing.Point(52, 452);
            this.suma_txt.Name = "suma_txt";
            this.suma_txt.ReadOnly = true;
            this.suma_txt.Size = new System.Drawing.Size(196, 20);
            this.suma_txt.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 455);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Suma:";
            // 
            // fm_PregledStorniranihDuploPovratnica_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 484);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.suma_txt);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.magacini_clb);
            this.Name = "fm_PregledStorniranihDuploPovratnica_Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pregled storniranih, duplofiskalizovanih racuna i povratnica";
            this.Load += new System.EventHandler(this.fm_PregledStorniranihDuploPovratnica_Index_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox magacini_clb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox tipStornoRacuna_cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox status_cmb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox suma_txt;
        private System.Windows.Forms.Label label5;
    }
}