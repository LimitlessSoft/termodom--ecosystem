
namespace TDOffice_v2
{
    partial class fm_Komercijalno_Dokument_Kloniraj_Index
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Kloniraj = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmb_DestinacionaBaza = new System.Windows.Forms.ComboBox();
            this.cmb_IzvornaBaza = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gb_DokumentZaKloniranje = new System.Windows.Forms.GroupBox();
            this.txt_BrDok = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_VrstaDokumenta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gb_DokumentZaKloniranje.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btn_Kloniraj);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.gb_DokumentZaKloniranje);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 261);
            this.panel1.TabIndex = 0;
            // 
            // btn_Kloniraj
            // 
            this.btn_Kloniraj.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Kloniraj.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Kloniraj.Location = new System.Drawing.Point(6, 125);
            this.btn_Kloniraj.Name = "btn_Kloniraj";
            this.btn_Kloniraj.Size = new System.Drawing.Size(767, 133);
            this.btn_Kloniraj.TabIndex = 2;
            this.btn_Kloniraj.Text = "Kloniraj";
            this.btn_Kloniraj.UseVisualStyleBackColor = true;
            this.btn_Kloniraj.Click += new System.EventHandler(this.btn_Kloniraj_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmb_DestinacionaBaza);
            this.groupBox1.Controls.Add(this.cmb_IzvornaBaza);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(354, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 109);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Izbor baza";
            // 
            // cmb_DestinacionaBaza
            // 
            this.cmb_DestinacionaBaza.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_DestinacionaBaza.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_DestinacionaBaza.FormattingEnabled = true;
            this.cmb_DestinacionaBaza.Location = new System.Drawing.Point(264, 54);
            this.cmb_DestinacionaBaza.Name = "cmb_DestinacionaBaza";
            this.cmb_DestinacionaBaza.Size = new System.Drawing.Size(149, 33);
            this.cmb_DestinacionaBaza.TabIndex = 17;
            // 
            // cmb_IzvornaBaza
            // 
            this.cmb_IzvornaBaza.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_IzvornaBaza.FormattingEnabled = true;
            this.cmb_IzvornaBaza.Location = new System.Drawing.Point(6, 54);
            this.cmb_IzvornaBaza.Name = "cmb_IzvornaBaza";
            this.cmb_IzvornaBaza.Size = new System.Drawing.Size(149, 33);
            this.cmb_IzvornaBaza.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(240, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "Destinaciona baza";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Izvorna baza";
            // 
            // gb_DokumentZaKloniranje
            // 
            this.gb_DokumentZaKloniranje.Controls.Add(this.txt_BrDok);
            this.gb_DokumentZaKloniranje.Controls.Add(this.label2);
            this.gb_DokumentZaKloniranje.Controls.Add(this.txt_VrstaDokumenta);
            this.gb_DokumentZaKloniranje.Controls.Add(this.label1);
            this.gb_DokumentZaKloniranje.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_DokumentZaKloniranje.Location = new System.Drawing.Point(6, 10);
            this.gb_DokumentZaKloniranje.Name = "gb_DokumentZaKloniranje";
            this.gb_DokumentZaKloniranje.Size = new System.Drawing.Size(329, 109);
            this.gb_DokumentZaKloniranje.TabIndex = 0;
            this.gb_DokumentZaKloniranje.TabStop = false;
            this.gb_DokumentZaKloniranje.Text = "Dokument za kloniranje";
            // 
            // txt_BrDok
            // 
            this.txt_BrDok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_BrDok.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_BrDok.Location = new System.Drawing.Point(223, 65);
            this.txt_BrDok.Name = "txt_BrDok";
            this.txt_BrDok.Size = new System.Drawing.Size(100, 30);
            this.txt_BrDok.TabIndex = 2;
            this.txt_BrDok.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(76, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Br. Dokumenta";
            // 
            // txt_VrstaDokumenta
            // 
            this.txt_VrstaDokumenta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_VrstaDokumenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_VrstaDokumenta.Location = new System.Drawing.Point(223, 29);
            this.txt_VrstaDokumenta.Name = "txt_VrstaDokumenta";
            this.txt_VrstaDokumenta.Size = new System.Drawing.Size(100, 30);
            this.txt_VrstaDokumenta.TabIndex = 1;
            this.txt_VrstaDokumenta.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(57, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Vrsta dokumenta";
            // 
            // fm_Komercijalno_Dokument_Kloniraj_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 285);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(826, 332);
            this.Name = "fm_Komercijalno_Dokument_Kloniraj_Index";
            this.Text = "fm_Komercijalno_Dokument_Kloniraj_Index";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb_DokumentZaKloniranje.ResumeLayout(false);
            this.gb_DokumentZaKloniranje.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gb_DokumentZaKloniranje;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_BrDok;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_VrstaDokumenta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_DestinacionaBaza;
        private System.Windows.Forms.ComboBox cmb_IzvornaBaza;
        private System.Windows.Forms.Button btn_Kloniraj;
    }
}