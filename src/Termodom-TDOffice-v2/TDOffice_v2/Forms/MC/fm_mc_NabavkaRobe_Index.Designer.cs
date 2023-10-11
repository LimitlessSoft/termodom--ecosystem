namespace TDOffice_v2.Forms.MC
{
    partial class fm_mc_NabavkaRobe_Index
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
            components = new System.ComponentModel.Container();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            poveziSaRobomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            uvuciCenovnik_btn = new System.Windows.Forms.Button();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.Location = new System.Drawing.Point(12, 41);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new System.Drawing.Size(948, 486);
            dataGridView1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { poveziSaRobomToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(162, 26);
            // 
            // poveziSaRobomToolStripMenuItem
            // 
            poveziSaRobomToolStripMenuItem.Name = "poveziSaRobomToolStripMenuItem";
            poveziSaRobomToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            poveziSaRobomToolStripMenuItem.Text = "Povezi sa robom";
            poveziSaRobomToolStripMenuItem.Click += poveziSaRobomToolStripMenuItem_Click;
            // 
            // uvuciCenovnik_btn
            // 
            uvuciCenovnik_btn.Location = new System.Drawing.Point(291, 12);
            uvuciCenovnik_btn.Name = "uvuciCenovnik_btn";
            uvuciCenovnik_btn.Size = new System.Drawing.Size(160, 23);
            uvuciCenovnik_btn.TabIndex = 1;
            uvuciCenovnik_btn.Text = "Izaberi Fajl";
            uvuciCenovnik_btn.UseVisualStyleBackColor = true;
            uvuciCenovnik_btn.Click += uvuciCenovnik_btn_Click;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(77, 12);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(208, 23);
            comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(59, 15);
            label1.TabIndex = 3;
            label1.Text = "Dobavljac";
            // 
            // fm_mc_NabavkaRobe_Index
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(972, 539);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            Controls.Add(uvuciCenovnik_btn);
            Controls.Add(dataGridView1);
            Name = "fm_mc_NabavkaRobe_Index";
            Text = "fm_mc_NabavkaRobe_Index";
            Load += fm_mc_NabavkaRobe_Index_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button uvuciCenovnik_btn;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem poveziSaRobomToolStripMenuItem;
    }
}