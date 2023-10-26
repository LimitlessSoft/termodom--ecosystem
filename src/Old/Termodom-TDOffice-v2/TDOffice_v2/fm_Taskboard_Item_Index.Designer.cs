
namespace TDOffice_v2
{
    partial class fm_Taskboard_Item_Index
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
            this.text_rtb = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.naslov_txt = new System.Windows.Forms.TextBox();
            this.itemStatus_cmb = new System.Windows.Forms.ComboBox();
            this.btn_Sacuvaj = new System.Windows.Forms.Button();
            this.komentari_gb = new System.Windows.Forms.GroupBox();
            this.btn_Komentarisi = new System.Windows.Forms.Button();
            this.tb_NoviKomentar = new System.Windows.Forms.TextBox();
            this.rtb_Komentari = new System.Windows.Forms.RichTextBox();
            this.komentari_gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // text_rtb
            // 
            this.text_rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text_rtb.Location = new System.Drawing.Point(12, 38);
            this.text_rtb.Name = "text_rtb";
            this.text_rtb.Size = new System.Drawing.Size(776, 234);
            this.text_rtb.TabIndex = 6;
            this.text_rtb.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Naslov:";
            // 
            // naslov_txt
            // 
            this.naslov_txt.Location = new System.Drawing.Point(61, 12);
            this.naslov_txt.Name = "naslov_txt";
            this.naslov_txt.Size = new System.Drawing.Size(399, 20);
            this.naslov_txt.TabIndex = 4;
            // 
            // itemStatus_cmb
            // 
            this.itemStatus_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.itemStatus_cmb.FormattingEnabled = true;
            this.itemStatus_cmb.Location = new System.Drawing.Point(592, 12);
            this.itemStatus_cmb.Name = "itemStatus_cmb";
            this.itemStatus_cmb.Size = new System.Drawing.Size(196, 21);
            this.itemStatus_cmb.TabIndex = 8;
            this.itemStatus_cmb.SelectedIndexChanged += new System.EventHandler(this.itemStatus_cmb_SelectedIndexChanged);
            // 
            // btn_Sacuvaj
            // 
            this.btn_Sacuvaj.BackgroundImage = global::TDOffice_v2.Properties.Resources.save_icon;
            this.btn_Sacuvaj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Sacuvaj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Sacuvaj.Location = new System.Drawing.Point(558, 10);
            this.btn_Sacuvaj.Name = "btn_Sacuvaj";
            this.btn_Sacuvaj.Size = new System.Drawing.Size(28, 25);
            this.btn_Sacuvaj.TabIndex = 39;
            this.btn_Sacuvaj.UseVisualStyleBackColor = true;
            this.btn_Sacuvaj.Click += new System.EventHandler(this.btn_Sacuvaj_Click);
            // 
            // komentari_gb
            // 
            this.komentari_gb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.komentari_gb.Controls.Add(this.btn_Komentarisi);
            this.komentari_gb.Controls.Add(this.tb_NoviKomentar);
            this.komentari_gb.Controls.Add(this.rtb_Komentari);
            this.komentari_gb.Location = new System.Drawing.Point(12, 278);
            this.komentari_gb.Name = "komentari_gb";
            this.komentari_gb.Size = new System.Drawing.Size(776, 250);
            this.komentari_gb.TabIndex = 40;
            this.komentari_gb.TabStop = false;
            this.komentari_gb.Text = "Komentari";
            // 
            // btn_Komentarisi
            // 
            this.btn_Komentarisi.Location = new System.Drawing.Point(592, 217);
            this.btn_Komentarisi.Name = "btn_Komentarisi";
            this.btn_Komentarisi.Size = new System.Drawing.Size(178, 23);
            this.btn_Komentarisi.TabIndex = 2;
            this.btn_Komentarisi.Text = "Komentarisi";
            this.btn_Komentarisi.UseVisualStyleBackColor = true;
            this.btn_Komentarisi.Click += new System.EventHandler(this.btn_Komentarisi_Click);
            // 
            // tb_NoviKomentar
            // 
            this.tb_NoviKomentar.Location = new System.Drawing.Point(16, 214);
            this.tb_NoviKomentar.Name = "tb_NoviKomentar";
            this.tb_NoviKomentar.Size = new System.Drawing.Size(558, 20);
            this.tb_NoviKomentar.TabIndex = 1;
            // 
            // rtb_Komentari
            // 
            this.rtb_Komentari.Location = new System.Drawing.Point(6, 19);
            this.rtb_Komentari.Name = "rtb_Komentari";
            this.rtb_Komentari.ReadOnly = true;
            this.rtb_Komentari.Size = new System.Drawing.Size(764, 172);
            this.rtb_Komentari.TabIndex = 0;
            this.rtb_Komentari.Text = "";
            // 
            // fm_Taskboard_Item_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 540);
            this.Controls.Add(this.komentari_gb);
            this.Controls.Add(this.btn_Sacuvaj);
            this.Controls.Add(this.itemStatus_cmb);
            this.Controls.Add(this.text_rtb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.naslov_txt);
            this.Name = "fm_Taskboard_Item_Index";
            this.Text = "fm_Tskboard_Item_Index";
            this.Load += new System.EventHandler(this.fm_Taskboard_Item_Index_Load);
            this.komentari_gb.ResumeLayout(false);
            this.komentari_gb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox text_rtb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox naslov_txt;
        private System.Windows.Forms.ComboBox itemStatus_cmb;
        private System.Windows.Forms.Button btn_Sacuvaj;
        private System.Windows.Forms.GroupBox komentari_gb;
        private System.Windows.Forms.Button btn_Komentarisi;
        private System.Windows.Forms.TextBox tb_NoviKomentar;
        private System.Windows.Forms.RichTextBox rtb_Komentari;
    }
}