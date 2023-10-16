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
            doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            label2 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            comboBox2 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.Location = new System.Drawing.Point(12, 71);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new System.Drawing.Size(948, 427);
            dataGridView1.TabIndex = 0;
            dataGridView1.Sorted += dataGridView1_Sorted;
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
            // doDatuma_dtp
            // 
            doDatuma_dtp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            doDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            doDatuma_dtp.Location = new System.Drawing.Point(151, 504);
            doDatuma_dtp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            doDatuma_dtp.Name = "doDatuma_dtp";
            doDatuma_dtp.Size = new System.Drawing.Size(167, 23);
            doDatuma_dtp.TabIndex = 5;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(15, 508);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(127, 15);
            label2.TabIndex = 6;
            label2.Text = "Cenovnik vazi od jutra:";
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button1.Location = new System.Drawing.Point(325, 504);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(160, 23);
            button1.TabIndex = 7;
            button1.Text = "Sacuvaj cene u bazi";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(420, 43);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(117, 23);
            button2.TabIndex = 10;
            button2.Text = "Filter pretraga";
            button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(161, 43);
            textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(252, 23);
            textBox1.TabIndex = 9;
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "KatBr", "Naziv" });
            comboBox2.Location = new System.Drawing.Point(13, 42);
            comboBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(140, 23);
            comboBox2.TabIndex = 8;
            // 
            // fm_mc_NabavkaRobe_Index
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(972, 539);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(comboBox2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(doDatuma_dtp);
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
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}