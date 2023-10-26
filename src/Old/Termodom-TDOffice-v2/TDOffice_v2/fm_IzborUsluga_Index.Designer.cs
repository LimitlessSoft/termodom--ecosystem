namespace TDOffice_v2
{
    partial class fm_IzborUsluga_Index
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
            this.pretraga_txt = new System.Windows.Forms.TextBox();
            this.pretraga_cmb = new System.Windows.Forms.ComboBox();
            this.unesi_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.kolicina_txt = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.cena_txt = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pretraga_txt
            // 
            this.pretraga_txt.Location = new System.Drawing.Point(161, 10);
            this.pretraga_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pretraga_txt.Name = "pretraga_txt";
            this.pretraga_txt.Size = new System.Drawing.Size(252, 23);
            this.pretraga_txt.TabIndex = 11;
            // 
            // pretraga_cmb
            // 
            this.pretraga_cmb.FormattingEnabled = true;
            this.pretraga_cmb.Location = new System.Drawing.Point(13, 10);
            this.pretraga_cmb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pretraga_cmb.Name = "pretraga_cmb";
            this.pretraga_cmb.Size = new System.Drawing.Size(140, 23);
            this.pretraga_cmb.TabIndex = 10;
            // 
            // unesi_btn
            // 
            this.unesi_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unesi_btn.Location = new System.Drawing.Point(413, 414);
            this.unesi_btn.Name = "unesi_btn";
            this.unesi_btn.Size = new System.Drawing.Size(108, 26);
            this.unesi_btn.TabIndex = 9;
            this.unesi_btn.Text = "Unesi";
            this.unesi_btn.UseVisualStyleBackColor = true;
            this.unesi_btn.Click += new System.EventHandler(this.unesi_btn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 418);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Kolicina";
            // 
            // kolicina_txt
            // 
            this.kolicina_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kolicina_txt.Location = new System.Drawing.Point(67, 415);
            this.kolicina_txt.Name = "kolicina_txt";
            this.kolicina_txt.Size = new System.Drawing.Size(145, 23);
            this.kolicina_txt.TabIndex = 7;
            this.kolicina_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.kolicina_txt_KeyDown);
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 39);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(796, 370);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 418);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Cena";
            // 
            // cena_txt
            // 
            this.cena_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cena_txt.Location = new System.Drawing.Point(262, 415);
            this.cena_txt.Name = "cena_txt";
            this.cena_txt.Size = new System.Drawing.Size(145, 23);
            this.cena_txt.TabIndex = 12;
            this.cena_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cena_txt_KeyDown);
            // 
            // fm_IzborUsluga_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 452);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cena_txt);
            this.Controls.Add(this.pretraga_txt);
            this.Controls.Add(this.pretraga_cmb);
            this.Controls.Add(this.unesi_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kolicina_txt);
            this.Controls.Add(this.dataGridView1);
            this.Name = "fm_IzborUsluga_Index";
            this.Text = "fm_IzborUsluga_Index";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fm_IzborUsluga_Index_FormClosing);
            this.Load += new System.EventHandler(this.fm_IzborUsluga_Index_Load);
            this.Shown += new System.EventHandler(this.fm_IzborUsluga_Index_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pretraga_txt;
        private System.Windows.Forms.ComboBox pretraga_cmb;
        private System.Windows.Forms.Button unesi_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox kolicina_txt;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox cena_txt;
    }
}