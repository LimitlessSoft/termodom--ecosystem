
namespace TDOffice_v2
{
    partial class _7_fm_TDPopis_Choice
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
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.vreme_cmb = new System.Windows.Forms.ComboBox();
            this.tip_cmb = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(14, 14);
            this.magacin_cmb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(236, 23);
            this.magacin_cmb.TabIndex = 0;
            // 
            // vreme_cmb
            // 
            this.vreme_cmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vreme_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vreme_cmb.FormattingEnabled = true;
            this.vreme_cmb.Items.AddRange(new object[] {
            "<Izaberite vreme popisa>",
            "Jucerasnji",
            "Nedeljni"});
            this.vreme_cmb.Location = new System.Drawing.Point(14, 45);
            this.vreme_cmb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.vreme_cmb.Name = "vreme_cmb";
            this.vreme_cmb.Size = new System.Drawing.Size(236, 23);
            this.vreme_cmb.TabIndex = 1;
            // 
            // tip_cmb
            // 
            this.tip_cmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tip_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tip_cmb.FormattingEnabled = true;
            this.tip_cmb.Items.AddRange(new object[] {
            "<Izaberi tip popisa>",
            "Za Nabavku",
            "Vanredni"});
            this.tip_cmb.Location = new System.Drawing.Point(14, 76);
            this.tip_cmb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tip_cmb.Name = "tip_cmb";
            this.tip_cmb.Size = new System.Drawing.Size(236, 23);
            this.tip_cmb.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(14, 106);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(237, 27);
            this.button1.TabIndex = 3;
            this.button1.Text = "Kreiraj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _7_fm_TDPopis_Choice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 147);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tip_cmb);
            this.Controls.Add(this.vreme_cmb);
            this.Controls.Add(this.magacin_cmb);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximumSize = new System.Drawing.Size(281, 186);
            this.MinimumSize = new System.Drawing.Size(281, 186);
            this.Name = "_7_fm_TDPopis_Choice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TDOffice Popis- Novi";
            this.Load += new System.EventHandler(this._7_fm_TDPopis_Choice_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.ComboBox vreme_cmb;
        private System.Windows.Forms.ComboBox tip_cmb;
        private System.Windows.Forms.Button button1;
    }
}