
namespace TDOffice_v2
{
    partial class _1301_fm_Poruka_Nova
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bccPrimalac_clb = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SelektujDeselektujSvePrimaoce = new System.Windows.Forms.ToolStripMenuItem();
            this.primalac_clb = new System.Windows.Forms.CheckedListBox();
            this.vidljivostPoruke_cmb = new System.Windows.Forms.ComboBox();
            this.posalji_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.naslov_txt = new System.Windows.Forms.TextBox();
            this.tekst_rtb = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.bccPrimalac_clb);
            this.splitContainer1.Panel1.Controls.Add(this.primalac_clb);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.vidljivostPoruke_cmb);
            this.splitContainer1.Panel2.Controls.Add(this.posalji_btn);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.naslov_txt);
            this.splitContainer1.Panel2.Controls.Add(this.tekst_rtb);
            this.splitContainer1.Size = new System.Drawing.Size(1012, 433);
            this.splitContainer1.SplitterDistance = 165;
            this.splitContainer1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(505, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "BCC Primalac";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Primalac";
            // 
            // bccPrimalac_clb
            // 
            this.bccPrimalac_clb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bccPrimalac_clb.CheckOnClick = true;
            this.bccPrimalac_clb.ContextMenuStrip = this.contextMenuStrip1;
            this.bccPrimalac_clb.FormattingEnabled = true;
            this.bccPrimalac_clb.Location = new System.Drawing.Point(505, 29);
            this.bccPrimalac_clb.MultiColumn = true;
            this.bccPrimalac_clb.Name = "bccPrimalac_clb";
            this.bccPrimalac_clb.Size = new System.Drawing.Size(494, 124);
            this.bccPrimalac_clb.TabIndex = 1;
            this.bccPrimalac_clb.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.bccPrimalac_clb_ItemCheck);
            this.bccPrimalac_clb.SelectedIndexChanged += new System.EventHandler(this.bccPrimalac_clb_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelektujDeselektujSvePrimaoce});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(298, 28);
            // 
            // SelektujDeselektujSvePrimaoce
            // 
            this.SelektujDeselektujSvePrimaoce.Name = "SelektujDeselektujSvePrimaoce";
            this.SelektujDeselektujSvePrimaoce.Size = new System.Drawing.Size(297, 24);
            this.SelektujDeselektujSvePrimaoce.Text = "Selektu/Deselektujj sve primaoce";
            this.SelektujDeselektujSvePrimaoce.Click += new System.EventHandler(this.SelektujDeselektujSvePrimaoce_Click);
            // 
            // primalac_clb
            // 
            this.primalac_clb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.primalac_clb.CheckOnClick = true;
            this.primalac_clb.ContextMenuStrip = this.contextMenuStrip1;
            this.primalac_clb.FormattingEnabled = true;
            this.primalac_clb.Location = new System.Drawing.Point(11, 29);
            this.primalac_clb.MultiColumn = true;
            this.primalac_clb.Name = "primalac_clb";
            this.primalac_clb.Size = new System.Drawing.Size(485, 124);
            this.primalac_clb.TabIndex = 0;
            this.primalac_clb.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.primalac_clb_ItemCheck);
            this.primalac_clb.SelectedIndexChanged += new System.EventHandler(this.primalac_clb_SelectedIndexChanged);
            // 
            // vidljivostPoruke_cmb
            // 
            this.vidljivostPoruke_cmb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.vidljivostPoruke_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vidljivostPoruke_cmb.FormattingEnabled = true;
            this.vidljivostPoruke_cmb.Location = new System.Drawing.Point(555, 228);
            this.vidljivostPoruke_cmb.Name = "vidljivostPoruke_cmb";
            this.vidljivostPoruke_cmb.Size = new System.Drawing.Size(194, 21);
            this.vidljivostPoruke_cmb.TabIndex = 4;
            // 
            // posalji_btn
            // 
            this.posalji_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.posalji_btn.Location = new System.Drawing.Point(832, 228);
            this.posalji_btn.Name = "posalji_btn";
            this.posalji_btn.Size = new System.Drawing.Size(167, 23);
            this.posalji_btn.TabIndex = 3;
            this.posalji_btn.Text = "Posalji";
            this.posalji_btn.UseVisualStyleBackColor = true;
            this.posalji_btn.Click += new System.EventHandler(this.posalji_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Naslov";
            // 
            // naslov_txt
            // 
            this.naslov_txt.Location = new System.Drawing.Point(57, 3);
            this.naslov_txt.Name = "naslov_txt";
            this.naslov_txt.Size = new System.Drawing.Size(360, 20);
            this.naslov_txt.TabIndex = 1;
            // 
            // tekst_rtb
            // 
            this.tekst_rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tekst_rtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tekst_rtb.Location = new System.Drawing.Point(11, 29);
            this.tekst_rtb.Name = "tekst_rtb";
            this.tekst_rtb.Size = new System.Drawing.Size(988, 193);
            this.tekst_rtb.TabIndex = 0;
            this.tekst_rtb.Text = "";
            // 
            // _1301_fm_Poruka_Nova
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 433);
            this.Controls.Add(this.splitContainer1);
            this.Name = "_1301_fm_Poruka_Nova";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nova Poruka";
            this.Load += new System.EventHandler(this._1301_fm_Poruka_Nova_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox primalac_clb;
        private System.Windows.Forms.Button posalji_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox naslov_txt;
        private System.Windows.Forms.RichTextBox tekst_rtb;
        private System.Windows.Forms.ComboBox vidljivostPoruke_cmb;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem SelektujDeselektujSvePrimaoce;
        private System.Windows.Forms.CheckedListBox bccPrimalac_clb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}