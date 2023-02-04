
namespace TDOffice_v2
{
    partial class _1366_fm_NalogZaPrevoz_Index
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
            this.datum_dtp = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cenaPrevoznikaBezPDV_txt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.placanje_cmb = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.novaDestinacija_btn = new System.Windows.Forms.Button();
            this.prevoznik_cmb = new System.Windows.Forms.ComboBox();
            this.btn_Stampaj = new System.Windows.Forms.Button();
            this.btn_OdbaciIzmene = new System.Windows.Forms.Button();
            this.btn_Sacuvaj = new System.Windows.Forms.Button();
            this.status_btn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.upravljajPrevoznicimaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // datum_dtp
            // 
            this.datum_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.datum_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datum_dtp.Location = new System.Drawing.Point(53, 37);
            this.datum_dtp.Name = "datum_dtp";
            this.datum_dtp.Size = new System.Drawing.Size(144, 20);
            this.datum_dtp.TabIndex = 24;
            this.datum_dtp.ValueChanged += new System.EventHandler(this.datum_dtp_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Datum:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Prevoznik:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Placanje:";
            // 
            // cenaPrevoznikaBezPDV_txt
            // 
            this.cenaPrevoznikaBezPDV_txt.Location = new System.Drawing.Point(165, 115);
            this.cenaPrevoznikaBezPDV_txt.Name = "cenaPrevoznikaBezPDV_txt";
            this.cenaPrevoznikaBezPDV_txt.Size = new System.Drawing.Size(166, 20);
            this.cenaPrevoznikaBezPDV_txt.TabIndex = 31;
            this.cenaPrevoznikaBezPDV_txt.TextChanged += new System.EventHandler(this.cenaPrevoznikaBezPDV_txt_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Cena Prevoznika Bez PDV:";
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(72, 141);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(153, 21);
            this.magacin_cmb.TabIndex = 32;
            this.magacin_cmb.SelectedIndexChanged += new System.EventHandler(this.magacin_cmb_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Polaziste:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.prevoznik_cmb);
            this.panel1.Controls.Add(this.btn_Stampaj);
            this.panel1.Controls.Add(this.btn_OdbaciIzmene);
            this.panel1.Controls.Add(this.btn_Sacuvaj);
            this.panel1.Controls.Add(this.status_btn);
            this.panel1.Controls.Add(this.placanje_cmb);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.datum_dtp);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.magacin_cmb);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cenaPrevoznikaBezPDV_txt);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(614, 440);
            this.panel1.TabIndex = 34;
            // 
            // placanje_cmb
            // 
            this.placanje_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.placanje_cmb.FormattingEnabled = true;
            this.placanje_cmb.Items.AddRange(new object[] {
            "Virmanom",
            "Gotovinom"});
            this.placanje_cmb.Location = new System.Drawing.Point(72, 89);
            this.placanje_cmb.Name = "placanje_cmb";
            this.placanje_cmb.Size = new System.Drawing.Size(188, 21);
            this.placanje_cmb.TabIndex = 36;
            this.placanje_cmb.SelectedIndexChanged += new System.EventHandler(this.placanje_cmb_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.novaDestinacija_btn);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 179);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(608, 258);
            this.flowLayoutPanel1.TabIndex = 34;
            // 
            // novaDestinacija_btn
            // 
            this.novaDestinacija_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.novaDestinacija_btn.Location = new System.Drawing.Point(3, 3);
            this.novaDestinacija_btn.Name = "novaDestinacija_btn";
            this.novaDestinacija_btn.Size = new System.Drawing.Size(50, 50);
            this.novaDestinacija_btn.TabIndex = 0;
            this.novaDestinacija_btn.Text = "+";
            this.novaDestinacija_btn.UseVisualStyleBackColor = true;
            this.novaDestinacija_btn.Click += new System.EventHandler(this.novaDestinacija_btn_Click);
            // 
            // prevoznik_cmb
            // 
            this.prevoznik_cmb.ContextMenuStrip = this.contextMenuStrip1;
            this.prevoznik_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.prevoznik_cmb.FormattingEnabled = true;
            this.prevoznik_cmb.Location = new System.Drawing.Point(69, 62);
            this.prevoznik_cmb.Name = "prevoznik_cmb";
            this.prevoznik_cmb.Size = new System.Drawing.Size(208, 21);
            this.prevoznik_cmb.TabIndex = 41;
            this.prevoznik_cmb.SelectedIndexChanged += new System.EventHandler(this.prevoznik_cmb_SelectedIndexChanged);
            // 
            // btn_Stampaj
            // 
            this.btn_Stampaj.BackgroundImage = global::TDOffice_v2.Properties.Resources.printer_icon;
            this.btn_Stampaj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Stampaj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Stampaj.Location = new System.Drawing.Point(82, 6);
            this.btn_Stampaj.Name = "btn_Stampaj";
            this.btn_Stampaj.Size = new System.Drawing.Size(42, 25);
            this.btn_Stampaj.TabIndex = 40;
            this.btn_Stampaj.UseVisualStyleBackColor = true;
            this.btn_Stampaj.Click += new System.EventHandler(this.btn_Stampaj_Click);
            // 
            // btn_OdbaciIzmene
            // 
            this.btn_OdbaciIzmene.BackgroundImage = global::TDOffice_v2.Properties.Resources.discard_icon;
            this.btn_OdbaciIzmene.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_OdbaciIzmene.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_OdbaciIzmene.Location = new System.Drawing.Point(43, 6);
            this.btn_OdbaciIzmene.Name = "btn_OdbaciIzmene";
            this.btn_OdbaciIzmene.Size = new System.Drawing.Size(33, 25);
            this.btn_OdbaciIzmene.TabIndex = 39;
            this.btn_OdbaciIzmene.UseVisualStyleBackColor = true;
            this.btn_OdbaciIzmene.Click += new System.EventHandler(this.btn__Click);
            // 
            // btn_Sacuvaj
            // 
            this.btn_Sacuvaj.BackgroundImage = global::TDOffice_v2.Properties.Resources.save_icon;
            this.btn_Sacuvaj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Sacuvaj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Sacuvaj.Location = new System.Drawing.Point(9, 6);
            this.btn_Sacuvaj.Name = "btn_Sacuvaj";
            this.btn_Sacuvaj.Size = new System.Drawing.Size(28, 25);
            this.btn_Sacuvaj.TabIndex = 38;
            this.btn_Sacuvaj.UseVisualStyleBackColor = true;
            this.btn_Sacuvaj.Click += new System.EventHandler(this.btn_Sacuvaj_Click);
            // 
            // status_btn
            // 
            this.status_btn.BackgroundImage = global::TDOffice_v2.Properties.Resources.key_red;
            this.status_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.status_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.status_btn.Location = new System.Drawing.Point(565, 6);
            this.status_btn.Name = "status_btn";
            this.status_btn.Size = new System.Drawing.Size(46, 25);
            this.status_btn.TabIndex = 37;
            this.status_btn.UseVisualStyleBackColor = true;
            this.status_btn.Click += new System.EventHandler(this.status_btn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upravljajPrevoznicimaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(195, 26);
            // 
            // upravljajPrevoznicimaToolStripMenuItem
            // 
            this.upravljajPrevoznicimaToolStripMenuItem.Name = "upravljajPrevoznicimaToolStripMenuItem";
            this.upravljajPrevoznicimaToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.upravljajPrevoznicimaToolStripMenuItem.Text = "Upravljaj Prevoznicima";
            this.upravljajPrevoznicimaToolStripMenuItem.Click += new System.EventHandler(this.upravljajPrevoznicimaToolStripMenuItem_Click);
            // 
            // _1366_fm_NalogZaPrevoz_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.ClientSize = new System.Drawing.Size(629, 455);
            this.Controls.Add(this.panel1);
            this.Name = "_1366_fm_NalogZaPrevoz_Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_1366_fm_NalogZaPrevoz_Index";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this._1366_fm_NalogZaPrevoz_Index_FormClosed);
            this.Load += new System.EventHandler(this._1366_fm_NalogZaPrevoz_Index_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker datum_dtp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox cenaPrevoznikaBezPDV_txt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button novaDestinacija_btn;
        private System.Windows.Forms.ComboBox placanje_cmb;
        private System.Windows.Forms.Button status_btn;
        private System.Windows.Forms.Button btn_Stampaj;
        private System.Windows.Forms.Button btn_OdbaciIzmene;
        private System.Windows.Forms.Button btn_Sacuvaj;
        private System.Windows.Forms.ComboBox prevoznik_cmb;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem upravljajPrevoznicimaToolStripMenuItem;
    }
}