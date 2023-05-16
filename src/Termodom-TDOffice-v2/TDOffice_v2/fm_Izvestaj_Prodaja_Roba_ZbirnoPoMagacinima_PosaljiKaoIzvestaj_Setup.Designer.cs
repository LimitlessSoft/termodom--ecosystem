
namespace TDOffice_v2
{
    partial class fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima_PosaljiKaoIzvestaj_Setup
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
            this.korisnicima_clb = new System.Windows.Forms.CheckedListBox();
            this.posalji_btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_ReperniMagacini = new System.Windows.Forms.Button();
            this.uIzvestajUbaciVrednosti_cb = new System.Windows.Forms.CheckBox();
            this.uIzvestajUbaciKolicine_btn = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.komentar_rtb = new System.Windows.Forms.RichTextBox();
            this.korisnicimaMagacina_gb = new System.Windows.Forms.GroupBox();
            this.izvestajKakavVIdim_rb = new System.Windows.Forms.RadioButton();
            this.korisnicimaMagacinaCeoIzvestaj_rb = new System.Windows.Forms.RadioButton();
            this.korisnicimaMagacinaSamoSvojePodatke_rb = new System.Windows.Forms.RadioButton();
            this.tb_NaslovIzvestaja = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.status_lbl = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.korisnicimaMagacina_gb.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // korisnicima_clb
            // 
            this.korisnicima_clb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.korisnicima_clb.CheckOnClick = true;
            this.korisnicima_clb.ContextMenuStrip = this.contextMenuStrip1;
            this.korisnicima_clb.FormattingEnabled = true;
            this.korisnicima_clb.Location = new System.Drawing.Point(12, 107);
            this.korisnicima_clb.Name = "korisnicima_clb";
            this.korisnicima_clb.Size = new System.Drawing.Size(610, 289);
            this.korisnicima_clb.TabIndex = 10;
            // 
            // posalji_btn
            // 
            this.posalji_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.posalji_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posalji_btn.Location = new System.Drawing.Point(133, 599);
            this.posalji_btn.Name = "posalji_btn";
            this.posalji_btn.Size = new System.Drawing.Size(479, 51);
            this.posalji_btn.TabIndex = 11;
            this.posalji_btn.Text = "Posalji";
            this.posalji_btn.UseVisualStyleBackColor = true;
            this.posalji_btn.Click += new System.EventHandler(this.posalji_btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btn_ReperniMagacini);
            this.groupBox1.Controls.Add(this.uIzvestajUbaciVrednosti_cb);
            this.groupBox1.Controls.Add(this.uIzvestajUbaciKolicine_btn);
            this.groupBox1.Location = new System.Drawing.Point(249, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 89);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "U izvestaj ubaci";
            // 
            // btn_ReperniMagacini
            // 
            this.btn_ReperniMagacini.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ReperniMagacini.Location = new System.Drawing.Point(168, 19);
            this.btn_ReperniMagacini.Name = "btn_ReperniMagacini";
            this.btn_ReperniMagacini.Size = new System.Drawing.Size(195, 44);
            this.btn_ReperniMagacini.TabIndex = 20;
            this.btn_ReperniMagacini.Text = "Reperni magacini";
            this.btn_ReperniMagacini.UseVisualStyleBackColor = true;
            this.btn_ReperniMagacini.Click += new System.EventHandler(this.btn_ReperniMagacini_Click);
            // 
            // uIzvestajUbaciVrednosti_cb
            // 
            this.uIzvestajUbaciVrednosti_cb.AutoSize = true;
            this.uIzvestajUbaciVrednosti_cb.Checked = true;
            this.uIzvestajUbaciVrednosti_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uIzvestajUbaciVrednosti_cb.Location = new System.Drawing.Point(6, 42);
            this.uIzvestajUbaciVrednosti_cb.Name = "uIzvestajUbaciVrednosti_cb";
            this.uIzvestajUbaciVrednosti_cb.Size = new System.Drawing.Size(80, 19);
            this.uIzvestajUbaciVrednosti_cb.TabIndex = 7;
            this.uIzvestajUbaciVrednosti_cb.Text = "Vrednosti";
            this.uIzvestajUbaciVrednosti_cb.UseVisualStyleBackColor = true;
            // 
            // uIzvestajUbaciKolicine_btn
            // 
            this.uIzvestajUbaciKolicine_btn.AutoSize = true;
            this.uIzvestajUbaciKolicine_btn.Checked = true;
            this.uIzvestajUbaciKolicine_btn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uIzvestajUbaciKolicine_btn.Location = new System.Drawing.Point(6, 20);
            this.uIzvestajUbaciKolicine_btn.Name = "uIzvestajUbaciKolicine_btn";
            this.uIzvestajUbaciKolicine_btn.Size = new System.Drawing.Size(73, 19);
            this.uIzvestajUbaciKolicine_btn.TabIndex = 6;
            this.uIzvestajUbaciKolicine_btn.Text = "Kolicine";
            this.uIzvestajUbaciKolicine_btn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 446);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Komentar:";
            // 
            // komentar_rtb
            // 
            this.komentar_rtb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.komentar_rtb.Location = new System.Drawing.Point(12, 462);
            this.komentar_rtb.Name = "komentar_rtb";
            this.komentar_rtb.Size = new System.Drawing.Size(610, 131);
            this.komentar_rtb.TabIndex = 13;
            this.komentar_rtb.Text = "";
            // 
            // korisnicimaMagacina_gb
            // 
            this.korisnicimaMagacina_gb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.korisnicimaMagacina_gb.Controls.Add(this.izvestajKakavVIdim_rb);
            this.korisnicimaMagacina_gb.Controls.Add(this.korisnicimaMagacinaCeoIzvestaj_rb);
            this.korisnicimaMagacina_gb.Controls.Add(this.korisnicimaMagacinaSamoSvojePodatke_rb);
            this.korisnicimaMagacina_gb.Location = new System.Drawing.Point(12, 12);
            this.korisnicimaMagacina_gb.Name = "korisnicimaMagacina_gb";
            this.korisnicimaMagacina_gb.Size = new System.Drawing.Size(237, 89);
            this.korisnicimaMagacina_gb.TabIndex = 15;
            this.korisnicimaMagacina_gb.TabStop = false;
            this.korisnicimaMagacina_gb.Text = "Salji izvestaj";
            // 
            // izvestajKakavVIdim_rb
            // 
            this.izvestajKakavVIdim_rb.AutoSize = true;
            this.izvestajKakavVIdim_rb.Enabled = false;
            this.izvestajKakavVIdim_rb.Location = new System.Drawing.Point(6, 42);
            this.izvestajKakavVIdim_rb.Name = "izvestajKakavVIdim_rb";
            this.izvestajKakavVIdim_rb.Size = new System.Drawing.Size(162, 19);
            this.izvestajKakavVIdim_rb.TabIndex = 5;
            this.izvestajKakavVIdim_rb.Text = "Izvestaj Bas Kakav Vidim";
            this.izvestajKakavVIdim_rb.UseVisualStyleBackColor = true;
            this.izvestajKakavVIdim_rb.CheckedChanged += new System.EventHandler(this.izvestajKakavVIdim_rb_CheckedChanged);
            // 
            // korisnicimaMagacinaCeoIzvestaj_rb
            // 
            this.korisnicimaMagacinaCeoIzvestaj_rb.AutoSize = true;
            this.korisnicimaMagacinaCeoIzvestaj_rb.Checked = true;
            this.korisnicimaMagacinaCeoIzvestaj_rb.Location = new System.Drawing.Point(6, 65);
            this.korisnicimaMagacinaCeoIzvestaj_rb.Name = "korisnicimaMagacinaCeoIzvestaj_rb";
            this.korisnicimaMagacinaCeoIzvestaj_rb.Size = new System.Drawing.Size(93, 19);
            this.korisnicimaMagacinaCeoIzvestaj_rb.TabIndex = 4;
            this.korisnicimaMagacinaCeoIzvestaj_rb.TabStop = true;
            this.korisnicimaMagacinaCeoIzvestaj_rb.Text = "Ceo Izvestaj";
            this.korisnicimaMagacinaCeoIzvestaj_rb.UseVisualStyleBackColor = true;
            this.korisnicimaMagacinaCeoIzvestaj_rb.CheckedChanged += new System.EventHandler(this.korisnicimaMagacinaCeoIzvestaj_rb_CheckedChanged);
            // 
            // korisnicimaMagacinaSamoSvojePodatke_rb
            // 
            this.korisnicimaMagacinaSamoSvojePodatke_rb.AutoSize = true;
            this.korisnicimaMagacinaSamoSvojePodatke_rb.Location = new System.Drawing.Point(6, 19);
            this.korisnicimaMagacinaSamoSvojePodatke_rb.Name = "korisnicimaMagacinaSamoSvojePodatke_rb";
            this.korisnicimaMagacinaSamoSvojePodatke_rb.Size = new System.Drawing.Size(93, 19);
            this.korisnicimaMagacinaSamoSvojePodatke_rb.TabIndex = 3;
            this.korisnicimaMagacinaSamoSvojePodatke_rb.Text = "Za Magacin";
            this.korisnicimaMagacinaSamoSvojePodatke_rb.UseVisualStyleBackColor = true;
            this.korisnicimaMagacinaSamoSvojePodatke_rb.CheckedChanged += new System.EventHandler(this.korisnicimaMagacinaSamoSvojePodatke_rb_CheckedChanged);
            // 
            // tb_NaslovIzvestaja
            // 
            this.tb_NaslovIzvestaja.Location = new System.Drawing.Point(20, 419);
            this.tb_NaslovIzvestaja.Name = "tb_NaslovIzvestaja";
            this.tb_NaslovIzvestaja.Size = new System.Drawing.Size(598, 20);
            this.tb_NaslovIzvestaja.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 401);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "Naslov izvestaja";
            // 
            // status_lbl
            // 
            this.status_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.status_lbl.AutoSize = true;
            this.status_lbl.Location = new System.Drawing.Point(17, 635);
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(41, 15);
            this.status_lbl.TabIndex = 21;
            this.status_lbl.Text = "label1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cekirajSveToolStripMenuItem,
            this.decekirajSveToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(168, 52);
            // 
            // cekirajSveToolStripMenuItem
            // 
            this.cekirajSveToolStripMenuItem.Name = "cekirajSveToolStripMenuItem";
            this.cekirajSveToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.cekirajSveToolStripMenuItem.Text = "Cekiraj Sve";
            this.cekirajSveToolStripMenuItem.Click += new System.EventHandler(this.cekirajSveToolStripMenuItem_Click);
            // 
            // decekirajSveToolStripMenuItem
            // 
            this.decekirajSveToolStripMenuItem.Name = "decekirajSveToolStripMenuItem";
            this.decekirajSveToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.decekirajSveToolStripMenuItem.Text = "Decekiraj Sve";
            this.decekirajSveToolStripMenuItem.Click += new System.EventHandler(this.decekirajSveToolStripMenuItem_Click);
            // 
            // fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima_PosaljiKaoIzvestaj_Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 662);
            this.Controls.Add(this.status_lbl);
            this.Controls.Add(this.tb_NaslovIzvestaja);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.korisnicimaMagacina_gb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.komentar_rtb);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.posalji_btn);
            this.Controls.Add(this.korisnicima_clb);
            this.Name = "fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima_PosaljiKaoIzvestaj_Setup";
            this.Text = "Posalji Kao Izvestaj Zbirno Po Magacinima Setup";
            this.Load += new System.EventHandler(this.fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima_PosaljiKaoIzvestaj_Setup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.korisnicimaMagacina_gb.ResumeLayout(false);
            this.korisnicimaMagacina_gb.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckedListBox korisnicima_clb;
        private System.Windows.Forms.Button posalji_btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox uIzvestajUbaciVrednosti_cb;
        private System.Windows.Forms.CheckBox uIzvestajUbaciKolicine_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox komentar_rtb;
        private System.Windows.Forms.GroupBox korisnicimaMagacina_gb;
        private System.Windows.Forms.RadioButton izvestajKakavVIdim_rb;
        private System.Windows.Forms.RadioButton korisnicimaMagacinaCeoIzvestaj_rb;
        private System.Windows.Forms.RadioButton korisnicimaMagacinaSamoSvojePodatke_rb;
        private System.Windows.Forms.TextBox tb_NaslovIzvestaja;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_ReperniMagacini;
        private System.Windows.Forms.Label status_lbl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cekirajSveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decekirajSveToolStripMenuItem;
    }
}