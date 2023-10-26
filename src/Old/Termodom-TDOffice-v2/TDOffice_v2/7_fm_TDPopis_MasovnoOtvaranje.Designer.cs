
namespace TDOffice_v2
{
    partial class _7_fm_TDPopis_MasovnoOtvaranje
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
            this.btn_KreirajPopis = new System.Windows.Forms.Button();
            this.cmb_Magacin = new System.Windows.Forms.ComboBox();
            this.gb_Kriterijum = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.gb_Kriterijum.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gb_Kriterijum);
            this.panel1.Controls.Add(this.cmb_Magacin);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(623, 181);
            this.panel1.TabIndex = 0;
            // 
            // btn_KreirajPopis
            // 
            this.btn_KreirajPopis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_KreirajPopis.Location = new System.Drawing.Point(535, 207);
            this.btn_KreirajPopis.Name = "btn_KreirajPopis";
            this.btn_KreirajPopis.Size = new System.Drawing.Size(96, 23);
            this.btn_KreirajPopis.TabIndex = 1;
            this.btn_KreirajPopis.Text = "Kreiraj";
            this.btn_KreirajPopis.UseVisualStyleBackColor = true;
            // 
            // cmb_Magacin
            // 
            this.cmb_Magacin.FormattingEnabled = true;
            this.cmb_Magacin.Location = new System.Drawing.Point(14, 21);
            this.cmb_Magacin.Name = "cmb_Magacin";
            this.cmb_Magacin.Size = new System.Drawing.Size(215, 21);
            this.cmb_Magacin.TabIndex = 0;
            // 
            // gb_Kriterijum
            // 
            this.gb_Kriterijum.Controls.Add(this.radioButton2);
            this.gb_Kriterijum.Controls.Add(this.radioButton1);
            this.gb_Kriterijum.Location = new System.Drawing.Point(14, 67);
            this.gb_Kriterijum.Name = "gb_Kriterijum";
            this.gb_Kriterijum.Size = new System.Drawing.Size(215, 100);
            this.gb_Kriterijum.TabIndex = 1;
            this.gb_Kriterijum.TabStop = false;
            this.gb_Kriterijum.Text = "Uslov kreiranja popisa";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 32);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(85, 17);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 64);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(85, 17);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // _7_fm_TDPopis_MasovnoOtvaranje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 242);
            this.Controls.Add(this.btn_KreirajPopis);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(659, 260);
            this.Name = "_7_fm_TDPopis_MasovnoOtvaranje";
            this.Text = "_7_fm_TDPopis_MasovnoOtvaranje";
            this.panel1.ResumeLayout(false);
            this.gb_Kriterijum.ResumeLayout(false);
            this.gb_Kriterijum.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_KreirajPopis;
        private System.Windows.Forms.GroupBox gb_Kriterijum;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox cmb_Magacin;
    }
}