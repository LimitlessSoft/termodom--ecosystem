namespace TDOffice_v2
{
    partial class fm_BiznisPlan_Index
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.pregled_tpg = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.planiraniPrometSum_txt = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.trenutniPrometSum_txt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.troskoviSumVrednost_txt = new System.Windows.Forms.TextBox();
            this.troskovi_tpg = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.troskovi_dgv = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.troskoviRacuni_txt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.troskoviSopstvenePotrebe_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ostvareniTroskovi_txt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.planiraniTroskovi_txt = new System.Windows.Forms.TextBox();
            this.promet_tpg = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.prometPoGodinama_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.prometCilj_txt = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.postaviNovuVrednostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label10 = new System.Windows.Forms.Label();
            this.ukupanPromet_txt = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.prodaja_tpg = new System.Windows.Forms.TabPage();
            this.prodajaSplitContainer = new System.Windows.Forms.SplitContainer();
            this.label9 = new System.Windows.Forms.Label();
            this.prodajaTargetKupci_dgv = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.postaviNovuPlaniranuVrednostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.pregled_tpg.SuspendLayout();
            this.troskovi_tpg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.troskovi_dgv)).BeginInit();
            this.promet_tpg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.prometPoGodinama_chart)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.prodaja_tpg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.prodajaSplitContainer)).BeginInit();
            this.prodajaSplitContainer.Panel1.SuspendLayout();
            this.prodajaSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.prodajaTargetKupci_dgv)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(64, 12);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(249, 21);
            this.magacin_cmb.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Magacin";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.pregled_tpg);
            this.tabControl1.Controls.Add(this.troskovi_tpg);
            this.tabControl1.Controls.Add(this.promet_tpg);
            this.tabControl1.Controls.Add(this.prodaja_tpg);
            this.tabControl1.Location = new System.Drawing.Point(12, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(740, 476);
            this.tabControl1.TabIndex = 2;
            // 
            // pregled_tpg
            // 
            this.pregled_tpg.Controls.Add(this.label13);
            this.pregled_tpg.Controls.Add(this.planiraniPrometSum_txt);
            this.pregled_tpg.Controls.Add(this.label12);
            this.pregled_tpg.Controls.Add(this.trenutniPrometSum_txt);
            this.pregled_tpg.Controls.Add(this.label6);
            this.pregled_tpg.Controls.Add(this.troskoviSumVrednost_txt);
            this.pregled_tpg.Location = new System.Drawing.Point(4, 22);
            this.pregled_tpg.Name = "pregled_tpg";
            this.pregled_tpg.Padding = new System.Windows.Forms.Padding(3);
            this.pregled_tpg.Size = new System.Drawing.Size(732, 450);
            this.pregled_tpg.TabIndex = 0;
            this.pregled_tpg.Text = "Pregled";
            this.pregled_tpg.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 35);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "Planirani Promet:";
            // 
            // planiraniPrometSum_txt
            // 
            this.planiraniPrometSum_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.planiraniPrometSum_txt.Location = new System.Drawing.Point(105, 32);
            this.planiraniPrometSum_txt.Name = "planiraniPrometSum_txt";
            this.planiraniPrometSum_txt.ReadOnly = true;
            this.planiraniPrometSum_txt.Size = new System.Drawing.Size(156, 20);
            this.planiraniPrometSum_txt.TabIndex = 7;
            this.planiraniPrometSum_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Trenutni Promet:";
            // 
            // trenutniPrometSum_txt
            // 
            this.trenutniPrometSum_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.trenutniPrometSum_txt.Location = new System.Drawing.Point(105, 6);
            this.trenutniPrometSum_txt.Name = "trenutniPrometSum_txt";
            this.trenutniPrometSum_txt.ReadOnly = true;
            this.trenutniPrometSum_txt.Size = new System.Drawing.Size(156, 20);
            this.trenutniPrometSum_txt.TabIndex = 5;
            this.trenutniPrometSum_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Troskovi Sum:";
            // 
            // troskoviSumVrednost_txt
            // 
            this.troskoviSumVrednost_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.troskoviSumVrednost_txt.Location = new System.Drawing.Point(105, 58);
            this.troskoviSumVrednost_txt.Name = "troskoviSumVrednost_txt";
            this.troskoviSumVrednost_txt.ReadOnly = true;
            this.troskoviSumVrednost_txt.Size = new System.Drawing.Size(176, 20);
            this.troskoviSumVrednost_txt.TabIndex = 2;
            this.troskoviSumVrednost_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // troskovi_tpg
            // 
            this.troskovi_tpg.Controls.Add(this.button2);
            this.troskovi_tpg.Controls.Add(this.troskovi_dgv);
            this.troskovi_tpg.Controls.Add(this.label5);
            this.troskovi_tpg.Controls.Add(this.troskoviRacuni_txt);
            this.troskovi_tpg.Controls.Add(this.label4);
            this.troskovi_tpg.Controls.Add(this.troskoviSopstvenePotrebe_txt);
            this.troskovi_tpg.Controls.Add(this.label3);
            this.troskovi_tpg.Controls.Add(this.ostvareniTroskovi_txt);
            this.troskovi_tpg.Controls.Add(this.label2);
            this.troskovi_tpg.Controls.Add(this.planiraniTroskovi_txt);
            this.troskovi_tpg.Location = new System.Drawing.Point(4, 22);
            this.troskovi_tpg.Name = "troskovi_tpg";
            this.troskovi_tpg.Padding = new System.Windows.Forms.Padding(3);
            this.troskovi_tpg.Size = new System.Drawing.Size(732, 450);
            this.troskovi_tpg.TabIndex = 1;
            this.troskovi_tpg.Text = "Troskovi";
            this.troskovi_tpg.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(554, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(172, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Upravljaj Grupama Troskova";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // troskovi_dgv
            // 
            this.troskovi_dgv.AllowUserToAddRows = false;
            this.troskovi_dgv.AllowUserToDeleteRows = false;
            this.troskovi_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.troskovi_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.troskovi_dgv.ContextMenuStrip = this.contextMenuStrip2;
            this.troskovi_dgv.Location = new System.Drawing.Point(359, 32);
            this.troskovi_dgv.Name = "troskovi_dgv";
            this.troskovi_dgv.ReadOnly = true;
            this.troskovi_dgv.Size = new System.Drawing.Size(367, 412);
            this.troskovi_dgv.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Troskovi (racuni):";
            // 
            // troskoviRacuni_txt
            // 
            this.troskoviRacuni_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.troskoviRacuni_txt.Location = new System.Drawing.Point(105, 84);
            this.troskoviRacuni_txt.Name = "troskoviRacuni_txt";
            this.troskoviRacuni_txt.ReadOnly = true;
            this.troskoviRacuni_txt.Size = new System.Drawing.Size(163, 20);
            this.troskoviRacuni_txt.TabIndex = 6;
            this.troskoviRacuni_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(174, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Razduzeno Za Sopstvenu Potrebu:";
            // 
            // troskoviSopstvenePotrebe_txt
            // 
            this.troskoviSopstvenePotrebe_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.troskoviSopstvenePotrebe_txt.Location = new System.Drawing.Point(190, 58);
            this.troskoviSopstvenePotrebe_txt.Name = "troskoviSopstvenePotrebe_txt";
            this.troskoviSopstvenePotrebe_txt.ReadOnly = true;
            this.troskoviSopstvenePotrebe_txt.Size = new System.Drawing.Size(163, 20);
            this.troskoviSopstvenePotrebe_txt.TabIndex = 4;
            this.troskoviSopstvenePotrebe_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Ostvareni Troskovi:";
            // 
            // ostvareniTroskovi_txt
            // 
            this.ostvareniTroskovi_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ostvareniTroskovi_txt.Location = new System.Drawing.Point(115, 32);
            this.ostvareniTroskovi_txt.Name = "ostvareniTroskovi_txt";
            this.ostvareniTroskovi_txt.ReadOnly = true;
            this.ostvareniTroskovi_txt.Size = new System.Drawing.Size(163, 20);
            this.ostvareniTroskovi_txt.TabIndex = 2;
            this.ostvareniTroskovi_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Planirani Troskovi:";
            // 
            // planiraniTroskovi_txt
            // 
            this.planiraniTroskovi_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.planiraniTroskovi_txt.Location = new System.Drawing.Point(110, 6);
            this.planiraniTroskovi_txt.Name = "planiraniTroskovi_txt";
            this.planiraniTroskovi_txt.ReadOnly = true;
            this.planiraniTroskovi_txt.Size = new System.Drawing.Size(158, 20);
            this.planiraniTroskovi_txt.TabIndex = 0;
            this.planiraniTroskovi_txt.Tag = "planiraniTroskovi";
            this.planiraniTroskovi_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // promet_tpg
            // 
            this.promet_tpg.Controls.Add(this.label7);
            this.promet_tpg.Controls.Add(this.prometPoGodinama_chart);
            this.promet_tpg.Controls.Add(this.prometCilj_txt);
            this.promet_tpg.Controls.Add(this.label10);
            this.promet_tpg.Controls.Add(this.ukupanPromet_txt);
            this.promet_tpg.Controls.Add(this.label11);
            this.promet_tpg.Location = new System.Drawing.Point(4, 22);
            this.promet_tpg.Name = "promet_tpg";
            this.promet_tpg.Padding = new System.Windows.Forms.Padding(3);
            this.promet_tpg.Size = new System.Drawing.Size(732, 450);
            this.promet_tpg.TabIndex = 2;
            this.promet_tpg.Text = "Promet";
            this.promet_tpg.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Promet Po Godinama";
            // 
            // prometPoGodinama_chart
            // 
            this.prometPoGodinama_chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea5.Name = "ChartArea1";
            this.prometPoGodinama_chart.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.prometPoGodinama_chart.Legends.Add(legend5);
            this.prometPoGodinama_chart.Location = new System.Drawing.Point(6, 85);
            this.prometPoGodinama_chart.Name = "prometPoGodinama_chart";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.prometPoGodinama_chart.Series.Add(series5);
            this.prometPoGodinama_chart.Size = new System.Drawing.Size(720, 362);
            this.prometPoGodinama_chart.TabIndex = 8;
            this.prometPoGodinama_chart.Text = "chart1";
            // 
            // prometCilj_txt
            // 
            this.prometCilj_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.prometCilj_txt.ContextMenuStrip = this.contextMenuStrip1;
            this.prometCilj_txt.Location = new System.Drawing.Point(32, 6);
            this.prometCilj_txt.Name = "prometCilj_txt";
            this.prometCilj_txt.ReadOnly = true;
            this.prometCilj_txt.Size = new System.Drawing.Size(159, 20);
            this.prometCilj_txt.TabIndex = 6;
            this.prometCilj_txt.Tag = "planiraniPromet";
            this.prometCilj_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.postaviNovuVrednostToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(195, 26);
            // 
            // postaviNovuVrednostToolStripMenuItem
            // 
            this.postaviNovuVrednostToolStripMenuItem.Name = "postaviNovuVrednostToolStripMenuItem";
            this.postaviNovuVrednostToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.postaviNovuVrednostToolStripMenuItem.Text = "Postavi Novu Vrednost";
            this.postaviNovuVrednostToolStripMenuItem.Click += new System.EventHandler(this.postaviNovuVrednostToolStripMenuItem_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Cilj";
            // 
            // ukupanPromet_txt
            // 
            this.ukupanPromet_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ukupanPromet_txt.Location = new System.Drawing.Point(130, 32);
            this.ukupanPromet_txt.Name = "ukupanPromet_txt";
            this.ukupanPromet_txt.ReadOnly = true;
            this.ukupanPromet_txt.Size = new System.Drawing.Size(184, 20);
            this.ukupanPromet_txt.TabIndex = 2;
            this.ukupanPromet_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Promet trenutne godine";
            // 
            // prodaja_tpg
            // 
            this.prodaja_tpg.Controls.Add(this.prodajaSplitContainer);
            this.prodaja_tpg.Location = new System.Drawing.Point(4, 22);
            this.prodaja_tpg.Name = "prodaja_tpg";
            this.prodaja_tpg.Padding = new System.Windows.Forms.Padding(3);
            this.prodaja_tpg.Size = new System.Drawing.Size(732, 450);
            this.prodaja_tpg.TabIndex = 3;
            this.prodaja_tpg.Text = "Prodaja";
            this.prodaja_tpg.UseVisualStyleBackColor = true;
            // 
            // prodajaSplitContainer
            // 
            this.prodajaSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prodajaSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prodajaSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.prodajaSplitContainer.Name = "prodajaSplitContainer";
            // 
            // prodajaSplitContainer.Panel1
            // 
            this.prodajaSplitContainer.Panel1.Controls.Add(this.label9);
            this.prodajaSplitContainer.Panel1.Controls.Add(this.prodajaTargetKupci_dgv);
            this.prodajaSplitContainer.Size = new System.Drawing.Size(726, 444);
            this.prodajaSplitContainer.SplitterDistance = 242;
            this.prodajaSplitContainer.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Target Kupci";
            // 
            // prodajaTargetKupci_dgv
            // 
            this.prodajaTargetKupci_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prodajaTargetKupci_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.prodajaTargetKupci_dgv.Location = new System.Drawing.Point(3, 16);
            this.prodajaTargetKupci_dgv.Name = "prodajaTargetKupci_dgv";
            this.prodajaTargetKupci_dgv.Size = new System.Drawing.Size(234, 423);
            this.prodajaTargetKupci_dgv.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(319, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Ucitaj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.postaviNovuPlaniranuVrednostToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(248, 48);
            // 
            // postaviNovuPlaniranuVrednostToolStripMenuItem
            // 
            this.postaviNovuPlaniranuVrednostToolStripMenuItem.Name = "postaviNovuPlaniranuVrednostToolStripMenuItem";
            this.postaviNovuPlaniranuVrednostToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.postaviNovuPlaniranuVrednostToolStripMenuItem.Text = "Postavi Novu Planiranu Vrednost";
            this.postaviNovuPlaniranuVrednostToolStripMenuItem.Click += new System.EventHandler(this.postaviNovuPlaniranuVrednostToolStripMenuItem_Click);
            // 
            // fm_BiznisPlan_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 527);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.magacin_cmb);
            this.Name = "fm_BiznisPlan_Index";
            this.Text = "Biznis Plan";
            this.Load += new System.EventHandler(this.fm_BiznisPlan_Index_Load);
            this.tabControl1.ResumeLayout(false);
            this.pregled_tpg.ResumeLayout(false);
            this.pregled_tpg.PerformLayout();
            this.troskovi_tpg.ResumeLayout(false);
            this.troskovi_tpg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.troskovi_dgv)).EndInit();
            this.promet_tpg.ResumeLayout(false);
            this.promet_tpg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.prometPoGodinama_chart)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.prodaja_tpg.ResumeLayout(false);
            this.prodajaSplitContainer.Panel1.ResumeLayout(false);
            this.prodajaSplitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.prodajaSplitContainer)).EndInit();
            this.prodajaSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.prodajaTargetKupci_dgv)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage pregled_tpg;
        private System.Windows.Forms.TabPage troskovi_tpg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox planiraniTroskovi_txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ostvareniTroskovi_txt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox troskoviSumVrednost_txt;
        private System.Windows.Forms.TabPage promet_tpg;
        private System.Windows.Forms.TextBox prometCilj_txt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox ukupanPromet_txt;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataVisualization.Charting.Chart prometPoGodinama_chart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox troskoviRacuni_txt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox troskoviSopstvenePotrebe_txt;
        private System.Windows.Forms.TabPage prodaja_tpg;
        private System.Windows.Forms.SplitContainer prodajaSplitContainer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView prodajaTargetKupci_dgv;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox planiraniPrometSum_txt;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox trenutniPrometSum_txt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem postaviNovuVrednostToolStripMenuItem;
        private System.Windows.Forms.DataGridView troskovi_dgv;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem postaviNovuPlaniranuVrednostToolStripMenuItem;
    }
}