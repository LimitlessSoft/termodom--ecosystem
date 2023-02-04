
namespace TDOffice_v2
{
    partial class _1301_fm_Poruka_Beleska
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.sacuvaj_btn = new System.Windows.Forms.Button();
            this.zatvori_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(776, 379);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // sacuvaj_btn
            // 
            this.sacuvaj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sacuvaj_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.sacuvaj_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.sacuvaj_btn.Location = new System.Drawing.Point(298, 397);
            this.sacuvaj_btn.Name = "sacuvaj_btn";
            this.sacuvaj_btn.Size = new System.Drawing.Size(490, 41);
            this.sacuvaj_btn.TabIndex = 15;
            this.sacuvaj_btn.Text = "Sacuvaj";
            this.sacuvaj_btn.UseVisualStyleBackColor = false;
            this.sacuvaj_btn.Click += new System.EventHandler(this.sacuvaj_btn_Click);
            // 
            // zatvori_btn
            // 
            this.zatvori_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zatvori_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.zatvori_btn.Location = new System.Drawing.Point(12, 397);
            this.zatvori_btn.Name = "zatvori_btn";
            this.zatvori_btn.Size = new System.Drawing.Size(280, 41);
            this.zatvori_btn.TabIndex = 14;
            this.zatvori_btn.Text = "Zatvori";
            this.zatvori_btn.UseVisualStyleBackColor = true;
            this.zatvori_btn.Click += new System.EventHandler(this.zatvori_btn_Click);
            // 
            // _1301_fm_Poruka_Beleska
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sacuvaj_btn);
            this.Controls.Add(this.zatvori_btn);
            this.Controls.Add(this.richTextBox1);
            this.MaximumSize = new System.Drawing.Size(816, 489);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "_1301_fm_Poruka_Beleska";
            this.Text = "Beleska";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._1301_fm_Poruka_Beleska_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button sacuvaj_btn;
        private System.Windows.Forms.Button zatvori_btn;
    }
}