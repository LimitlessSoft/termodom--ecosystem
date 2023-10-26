namespace TDOffice_v2
{
    partial class fm_Ugovor_Index
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.datumUgovora_dtp = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_OdbaciIzmene = new System.Windows.Forms.Button();
            this.btn_Sacuvaj = new System.Windows.Forms.Button();
            this.osvezi_btn = new System.Windows.Forms.Button();
            this.nova_btn = new System.Windows.Forms.Button();
            this.prostornaJedinica_dgv = new System.Windows.Forms.DataGridView();
            this.tipUgovora_cmb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.prodavac_cmb = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.kupac_cmb = new System.Windows.Forms.ComboBox();
            this.vrednostUgovoraRSD_txt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.novaRata_btn = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.prostornaJedinica_dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.magacin_cmb);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.vrednostUgovoraRSD_txt);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.kupac_cmb);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.prodavac_cmb);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.tipUgovora_cmb);
            this.panel2.Controls.Add(this.datumUgovora_dtp);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(12, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1047, 172);
            this.panel2.TabIndex = 28;
            // 
            // datumUgovora_dtp
            // 
            this.datumUgovora_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.datumUgovora_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datumUgovora_dtp.Location = new System.Drawing.Point(94, 31);
            this.datumUgovora_dtp.Name = "datumUgovora_dtp";
            this.datumUgovora_dtp.Size = new System.Drawing.Size(144, 20);
            this.datumUgovora_dtp.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Datum Ugovora:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btn_OdbaciIzmene);
            this.panel1.Controls.Add(this.btn_Sacuvaj);
            this.panel1.Controls.Add(this.osvezi_btn);
            this.panel1.Controls.Add(this.nova_btn);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1047, 35);
            this.panel1.TabIndex = 27;
            // 
            // btn_OdbaciIzmene
            // 
            this.btn_OdbaciIzmene.BackgroundImage = global::TDOffice_v2.Properties.Resources.discard_icon;
            this.btn_OdbaciIzmene.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_OdbaciIzmene.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_OdbaciIzmene.Location = new System.Drawing.Point(84, 3);
            this.btn_OdbaciIzmene.Name = "btn_OdbaciIzmene";
            this.btn_OdbaciIzmene.Size = new System.Drawing.Size(33, 29);
            this.btn_OdbaciIzmene.TabIndex = 41;
            this.btn_OdbaciIzmene.UseVisualStyleBackColor = true;
            // 
            // btn_Sacuvaj
            // 
            this.btn_Sacuvaj.BackgroundImage = global::TDOffice_v2.Properties.Resources.save_icon;
            this.btn_Sacuvaj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Sacuvaj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Sacuvaj.Location = new System.Drawing.Point(50, 3);
            this.btn_Sacuvaj.Name = "btn_Sacuvaj";
            this.btn_Sacuvaj.Size = new System.Drawing.Size(28, 29);
            this.btn_Sacuvaj.TabIndex = 40;
            this.btn_Sacuvaj.UseVisualStyleBackColor = true;
            // 
            // osvezi_btn
            // 
            this.osvezi_btn.BackgroundImage = global::TDOffice_v2.Properties.Resources.refresh_button;
            this.osvezi_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.osvezi_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.osvezi_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.osvezi_btn.Location = new System.Drawing.Point(148, 3);
            this.osvezi_btn.Name = "osvezi_btn";
            this.osvezi_btn.Size = new System.Drawing.Size(27, 29);
            this.osvezi_btn.TabIndex = 14;
            this.osvezi_btn.UseVisualStyleBackColor = true;
            // 
            // nova_btn
            // 
            this.nova_btn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.nova_btn.BackgroundImage = global::TDOffice_v2.Properties.Resources.new_icon;
            this.nova_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.nova_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nova_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.nova_btn.Location = new System.Drawing.Point(4, 3);
            this.nova_btn.Name = "nova_btn";
            this.nova_btn.Size = new System.Drawing.Size(27, 29);
            this.nova_btn.TabIndex = 13;
            this.nova_btn.UseVisualStyleBackColor = false;
            // 
            // prostornaJedinica_dgv
            // 
            this.prostornaJedinica_dgv.AllowUserToAddRows = false;
            this.prostornaJedinica_dgv.AllowUserToDeleteRows = false;
            this.prostornaJedinica_dgv.AllowUserToResizeRows = false;
            this.prostornaJedinica_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prostornaJedinica_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.prostornaJedinica_dgv.Location = new System.Drawing.Point(6, 19);
            this.prostornaJedinica_dgv.Name = "prostornaJedinica_dgv";
            this.prostornaJedinica_dgv.ReadOnly = true;
            this.prostornaJedinica_dgv.RowHeadersVisible = false;
            this.prostornaJedinica_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.prostornaJedinica_dgv.Size = new System.Drawing.Size(1035, 108);
            this.prostornaJedinica_dgv.TabIndex = 26;
            // 
            // tipUgovora_cmb
            // 
            this.tipUgovora_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tipUgovora_cmb.FormattingEnabled = true;
            this.tipUgovora_cmb.Location = new System.Drawing.Point(78, 3);
            this.tipUgovora_cmb.Name = "tipUgovora_cmb";
            this.tipUgovora_cmb.Size = new System.Drawing.Size(286, 21);
            this.tipUgovora_cmb.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Tip Ugovora:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Prodavac:";
            // 
            // prodavac_cmb
            // 
            this.prodavac_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.prodavac_cmb.FormattingEnabled = true;
            this.prodavac_cmb.Location = new System.Drawing.Point(65, 66);
            this.prodavac_cmb.Name = "prodavac_cmb";
            this.prodavac_cmb.Size = new System.Drawing.Size(286, 21);
            this.prodavac_cmb.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Kupac:";
            // 
            // kupac_cmb
            // 
            this.kupac_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kupac_cmb.FormattingEnabled = true;
            this.kupac_cmb.Location = new System.Drawing.Point(50, 93);
            this.kupac_cmb.Name = "kupac_cmb";
            this.kupac_cmb.Size = new System.Drawing.Size(286, 21);
            this.kupac_cmb.TabIndex = 10;
            // 
            // vrednostUgovoraRSD_txt
            // 
            this.vrednostUgovoraRSD_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.vrednostUgovoraRSD_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.vrednostUgovoraRSD_txt.Location = new System.Drawing.Point(857, 26);
            this.vrednostUgovoraRSD_txt.Name = "vrednostUgovoraRSD_txt";
            this.vrednostUgovoraRSD_txt.ReadOnly = true;
            this.vrednostUgovoraRSD_txt.Size = new System.Drawing.Size(187, 20);
            this.vrednostUgovoraRSD_txt.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(818, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "RSD:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(907, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Ukupna Vrednost";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(818, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "EUR:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox1.Location = new System.Drawing.Point(857, 52);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(187, 20);
            this.textBox1.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 138);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Objekat:";
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(56, 135);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(397, 21);
            this.magacin_cmb.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.prostornaJedinica_dgv);
            this.groupBox1.Location = new System.Drawing.Point(12, 231);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1047, 133);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Prostorne Jedinice Unutar Objekta Koje su tema ovog ugovora";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 399);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1047, 246);
            this.flowLayoutPanel1.TabIndex = 30;
            // 
            // novaRata_btn
            // 
            this.novaRata_btn.Location = new System.Drawing.Point(12, 370);
            this.novaRata_btn.Name = "novaRata_btn";
            this.novaRata_btn.Size = new System.Drawing.Size(75, 23);
            this.novaRata_btn.TabIndex = 31;
            this.novaRata_btn.Text = "Nova Rata";
            this.novaRata_btn.UseVisualStyleBackColor = true;
            // 
            // fm_Ugovor_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 657);
            this.Controls.Add(this.novaRata_btn);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fm_Ugovor_Index";
            this.Text = "fm_Ugovor_Index";
            this.Load += new System.EventHandler(this.fm_Ugovor_Index_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.prostornaJedinica_dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox tipUgovora_cmb;
        private System.Windows.Forms.DateTimePicker datumUgovora_dtp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_OdbaciIzmene;
        private System.Windows.Forms.Button btn_Sacuvaj;
        private System.Windows.Forms.Button osvezi_btn;
        private System.Windows.Forms.Button nova_btn;
        private System.Windows.Forms.DataGridView prostornaJedinica_dgv;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox vrednostUgovoraRSD_txt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox kupac_cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox prodavac_cmb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button novaRata_btn;
    }
}