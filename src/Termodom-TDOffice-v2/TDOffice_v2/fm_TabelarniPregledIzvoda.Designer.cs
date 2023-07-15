namespace TDOffice_v2
{
    partial class fm_TabelarniPregledIzvoda
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.od_dtp = new System.Windows.Forms.DateTimePicker();
            this.btn_UvuciIzvode = new System.Windows.Forms.Button();
            this.baza_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.od_dtp);
            this.panel1.Controls.Add(this.btn_UvuciIzvode);
            this.panel1.Controls.Add(this.baza_cmb);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(859, 92);
            this.panel1.TabIndex = 0;
            // 
            // od_dtp
            // 
            this.od_dtp.Location = new System.Drawing.Point(383, 36);
            this.od_dtp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.od_dtp.Name = "od_dtp";
            this.od_dtp.Size = new System.Drawing.Size(165, 27);
            this.od_dtp.TabIndex = 6;
            // 
            // btn_UvuciIzvode
            // 
            this.btn_UvuciIzvode.Location = new System.Drawing.Point(665, 34);
            this.btn_UvuciIzvode.Name = "btn_UvuciIzvode";
            this.btn_UvuciIzvode.Size = new System.Drawing.Size(159, 29);
            this.btn_UvuciIzvode.TabIndex = 5;
            this.btn_UvuciIzvode.Text = "Uvuci izvode";
            this.btn_UvuciIzvode.UseVisualStyleBackColor = true;
            this.btn_UvuciIzvode.Click += new System.EventHandler(this.btn_UvuciIzvode_Click);
            // 
            // baza_cmb
            // 
            this.baza_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.baza_cmb.Enabled = false;
            this.baza_cmb.FormattingEnabled = true;
            this.baza_cmb.Location = new System.Drawing.Point(13, 35);
            this.baza_cmb.Margin = new System.Windows.Forms.Padding(5);
            this.baza_cmb.Name = "baza_cmb";
            this.baza_cmb.Size = new System.Drawing.Size(291, 28);
            this.baza_cmb.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Izbor baze";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Location = new System.Drawing.Point(12, 127);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(857, 324);
            this.panel2.TabIndex = 1;
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
            this.dataGridView1.Location = new System.Drawing.Point(4, 5);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(849, 314);
            this.dataGridView1.TabIndex = 1;
            // 
            // fm_TabelarniPregledIzvoda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 496);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fm_TabelarniPregledIzvoda";
            this.Text = "fm_TabelarniPregledIzvoda";
            this.Load += new System.EventHandler(this.fm_TabelarniPregledIzvoda_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox baza_cmb;
        private System.Windows.Forms.Button btn_UvuciIzvode;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker od_dtp;
    }
}