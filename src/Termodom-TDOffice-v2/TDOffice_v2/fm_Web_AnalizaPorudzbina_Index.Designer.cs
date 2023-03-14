namespace TDOffice_v2
{
    partial class fm_Web_AnalizaPorudzbina_Index
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.brojDokumenata_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.realizovano_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.filtriraj_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 104);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1432, 569);
            this.dataGridView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.brojDokumenata_lbl,
            this.realizovano_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 676);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1456, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // brojDokumenata_lbl
            // 
            this.brojDokumenata_lbl.Name = "brojDokumenata_lbl";
            this.brojDokumenata_lbl.Size = new System.Drawing.Size(102, 17);
            this.brojDokumenata_lbl.Text = "Broj Dokumenata:";
            // 
            // realizovano_lbl
            // 
            this.realizovano_lbl.Name = "realizovano_lbl";
            this.realizovano_lbl.Size = new System.Drawing.Size(73, 17);
            this.realizovano_lbl.Text = "Realizovano:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(836, 86);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filteri";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.filtriraj_btn);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.magacin_cmb);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.odDatuma_dtp);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.doDatuma_dtp);
            this.panel2.Location = new System.Drawing.Point(6, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(824, 58);
            this.panel2.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Magacin";
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(77, 29);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(228, 21);
            this.magacin_cmb.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Do datuma:";
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odDatuma_dtp.Location = new System.Drawing.Point(77, 3);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.odDatuma_dtp.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Od datuma:";
            // 
            // doDatuma_dtp
            // 
            this.doDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.doDatuma_dtp.Location = new System.Drawing.Point(295, 3);
            this.doDatuma_dtp.Name = "doDatuma_dtp";
            this.doDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.doDatuma_dtp.TabIndex = 4;
            // 
            // filtriraj_btn
            // 
            this.filtriraj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filtriraj_btn.Location = new System.Drawing.Point(648, 3);
            this.filtriraj_btn.Name = "filtriraj_btn";
            this.filtriraj_btn.Size = new System.Drawing.Size(173, 52);
            this.filtriraj_btn.TabIndex = 9;
            this.filtriraj_btn.Text = "Filtriraj";
            this.filtriraj_btn.UseVisualStyleBackColor = true;
            this.filtriraj_btn.Click += new System.EventHandler(this.filtriraj_btn_Click);
            // 
            // fm_Web_AnalizaPorudzbina_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1456, 698);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "fm_Web_AnalizaPorudzbina_Index";
            this.Text = "Analiza Porudzbina";
            this.Load += new System.EventHandler(this.fm_Web_AnalizaPorudzbina_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel brojDokumenata_lbl;
        private System.Windows.Forms.ToolStripStatusLabel realizovano_lbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
        private System.Windows.Forms.Button filtriraj_btn;
    }
}