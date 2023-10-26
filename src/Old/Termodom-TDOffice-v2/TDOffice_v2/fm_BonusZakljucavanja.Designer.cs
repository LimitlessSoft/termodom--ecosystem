namespace TDOffice_v2
{
    partial class fm_BonusZakljucavanja
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtMpRacun = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnZatraziBonuseZaklkjucavanja = new System.Windows.Forms.Button();
            this.btnPodesavanjaBonusa = new System.Windows.Forms.Button();
            this.btnZakljucajuzpomocbonusa = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Broj MP racuna:";
            // 
            // txtMpRacun
            // 
            this.txtMpRacun.Location = new System.Drawing.Point(119, 9);
            this.txtMpRacun.Name = "txtMpRacun";
            this.txtMpRacun.Size = new System.Drawing.Size(153, 22);
            this.txtMpRacun.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(322, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ostalo bonusa:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(491, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Limit bonusa:";
            // 
            // btnZatraziBonuseZaklkjucavanja
            // 
            this.btnZatraziBonuseZaklkjucavanja.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnZatraziBonuseZaklkjucavanja.Location = new System.Drawing.Point(53, 116);
            this.btnZatraziBonuseZaklkjucavanja.Name = "btnZatraziBonuseZaklkjucavanja";
            this.btnZatraziBonuseZaklkjucavanja.Size = new System.Drawing.Size(234, 23);
            this.btnZatraziBonuseZaklkjucavanja.TabIndex = 4;
            this.btnZatraziBonuseZaklkjucavanja.Text = "Zatrazi bonuse zakljucavanja";
            this.btnZatraziBonuseZaklkjucavanja.UseVisualStyleBackColor = true;
            this.btnZatraziBonuseZaklkjucavanja.Click += new System.EventHandler(this.btnZatraziBonuseZaklkjucavanja_Click);
            // 
            // btnPodesavanjaBonusa
            // 
            this.btnPodesavanjaBonusa.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPodesavanjaBonusa.Location = new System.Drawing.Point(331, 116);
            this.btnPodesavanjaBonusa.Name = "btnPodesavanjaBonusa";
            this.btnPodesavanjaBonusa.Size = new System.Drawing.Size(180, 23);
            this.btnPodesavanjaBonusa.TabIndex = 5;
            this.btnPodesavanjaBonusa.Text = "Podesavanja bonusa";
            this.btnPodesavanjaBonusa.UseVisualStyleBackColor = true;
            this.btnPodesavanjaBonusa.Click += new System.EventHandler(this.btnPodesavanjaBonusa_Click);
            // 
            // btnZakljucajuzpomocbonusa
            // 
            this.btnZakljucajuzpomocbonusa.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnZakljucajuzpomocbonusa.Location = new System.Drawing.Point(545, 116);
            this.btnZakljucajuzpomocbonusa.Name = "btnZakljucajuzpomocbonusa";
            this.btnZakljucajuzpomocbonusa.Size = new System.Drawing.Size(211, 23);
            this.btnZakljucajuzpomocbonusa.TabIndex = 6;
            this.btnZakljucajuzpomocbonusa.Text = "Zakljucaj uz pomoc bonusa";
            this.btnZakljucajuzpomocbonusa.UseVisualStyleBackColor = true;
            this.btnZakljucajuzpomocbonusa.Click += new System.EventHandler(this.btnZakljucajuzpomocbonusa_Click);
            // 
            // fm_BonusZakljucavanja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 206);
            this.Controls.Add(this.btnZakljucajuzpomocbonusa);
            this.Controls.Add(this.btnPodesavanjaBonusa);
            this.Controls.Add(this.btnZatraziBonuseZaklkjucavanja);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMpRacun);
            this.Controls.Add(this.label1);
            this.Name = "fm_BonusZakljucavanja";
            this.Text = "fm_BonusZakljucavanja";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMpRacun;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnZatraziBonuseZaklkjucavanja;
        private System.Windows.Forms.Button btnPodesavanjaBonusa;
        private System.Windows.Forms.Button btnZakljucajuzpomocbonusa;
    }
}