namespace TDOffice_v2
{
    partial class fm_IzborRobe_v2_Index
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
            this.kolicina_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.unesi_btn = new System.Windows.Forms.Button();
            this.pretraga_txt = new System.Windows.Forms.TextBox();
            this.pretraga_cmb = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 41);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(705, 302);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // kolicina_txt
            // 
            this.kolicina_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kolicina_txt.Location = new System.Drawing.Point(67, 357);
            this.kolicina_txt.Name = "kolicina_txt";
            this.kolicina_txt.Size = new System.Drawing.Size(145, 23);
            this.kolicina_txt.TabIndex = 1;
            this.kolicina_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.kolicina_txt_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 360);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Kolicina";
            // 
            // unesi_btn
            // 
            this.unesi_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unesi_btn.Location = new System.Drawing.Point(218, 357);
            this.unesi_btn.Name = "unesi_btn";
            this.unesi_btn.Size = new System.Drawing.Size(82, 23);
            this.unesi_btn.TabIndex = 3;
            this.unesi_btn.Text = "Unesi";
            this.unesi_btn.UseVisualStyleBackColor = true;
            this.unesi_btn.Click += new System.EventHandler(this.unesi_btn_Click);
            // 
            // pretraga_txt
            // 
            this.pretraga_txt.Location = new System.Drawing.Point(161, 12);
            this.pretraga_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pretraga_txt.Name = "pretraga_txt";
            this.pretraga_txt.Size = new System.Drawing.Size(252, 23);
            this.pretraga_txt.TabIndex = 5;
            // 
            // pretraga_cmb
            // 
            this.pretraga_cmb.FormattingEnabled = true;
            this.pretraga_cmb.Location = new System.Drawing.Point(13, 12);
            this.pretraga_cmb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pretraga_cmb.Name = "pretraga_cmb";
            this.pretraga_cmb.Size = new System.Drawing.Size(140, 23);
            this.pretraga_cmb.TabIndex = 4;
            // 
            // fm_IzborRobe_v2_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 392);
            this.Controls.Add(this.pretraga_txt);
            this.Controls.Add(this.pretraga_cmb);
            this.Controls.Add(this.unesi_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kolicina_txt);
            this.Controls.Add(this.dataGridView1);
            this.Name = "fm_IzborRobe_v2_Index";
            this.Text = "fm_IzborRobe_v2_Index";
            this.Load += new System.EventHandler(this.fm_IzborRobe_v2_Index_Load);
            this.Shown += new System.EventHandler(this.fm_IzborRobe_v2_Index_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox kolicina_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button unesi_btn;
        private System.Windows.Forms.TextBox pretraga_txt;
        private System.Windows.Forms.ComboBox pretraga_cmb;
    }
}