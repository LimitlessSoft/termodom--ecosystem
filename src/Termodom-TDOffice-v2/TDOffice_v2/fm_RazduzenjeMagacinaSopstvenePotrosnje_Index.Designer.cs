namespace TDOffice_v2
{
    partial class fm_RazduzenjeMagacinaSopstvenePotrosnje_Index
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
            izvor_txt = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            ucitaj_btn = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            izvornaBaza_cmb = new System.Windows.Forms.ComboBox();
            ufu_gb = new System.Windows.Forms.GroupBox();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            label4 = new System.Windows.Forms.Label();
            preostalaVrednostRobe_txt = new System.Windows.Forms.TextBox();
            button1 = new System.Windows.Forms.Button();
            ufu_gb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // izvor_txt
            // 
            izvor_txt.Location = new System.Drawing.Point(196, 46);
            izvor_txt.Name = "izvor_txt";
            izvor_txt.Size = new System.Drawing.Size(192, 23);
            izvor_txt.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 49);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(171, 15);
            label1.TabIndex = 1;
            label1.Text = "Broj Izvorne Interne Kalkulacije:";
            // 
            // ucitaj_btn
            // 
            ucitaj_btn.Location = new System.Drawing.Point(394, 46);
            ucitaj_btn.Name = "ucitaj_btn";
            ucitaj_btn.Size = new System.Drawing.Size(75, 23);
            ucitaj_btn.TabIndex = 2;
            ucitaj_btn.Text = "Ucitaj";
            ucitaj_btn.UseVisualStyleBackColor = true;
            ucitaj_btn.Click += ucitaj_btn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 15);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(75, 15);
            label2.TabIndex = 4;
            label2.Text = "Izvorna baza:";
            // 
            // izvornaBaza_cmb
            // 
            izvornaBaza_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            izvornaBaza_cmb.FormattingEnabled = true;
            izvornaBaza_cmb.Location = new System.Drawing.Point(95, 12);
            izvornaBaza_cmb.Name = "izvornaBaza_cmb";
            izvornaBaza_cmb.Size = new System.Drawing.Size(265, 23);
            izvornaBaza_cmb.TabIndex = 3;
            // 
            // ufu_gb
            // 
            ufu_gb.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ufu_gb.Controls.Add(dataGridView1);
            ufu_gb.Controls.Add(label4);
            ufu_gb.Controls.Add(preostalaVrednostRobe_txt);
            ufu_gb.Location = new System.Drawing.Point(12, 75);
            ufu_gb.Name = "ufu_gb";
            ufu_gb.Size = new System.Drawing.Size(776, 307);
            ufu_gb.TabIndex = 5;
            ufu_gb.TabStop = false;
            ufu_gb.Text = "Formiranje ulazne fakture za usluge";
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new System.Drawing.Point(6, 51);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new System.Drawing.Size(764, 250);
            dataGridView1.TabIndex = 4;
            dataGridView1.MouseDoubleClick += dataGridView1_MouseDoubleClick;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(11, 25);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(291, 15);
            label4.TabIndex = 3;
            label4.Text = "Preostala vrednost robe za koju treba napraviti usluge:";
            // 
            // preostalaVrednostRobe_txt
            // 
            preostalaVrednostRobe_txt.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            preostalaVrednostRobe_txt.Location = new System.Drawing.Point(308, 22);
            preostalaVrednostRobe_txt.Name = "preostalaVrednostRobe_txt";
            preostalaVrednostRobe_txt.ReadOnly = true;
            preostalaVrednostRobe_txt.Size = new System.Drawing.Size(192, 23);
            preostalaVrednostRobe_txt.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(550, 388);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(232, 50);
            button1.TabIndex = 6;
            button1.Text = "Razduzi Magacin";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // fm_RazduzenjeMagacinaSopstvenePotrosnje_Index
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(button1);
            Controls.Add(ufu_gb);
            Controls.Add(label2);
            Controls.Add(izvornaBaza_cmb);
            Controls.Add(ucitaj_btn);
            Controls.Add(label1);
            Controls.Add(izvor_txt);
            Name = "fm_RazduzenjeMagacinaSopstvenePotrosnje_Index";
            Text = "fm_RazduzenjeMagacinaSopstvenePotrosnje_Index";
            Load += fm_RazduzenjeMagacinaSopstvenePotrosnje_Index_Load;
            ufu_gb.ResumeLayout(false);
            ufu_gb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox izvor_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ucitaj_btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox izvornaBaza_cmb;
        private System.Windows.Forms.GroupBox ufu_gb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox preostalaVrednostRobe_txt;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
    }
}