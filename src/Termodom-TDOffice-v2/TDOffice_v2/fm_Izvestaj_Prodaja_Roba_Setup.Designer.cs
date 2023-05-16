
namespace TDOffice_v2
{
    partial class fm_Izvestaj_Prodaja_Roba_Setup
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
            this.magacini_clb = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.godine_panel = new System.Windows.Forms.Panel();
            this.godine_dgv = new System.Windows.Forms.DataGridView();
            this.roba_dgv = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.help_btn = new System.Windows.Forms.Button();
            this.vrednostPreracunavajUzPomoc_gb = new System.Windows.Forms.GroupBox();
            this.realneCene_rb = new System.Windows.Forms.RadioButton();
            this.poslednjeCene_rb = new System.Windows.Forms.RadioButton();
            this.listaRobe_cb = new System.Windows.Forms.CheckBox();
            this.listaRobe_btn = new System.Windows.Forms.Button();
            this.prikaziIzvestaj_btn = new System.Windows.Forms.Button();
            this.status_lbl = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.godine_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.godine_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roba_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.vrednostPreracunavajUzPomoc_gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // magacini_clb
            // 
            this.magacini_clb.CheckOnClick = true;
            this.magacini_clb.ContextMenuStrip = this.contextMenuStrip1;
            this.magacini_clb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magacini_clb.FormattingEnabled = true;
            this.magacini_clb.Location = new System.Drawing.Point(0, 0);
            this.magacini_clb.Name = "magacini_clb";
            this.magacini_clb.Size = new System.Drawing.Size(956, 311);
            this.magacini_clb.TabIndex = 0;
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
            this.godine_panel.Controls.Add(this.godine_dgv);
            this.godine_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.godine_panel.Location = new System.Drawing.Point(0, 0);
            this.godine_panel.Name = "godine_panel";
            this.godine_panel.Size = new System.Drawing.Size(956, 311);
            this.godine_panel.TabIndex = 1;
            // 
            // godine_dgv
            // 
            this.godine_dgv.AllowUserToAddRows = false;
            this.godine_dgv.AllowUserToDeleteRows = false;
            this.godine_dgv.AllowUserToResizeRows = false;
            this.godine_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.godine_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.godine_dgv.Location = new System.Drawing.Point(7, 3);
            this.godine_dgv.Name = "godine_dgv";
            this.godine_dgv.RowHeadersVisible = false;
            this.godine_dgv.RowHeadersWidth = 51;
            this.godine_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.godine_dgv.Size = new System.Drawing.Size(942, 305);
            this.godine_dgv.TabIndex = 10;
            this.godine_dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.godine_dgv_CellClick);
            this.godine_dgv.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.godine_dgv_CellValidating);
            // 
            // roba_dgv
            // 
            this.roba_dgv.AllowUserToAddRows = false;
            this.roba_dgv.AllowUserToDeleteRows = false;
            this.roba_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roba_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.roba_dgv.Location = new System.Drawing.Point(3, 156);
            this.roba_dgv.Name = "roba_dgv";
            this.roba_dgv.ReadOnly = true;
            this.roba_dgv.RowHeadersVisible = false;
            this.roba_dgv.RowHeadersWidth = 51;
            this.roba_dgv.Size = new System.Drawing.Size(511, 469);
            this.roba_dgv.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.help_btn);
            this.splitContainer1.Panel2.Controls.Add(this.vrednostPreracunavajUzPomoc_gb);
            this.splitContainer1.Panel2.Controls.Add(this.listaRobe_cb);
            this.splitContainer1.Panel2.Controls.Add(this.listaRobe_btn);
            this.splitContainer1.Panel2.Controls.Add(this.roba_dgv);
            this.splitContainer1.Size = new System.Drawing.Size(1481, 630);
            this.splitContainer1.SplitterDistance = 958;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.magacini_clb);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.godine_panel);
            this.splitContainer2.Size = new System.Drawing.Size(958, 630);
            this.splitContainer2.SplitterDistance = 313;
            this.splitContainer2.TabIndex = 2;
            // 
            // help_btn
            // 
            this.help_btn.Location = new System.Drawing.Point(439, 3);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(75, 23);
            this.help_btn.TabIndex = 9;
            this.help_btn.Text = "HELP";
            this.help_btn.UseVisualStyleBackColor = true;
            this.help_btn.Click += new System.EventHandler(this.help_btn_Click);
            // 
            // vrednostPreracunavajUzPomoc_gb
            // 
            this.vrednostPreracunavajUzPomoc_gb.Controls.Add(this.realneCene_rb);
            this.vrednostPreracunavajUzPomoc_gb.Controls.Add(this.poslednjeCene_rb);
            this.vrednostPreracunavajUzPomoc_gb.Location = new System.Drawing.Point(1, 3);
            this.vrednostPreracunavajUzPomoc_gb.Name = "vrednostPreracunavajUzPomoc_gb";
            this.vrednostPreracunavajUzPomoc_gb.Size = new System.Drawing.Size(270, 74);
            this.vrednostPreracunavajUzPomoc_gb.TabIndex = 8;
            this.vrednostPreracunavajUzPomoc_gb.TabStop = false;
            this.vrednostPreracunavajUzPomoc_gb.Text = "Vrednost preracunavaj uz pomoc";
            // 
            // realneCene_rb
            // 
            this.realneCene_rb.AutoSize = true;
            this.realneCene_rb.Location = new System.Drawing.Point(6, 42);
            this.realneCene_rb.Name = "realneCene_rb";
            this.realneCene_rb.Size = new System.Drawing.Size(213, 19);
            this.realneCene_rb.TabIndex = 1;
            this.realneCene_rb.Text = "Realne cene (cene iz dokumenta)";
            this.realneCene_rb.UseVisualStyleBackColor = true;
            // 
            // poslednjeCene_rb
            // 
            this.poslednjeCene_rb.AutoSize = true;
            this.poslednjeCene_rb.Checked = true;
            this.poslednjeCene_rb.Location = new System.Drawing.Point(6, 19);
            this.poslednjeCene_rb.Name = "poslednjeCene_rb";
            this.poslednjeCene_rb.Size = new System.Drawing.Size(238, 19);
            this.poslednjeCene_rb.TabIndex = 0;
            this.poslednjeCene_rb.TabStop = true;
            this.poslednjeCene_rb.Text = "Poslednje Cene (ponderisan volumen)";
            this.poslednjeCene_rb.UseVisualStyleBackColor = true;
            // 
            // listaRobe_cb
            // 
            this.listaRobe_cb.AutoSize = true;
            this.listaRobe_cb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listaRobe_cb.Location = new System.Drawing.Point(7, 83);
            this.listaRobe_cb.Name = "listaRobe_cb";
            this.listaRobe_cb.Size = new System.Drawing.Size(224, 33);
            this.listaRobe_cb.TabIndex = 7;
            this.listaRobe_cb.Text = "Koristi Listu Robe";
            this.listaRobe_cb.UseVisualStyleBackColor = true;
            // 
            // listaRobe_btn
            // 
            this.listaRobe_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listaRobe_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listaRobe_btn.Location = new System.Drawing.Point(3, 117);
            this.listaRobe_btn.Name = "listaRobe_btn";
            this.listaRobe_btn.Size = new System.Drawing.Size(511, 33);
            this.listaRobe_btn.TabIndex = 6;
            this.listaRobe_btn.Text = "Lista Robe";
            this.listaRobe_btn.UseVisualStyleBackColor = true;
            this.listaRobe_btn.Click += new System.EventHandler(this.listaRobe_btn_Click);
            // 
            // prikaziIzvestaj_btn
            // 
            this.prikaziIzvestaj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.prikaziIzvestaj_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prikaziIzvestaj_btn.Location = new System.Drawing.Point(1199, 648);
            this.prikaziIzvestaj_btn.Name = "prikaziIzvestaj_btn";
            this.prikaziIzvestaj_btn.Size = new System.Drawing.Size(293, 33);
            this.prikaziIzvestaj_btn.TabIndex = 4;
            this.prikaziIzvestaj_btn.Text = "Prikazi Izvestaj";
            this.prikaziIzvestaj_btn.UseVisualStyleBackColor = true;
            this.prikaziIzvestaj_btn.Click += new System.EventHandler(this.prikaziIzvestaj_btn_Click);
            // 
            // status_lbl
            // 
            this.status_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.status_lbl.AutoSize = true;
            this.status_lbl.Location = new System.Drawing.Point(13, 668);
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(41, 15);
            this.status_lbl.TabIndex = 5;
            this.status_lbl.Text = "label1";
            // 
            // fm_Izvestaj_Prodaja_Roba_Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1505, 693);
            this.Controls.Add(this.status_lbl);
            this.Controls.Add(this.prikaziIzvestaj_btn);
            this.Controls.Add(this.splitContainer1);
            this.Name = "fm_Izvestaj_Prodaja_Roba_Setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Izvestaj Prodaje Robe";
            this.Load += new System.EventHandler(this.fm_Izvestaj_Prodaja_Roba_Setup_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.godine_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.godine_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roba_dgv)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.vrednostPreracunavajUzPomoc_gb.ResumeLayout(false);
            this.vrednostPreracunavajUzPomoc_gb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox magacini_clb;
        private System.Windows.Forms.Panel godine_panel;
        private System.Windows.Forms.DataGridView roba_dgv;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button prikaziIzvestaj_btn;
        private System.Windows.Forms.Label status_lbl;
        private System.Windows.Forms.DataGridView godine_dgv;
        private System.Windows.Forms.CheckBox listaRobe_cb;
        private System.Windows.Forms.Button listaRobe_btn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cekirajSveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decekirajSveToolStripMenuItem;
        private System.Windows.Forms.GroupBox vrednostPreracunavajUzPomoc_gb;
        private System.Windows.Forms.RadioButton realneCene_rb;
        private System.Windows.Forms.RadioButton poslednjeCene_rb;
        private System.Windows.Forms.Button help_btn;
    }
}