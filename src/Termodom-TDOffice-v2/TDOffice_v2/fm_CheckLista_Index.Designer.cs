
namespace TDOffice_v2
{
    partial class fm_CheckLista_Index
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
            this.korisnik_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.zadaci_cmb = new System.Windows.Forms.ComboBox();
            this.dodajZadatak_btn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ukloniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.izmeniIntervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // korisnik_cmb
            // 
            this.korisnik_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.korisnik_cmb.FormattingEnabled = true;
            this.korisnik_cmb.Location = new System.Drawing.Point(65, 12);
            this.korisnik_cmb.Name = "korisnik_cmb";
            this.korisnik_cmb.Size = new System.Drawing.Size(248, 21);
            this.korisnik_cmb.TabIndex = 0;
            this.korisnik_cmb.SelectedIndexChanged += new System.EventHandler(this.korisnik_cmb_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Korisnik:";
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 39);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(1097, 549);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // zadaci_cmb
            // 
            this.zadaci_cmb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.zadaci_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zadaci_cmb.FormattingEnabled = true;
            this.zadaci_cmb.Location = new System.Drawing.Point(747, 12);
            this.zadaci_cmb.Name = "zadaci_cmb";
            this.zadaci_cmb.Size = new System.Drawing.Size(248, 21);
            this.zadaci_cmb.TabIndex = 3;
            // 
            // dodajZadatak_btn
            // 
            this.dodajZadatak_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dodajZadatak_btn.Location = new System.Drawing.Point(1001, 10);
            this.dodajZadatak_btn.Name = "dodajZadatak_btn";
            this.dodajZadatak_btn.Size = new System.Drawing.Size(108, 23);
            this.dodajZadatak_btn.TabIndex = 4;
            this.dodajZadatak_btn.Text = "Dodaj Zadatak";
            this.dodajZadatak_btn.UseVisualStyleBackColor = true;
            this.dodajZadatak_btn.Click += new System.EventHandler(this.dodajZadatak_btn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.izmeniIntervalToolStripMenuItem,
            this.toolStripSeparator1,
            this.ukloniToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(152, 54);
            // 
            // ukloniToolStripMenuItem
            // 
            this.ukloniToolStripMenuItem.Name = "ukloniToolStripMenuItem";
            this.ukloniToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ukloniToolStripMenuItem.Text = "Ukloni";
            this.ukloniToolStripMenuItem.Click += new System.EventHandler(this.ukloniToolStripMenuItem_Click);
            // 
            // izmeniIntervalToolStripMenuItem
            // 
            this.izmeniIntervalToolStripMenuItem.Name = "izmeniIntervalToolStripMenuItem";
            this.izmeniIntervalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.izmeniIntervalToolStripMenuItem.Text = "Izmeni Interval";
            this.izmeniIntervalToolStripMenuItem.Click += new System.EventHandler(this.izmeniIntervalToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // fm_CheckLista_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 600);
            this.Controls.Add(this.dodajZadatak_btn);
            this.Controls.Add(this.zadaci_cmb);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.korisnik_cmb);
            this.Name = "fm_CheckLista_Index";
            this.Text = "Check Lista";
            this.Load += new System.EventHandler(this.fm_CheckLista_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox korisnik_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox zadaci_cmb;
        private System.Windows.Forms.Button dodajZadatak_btn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem izmeniIntervalToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ukloniToolStripMenuItem;
    }
}