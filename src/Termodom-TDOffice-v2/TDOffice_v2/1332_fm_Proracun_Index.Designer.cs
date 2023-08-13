
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
            components = new System.ComponentModel.Container();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            obrisiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            obrisiSveStavkeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            kartcaRobeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            magacin_cmb = new System.Windows.Forms.ComboBox();
            ppid_cmb = new System.Windows.Forms.ComboBox();
            pretvoriUMPRacun_btn = new System.Windows.Forms.Button();
            brDokKomMPRacun_txt = new System.Windows.Forms.TextBox();
            panel1 = new System.Windows.Forms.Panel();
            navigacija_gb = new System.Windows.Forms.GroupBox();
            help_btn = new System.Windows.Forms.Button();
            navigacijaPrethodni_btn = new System.Windows.Forms.Button();
            navigacijaSledeci_btn = new System.Windows.Forms.Button();
            navigacijaIdiNa_txt = new System.Windows.Forms.TextBox();
            dokument_gbx = new System.Windows.Forms.GroupBox();
            contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(components);
            podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cmb_NacinPlacanja = new System.Windows.Forms.ComboBox();
            referent_txt = new System.Windows.Forms.TextBox();
            brDokKomProracun_txt = new System.Windows.Forms.TextBox();
            interniKomentar_btn = new System.Windows.Forms.Button();
            klonirajUKomercijalno_btn = new System.Windows.Forms.Button();
            komentar_btn = new System.Windows.Forms.Button();
            dokumentStatus_btn = new System.Windows.Forms.Button();
            label5 = new System.Windows.Forms.Label();
            datum_txt = new System.Windows.Forms.TextBox();
            brDok_lbl = new System.Windows.Forms.Label();
            brojDokumenta_txt = new System.Windows.Forms.TextBox();
            istorijaNabavkeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            navigacija_gb.SuspendLayout();
            dokument_gbx.SuspendLayout();
            contextMenuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.Location = new System.Drawing.Point(7, 230);
            dataGridView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new System.Drawing.Size(1253, 466);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellValidated += dataGridView1_CellValidated;
            dataGridView1.DoubleClick += dataGridView1_DoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { obrisiToolStripMenuItem, obrisiSveStavkeToolStripMenuItem, kartcaRobeToolStripMenuItem, istorijaNabavkeToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(181, 114);
            // 
            // obrisiToolStripMenuItem
            // 
            obrisiToolStripMenuItem.Name = "obrisiToolStripMenuItem";
            obrisiToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            obrisiToolStripMenuItem.Text = "Obrisi";
            obrisiToolStripMenuItem.Click += obrisiToolStripMenuItem_Click;
            // 
            // obrisiSveStavkeToolStripMenuItem
            // 
            obrisiSveStavkeToolStripMenuItem.Name = "obrisiSveStavkeToolStripMenuItem";
            obrisiSveStavkeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            obrisiSveStavkeToolStripMenuItem.Text = "Obrisi sve stavke";
            obrisiSveStavkeToolStripMenuItem.Click += obrisiSveStavkeToolStripMenuItem_Click;
            // 
            // kartcaRobeToolStripMenuItem
            // 
            kartcaRobeToolStripMenuItem.Name = "kartcaRobeToolStripMenuItem";
            kartcaRobeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            kartcaRobeToolStripMenuItem.Text = "Kartca robe";
            kartcaRobeToolStripMenuItem.Click += kartcaRobeToolStripMenuItem_Click;
            // 
            // magacin_cmb
            // 
            magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            magacin_cmb.FormattingEnabled = true;
            magacin_cmb.Location = new System.Drawing.Point(82, 52);
            magacin_cmb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            magacin_cmb.Name = "magacin_cmb";
            magacin_cmb.Size = new System.Drawing.Size(293, 23);
            magacin_cmb.TabIndex = 1;
            magacin_cmb.SelectedIndexChanged += magacin_cmb_SelectedIndexChanged;
            // 
            // ppid_cmb
            // 
            ppid_cmb.Enabled = false;
            ppid_cmb.FormattingEnabled = true;
            ppid_cmb.Location = new System.Drawing.Point(82, 83);
            ppid_cmb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ppid_cmb.Name = "ppid_cmb";
            ppid_cmb.Size = new System.Drawing.Size(268, 23);
            ppid_cmb.TabIndex = 4;
            ppid_cmb.SelectedIndexChanged += ppid_cmb_SelectedIndexChanged;
            // 
            // pretvoriUMPRacun_btn
            // 
            pretvoriUMPRacun_btn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            pretvoriUMPRacun_btn.Location = new System.Drawing.Point(1076, 85);
            pretvoriUMPRacun_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pretvoriUMPRacun_btn.Name = "pretvoriUMPRacun_btn";
            pretvoriUMPRacun_btn.Size = new System.Drawing.Size(167, 27);
            pretvoriUMPRacun_btn.TabIndex = 6;
            pretvoriUMPRacun_btn.Text = "Pretvori U MP Racun";
            pretvoriUMPRacun_btn.UseVisualStyleBackColor = true;
            pretvoriUMPRacun_btn.Click += button1_Click;
            // 
            // brDokKomMPRacun_txt
            // 
            brDokKomMPRacun_txt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            brDokKomMPRacun_txt.BackColor = System.Drawing.SystemColors.Info;
            brDokKomMPRacun_txt.Location = new System.Drawing.Point(926, 88);
            brDokKomMPRacun_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            brDokKomMPRacun_txt.Name = "brDokKomMPRacun_txt";
            brDokKomMPRacun_txt.ReadOnly = true;
            brDokKomMPRacun_txt.Size = new System.Drawing.Size(138, 23);
            brDokKomMPRacun_txt.TabIndex = 7;
            brDokKomMPRacun_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.BackColor = System.Drawing.SystemColors.Control;
            panel1.Controls.Add(navigacija_gb);
            panel1.Controls.Add(dokument_gbx);
            panel1.Controls.Add(dataGridView1);
            panel1.Location = new System.Drawing.Point(7, 7);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1260, 699);
            panel1.TabIndex = 11;
            // 
            // navigacija_gb
            // 
            navigacija_gb.Controls.Add(help_btn);
            navigacija_gb.Controls.Add(navigacijaPrethodni_btn);
            navigacija_gb.Controls.Add(navigacijaSledeci_btn);
            navigacija_gb.Controls.Add(navigacijaIdiNa_txt);
            navigacija_gb.Location = new System.Drawing.Point(7, 7);
            navigacija_gb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            navigacija_gb.Name = "navigacija_gb";
            navigacija_gb.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            navigacija_gb.Size = new System.Drawing.Size(1242, 63);
            navigacija_gb.TabIndex = 14;
            navigacija_gb.TabStop = false;
            navigacija_gb.Text = "Navigacija";
            // 
            // help_btn
            // 
            help_btn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            help_btn.Location = new System.Drawing.Point(1148, 24);
            help_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            help_btn.Name = "help_btn";
            help_btn.Size = new System.Drawing.Size(88, 27);
            help_btn.TabIndex = 14;
            help_btn.Text = "HELP";
            help_btn.UseVisualStyleBackColor = true;
            help_btn.Click += help_btn_Click;
            // 
            // navigacijaPrethodni_btn
            // 
            navigacijaPrethodni_btn.Location = new System.Drawing.Point(16, 22);
            navigacijaPrethodni_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            navigacijaPrethodni_btn.Name = "navigacijaPrethodni_btn";
            navigacijaPrethodni_btn.Size = new System.Drawing.Size(88, 29);
            navigacijaPrethodni_btn.TabIndex = 16;
            navigacijaPrethodni_btn.Text = "Prethodni";
            navigacijaPrethodni_btn.UseVisualStyleBackColor = true;
            navigacijaPrethodni_btn.Click += navigacijaPrethodni_btn_Click;
            // 
            // navigacijaSledeci_btn
            // 
            navigacijaSledeci_btn.Location = new System.Drawing.Point(234, 22);
            navigacijaSledeci_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            navigacijaSledeci_btn.Name = "navigacijaSledeci_btn";
            navigacijaSledeci_btn.Size = new System.Drawing.Size(88, 29);
            navigacijaSledeci_btn.TabIndex = 15;
            navigacijaSledeci_btn.Text = "Sledeci";
            navigacijaSledeci_btn.UseVisualStyleBackColor = true;
            navigacijaSledeci_btn.Click += navigacijaSledeci_btn_Click;
            // 
            // navigacijaIdiNa_txt
            // 
            navigacijaIdiNa_txt.Location = new System.Drawing.Point(111, 25);
            navigacijaIdiNa_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            navigacijaIdiNa_txt.Name = "navigacijaIdiNa_txt";
            navigacijaIdiNa_txt.Size = new System.Drawing.Size(116, 23);
            navigacijaIdiNa_txt.TabIndex = 0;
            navigacijaIdiNa_txt.KeyDown += navigacijaIdiNa_txt_KeyDown;
            // 
            // dokument_gbx
            // 
            dokument_gbx.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dokument_gbx.ContextMenuStrip = contextMenuStrip2;
            dokument_gbx.Controls.Add(cmb_NacinPlacanja);
            dokument_gbx.Controls.Add(referent_txt);
            dokument_gbx.Controls.Add(brDokKomProracun_txt);
            dokument_gbx.Controls.Add(magacin_cmb);
            dokument_gbx.Controls.Add(ppid_cmb);
            dokument_gbx.Controls.Add(interniKomentar_btn);
            dokument_gbx.Controls.Add(klonirajUKomercijalno_btn);
            dokument_gbx.Controls.Add(komentar_btn);
            dokument_gbx.Controls.Add(dokumentStatus_btn);
            dokument_gbx.Controls.Add(label5);
            dokument_gbx.Controls.Add(brDokKomMPRacun_txt);
            dokument_gbx.Controls.Add(datum_txt);
            dokument_gbx.Controls.Add(brDok_lbl);
            dokument_gbx.Controls.Add(pretvoriUMPRacun_btn);
            dokument_gbx.Controls.Add(brojDokumenta_txt);
            dokument_gbx.Location = new System.Drawing.Point(7, 77);
            dokument_gbx.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dokument_gbx.Name = "dokument_gbx";
            dokument_gbx.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dokument_gbx.Size = new System.Drawing.Size(1246, 145);
            dokument_gbx.TabIndex = 13;
            dokument_gbx.TabStop = false;
            dokument_gbx.Text = "Dokument";
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new System.Drawing.Size(466, 26);
            // 
            // podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem
            // 
            podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem.Name = "podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem";
            podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem.Size = new System.Drawing.Size(465, 22);
            podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem.Text = "Podesi maksimalnu zastarelost proracuna prilikom pretvaranja u MP racun";
            podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem.Click += podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem_Click;
            // 
            // cmb_NacinPlacanja
            // 
            cmb_NacinPlacanja.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmb_NacinPlacanja.FormattingEnabled = true;
            cmb_NacinPlacanja.Location = new System.Drawing.Point(82, 114);
            cmb_NacinPlacanja.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmb_NacinPlacanja.Name = "cmb_NacinPlacanja";
            cmb_NacinPlacanja.Size = new System.Drawing.Size(268, 23);
            cmb_NacinPlacanja.TabIndex = 13;
            cmb_NacinPlacanja.SelectedIndexChanged += cmb_NacinPlacanja_SelectedIndexChanged;
            // 
            // referent_txt
            // 
            referent_txt.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            referent_txt.Location = new System.Drawing.Point(309, 22);
            referent_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            referent_txt.Name = "referent_txt";
            referent_txt.ReadOnly = true;
            referent_txt.Size = new System.Drawing.Size(177, 23);
            referent_txt.TabIndex = 8;
            referent_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // brDokKomProracun_txt
            // 
            brDokKomProracun_txt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            brDokKomProracun_txt.BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
            brDokKomProracun_txt.Location = new System.Drawing.Point(926, 55);
            brDokKomProracun_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            brDokKomProracun_txt.Name = "brDokKomProracun_txt";
            brDokKomProracun_txt.ReadOnly = true;
            brDokKomProracun_txt.Size = new System.Drawing.Size(138, 23);
            brDokKomProracun_txt.TabIndex = 12;
            brDokKomProracun_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // interniKomentar_btn
            // 
            interniKomentar_btn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            interniKomentar_btn.Location = new System.Drawing.Point(982, 20);
            interniKomentar_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            interniKomentar_btn.Name = "interniKomentar_btn";
            interniKomentar_btn.Size = new System.Drawing.Size(122, 27);
            interniKomentar_btn.TabIndex = 7;
            interniKomentar_btn.Text = "Interni Komentar";
            interniKomentar_btn.UseVisualStyleBackColor = true;
            interniKomentar_btn.Click += interniKomentar_btn_Click_1;
            // 
            // klonirajUKomercijalno_btn
            // 
            klonirajUKomercijalno_btn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            klonirajUKomercijalno_btn.Location = new System.Drawing.Point(1072, 52);
            klonirajUKomercijalno_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            klonirajUKomercijalno_btn.Name = "klonirajUKomercijalno_btn";
            klonirajUKomercijalno_btn.Size = new System.Drawing.Size(167, 27);
            klonirajUKomercijalno_btn.TabIndex = 11;
            klonirajUKomercijalno_btn.Text = "Prebaci u komercijalno";
            klonirajUKomercijalno_btn.UseVisualStyleBackColor = true;
            klonirajUKomercijalno_btn.Click += klonirajUKomercijalno_btn_Click;
            // 
            // komentar_btn
            // 
            komentar_btn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            komentar_btn.Location = new System.Drawing.Point(888, 20);
            komentar_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            komentar_btn.Name = "komentar_btn";
            komentar_btn.Size = new System.Drawing.Size(88, 27);
            komentar_btn.TabIndex = 6;
            komentar_btn.Text = "Komentar";
            komentar_btn.UseVisualStyleBackColor = true;
            komentar_btn.Click += komentar_btn_Click;
            // 
            // dokumentStatus_btn
            // 
            dokumentStatus_btn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            dokumentStatus_btn.Location = new System.Drawing.Point(1152, 20);
            dokumentStatus_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dokumentStatus_btn.Name = "dokumentStatus_btn";
            dokumentStatus_btn.Size = new System.Drawing.Size(88, 27);
            dokumentStatus_btn.TabIndex = 5;
            dokumentStatus_btn.Text = "Otkljucaj";
            dokumentStatus_btn.UseVisualStyleBackColor = true;
            dokumentStatus_btn.Click += dokumentStatus_btn_Click_1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(7, 55);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(56, 15);
            label5.TabIndex = 4;
            label5.Text = "Magacin:";
            // 
            // datum_txt
            // 
            datum_txt.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            datum_txt.Location = new System.Drawing.Point(167, 22);
            datum_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            datum_txt.Name = "datum_txt";
            datum_txt.ReadOnly = true;
            datum_txt.Size = new System.Drawing.Size(135, 23);
            datum_txt.TabIndex = 2;
            datum_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // brDok_lbl
            // 
            brDok_lbl.AutoSize = true;
            brDok_lbl.Location = new System.Drawing.Point(7, 25);
            brDok_lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            brDok_lbl.Name = "brDok_lbl";
            brDok_lbl.Size = new System.Drawing.Size(48, 15);
            brDok_lbl.TabIndex = 1;
            brDok_lbl.Text = "Br. Dok.";
            // 
            // brojDokumenta_txt
            // 
            brojDokumenta_txt.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            brojDokumenta_txt.Location = new System.Drawing.Point(68, 22);
            brojDokumenta_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            brojDokumenta_txt.Name = "brojDokumenta_txt";
            brojDokumenta_txt.ReadOnly = true;
            brojDokumenta_txt.Size = new System.Drawing.Size(92, 23);
            brojDokumenta_txt.TabIndex = 0;
            brojDokumenta_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // istorijaNabavkeToolStripMenuItem
            // 
            istorijaNabavkeToolStripMenuItem.Name = "istorijaNabavkeToolStripMenuItem";
            istorijaNabavkeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            istorijaNabavkeToolStripMenuItem.Text = "Istorija Nabavke";
            istorijaNabavkeToolStripMenuItem.Click += istorijaNabavkeToolStripMenuItem_Click;
            // 
            // _1332_fm_Proracun_Index
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Red;
            ClientSize = new System.Drawing.Size(1274, 713);
            Controls.Add(panel1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "_1332_fm_Proracun_Index";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "_1332_fm_PredlogProracuna_Index";
            Load += _1332_fm_PredlogProracuna_Index_LoadAsync;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            navigacija_gb.ResumeLayout(false);
            navigacija_gb.PerformLayout();
            dokument_gbx.ResumeLayout(false);
            dokument_gbx.PerformLayout();
            contextMenuStrip2.ResumeLayout(false);
            ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem kartcaRobeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem istorijaNabavkeToolStripMenuItem;
    }
}