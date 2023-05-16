
namespace TDOffice_v2
{
    partial class _1370_fm_NacinIzracunavanjaRabata
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
            this.rb_BezDecimala = new System.Windows.Forms.RadioButton();
            this.rb_UprosecenRabat = new System.Windows.Forms.RadioButton();
            this.rb_Standard = new System.Windows.Forms.RadioButton();
            this.rb_DatiRabat = new System.Windows.Forms.RadioButton();
            this.btn_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rb_BezDecimala
            // 
            this.rb_BezDecimala.AutoSize = true;
            this.rb_BezDecimala.Location = new System.Drawing.Point(12, 12);
            this.rb_BezDecimala.Name = "rb_BezDecimala";
            this.rb_BezDecimala.Size = new System.Drawing.Size(88, 17);
            this.rb_BezDecimala.TabIndex = 0;
            this.rb_BezDecimala.TabStop = true;
            this.rb_BezDecimala.Text = "Bez decimala";
            this.rb_BezDecimala.UseVisualStyleBackColor = true;
            // 
            // rb_UprosecenRabat
            // 
            this.rb_UprosecenRabat.AutoSize = true;
            this.rb_UprosecenRabat.Location = new System.Drawing.Point(12, 37);
            this.rb_UprosecenRabat.Name = "rb_UprosecenRabat";
            this.rb_UprosecenRabat.Size = new System.Drawing.Size(99, 17);
            this.rb_UprosecenRabat.TabIndex = 1;
            this.rb_UprosecenRabat.TabStop = true;
            this.rb_UprosecenRabat.Text = "Uproseciti rabat";
            this.rb_UprosecenRabat.UseVisualStyleBackColor = true;
            // 
            // rb_Standard
            // 
            this.rb_Standard.AutoSize = true;
            this.rb_Standard.Location = new System.Drawing.Point(12, 60);
            this.rb_Standard.Name = "rb_Standard";
            this.rb_Standard.Size = new System.Drawing.Size(68, 17);
            this.rb_Standard.TabIndex = 2;
            this.rb_Standard.TabStop = true;
            this.rb_Standard.Text = "Standard";
            this.rb_Standard.UseVisualStyleBackColor = true;
            // 
            // rb_DatiRabat
            // 
            this.rb_DatiRabat.AutoSize = true;
            this.rb_DatiRabat.Location = new System.Drawing.Point(12, 83);
            this.rb_DatiRabat.Name = "rb_DatiRabat";
            this.rb_DatiRabat.Size = new System.Drawing.Size(71, 17);
            this.rb_DatiRabat.TabIndex = 3;
            this.rb_DatiRabat.TabStop = true;
            this.rb_DatiRabat.Text = "Dati rabat";
            this.rb_DatiRabat.UseVisualStyleBackColor = true;
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(224, 115);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 4;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // _1370_fm_NacinIzracunavanjaRabata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 150);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.rb_DatiRabat);
            this.Controls.Add(this.rb_Standard);
            this.Controls.Add(this.rb_UprosecenRabat);
            this.Controls.Add(this.rb_BezDecimala);
            this.Name = "_1370_fm_NacinIzracunavanjaRabata";
            this.Text = "Nacin izracunavanja rabata";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rb_BezDecimala;
        private System.Windows.Forms.RadioButton rb_UprosecenRabat;
        private System.Windows.Forms.RadioButton rb_Standard;
        private System.Windows.Forms.RadioButton rb_DatiRabat;
        private System.Windows.Forms.Button btn_OK;
    }
}