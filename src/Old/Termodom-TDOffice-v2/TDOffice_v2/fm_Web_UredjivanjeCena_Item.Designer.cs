
namespace TDOffice_v2
{
    partial class fm_Web_UredjivanjeCena_Item
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
            this.uslovi_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.modifikator_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nazivArtikla_txt = new System.Windows.Forms.TextBox();
            this.robaID_txt = new System.Windows.Forms.TextBox();
            this.btn_OdbaciIzmene = new System.Windows.Forms.Button();
            this.btn_Sacuvaj = new System.Windows.Forms.Button();
            this.referentiProizvod_cmb = new System.Windows.Forms.ComboBox();
            this.periodAnalize_gb = new System.Windows.Forms.GroupBox();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.periodAnalize_gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // uslovi_cmb
            // 
            this.uslovi_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uslovi_cmb.FormattingEnabled = true;
            this.uslovi_cmb.Location = new System.Drawing.Point(54, 126);
            this.uslovi_cmb.Name = "uslovi_cmb";
            this.uslovi_cmb.Size = new System.Drawing.Size(216, 21);
            this.uslovi_cmb.TabIndex = 0;
            this.uslovi_cmb.SelectedIndexChanged += new System.EventHandler(this.uslovi_cmb_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Uslov:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cena iz magacina:";
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(112, 179);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(195, 21);
            this.magacin_cmb.TabIndex = 2;
            this.magacin_cmb.SelectedIndexChanged += new System.EventHandler(this.magacin_cmb_SelectedIndexChanged);
            // 
            // modifikator_txt
            // 
            this.modifikator_txt.Location = new System.Drawing.Point(114, 153);
            this.modifikator_txt.Name = "modifikator_txt";
            this.modifikator_txt.Size = new System.Drawing.Size(100, 20);
            this.modifikator_txt.TabIndex = 4;
            this.modifikator_txt.TextChanged += new System.EventHandler(this.modifikator_txt_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Modifikator (+/- %):";
            // 
            // nazivArtikla_txt
            // 
            this.nazivArtikla_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.nazivArtikla_txt.Location = new System.Drawing.Point(12, 100);
            this.nazivArtikla_txt.Name = "nazivArtikla_txt";
            this.nazivArtikla_txt.ReadOnly = true;
            this.nazivArtikla_txt.Size = new System.Drawing.Size(273, 20);
            this.nazivArtikla_txt.TabIndex = 6;
            // 
            // robaID_txt
            // 
            this.robaID_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.robaID_txt.Location = new System.Drawing.Point(12, 73);
            this.robaID_txt.Name = "robaID_txt";
            this.robaID_txt.ReadOnly = true;
            this.robaID_txt.Size = new System.Drawing.Size(126, 20);
            this.robaID_txt.TabIndex = 7;
            // 
            // btn_OdbaciIzmene
            // 
            this.btn_OdbaciIzmene.BackColor = System.Drawing.Color.Gray;
            this.btn_OdbaciIzmene.BackgroundImage = global::TDOffice_v2.Properties.Resources.discard_icon;
            this.btn_OdbaciIzmene.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_OdbaciIzmene.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_OdbaciIzmene.Location = new System.Drawing.Point(46, 12);
            this.btn_OdbaciIzmene.Name = "btn_OdbaciIzmene";
            this.btn_OdbaciIzmene.Size = new System.Drawing.Size(33, 25);
            this.btn_OdbaciIzmene.TabIndex = 41;
            this.btn_OdbaciIzmene.UseVisualStyleBackColor = false;
            this.btn_OdbaciIzmene.Click += new System.EventHandler(this.btn_OdbaciIzmene_Click);
            // 
            // btn_Sacuvaj
            // 
            this.btn_Sacuvaj.BackgroundImage = global::TDOffice_v2.Properties.Resources.save_icon;
            this.btn_Sacuvaj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Sacuvaj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Sacuvaj.Location = new System.Drawing.Point(12, 12);
            this.btn_Sacuvaj.Name = "btn_Sacuvaj";
            this.btn_Sacuvaj.Size = new System.Drawing.Size(28, 25);
            this.btn_Sacuvaj.TabIndex = 40;
            this.btn_Sacuvaj.UseVisualStyleBackColor = true;
            this.btn_Sacuvaj.Click += new System.EventHandler(this.btn_Sacuvaj_Click);
            // 
            // referentiProizvod_cmb
            // 
            this.referentiProizvod_cmb.FormattingEnabled = true;
            this.referentiProizvod_cmb.Location = new System.Drawing.Point(276, 126);
            this.referentiProizvod_cmb.Name = "referentiProizvod_cmb";
            this.referentiProizvod_cmb.Size = new System.Drawing.Size(271, 21);
            this.referentiProizvod_cmb.TabIndex = 42;
            this.referentiProizvod_cmb.SelectedIndexChanged += new System.EventHandler(this.referentiProizvod_cmb_SelectedIndexChanged);
            // 
            // periodAnalize_gb
            // 
            this.periodAnalize_gb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.periodAnalize_gb.Controls.Add(this.odDatuma_dtp);
            this.periodAnalize_gb.Controls.Add(this.label4);
            this.periodAnalize_gb.Controls.Add(this.doDatuma_dtp);
            this.periodAnalize_gb.Controls.Add(this.label5);
            this.periodAnalize_gb.Location = new System.Drawing.Point(198, 12);
            this.periodAnalize_gb.Name = "periodAnalize_gb";
            this.periodAnalize_gb.Size = new System.Drawing.Size(470, 53);
            this.periodAnalize_gb.TabIndex = 43;
            this.periodAnalize_gb.TabStop = false;
            this.periodAnalize_gb.Text = "Period Analize";
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odDatuma_dtp.Location = new System.Drawing.Point(83, 19);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.odDatuma_dtp.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(233, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Do datuma:";
            // 
            // doDatuma_dtp
            // 
            this.doDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.doDatuma_dtp.Location = new System.Drawing.Point(301, 19);
            this.doDatuma_dtp.Name = "doDatuma_dtp";
            this.doDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.doDatuma_dtp.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Od datuma:";
            // 
            // fm_Web_UredjivanjeCena_Item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 218);
            this.Controls.Add(this.periodAnalize_gb);
            this.Controls.Add(this.referentiProizvod_cmb);
            this.Controls.Add(this.btn_OdbaciIzmene);
            this.Controls.Add(this.btn_Sacuvaj);
            this.Controls.Add(this.robaID_txt);
            this.Controls.Add(this.nazivArtikla_txt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.modifikator_txt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.magacin_cmb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uslovi_cmb);
            this.Name = "fm_Web_UredjivanjeCena_Item";
            this.Text = "Stavka";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fm_Web_UredjivanjeCena_Item_FormClosing);
            this.Load += new System.EventHandler(this.fm_Web_UredjivanjeCena_Item_Load);
            this.periodAnalize_gb.ResumeLayout(false);
            this.periodAnalize_gb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox uslovi_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.TextBox modifikator_txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nazivArtikla_txt;
        private System.Windows.Forms.TextBox robaID_txt;
        private System.Windows.Forms.Button btn_OdbaciIzmene;
        private System.Windows.Forms.Button btn_Sacuvaj;
        private System.Windows.Forms.ComboBox referentiProizvod_cmb;
        private System.Windows.Forms.GroupBox periodAnalize_gb;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
        private System.Windows.Forms.Label label5;
    }
}