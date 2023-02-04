
namespace TDOffice_v2
{
    partial class fm_PravoPristupaModulu_OdobriZabrani
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
            this.btn_OdobriPravo = new System.Windows.Forms.Button();
            this.btn_ZauvekZabraniPravo = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.odustani_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_OdobriPravo
            // 
            this.btn_OdobriPravo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OdobriPravo.BackColor = System.Drawing.Color.Lime;
            this.btn_OdobriPravo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OdobriPravo.Location = new System.Drawing.Point(3, 3);
            this.btn_OdobriPravo.Name = "btn_OdobriPravo";
            this.btn_OdobriPravo.Size = new System.Drawing.Size(337, 111);
            this.btn_OdobriPravo.TabIndex = 0;
            this.btn_OdobriPravo.Text = "Odobri pravo";
            this.btn_OdobriPravo.UseVisualStyleBackColor = false;
            this.btn_OdobriPravo.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_ZauvekZabraniPravo
            // 
            this.btn_ZauvekZabraniPravo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ZauvekZabraniPravo.BackColor = System.Drawing.Color.Red;
            this.btn_ZauvekZabraniPravo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ZauvekZabraniPravo.Location = new System.Drawing.Point(3, 3);
            this.btn_ZauvekZabraniPravo.Name = "btn_ZauvekZabraniPravo";
            this.btn_ZauvekZabraniPravo.Size = new System.Drawing.Size(334, 111);
            this.btn_ZauvekZabraniPravo.TabIndex = 1;
            this.btn_ZauvekZabraniPravo.Text = "Zauvek Zabrani";
            this.btn_ZauvekZabraniPravo.UseVisualStyleBackColor = false;
            this.btn_ZauvekZabraniPravo.Click += new System.EventHandler(this.button2_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btn_OdobriPravo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btn_ZauvekZabraniPravo);
            this.splitContainer1.Size = new System.Drawing.Size(687, 117);
            this.splitContainer1.SplitterDistance = 343;
            this.splitContainer1.TabIndex = 2;
            // 
            // odustani_btn
            // 
            this.odustani_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.odustani_btn.BackColor = System.Drawing.Color.Gray;
            this.odustani_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.odustani_btn.Location = new System.Drawing.Point(12, 132);
            this.odustani_btn.Name = "odustani_btn";
            this.odustani_btn.Size = new System.Drawing.Size(687, 64);
            this.odustani_btn.TabIndex = 1;
            this.odustani_btn.Text = "Odustani";
            this.odustani_btn.UseVisualStyleBackColor = false;
            this.odustani_btn.Click += new System.EventHandler(this.odustani_btn_Click);
            // 
            // fm_PravoPristupaModulu_OdobriZabrani
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 205);
            this.Controls.Add(this.odustani_btn);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "fm_PravoPristupaModulu_OdobriZabrani";
            this.Text = "Upravljaj pravom";
            this.Load += new System.EventHandler(this.fm_PravoPristupaModulu_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_OdobriPravo;
        private System.Windows.Forms.Button btn_ZauvekZabraniPravo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button odustani_btn;
    }
}