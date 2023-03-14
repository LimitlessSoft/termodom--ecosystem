namespace TDOffice_v2
{
    partial class fm_Cekovi_Index
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
            this.label9 = new System.Windows.Forms.Label();
            this.datumValute_dtp = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.vrednost_txt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.brojCeka_txt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.trGradjana_txt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.banka_cmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.datumUnosa_dtp = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 38;
            this.label9.Text = "Datum Realizacije:";
            // 
            // datumValute_dtp
            // 
            this.datumValute_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.datumValute_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datumValute_dtp.Location = new System.Drawing.Point(113, 31);
            this.datumValute_dtp.Name = "datumValute_dtp";
            this.datumValute_dtp.Size = new System.Drawing.Size(144, 20);
            this.datumValute_dtp.TabIndex = 37;
            this.datumValute_dtp.ValueChanged += new System.EventHandler(this.datumValute_dtp_ValueChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(239, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(189, 23);
            this.button1.TabIndex = 36;
            this.button1.Text = "Sacuvaj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 166);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Vrednost:";
            // 
            // vrednost_txt
            // 
            this.vrednost_txt.Location = new System.Drawing.Point(81, 163);
            this.vrednost_txt.Name = "vrednost_txt";
            this.vrednost_txt.Size = new System.Drawing.Size(251, 20);
            this.vrednost_txt.TabIndex = 34;
            this.vrednost_txt.TextChanged += new System.EventHandler(this.vrednost_txt_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Broj Ceka:";
            // 
            // brojCeka_txt
            // 
            this.brojCeka_txt.Location = new System.Drawing.Point(81, 137);
            this.brojCeka_txt.Name = "brojCeka_txt";
            this.brojCeka_txt.Size = new System.Drawing.Size(251, 20);
            this.brojCeka_txt.TabIndex = 32;
            this.brojCeka_txt.TextChanged += new System.EventHandler(this.brojCeka_txt_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Tekuci Racun Gradjana:";
            // 
            // trGradjana_txt
            // 
            this.trGradjana_txt.Location = new System.Drawing.Point(149, 111);
            this.trGradjana_txt.Name = "trGradjana_txt";
            this.trGradjana_txt.Size = new System.Drawing.Size(251, 20);
            this.trGradjana_txt.TabIndex = 30;
            this.trGradjana_txt.TextChanged += new System.EventHandler(this.trGradjana_txt_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Podnosilac Banka:";
            // 
            // banka_cmb
            // 
            this.banka_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.banka_cmb.FormattingEnabled = true;
            this.banka_cmb.Items.AddRange(new object[] {
            "Banka"});
            this.banka_cmb.Location = new System.Drawing.Point(121, 84);
            this.banka_cmb.Name = "banka_cmb";
            this.banka_cmb.Size = new System.Drawing.Size(196, 21);
            this.banka_cmb.TabIndex = 28;
            this.banka_cmb.SelectedIndexChanged += new System.EventHandler(this.banka_cmb_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Magacin:";
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(76, 57);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(196, 21);
            this.magacin_cmb.TabIndex = 26;
            this.magacin_cmb.SelectedIndexChanged += new System.EventHandler(this.magacin_cmb_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Datum:";
            // 
            // datumUnosa_dtp
            // 
            this.datumUnosa_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.datumUnosa_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datumUnosa_dtp.Location = new System.Drawing.Point(59, 5);
            this.datumUnosa_dtp.Name = "datumUnosa_dtp";
            this.datumUnosa_dtp.Size = new System.Drawing.Size(144, 20);
            this.datumUnosa_dtp.TabIndex = 24;
            this.datumUnosa_dtp.ValueChanged += new System.EventHandler(this.datumUnosa_dtp_ValueChanged);
            // 
            // fm_Cekovi_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 230);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.datumValute_dtp);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.vrednost_txt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.brojCeka_txt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.trGradjana_txt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.banka_cmb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.magacin_cmb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.datumUnosa_dtp);
            this.Name = "fm_Cekovi_Index";
            this.Text = "fm_Cekovi_Index";
            this.Load += new System.EventHandler(this.fm_Cekovi_Index_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker datumValute_dtp;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox vrednost_txt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox brojCeka_txt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox trGradjana_txt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox banka_cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker datumUnosa_dtp;
    }
}