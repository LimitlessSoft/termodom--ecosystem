
namespace TDOffice_v2
{
    partial class _1347_fm_PartnerPocetnoKrajnje_Index
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.karticaPartneraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.status_lbl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.doPPID_nud = new System.Windows.Forms.NumericUpDown();
            this.odPPID_nud = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb2021 = new System.Windows.Forms.CheckBox();
            this.v2022 = new System.Windows.Forms.CheckBox();
            this.v2021 = new System.Windows.Forms.CheckBox();
            this.v2020 = new System.Windows.Forms.CheckBox();
            this.v2018 = new System.Windows.Forms.CheckBox();
            this.v2019 = new System.Windows.Forms.CheckBox();
            this.v2017 = new System.Windows.Forms.CheckBox();
            this.cb2017 = new System.Windows.Forms.CheckBox();
            this.cb2018 = new System.Windows.Forms.CheckBox();
            this.cb2019 = new System.Windows.Forms.CheckBox();
            this.cb2020 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.vPrag_nud = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.hPrag_nud = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.cb2022 = new System.Windows.Forms.CheckBox();
            this.v2023 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.doPPID_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.odPPID_nud)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vPrag_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hPrag_nud)).BeginInit();
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
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 142);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1154, 386);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Sorted += new System.EventHandler(this.dataGridView1_Sorted);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.karticaPartneraToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(158, 26);
            // 
            // karticaPartneraToolStripMenuItem
            // 
            this.karticaPartneraToolStripMenuItem.Name = "karticaPartneraToolStripMenuItem";
            this.karticaPartneraToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.karticaPartneraToolStripMenuItem.Text = "Kartica Partnera";
            this.karticaPartneraToolStripMenuItem.Click += new System.EventHandler(this.karticaPartneraToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(271, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Ucitaj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // status_lbl
            // 
            this.status_lbl.AutoSize = true;
            this.status_lbl.Location = new System.Drawing.Point(118, 16);
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(35, 13);
            this.status_lbl.TabIndex = 2;
            this.status_lbl.Text = "status";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.doPPID_nud);
            this.groupBox1.Controls.Add(this.odPPID_nud);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.status_lbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 95);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(143, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Od PPID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Od PPID:";
            // 
            // doPPID_nud
            // 
            this.doPPID_nud.Location = new System.Drawing.Point(201, 38);
            this.doPPID_nud.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.doPPID_nud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.doPPID_nud.Name = "doPPID_nud";
            this.doPPID_nud.Size = new System.Drawing.Size(76, 20);
            this.doPPID_nud.TabIndex = 2;
            this.doPPID_nud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // odPPID_nud
            // 
            this.odPPID_nud.Location = new System.Drawing.Point(64, 38);
            this.odPPID_nud.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.odPPID_nud.Name = "odPPID_nud";
            this.odPPID_nud.Size = new System.Drawing.Size(73, 20);
            this.odPPID_nud.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.v2023);
            this.groupBox2.Controls.Add(this.cb2022);
            this.groupBox2.Controls.Add(this.cb2021);
            this.groupBox2.Controls.Add(this.v2022);
            this.groupBox2.Controls.Add(this.v2021);
            this.groupBox2.Controls.Add(this.v2020);
            this.groupBox2.Controls.Add(this.v2018);
            this.groupBox2.Controls.Add(this.v2019);
            this.groupBox2.Controls.Add(this.v2017);
            this.groupBox2.Controls.Add(this.cb2017);
            this.groupBox2.Controls.Add(this.cb2018);
            this.groupBox2.Controls.Add(this.cb2019);
            this.groupBox2.Controls.Add(this.cb2020);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.vPrag_nud);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.hPrag_nud);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Location = new System.Drawing.Point(301, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(865, 124);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filteri";
            this.groupBox2.Visible = false;
            // 
            // cb2021
            // 
            this.cb2021.AutoSize = true;
            this.cb2021.Checked = true;
            this.cb2021.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb2021.Location = new System.Drawing.Point(605, 44);
            this.cb2021.Name = "cb2021";
            this.cb2021.Size = new System.Drawing.Size(83, 17);
            this.cb2021.TabIndex = 22;
            this.cb2021.Text = "2021 - 2022";
            this.cb2021.UseVisualStyleBackColor = true;
            // 
            // v2022
            // 
            this.v2022.AutoSize = true;
            this.v2022.Checked = true;
            this.v2022.CheckState = System.Windows.Forms.CheckState.Checked;
            this.v2022.Location = new System.Drawing.Point(529, 104);
            this.v2022.Name = "v2022";
            this.v2022.Size = new System.Drawing.Size(50, 17);
            this.v2022.TabIndex = 21;
            this.v2022.Text = "2022";
            this.v2022.UseVisualStyleBackColor = true;
            this.v2022.Visible = false;
            // 
            // v2021
            // 
            this.v2021.AutoSize = true;
            this.v2021.Checked = true;
            this.v2021.CheckState = System.Windows.Forms.CheckState.Checked;
            this.v2021.Location = new System.Drawing.Point(473, 104);
            this.v2021.Name = "v2021";
            this.v2021.Size = new System.Drawing.Size(50, 17);
            this.v2021.TabIndex = 20;
            this.v2021.Text = "2021";
            this.v2021.UseVisualStyleBackColor = true;
            this.v2021.Visible = false;
            // 
            // v2020
            // 
            this.v2020.AutoSize = true;
            this.v2020.Checked = true;
            this.v2020.CheckState = System.Windows.Forms.CheckState.Checked;
            this.v2020.Location = new System.Drawing.Point(417, 104);
            this.v2020.Name = "v2020";
            this.v2020.Size = new System.Drawing.Size(50, 17);
            this.v2020.TabIndex = 19;
            this.v2020.Text = "2020";
            this.v2020.UseVisualStyleBackColor = true;
            this.v2020.Visible = false;
            // 
            // v2018
            // 
            this.v2018.AutoSize = true;
            this.v2018.Checked = true;
            this.v2018.CheckState = System.Windows.Forms.CheckState.Checked;
            this.v2018.Location = new System.Drawing.Point(305, 104);
            this.v2018.Name = "v2018";
            this.v2018.Size = new System.Drawing.Size(50, 17);
            this.v2018.TabIndex = 18;
            this.v2018.Text = "2018";
            this.v2018.UseVisualStyleBackColor = true;
            this.v2018.Visible = false;
            // 
            // v2019
            // 
            this.v2019.AutoSize = true;
            this.v2019.Checked = true;
            this.v2019.CheckState = System.Windows.Forms.CheckState.Checked;
            this.v2019.Location = new System.Drawing.Point(361, 104);
            this.v2019.Name = "v2019";
            this.v2019.Size = new System.Drawing.Size(50, 17);
            this.v2019.TabIndex = 17;
            this.v2019.Text = "2019";
            this.v2019.UseVisualStyleBackColor = true;
            this.v2019.Visible = false;
            // 
            // v2017
            // 
            this.v2017.AutoSize = true;
            this.v2017.Checked = true;
            this.v2017.CheckState = System.Windows.Forms.CheckState.Checked;
            this.v2017.Location = new System.Drawing.Point(249, 104);
            this.v2017.Name = "v2017";
            this.v2017.Size = new System.Drawing.Size(50, 17);
            this.v2017.TabIndex = 16;
            this.v2017.Text = "2017";
            this.v2017.UseVisualStyleBackColor = true;
            this.v2017.Visible = false;
            // 
            // cb2017
            // 
            this.cb2017.AutoSize = true;
            this.cb2017.Checked = true;
            this.cb2017.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb2017.Location = new System.Drawing.Point(249, 45);
            this.cb2017.Name = "cb2017";
            this.cb2017.Size = new System.Drawing.Size(83, 17);
            this.cb2017.TabIndex = 12;
            this.cb2017.Text = "2017 - 2018";
            this.cb2017.UseVisualStyleBackColor = true;
            this.cb2017.Visible = false;
            // 
            // cb2018
            // 
            this.cb2018.AutoSize = true;
            this.cb2018.Checked = true;
            this.cb2018.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb2018.Location = new System.Drawing.Point(338, 45);
            this.cb2018.Name = "cb2018";
            this.cb2018.Size = new System.Drawing.Size(83, 17);
            this.cb2018.TabIndex = 11;
            this.cb2018.Text = "2018 - 2019";
            this.cb2018.UseVisualStyleBackColor = true;
            this.cb2018.Visible = false;
            // 
            // cb2019
            // 
            this.cb2019.AutoSize = true;
            this.cb2019.Checked = true;
            this.cb2019.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb2019.Location = new System.Drawing.Point(427, 45);
            this.cb2019.Name = "cb2019";
            this.cb2019.Size = new System.Drawing.Size(83, 17);
            this.cb2019.TabIndex = 10;
            this.cb2019.Text = "2019 - 2020";
            this.cb2019.UseVisualStyleBackColor = true;
            this.cb2019.Visible = false;
            // 
            // cb2020
            // 
            this.cb2020.AutoSize = true;
            this.cb2020.Checked = true;
            this.cb2020.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb2020.Location = new System.Drawing.Point(516, 45);
            this.cb2020.Name = "cb2020";
            this.cb2020.Size = new System.Drawing.Size(83, 17);
            this.cb2020.TabIndex = 9;
            this.cb2020.Text = "2020 - 2021";
            this.cb2020.UseVisualStyleBackColor = true;
            this.cb2020.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(247, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Vertikalni Prag Tolerancije:";
            this.label4.Visible = false;
            // 
            // vPrag_nud
            // 
            this.vPrag_nud.Location = new System.Drawing.Point(398, 78);
            this.vPrag_nud.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.vPrag_nud.Name = "vPrag_nud";
            this.vPrag_nud.Size = new System.Drawing.Size(120, 20);
            this.vPrag_nud.TabIndex = 15;
            this.vPrag_nud.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(247, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Horizontalni Prag Tolerancije:";
            this.label3.Visible = false;
            // 
            // hPrag_nud
            // 
            this.hPrag_nud.Location = new System.Drawing.Point(398, 19);
            this.hPrag_nud.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.hPrag_nud.Name = "hPrag_nud";
            this.hPrag_nud.Size = new System.Drawing.Size(120, 20);
            this.hPrag_nud.TabIndex = 10;
            this.hPrag_nud.Visible = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(719, 93);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(140, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Primeni Filter";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Prikazi Sve",
            "Prikazi Samo Neispravne"});
            this.comboBox1.Location = new System.Drawing.Point(6, 18);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(213, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(18, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(271, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Uvuci nove podatke";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // cb2022
            // 
            this.cb2022.AutoSize = true;
            this.cb2022.Checked = true;
            this.cb2022.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb2022.Location = new System.Drawing.Point(694, 44);
            this.cb2022.Name = "cb2022";
            this.cb2022.Size = new System.Drawing.Size(83, 17);
            this.cb2022.TabIndex = 23;
            this.cb2022.Text = "2022 - 2023";
            this.cb2022.UseVisualStyleBackColor = true;
            // 
            // v2023
            // 
            this.v2023.AutoSize = true;
            this.v2023.Checked = true;
            this.v2023.CheckState = System.Windows.Forms.CheckState.Checked;
            this.v2023.Location = new System.Drawing.Point(585, 104);
            this.v2023.Name = "v2023";
            this.v2023.Size = new System.Drawing.Size(50, 17);
            this.v2023.TabIndex = 24;
            this.v2023.Text = "2023";
            this.v2023.UseVisualStyleBackColor = true;
            this.v2023.Visible = false;
            // 
            // _1347_fm_PartnerPocetnoKrajnje_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 540);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "_1347_fm_PartnerPocetnoKrajnje_Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_1347_fm_PartnerPocetnoKrajnje_Index";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._1347_fm_PartnerPocetnoKrajnje_Index_FormClosing);
            this.Load += new System.EventHandler(this._1347_fm_PartnerPocetnoKrajnje_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.doPPID_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.odPPID_nud)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vPrag_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hPrag_nud)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label status_lbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown vPrag_nud;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown hPrag_nud;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox cb2017;
        private System.Windows.Forms.CheckBox cb2018;
        private System.Windows.Forms.CheckBox cb2019;
        private System.Windows.Forms.CheckBox cb2020;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown doPPID_nud;
        private System.Windows.Forms.NumericUpDown odPPID_nud;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox v2017;
        private System.Windows.Forms.CheckBox v2021;
        private System.Windows.Forms.CheckBox v2020;
        private System.Windows.Forms.CheckBox v2018;
        private System.Windows.Forms.CheckBox v2019;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem karticaPartneraToolStripMenuItem;
        private System.Windows.Forms.CheckBox cb2021;
        private System.Windows.Forms.CheckBox v2022;
        private System.Windows.Forms.CheckBox cb2022;
        private System.Windows.Forms.CheckBox v2023;
    }
}