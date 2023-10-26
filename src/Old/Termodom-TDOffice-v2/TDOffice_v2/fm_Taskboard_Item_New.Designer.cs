
namespace TDOffice_v2
{
    partial class fm_Taskboard_Item_New
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
            this.naslov_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.text_rtb = new System.Windows.Forms.RichTextBox();
            this.kreiraj_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // naslov_txt
            // 
            this.naslov_txt.Location = new System.Drawing.Point(61, 12);
            this.naslov_txt.Name = "naslov_txt";
            this.naslov_txt.Size = new System.Drawing.Size(399, 20);
            this.naslov_txt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Naslov:";
            // 
            // text_rtb
            // 
            this.text_rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text_rtb.Location = new System.Drawing.Point(12, 38);
            this.text_rtb.Name = "text_rtb";
            this.text_rtb.Size = new System.Drawing.Size(776, 371);
            this.text_rtb.TabIndex = 2;
            this.text_rtb.Text = "";
            // 
            // kreiraj_btn
            // 
            this.kreiraj_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kreiraj_btn.Location = new System.Drawing.Point(680, 415);
            this.kreiraj_btn.Name = "kreiraj_btn";
            this.kreiraj_btn.Size = new System.Drawing.Size(108, 23);
            this.kreiraj_btn.TabIndex = 3;
            this.kreiraj_btn.Text = "Kreiraj";
            this.kreiraj_btn.UseVisualStyleBackColor = true;
            this.kreiraj_btn.Click += new System.EventHandler(this.kreiraj_btn_Click);
            // 
            // fm_Taskboard_Item_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.kreiraj_btn);
            this.Controls.Add(this.text_rtb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.naslov_txt);
            this.Name = "fm_Taskboard_Item_New";
            this.Text = "fm_Taskboard_Item_New";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox naslov_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox text_rtb;
        private System.Windows.Forms.Button kreiraj_btn;
    }
}