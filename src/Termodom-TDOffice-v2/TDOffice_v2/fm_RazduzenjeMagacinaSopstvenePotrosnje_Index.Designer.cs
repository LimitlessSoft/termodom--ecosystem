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
            this.izvor_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ucitaj_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.izvornaBaza_cmb = new System.Windows.Forms.ComboBox();
            this.ufu_gb = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.preostalaVrednostRobe_txt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ufu_gb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // izvor_txt
            // 
            this.izvor_txt.Location = new System.Drawing.Point(196, 46);
            this.izvor_txt.Name = "izvor_txt";
            this.izvor_txt.Size = new System.Drawing.Size(192, 23);
            this.izvor_txt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Broj Izvorne Interne Otpremnice:";
            // 
            // ucitaj_btn
            // 
            this.ucitaj_btn.Location = new System.Drawing.Point(394, 46);
            this.ucitaj_btn.Name = "ucitaj_btn";
            this.ucitaj_btn.Size = new System.Drawing.Size(75, 23);
            this.ucitaj_btn.TabIndex = 2;
            this.ucitaj_btn.Text = "Ucitaj";
            this.ucitaj_btn.UseVisualStyleBackColor = true;
            this.ucitaj_btn.Click += new System.EventHandler(this.ucitaj_btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Izvorna baza:";
            // 
            // izvornaBaza_cmb
            // 
            this.izvornaBaza_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.izvornaBaza_cmb.FormattingEnabled = true;
            this.izvornaBaza_cmb.Location = new System.Drawing.Point(95, 12);
            this.izvornaBaza_cmb.Name = "izvornaBaza_cmb";
            this.izvornaBaza_cmb.Size = new System.Drawing.Size(265, 23);
            this.izvornaBaza_cmb.TabIndex = 3;
            // 
            // ufu_gb
            // 
            this.ufu_gb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ufu_gb.Controls.Add(this.dataGridView1);
            this.ufu_gb.Controls.Add(this.label4);
            this.ufu_gb.Controls.Add(this.preostalaVrednostRobe_txt);
            this.ufu_gb.Location = new System.Drawing.Point(12, 75);
            this.ufu_gb.Name = "ufu_gb";
            this.ufu_gb.Size = new System.Drawing.Size(776, 307);
            this.ufu_gb.TabIndex = 5;
            this.ufu_gb.TabStop = false;
            this.ufu_gb.Text = "Formiranje ulazne fakture za usluge";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 51);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(764, 250);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(291, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Preostala vrednost robe za koju treba napraviti usluge:";
            // 
            // preostalaVrednostRobe_txt
            // 
            this.preostalaVrednostRobe_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.preostalaVrednostRobe_txt.Location = new System.Drawing.Point(308, 22);
            this.preostalaVrednostRobe_txt.Name = "preostalaVrednostRobe_txt";
            this.preostalaVrednostRobe_txt.ReadOnly = true;
            this.preostalaVrednostRobe_txt.Size = new System.Drawing.Size(192, 23);
            this.preostalaVrednostRobe_txt.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(550, 388);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(232, 50);
            this.button1.TabIndex = 6;
            this.button1.Text = "Razduzi Magacin";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fm_RazduzenjeMagacinaSopstvenePotrosnje_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ufu_gb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.izvornaBaza_cmb);
            this.Controls.Add(this.ucitaj_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.izvor_txt);
            this.Name = "fm_RazduzenjeMagacinaSopstvenePotrosnje_Index";
            this.Text = "fm_RazduzenjeMagacinaSopstvenePotrosnje_Index";
            this.Load += new System.EventHandler(this.fm_RazduzenjeMagacinaSopstvenePotrosnje_Index_Load);
            this.ufu_gb.ResumeLayout(false);
            this.ufu_gb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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