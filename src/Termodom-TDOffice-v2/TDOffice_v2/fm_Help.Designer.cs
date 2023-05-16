
namespace TDOffice_v2
{
    partial class fm_Help
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtb_Komentar = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rtb_InterniKomentar = new System.Windows.Forms.RichTextBox();
            this.odustaniOdCuvanjaHelpa_btn = new System.Windows.Forms.Button();
            this.sacuvajHelp_btn = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(706, 428);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtb_Komentar);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(698, 402);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Help";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rtb_Komentar
            // 
            this.rtb_Komentar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Komentar.Location = new System.Drawing.Point(3, 3);
            this.rtb_Komentar.Name = "rtb_Komentar";
            this.rtb_Komentar.Size = new System.Drawing.Size(692, 396);
            this.rtb_Komentar.TabIndex = 0;
            this.rtb_Komentar.Text = "";
            this.rtb_Komentar.TextChanged += new System.EventHandler(this.rtb_Komentar_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rtb_InterniKomentar);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(735, 402);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Admin beleska";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rtb_InterniKomentar
            // 
            this.rtb_InterniKomentar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_InterniKomentar.Location = new System.Drawing.Point(3, 3);
            this.rtb_InterniKomentar.Name = "rtb_InterniKomentar";
            this.rtb_InterniKomentar.Size = new System.Drawing.Size(729, 396);
            this.rtb_InterniKomentar.TabIndex = 1;
            this.rtb_InterniKomentar.Text = "";
            this.rtb_InterniKomentar.TextChanged += new System.EventHandler(this.rtb_InterniKomentar_TextChanged);
            // 
            // odustaniOdCuvanjaHelpa_btn
            // 
            this.odustaniOdCuvanjaHelpa_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.odustaniOdCuvanjaHelpa_btn.Location = new System.Drawing.Point(19, 446);
            this.odustaniOdCuvanjaHelpa_btn.Name = "odustaniOdCuvanjaHelpa_btn";
            this.odustaniOdCuvanjaHelpa_btn.Size = new System.Drawing.Size(115, 23);
            this.odustaniOdCuvanjaHelpa_btn.TabIndex = 10;
            this.odustaniOdCuvanjaHelpa_btn.Text = "Odbaci";
            this.odustaniOdCuvanjaHelpa_btn.UseVisualStyleBackColor = true;
            this.odustaniOdCuvanjaHelpa_btn.Visible = false;
            this.odustaniOdCuvanjaHelpa_btn.Click += new System.EventHandler(this.odustaniOdCuvanjaHelpa_btn_Click);
            // 
            // sacuvajHelp_btn
            // 
            this.sacuvajHelp_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sacuvajHelp_btn.Location = new System.Drawing.Point(160, 446);
            this.sacuvajHelp_btn.Name = "sacuvajHelp_btn";
            this.sacuvajHelp_btn.Size = new System.Drawing.Size(551, 23);
            this.sacuvajHelp_btn.TabIndex = 9;
            this.sacuvajHelp_btn.Text = "Sacuvaj";
            this.sacuvajHelp_btn.UseVisualStyleBackColor = true;
            this.sacuvajHelp_btn.Visible = false;
            this.sacuvajHelp_btn.Click += new System.EventHandler(this.sacuvajHelp_btn_Click);
            // 
            // fm_Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 471);
            this.Controls.Add(this.odustaniOdCuvanjaHelpa_btn);
            this.Controls.Add(this.sacuvajHelp_btn);
            this.Controls.Add(this.tabControl1);
            this.Name = "fm_Help";
            this.Text = "fm_Help";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fm_Help_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fm_Help_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox rtb_Komentar;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button odustaniOdCuvanjaHelpa_btn;
        private System.Windows.Forms.Button sacuvajHelp_btn;
        private System.Windows.Forms.RichTextBox rtb_InterniKomentar;
    }
}