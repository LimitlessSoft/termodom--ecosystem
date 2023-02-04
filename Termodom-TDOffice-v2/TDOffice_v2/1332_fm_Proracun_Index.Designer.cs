
namespace TDOffice_v2
{
    partial class _1332_fm_Proracun_Index
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.obrisiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.obrisiSveStavkeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.ppid_cmb = new System.Windows.Forms.ComboBox();
            this.pretvoriUMPRacun_btn = new System.Windows.Forms.Button();
            this.brDokKomMPRacun_txt = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.navigacija_gb = new System.Windows.Forms.GroupBox();
            this.navigacijaPrethodni_btn = new System.Windows.Forms.Button();
            this.navigacijaSledeci_btn = new System.Windows.Forms.Button();
            this.navigacijaIdiNa_txt = new System.Windows.Forms.TextBox();
            this.dokument_gbx = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmb_NacinPlacanja = new System.Windows.Forms.ComboBox();
            this.referent_txt = new System.Windows.Forms.TextBox();
            this.brDokKomProracun_txt = new System.Windows.Forms.TextBox();
            this.interniKomentar_btn = new System.Windows.Forms.Button();
            this.klonirajUKomercijalno_btn = new System.Windows.Forms.Button();
            this.komentar_btn = new System.Windows.Forms.Button();
            this.dokumentStatus_btn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.datum_txt = new System.Windows.Forms.TextBox();
            this.brDok_lbl = new System.Windows.Forms.Label();
            this.brojDokumenta_txt = new System.Windows.Forms.TextBox();
            this.help_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.navigacija_gb.SuspendLayout();
            this.dokument_gbx.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(6, 199);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1074, 404);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValidated);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.obrisiToolStripMenuItem,
            this.obrisiSveStavkeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(162, 48);
            // 
            // obrisiToolStripMenuItem
            // 
            this.obrisiToolStripMenuItem.Name = "obrisiToolStripMenuItem";
            this.obrisiToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.obrisiToolStripMenuItem.Text = "Obrisi";
            this.obrisiToolStripMenuItem.Click += new System.EventHandler(this.obrisiToolStripMenuItem_Click);
            // 
            // obrisiSveStavkeToolStripMenuItem
            // 
            this.obrisiSveStavkeToolStripMenuItem.Name = "obrisiSveStavkeToolStripMenuItem";
            this.obrisiSveStavkeToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.obrisiSveStavkeToolStripMenuItem.Text = "Obrisi sve stavke";
            this.obrisiSveStavkeToolStripMenuItem.Click += new System.EventHandler(this.obrisiSveStavkeToolStripMenuItem_Click);
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(70, 45);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(252, 21);
            this.magacin_cmb.TabIndex = 1;
            this.magacin_cmb.SelectedIndexChanged += new System.EventHandler(this.magacin_cmb_SelectedIndexChanged);
            // 
            // ppid_cmb
            // 
            this.ppid_cmb.Enabled = false;
            this.ppid_cmb.FormattingEnabled = true;
            this.ppid_cmb.Location = new System.Drawing.Point(70, 72);
            this.ppid_cmb.Name = "ppid_cmb";
            this.ppid_cmb.Size = new System.Drawing.Size(230, 21);
            this.ppid_cmb.TabIndex = 4;
            this.ppid_cmb.SelectedIndexChanged += new System.EventHandler(this.ppid_cmb_SelectedIndexChanged);
            // 
            // pretvoriUMPRacun_btn
            // 
            this.pretvoriUMPRacun_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pretvoriUMPRacun_btn.Location = new System.Drawing.Point(922, 74);
            this.pretvoriUMPRacun_btn.Name = "pretvoriUMPRacun_btn";
            this.pretvoriUMPRacun_btn.Size = new System.Drawing.Size(143, 23);
            this.pretvoriUMPRacun_btn.TabIndex = 6;
            this.pretvoriUMPRacun_btn.Text = "Pretvori U MP Racun";
            this.pretvoriUMPRacun_btn.UseVisualStyleBackColor = true;
            this.pretvoriUMPRacun_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // brDokKomMPRacun_txt
            // 
            this.brDokKomMPRacun_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.brDokKomMPRacun_txt.BackColor = System.Drawing.SystemColors.Info;
            this.brDokKomMPRacun_txt.Location = new System.Drawing.Point(794, 76);
            this.brDokKomMPRacun_txt.Name = "brDokKomMPRacun_txt";
            this.brDokKomMPRacun_txt.ReadOnly = true;
            this.brDokKomMPRacun_txt.Size = new System.Drawing.Size(119, 20);
            this.brDokKomMPRacun_txt.TabIndex = 7;
            this.brDokKomMPRacun_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.navigacija_gb);
            this.panel1.Controls.Add(this.dokument_gbx);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1080, 606);
            this.panel1.TabIndex = 11;
            // 
            // navigacija_gb
            // 
            this.navigacija_gb.Controls.Add(this.help_btn);
            this.navigacija_gb.Controls.Add(this.navigacijaPrethodni_btn);
            this.navigacija_gb.Controls.Add(this.navigacijaSledeci_btn);
            this.navigacija_gb.Controls.Add(this.navigacijaIdiNa_txt);
            this.navigacija_gb.Location = new System.Drawing.Point(6, 6);
            this.navigacija_gb.Name = "navigacija_gb";
            this.navigacija_gb.Size = new System.Drawing.Size(1065, 55);
            this.navigacija_gb.TabIndex = 14;
            this.navigacija_gb.TabStop = false;
            this.navigacija_gb.Text = "Navigacija";
            // 
            // navigacijaPrethodni_btn
            // 
            this.navigacijaPrethodni_btn.Location = new System.Drawing.Point(14, 19);
            this.navigacijaPrethodni_btn.Name = "navigacijaPrethodni_btn";
            this.navigacijaPrethodni_btn.Size = new System.Drawing.Size(75, 25);
            this.navigacijaPrethodni_btn.TabIndex = 16;
            this.navigacijaPrethodni_btn.Text = "Prethodni";
            this.navigacijaPrethodni_btn.UseVisualStyleBackColor = true;
            this.navigacijaPrethodni_btn.Click += new System.EventHandler(this.navigacijaPrethodni_btn_Click);
            // 
            // navigacijaSledeci_btn
            // 
            this.navigacijaSledeci_btn.Location = new System.Drawing.Point(201, 19);
            this.navigacijaSledeci_btn.Name = "navigacijaSledeci_btn";
            this.navigacijaSledeci_btn.Size = new System.Drawing.Size(75, 25);
            this.navigacijaSledeci_btn.TabIndex = 15;
            this.navigacijaSledeci_btn.Text = "Sledeci";
            this.navigacijaSledeci_btn.UseVisualStyleBackColor = true;
            this.navigacijaSledeci_btn.Click += new System.EventHandler(this.navigacijaSledeci_btn_Click);
            // 
            // navigacijaIdiNa_txt
            // 
            this.navigacijaIdiNa_txt.Location = new System.Drawing.Point(95, 22);
            this.navigacijaIdiNa_txt.Name = "navigacijaIdiNa_txt";
            this.navigacijaIdiNa_txt.Size = new System.Drawing.Size(100, 20);
            this.navigacijaIdiNa_txt.TabIndex = 0;
            this.navigacijaIdiNa_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.navigacijaIdiNa_txt_KeyDown);
            // 
            // dokument_gbx
            // 
            this.dokument_gbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dokument_gbx.ContextMenuStrip = this.contextMenuStrip2;
            this.dokument_gbx.Controls.Add(this.cmb_NacinPlacanja);
            this.dokument_gbx.Controls.Add(this.referent_txt);
            this.dokument_gbx.Controls.Add(this.brDokKomProracun_txt);
            this.dokument_gbx.Controls.Add(this.magacin_cmb);
            this.dokument_gbx.Controls.Add(this.ppid_cmb);
            this.dokument_gbx.Controls.Add(this.interniKomentar_btn);
            this.dokument_gbx.Controls.Add(this.klonirajUKomercijalno_btn);
            this.dokument_gbx.Controls.Add(this.komentar_btn);
            this.dokument_gbx.Controls.Add(this.dokumentStatus_btn);
            this.dokument_gbx.Controls.Add(this.label5);
            this.dokument_gbx.Controls.Add(this.brDokKomMPRacun_txt);
            this.dokument_gbx.Controls.Add(this.datum_txt);
            this.dokument_gbx.Controls.Add(this.brDok_lbl);
            this.dokument_gbx.Controls.Add(this.pretvoriUMPRacun_btn);
            this.dokument_gbx.Controls.Add(this.brojDokumenta_txt);
            this.dokument_gbx.Location = new System.Drawing.Point(6, 67);
            this.dokument_gbx.Name = "dokument_gbx";
            this.dokument_gbx.Size = new System.Drawing.Size(1068, 126);
            this.dokument_gbx.TabIndex = 13;
            this.dokument_gbx.TabStop = false;
            this.dokument_gbx.Text = "Dokument";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(466, 26);
            // 
            // podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem
            // 
            this.podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem.Name = "podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem";
            this.podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem.Size = new System.Drawing.Size(465, 22);
            this.podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem.Text = "Podesi maksimalnu zastarelost proracuna prilikom pretvaranja u MP racun";
            this.podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem.Click += new System.EventHandler(this.podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem_Click);
            // 
            // cmb_NacinPlacanja
            // 
            this.cmb_NacinPlacanja.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_NacinPlacanja.FormattingEnabled = true;
            this.cmb_NacinPlacanja.Location = new System.Drawing.Point(70, 99);
            this.cmb_NacinPlacanja.Name = "cmb_NacinPlacanja";
            this.cmb_NacinPlacanja.Size = new System.Drawing.Size(230, 21);
            this.cmb_NacinPlacanja.TabIndex = 13;
            this.cmb_NacinPlacanja.SelectedIndexChanged += new System.EventHandler(this.cmb_NacinPlacanja_SelectedIndexChanged);
            // 
            // referent_txt
            // 
            this.referent_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.referent_txt.Location = new System.Drawing.Point(265, 19);
            this.referent_txt.Name = "referent_txt";
            this.referent_txt.ReadOnly = true;
            this.referent_txt.Size = new System.Drawing.Size(152, 20);
            this.referent_txt.TabIndex = 8;
            this.referent_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // brDokKomProracun_txt
            // 
            this.brDokKomProracun_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.brDokKomProracun_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.brDokKomProracun_txt.Location = new System.Drawing.Point(794, 48);
            this.brDokKomProracun_txt.Name = "brDokKomProracun_txt";
            this.brDokKomProracun_txt.ReadOnly = true;
            this.brDokKomProracun_txt.Size = new System.Drawing.Size(119, 20);
            this.brDokKomProracun_txt.TabIndex = 12;
            this.brDokKomProracun_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // interniKomentar_btn
            // 
            this.interniKomentar_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.interniKomentar_btn.Location = new System.Drawing.Point(842, 17);
            this.interniKomentar_btn.Name = "interniKomentar_btn";
            this.interniKomentar_btn.Size = new System.Drawing.Size(105, 23);
            this.interniKomentar_btn.TabIndex = 7;
            this.interniKomentar_btn.Text = "Interni Komentar";
            this.interniKomentar_btn.UseVisualStyleBackColor = true;
            this.interniKomentar_btn.Click += new System.EventHandler(this.interniKomentar_btn_Click_1);
            // 
            // klonirajUKomercijalno_btn
            // 
            this.klonirajUKomercijalno_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.klonirajUKomercijalno_btn.Location = new System.Drawing.Point(919, 45);
            this.klonirajUKomercijalno_btn.Name = "klonirajUKomercijalno_btn";
            this.klonirajUKomercijalno_btn.Size = new System.Drawing.Size(143, 23);
            this.klonirajUKomercijalno_btn.TabIndex = 11;
            this.klonirajUKomercijalno_btn.Text = "Prebaci u komercijalno";
            this.klonirajUKomercijalno_btn.UseVisualStyleBackColor = true;
            this.klonirajUKomercijalno_btn.Click += new System.EventHandler(this.klonirajUKomercijalno_btn_Click);
            // 
            // komentar_btn
            // 
            this.komentar_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.komentar_btn.Location = new System.Drawing.Point(761, 17);
            this.komentar_btn.Name = "komentar_btn";
            this.komentar_btn.Size = new System.Drawing.Size(75, 23);
            this.komentar_btn.TabIndex = 6;
            this.komentar_btn.Text = "Komentar";
            this.komentar_btn.UseVisualStyleBackColor = true;
            this.komentar_btn.Click += new System.EventHandler(this.komentar_btn_Click);
            // 
            // dokumentStatus_btn
            // 
            this.dokumentStatus_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dokumentStatus_btn.Location = new System.Drawing.Point(987, 17);
            this.dokumentStatus_btn.Name = "dokumentStatus_btn";
            this.dokumentStatus_btn.Size = new System.Drawing.Size(75, 23);
            this.dokumentStatus_btn.TabIndex = 5;
            this.dokumentStatus_btn.Text = "Otkljucaj";
            this.dokumentStatus_btn.UseVisualStyleBackColor = true;
            this.dokumentStatus_btn.Click += new System.EventHandler(this.dokumentStatus_btn_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Magacin:";
            // 
            // datum_txt
            // 
            this.datum_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.datum_txt.Location = new System.Drawing.Point(143, 19);
            this.datum_txt.Name = "datum_txt";
            this.datum_txt.ReadOnly = true;
            this.datum_txt.Size = new System.Drawing.Size(116, 20);
            this.datum_txt.TabIndex = 2;
            this.datum_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // brDok_lbl
            // 
            this.brDok_lbl.AutoSize = true;
            this.brDok_lbl.Location = new System.Drawing.Point(6, 22);
            this.brDok_lbl.Name = "brDok_lbl";
            this.brDok_lbl.Size = new System.Drawing.Size(46, 13);
            this.brDok_lbl.TabIndex = 1;
            this.brDok_lbl.Text = "Br. Dok.";
            // 
            // brojDokumenta_txt
            // 
            this.brojDokumenta_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.brojDokumenta_txt.Location = new System.Drawing.Point(58, 19);
            this.brojDokumenta_txt.Name = "brojDokumenta_txt";
            this.brojDokumenta_txt.ReadOnly = true;
            this.brojDokumenta_txt.Size = new System.Drawing.Size(79, 20);
            this.brojDokumenta_txt.TabIndex = 0;
            this.brojDokumenta_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // help_btn
            // 
            this.help_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.help_btn.Location = new System.Drawing.Point(984, 21);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(75, 23);
            this.help_btn.TabIndex = 14;
            this.help_btn.Text = "HELP";
            this.help_btn.UseVisualStyleBackColor = true;
            this.help_btn.Click += new System.EventHandler(this.help_btn_Click);
            // 
            // _1332_fm_Proracun_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(1092, 618);
            this.Controls.Add(this.panel1);
            this.Name = "_1332_fm_Proracun_Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_1332_fm_PredlogProracuna_Index";
            this.Load += new System.EventHandler(this._1332_fm_PredlogProracuna_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.navigacija_gb.ResumeLayout(false);
            this.navigacija_gb.PerformLayout();
            this.dokument_gbx.ResumeLayout(false);
            this.dokument_gbx.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.ComboBox ppid_cmb;
        private System.Windows.Forms.Button pretvoriUMPRacun_btn;
        private System.Windows.Forms.TextBox brDokKomMPRacun_txt;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox brDokKomProracun_txt;
        private System.Windows.Forms.Button klonirajUKomercijalno_btn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem obrisiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem obrisiSveStavkeToolStripMenuItem;
        private System.Windows.Forms.GroupBox dokument_gbx;
        private System.Windows.Forms.TextBox referent_txt;
        private System.Windows.Forms.Button interniKomentar_btn;
        private System.Windows.Forms.Button komentar_btn;
        private System.Windows.Forms.Button dokumentStatus_btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox datum_txt;
        private System.Windows.Forms.Label brDok_lbl;
        private System.Windows.Forms.TextBox brojDokumenta_txt;
        private System.Windows.Forms.GroupBox navigacija_gb;
        private System.Windows.Forms.Button navigacijaPrethodni_btn;
        private System.Windows.Forms.Button navigacijaSledeci_btn;
        private System.Windows.Forms.TextBox navigacijaIdiNa_txt;
        private System.Windows.Forms.ComboBox cmb_NacinPlacanja;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem;
        private System.Windows.Forms.Button help_btn;
    }
}