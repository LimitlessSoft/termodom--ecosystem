
namespace TDOffice_v2
{
    partial class Input_CheckedListBox
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.potvrdi_btn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decekirajSveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 12);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(776, 394);
            this.checkedListBox1.TabIndex = 0;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            // 
            // potvrdi_btn
            // 
            this.potvrdi_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.potvrdi_btn.Location = new System.Drawing.Point(713, 415);
            this.potvrdi_btn.Name = "potvrdi_btn";
            this.potvrdi_btn.Size = new System.Drawing.Size(75, 23);
            this.potvrdi_btn.TabIndex = 1;
            this.potvrdi_btn.Text = "Potvrdi";
            this.potvrdi_btn.UseVisualStyleBackColor = true;
            this.potvrdi_btn.Click += new System.EventHandler(this.potvrdi_btn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cekirajSveToolStripMenuItem,
            this.decekirajSveToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(168, 52);
            // 
            // cekirajSveToolStripMenuItem
            // 
            this.cekirajSveToolStripMenuItem.Name = "cekirajSveToolStripMenuItem";
            this.cekirajSveToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.cekirajSveToolStripMenuItem.Text = "Cekiraj Sve";
            this.cekirajSveToolStripMenuItem.Click += new System.EventHandler(this.cekirajSveToolStripMenuItem_Click);
            // 
            // decekirajSveToolStripMenuItem
            // 
            this.decekirajSveToolStripMenuItem.Name = "decekirajSveToolStripMenuItem";
            this.decekirajSveToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.decekirajSveToolStripMenuItem.Text = "Decekiraj Sve";
            this.decekirajSveToolStripMenuItem.Click += new System.EventHandler(this.decekirajSveToolStripMenuItem_Click);
            // 
            // Input_CheckedListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.potvrdi_btn);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "Input_CheckedListBox";
            this.Text = "Input_CheckedListBox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Input_CheckedListBox_FormClosing);
            this.Load += new System.EventHandler(this.Input_CheckedListBox_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button potvrdi_btn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cekirajSveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decekirajSveToolStripMenuItem;
    }
}