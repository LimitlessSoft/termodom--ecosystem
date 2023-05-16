namespace TDOffice_v2
{
    partial class fm_LagerListaStavki_Index
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vrednosno_cb = new System.Windows.Forms.CheckBox();
            this.kolicinski_cb = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 99);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1689, 571);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Sorted += new System.EventHandler(this.dataGridView1_Sorted);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.vrednosno_cb);
            this.groupBox1.Controls.Add(this.kolicinski_cb);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(1275, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 81);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filteri";
            // 
            // vrednosno_cb
            // 
            this.vrednosno_cb.AutoSize = true;
            this.vrednosno_cb.Checked = true;
            this.vrednosno_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.vrednosno_cb.Location = new System.Drawing.Point(6, 42);
            this.vrednosno_cb.Name = "vrednosno_cb";
            this.vrednosno_cb.Size = new System.Drawing.Size(77, 17);
            this.vrednosno_cb.TabIndex = 1;
            this.vrednosno_cb.Text = "Vrednosno";
            this.vrednosno_cb.UseVisualStyleBackColor = true;
            this.vrednosno_cb.CheckedChanged += new System.EventHandler(this.vrednosno_cb_CheckedChanged);
            // 
            // kolicinski_cb
            // 
            this.kolicinski_cb.AutoSize = true;
            this.kolicinski_cb.Checked = true;
            this.kolicinski_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.kolicinski_cb.Location = new System.Drawing.Point(6, 19);
            this.kolicinski_cb.Name = "kolicinski_cb";
            this.kolicinski_cb.Size = new System.Drawing.Size(70, 17);
            this.kolicinski_cb.TabIndex = 0;
            this.kolicinski_cb.Text = "Kolicinski";
            this.kolicinski_cb.UseVisualStyleBackColor = true;
            this.kolicinski_cb.CheckedChanged += new System.EventHandler(this.kolicinski_cb_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ucitavanje... Sacekajte...";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1107, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Ukloni redove bez greske";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fm_LagerListaStavki_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 682);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "fm_LagerListaStavki_Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stanje robe po godinama";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.fm_LagerListaStavki_Index_Load);
            this.Shown += new System.EventHandler(this.fm_LagerListaStavki_Index_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox vrednosno_cb;
        private System.Windows.Forms.CheckBox kolicinski_cb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}