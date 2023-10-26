
namespace TDOffice_v2
{
    partial class _1389_fm_PregledDana_Index
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lokalniCilj_txt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.globalCilj_txt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ukupanPrometM_txt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ukupanPromet_txt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.odlozeno_txt = new System.Windows.Forms.TextBox();
            this.gotovina_txt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.virman_txt = new System.Windows.Forms.TextBox();
            this.kartica_txt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.ucitaj_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(12, 218);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(971, 349);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(12, 12);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(205, 21);
            this.magacin_cmb.TabIndex = 1;
            this.magacin_cmb.SelectedIndexChanged += new System.EventHandler(this.magacin_cmb_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lokalniCilj_txt);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.globalCilj_txt);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.ukupanPrometM_txt);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(288, 39);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 173);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Promet izabranog meseca";
            // 
            // lokalniCilj_txt
            // 
            this.lokalniCilj_txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lokalniCilj_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lokalniCilj_txt.Location = new System.Drawing.Point(91, 71);
            this.lokalniCilj_txt.Name = "lokalniCilj_txt";
            this.lokalniCilj_txt.ReadOnly = true;
            this.lokalniCilj_txt.Size = new System.Drawing.Size(148, 20);
            this.lokalniCilj_txt.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 15);
            this.label10.TabIndex = 7;
            this.label10.Text = "Lokalni cilj:";
            // 
            // globalCilj_txt
            // 
            this.globalCilj_txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.globalCilj_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.globalCilj_txt.Location = new System.Drawing.Point(91, 45);
            this.globalCilj_txt.Name = "globalCilj_txt";
            this.globalCilj_txt.ReadOnly = true;
            this.globalCilj_txt.Size = new System.Drawing.Size(148, 20);
            this.globalCilj_txt.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 15);
            this.label8.TabIndex = 5;
            this.label8.Text = "Globalni cilj:";
            // 
            // ukupanPrometM_txt
            // 
            this.ukupanPrometM_txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ukupanPrometM_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ukupanPrometM_txt.Location = new System.Drawing.Point(91, 19);
            this.ukupanPrometM_txt.Name = "ukupanPrometM_txt";
            this.ukupanPrometM_txt.ReadOnly = true;
            this.ukupanPrometM_txt.Size = new System.Drawing.Size(148, 20);
            this.ukupanPrometM_txt.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 15);
            this.label9.TabIndex = 3;
            this.label9.Text = "Ukupan promet";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ukupanPromet_txt);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.odlozeno_txt);
            this.groupBox1.Controls.Add(this.gotovina_txt);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.virman_txt);
            this.groupBox1.Controls.Add(this.kartica_txt);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 173);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Promet na dan";
            // 
            // ukupanPromet_txt
            // 
            this.ukupanPromet_txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ukupanPromet_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ukupanPromet_txt.Location = new System.Drawing.Point(91, 19);
            this.ukupanPromet_txt.Name = "ukupanPromet_txt";
            this.ukupanPromet_txt.ReadOnly = true;
            this.ukupanPromet_txt.Size = new System.Drawing.Size(173, 20);
            this.ukupanPromet_txt.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "Odlozeno:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ukupan promet";
            // 
            // odlozeno_txt
            // 
            this.odlozeno_txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.odlozeno_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.odlozeno_txt.Location = new System.Drawing.Point(91, 123);
            this.odlozeno_txt.Name = "odlozeno_txt";
            this.odlozeno_txt.ReadOnly = true;
            this.odlozeno_txt.Size = new System.Drawing.Size(173, 20);
            this.odlozeno_txt.TabIndex = 14;
            // 
            // gotovina_txt
            // 
            this.gotovina_txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gotovina_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gotovina_txt.Location = new System.Drawing.Point(91, 45);
            this.gotovina_txt.Name = "gotovina_txt";
            this.gotovina_txt.ReadOnly = true;
            this.gotovina_txt.Size = new System.Drawing.Size(173, 20);
            this.gotovina_txt.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Virman:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Gotovina:";
            // 
            // virman_txt
            // 
            this.virman_txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.virman_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.virman_txt.Location = new System.Drawing.Point(91, 71);
            this.virman_txt.Name = "virman_txt";
            this.virman_txt.ReadOnly = true;
            this.virman_txt.Size = new System.Drawing.Size(173, 20);
            this.virman_txt.TabIndex = 6;
            // 
            // kartica_txt
            // 
            this.kartica_txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kartica_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kartica_txt.Location = new System.Drawing.Point(91, 97);
            this.kartica_txt.Name = "kartica_txt";
            this.kartica_txt.ReadOnly = true;
            this.kartica_txt.Size = new System.Drawing.Size(173, 20);
            this.kartica_txt.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Kartica:";
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odDatuma_dtp.Location = new System.Drawing.Point(223, 12);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.odDatuma_dtp.TabIndex = 20;
            this.odDatuma_dtp.ValueChanged += new System.EventHandler(this.odDatuma_dtp_ValueChanged);
            // 
            // ucitaj_btn
            // 
            this.ucitaj_btn.Location = new System.Drawing.Point(373, 12);
            this.ucitaj_btn.Name = "ucitaj_btn";
            this.ucitaj_btn.Size = new System.Drawing.Size(75, 20);
            this.ucitaj_btn.TabIndex = 21;
            this.ucitaj_btn.Text = "Ucitaj";
            this.ucitaj_btn.UseVisualStyleBackColor = true;
            this.ucitaj_btn.Visible = false;
            this.ucitaj_btn.Click += new System.EventHandler(this.ucitaj_btn_Click);
            // 
            // _1389_fm_PregledDana_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 579);
            this.Controls.Add(this.ucitaj_btn);
            this.Controls.Add(this.odDatuma_dtp);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.magacin_cmb);
            this.Controls.Add(this.chart1);
            this.Name = "_1389_fm_PregledDana_Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_1389_fm_PregledDana_Index";
            this.Load += new System.EventHandler(this._1389_fm_PregledDana_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox lokalniCilj_txt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox globalCilj_txt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ukupanPrometM_txt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ukupanPromet_txt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox odlozeno_txt;
        private System.Windows.Forms.TextBox gotovina_txt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox virman_txt;
        private System.Windows.Forms.TextBox kartica_txt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Button ucitaj_btn;
    }
}