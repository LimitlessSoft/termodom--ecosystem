namespace TDOffice_v2
{
    partial class fm_MojKupac_Index
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.uvrstiNovogKupcaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.detaljnaAnaliza_btn = new System.Windows.Forms.Button();
            this.cagr_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.prvaKupovina_lbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.prometUOdnosuNaPrethodnuGodinu_txt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ocekivanPromet_txt = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.beleska_txt = new System.Windows.Forms.RichTextBox();
            this.sacuvajBelesku_txt = new System.Windows.Forms.Button();
            this.korisnik_cmb = new System.Windows.Forms.ComboBox();
            this.ukloniKupcaIzMojeListeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prometZaTrenutniPeriod_txt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.prosecniMesecniPromet_txt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.brojRacunaOVeGodine_txt = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(922, 536);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ukloniKupcaIzMojeListeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(209, 26);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uvrstiNovogKupcaToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1279, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // uvrstiNovogKupcaToolStripMenuItem1
            // 
            this.uvrstiNovogKupcaToolStripMenuItem1.Name = "uvrstiNovogKupcaToolStripMenuItem1";
            this.uvrstiNovogKupcaToolStripMenuItem1.Size = new System.Drawing.Size(124, 20);
            this.uvrstiNovogKupcaToolStripMenuItem1.Text = "Uvrsti Novog Kupca";
            this.uvrstiNovogKupcaToolStripMenuItem1.Click += new System.EventHandler(this.uvrstiNovogKupcaToolStripMenuItem1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(12, 39);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sacuvajBelesku_txt);
            this.splitContainer1.Panel2.Controls.Add(this.beleska_txt);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.detaljnaAnaliza_btn);
            this.splitContainer1.Size = new System.Drawing.Size(1255, 538);
            this.splitContainer1.SplitterDistance = 924;
            this.splitContainer1.TabIndex = 3;
            // 
            // detaljnaAnaliza_btn
            // 
            this.detaljnaAnaliza_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detaljnaAnaliza_btn.Location = new System.Drawing.Point(3, 258);
            this.detaljnaAnaliza_btn.Name = "detaljnaAnaliza_btn";
            this.detaljnaAnaliza_btn.Size = new System.Drawing.Size(319, 42);
            this.detaljnaAnaliza_btn.TabIndex = 0;
            this.detaljnaAnaliza_btn.Text = "Detaljna Analiza";
            this.detaljnaAnaliza_btn.UseVisualStyleBackColor = true;
            this.detaljnaAnaliza_btn.Click += new System.EventHandler(this.detaljnaAnaliza_btn_Click);
            // 
            // cagr_txt
            // 
            this.cagr_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cagr_txt.Location = new System.Drawing.Point(6, 219);
            this.cagr_txt.Name = "cagr_txt";
            this.cagr_txt.ReadOnly = true;
            this.cagr_txt.Size = new System.Drawing.Size(48, 20);
            this.cagr_txt.TabIndex = 1;
            this.cagr_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 203);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "CAGR:";
            // 
            // prvaKupovina_lbl
            // 
            this.prvaKupovina_lbl.AutoSize = true;
            this.prvaKupovina_lbl.Location = new System.Drawing.Point(6, 182);
            this.prvaKupovina_lbl.Name = "prvaKupovina_lbl";
            this.prvaKupovina_lbl.Size = new System.Drawing.Size(139, 13);
            this.prvaKupovina_lbl.TabIndex = 4;
            this.prvaKupovina_lbl.Text = "Prva kupovina pre X godina";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(255, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Promet u odnosu na prethodnu godinu (cela godina):";
            // 
            // prometUOdnosuNaPrethodnuGodinu_txt
            // 
            this.prometUOdnosuNaPrethodnuGodinu_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.prometUOdnosuNaPrethodnuGodinu_txt.Location = new System.Drawing.Point(6, 76);
            this.prometUOdnosuNaPrethodnuGodinu_txt.Name = "prometUOdnosuNaPrethodnuGodinu_txt";
            this.prometUOdnosuNaPrethodnuGodinu_txt.ReadOnly = true;
            this.prometUOdnosuNaPrethodnuGodinu_txt.Size = new System.Drawing.Size(48, 20);
            this.prometUOdnosuNaPrethodnuGodinu_txt.TabIndex = 5;
            this.prometUOdnosuNaPrethodnuGodinu_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.brojRacunaOVeGodine_txt);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.prosecniMesecniPromet_txt);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.prometZaTrenutniPeriod_txt);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ocekivanPromet_txt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.prometUOdnosuNaPrethodnuGodinu_txt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cagr_txt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.prvaKupovina_lbl);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(317, 249);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Detaljna analiza";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Ocekivano:";
            // 
            // ocekivanPromet_txt
            // 
            this.ocekivanPromet_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ocekivanPromet_txt.Location = new System.Drawing.Point(161, 76);
            this.ocekivanPromet_txt.Name = "ocekivanPromet_txt";
            this.ocekivanPromet_txt.ReadOnly = true;
            this.ocekivanPromet_txt.Size = new System.Drawing.Size(48, 20);
            this.ocekivanPromet_txt.TabIndex = 8;
            this.ocekivanPromet_txt.Text = "0%";
            this.ocekivanPromet_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox1.Location = new System.Drawing.Point(161, 219);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(48, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "0%";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(93, 222);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Ocekivano:";
            // 
            // beleska_txt
            // 
            this.beleska_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.beleska_txt.Location = new System.Drawing.Point(5, 306);
            this.beleska_txt.Name = "beleska_txt";
            this.beleska_txt.Size = new System.Drawing.Size(317, 198);
            this.beleska_txt.TabIndex = 8;
            this.beleska_txt.Text = "";
            // 
            // sacuvajBelesku_txt
            // 
            this.sacuvajBelesku_txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sacuvajBelesku_txt.Location = new System.Drawing.Point(3, 510);
            this.sacuvajBelesku_txt.Name = "sacuvajBelesku_txt";
            this.sacuvajBelesku_txt.Size = new System.Drawing.Size(319, 23);
            this.sacuvajBelesku_txt.TabIndex = 9;
            this.sacuvajBelesku_txt.Text = "Sacuvaj Belesku";
            this.sacuvajBelesku_txt.UseVisualStyleBackColor = true;
            this.sacuvajBelesku_txt.Click += new System.EventHandler(this.sacuvajBelesku_txt_Click);
            // 
            // korisnik_cmb
            // 
            this.korisnik_cmb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.korisnik_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.korisnik_cmb.Enabled = false;
            this.korisnik_cmb.FormattingEnabled = true;
            this.korisnik_cmb.Location = new System.Drawing.Point(997, 12);
            this.korisnik_cmb.Name = "korisnik_cmb";
            this.korisnik_cmb.Size = new System.Drawing.Size(270, 21);
            this.korisnik_cmb.TabIndex = 4;
            this.korisnik_cmb.SelectedIndexChanged += new System.EventHandler(this.korisnik_cmb_SelectedIndexChanged);
            // 
            // ukloniKupcaIzMojeListeToolStripMenuItem
            // 
            this.ukloniKupcaIzMojeListeToolStripMenuItem.Name = "ukloniKupcaIzMojeListeToolStripMenuItem";
            this.ukloniKupcaIzMojeListeToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.ukloniKupcaIzMojeListeToolStripMenuItem.Text = "Ukloni kupca iz moje liste";
            this.ukloniKupcaIzMojeListeToolStripMenuItem.Click += new System.EventHandler(this.ukloniKupcaIzMojeListeToolStripMenuItem_Click);
            // 
            // prometZaTrenutniPeriod_txt
            // 
            this.prometZaTrenutniPeriod_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.prometZaTrenutniPeriod_txt.Location = new System.Drawing.Point(6, 37);
            this.prometZaTrenutniPeriod_txt.Name = "prometZaTrenutniPeriod_txt";
            this.prometZaTrenutniPeriod_txt.ReadOnly = true;
            this.prometZaTrenutniPeriod_txt.Size = new System.Drawing.Size(93, 20);
            this.prometZaTrenutniPeriod_txt.TabIndex = 11;
            this.prometZaTrenutniPeriod_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(276, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Promet u odnosu na prethodnu godinu (period do danas):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Prosecni Mesecni Promet:";
            // 
            // prosecniMesecniPromet_txt
            // 
            this.prosecniMesecniPromet_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.prosecniMesecniPromet_txt.Location = new System.Drawing.Point(6, 120);
            this.prosecniMesecniPromet_txt.Name = "prosecniMesecniPromet_txt";
            this.prosecniMesecniPromet_txt.ReadOnly = true;
            this.prosecniMesecniPromet_txt.Size = new System.Drawing.Size(93, 20);
            this.prosecniMesecniPromet_txt.TabIndex = 15;
            this.prosecniMesecniPromet_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Broj Racuna Ove Godine:";
            // 
            // brojRacunaOVeGodine_txt
            // 
            this.brojRacunaOVeGodine_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.brojRacunaOVeGodine_txt.Location = new System.Drawing.Point(136, 149);
            this.brojRacunaOVeGodine_txt.Name = "brojRacunaOVeGodine_txt";
            this.brojRacunaOVeGodine_txt.ReadOnly = true;
            this.brojRacunaOVeGodine_txt.Size = new System.Drawing.Size(93, 20);
            this.brojRacunaOVeGodine_txt.TabIndex = 17;
            this.brojRacunaOVeGodine_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // fm_MojKupac_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 589);
            this.Controls.Add(this.korisnik_cmb);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "fm_MojKupac_Index";
            this.Text = "Moj Kupac";
            this.Load += new System.EventHandler(this.fm_MojKupac_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem uvrstiNovogKupcaToolStripMenuItem1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button detaljnaAnaliza_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox cagr_txt;
        private System.Windows.Forms.Label prvaKupovina_lbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox prometUOdnosuNaPrethodnuGodinu_txt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ocekivanPromet_txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button sacuvajBelesku_txt;
        private System.Windows.Forms.RichTextBox beleska_txt;
        private System.Windows.Forms.ComboBox korisnik_cmb;
        private System.Windows.Forms.ToolStripMenuItem ukloniKupcaIzMojeListeToolStripMenuItem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox prometZaTrenutniPeriod_txt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox prosecniMesecniPromet_txt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox brojRacunaOVeGodine_txt;
    }
}