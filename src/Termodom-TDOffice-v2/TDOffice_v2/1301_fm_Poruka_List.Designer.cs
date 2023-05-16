
namespace TDOffice_v2
{
    partial class _1301_fm_Poruka_List
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.nova_btn = new System.Windows.Forms.Button();
            this.tip_gb = new System.Windows.Forms.GroupBox();
            this.tipSve_rb = new System.Windows.Forms.RadioButton();
            this.poslate_rb = new System.Windows.Forms.RadioButton();
            this.primljene_rb = new System.Windows.Forms.RadioButton();
            this.period_gb = new System.Windows.Forms.GroupBox();
            this.do_lbl = new System.Windows.Forms.Label();
            this.do_dtp = new System.Windows.Forms.DateTimePicker();
            this.od_lbl = new System.Windows.Forms.Label();
            this.od_dtp = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbIzborAkcijePoruka = new System.Windows.Forms.ComboBox();
            this.gbIzborPosiljaoca = new System.Windows.Forms.GroupBox();
            this.cmb_Tip = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.izborKorespodenta_cmb = new System.Windows.Forms.ComboBox();
            this.status_gb = new System.Windows.Forms.GroupBox();
            this.statusSve_rb = new System.Windows.Forms.RadioButton();
            this.neprocitane_rb = new System.Windows.Forms.RadioButton();
            this.procitane_rb = new System.Windows.Forms.RadioButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.arhivirajToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            this.tip_gb.SuspendLayout();
            this.period_gb.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbIzborPosiljaoca.SuspendLayout();
            this.status_gb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btn_Refresh);
            this.panel1.Controls.Add(this.nova_btn);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1028, 35);
            this.panel1.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(845, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "HELP";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Refresh.Location = new System.Drawing.Point(941, 6);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_Refresh.TabIndex = 14;
            this.btn_Refresh.Text = "Refresh";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // nova_btn
            // 
            this.nova_btn.BackgroundImage = global::TDOffice_v2.Properties.Resources.new_icon;
            this.nova_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.nova_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nova_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.nova_btn.Location = new System.Drawing.Point(4, 3);
            this.nova_btn.Name = "nova_btn";
            this.nova_btn.Size = new System.Drawing.Size(27, 29);
            this.nova_btn.TabIndex = 13;
            this.nova_btn.UseVisualStyleBackColor = true;
            this.nova_btn.Click += new System.EventHandler(this.nova_btn_Click);
            // 
            // tip_gb
            // 
            this.tip_gb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tip_gb.Controls.Add(this.tipSve_rb);
            this.tip_gb.Controls.Add(this.poslate_rb);
            this.tip_gb.Controls.Add(this.primljene_rb);
            this.tip_gb.Location = new System.Drawing.Point(4, 3);
            this.tip_gb.Name = "tip_gb";
            this.tip_gb.Size = new System.Drawing.Size(106, 94);
            this.tip_gb.TabIndex = 15;
            this.tip_gb.TabStop = false;
            this.tip_gb.Text = "Tip";
            // 
            // tipSve_rb
            // 
            this.tipSve_rb.AutoSize = true;
            this.tipSve_rb.Location = new System.Drawing.Point(6, 65);
            this.tipSve_rb.Name = "tipSve_rb";
            this.tipSve_rb.Size = new System.Drawing.Size(44, 17);
            this.tipSve_rb.TabIndex = 2;
            this.tipSve_rb.Text = "Sve";
            this.tipSve_rb.UseVisualStyleBackColor = true;
            this.tipSve_rb.CheckedChanged += new System.EventHandler(this.tipPorukeRB_CheckedChanged);
            this.tipSve_rb.Click += new System.EventHandler(this.tipSve_rb_Click);
            // 
            // poslate_rb
            // 
            this.poslate_rb.AutoSize = true;
            this.poslate_rb.Location = new System.Drawing.Point(6, 42);
            this.poslate_rb.Name = "poslate_rb";
            this.poslate_rb.Size = new System.Drawing.Size(60, 17);
            this.poslate_rb.TabIndex = 1;
            this.poslate_rb.Text = "Poslate";
            this.poslate_rb.UseVisualStyleBackColor = true;
            this.poslate_rb.CheckedChanged += new System.EventHandler(this.tipPorukeRB_CheckedChanged);
            this.poslate_rb.Click += new System.EventHandler(this.poslate_rb_Click);
            // 
            // primljene_rb
            // 
            this.primljene_rb.AutoSize = true;
            this.primljene_rb.Checked = true;
            this.primljene_rb.Location = new System.Drawing.Point(6, 19);
            this.primljene_rb.Name = "primljene_rb";
            this.primljene_rb.Size = new System.Drawing.Size(67, 17);
            this.primljene_rb.TabIndex = 0;
            this.primljene_rb.TabStop = true;
            this.primljene_rb.Text = "Primljene";
            this.primljene_rb.UseVisualStyleBackColor = true;
            this.primljene_rb.CheckedChanged += new System.EventHandler(this.tipPorukeRB_CheckedChanged);
            this.primljene_rb.Click += new System.EventHandler(this.primljene_rb_Click);
            // 
            // period_gb
            // 
            this.period_gb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.period_gb.Controls.Add(this.do_lbl);
            this.period_gb.Controls.Add(this.do_dtp);
            this.period_gb.Controls.Add(this.od_lbl);
            this.period_gb.Controls.Add(this.od_dtp);
            this.period_gb.Location = new System.Drawing.Point(116, 3);
            this.period_gb.Name = "period_gb";
            this.period_gb.Size = new System.Drawing.Size(272, 94);
            this.period_gb.TabIndex = 16;
            this.period_gb.TabStop = false;
            this.period_gb.Text = "Period";
            // 
            // do_lbl
            // 
            this.do_lbl.AutoSize = true;
            this.do_lbl.Location = new System.Drawing.Point(141, 16);
            this.do_lbl.Name = "do_lbl";
            this.do_lbl.Size = new System.Drawing.Size(24, 13);
            this.do_lbl.TabIndex = 3;
            this.do_lbl.Text = "Od:";
            // 
            // do_dtp
            // 
            this.do_dtp.Location = new System.Drawing.Point(144, 32);
            this.do_dtp.Name = "do_dtp";
            this.do_dtp.Size = new System.Drawing.Size(125, 20);
            this.do_dtp.TabIndex = 2;
            this.do_dtp.ValueChanged += new System.EventHandler(this.doDatuma_dtp_ValueChanged);
            // 
            // od_lbl
            // 
            this.od_lbl.AutoSize = true;
            this.od_lbl.Location = new System.Drawing.Point(6, 16);
            this.od_lbl.Name = "od_lbl";
            this.od_lbl.Size = new System.Drawing.Size(24, 13);
            this.od_lbl.TabIndex = 1;
            this.od_lbl.Text = "Od:";
            // 
            // od_dtp
            // 
            this.od_dtp.Location = new System.Drawing.Point(9, 32);
            this.od_dtp.Name = "od_dtp";
            this.od_dtp.Size = new System.Drawing.Size(125, 20);
            this.od_dtp.TabIndex = 0;
            this.od_dtp.ValueChanged += new System.EventHandler(this.odDatuma_dtp_ValueChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.gbIzborPosiljaoca);
            this.panel2.Controls.Add(this.status_gb);
            this.panel2.Controls.Add(this.tip_gb);
            this.panel2.Controls.Add(this.period_gb);
            this.panel2.Location = new System.Drawing.Point(12, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1028, 100);
            this.panel2.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbIzborAkcijePoruka);
            this.groupBox1.Location = new System.Drawing.Point(642, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(241, 94);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter akcije poruka";
            // 
            // cmbIzborAkcijePoruka
            // 
            this.cmbIzborAkcijePoruka.FormattingEnabled = true;
            this.cmbIzborAkcijePoruka.Location = new System.Drawing.Point(6, 31);
            this.cmbIzborAkcijePoruka.Name = "cmbIzborAkcijePoruka";
            this.cmbIzborAkcijePoruka.Size = new System.Drawing.Size(210, 21);
            this.cmbIzborAkcijePoruka.TabIndex = 1;
            this.cmbIzborAkcijePoruka.SelectedIndexChanged += new System.EventHandler(this.cmbIzborAkcijePoruka_SelectedIndexChanged);
            // 
            // gbIzborPosiljaoca
            // 
            this.gbIzborPosiljaoca.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbIzborPosiljaoca.Controls.Add(this.cmb_Tip);
            this.gbIzborPosiljaoca.Controls.Add(this.label1);
            this.gbIzborPosiljaoca.Controls.Add(this.izborKorespodenta_cmb);
            this.gbIzborPosiljaoca.Location = new System.Drawing.Point(396, 3);
            this.gbIzborPosiljaoca.Name = "gbIzborPosiljaoca";
            this.gbIzborPosiljaoca.Size = new System.Drawing.Size(240, 94);
            this.gbIzborPosiljaoca.TabIndex = 19;
            this.gbIzborPosiljaoca.TabStop = false;
            this.gbIzborPosiljaoca.Text = "Izbor posiljaoca";
            // 
            // cmb_Tip
            // 
            this.cmb_Tip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Tip.FormattingEnabled = true;
            this.cmb_Tip.Location = new System.Drawing.Point(6, 61);
            this.cmb_Tip.Name = "cmb_Tip";
            this.cmb_Tip.Size = new System.Drawing.Size(210, 21);
            this.cmb_Tip.TabIndex = 2;
            this.cmb_Tip.SelectedIndexChanged += new System.EventHandler(this.cmb_Tip_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tip poruke";
            // 
            // izborKorespodenta_cmb
            // 
            this.izborKorespodenta_cmb.FormattingEnabled = true;
            this.izborKorespodenta_cmb.Location = new System.Drawing.Point(6, 19);
            this.izborKorespodenta_cmb.Name = "izborKorespodenta_cmb";
            this.izborKorespodenta_cmb.Size = new System.Drawing.Size(210, 21);
            this.izborKorespodenta_cmb.TabIndex = 0;
            this.izborKorespodenta_cmb.SelectedIndexChanged += new System.EventHandler(this.cmbIzborPosiljaoca_SelectedIndexChanged);
            // 
            // status_gb
            // 
            this.status_gb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status_gb.Controls.Add(this.statusSve_rb);
            this.status_gb.Controls.Add(this.neprocitane_rb);
            this.status_gb.Controls.Add(this.procitane_rb);
            this.status_gb.Location = new System.Drawing.Point(889, 3);
            this.status_gb.Name = "status_gb";
            this.status_gb.Size = new System.Drawing.Size(127, 94);
            this.status_gb.TabIndex = 18;
            this.status_gb.TabStop = false;
            this.status_gb.Text = "Status";
            // 
            // statusSve_rb
            // 
            this.statusSve_rb.AutoSize = true;
            this.statusSve_rb.Checked = true;
            this.statusSve_rb.Location = new System.Drawing.Point(6, 65);
            this.statusSve_rb.Name = "statusSve_rb";
            this.statusSve_rb.Size = new System.Drawing.Size(44, 17);
            this.statusSve_rb.TabIndex = 5;
            this.statusSve_rb.TabStop = true;
            this.statusSve_rb.Text = "Sve";
            this.statusSve_rb.UseVisualStyleBackColor = true;
            this.statusSve_rb.CheckedChanged += new System.EventHandler(this.tipPorukeRB_CheckedChanged);
            // 
            // neprocitane_rb
            // 
            this.neprocitane_rb.AutoSize = true;
            this.neprocitane_rb.Location = new System.Drawing.Point(6, 42);
            this.neprocitane_rb.Name = "neprocitane_rb";
            this.neprocitane_rb.Size = new System.Drawing.Size(89, 17);
            this.neprocitane_rb.TabIndex = 4;
            this.neprocitane_rb.Text = "Ne Arhivirana";
            this.neprocitane_rb.UseVisualStyleBackColor = true;
            this.neprocitane_rb.CheckedChanged += new System.EventHandler(this.tipPorukeRB_CheckedChanged);
            // 
            // procitane_rb
            // 
            this.procitane_rb.AutoSize = true;
            this.procitane_rb.Location = new System.Drawing.Point(6, 19);
            this.procitane_rb.Name = "procitane_rb";
            this.procitane_rb.Size = new System.Drawing.Size(72, 17);
            this.procitane_rb.TabIndex = 3;
            this.procitane_rb.Text = "Arhivirana";
            this.procitane_rb.UseVisualStyleBackColor = true;
            this.procitane_rb.CheckedChanged += new System.EventHandler(this.tipPorukeRB_CheckedChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 159);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1025, 419);
            this.dataGridView1.TabIndex = 18;
            this.dataGridView1.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.dataGridView1_CellContextMenuStripNeeded);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick_1);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arhivirajToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 26);
            // 
            // arhivirajToolStripMenuItem
            // 
            this.arhivirajToolStripMenuItem.Name = "arhivirajToolStripMenuItem";
            this.arhivirajToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.arhivirajToolStripMenuItem.Text = "Arhiviraj";
            this.arhivirajToolStripMenuItem.Click += new System.EventHandler(this.arhivirajToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 581);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1052, 22);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status_lbl
            // 
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(141, 17);
            this.status_lbl.Text = "Spremam ostale poruke...";
            // 
            // _1301_fm_Poruka_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 603);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "_1301_fm_Poruka_List";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Poruke";
            this.Load += new System.EventHandler(this._1301_fm_Poruka_List_Load);
            this.panel1.ResumeLayout(false);
            this.tip_gb.ResumeLayout(false);
            this.tip_gb.PerformLayout();
            this.period_gb.ResumeLayout(false);
            this.period_gb.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gbIzborPosiljaoca.ResumeLayout(false);
            this.status_gb.ResumeLayout(false);
            this.status_gb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button nova_btn;
        private System.Windows.Forms.GroupBox tip_gb;
        private System.Windows.Forms.RadioButton tipSve_rb;
        private System.Windows.Forms.RadioButton poslate_rb;
        private System.Windows.Forms.RadioButton primljene_rb;
        private System.Windows.Forms.GroupBox period_gb;
        private System.Windows.Forms.Label do_lbl;
        private System.Windows.Forms.DateTimePicker do_dtp;
        private System.Windows.Forms.Label od_lbl;
        private System.Windows.Forms.DateTimePicker od_dtp;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox status_gb;
        private System.Windows.Forms.RadioButton statusSve_rb;
        private System.Windows.Forms.RadioButton neprocitane_rb;
        private System.Windows.Forms.RadioButton procitane_rb;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox gbIzborPosiljaoca;
        private System.Windows.Forms.ComboBox izborKorespodenta_cmb;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.ComboBox cmb_Tip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arhivirajToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbIzborAkcijePoruka;
    }
}