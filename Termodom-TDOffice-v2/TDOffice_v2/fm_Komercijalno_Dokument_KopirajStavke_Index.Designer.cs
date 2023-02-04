
namespace TDOffice_v2
{
    partial class fm_Komercijalno_Dokument_KopirajStavke_Index
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
            this.izVrdok_cmb = new System.Windows.Forms.ComboBox();
            this.izBrDok_txt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.izGodine_cmb = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uGodinu_cmb = new System.Windows.Forms.ComboBox();
            this.uVrDok_cmb = new System.Windows.Forms.ComboBox();
            this.uBrDok_txt = new System.Windows.Forms.TextBox();
            this.kopiraj_btn = new System.Windows.Forms.Button();
            this.prodajneCeneOstaviKaoUIzvornomDokumentu_cb = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.duplirajStavke_rb = new System.Windows.Forms.RadioButton();
            this.zaobidjiDuplikat_rb = new System.Windows.Forms.RadioButton();
            this.destinactioniDokumentMoraBitiOtkljucan_cb = new System.Windows.Forms.CheckBox();
            this.destinacioniDokumentMoraBitiPrazan_cb = new System.Windows.Forms.CheckBox();
            this.nabavneCeneOstaviKaoUIzvornomDokumentu_cb = new System.Windows.Forms.CheckBox();
            this.status_lbl = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // izVrdok_cmb
            // 
            this.izVrdok_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.izVrdok_cmb.FormattingEnabled = true;
            this.izVrdok_cmb.Location = new System.Drawing.Point(6, 92);
            this.izVrdok_cmb.Name = "izVrdok_cmb";
            this.izVrdok_cmb.Size = new System.Drawing.Size(219, 21);
            this.izVrdok_cmb.TabIndex = 0;
            // 
            // izBrDok_txt
            // 
            this.izBrDok_txt.Location = new System.Drawing.Point(46, 119);
            this.izBrDok_txt.Name = "izBrDok_txt";
            this.izBrDok_txt.Size = new System.Drawing.Size(179, 20);
            this.izBrDok_txt.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.izGodine_cmb);
            this.groupBox1.Controls.Add(this.izVrdok_cmb);
            this.groupBox1.Controls.Add(this.izBrDok_txt);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 152);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kopiraj Stavke Iz Dokumenta";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "BrDok";
            // 
            // izGodine_cmb
            // 
            this.izGodine_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.izGodine_cmb.FormattingEnabled = true;
            this.izGodine_cmb.Location = new System.Drawing.Point(6, 61);
            this.izGodine_cmb.Name = "izGodine_cmb";
            this.izGodine_cmb.Size = new System.Drawing.Size(121, 21);
            this.izGodine_cmb.TabIndex = 3;
            this.izGodine_cmb.SelectedIndexChanged += new System.EventHandler(this.izGodine_cmb_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.uGodinu_cmb);
            this.groupBox2.Controls.Add(this.uVrDok_cmb);
            this.groupBox2.Controls.Add(this.uBrDok_txt);
            this.groupBox2.Location = new System.Drawing.Point(258, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 153);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Kopiraj Stavke U Dokument";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "BrDok";
            // 
            // uGodinu_cmb
            // 
            this.uGodinu_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uGodinu_cmb.FormattingEnabled = true;
            this.uGodinu_cmb.Location = new System.Drawing.Point(6, 61);
            this.uGodinu_cmb.Name = "uGodinu_cmb";
            this.uGodinu_cmb.Size = new System.Drawing.Size(121, 21);
            this.uGodinu_cmb.TabIndex = 3;
            this.uGodinu_cmb.SelectedIndexChanged += new System.EventHandler(this.uGodinu_cmb_SelectedIndexChanged);
            // 
            // uVrDok_cmb
            // 
            this.uVrDok_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uVrDok_cmb.FormattingEnabled = true;
            this.uVrDok_cmb.Location = new System.Drawing.Point(6, 92);
            this.uVrDok_cmb.Name = "uVrDok_cmb";
            this.uVrDok_cmb.Size = new System.Drawing.Size(219, 21);
            this.uVrDok_cmb.TabIndex = 0;
            // 
            // uBrDok_txt
            // 
            this.uBrDok_txt.Location = new System.Drawing.Point(46, 119);
            this.uBrDok_txt.Name = "uBrDok_txt";
            this.uBrDok_txt.Size = new System.Drawing.Size(179, 20);
            this.uBrDok_txt.TabIndex = 1;
            // 
            // kopiraj_btn
            // 
            this.kopiraj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kopiraj_btn.Location = new System.Drawing.Point(12, 373);
            this.kopiraj_btn.Name = "kopiraj_btn";
            this.kopiraj_btn.Size = new System.Drawing.Size(486, 32);
            this.kopiraj_btn.TabIndex = 6;
            this.kopiraj_btn.Text = "Kopiraj";
            this.kopiraj_btn.UseVisualStyleBackColor = true;
            this.kopiraj_btn.Click += new System.EventHandler(this.kopiraj_btn_Click);
            // 
            // prodajneCeneOstaviKaoUIzvornomDokumentu_cb
            // 
            this.prodajneCeneOstaviKaoUIzvornomDokumentu_cb.AutoSize = true;
            this.prodajneCeneOstaviKaoUIzvornomDokumentu_cb.Location = new System.Drawing.Point(6, 19);
            this.prodajneCeneOstaviKaoUIzvornomDokumentu_cb.Name = "prodajneCeneOstaviKaoUIzvornomDokumentu_cb";
            this.prodajneCeneOstaviKaoUIzvornomDokumentu_cb.Size = new System.Drawing.Size(257, 17);
            this.prodajneCeneOstaviKaoUIzvornomDokumentu_cb.TabIndex = 7;
            this.prodajneCeneOstaviKaoUIzvornomDokumentu_cb.Text = "Prodajne cene ostavi kao u izvornom dokumentu";
            this.prodajneCeneOstaviKaoUIzvornomDokumentu_cb.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.destinactioniDokumentMoraBitiOtkljucan_cb);
            this.groupBox3.Controls.Add(this.destinacioniDokumentMoraBitiPrazan_cb);
            this.groupBox3.Controls.Add(this.nabavneCeneOstaviKaoUIzvornomDokumentu_cb);
            this.groupBox3.Controls.Add(this.prodajneCeneOstaviKaoUIzvornomDokumentu_cb);
            this.groupBox3.Location = new System.Drawing.Point(12, 171);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(486, 130);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Dodatna Podesavanja";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.duplirajStavke_rb);
            this.panel1.Controls.Add(this.zaobidjiDuplikat_rb);
            this.panel1.Location = new System.Drawing.Point(223, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 28);
            this.panel1.TabIndex = 13;
            this.panel1.Visible = false;
            // 
            // duplirajStavke_rb
            // 
            this.duplirajStavke_rb.AutoSize = true;
            this.duplirajStavke_rb.Checked = true;
            this.duplirajStavke_rb.Location = new System.Drawing.Point(3, 3);
            this.duplirajStavke_rb.Name = "duplirajStavke_rb";
            this.duplirajStavke_rb.Size = new System.Drawing.Size(97, 17);
            this.duplirajStavke_rb.TabIndex = 11;
            this.duplirajStavke_rb.TabStop = true;
            this.duplirajStavke_rb.Text = "Dupliraj Stavke";
            this.duplirajStavke_rb.UseVisualStyleBackColor = true;
            // 
            // zaobidjiDuplikat_rb
            // 
            this.zaobidjiDuplikat_rb.AutoSize = true;
            this.zaobidjiDuplikat_rb.Location = new System.Drawing.Point(103, 3);
            this.zaobidjiDuplikat_rb.Name = "zaobidjiDuplikat_rb";
            this.zaobidjiDuplikat_rb.Size = new System.Drawing.Size(102, 17);
            this.zaobidjiDuplikat_rb.TabIndex = 12;
            this.zaobidjiDuplikat_rb.Text = "Zaobidji duplikat";
            this.zaobidjiDuplikat_rb.UseVisualStyleBackColor = true;
            // 
            // destinactioniDokumentMoraBitiOtkljucan_cb
            // 
            this.destinactioniDokumentMoraBitiOtkljucan_cb.AutoSize = true;
            this.destinactioniDokumentMoraBitiOtkljucan_cb.Checked = true;
            this.destinactioniDokumentMoraBitiOtkljucan_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.destinactioniDokumentMoraBitiOtkljucan_cb.Location = new System.Drawing.Point(6, 88);
            this.destinactioniDokumentMoraBitiOtkljucan_cb.Name = "destinactioniDokumentMoraBitiOtkljucan_cb";
            this.destinactioniDokumentMoraBitiOtkljucan_cb.Size = new System.Drawing.Size(222, 17);
            this.destinactioniDokumentMoraBitiOtkljucan_cb.TabIndex = 10;
            this.destinactioniDokumentMoraBitiOtkljucan_cb.Text = "Destinacioni dokument mora biti otkljucan";
            this.destinactioniDokumentMoraBitiOtkljucan_cb.UseVisualStyleBackColor = true;
            // 
            // destinacioniDokumentMoraBitiPrazan_cb
            // 
            this.destinacioniDokumentMoraBitiPrazan_cb.AutoSize = true;
            this.destinacioniDokumentMoraBitiPrazan_cb.Checked = true;
            this.destinacioniDokumentMoraBitiPrazan_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.destinacioniDokumentMoraBitiPrazan_cb.Location = new System.Drawing.Point(6, 65);
            this.destinacioniDokumentMoraBitiPrazan_cb.Name = "destinacioniDokumentMoraBitiPrazan_cb";
            this.destinacioniDokumentMoraBitiPrazan_cb.Size = new System.Drawing.Size(211, 17);
            this.destinacioniDokumentMoraBitiPrazan_cb.TabIndex = 9;
            this.destinacioniDokumentMoraBitiPrazan_cb.Text = "Destinacioni dokument mora biti prazan";
            this.destinacioniDokumentMoraBitiPrazan_cb.UseVisualStyleBackColor = true;
            this.destinacioniDokumentMoraBitiPrazan_cb.CheckedChanged += new System.EventHandler(this.destinacioniDokumentMoraBitiPrazan_cb_CheckedChanged);
            // 
            // nabavneCeneOstaviKaoUIzvornomDokumentu_cb
            // 
            this.nabavneCeneOstaviKaoUIzvornomDokumentu_cb.AutoSize = true;
            this.nabavneCeneOstaviKaoUIzvornomDokumentu_cb.Location = new System.Drawing.Point(6, 42);
            this.nabavneCeneOstaviKaoUIzvornomDokumentu_cb.Name = "nabavneCeneOstaviKaoUIzvornomDokumentu_cb";
            this.nabavneCeneOstaviKaoUIzvornomDokumentu_cb.Size = new System.Drawing.Size(259, 17);
            this.nabavneCeneOstaviKaoUIzvornomDokumentu_cb.TabIndex = 8;
            this.nabavneCeneOstaviKaoUIzvornomDokumentu_cb.Text = "Nabavne cene ostavi kao u izvornom dokumentu";
            this.nabavneCeneOstaviKaoUIzvornomDokumentu_cb.UseVisualStyleBackColor = true;
            // 
            // status_lbl
            // 
            this.status_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.status_lbl.AutoSize = true;
            this.status_lbl.Location = new System.Drawing.Point(15, 413);
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(35, 13);
            this.status_lbl.TabIndex = 5;
            this.status_lbl.Text = "status";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(179, 20);
            this.textBox1.TabIndex = 5;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 35);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(179, 20);
            this.textBox2.TabIndex = 5;
            // 
            // fm_Komercijalno_Dokument_KopirajStavke_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 435);
            this.Controls.Add(this.status_lbl);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.kopiraj_btn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(524, 203);
            this.Name = "fm_Komercijalno_Dokument_KopirajStavke_Index";
            this.Text = "Komercijalno Kopiraj Stavke Iz Dokumenta U Dokument";
            this.Load += new System.EventHandler(this.fm_Komercijalno_Dokument_KopirajStavke_Index_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox izVrdok_cmb;
        private System.Windows.Forms.TextBox izBrDok_txt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox izGodine_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox uGodinu_cmb;
        private System.Windows.Forms.ComboBox uVrDok_cmb;
        private System.Windows.Forms.TextBox uBrDok_txt;
        private System.Windows.Forms.Button kopiraj_btn;
        private System.Windows.Forms.CheckBox prodajneCeneOstaviKaoUIzvornomDokumentu_cb;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox nabavneCeneOstaviKaoUIzvornomDokumentu_cb;
        private System.Windows.Forms.Label status_lbl;
        private System.Windows.Forms.CheckBox destinactioniDokumentMoraBitiOtkljucan_cb;
        private System.Windows.Forms.CheckBox destinacioniDokumentMoraBitiPrazan_cb;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton duplirajStavke_rb;
        private System.Windows.Forms.RadioButton zaobidjiDuplikat_rb;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}