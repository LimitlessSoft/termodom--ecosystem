namespace TDOffice_v2
{
    partial class fm_Partner_Analiza_Retro_Popust_Index
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
            this.analiziraj_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.partner_cmb = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.mesec_cmb = new System.Windows.Forms.ComboBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.nacinUplate_clb = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // analiziraj_btn
            // 
            this.analiziraj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.analiziraj_btn.Enabled = false;
            this.analiziraj_btn.Location = new System.Drawing.Point(676, 25);
            this.analiziraj_btn.Name = "analiziraj_btn";
            this.analiziraj_btn.Size = new System.Drawing.Size(112, 78);
            this.analiziraj_btn.TabIndex = 5;
            this.analiziraj_btn.Text = "Analiziraj";
            this.analiziraj_btn.UseVisualStyleBackColor = true;
            this.analiziraj_btn.Click += new System.EventHandler(this.analiziraj_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Partner:";
            // 
            // partner_cmb
            // 
            this.partner_cmb.Enabled = false;
            this.partner_cmb.FormattingEnabled = true;
            this.partner_cmb.Location = new System.Drawing.Point(12, 25);
            this.partner_cmb.Name = "partner_cmb";
            this.partner_cmb.Size = new System.Drawing.Size(342, 21);
            this.partner_cmb.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 106);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(426, 332);
            this.dataGridView1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Mesec:";
            // 
            // mesec_cmb
            // 
            this.mesec_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mesec_cmb.Enabled = false;
            this.mesec_cmb.FormattingEnabled = true;
            this.mesec_cmb.Location = new System.Drawing.Point(60, 52);
            this.mesec_cmb.Name = "mesec_cmb";
            this.mesec_cmb.Size = new System.Drawing.Size(213, 21);
            this.mesec_cmb.TabIndex = 8;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(444, 106);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.Size = new System.Drawing.Size(344, 332);
            this.dataGridView2.TabIndex = 9;
            // 
            // nacinUplate_clb
            // 
            this.nacinUplate_clb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nacinUplate_clb.CheckOnClick = true;
            this.nacinUplate_clb.FormattingEnabled = true;
            this.nacinUplate_clb.Location = new System.Drawing.Point(360, 25);
            this.nacinUplate_clb.Name = "nacinUplate_clb";
            this.nacinUplate_clb.Size = new System.Drawing.Size(310, 79);
            this.nacinUplate_clb.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.IndianRed;
            this.label3.Location = new System.Drawing.Point(66, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(225, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Sve prikazane vrednosti su bez PDV-a";
            // 
            // fm_Partner_Analiza_Retro_Popust_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nacinUplate_clb);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.mesec_cmb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.analiziraj_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.partner_cmb);
            this.Name = "fm_Partner_Analiza_Retro_Popust_Index";
            this.Text = "fm_Partner_Analiza_Retro_Popust_Index";
            this.Load += new System.EventHandler(this.fm_Partner_Analiza_Retro_Popust_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button analiziraj_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox partner_cmb;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox mesec_cmb;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.CheckedListBox nacinUplate_clb;
        private System.Windows.Forms.Label label3;
    }
}