
namespace TDOffice_v2
{
    partial class fm_PravoPristupaModulu_Zatrazi
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
            this.btn_ZatraziPravo = new System.Windows.Forms.Button();
            this.ipakMiNeTreba_btn = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(94, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(369, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nemate prava pristupa modulu";
            // 
            // btn_ZatraziPravo
            // 
            this.btn_ZatraziPravo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ZatraziPravo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ZatraziPravo.Location = new System.Drawing.Point(3, 3);
            this.btn_ZatraziPravo.Name = "btn_ZatraziPravo";
            this.btn_ZatraziPravo.Size = new System.Drawing.Size(264, 125);
            this.btn_ZatraziPravo.TabIndex = 1;
            this.btn_ZatraziPravo.Text = "Zatrazi  ovo pravo";
            this.btn_ZatraziPravo.UseVisualStyleBackColor = true;
            this.btn_ZatraziPravo.Click += new System.EventHandler(this.btn_ZatraziPravo_Click);
            // 
            // ipakMiNeTreba_btn
            // 
            this.ipakMiNeTreba_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ipakMiNeTreba_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipakMiNeTreba_btn.Location = new System.Drawing.Point(3, 3);
            this.ipakMiNeTreba_btn.Name = "ipakMiNeTreba_btn";
            this.ipakMiNeTreba_btn.Size = new System.Drawing.Size(260, 125);
            this.ipakMiNeTreba_btn.TabIndex = 2;
            this.ipakMiNeTreba_btn.Text = "Ipak mi ne treba";
            this.ipakMiNeTreba_btn.UseVisualStyleBackColor = true;
            this.ipakMiNeTreba_btn.Click += new System.EventHandler(this.ipakMiNeTreba_btn_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 64);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btn_ZatraziPravo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ipakMiNeTreba_btn);
            this.splitContainer1.Size = new System.Drawing.Size(540, 131);
            this.splitContainer1.SplitterDistance = 270;
            this.splitContainer1.TabIndex = 3;
            // 
            // fm_PravoPristupaModulu_Zatrazi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 207);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "fm_PravoPristupaModulu_Zatrazi";
            this.Text = "Zatrazi Pravo Pristupa Modulu";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_ZatraziPravo;
        private System.Windows.Forms.Button ipakMiNeTreba_btn;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}