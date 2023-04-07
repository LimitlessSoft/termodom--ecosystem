namespace TDOffice_v2
{
    partial class fm_FiskalniRacuni_list
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
            this.uvuciFiskalne_btn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.senderFirma_cmb = new System.Windows.Forms.ComboBox();
            this.pib_txt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tipTransakcije_clb = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.dataGridView1.Location = new System.Drawing.Point(18, 292);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1580, 391);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Sorted += new System.EventHandler(this.dataGridView1_Sorted);
            // 
            // uvuciFiskalne_btn
            // 
            this.uvuciFiskalne_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uvuciFiskalne_btn.Location = new System.Drawing.Point(1014, 9);
            this.uvuciFiskalne_btn.Margin = new System.Windows.Forms.Padding(4);
            this.uvuciFiskalne_btn.Name = "uvuciFiskalne_btn";
            this.uvuciFiskalne_btn.Size = new System.Drawing.Size(144, 26);
            this.uvuciFiskalne_btn.TabIndex = 1;
            this.uvuciFiskalne_btn.Text = "Uvuci Fiskalne";
            this.uvuciFiskalne_btn.UseVisualStyleBackColor = true;
            this.uvuciFiskalne_btn.Click += new System.EventHandler(this.uvuciFiskalne_btn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(85, 68);
            this.magacin_cmb.Margin = new System.Windows.Forms.Padding(4);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(455, 23);
            this.magacin_cmb.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 71);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Fiskalizator";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.senderFirma_cmb);
            this.panel1.Controls.Add(this.pib_txt);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.uvuciFiskalne_btn);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.tipTransakcije_clb);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.odDatuma_dtp);
            this.panel1.Controls.Add(this.magacin_cmb);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.doDatuma_dtp);
            this.panel1.Location = new System.Drawing.Point(14, 14);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1165, 272);
            this.panel1.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 43);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 15);
            this.label5.TabIndex = 25;
            this.label5.Text = "Firma";
            // 
            // senderFirma_cmb
            // 
            this.senderFirma_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.senderFirma_cmb.FormattingEnabled = true;
            this.senderFirma_cmb.Location = new System.Drawing.Point(57, 40);
            this.senderFirma_cmb.Margin = new System.Windows.Forms.Padding(4);
            this.senderFirma_cmb.Name = "senderFirma_cmb";
            this.senderFirma_cmb.Size = new System.Drawing.Size(225, 23);
            this.senderFirma_cmb.TabIndex = 24;
            this.senderFirma_cmb.SelectedIndexChanged += new System.EventHandler(this.senderFirma_cmb_SelectedIndexChanged);
            // 
            // pib_txt
            // 
            this.pib_txt.Location = new System.Drawing.Point(228, 99);
            this.pib_txt.Margin = new System.Windows.Forms.Padding(4);
            this.pib_txt.Name = "pib_txt";
            this.pib_txt.Size = new System.Drawing.Size(214, 23);
            this.pib_txt.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 103);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(199, 15);
            this.label4.TabIndex = 22;
            this.label4.Text = "PIB (ostaviti prazno ako se ne koristi)";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1014, 88);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(147, 99);
            this.button2.TabIndex = 20;
            this.button2.Text = "Izdvoj neslaganja sa komercijalnim poslovanjem za dati period";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(634, 148);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(209, 16);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = "Ima Povratnicu Vezanu Za Sebe";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tipTransakcije_clb
            // 
            this.tipTransakcije_clb.CheckOnClick = true;
            this.tipTransakcije_clb.FormattingEnabled = true;
            this.tipTransakcije_clb.Items.AddRange(new object[] {
            "Prodaja - Promet",
            "Prodaja - Kopija",
            "Prodaja - Obuka",
            "Prodaja - Avans",
            "Povracaj - Promet",
            "Povracaj - Kopija",
            "Povracaj - Obuka",
            "Povracaj - Avans"});
            this.tipTransakcije_clb.Location = new System.Drawing.Point(4, 159);
            this.tipTransakcije_clb.Margin = new System.Windows.Forms.Padding(4);
            this.tipTransakcije_clb.Name = "tipTransakcije_clb";
            this.tipTransakcije_clb.Size = new System.Drawing.Size(498, 94);
            this.tipTransakcije_clb.TabIndex = 19;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(634, 171);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(209, 64);
            this.button1.TabIndex = 18;
            this.button1.Text = "Prikazi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "Do datuma:";
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odDatuma_dtp.Location = new System.Drawing.Point(95, 9);
            this.odDatuma_dtp.Margin = new System.Windows.Forms.Padding(4);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(152, 23);
            this.odDatuma_dtp.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Od datuma:";
            // 
            // doDatuma_dtp
            // 
            this.doDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.doDatuma_dtp.Location = new System.Drawing.Point(350, 9);
            this.doDatuma_dtp.Margin = new System.Windows.Forms.Padding(4);
            this.doDatuma_dtp.Name = "doDatuma_dtp";
            this.doDatuma_dtp.Size = new System.Drawing.Size(152, 23);
            this.doDatuma_dtp.TabIndex = 15;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(1186, 14);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.Size = new System.Drawing.Size(412, 272);
            this.dataGridView2.TabIndex = 22;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 687);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1611, 22);
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status_lbl
            // 
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(118, 17);
            this.status_lbl.Text = "toolStripStatusLabel1";
            // 
            // fm_FiskalniRacuni_list
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1611, 709);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fm_FiskalniRacuni_list";
            this.Text = "fm_FiskalniRacuni_list";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fm_FiskalniRacuni_list_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button uvuciFiskalne_btn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
        private System.Windows.Forms.CheckedListBox tipTransakcije_clb;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pib_txt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox senderFirma_cmb;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl;
    }
}