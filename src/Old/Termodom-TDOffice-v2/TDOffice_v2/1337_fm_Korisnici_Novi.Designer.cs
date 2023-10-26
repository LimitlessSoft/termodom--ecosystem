
namespace TDOffice_v2
{
    partial class _1337_fm_Korisnici_Novi
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
            this.kreiraj_btn = new System.Windows.Forms.Button();
            this.korisnickoIme_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sifra_txt = new System.Windows.Forms.TextBox();
            this.grad_cmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // kreiraj_btn
            // 
            this.kreiraj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kreiraj_btn.Location = new System.Drawing.Point(243, 91);
            this.kreiraj_btn.Name = "kreiraj_btn";
            this.kreiraj_btn.Size = new System.Drawing.Size(75, 23);
            this.kreiraj_btn.TabIndex = 0;
            this.kreiraj_btn.Text = "Kreiraj";
            this.kreiraj_btn.UseVisualStyleBackColor = true;
            this.kreiraj_btn.Click += new System.EventHandler(this.kreiraj_btn_Click);
            // 
            // korisnickoIme_txt
            // 
            this.korisnickoIme_txt.Location = new System.Drawing.Point(94, 12);
            this.korisnickoIme_txt.Name = "korisnickoIme_txt";
            this.korisnickoIme_txt.Size = new System.Drawing.Size(229, 20);
            this.korisnickoIme_txt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Korisnicko Ime";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sifra";
            // 
            // sifra_txt
            // 
            this.sifra_txt.Location = new System.Drawing.Point(46, 38);
            this.sifra_txt.Name = "sifra_txt";
            this.sifra_txt.Size = new System.Drawing.Size(229, 20);
            this.sifra_txt.TabIndex = 3;
            // 
            // grad_cmb
            // 
            this.grad_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.grad_cmb.FormattingEnabled = true;
            this.grad_cmb.Location = new System.Drawing.Point(48, 64);
            this.grad_cmb.Name = "grad_cmb";
            this.grad_cmb.Size = new System.Drawing.Size(270, 21);
            this.grad_cmb.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Grad";
            // 
            // _1337_fm_Korisnici_Novi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 126);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.grad_cmb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sifra_txt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.korisnickoIme_txt);
            this.Controls.Add(this.kreiraj_btn);
            this.MinimumSize = new System.Drawing.Size(346, 165);
            this.Name = "_1337_fm_Korisnici_Novi";
            this.Text = "Novi Korisnik";
            this.Load += new System.EventHandler(this._1337_fm_Korisnici_Novi_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button kreiraj_btn;
        private System.Windows.Forms.TextBox korisnickoIme_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sifra_txt;
        private System.Windows.Forms.ComboBox grad_cmb;
        private System.Windows.Forms.Label label3;
    }
}