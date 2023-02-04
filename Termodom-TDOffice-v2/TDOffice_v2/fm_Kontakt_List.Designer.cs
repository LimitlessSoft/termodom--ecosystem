
namespace TDOffice_v2
{
    partial class fm_Kontakt_List
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
            this.blokirajToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.odBlokirajToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_Pretraga = new System.Windows.Forms.TextBox();
            this.cmb_PoljePretrage = new System.Windows.Forms.ComboBox();
            this.blokirani_btn = new System.Windows.Forms.Button();
            this.nova_btn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slogova_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 53);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(785, 388);
            this.dataGridView1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blokirajToolStripMenuItem,
            this.odBlokirajToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 52);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // blokirajToolStripMenuItem
            // 
            this.blokirajToolStripMenuItem.Name = "blokirajToolStripMenuItem";
            this.blokirajToolStripMenuItem.Size = new System.Drawing.Size(148, 24);
            this.blokirajToolStripMenuItem.Text = "Blokiraj";
            this.blokirajToolStripMenuItem.Click += new System.EventHandler(this.blokirajToolStripMenuItem_Click);
            // 
            // odBlokirajToolStripMenuItem
            // 
            this.odBlokirajToolStripMenuItem.Name = "odBlokirajToolStripMenuItem";
            this.odBlokirajToolStripMenuItem.Size = new System.Drawing.Size(148, 24);
            this.odBlokirajToolStripMenuItem.Text = "OdBlokiraj";
            this.odBlokirajToolStripMenuItem.Click += new System.EventHandler(this.odBlokirajToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.txt_Pretraga);
            this.panel1.Controls.Add(this.cmb_PoljePretrage);
            this.panel1.Controls.Add(this.blokirani_btn);
            this.panel1.Controls.Add(this.nova_btn);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(785, 35);
            this.panel1.TabIndex = 19;
            // 
            // txt_Pretraga
            // 
            this.txt_Pretraga.Location = new System.Drawing.Point(200, 7);
            this.txt_Pretraga.Name = "txt_Pretraga";
            this.txt_Pretraga.Size = new System.Drawing.Size(217, 20);
            this.txt_Pretraga.TabIndex = 16;
            this.txt_Pretraga.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Pretraga_KeyDown);
            // 
            // cmb_PoljePretrage
            // 
            this.cmb_PoljePretrage.FormattingEnabled = true;
            this.cmb_PoljePretrage.Location = new System.Drawing.Point(73, 6);
            this.cmb_PoljePretrage.Name = "cmb_PoljePretrage";
            this.cmb_PoljePretrage.Size = new System.Drawing.Size(121, 21);
            this.cmb_PoljePretrage.TabIndex = 15;
            // 
            // blokirani_btn
            // 
            this.blokirani_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.blokirani_btn.Location = new System.Drawing.Point(707, 6);
            this.blokirani_btn.Name = "blokirani_btn";
            this.blokirani_btn.Size = new System.Drawing.Size(75, 23);
            this.blokirani_btn.TabIndex = 14;
            this.blokirani_btn.Text = "Blokirani";
            this.blokirani_btn.UseVisualStyleBackColor = true;
            this.blokirani_btn.Click += new System.EventHandler(this.blokirani_btn_Click);
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
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slogova_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 444);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(809, 26);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slogova_lbl
            // 
            this.slogova_lbl.Name = "slogova_lbl";
            this.slogova_lbl.Size = new System.Drawing.Size(70, 20);
            this.slogova_lbl.Text = "Slogova: ";
            // 
            // fm_Kontakt_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 470);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "fm_Kontakt_List";
            this.Text = "Kontakti";
            this.Load += new System.EventHandler(this.fm_Kontakt_List_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button nova_btn;
        private System.Windows.Forms.Button blokirani_btn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem blokirajToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem odBlokirajToolStripMenuItem;
        private System.Windows.Forms.TextBox txt_Pretraga;
        private System.Windows.Forms.ComboBox cmb_PoljePretrage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slogova_lbl;
    }
}