namespace TDOffice_v2
{
    partial class fm_Partner_List
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
            this.ukloniPartneraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nova_btn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.akcijeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.posaljiSMSPorukeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grupe_clb = new System.Windows.Forms.CheckedListBox();
            this.filtriraj_btn = new System.Windows.Forms.Button();
            this.magacin_clb = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 168);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(776, 257);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ukloniPartneraToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(156, 26);
            // 
            // ukloniPartneraToolStripMenuItem
            // 
            this.ukloniPartneraToolStripMenuItem.Name = "ukloniPartneraToolStripMenuItem";
            this.ukloniPartneraToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.ukloniPartneraToolStripMenuItem.Text = "Ukloni Partnera";
            this.ukloniPartneraToolStripMenuItem.Click += new System.EventHandler(this.ukloniPartneraToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.nova_btn);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Location = new System.Drawing.Point(14, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 35);
            this.panel1.TabIndex = 21;
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
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.akcijeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(189, 5);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(59, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // akcijeToolStripMenuItem
            // 
            this.akcijeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.posaljiSMSPorukeToolStripMenuItem});
            this.akcijeToolStripMenuItem.Name = "akcijeToolStripMenuItem";
            this.akcijeToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.akcijeToolStripMenuItem.Text = "Akcije";
            // 
            // posaljiSMSPorukeToolStripMenuItem
            // 
            this.posaljiSMSPorukeToolStripMenuItem.Name = "posaljiSMSPorukeToolStripMenuItem";
            this.posaljiSMSPorukeToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.posaljiSMSPorukeToolStripMenuItem.Text = "Izlistanim partnerima posalji sms";
            this.posaljiSMSPorukeToolStripMenuItem.Click += new System.EventHandler(this.posaljiSMSPorukeToolStripMenuItem_Click);
            // 
            // grupe_clb
            // 
            this.grupe_clb.CheckOnClick = true;
            this.grupe_clb.FormattingEnabled = true;
            this.grupe_clb.Location = new System.Drawing.Point(14, 53);
            this.grupe_clb.Name = "grupe_clb";
            this.grupe_clb.Size = new System.Drawing.Size(269, 109);
            this.grupe_clb.TabIndex = 15;
            // 
            // filtriraj_btn
            // 
            this.filtriraj_btn.Location = new System.Drawing.Point(646, 136);
            this.filtriraj_btn.Name = "filtriraj_btn";
            this.filtriraj_btn.Size = new System.Drawing.Size(142, 23);
            this.filtriraj_btn.TabIndex = 23;
            this.filtriraj_btn.Text = "Filtriraj";
            this.filtriraj_btn.UseVisualStyleBackColor = true;
            this.filtriraj_btn.Click += new System.EventHandler(this.filtriraj_btn_Click);
            // 
            // magacin_clb
            // 
            this.magacin_clb.CheckOnClick = true;
            this.magacin_clb.FormattingEnabled = true;
            this.magacin_clb.Location = new System.Drawing.Point(289, 53);
            this.magacin_clb.Name = "magacin_clb";
            this.magacin_clb.Size = new System.Drawing.Size(269, 109);
            this.magacin_clb.TabIndex = 24;
            // 
            // fm_Partner_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.magacin_clb);
            this.Controls.Add(this.filtriraj_btn);
            this.Controls.Add(this.grupe_clb);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "fm_Partner_List";
            this.Text = "Svi Partneri";
            this.Load += new System.EventHandler(this.fm_Partner_List_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button nova_btn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ukloniPartneraToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem akcijeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem posaljiSMSPorukeToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox grupe_clb;
        private System.Windows.Forms.Button filtriraj_btn;
        private System.Windows.Forms.CheckedListBox magacin_clb;
    }
}