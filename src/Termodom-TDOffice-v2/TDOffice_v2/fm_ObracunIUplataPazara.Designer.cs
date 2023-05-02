namespace TDOffice_v2
{
    partial class fm_ObracunIUplataPazara
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
            panel1 = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();
            ukupnaRazlika_txt = new System.Windows.Forms.TextBox();
            tb_tolerancija = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            clb_Magacini = new System.Windows.Forms.CheckedListBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            cekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            decekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            btn_Prikazi = new System.Windows.Forms.Button();
            doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            panel2 = new System.Windows.Forms.Panel();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            panel1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.Controls.Add(label4);
            panel1.Controls.Add(ukupnaRazlika_txt);
            panel1.Controls.Add(tb_tolerancija);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(clb_Magacini);
            panel1.Controls.Add(btn_Prikazi);
            panel1.Controls.Add(doDatuma_dtp);
            panel1.Controls.Add(odDatuma_dtp);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Location = new System.Drawing.Point(10, 9);
            panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(680, 178);
            panel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(419, 158);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(84, 15);
            label4.TabIndex = 12;
            label4.Text = "Ukupna razlika";
            // 
            // ukupnaRazlika_txt
            // 
            ukupnaRazlika_txt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ukupnaRazlika_txt.BackColor = System.Drawing.SystemColors.Info;
            ukupnaRazlika_txt.Location = new System.Drawing.Point(509, 153);
            ukupnaRazlika_txt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ukupnaRazlika_txt.Name = "ukupnaRazlika_txt";
            ukupnaRazlika_txt.ReadOnly = true;
            ukupnaRazlika_txt.Size = new System.Drawing.Size(160, 23);
            ukupnaRazlika_txt.TabIndex = 11;
            ukupnaRazlika_txt.Text = "0";
            ukupnaRazlika_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tb_tolerancija
            // 
            tb_tolerancija.Location = new System.Drawing.Point(139, 78);
            tb_tolerancija.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tb_tolerancija.Name = "tb_tolerancija";
            tb_tolerancija.Size = new System.Drawing.Size(78, 23);
            tb_tolerancija.TabIndex = 10;
            tb_tolerancija.Text = "30";
            tb_tolerancija.KeyPress += tb_tolerancija_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(154, 61);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(63, 15);
            label1.TabIndex = 9;
            label1.Text = "Tolerancija";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(3, 154);
            button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(82, 22);
            button1.TabIndex = 8;
            button1.Text = "HELP";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // clb_Magacini
            // 
            clb_Magacini.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            clb_Magacini.CheckOnClick = true;
            clb_Magacini.ColumnWidth = 300;
            clb_Magacini.ContextMenuStrip = contextMenuStrip1;
            clb_Magacini.FormattingEnabled = true;
            clb_Magacini.Location = new System.Drawing.Point(227, 9);
            clb_Magacini.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            clb_Magacini.MultiColumn = true;
            clb_Magacini.Name = "clb_Magacini";
            clb_Magacini.Size = new System.Drawing.Size(442, 130);
            clb_Magacini.TabIndex = 7;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { cekirajSveToolStripMenuItem, toolStripSeparator1, decekirajSveToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(143, 54);
            // 
            // cekirajSveToolStripMenuItem
            // 
            cekirajSveToolStripMenuItem.Name = "cekirajSveToolStripMenuItem";
            cekirajSveToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            cekirajSveToolStripMenuItem.Text = "Cekiraj sve";
            cekirajSveToolStripMenuItem.Click += cekirajSveToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // decekirajSveToolStripMenuItem
            // 
            decekirajSveToolStripMenuItem.Name = "decekirajSveToolStripMenuItem";
            decekirajSveToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            decekirajSveToolStripMenuItem.Text = "Decekiraj sve";
            decekirajSveToolStripMenuItem.Click += decekirajSveToolStripMenuItem_Click;
            // 
            // btn_Prikazi
            // 
            btn_Prikazi.Location = new System.Drawing.Point(135, 105);
            btn_Prikazi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            btn_Prikazi.Name = "btn_Prikazi";
            btn_Prikazi.Size = new System.Drawing.Size(82, 22);
            btn_Prikazi.TabIndex = 6;
            btn_Prikazi.Text = "Prikazi";
            btn_Prikazi.UseVisualStyleBackColor = true;
            btn_Prikazi.Click += btn_Prikazi_Click;
            // 
            // doDatuma_dtp
            // 
            doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            doDatuma_dtp.Location = new System.Drawing.Point(47, 36);
            doDatuma_dtp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            doDatuma_dtp.Name = "doDatuma_dtp";
            doDatuma_dtp.Size = new System.Drawing.Size(175, 23);
            doDatuma_dtp.TabIndex = 5;
            // 
            // odDatuma_dtp
            // 
            odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            odDatuma_dtp.Location = new System.Drawing.Point(47, 9);
            odDatuma_dtp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            odDatuma_dtp.Name = "odDatuma_dtp";
            odDatuma_dtp.Size = new System.Drawing.Size(175, 23);
            odDatuma_dtp.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(14, 40);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(25, 15);
            label3.TabIndex = 3;
            label3.Text = "Do:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 13);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(26, 15);
            label2.TabIndex = 2;
            label2.Text = "Od:";
            // 
            // panel2
            // 
            panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel2.Controls.Add(dataGridView1);
            panel2.Location = new System.Drawing.Point(13, 200);
            panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(675, 167);
            panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new System.Drawing.Size(675, 167);
            dataGridView1.TabIndex = 0;
            dataGridView1.Sorted += dataGridView1_Sorted;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new System.Drawing.Point(0, 365);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            statusStrip1.Size = new System.Drawing.Size(701, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // fm_ObracunIUplataPazara
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(701, 387);
            Controls.Add(statusStrip1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "fm_ObracunIUplataPazara";
            Text = "fm_ObracunIUplataPazara";
            Load += fm_ObracunIUplataPazara_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckedListBox clb_Magacini;
        private System.Windows.Forms.Button btn_Prikazi;
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cekirajSveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem decekirajSveToolStripMenuItem;
        private System.Windows.Forms.TextBox tb_tolerancija;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ukupnaRazlika_txt;
    }
}