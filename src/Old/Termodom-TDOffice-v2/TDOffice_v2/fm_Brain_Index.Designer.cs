namespace TDOffice_v2
{
    partial class fm_Brain_Index
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
            this.txt_fbdbServerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_fbdbPassword = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.vudimConfigBaza_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ftpNovaSifra_txt = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ftpUrl_txt = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ftpUsername_txt = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_fbdbServerName
            // 
            this.txt_fbdbServerName.Enabled = false;
            this.txt_fbdbServerName.Location = new System.Drawing.Point(79, 19);
            this.txt_fbdbServerName.Name = "txt_fbdbServerName";
            this.txt_fbdbServerName.Size = new System.Drawing.Size(212, 20);
            this.txt_fbdbServerName.TabIndex = 0;
            this.txt_fbdbServerName.TextChanged += new System.EventHandler(this.txt_dbServerName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "DB Server:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(297, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 2;
            this.button1.Text = "Azuriraj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1051, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 24);
            this.button2.TabIndex = 3;
            this.button2.Text = "Osvezi Podatke";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_ClickAsync);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(297, 45);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 20);
            this.button3.TabIndex = 6;
            this.button3.Text = "Resetuj Sifru";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nova Sifra:";
            // 
            // txt_fbdbPassword
            // 
            this.txt_fbdbPassword.Location = new System.Drawing.Point(79, 45);
            this.txt_fbdbPassword.Name = "txt_fbdbPassword";
            this.txt_fbdbPassword.Size = new System.Drawing.Size(212, 20);
            this.txt_fbdbPassword.TabIndex = 4;
            this.txt_fbdbPassword.TextChanged += new System.EventHandler(this.txt_fbdbPassword_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_fbdbServerName);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txt_fbdbPassword);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 83);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Firebird";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(12, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(396, 363);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Komercijalno Poslovanje Baze";
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(6, 326);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(384, 31);
            this.button4.TabIndex = 7;
            this.button4.Text = "Sacuvaj Izmene Baza";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Enabled = false;
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(384, 301);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.vudimConfigBaza_txt);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Location = new System.Drawing.Point(12, 466);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(396, 83);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Ostale baze";
            // 
            // vudimConfigBaza_txt
            // 
            this.vudimConfigBaza_txt.Enabled = false;
            this.vudimConfigBaza_txt.Location = new System.Drawing.Point(17, 38);
            this.vudimConfigBaza_txt.Name = "vudimConfigBaza_txt";
            this.vudimConfigBaza_txt.Size = new System.Drawing.Size(292, 20);
            this.vudimConfigBaza_txt.TabIndex = 0;
            this.vudimConfigBaza_txt.TextChanged += new System.EventHandler(this.vudimConfigBaza_txt_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Vudim config baza:";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(315, 38);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 20);
            this.button6.TabIndex = 2;
            this.button6.Text = "Azuriraj";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button8);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.ftpUsername_txt);
            this.groupBox4.Controls.Add(this.button7);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.ftpUrl_txt);
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.ftpNovaSifra_txt);
            this.groupBox4.Location = new System.Drawing.Point(414, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(418, 106);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "FTP";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(292, 71);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(84, 20);
            this.button5.TabIndex = 9;
            this.button5.Text = "Resetuj Sifru";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Nova Sifra:";
            // 
            // ftpNovaSifra_txt
            // 
            this.ftpNovaSifra_txt.Location = new System.Drawing.Point(74, 71);
            this.ftpNovaSifra_txt.Name = "ftpNovaSifra_txt";
            this.ftpNovaSifra_txt.Size = new System.Drawing.Size(212, 20);
            this.ftpNovaSifra_txt.TabIndex = 7;
            this.ftpNovaSifra_txt.TextChanged += new System.EventHandler(this.novaSifra_ftp_TextChanged);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(292, 19);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(84, 20);
            this.button7.TabIndex = 12;
            this.button7.Text = "Azuriraj";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Visible = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "Url:";
            // 
            // ftpUrl_txt
            // 
            this.ftpUrl_txt.Location = new System.Drawing.Point(74, 19);
            this.ftpUrl_txt.Name = "ftpUrl_txt";
            this.ftpUrl_txt.Size = new System.Drawing.Size(212, 20);
            this.ftpUrl_txt.TabIndex = 10;
            this.ftpUrl_txt.TextChanged += new System.EventHandler(this.ftpUrl_txt_TextChanged);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(292, 45);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(84, 20);
            this.button8.TabIndex = 15;
            this.button8.Text = "Azuriraj";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Visible = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "Username:";
            // 
            // ftpUsername_txt
            // 
            this.ftpUsername_txt.Location = new System.Drawing.Point(74, 45);
            this.ftpUsername_txt.Name = "ftpUsername_txt";
            this.ftpUsername_txt.Size = new System.Drawing.Size(212, 20);
            this.ftpUsername_txt.TabIndex = 13;
            this.ftpUsername_txt.TextChanged += new System.EventHandler(this.ftpUsername_txt_TextChanged);
            // 
            // fm_Brain_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 590);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Name = "fm_Brain_Index";
            this.Text = "Brain_v3";
            this.Load += new System.EventHandler(this.fm_Brain_Index_LoadAsync);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_fbdbServerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_fbdbPassword;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox vudimConfigBaza_txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ftpNovaSifra_txt;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ftpUrl_txt;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ftpUsername_txt;
    }
}