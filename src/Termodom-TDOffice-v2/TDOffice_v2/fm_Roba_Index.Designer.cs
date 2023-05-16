namespace TDOffice_v2
{
    partial class fm_Roba_Index
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
            this.label10 = new System.Windows.Forms.Label();
            this.pdvTarifa_cmb = new System.Windows.Forms.ComboBox();
            this.transportnoPakovanje_cb = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.transportnoPakovanjeKolicina_txt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.transportnoPakovanjeJM_cmb = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.transportnoPakovanje_gb = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.jm_cmb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.podgrupa_cmb = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grupa_cmb = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.proizvodjac_cmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.katBrPro_txt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.katBr_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.naziv_txt = new System.Windows.Forms.TextBox();
            this.robaID_txt = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.transportnoPakovanje_gb.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 218);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "PDV Tarifa:";
            // 
            // pdvTarifa_cmb
            // 
            this.pdvTarifa_cmb.FormattingEnabled = true;
            this.pdvTarifa_cmb.Location = new System.Drawing.Point(81, 215);
            this.pdvTarifa_cmb.Name = "pdvTarifa_cmb";
            this.pdvTarifa_cmb.Size = new System.Drawing.Size(108, 21);
            this.pdvTarifa_cmb.TabIndex = 35;
            // 
            // transportnoPakovanje_cb
            // 
            this.transportnoPakovanje_cb.AutoSize = true;
            this.transportnoPakovanje_cb.Location = new System.Drawing.Point(145, 252);
            this.transportnoPakovanje_cb.Name = "transportnoPakovanje_cb";
            this.transportnoPakovanje_cb.Size = new System.Drawing.Size(15, 14);
            this.transportnoPakovanje_cb.TabIndex = 20;
            this.transportnoPakovanje_cb.UseVisualStyleBackColor = true;
            this.transportnoPakovanje_cb.CheckedChanged += new System.EventHandler(this.transportnoPakovanje_cb_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(32, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Kolicina:";
            // 
            // transportnoPakovanjeKolicina_txt
            // 
            this.transportnoPakovanjeKolicina_txt.Location = new System.Drawing.Point(85, 47);
            this.transportnoPakovanjeKolicina_txt.Name = "transportnoPakovanjeKolicina_txt";
            this.transportnoPakovanjeKolicina_txt.Size = new System.Drawing.Size(92, 20);
            this.transportnoPakovanjeKolicina_txt.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "JM:";
            // 
            // transportnoPakovanjeJM_cmb
            // 
            this.transportnoPakovanjeJM_cmb.FormattingEnabled = true;
            this.transportnoPakovanjeJM_cmb.Location = new System.Drawing.Point(69, 20);
            this.transportnoPakovanjeJM_cmb.Name = "transportnoPakovanjeJM_cmb";
            this.transportnoPakovanjeJM_cmb.Size = new System.Drawing.Size(108, 21);
            this.transportnoPakovanjeJM_cmb.TabIndex = 15;
            this.transportnoPakovanjeJM_cmb.SelectedIndexChanged += new System.EventHandler(this.transportnoPakovanjeJM_cmb_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 356);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(322, 23);
            this.button1.TabIndex = 37;
            this.button1.Text = "Azuriraj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // transportnoPakovanje_gb
            // 
            this.transportnoPakovanje_gb.Controls.Add(this.label9);
            this.transportnoPakovanje_gb.Controls.Add(this.transportnoPakovanjeKolicina_txt);
            this.transportnoPakovanje_gb.Controls.Add(this.label8);
            this.transportnoPakovanje_gb.Controls.Add(this.transportnoPakovanjeJM_cmb);
            this.transportnoPakovanje_gb.Enabled = false;
            this.transportnoPakovanje_gb.Location = new System.Drawing.Point(16, 252);
            this.transportnoPakovanje_gb.Name = "transportnoPakovanje_gb";
            this.transportnoPakovanje_gb.Size = new System.Drawing.Size(209, 98);
            this.transportnoPakovanje_gb.TabIndex = 34;
            this.transportnoPakovanje_gb.TabStop = false;
            this.transportnoPakovanje_gb.Text = "Transportno Pakovajne";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 191);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "JM:";
            // 
            // jm_cmb
            // 
            this.jm_cmb.FormattingEnabled = true;
            this.jm_cmb.Location = new System.Drawing.Point(43, 188);
            this.jm_cmb.Name = "jm_cmb";
            this.jm_cmb.Size = new System.Drawing.Size(108, 21);
            this.jm_cmb.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Podgrupa:";
            // 
            // podgrupa_cmb
            // 
            this.podgrupa_cmb.FormattingEnabled = true;
            this.podgrupa_cmb.Location = new System.Drawing.Point(75, 161);
            this.podgrupa_cmb.Name = "podgrupa_cmb";
            this.podgrupa_cmb.Size = new System.Drawing.Size(220, 21);
            this.podgrupa_cmb.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Grupa:";
            // 
            // grupa_cmb
            // 
            this.grupa_cmb.FormattingEnabled = true;
            this.grupa_cmb.Location = new System.Drawing.Point(58, 134);
            this.grupa_cmb.Name = "grupa_cmb";
            this.grupa_cmb.Size = new System.Drawing.Size(189, 21);
            this.grupa_cmb.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Proizvodjac:";
            // 
            // proizvodjac_cmb
            // 
            this.proizvodjac_cmb.FormattingEnabled = true;
            this.proizvodjac_cmb.Location = new System.Drawing.Point(84, 107);
            this.proizvodjac_cmb.Name = "proizvodjac_cmb";
            this.proizvodjac_cmb.Size = new System.Drawing.Size(226, 21);
            this.proizvodjac_cmb.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Kat Br Proizvodjaca:";
            // 
            // katBrPro_txt
            // 
            this.katBrPro_txt.Location = new System.Drawing.Point(120, 55);
            this.katBrPro_txt.Name = "katBrPro_txt";
            this.katBrPro_txt.Size = new System.Drawing.Size(175, 20);
            this.katBrPro_txt.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Naizv:";
            // 
            // katBr_txt
            // 
            this.katBr_txt.BackColor = System.Drawing.SystemColors.Window;
            this.katBr_txt.Location = new System.Drawing.Point(56, 29);
            this.katBr_txt.Name = "katBr_txt";
            this.katBr_txt.Size = new System.Drawing.Size(203, 20);
            this.katBr_txt.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Kat Br.";
            // 
            // naziv_txt
            // 
            this.naziv_txt.Location = new System.Drawing.Point(56, 81);
            this.naziv_txt.Name = "naziv_txt";
            this.naziv_txt.Size = new System.Drawing.Size(276, 20);
            this.naziv_txt.TabIndex = 19;
            // 
            // robaID_txt
            // 
            this.robaID_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.robaID_txt.Location = new System.Drawing.Point(64, 3);
            this.robaID_txt.Name = "robaID_txt";
            this.robaID_txt.ReadOnly = true;
            this.robaID_txt.Size = new System.Drawing.Size(129, 20);
            this.robaID_txt.TabIndex = 40;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 6);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 39;
            this.label11.Text = "RobaID:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.robaID_txt);
            this.panel1.Controls.Add(this.naziv_txt);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.katBr_txt);
            this.panel1.Controls.Add(this.pdvTarifa_cmb);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.transportnoPakovanje_cb);
            this.panel1.Controls.Add(this.katBrPro_txt);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.transportnoPakovanje_gb);
            this.panel1.Controls.Add(this.proizvodjac_cmb);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.jm_cmb);
            this.panel1.Controls.Add(this.grupa_cmb);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.podgrupa_cmb);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(346, 391);
            this.panel1.TabIndex = 41;
            // 
            // fm_Roba_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 415);
            this.Controls.Add(this.panel1);
            this.Name = "fm_Roba_Index";
            this.Text = "fm_Roba_Index";
            this.Load += new System.EventHandler(this.fm_Roba_Index_Load);
            this.transportnoPakovanje_gb.ResumeLayout(false);
            this.transportnoPakovanje_gb.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox pdvTarifa_cmb;
        private System.Windows.Forms.CheckBox transportnoPakovanje_cb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox transportnoPakovanjeKolicina_txt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox transportnoPakovanjeJM_cmb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox transportnoPakovanje_gb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox jm_cmb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox podgrupa_cmb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox grupa_cmb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox proizvodjac_cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox katBrPro_txt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox katBr_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox naziv_txt;
        private System.Windows.Forms.TextBox robaID_txt;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
    }
}