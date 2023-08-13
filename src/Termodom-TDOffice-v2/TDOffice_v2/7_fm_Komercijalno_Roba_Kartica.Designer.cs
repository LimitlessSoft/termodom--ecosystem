
namespace TDOffice_v2
{
    partial class _7_fm_Komercijalno_Roba_Kartica
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
            components = new System.ComponentModel.Container();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            prikaziKomentarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            prikaziInterniKomentarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            podesavanjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            racunajVazecuNabavnuCenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            boldujDokument36ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            partner_cmb = new System.Windows.Forms.ComboBox();
            magacin_cmb = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            cene_dgv = new System.Windows.Forms.DataGridView();
            label2 = new System.Windows.Forms.Label();
            filter_gb = new System.Windows.Forms.GroupBox();
            samoNabavka_rb = new System.Windows.Forms.RadioButton();
            sviDokumenti_rb = new System.Windows.Forms.RadioButton();
            button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cene_dgv).BeginInit();
            filter_gb.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.Location = new System.Drawing.Point(14, 192);
            dataGridView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new System.Drawing.Size(1261, 524);
            dataGridView1.TabIndex = 0;
            dataGridView1.Sorted += dataGridView1_Sorted;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { prikaziKomentarToolStripMenuItem, prikaziInterniKomentarToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(201, 48);
            // 
            // prikaziKomentarToolStripMenuItem
            // 
            prikaziKomentarToolStripMenuItem.Name = "prikaziKomentarToolStripMenuItem";
            prikaziKomentarToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            prikaziKomentarToolStripMenuItem.Text = "Prikazi Komentar";
            prikaziKomentarToolStripMenuItem.Click += prikaziKomentarToolStripMenuItem_Click;
            // 
            // prikaziInterniKomentarToolStripMenuItem
            // 
            prikaziInterniKomentarToolStripMenuItem.Name = "prikaziInterniKomentarToolStripMenuItem";
            prikaziInterniKomentarToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            prikaziInterniKomentarToolStripMenuItem.Text = "Prikazi Interni Komentar";
            prikaziInterniKomentarToolStripMenuItem.Click += prikaziInterniKomentarToolStripMenuItem_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { podesavanjaToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(1289, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // podesavanjaToolStripMenuItem
            // 
            podesavanjaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { racunajVazecuNabavnuCenuToolStripMenuItem, boldujDokument36ToolStripMenuItem });
            podesavanjaToolStripMenuItem.Name = "podesavanjaToolStripMenuItem";
            podesavanjaToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            podesavanjaToolStripMenuItem.Text = "Podesavanja";
            // 
            // racunajVazecuNabavnuCenuToolStripMenuItem
            // 
            racunajVazecuNabavnuCenuToolStripMenuItem.Name = "racunajVazecuNabavnuCenuToolStripMenuItem";
            racunajVazecuNabavnuCenuToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            racunajVazecuNabavnuCenuToolStripMenuItem.Text = "Racunaj Vazecu Nabavnu Cenu";
            racunajVazecuNabavnuCenuToolStripMenuItem.Click += racunajVazecuNabavnuCenuToolStripMenuItem_Click;
            // 
            // boldujDokument36ToolStripMenuItem
            // 
            boldujDokument36ToolStripMenuItem.Name = "boldujDokument36ToolStripMenuItem";
            boldujDokument36ToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            boldujDokument36ToolStripMenuItem.Text = "Bolduj Dokument 36 ( Ulazna Ponuda )";
            boldujDokument36ToolStripMenuItem.Click += boldujDokument36ToolStripMenuItem_Click;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(partner_cmb);
            panel1.Controls.Add(magacin_cmb);
            panel1.Controls.Add(label3);
            panel1.Location = new System.Drawing.Point(14, 32);
            panel1.Margin = new System.Windows.Forms.Padding(5);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(453, 70);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 40);
            label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(48, 15);
            label1.TabIndex = 29;
            label1.Text = "Partner:";
            // 
            // partner_cmb
            // 
            partner_cmb.FormattingEnabled = true;
            partner_cmb.Location = new System.Drawing.Point(70, 37);
            partner_cmb.Margin = new System.Windows.Forms.Padding(5);
            partner_cmb.Name = "partner_cmb";
            partner_cmb.Size = new System.Drawing.Size(353, 23);
            partner_cmb.TabIndex = 28;
            partner_cmb.SelectedIndexChanged += partner_cmb_SelectedIndexChanged;
            // 
            // magacin_cmb
            // 
            magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            magacin_cmb.Enabled = false;
            magacin_cmb.FormattingEnabled = true;
            magacin_cmb.Location = new System.Drawing.Point(70, 3);
            magacin_cmb.Margin = new System.Windows.Forms.Padding(5);
            magacin_cmb.Name = "magacin_cmb";
            magacin_cmb.Size = new System.Drawing.Size(353, 23);
            magacin_cmb.TabIndex = 27;
            magacin_cmb.SelectedIndexChanged += magacin_cmb_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(5, 8);
            label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(53, 15);
            label3.TabIndex = 26;
            label3.Text = "Magacin";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(cene_dgv);
            groupBox1.Location = new System.Drawing.Point(761, 32);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(514, 152);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Cene Na Danasnji Dan";
            // 
            // cene_dgv
            // 
            cene_dgv.AllowUserToAddRows = false;
            cene_dgv.AllowUserToDeleteRows = false;
            cene_dgv.AllowUserToResizeRows = false;
            cene_dgv.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cene_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            cene_dgv.Location = new System.Drawing.Point(7, 22);
            cene_dgv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cene_dgv.Name = "cene_dgv";
            cene_dgv.ReadOnly = true;
            cene_dgv.RowHeadersVisible = false;
            cene_dgv.RowHeadersWidth = 51;
            cene_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            cene_dgv.Size = new System.Drawing.Size(500, 123);
            cene_dgv.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.SystemColors.Control;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.ForeColor = System.Drawing.Color.Red;
            label2.Location = new System.Drawing.Point(10, 163);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(278, 13);
            label2.TabIndex = 4;
            label2.Text = "Podaci obuhvataju trenutnu i prethodnu godinu!";
            // 
            // filter_gb
            // 
            filter_gb.Controls.Add(button1);
            filter_gb.Controls.Add(samoNabavka_rb);
            filter_gb.Controls.Add(sviDokumenti_rb);
            filter_gb.Location = new System.Drawing.Point(475, 32);
            filter_gb.Name = "filter_gb";
            filter_gb.Size = new System.Drawing.Size(279, 144);
            filter_gb.TabIndex = 5;
            filter_gb.TabStop = false;
            filter_gb.Text = "Filter";
            // 
            // samoNabavka_rb
            // 
            samoNabavka_rb.AutoSize = true;
            samoNabavka_rb.Location = new System.Drawing.Point(6, 47);
            samoNabavka_rb.Name = "samoNabavka_rb";
            samoNabavka_rb.Size = new System.Drawing.Size(102, 19);
            samoNabavka_rb.TabIndex = 1;
            samoNabavka_rb.Text = "Samo nabavka";
            samoNabavka_rb.UseVisualStyleBackColor = true;
            // 
            // sviDokumenti_rb
            // 
            sviDokumenti_rb.AutoSize = true;
            sviDokumenti_rb.Checked = true;
            sviDokumenti_rb.Location = new System.Drawing.Point(6, 22);
            sviDokumenti_rb.Name = "sviDokumenti_rb";
            sviDokumenti_rb.Size = new System.Drawing.Size(102, 19);
            sviDokumenti_rb.TabIndex = 0;
            sviDokumenti_rb.TabStop = true;
            sviDokumenti_rb.Text = "Svi Dokumenti";
            sviDokumenti_rb.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(198, 18);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Uvek gore";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // _7_fm_Komercijalno_Roba_Kartica
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1289, 721);
            Controls.Add(filter_gb);
            Controls.Add(label2);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Controls.Add(dataGridView1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(1305, 760);
            Name = "_7_fm_Komercijalno_Roba_Kartica";
            Text = "_7_fm_TDPopis_Kartica";
            Load += _7_fm_TDPopis_Kartica_Load;
            Shown += _7_fm_Komercijalno_Roba_Kartica_Shown;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cene_dgv).EndInit();
            filter_gb.ResumeLayout(false);
            filter_gb.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem podesavanjaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem racunajVazecuNabavnuCenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boldujDokument36ToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox partner_cmb;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem prikaziKomentarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prikaziInterniKomentarToolStripMenuItem;
        private System.Windows.Forms.DataGridView cene_dgv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox filter_gb;
        private System.Windows.Forms.RadioButton samoNabavka_rb;
        private System.Windows.Forms.RadioButton sviDokumenti_rb;
        private System.Windows.Forms.Button button1;
    }
}