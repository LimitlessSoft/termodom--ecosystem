namespace TDOffice_v2
{
    partial class fm_UlaznaPonuda_Index
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.krajVazenja_dtp = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.pocetakVazenja_dtp = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.datumDokumenta_dtp = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.osvezi_btn = new System.Windows.Forms.Button();
            this.nova_btn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_OdbaciIzmene = new System.Windows.Forms.Button();
            this.btn_Sacuvaj = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.krajVazenja_dtp);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.pocetakVazenja_dtp);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.datumDokumenta_dtp);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(12, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(942, 83);
            this.panel2.TabIndex = 25;
            // 
            // krajVazenja_dtp
            // 
            this.krajVazenja_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.krajVazenja_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.krajVazenja_dtp.Location = new System.Drawing.Point(84, 55);
            this.krajVazenja_dtp.Name = "krajVazenja_dtp";
            this.krajVazenja_dtp.Size = new System.Drawing.Size(144, 20);
            this.krajVazenja_dtp.TabIndex = 8;
            this.krajVazenja_dtp.ValueChanged += new System.EventHandler(this.krajVazenja_dtp_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Kraj Vazenja:";
            // 
            // pocetakVazenja_dtp
            // 
            this.pocetakVazenja_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.pocetakVazenja_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.pocetakVazenja_dtp.Location = new System.Drawing.Point(106, 29);
            this.pocetakVazenja_dtp.Name = "pocetakVazenja_dtp";
            this.pocetakVazenja_dtp.Size = new System.Drawing.Size(144, 20);
            this.pocetakVazenja_dtp.TabIndex = 6;
            this.pocetakVazenja_dtp.ValueChanged += new System.EventHandler(this.pocetakVazenja_dtp_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Pocetak Vazenja:";
            // 
            // datumDokumenta_dtp
            // 
            this.datumDokumenta_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.datumDokumenta_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datumDokumenta_dtp.Location = new System.Drawing.Point(114, 3);
            this.datumDokumenta_dtp.Name = "datumDokumenta_dtp";
            this.datumDokumenta_dtp.Size = new System.Drawing.Size(144, 20);
            this.datumDokumenta_dtp.TabIndex = 3;
            this.datumDokumenta_dtp.ValueChanged += new System.EventHandler(this.datumDokumenta_dtp_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Datum Dokumenta:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btn_OdbaciIzmene);
            this.panel1.Controls.Add(this.btn_Sacuvaj);
            this.panel1.Controls.Add(this.osvezi_btn);
            this.panel1.Controls.Add(this.nova_btn);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(942, 35);
            this.panel1.TabIndex = 24;
            // 
            // osvezi_btn
            // 
            this.osvezi_btn.BackgroundImage = global::TDOffice_v2.Properties.Resources.refresh_button;
            this.osvezi_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.osvezi_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.osvezi_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.osvezi_btn.Location = new System.Drawing.Point(148, 3);
            this.osvezi_btn.Name = "osvezi_btn";
            this.osvezi_btn.Size = new System.Drawing.Size(27, 29);
            this.osvezi_btn.TabIndex = 14;
            this.osvezi_btn.UseVisualStyleBackColor = true;
            this.osvezi_btn.Click += new System.EventHandler(this.osvezi_btn_Click);
            // 
            // nova_btn
            // 
            this.nova_btn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.nova_btn.BackgroundImage = global::TDOffice_v2.Properties.Resources.new_icon;
            this.nova_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.nova_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nova_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.nova_btn.Location = new System.Drawing.Point(4, 3);
            this.nova_btn.Name = "nova_btn";
            this.nova_btn.Size = new System.Drawing.Size(27, 29);
            this.nova_btn.TabIndex = 13;
            this.nova_btn.UseVisualStyleBackColor = false;
            this.nova_btn.Click += new System.EventHandler(this.nova_btn_Click);
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 142);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(942, 437);
            this.dataGridView1.TabIndex = 23;
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
            // 
            // btn_OdbaciIzmene
            // 
            this.btn_OdbaciIzmene.BackgroundImage = global::TDOffice_v2.Properties.Resources.discard_icon;
            this.btn_OdbaciIzmene.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_OdbaciIzmene.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_OdbaciIzmene.Location = new System.Drawing.Point(84, 3);
            this.btn_OdbaciIzmene.Name = "btn_OdbaciIzmene";
            this.btn_OdbaciIzmene.Size = new System.Drawing.Size(33, 29);
            this.btn_OdbaciIzmene.TabIndex = 41;
            this.btn_OdbaciIzmene.UseVisualStyleBackColor = true;
            this.btn_OdbaciIzmene.EnabledChanged += new System.EventHandler(this.btn_OdbaciIzmene_EnabledChanged);
            this.btn_OdbaciIzmene.Click += new System.EventHandler(this.btn_OdbaciIzmene_Click);
            // 
            // btn_Sacuvaj
            // 
            this.btn_Sacuvaj.BackgroundImage = global::TDOffice_v2.Properties.Resources.save_icon;
            this.btn_Sacuvaj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Sacuvaj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Sacuvaj.Location = new System.Drawing.Point(50, 3);
            this.btn_Sacuvaj.Name = "btn_Sacuvaj";
            this.btn_Sacuvaj.Size = new System.Drawing.Size(28, 29);
            this.btn_Sacuvaj.TabIndex = 40;
            this.btn_Sacuvaj.UseVisualStyleBackColor = true;
            this.btn_Sacuvaj.EnabledChanged += new System.EventHandler(this.btn_OdbaciIzmene_EnabledChanged);
            this.btn_Sacuvaj.Click += new System.EventHandler(this.btn_Sacuvaj_Click);
            // 
            // fm_UlaznaPonuda_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 591);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "fm_UlaznaPonuda_Index";
            this.Text = "Ulazna Ponuda";
            this.Load += new System.EventHandler(this.fm_UlaznaPonuda_Index_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker datumDokumenta_dtp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button osvezi_btn;
        private System.Windows.Forms.Button nova_btn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker krajVazenja_dtp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker pocetakVazenja_dtp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_OdbaciIzmene;
        private System.Windows.Forms.Button btn_Sacuvaj;
    }
}