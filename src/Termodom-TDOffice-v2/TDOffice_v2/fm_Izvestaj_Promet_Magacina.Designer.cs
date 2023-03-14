
namespace TDOffice_v2
{
    partial class fm_Izvestaj_Promet_Magacina
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
            this.magacini_panel = new System.Windows.Forms.Panel();
            this.magacini_clb = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.godine_panel = new System.Windows.Forms.Panel();
            this.godine_dgv = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.prikaziIzvestaj_btn = new System.Windows.Forms.Button();
            this.status_lbl = new System.Windows.Forms.Label();
            this.btn_Help = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btn_PosaljiIzvestaj = new System.Windows.Forms.Button();
            this.magacini_panel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.godine_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.godine_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // magacini_panel
            // 
            this.magacini_panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.magacini_panel.Controls.Add(this.magacini_clb);
            this.magacini_panel.Location = new System.Drawing.Point(3, 3);
            this.magacini_panel.Name = "magacini_panel";
            this.magacini_panel.Size = new System.Drawing.Size(385, 277);
            this.magacini_panel.TabIndex = 0;
            // 
            // magacini_clb
            // 
            this.magacini_clb.CheckOnClick = true;
            this.magacini_clb.ContextMenuStrip = this.contextMenuStrip1;
            this.magacini_clb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magacini_clb.FormattingEnabled = true;
            this.magacini_clb.Location = new System.Drawing.Point(0, 0);
            this.magacini_clb.Name = "magacini_clb";
            this.magacini_clb.Size = new System.Drawing.Size(385, 277);
            this.magacini_clb.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cekirajSveToolStripMenuItem,
            this.decekirajSveToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(168, 52);
            // 
            // cekirajSveToolStripMenuItem
            // 
            this.cekirajSveToolStripMenuItem.Name = "cekirajSveToolStripMenuItem";
            this.cekirajSveToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.cekirajSveToolStripMenuItem.Text = "Cekiraj Sve";
            this.cekirajSveToolStripMenuItem.Click += new System.EventHandler(this.cekirajSveToolStripMenuItem_Click);
            // 
            // decekirajSveToolStripMenuItem
            // 
            this.decekirajSveToolStripMenuItem.Name = "decekirajSveToolStripMenuItem";
            this.decekirajSveToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.decekirajSveToolStripMenuItem.Text = "Decekiraj Sve";
            this.decekirajSveToolStripMenuItem.Click += new System.EventHandler(this.decekirajSveToolStripMenuItem_Click);
            // 
            // godine_panel
            // 
            this.godine_panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.godine_panel.Controls.Add(this.godine_dgv);
            this.godine_panel.Location = new System.Drawing.Point(6, 286);
            this.godine_panel.Name = "godine_panel";
            this.godine_panel.Size = new System.Drawing.Size(382, 155);
            this.godine_panel.TabIndex = 2;
            // 
            // godine_dgv
            // 
            this.godine_dgv.AllowUserToAddRows = false;
            this.godine_dgv.AllowUserToDeleteRows = false;
            this.godine_dgv.AllowUserToResizeRows = false;
            this.godine_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.godine_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.godine_dgv.Location = new System.Drawing.Point(0, 0);
            this.godine_dgv.Name = "godine_dgv";
            this.godine_dgv.RowHeadersVisible = false;
            this.godine_dgv.RowHeadersWidth = 51;
            this.godine_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.godine_dgv.Size = new System.Drawing.Size(382, 155);
            this.godine_dgv.TabIndex = 11;
            this.godine_dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.godine_dgv_CellClick);
            this.godine_dgv.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.godine_dgv_CellValidating);
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
            this.dataGridView1.Location = new System.Drawing.Point(3, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(659, 435);
            this.dataGridView1.TabIndex = 12;
            // 
            // prikaziIzvestaj_btn
            // 
            this.prikaziIzvestaj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.prikaziIzvestaj_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prikaziIzvestaj_btn.Location = new System.Drawing.Point(720, 482);
            this.prikaziIzvestaj_btn.Name = "prikaziIzvestaj_btn";
            this.prikaziIzvestaj_btn.Size = new System.Drawing.Size(344, 35);
            this.prikaziIzvestaj_btn.TabIndex = 3;
            this.prikaziIzvestaj_btn.Text = "Prikazi";
            this.prikaziIzvestaj_btn.UseVisualStyleBackColor = true;
            this.prikaziIzvestaj_btn.Click += new System.EventHandler(this.prikaziIzvestaj_btn_Click);
            // 
            // status_lbl
            // 
            this.status_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.status_lbl.AutoSize = true;
            this.status_lbl.Location = new System.Drawing.Point(15, 497);
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(41, 15);
            this.status_lbl.TabIndex = 6;
            this.status_lbl.Text = "label1";
            // 
            // btn_Help
            // 
            this.btn_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Help.Location = new System.Drawing.Point(986, 3);
            this.btn_Help.Name = "btn_Help";
            this.btn_Help.Size = new System.Drawing.Size(75, 23);
            this.btn_Help.TabIndex = 7;
            this.btn_Help.Text = "HELP";
            this.btn_Help.UseVisualStyleBackColor = true;
            this.btn_Help.Click += new System.EventHandler(this.btn_Help_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 35);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.magacini_panel);
            this.splitContainer1.Panel1.Controls.Add(this.godine_panel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1052, 444);
            this.splitContainer1.SplitterDistance = 391;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 8;
            // 
            // btn_PosaljiIzvestaj
            // 
            this.btn_PosaljiIzvestaj.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PosaljiIzvestaj.Location = new System.Drawing.Point(764, 3);
            this.btn_PosaljiIzvestaj.Name = "btn_PosaljiIzvestaj";
            this.btn_PosaljiIzvestaj.Size = new System.Drawing.Size(188, 28);
            this.btn_PosaljiIzvestaj.TabIndex = 9;
            this.btn_PosaljiIzvestaj.Text = "Posalji izvestaj";
            this.btn_PosaljiIzvestaj.UseVisualStyleBackColor = true;
            this.btn_PosaljiIzvestaj.Click += new System.EventHandler(this.btn_PosaljiIzvestaj_Click);
            // 
            // fm_Izvestaj_Promet_Magacina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 521);
            this.Controls.Add(this.btn_PosaljiIzvestaj);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btn_Help);
            this.Controls.Add(this.status_lbl);
            this.Controls.Add(this.prikaziIzvestaj_btn);
            this.MinimumSize = new System.Drawing.Size(975, 568);
            this.Name = "fm_Izvestaj_Promet_Magacina";
            this.Text = "Izvestaj Promet Magacina";
            this.Load += new System.EventHandler(this.fm_Izvestaj_Promet_Magacina_Load);
            this.magacini_panel.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.godine_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.godine_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel magacini_panel;
        private System.Windows.Forms.CheckedListBox magacini_clb;
        private System.Windows.Forms.Panel godine_panel;
        private System.Windows.Forms.DataGridView godine_dgv;
        private System.Windows.Forms.Button prikaziIzvestaj_btn;
        private System.Windows.Forms.Label status_lbl;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cekirajSveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decekirajSveToolStripMenuItem;
        private System.Windows.Forms.Button btn_Help;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btn_PosaljiIzvestaj;
    }
}