namespace TDOffice_v2
{
    partial class fm_NelogicniNaciniUplata_Index
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
            this.btn_ProveriNelogicnostiNacinaPlacanja = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slogova_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.clb_Filter = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_ProveriNelogicnostiNacinaPlacanja
            // 
            this.btn_ProveriNelogicnostiNacinaPlacanja.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ProveriNelogicnostiNacinaPlacanja.Location = new System.Drawing.Point(12, 9);
            this.btn_ProveriNelogicnostiNacinaPlacanja.Name = "btn_ProveriNelogicnostiNacinaPlacanja";
            this.btn_ProveriNelogicnostiNacinaPlacanja.Size = new System.Drawing.Size(383, 51);
            this.btn_ProveriNelogicnostiNacinaPlacanja.TabIndex = 0;
            this.btn_ProveriNelogicnostiNacinaPlacanja.Text = "Proveri nelogicnosti nacina placanja";
            this.btn_ProveriNelogicnostiNacinaPlacanja.UseVisualStyleBackColor = true;
            this.btn_ProveriNelogicnostiNacinaPlacanja.Click += new System.EventHandler(this.btn_ProveriNelogicnostiNacinaPlacanja_Click);
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 121);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(919, 300);
            this.dataGridView1.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slogova_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 424);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(943, 26);
            this.statusStrip1.TabIndex = 22;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slogova_lbl
            // 
            this.slogova_lbl.Name = "slogova_lbl";
            this.slogova_lbl.Size = new System.Drawing.Size(70, 20);
            this.slogova_lbl.Text = "Slogova: ";
            // 
            // clb_Filter
            // 
            this.clb_Filter.FormattingEnabled = true;
            this.clb_Filter.Items.AddRange(new object[] {
            "Razlicite nacine uplata od istorije uplata",
            "Razliciti iznos od istorije uplata",
            "Virmanske racune gde je uplata manja od iznosa racuna"});
            this.clb_Filter.Location = new System.Drawing.Point(401, 36);
            this.clb_Filter.Name = "clb_Filter";
            this.clb_Filter.Size = new System.Drawing.Size(336, 79);
            this.clb_Filter.TabIndex = 25;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(764, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 47);
            this.button1.TabIndex = 26;
            this.button1.Text = "Primeni Filter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(401, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 15);
            this.label1.TabIndex = 27;
            this.label1.Text = "Uzmi u obzir dokumente koji imaju:";
            // 
            // fm_NelogicniNaciniUplata_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.clb_Filter);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_ProveriNelogicnostiNacinaPlacanja);
            this.Name = "fm_NelogicniNaciniUplata_Index";
            this.Text = "Nelogicni Nacini Uplata";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ProveriNelogicnostiNacinaPlacanja;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slogova_lbl;
        private System.Windows.Forms.CheckedListBox clb_Filter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
    }
}