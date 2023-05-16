
namespace TDOffice_v2
{
    partial class fm_ListaRobe
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.obrisiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.obrisiSveStavkeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_DodajListu = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtn_SacuvajListu = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtn_OtvoriListu = new System.Windows.Forms.ToolStripButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Location = new System.Drawing.Point(12, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(958, 451);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeight = 29;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(952, 445);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.obrisiToolStripMenuItem,
            this.obrisiSveStavkeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(162, 48);
            // 
            // obrisiToolStripMenuItem
            // 
            this.obrisiToolStripMenuItem.Name = "obrisiToolStripMenuItem";
            this.obrisiToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.obrisiToolStripMenuItem.Text = "Obrisi";
            this.obrisiToolStripMenuItem.Click += new System.EventHandler(this.obrisiToolStripMenuItem_Click);
            // 
            // obrisiSveStavkeToolStripMenuItem
            // 
            this.obrisiSveStavkeToolStripMenuItem.Name = "obrisiSveStavkeToolStripMenuItem";
            this.obrisiSveStavkeToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.obrisiSveStavkeToolStripMenuItem.Text = "Obrisi sve stavke";
            this.obrisiSveStavkeToolStripMenuItem.Click += new System.EventHandler(this.obrisiSveStavkeToolStripMenuItem_Click);
            // 
            // btn_DodajListu
            // 
            this.btn_DodajListu.Location = new System.Drawing.Point(632, 488);
            this.btn_DodajListu.Name = "btn_DodajListu";
            this.btn_DodajListu.Size = new System.Drawing.Size(338, 37);
            this.btn_DodajListu.TabIndex = 2;
            this.btn_DodajListu.Text = "Dodaj listu";
            this.btn_DodajListu.UseVisualStyleBackColor = true;
            this.btn_DodajListu.Click += new System.EventHandler(this.btn_DodajListu_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Lista Robe Komercijalno|*.rlt";
            this.openFileDialog1.Title = "Izbor liste robe";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Lista robe|*.lro";
            this.saveFileDialog1.Title = "Save liste  robe";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.tsbtn_SacuvajListu,
            this.toolStripSeparator2,
            this.tsbtn_OtvoriListu});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(982, 27);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbtn_SacuvajListu
            // 
            this.tsbtn_SacuvajListu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_SacuvajListu.Image = global::TDOffice_v2.Properties.Resources.save_icon;
            this.tsbtn_SacuvajListu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_SacuvajListu.Name = "tsbtn_SacuvajListu";
            this.tsbtn_SacuvajListu.Size = new System.Drawing.Size(24, 24);
            this.tsbtn_SacuvajListu.Text = "Sacuvaj listu";
            this.tsbtn_SacuvajListu.Click += new System.EventHandler(this.tsbtn_SacuvajListu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbtn_OtvoriListu
            // 
            this.tsbtn_OtvoriListu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_OtvoriListu.Image = global::TDOffice_v2.Properties.Resources.new_icon;
            this.tsbtn_OtvoriListu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_OtvoriListu.Name = "tsbtn_OtvoriListu";
            this.tsbtn_OtvoriListu.Size = new System.Drawing.Size(24, 24);
            this.tsbtn_OtvoriListu.Text = "Otvori listu";
            this.tsbtn_OtvoriListu.Click += new System.EventHandler(this.tsbtn_OtvoriListu_Click);
            // 
            // fm_ListaRobe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 533);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btn_DodajListu);
            this.Controls.Add(this.panel2);
            this.Name = "fm_ListaRobe";
            this.Text = "fm_ListaRobe";
            this.Load += new System.EventHandler(this.fm_ListaRobe_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_DodajListu;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem obrisiSveStavkeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtn_SacuvajListu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbtn_OtvoriListu;
        private System.Windows.Forms.ToolStripMenuItem obrisiToolStripMenuItem;
    }
}