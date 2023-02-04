
namespace TDOffice_v2
{
    partial class fm_StampaEtikete
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.brisiSelektovanuStavkuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brisiSveStavkeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmb_VrstaDokumenta = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBrojDokumenta = new System.Windows.Forms.Label();
            this.tb_BrojDokumenta = new System.Windows.Forms.TextBox();
            this.btn_Prikazi = new System.Windows.Forms.Button();
            this.btn_Stampaj = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slogova_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(7, 63);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(786, 357);
            this.dataGridView1.TabIndex = 20;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.brisiSelektovanuStavkuToolStripMenuItem,
            this.brisiSveStavkeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(200, 48);
            // 
            // brisiSelektovanuStavkuToolStripMenuItem
            // 
            this.brisiSelektovanuStavkuToolStripMenuItem.Name = "brisiSelektovanuStavkuToolStripMenuItem";
            this.brisiSelektovanuStavkuToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.brisiSelektovanuStavkuToolStripMenuItem.Text = "Brisi selektovanu stavku";
            this.brisiSelektovanuStavkuToolStripMenuItem.Click += new System.EventHandler(this.brisiSelektovanuStavkuToolStripMenuItem_Click);
            // 
            // brisiSveStavkeToolStripMenuItem
            // 
            this.brisiSveStavkeToolStripMenuItem.Name = "brisiSveStavkeToolStripMenuItem";
            this.brisiSveStavkeToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.brisiSveStavkeToolStripMenuItem.Text = "Brisi sve stavke";
            this.brisiSveStavkeToolStripMenuItem.Click += new System.EventHandler(this.brisiSveStavkeToolStripMenuItem_Click);
            // 
            // cmb_VrstaDokumenta
            // 
            this.cmb_VrstaDokumenta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_VrstaDokumenta.FormattingEnabled = true;
            this.cmb_VrstaDokumenta.Location = new System.Drawing.Point(7, 36);
            this.cmb_VrstaDokumenta.Name = "cmb_VrstaDokumenta";
            this.cmb_VrstaDokumenta.Size = new System.Drawing.Size(194, 21);
            this.cmb_VrstaDokumenta.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Vrsta dokumenta";
            // 
            // lblBrojDokumenta
            // 
            this.lblBrojDokumenta.AutoSize = true;
            this.lblBrojDokumenta.Location = new System.Drawing.Point(234, 18);
            this.lblBrojDokumenta.Name = "lblBrojDokumenta";
            this.lblBrojDokumenta.Size = new System.Drawing.Size(81, 13);
            this.lblBrojDokumenta.TabIndex = 23;
            this.lblBrojDokumenta.Text = "Broj dokumenta";
            // 
            // tb_BrojDokumenta
            // 
            this.tb_BrojDokumenta.Location = new System.Drawing.Point(237, 34);
            this.tb_BrojDokumenta.Name = "tb_BrojDokumenta";
            this.tb_BrojDokumenta.Size = new System.Drawing.Size(111, 20);
            this.tb_BrojDokumenta.TabIndex = 24;
            this.tb_BrojDokumenta.Text = "0";
            this.tb_BrojDokumenta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_BrojDokumenta_KeyPress);
            // 
            // btn_Prikazi
            // 
            this.btn_Prikazi.Location = new System.Drawing.Point(393, 31);
            this.btn_Prikazi.Name = "btn_Prikazi";
            this.btn_Prikazi.Size = new System.Drawing.Size(112, 23);
            this.btn_Prikazi.TabIndex = 25;
            this.btn_Prikazi.Text = "Prikazi";
            this.btn_Prikazi.UseVisualStyleBackColor = true;
            this.btn_Prikazi.Click += new System.EventHandler(this.btn_Prikazi_Click);
            // 
            // btn_Stampaj
            // 
            this.btn_Stampaj.BackgroundImage = global::TDOffice_v2.Properties.Resources.printer_icon;
            this.btn_Stampaj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Stampaj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Stampaj.Location = new System.Drawing.Point(536, 29);
            this.btn_Stampaj.Name = "btn_Stampaj";
            this.btn_Stampaj.Size = new System.Drawing.Size(42, 25);
            this.btn_Stampaj.TabIndex = 42;
            this.btn_Stampaj.UseVisualStyleBackColor = true;
            this.btn_Stampaj.Click += new System.EventHandler(this.btn_Stampaj_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slogova_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 44;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slogova_lbl
            // 
            this.slogova_lbl.Name = "slogova_lbl";
            this.slogova_lbl.Size = new System.Drawing.Size(55, 17);
            this.slogova_lbl.Text = "Slogova: ";
            // 
            // fm_StampaEtikete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btn_Stampaj);
            this.Controls.Add(this.btn_Prikazi);
            this.Controls.Add(this.lblBrojDokumenta);
            this.Controls.Add(this.tb_BrojDokumenta);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmb_VrstaDokumenta);
            this.Controls.Add(this.dataGridView1);
            this.Name = "fm_StampaEtikete";
            this.Text = "fm_StampaEtikete";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmb_VrstaDokumenta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblBrojDokumenta;
        private System.Windows.Forms.TextBox tb_BrojDokumenta;
        private System.Windows.Forms.Button btn_Prikazi;
        private System.Windows.Forms.Button btn_Stampaj;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slogova_lbl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem brisiSelektovanuStavkuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brisiSveStavkeToolStripMenuItem;
    }
}