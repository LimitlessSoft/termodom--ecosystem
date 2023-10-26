
namespace TDOffice_v2
{
    partial class fm_Magacin_Index
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
            this.naziv_txt = new System.Windows.Forms.TextBox();
            this.vlasnik_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.korisnici_dgv = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ukloniKorisnikaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noviKorisnik_btn = new System.Windows.Forms.Button();
            this.korisnici_gb = new System.Windows.Forms.GroupBox();
            this.noviKorisnik_cmb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nadredjeniMagacin_cmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.magacinRazduzenja_cmb = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.korisnici_dgv)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.korisnici_gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // id_txt
            // 
            this.id_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.id_txt.Location = new System.Drawing.Point(12, 12);
            this.id_txt.Name = "id_txt";
            this.id_txt.ReadOnly = true;
            this.id_txt.Size = new System.Drawing.Size(56, 20);
            this.id_txt.TabIndex = 0;
            // 
            // naziv_txt
            // 
            this.naziv_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.naziv_txt.Location = new System.Drawing.Point(74, 12);
            this.naziv_txt.Name = "naziv_txt";
            this.naziv_txt.ReadOnly = true;
            this.naziv_txt.Size = new System.Drawing.Size(275, 20);
            this.naziv_txt.TabIndex = 1;
            // 
            // vlasnik_cmb
            // 
            this.vlasnik_cmb.Enabled = false;
            this.vlasnik_cmb.FormattingEnabled = true;
            this.vlasnik_cmb.Location = new System.Drawing.Point(62, 38);
            this.vlasnik_cmb.Name = "vlasnik_cmb";
            this.vlasnik_cmb.Size = new System.Drawing.Size(237, 21);
            this.vlasnik_cmb.TabIndex = 2;
            this.vlasnik_cmb.SelectedIndexChanged += new System.EventHandler(this.vlasnik_cmb_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Vlasnik:";
            // 
            // korisnici_dgv
            // 
            this.korisnici_dgv.AllowUserToAddRows = false;
            this.korisnici_dgv.AllowUserToDeleteRows = false;
            this.korisnici_dgv.AllowUserToResizeRows = false;
            this.korisnici_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.korisnici_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.korisnici_dgv.ContextMenuStrip = this.contextMenuStrip1;
            this.korisnici_dgv.Location = new System.Drawing.Point(6, 48);
            this.korisnici_dgv.Name = "korisnici_dgv";
            this.korisnici_dgv.ReadOnly = true;
            this.korisnici_dgv.RowHeadersVisible = false;
            this.korisnici_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.korisnici_dgv.Size = new System.Drawing.Size(557, 250);
            this.korisnici_dgv.TabIndex = 4;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ukloniKorisnikaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 26);
            // 
            // ukloniKorisnikaToolStripMenuItem
            // 
            this.ukloniKorisnikaToolStripMenuItem.Name = "ukloniKorisnikaToolStripMenuItem";
            this.ukloniKorisnikaToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.ukloniKorisnikaToolStripMenuItem.Text = "Ukloni korisnika";
            this.ukloniKorisnikaToolStripMenuItem.Click += new System.EventHandler(this.ukloniKorisnikaToolStripMenuItem_Click);
            // 
            // noviKorisnik_btn
            // 
            this.noviKorisnik_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.noviKorisnik_btn.Location = new System.Drawing.Point(465, 19);
            this.noviKorisnik_btn.Name = "noviKorisnik_btn";
            this.noviKorisnik_btn.Size = new System.Drawing.Size(98, 23);
            this.noviKorisnik_btn.TabIndex = 42;
            this.noviKorisnik_btn.Text = "Dodaj korisnika";
            this.noviKorisnik_btn.UseVisualStyleBackColor = true;
            this.noviKorisnik_btn.Click += new System.EventHandler(this.noviKorisnik_btn_Click);
            // 
            // korisnici_gb
            // 
            this.korisnici_gb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.korisnici_gb.Controls.Add(this.noviKorisnik_cmb);
            this.korisnici_gb.Controls.Add(this.noviKorisnik_btn);
            this.korisnici_gb.Controls.Add(this.korisnici_dgv);
            this.korisnici_gb.Location = new System.Drawing.Point(12, 122);
            this.korisnici_gb.Name = "korisnici_gb";
            this.korisnici_gb.Size = new System.Drawing.Size(569, 304);
            this.korisnici_gb.TabIndex = 43;
            this.korisnici_gb.TabStop = false;
            this.korisnici_gb.Text = "Komercijalno korisnici magacina";
            // 
            // noviKorisnik_cmb
            // 
            this.noviKorisnik_cmb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.noviKorisnik_cmb.Enabled = false;
            this.noviKorisnik_cmb.FormattingEnabled = true;
            this.noviKorisnik_cmb.Location = new System.Drawing.Point(222, 21);
            this.noviKorisnik_cmb.Name = "noviKorisnik_cmb";
            this.noviKorisnik_cmb.Size = new System.Drawing.Size(237, 21);
            this.noviKorisnik_cmb.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "Nadredjeni Magacin:";
            // 
            // nadredjeniMagacin_cmb
            // 
            this.nadredjeniMagacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nadredjeniMagacin_cmb.Enabled = false;
            this.nadredjeniMagacin_cmb.FormattingEnabled = true;
            this.nadredjeniMagacin_cmb.Location = new System.Drawing.Point(123, 65);
            this.nadredjeniMagacin_cmb.Name = "nadredjeniMagacin_cmb";
            this.nadredjeniMagacin_cmb.Size = new System.Drawing.Size(237, 21);
            this.nadredjeniMagacin_cmb.TabIndex = 44;
            this.nadredjeniMagacin_cmb.SelectedIndexChanged += new System.EventHandler(this.nadredjeniMagacin_cmb_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "Magacin Razduzenja:";
            // 
            // magacinRazduzenja_cmb
            // 
            this.magacinRazduzenja_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacinRazduzenja_cmb.Enabled = false;
            this.magacinRazduzenja_cmb.FormattingEnabled = true;
            this.magacinRazduzenja_cmb.Location = new System.Drawing.Point(128, 92);
            this.magacinRazduzenja_cmb.Name = "magacinRazduzenja_cmb";
            this.magacinRazduzenja_cmb.Size = new System.Drawing.Size(260, 21);
            this.magacinRazduzenja_cmb.TabIndex = 46;
            this.magacinRazduzenja_cmb.SelectedIndexChanged += new System.EventHandler(this.magacinRazduzenja_cmb_SelectedIndexChanged);
            // 
            // fm_Magacin_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 438);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.magacinRazduzenja_cmb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nadredjeniMagacin_cmb);
            this.Controls.Add(this.korisnici_gb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vlasnik_cmb);
            this.Controls.Add(this.naziv_txt);
            this.Controls.Add(this.id_txt);
            this.MinimumSize = new System.Drawing.Size(434, 477);
            this.Name = "fm_Magacin_Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "fm_Magacin";
            this.Load += new System.EventHandler(this.fm_Magacin_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.korisnici_dgv)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.korisnici_gb.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox id_txt;
        private System.Windows.Forms.TextBox naziv_txt;
        private System.Windows.Forms.ComboBox vlasnik_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView korisnici_dgv;
        private System.Windows.Forms.Button noviKorisnik_btn;
        private System.Windows.Forms.GroupBox korisnici_gb;
        private System.Windows.Forms.ComboBox noviKorisnik_cmb;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ukloniKorisnikaToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox nadredjeniMagacin_cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox magacinRazduzenja_cmb;
    }
}