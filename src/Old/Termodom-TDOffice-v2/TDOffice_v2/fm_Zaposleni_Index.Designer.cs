namespace TDOffice_v2
{
    partial class fm_Zaposleni_Index
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.firma_txt = new System.Windows.Forms.TextBox();
            this.btn_Sacuvaj = new System.Windows.Forms.Button();
            this.txt_Prezime = new System.Windows.Forms.TextBox();
            this.txt_Ime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.UkloniUgovor = new System.Windows.Forms.ToolStripMenuItem();
            this.nova_btn = new System.Windows.Forms.Button();
            this.btn_ObrisiZaposlenog = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btn_ObrisiZaposlenog);
            this.panel1.Controls.Add(this.firma_txt);
            this.panel1.Controls.Add(this.btn_Sacuvaj);
            this.panel1.Controls.Add(this.txt_Prezime);
            this.panel1.Controls.Add(this.txt_Ime);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 162);
            this.panel1.TabIndex = 0;
            // 
            // firma_txt
            // 
            this.firma_txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.firma_txt.Location = new System.Drawing.Point(50, 55);
            this.firma_txt.Name = "firma_txt";
            this.firma_txt.ReadOnly = true;
            this.firma_txt.Size = new System.Drawing.Size(173, 20);
            this.firma_txt.TabIndex = 7;
            // 
            // btn_Sacuvaj
            // 
            this.btn_Sacuvaj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Sacuvaj.Location = new System.Drawing.Point(198, 82);
            this.btn_Sacuvaj.Name = "btn_Sacuvaj";
            this.btn_Sacuvaj.Size = new System.Drawing.Size(145, 23);
            this.btn_Sacuvaj.TabIndex = 6;
            this.btn_Sacuvaj.Text = "Sacuvaj";
            this.btn_Sacuvaj.UseVisualStyleBackColor = true;
            this.btn_Sacuvaj.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_Prezime
            // 
            this.txt_Prezime.Location = new System.Drawing.Point(61, 29);
            this.txt_Prezime.Name = "txt_Prezime";
            this.txt_Prezime.Size = new System.Drawing.Size(173, 20);
            this.txt_Prezime.TabIndex = 4;
            this.txt_Prezime.TextChanged += new System.EventHandler(this.txt_Prezime_TextChanged);
            // 
            // txt_Ime
            // 
            this.txt_Ime.Location = new System.Drawing.Point(41, 3);
            this.txt_Ime.Name = "txt_Ime";
            this.txt_Ime.Size = new System.Drawing.Size(173, 20);
            this.txt_Ime.TabIndex = 3;
            this.txt_Ime.TextChanged += new System.EventHandler(this.txt_Ime_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Firma:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Prezime:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ime:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.nova_btn);
            this.panel2.Location = new System.Drawing.Point(12, 180);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(349, 453);
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
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(3, 44);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(339, 398);
            this.dataGridView1.TabIndex = 26;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UkloniUgovor});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(172, 28);
            // 
            // UkloniUgovor
            // 
            this.UkloniUgovor.Name = "UkloniUgovor";
            this.UkloniUgovor.Size = new System.Drawing.Size(171, 24);
            this.UkloniUgovor.Text = "Ukloni ugovor";
            this.UkloniUgovor.Click += new System.EventHandler(this.UkloniUgovor_Click);
            // 
            // nova_btn
            // 
            this.nova_btn.BackgroundImage = global::TDOffice_v2.Properties.Resources.new_icon;
            this.nova_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.nova_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nova_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.nova_btn.Location = new System.Drawing.Point(14, 10);
            this.nova_btn.Name = "nova_btn";
            this.nova_btn.Size = new System.Drawing.Size(27, 29);
            this.nova_btn.TabIndex = 25;
            this.nova_btn.UseVisualStyleBackColor = true;
            this.nova_btn.Click += new System.EventHandler(this.nova_btn_Click);
            // 
            // btn_ObrisiZaposlenog
            // 
            this.btn_ObrisiZaposlenog.Location = new System.Drawing.Point(198, 121);
            this.btn_ObrisiZaposlenog.Name = "btn_ObrisiZaposlenog";
            this.btn_ObrisiZaposlenog.Size = new System.Drawing.Size(145, 23);
            this.btn_ObrisiZaposlenog.TabIndex = 8;
            this.btn_ObrisiZaposlenog.Text = "Obrisi zaposlenog";
            this.btn_ObrisiZaposlenog.UseVisualStyleBackColor = true;
            this.btn_ObrisiZaposlenog.Click += new System.EventHandler(this.btn_ObrisiZaposlenog_Click);
            // 
            // fm_Zaposleni_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 645);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fm_Zaposleni_Index";
            this.Text = "Zaposleni";
            this.Load += new System.EventHandler(this.fm_Zaposleni_Index_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_Prezime;
        private System.Windows.Forms.TextBox txt_Ime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Sacuvaj;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button nova_btn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem UkloniUgovor;
        private System.Windows.Forms.TextBox firma_txt;
        private System.Windows.Forms.Button btn_ObrisiZaposlenog;
    }
}