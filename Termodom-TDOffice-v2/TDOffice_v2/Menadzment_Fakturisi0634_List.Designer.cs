
namespace TDOffice_v2
{
    partial class Menadzment_Fakturisi0634_List
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.nova_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.ucitaj_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 68);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(821, 361);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // nova_btn
            // 
            this.nova_btn.Location = new System.Drawing.Point(12, 12);
            this.nova_btn.Name = "nova_btn";
            this.nova_btn.Size = new System.Drawing.Size(75, 23);
            this.nova_btn.TabIndex = 1;
            this.nova_btn.Text = "Nova";
            this.nova_btn.UseVisualStyleBackColor = true;
            this.nova_btn.Click += new System.EventHandler(this.nova_btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Magacin";
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(71, 41);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(197, 21);
            this.magacin_cmb.TabIndex = 4;
            // 
            // ucitaj_btn
            // 
            this.ucitaj_btn.Location = new System.Drawing.Point(274, 39);
            this.ucitaj_btn.Name = "ucitaj_btn";
            this.ucitaj_btn.Size = new System.Drawing.Size(75, 23);
            this.ucitaj_btn.TabIndex = 6;
            this.ucitaj_btn.Text = "Ucitaj";
            this.ucitaj_btn.UseVisualStyleBackColor = true;
            this.ucitaj_btn.Click += new System.EventHandler(this.Ucitaj_Click);
            // 
            // Menadzment_Fakturisi0634_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 441);
            this.Controls.Add(this.ucitaj_btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.magacin_cmb);
            this.Controls.Add(this.nova_btn);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Menadzment_Fakturisi0634_List";
            this.Text = "Menadzment_Fakturisi0634_List";
            this.Load += new System.EventHandler(this.Menadzment_Fakturisi0634_List_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button nova_btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Button ucitaj_btn;
    }
}