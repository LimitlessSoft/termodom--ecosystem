
namespace TDOffice_v2
{
    partial class fm_Izvestaj_Prodaja_Roba_Index
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
            this.posaljiKaoIzvestaj_btn = new System.Windows.Forms.Button();
            this.prikaziKolicine_cb = new System.Windows.Forms.CheckBox();
            this.prikaziVrednosti_cb = new System.Windows.Forms.CheckBox();
            this.filterMagacini_clb = new System.Windows.Forms.CheckedListBox();
            this.prikaziZbirnoPoMagacinima_btn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.decekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prikaziZbirnoPoRobi_btn = new System.Windows.Forms.Button();
            this.help_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 156);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1537, 502);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Visible = false;
            // 
            // posaljiKaoIzvestaj_btn
            // 
            this.posaljiKaoIzvestaj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.posaljiKaoIzvestaj_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posaljiKaoIzvestaj_btn.Location = new System.Drawing.Point(1253, 12);
            this.posaljiKaoIzvestaj_btn.Name = "posaljiKaoIzvestaj_btn";
            this.posaljiKaoIzvestaj_btn.Size = new System.Drawing.Size(215, 62);
            this.posaljiKaoIzvestaj_btn.TabIndex = 1;
            this.posaljiKaoIzvestaj_btn.Text = "Posalji Kao Izvestaj";
            this.posaljiKaoIzvestaj_btn.UseVisualStyleBackColor = true;
            this.posaljiKaoIzvestaj_btn.Click += new System.EventHandler(this.posaljiKaoIzvestaj_btn_Click);
            // 
            // prikaziKolicine_cb
            // 
            this.prikaziKolicine_cb.AutoSize = true;
            this.prikaziKolicine_cb.Checked = true;
            this.prikaziKolicine_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.prikaziKolicine_cb.Location = new System.Drawing.Point(12, 12);
            this.prikaziKolicine_cb.Name = "prikaziKolicine_cb";
            this.prikaziKolicine_cb.Size = new System.Drawing.Size(113, 19);
            this.prikaziKolicine_cb.TabIndex = 2;
            this.prikaziKolicine_cb.Text = "Prikazi Kolicine";
            this.prikaziKolicine_cb.UseVisualStyleBackColor = true;
            this.prikaziKolicine_cb.CheckedChanged += new System.EventHandler(this.prikaziKolicine_cb_CheckedChanged);
            // 
            // prikaziVrednosti_cb
            // 
            this.prikaziVrednosti_cb.AutoSize = true;
            this.prikaziVrednosti_cb.Checked = true;
            this.prikaziVrednosti_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.prikaziVrednosti_cb.Location = new System.Drawing.Point(12, 35);
            this.prikaziVrednosti_cb.Name = "prikaziVrednosti_cb";
            this.prikaziVrednosti_cb.Size = new System.Drawing.Size(120, 19);
            this.prikaziVrednosti_cb.TabIndex = 3;
            this.prikaziVrednosti_cb.Text = "Prikazi Vrednosti";
            this.prikaziVrednosti_cb.UseVisualStyleBackColor = true;
            this.prikaziVrednosti_cb.CheckedChanged += new System.EventHandler(this.prikaziVrednosti_cb_CheckedChanged);
            // 
            // filterMagacini_clb
            // 
            this.filterMagacini_clb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterMagacini_clb.CheckOnClick = true;
            this.filterMagacini_clb.ColumnWidth = 300;
            this.filterMagacini_clb.FormattingEnabled = true;
            this.filterMagacini_clb.Location = new System.Drawing.Point(144, 12);
            this.filterMagacini_clb.MultiColumn = true;
            this.filterMagacini_clb.Name = "filterMagacini_clb";
            this.filterMagacini_clb.Size = new System.Drawing.Size(1103, 139);
            this.filterMagacini_clb.TabIndex = 6;
            this.filterMagacini_clb.SelectedIndexChanged += new System.EventHandler(this.filterMagacini_clb_SelectedIndexChanged);
            // 
            // prikaziZbirnoPoMagacinima_btn
            // 
            this.prikaziZbirnoPoMagacinima_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prikaziZbirnoPoMagacinima_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prikaziZbirnoPoMagacinima_btn.Location = new System.Drawing.Point(1253, 80);
            this.prikaziZbirnoPoMagacinima_btn.Name = "prikaziZbirnoPoMagacinima_btn";
            this.prikaziZbirnoPoMagacinima_btn.Size = new System.Drawing.Size(215, 32);
            this.prikaziZbirnoPoMagacinima_btn.TabIndex = 7;
            this.prikaziZbirnoPoMagacinima_btn.Text = "Prikazi Zbirno Po Magacinima";
            this.prikaziZbirnoPoMagacinima_btn.UseVisualStyleBackColor = true;
            this.prikaziZbirnoPoMagacinima_btn.Click += new System.EventHandler(this.prikaziZbirnoPoMagacinima_btn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cekirajSveToolStripMenuItem,
            this.toolStripSeparator1,
            this.decekirajSveToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(166, 58);
            // 
            // cekirajSveToolStripMenuItem
            // 
            this.cekirajSveToolStripMenuItem.Name = "cekirajSveToolStripMenuItem";
            this.cekirajSveToolStripMenuItem.Size = new System.Drawing.Size(165, 24);
            this.cekirajSveToolStripMenuItem.Text = "Cekiraj sve";
            this.cekirajSveToolStripMenuItem.Click += new System.EventHandler(this.cekirajSveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(162, 6);
            // 
            // decekirajSveToolStripMenuItem
            // 
            this.decekirajSveToolStripMenuItem.Name = "decekirajSveToolStripMenuItem";
            this.decekirajSveToolStripMenuItem.Size = new System.Drawing.Size(165, 24);
            this.decekirajSveToolStripMenuItem.Text = "Decekiraj sve";
            this.decekirajSveToolStripMenuItem.Click += new System.EventHandler(this.decekirajSveToolStripMenuItem_Click);
            // 
            // prikaziZbirnoPoRobi_btn
            // 
            this.prikaziZbirnoPoRobi_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prikaziZbirnoPoRobi_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prikaziZbirnoPoRobi_btn.Location = new System.Drawing.Point(1253, 118);
            this.prikaziZbirnoPoRobi_btn.Name = "prikaziZbirnoPoRobi_btn";
            this.prikaziZbirnoPoRobi_btn.Size = new System.Drawing.Size(215, 32);
            this.prikaziZbirnoPoRobi_btn.TabIndex = 9;
            this.prikaziZbirnoPoRobi_btn.Text = "Prikazi Zbirno Po Robi";
            this.prikaziZbirnoPoRobi_btn.UseVisualStyleBackColor = true;
            this.prikaziZbirnoPoRobi_btn.Click += new System.EventHandler(this.prikaziZbirnoPoRobi_btn_Click);
            // 
            // help_btn
            // 
            this.help_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.help_btn.Location = new System.Drawing.Point(1474, 12);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(75, 23);
            this.help_btn.TabIndex = 10;
            this.help_btn.Text = "HELP";
            this.help_btn.UseVisualStyleBackColor = true;
            this.help_btn.Click += new System.EventHandler(this.help_btn_Click);
            // 
            // fm_Izvestaj_Prodaja_Roba_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1561, 670);
            this.Controls.Add(this.help_btn);
            this.Controls.Add(this.prikaziZbirnoPoRobi_btn);
            this.Controls.Add(this.prikaziZbirnoPoMagacinima_btn);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.filterMagacini_clb);
            this.Controls.Add(this.prikaziVrednosti_cb);
            this.Controls.Add(this.prikaziKolicine_cb);
            this.Controls.Add(this.posaljiKaoIzvestaj_btn);
            this.Name = "fm_Izvestaj_Prodaja_Roba_Index";
            this.Text = "Izvestaj Prodaje Robe";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fm_Izvestaj_Prodaja_Roba_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button posaljiKaoIzvestaj_btn;
        private System.Windows.Forms.CheckBox prikaziKolicine_cb;
        private System.Windows.Forms.CheckBox prikaziVrednosti_cb;
        private System.Windows.Forms.CheckedListBox filterMagacini_clb;
        private System.Windows.Forms.Button prikaziZbirnoPoMagacinima_btn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cekirajSveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem decekirajSveToolStripMenuItem;
        private System.Windows.Forms.Button prikaziZbirnoPoRobi_btn;
        private System.Windows.Forms.Button help_btn;
    }
}