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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.ukupnaRazlika_txt = new System.Windows.Forms.TextBox();
            this.tb_tolerancija = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.clb_Magacini = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.decekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_Prikazi = new System.Windows.Forms.Button();
            this.doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.analitikaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.ukupnaRazlika_txt);
            this.panel1.Controls.Add(this.tb_tolerancija);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.clb_Magacini);
            this.panel1.Controls.Add(this.btn_Prikazi);
            this.panel1.Controls.Add(this.doDatuma_dtp);
            this.panel1.Controls.Add(this.odDatuma_dtp);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(10, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1003, 178);
            this.panel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(742, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "Ukupna razlika";
            // 
            // ukupnaRazlika_txt
            // 
            this.ukupnaRazlika_txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ukupnaRazlika_txt.BackColor = System.Drawing.SystemColors.Info;
            this.ukupnaRazlika_txt.Location = new System.Drawing.Point(832, 153);
            this.ukupnaRazlika_txt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ukupnaRazlika_txt.Name = "ukupnaRazlika_txt";
            this.ukupnaRazlika_txt.ReadOnly = true;
            this.ukupnaRazlika_txt.Size = new System.Drawing.Size(160, 23);
            this.ukupnaRazlika_txt.TabIndex = 11;
            this.ukupnaRazlika_txt.Text = "0";
            this.ukupnaRazlika_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tb_tolerancija
            // 
            this.tb_tolerancija.Location = new System.Drawing.Point(139, 78);
            this.tb_tolerancija.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_tolerancija.Name = "tb_tolerancija";
            this.tb_tolerancija.Size = new System.Drawing.Size(78, 23);
            this.tb_tolerancija.TabIndex = 10;
            this.tb_tolerancija.Text = "30";
            this.tb_tolerancija.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_tolerancija_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Tolerancija";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 154);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 22);
            this.button1.TabIndex = 8;
            this.button1.Text = "HELP";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // clb_Magacini
            // 
            this.clb_Magacini.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clb_Magacini.CheckOnClick = true;
            this.clb_Magacini.ColumnWidth = 300;
            this.clb_Magacini.ContextMenuStrip = this.contextMenuStrip1;
            this.clb_Magacini.FormattingEnabled = true;
            this.clb_Magacini.Location = new System.Drawing.Point(227, 9);
            this.clb_Magacini.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clb_Magacini.MultiColumn = true;
            this.clb_Magacini.Name = "clb_Magacini";
            this.clb_Magacini.Size = new System.Drawing.Size(765, 130);
            this.clb_Magacini.TabIndex = 7;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cekirajSveToolStripMenuItem,
            this.toolStripSeparator1,
            this.decekirajSveToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(143, 54);
            // 
            // cekirajSveToolStripMenuItem
            // 
            this.cekirajSveToolStripMenuItem.Name = "cekirajSveToolStripMenuItem";
            this.cekirajSveToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.cekirajSveToolStripMenuItem.Text = "Cekiraj sve";
            this.cekirajSveToolStripMenuItem.Click += new System.EventHandler(this.cekirajSveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // decekirajSveToolStripMenuItem
            // 
            this.decekirajSveToolStripMenuItem.Name = "decekirajSveToolStripMenuItem";
            this.decekirajSveToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.decekirajSveToolStripMenuItem.Text = "Decekiraj sve";
            this.decekirajSveToolStripMenuItem.Click += new System.EventHandler(this.decekirajSveToolStripMenuItem_Click);
            // 
            // btn_Prikazi
            // 
            this.btn_Prikazi.Location = new System.Drawing.Point(135, 105);
            this.btn_Prikazi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Prikazi.Name = "btn_Prikazi";
            this.btn_Prikazi.Size = new System.Drawing.Size(82, 22);
            this.btn_Prikazi.TabIndex = 6;
            this.btn_Prikazi.Text = "Prikazi";
            this.btn_Prikazi.UseVisualStyleBackColor = true;
            this.btn_Prikazi.Click += new System.EventHandler(this.btn_Prikazi_Click);
            // 
            // doDatuma_dtp
            // 
            this.doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.doDatuma_dtp.Location = new System.Drawing.Point(47, 36);
            this.doDatuma_dtp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.doDatuma_dtp.Name = "doDatuma_dtp";
            this.doDatuma_dtp.Size = new System.Drawing.Size(175, 23);
            this.doDatuma_dtp.TabIndex = 5;
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.odDatuma_dtp.Location = new System.Drawing.Point(47, 9);
            this.odDatuma_dtp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(175, 23);
            this.odDatuma_dtp.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Do:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Od:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Location = new System.Drawing.Point(13, 200);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(998, 335);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(998, 335);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Sorted += new System.EventHandler(this.dataGridView1_Sorted);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analitikaToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(121, 26);
            // 
            // analitikaToolStripMenuItem
            // 
            this.analitikaToolStripMenuItem.Name = "analitikaToolStripMenuItem";
            this.analitikaToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.analitikaToolStripMenuItem.Text = "Analitika";
            this.analitikaToolStripMenuItem.Click += new System.EventHandler(this.analitikaToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 533);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1024, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(227, 150);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(379, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Proveri da li ima neispravno unetih stavki izvoda uplate pazara";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // fm_ObracunIUplataPazara
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 555);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "fm_ObracunIUplataPazara";
            this.Text = "fm_ObracunIUplataPazara";
            this.Load += new System.EventHandler(this.fm_ObracunIUplataPazara_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem analitikaToolStripMenuItem;
        private System.Windows.Forms.Button button2;
    }
}