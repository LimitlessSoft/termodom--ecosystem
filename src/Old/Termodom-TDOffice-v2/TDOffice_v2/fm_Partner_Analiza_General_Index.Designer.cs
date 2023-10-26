
namespace TDOffice_v2
{
    partial class fm_Partner_Analiza_General_Index
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
            this.partner_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.analiziraj_btn = new System.Windows.Forms.Button();
            this.analiza_rtb = new System.Windows.Forms.RichTextBox();
            this.promet_dgv = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.roba_dgv = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.karticaRobeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.podesavanjeSetovaProizvoda_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.promet_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roba_dgv)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // partner_cmb
            // 
            this.partner_cmb.FormattingEnabled = true;
            this.partner_cmb.Location = new System.Drawing.Point(12, 25);
            this.partner_cmb.Name = "partner_cmb";
            this.partner_cmb.Size = new System.Drawing.Size(342, 21);
            this.partner_cmb.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Partner:";
            // 
            // analiziraj_btn
            // 
            this.analiziraj_btn.Location = new System.Drawing.Point(360, 23);
            this.analiziraj_btn.Name = "analiziraj_btn";
            this.analiziraj_btn.Size = new System.Drawing.Size(75, 23);
            this.analiziraj_btn.TabIndex = 2;
            this.analiziraj_btn.Text = "Analiziraj";
            this.analiziraj_btn.UseVisualStyleBackColor = true;
            this.analiziraj_btn.Click += new System.EventHandler(this.analiziraj_btn_Click);
            // 
            // analiza_rtb
            // 
            this.analiza_rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.analiza_rtb.Location = new System.Drawing.Point(3, 3);
            this.analiza_rtb.Name = "analiza_rtb";
            this.analiza_rtb.Size = new System.Drawing.Size(331, 446);
            this.analiza_rtb.TabIndex = 3;
            this.analiza_rtb.Text = "";
            // 
            // promet_dgv
            // 
            this.promet_dgv.AllowUserToAddRows = false;
            this.promet_dgv.AllowUserToDeleteRows = false;
            this.promet_dgv.AllowUserToResizeRows = false;
            this.promet_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promet_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.promet_dgv.Location = new System.Drawing.Point(3, 3);
            this.promet_dgv.Name = "promet_dgv";
            this.promet_dgv.ReadOnly = true;
            this.promet_dgv.RowHeadersVisible = false;
            this.promet_dgv.RowHeadersWidth = 51;
            this.promet_dgv.Size = new System.Drawing.Size(1230, 163);
            this.promet_dgv.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(12, 52);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.promet_dgv);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1238, 629);
            this.splitContainer1.SplitterDistance = 171;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.comboBox1);
            this.splitContainer2.Panel1.Controls.Add(this.textBox1);
            this.splitContainer2.Panel1.Controls.Add(this.roba_dgv);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.analiza_rtb);
            this.splitContainer2.Size = new System.Drawing.Size(1238, 454);
            this.splitContainer2.SplitterDistance = 895;
            this.splitContainer2.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.IndianRed;
            this.label3.Location = new System.Drawing.Point(665, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(287, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Sve prikazane vrednosti su bez PDV-a";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(147, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(217, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // roba_dgv
            // 
            this.roba_dgv.AllowUserToAddRows = false;
            this.roba_dgv.AllowUserToDeleteRows = false;
            this.roba_dgv.AllowUserToResizeRows = false;
            this.roba_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roba_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.roba_dgv.ContextMenuStrip = this.contextMenuStrip1;
            this.roba_dgv.Location = new System.Drawing.Point(3, 49);
            this.roba_dgv.Name = "roba_dgv";
            this.roba_dgv.ReadOnly = true;
            this.roba_dgv.RowHeadersVisible = false;
            this.roba_dgv.RowHeadersWidth = 51;
            this.roba_dgv.Size = new System.Drawing.Size(887, 400);
            this.roba_dgv.TabIndex = 5;
            this.roba_dgv.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.roba_dgv_CellMouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.karticaRobeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 28);
            // 
            // karticaRobeToolStripMenuItem
            // 
            this.karticaRobeToolStripMenuItem.Name = "karticaRobeToolStripMenuItem";
            this.karticaRobeToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.karticaRobeToolStripMenuItem.Text = "Kartica robe";
            this.karticaRobeToolStripMenuItem.Click += new System.EventHandler(this.karticaRobeToolStripMenuItem_Click);
            // 
            // podesavanjeSetovaProizvoda_btn
            // 
            this.podesavanjeSetovaProizvoda_btn.Location = new System.Drawing.Point(1096, 12);
            this.podesavanjeSetovaProizvoda_btn.Name = "podesavanjeSetovaProizvoda_btn";
            this.podesavanjeSetovaProizvoda_btn.Size = new System.Drawing.Size(150, 23);
            this.podesavanjeSetovaProizvoda_btn.TabIndex = 6;
            this.podesavanjeSetovaProizvoda_btn.Text = "Podesavanje Setova Proizvoda";
            this.podesavanjeSetovaProizvoda_btn.UseVisualStyleBackColor = true;
            this.podesavanjeSetovaProizvoda_btn.Click += new System.EventHandler(this.podesavanjeSetovaProizvoda_btn_Click);
            // 
            // fm_Partner_Analiza_General_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 693);
            this.Controls.Add(this.podesavanjeSetovaProizvoda_btn);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.analiziraj_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.partner_cmb);
            this.Name = "fm_Partner_Analiza_General_Index";
            this.Text = "Analiza Partnera";
            this.Load += new System.EventHandler(this.fm_Partner_Analiza_General_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.promet_dgv)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.roba_dgv)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox partner_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button analiziraj_btn;
        private System.Windows.Forms.RichTextBox analiza_rtb;
        private System.Windows.Forms.DataGridView promet_dgv;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView roba_dgv;
        private System.Windows.Forms.Button podesavanjeSetovaProizvoda_btn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem karticaRobeToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
    }
}