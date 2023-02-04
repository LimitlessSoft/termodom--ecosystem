
namespace TDOffice_v2
{
    partial class _1337_fm_Korisnici_Index
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
            this.id_txt = new System.Windows.Forms.TextBox();
            this.usernam_txt = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.robaPopisPPIDNarudbeniceSacuvaj_btn = new System.Windows.Forms.Button();
            this.robaPopisPPIDNarudzbenice_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dodeliTask_btn = new System.Windows.Forms.Button();
            this.taskovi_dgv = new System.Windows.Forms.DataGridView();
            this.taskovi_cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ukloniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.komercijalnoNalogID_txt = new System.Windows.Forms.TextBox();
            this.komercijalnoNalogIDSacuvaj_btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clb_PrimalacObavestenja = new System.Windows.Forms.CheckedListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.grad_cmb = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.resetujSifru_txt = new System.Windows.Forms.Button();
            this.pretragaPrava_txt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.help_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskovi_dgv)).BeginInit();
            this.taskovi_cms.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // id_txt
            // 
            this.id_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.id_txt.Location = new System.Drawing.Point(42, 19);
            this.id_txt.Name = "id_txt";
            this.id_txt.ReadOnly = true;
            this.id_txt.Size = new System.Drawing.Size(88, 20);
            this.id_txt.TabIndex = 0;
            // 
            // usernam_txt
            // 
            this.usernam_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.usernam_txt.Location = new System.Drawing.Point(68, 45);
            this.usernam_txt.Name = "usernam_txt";
            this.usernam_txt.ReadOnly = true;
            this.usernam_txt.Size = new System.Drawing.Size(247, 20);
            this.usernam_txt.TabIndex = 1;
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 198);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1408, 347);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.dataGridView1.Sorted += new System.EventHandler(this.dataGridView1_Sorted);
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(76, 71);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(185, 21);
            this.magacin_cmb.TabIndex = 3;
            this.magacin_cmb.SelectedIndexChanged += new System.EventHandler(this.magacin_cmb_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.robaPopisPPIDNarudbeniceSacuvaj_btn);
            this.tabPage1.Controls.Add(this.robaPopisPPIDNarudzbenice_txt);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1400, 122);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Roba > Popis";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // robaPopisPPIDNarudbeniceSacuvaj_btn
            // 
            this.robaPopisPPIDNarudbeniceSacuvaj_btn.Location = new System.Drawing.Point(237, 6);
            this.robaPopisPPIDNarudbeniceSacuvaj_btn.Name = "robaPopisPPIDNarudbeniceSacuvaj_btn";
            this.robaPopisPPIDNarudbeniceSacuvaj_btn.Size = new System.Drawing.Size(68, 20);
            this.robaPopisPPIDNarudbeniceSacuvaj_btn.TabIndex = 6;
            this.robaPopisPPIDNarudbeniceSacuvaj_btn.Text = "sacuvaj";
            this.robaPopisPPIDNarudbeniceSacuvaj_btn.UseVisualStyleBackColor = true;
            this.robaPopisPPIDNarudbeniceSacuvaj_btn.Visible = false;
            this.robaPopisPPIDNarudbeniceSacuvaj_btn.Click += new System.EventHandler(this.robaPopisPPIDNarudbeniceSacuvaj_btn_Click);
            // 
            // robaPopisPPIDNarudzbenice_txt
            // 
            this.robaPopisPPIDNarudzbenice_txt.BackColor = System.Drawing.SystemColors.Window;
            this.robaPopisPPIDNarudzbenice_txt.Location = new System.Drawing.Point(114, 6);
            this.robaPopisPPIDNarudzbenice_txt.Name = "robaPopisPPIDNarudzbenice_txt";
            this.robaPopisPPIDNarudzbenice_txt.Size = new System.Drawing.Size(117, 20);
            this.robaPopisPPIDNarudzbenice_txt.TabIndex = 5;
            this.robaPopisPPIDNarudzbenice_txt.TextChanged += new System.EventHandler(this.robaPopisPPIDNarudzbenice_txt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "PPID narudzbenice:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 551);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1408, 148);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dodeliTask_btn);
            this.tabPage2.Controls.Add(this.taskovi_dgv);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1400, 122);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Taskovi";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dodeliTask_btn
            // 
            this.dodeliTask_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dodeliTask_btn.Location = new System.Drawing.Point(1273, 6);
            this.dodeliTask_btn.Name = "dodeliTask_btn";
            this.dodeliTask_btn.Size = new System.Drawing.Size(121, 110);
            this.dodeliTask_btn.TabIndex = 1;
            this.dodeliTask_btn.Text = "Novi Task";
            this.dodeliTask_btn.UseVisualStyleBackColor = true;
            this.dodeliTask_btn.Click += new System.EventHandler(this.dodeliTask_btn_Click);
            // 
            // taskovi_dgv
            // 
            this.taskovi_dgv.AllowUserToAddRows = false;
            this.taskovi_dgv.AllowUserToDeleteRows = false;
            this.taskovi_dgv.AllowUserToResizeRows = false;
            this.taskovi_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskovi_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.taskovi_dgv.ContextMenuStrip = this.taskovi_cms;
            this.taskovi_dgv.Location = new System.Drawing.Point(6, 6);
            this.taskovi_dgv.Name = "taskovi_dgv";
            this.taskovi_dgv.RowHeadersVisible = false;
            this.taskovi_dgv.RowHeadersWidth = 51;
            this.taskovi_dgv.Size = new System.Drawing.Size(1261, 110);
            this.taskovi_dgv.TabIndex = 0;
            this.taskovi_dgv.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.taskovi_dgv_CellValidating);
            // 
            // taskovi_cms
            // 
            this.taskovi_cms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.taskovi_cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ukloniToolStripMenuItem});
            this.taskovi_cms.Name = "taskovi_cms";
            this.taskovi_cms.Size = new System.Drawing.Size(109, 26);
            // 
            // ukloniToolStripMenuItem
            // 
            this.ukloniToolStripMenuItem.Name = "ukloniToolStripMenuItem";
            this.ukloniToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.ukloniToolStripMenuItem.Text = "Ukloni";
            this.ukloniToolStripMenuItem.Click += new System.EventHandler(this.ukloniToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Korisnik:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Magacin:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Komercijalno Nalog ID:";
            // 
            // komercijalnoNalogID_txt
            // 
            this.komercijalnoNalogID_txt.BackColor = System.Drawing.SystemColors.Window;
            this.komercijalnoNalogID_txt.Location = new System.Drawing.Point(140, 126);
            this.komercijalnoNalogID_txt.Name = "komercijalnoNalogID_txt";
            this.komercijalnoNalogID_txt.Size = new System.Drawing.Size(117, 20);
            this.komercijalnoNalogID_txt.TabIndex = 10;
            this.komercijalnoNalogID_txt.TextChanged += new System.EventHandler(this.komercijalnoNalogID_txt_TextChanged);
            // 
            // komercijalnoNalogIDSacuvaj_btn
            // 
            this.komercijalnoNalogIDSacuvaj_btn.Location = new System.Drawing.Point(342, 19);
            this.komercijalnoNalogIDSacuvaj_btn.Name = "komercijalnoNalogIDSacuvaj_btn";
            this.komercijalnoNalogIDSacuvaj_btn.Size = new System.Drawing.Size(108, 95);
            this.komercijalnoNalogIDSacuvaj_btn.TabIndex = 11;
            this.komercijalnoNalogIDSacuvaj_btn.Text = "sacuvaj";
            this.komercijalnoNalogIDSacuvaj_btn.UseVisualStyleBackColor = true;
            this.komercijalnoNalogIDSacuvaj_btn.Visible = false;
            this.komercijalnoNalogIDSacuvaj_btn.Click += new System.EventHandler(this.komercijalnoNalogIDSacuvaj_btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.clb_PrimalacObavestenja);
            this.groupBox1.Location = new System.Drawing.Point(474, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(829, 155);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Prima obavestenja:";
            // 
            // clb_PrimalacObavestenja
            // 
            this.clb_PrimalacObavestenja.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clb_PrimalacObavestenja.CheckOnClick = true;
            this.clb_PrimalacObavestenja.ColumnWidth = 600;
            this.clb_PrimalacObavestenja.FormattingEnabled = true;
            this.clb_PrimalacObavestenja.Location = new System.Drawing.Point(6, 28);
            this.clb_PrimalacObavestenja.MultiColumn = true;
            this.clb_PrimalacObavestenja.Name = "clb_PrimalacObavestenja";
            this.clb_PrimalacObavestenja.Size = new System.Drawing.Size(817, 109);
            this.clb_PrimalacObavestenja.TabIndex = 1;
            this.clb_PrimalacObavestenja.SelectedIndexChanged += new System.EventHandler(this.clb_PrimalacObavestenja_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Grad:";
            // 
            // grad_cmb
            // 
            this.grad_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.grad_cmb.FormattingEnabled = true;
            this.grad_cmb.Location = new System.Drawing.Point(58, 98);
            this.grad_cmb.Name = "grad_cmb";
            this.grad_cmb.Size = new System.Drawing.Size(203, 21);
            this.grad_cmb.TabIndex = 13;
            this.grad_cmb.SelectedIndexChanged += new System.EventHandler(this.grad_cmb_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.resetujSifru_txt);
            this.groupBox2.Controls.Add(this.id_txt);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.usernam_txt);
            this.groupBox2.Controls.Add(this.grad_cmb);
            this.groupBox2.Controls.Add(this.magacin_cmb);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.komercijalnoNalogIDSacuvaj_btn);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.komercijalnoNalogID_txt);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 155);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // resetujSifru_txt
            // 
            this.resetujSifru_txt.Location = new System.Drawing.Point(342, 120);
            this.resetujSifru_txt.Name = "resetujSifru_txt";
            this.resetujSifru_txt.Size = new System.Drawing.Size(108, 29);
            this.resetujSifru_txt.TabIndex = 15;
            this.resetujSifru_txt.Text = "Resetuj Sifru";
            this.resetujSifru_txt.UseVisualStyleBackColor = true;
            this.resetujSifru_txt.Click += new System.EventHandler(this.resetujSifru_txt_Click);
            // 
            // pretragaPrava_txt
            // 
            this.pretragaPrava_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pretragaPrava_txt.Location = new System.Drawing.Point(1185, 173);
            this.pretragaPrava_txt.Name = "pretragaPrava_txt";
            this.pretragaPrava_txt.Size = new System.Drawing.Size(235, 20);
            this.pretragaPrava_txt.TabIndex = 16;
            this.pretragaPrava_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pretragaPrava_txt_KeyDown);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1101, 176);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Pretraga Prava";
            // 
            // help_btn
            // 
            this.help_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.help_btn.Location = new System.Drawing.Point(1312, 12);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(108, 29);
            this.help_btn.TabIndex = 16;
            this.help_btn.Text = "HELP";
            this.help_btn.UseVisualStyleBackColor = true;
            this.help_btn.Click += new System.EventHandler(this.help_btn_Click);
            // 
            // _1337_fm_Korisnici_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1432, 711);
            this.Controls.Add(this.help_btn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pretragaPrava_txt);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "_1337_fm_Korisnici_Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_1337_fm_Korisnici_Index";
            this.Load += new System.EventHandler(this._1337_fm_Korisnici_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.taskovi_dgv)).EndInit();
            this.taskovi_cms.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox id_txt;
        private System.Windows.Forms.TextBox usernam_txt;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button robaPopisPPIDNarudbeniceSacuvaj_btn;
        private System.Windows.Forms.TextBox robaPopisPPIDNarudzbenice_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox komercijalnoNalogID_txt;
        private System.Windows.Forms.Button komercijalnoNalogIDSacuvaj_btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox clb_PrimalacObavestenja;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView taskovi_dgv;
        private System.Windows.Forms.Button dodeliTask_btn;
        private System.Windows.Forms.ContextMenuStrip taskovi_cms;
        private System.Windows.Forms.ToolStripMenuItem ukloniToolStripMenuItem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox grad_cmb;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox pretragaPrava_txt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button resetujSifru_txt;
        private System.Windows.Forms.Button help_btn;
    }
}