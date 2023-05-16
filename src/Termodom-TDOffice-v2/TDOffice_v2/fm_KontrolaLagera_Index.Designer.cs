
namespace TDOffice_v2
{
    partial class fm_KontrolaLagera_Index
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listaRobe_cb = new System.Windows.Forms.CheckBox();
            this.listaRobe_btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.prikazi_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.pretraga_txt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.kolone_cmb = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.filtriraj_btn = new System.Windows.Forms.Button();
            this.filterStanjeUslov_cmb = new System.Windows.Forms.ComboBox();
            this.filterStanjeOperacija_cmb = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.kolonaVrednsot_cmb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ukupnaVrednostLagera_txt = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slogova_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.listaRobe_cb);
            this.groupBox1.Controls.Add(this.listaRobe_btn);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.odDatuma_dtp);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.doDatuma_dtp);
            this.groupBox1.Controls.Add(this.prikazi_btn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.magacin_cmb);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1290, 70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // listaRobe_cb
            // 
            this.listaRobe_cb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listaRobe_cb.AutoSize = true;
            this.listaRobe_cb.Location = new System.Drawing.Point(955, 35);
            this.listaRobe_cb.Name = "listaRobe_cb";
            this.listaRobe_cb.Size = new System.Drawing.Size(18, 17);
            this.listaRobe_cb.TabIndex = 12;
            this.listaRobe_cb.UseVisualStyleBackColor = true;
            // 
            // listaRobe_btn
            // 
            this.listaRobe_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listaRobe_btn.Location = new System.Drawing.Point(976, 31);
            this.listaRobe_btn.Name = "listaRobe_btn";
            this.listaRobe_btn.Size = new System.Drawing.Size(157, 23);
            this.listaRobe_btn.TabIndex = 11;
            this.listaRobe_btn.Text = "Lista Robe";
            this.listaRobe_btn.UseVisualStyleBackColor = true;
            this.listaRobe_btn.Click += new System.EventHandler(this.listaRobe_btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(475, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Do datuma:";
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odDatuma_dtp.Location = new System.Drawing.Point(310, 33);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.odDatuma_dtp.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(307, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Od datuma:";
            // 
            // doDatuma_dtp
            // 
            this.doDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.doDatuma_dtp.Location = new System.Drawing.Point(478, 33);
            this.doDatuma_dtp.Name = "doDatuma_dtp";
            this.doDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.doDatuma_dtp.TabIndex = 8;
            // 
            // prikazi_btn
            // 
            this.prikazi_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prikazi_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prikazi_btn.Location = new System.Drawing.Point(1139, 11);
            this.prikazi_btn.Name = "prikazi_btn";
            this.prikazi_btn.Size = new System.Drawing.Size(145, 53);
            this.prikazi_btn.TabIndex = 2;
            this.prikazi_btn.Text = "Prikazi";
            this.prikazi_btn.UseVisualStyleBackColor = true;
            this.prikazi_btn.Click += new System.EventHandler(this.prikazi_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Magacin";
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.Enabled = false;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(67, 33);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(214, 21);
            this.magacin_cmb.TabIndex = 0;
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 213);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1290, 440);
            this.dataGridView1.TabIndex = 1;
            // 
            // pretraga_txt
            // 
            this.pretraga_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pretraga_txt.Location = new System.Drawing.Point(322, 184);
            this.pretraga_txt.Name = "pretraga_txt";
            this.pretraga_txt.Size = new System.Drawing.Size(387, 26);
            this.pretraga_txt.TabIndex = 2;
            this.pretraga_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pretraga_txt_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Pretraga";
            // 
            // kolone_cmb
            // 
            this.kolone_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kolone_cmb.FormattingEnabled = true;
            this.kolone_cmb.Location = new System.Drawing.Point(199, 186);
            this.kolone_cmb.Name = "kolone_cmb";
            this.kolone_cmb.Size = new System.Drawing.Size(117, 21);
            this.kolone_cmb.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.filtriraj_btn);
            this.groupBox2.Controls.Add(this.filterStanjeUslov_cmb);
            this.groupBox2.Controls.Add(this.filterStanjeOperacija_cmb);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(787, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(515, 92);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filteri";
            // 
            // filtriraj_btn
            // 
            this.filtriraj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filtriraj_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtriraj_btn.Location = new System.Drawing.Point(290, 19);
            this.filtriraj_btn.Name = "filtriraj_btn";
            this.filtriraj_btn.Size = new System.Drawing.Size(219, 67);
            this.filtriraj_btn.TabIndex = 11;
            this.filtriraj_btn.Text = "Filtriraj";
            this.filtriraj_btn.UseVisualStyleBackColor = true;
            this.filtriraj_btn.Click += new System.EventHandler(this.filtriraj_btn_Click);
            // 
            // filterStanjeUslov_cmb
            // 
            this.filterStanjeUslov_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterStanjeUslov_cmb.FormattingEnabled = true;
            this.filterStanjeUslov_cmb.Items.AddRange(new object[] {
            "< nista >",
            "Opt Zalihe",
            "Krit Zalihe",
            "Izaslo Mes",
            "Prodato Mes"});
            this.filterStanjeUslov_cmb.Location = new System.Drawing.Point(128, 19);
            this.filterStanjeUslov_cmb.Name = "filterStanjeUslov_cmb";
            this.filterStanjeUslov_cmb.Size = new System.Drawing.Size(156, 21);
            this.filterStanjeUslov_cmb.TabIndex = 6;
            this.filterStanjeUslov_cmb.SelectedIndexChanged += new System.EventHandler(this.filterStanjeUslov_cmb_SelectedIndexChanged);
            // 
            // filterStanjeOperacija_cmb
            // 
            this.filterStanjeOperacija_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterStanjeOperacija_cmb.FormattingEnabled = true;
            this.filterStanjeOperacija_cmb.Items.AddRange(new object[] {
            "< nista >",
            "=",
            ">",
            "<"});
            this.filterStanjeOperacija_cmb.Location = new System.Drawing.Point(57, 19);
            this.filterStanjeOperacija_cmb.Name = "filterStanjeOperacija_cmb";
            this.filterStanjeOperacija_cmb.Size = new System.Drawing.Size(65, 21);
            this.filterStanjeOperacija_cmb.TabIndex = 5;
            this.filterStanjeOperacija_cmb.SelectedIndexChanged += new System.EventHandler(this.filterStanjeOperacija_cmb_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Stanje:";
            // 
            // kolonaVrednsot_cmb
            // 
            this.kolonaVrednsot_cmb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kolonaVrednsot_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kolonaVrednsot_cmb.FormattingEnabled = true;
            this.kolonaVrednsot_cmb.Items.AddRange(new object[] {
            "Stanje * Prodajna Cena",
            "Stanje - Izaslo Mesecno",
            "Stanje - Opt Zalihe",
            "Opt Zalihe - Izaslo Mesecno",
            "Prodato Mesecno * Prodajna Cena",
            "Stanje - Prodato Mesecno",
            "Opt Zalihe - Prodato Mesecno",
            "VIsak Zaliha * Prodajna Cena"});
            this.kolonaVrednsot_cmb.Location = new System.Drawing.Point(1087, 186);
            this.kolonaVrednsot_cmb.Name = "kolonaVrednsot_cmb";
            this.kolonaVrednsot_cmb.Size = new System.Drawing.Size(215, 21);
            this.kolonaVrednsot_cmb.TabIndex = 8;
            this.kolonaVrednsot_cmb.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(988, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 15);
            this.label6.TabIndex = 7;
            this.label6.Text = "Kolona vrednost =";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 15);
            this.label7.TabIndex = 5;
            this.label7.Text = "Ukupna Vrednost Lagera:";
            // 
            // ukupnaVrednostLagera_txt
            // 
            this.ukupnaVrednostLagera_txt.BackColor = System.Drawing.SystemColors.Info;
            this.ukupnaVrednostLagera_txt.Location = new System.Drawing.Point(148, 125);
            this.ukupnaVrednostLagera_txt.Name = "ukupnaVrednostLagera_txt";
            this.ukupnaVrednostLagera_txt.ReadOnly = true;
            this.ukupnaVrednostLagera_txt.Size = new System.Drawing.Size(191, 20);
            this.ukupnaVrednostLagera_txt.TabIndex = 8;
            this.ukupnaVrednostLagera_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slogova_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 652);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1314, 26);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slogova_lbl
            // 
            this.slogova_lbl.Name = "slogova_lbl";
            this.slogova_lbl.Size = new System.Drawing.Size(84, 20);
            this.slogova_lbl.Text = "slogova_lbl";
            // 
            // fm_KontrolaLagera_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1314, 678);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ukupnaVrednostLagera_txt);
            this.Controls.Add(this.kolonaVrednsot_cmb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.kolone_cmb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pretraga_txt);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.HelpButton = true;
            this.MinimumSize = new System.Drawing.Size(1063, 657);
            this.Name = "fm_KontrolaLagera_Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kontrola Lagera";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fm_KontrolaLagera_Index_Load);
            this.Shown += new System.EventHandler(this.fm_KontrolaLagera_Index_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button prikazi_btn;
        private System.Windows.Forms.TextBox pretraga_txt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox kolone_cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button filtriraj_btn;
        private System.Windows.Forms.ComboBox kolonaVrednsot_cmb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox filterStanjeUslov_cmb;
        private System.Windows.Forms.ComboBox filterStanjeOperacija_cmb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox ukupnaVrednostLagera_txt;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slogova_lbl;
        private System.Windows.Forms.Button listaRobe_btn;
        private System.Windows.Forms.CheckBox listaRobe_cb;
    }
}