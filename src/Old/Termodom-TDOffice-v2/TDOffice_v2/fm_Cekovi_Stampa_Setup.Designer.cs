
namespace TDOffice_v2
{
    partial class fm_Cekovi_Stampa_Setup
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
            this.prijemnoMesto_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mesto_cmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.firma_cmb = new System.Windows.Forms.ComboBox();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // prijemnoMesto_cmb
            // 
            this.prijemnoMesto_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.prijemnoMesto_cmb.FormattingEnabled = true;
            this.prijemnoMesto_cmb.Location = new System.Drawing.Point(12, 28);
            this.prijemnoMesto_cmb.Name = "prijemnoMesto_cmb";
            this.prijemnoMesto_cmb.Size = new System.Drawing.Size(230, 21);
            this.prijemnoMesto_cmb.TabIndex = 0;
            this.prijemnoMesto_cmb.SelectedIndexChanged += new System.EventHandler(this.prijemnoMesto_cmb_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Prijemno mesto:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mesto:";
            // 
            // mesto_cmb
            // 
            this.mesto_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mesto_cmb.FormattingEnabled = true;
            this.mesto_cmb.Location = new System.Drawing.Point(12, 72);
            this.mesto_cmb.Name = "mesto_cmb";
            this.mesto_cmb.Size = new System.Drawing.Size(230, 21);
            this.mesto_cmb.TabIndex = 2;
            this.mesto_cmb.SelectedIndexChanged += new System.EventHandler(this.mesto_cmb_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Firma";
            // 
            // firma_cmb
            // 
            this.firma_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.firma_cmb.FormattingEnabled = true;
            this.firma_cmb.Location = new System.Drawing.Point(12, 119);
            this.firma_cmb.Name = "firma_cmb";
            this.firma_cmb.Size = new System.Drawing.Size(230, 21);
            this.firma_cmb.TabIndex = 4;
            this.firma_cmb.SelectedIndexChanged += new System.EventHandler(this.firma_cmb_SelectedIndexChanged);
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odDatuma_dtp.Location = new System.Drawing.Point(15, 168);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.odDatuma_dtp.TabIndex = 6;
            this.odDatuma_dtp.ValueChanged += new System.EventHandler(this.odDatuma_dtp_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Datum izdavanja ceka:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(227, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Stampaj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fm_Cekovi_Stampa_Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 220);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.odDatuma_dtp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.firma_cmb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mesto_cmb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.prijemnoMesto_cmb);
            this.Name = "fm_Cekovi_Stampa_Setup";
            this.Text = "Stampa Cekova";
            this.Load += new System.EventHandler(this.fm_Cekovi_Stampa_Setup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox prijemnoMesto_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox mesto_cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox firma_cmb;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}