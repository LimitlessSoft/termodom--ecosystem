namespace TDOffice_v2
{
    partial class fm_StanjeRacuna_Index
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
            dataGridView1 = new System.Windows.Forms.DataGridView();
            checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            godina_cmb = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            status_lbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new System.Drawing.Point(12, 94);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new System.Drawing.Size(1166, 456);
            dataGridView1.TabIndex = 0;
            // 
            // checkedListBox1
            // 
            checkedListBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new System.Drawing.Point(12, 12);
            checkedListBox1.MultiColumn = true;
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new System.Drawing.Size(974, 76);
            checkedListBox1.TabIndex = 1;
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // godina_cmb
            // 
            godina_cmb.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            godina_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            godina_cmb.FormattingEnabled = true;
            godina_cmb.Location = new System.Drawing.Point(1003, 35);
            godina_cmb.Name = "godina_cmb";
            godina_cmb.Size = new System.Drawing.Size(175, 23);
            godina_cmb.TabIndex = 2;
            godina_cmb.SelectedIndexChanged += godina_cmb_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(1003, 17);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(45, 15);
            label1.TabIndex = 3;
            label1.Text = "Godina";
            // 
            // status_lbl
            // 
            status_lbl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            status_lbl.AutoSize = true;
            status_lbl.Location = new System.Drawing.Point(1003, 73);
            status_lbl.Name = "status_lbl";
            status_lbl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            status_lbl.Size = new System.Drawing.Size(56, 15);
            status_lbl.TabIndex = 4;
            status_lbl.Text = "status_lbl";
            // 
            // fm_StanjeRacuna_Index
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1190, 562);
            Controls.Add(status_lbl);
            Controls.Add(label1);
            Controls.Add(godina_cmb);
            Controls.Add(checkedListBox1);
            Controls.Add(dataGridView1);
            Name = "fm_StanjeRacuna_Index";
            Text = "fm_StanjeRacuna_Index";
            Load += fm_StanjeRacuna_Index_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ComboBox godina_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label status_lbl;
    }
}