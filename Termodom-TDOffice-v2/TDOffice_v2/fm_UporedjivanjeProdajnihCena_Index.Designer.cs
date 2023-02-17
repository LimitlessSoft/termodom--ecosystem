namespace TDOffice_v2
{
    partial class fm_UporedjivanjeProdajnihCena_Index
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
            this.izvorniVrDok_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.izvorniBrDok_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.uporediKolicine_cb = new System.Windows.Forms.CheckBox();
            this.uporediNabavneCene_cb = new System.Windows.Forms.CheckBox();
            this.uporediProdajneCene_cb = new System.Windows.Forms.CheckBox();
            this.uporedi_btn = new System.Windows.Forms.Button();
            this.izvornaBaza_cmb = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.proveriDaliImaDestinacioniDokument_cb = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.period_rb = new System.Windows.Forms.RadioButton();
            this.broj_rb = new System.Windows.Forms.RadioButton();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // izvorniVrDok_txt
            // 
            this.izvorniVrDok_txt.Location = new System.Drawing.Point(98, 66);
            this.izvorniVrDok_txt.Name = "izvorniVrDok_txt";
            this.izvorniVrDok_txt.Size = new System.Drawing.Size(130, 23);
            this.izvorniVrDok_txt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Izvorni Vr Dok";
            // 
            // izvorniBrDok_txt
            // 
            this.izvorniBrDok_txt.Location = new System.Drawing.Point(65, 76);
            this.izvorniBrDok_txt.Name = "izvorniBrDok_txt";
            this.izvorniBrDok_txt.Size = new System.Drawing.Size(130, 23);
            this.izvorniBrDok_txt.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(637, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Uporedjuje stavke unutar izvornog dokumenta i njegovog destinacionog dokumenta i " +
    "prijavljuje ukoliko ima neslaganja";
            // 
            // uporediKolicine_cb
            // 
            this.uporediKolicine_cb.AutoSize = true;
            this.uporediKolicine_cb.Location = new System.Drawing.Point(12, 243);
            this.uporediKolicine_cb.Name = "uporediKolicine_cb";
            this.uporediKolicine_cb.Size = new System.Drawing.Size(113, 19);
            this.uporediKolicine_cb.TabIndex = 6;
            this.uporediKolicine_cb.Text = "Uporedi Kolicine";
            this.uporediKolicine_cb.UseVisualStyleBackColor = true;
            // 
            // uporediNabavneCene_cb
            // 
            this.uporediNabavneCene_cb.AutoSize = true;
            this.uporediNabavneCene_cb.Location = new System.Drawing.Point(12, 268);
            this.uporediNabavneCene_cb.Name = "uporediNabavneCene_cb";
            this.uporediNabavneCene_cb.Size = new System.Drawing.Size(148, 19);
            this.uporediNabavneCene_cb.TabIndex = 7;
            this.uporediNabavneCene_cb.Text = "Uporedi Nabavne Cene";
            this.uporediNabavneCene_cb.UseVisualStyleBackColor = true;
            // 
            // uporediProdajneCene_cb
            // 
            this.uporediProdajneCene_cb.AutoSize = true;
            this.uporediProdajneCene_cb.Location = new System.Drawing.Point(12, 293);
            this.uporediProdajneCene_cb.Name = "uporediProdajneCene_cb";
            this.uporediProdajneCene_cb.Size = new System.Drawing.Size(148, 19);
            this.uporediProdajneCene_cb.TabIndex = 8;
            this.uporediProdajneCene_cb.Text = "Uporedi Prodajne Cene";
            this.uporediProdajneCene_cb.UseVisualStyleBackColor = true;
            // 
            // uporedi_btn
            // 
            this.uporedi_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uporedi_btn.Location = new System.Drawing.Point(423, 262);
            this.uporedi_btn.Name = "uporedi_btn";
            this.uporedi_btn.Size = new System.Drawing.Size(239, 61);
            this.uporedi_btn.TabIndex = 9;
            this.uporedi_btn.Text = "Uporedi";
            this.uporedi_btn.UseVisualStyleBackColor = true;
            this.uporedi_btn.Click += new System.EventHandler(this.uporedi_btn_ClickAsync);
            // 
            // izvornaBaza_cmb
            // 
            this.izvornaBaza_cmb.FormattingEnabled = true;
            this.izvornaBaza_cmb.Location = new System.Drawing.Point(90, 37);
            this.izvornaBaza_cmb.Name = "izvornaBaza_cmb";
            this.izvornaBaza_cmb.Size = new System.Drawing.Size(458, 23);
            this.izvornaBaza_cmb.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "Izvorna baza";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 326);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(674, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status_lbl
            // 
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(0, 17);
            // 
            // proveriDaliImaDestinacioniDokument_cb
            // 
            this.proveriDaliImaDestinacioniDokument_cb.AutoSize = true;
            this.proveriDaliImaDestinacioniDokument_cb.Checked = true;
            this.proveriDaliImaDestinacioniDokument_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.proveriDaliImaDestinacioniDokument_cb.Enabled = false;
            this.proveriDaliImaDestinacioniDokument_cb.Location = new System.Drawing.Point(12, 218);
            this.proveriDaliImaDestinacioniDokument_cb.Name = "proveriDaliImaDestinacioniDokument_cb";
            this.proveriDaliImaDestinacioniDokument_cb.Size = new System.Drawing.Size(236, 19);
            this.proveriDaliImaDestinacioniDokument_cb.TabIndex = 13;
            this.proveriDaliImaDestinacioniDokument_cb.Text = "Proveri da li ima destinacioni dokument";
            this.proveriDaliImaDestinacioniDokument_cb.UseVisualStyleBackColor = true;
            this.proveriDaliImaDestinacioniDokument_cb.CheckedChanged += new System.EventHandler(this.proveriDaliImaDestinacioniDokument_cb_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.doDatuma_dtp);
            this.panel1.Controls.Add(this.odDatuma_dtp);
            this.panel1.Location = new System.Drawing.Point(34, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 41);
            this.panel1.TabIndex = 14;
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MM.yyyy";
            this.odDatuma_dtp.Location = new System.Drawing.Point(34, 8);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(150, 23);
            this.odDatuma_dtp.TabIndex = 0;
            // 
            // doDatuma_dtp
            // 
            this.doDatuma_dtp.CustomFormat = "dd.MM.yyyy";
            this.doDatuma_dtp.Location = new System.Drawing.Point(220, 8);
            this.doDatuma_dtp.Name = "doDatuma_dtp";
            this.doDatuma_dtp.Size = new System.Drawing.Size(155, 23);
            this.doDatuma_dtp.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(193, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "do";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 15);
            this.label6.TabIndex = 3;
            this.label6.Text = "od";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.broj_rb);
            this.panel2.Controls.Add(this.period_rb);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.izvorniBrDok_txt);
            this.panel2.Location = new System.Drawing.Point(12, 95);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(441, 117);
            this.panel2.TabIndex = 15;
            // 
            // period_rb
            // 
            this.period_rb.AutoSize = true;
            this.period_rb.Checked = true;
            this.period_rb.Location = new System.Drawing.Point(13, 5);
            this.period_rb.Name = "period_rb";
            this.period_rb.Size = new System.Drawing.Size(59, 19);
            this.period_rb.TabIndex = 15;
            this.period_rb.TabStop = true;
            this.period_rb.Text = "Period";
            this.period_rb.UseVisualStyleBackColor = true;
            // 
            // broj_rb
            // 
            this.broj_rb.AutoSize = true;
            this.broj_rb.Location = new System.Drawing.Point(13, 78);
            this.broj_rb.Name = "broj_rb";
            this.broj_rb.Size = new System.Drawing.Size(46, 19);
            this.broj_rb.TabIndex = 16;
            this.broj_rb.TabStop = true;
            this.broj_rb.Text = "Broj";
            this.broj_rb.UseVisualStyleBackColor = true;
            // 
            // fm_UporedjivanjeProdajnihCena_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 348);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.proveriDaliImaDestinacioniDokument_cb);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.izvornaBaza_cmb);
            this.Controls.Add(this.uporedi_btn);
            this.Controls.Add(this.uporediProdajneCene_cb);
            this.Controls.Add(this.uporediNabavneCene_cb);
            this.Controls.Add(this.uporediKolicine_cb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.izvorniVrDok_txt);
            this.Name = "fm_UporedjivanjeProdajnihCena_Index";
            this.Text = "fm_UporedjivanjeProdajnihCena_Index";
            this.Load += new System.EventHandler(this.fm_UporedjivanjeProdajnihCena_Index_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox izvorniVrDok_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox izvorniBrDok_txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox uporediKolicine_cb;
        private System.Windows.Forms.CheckBox uporediNabavneCene_cb;
        private System.Windows.Forms.CheckBox uporediProdajneCene_cb;
        private System.Windows.Forms.Button uporedi_btn;
        private System.Windows.Forms.ComboBox izvornaBaza_cmb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl;
        private System.Windows.Forms.CheckBox proveriDaliImaDestinacioniDokument_cb;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton broj_rb;
        private System.Windows.Forms.RadioButton period_rb;
    }
}