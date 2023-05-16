
namespace TDOffice_v2
{
    partial class fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi
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
            this.help_btn = new System.Windows.Forms.Button();
            this.posaljiKaoIzvestaj_btn = new System.Windows.Forms.Button();
            this.prikaziVrednosti_cb = new System.Windows.Forms.CheckBox();
            this.prikaziKolicine_cb = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // help_btn
            // 
            this.help_btn.Location = new System.Drawing.Point(989, 8);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(75, 23);
            this.help_btn.TabIndex = 13;
            this.help_btn.Text = "HELP";
            this.help_btn.UseVisualStyleBackColor = true;
            this.help_btn.Click += new System.EventHandler(this.help_btn_Click);
            // 
            // posaljiKaoIzvestaj_btn
            // 
            this.posaljiKaoIzvestaj_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posaljiKaoIzvestaj_btn.Location = new System.Drawing.Point(324, 8);
            this.posaljiKaoIzvestaj_btn.Name = "posaljiKaoIzvestaj_btn";
            this.posaljiKaoIzvestaj_btn.Size = new System.Drawing.Size(161, 44);
            this.posaljiKaoIzvestaj_btn.TabIndex = 12;
            this.posaljiKaoIzvestaj_btn.Text = "Posalji Kao Izvestaj";
            this.posaljiKaoIzvestaj_btn.UseVisualStyleBackColor = true;
            this.posaljiKaoIzvestaj_btn.Click += new System.EventHandler(this.posaljiKaoIzvestaj_btn_Click);
            // 
            // prikaziVrednosti_cb
            // 
            this.prikaziVrednosti_cb.AutoSize = true;
            this.prikaziVrednosti_cb.Checked = true;
            this.prikaziVrednosti_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.prikaziVrednosti_cb.Location = new System.Drawing.Point(12, 35);
            this.prikaziVrednosti_cb.Name = "prikaziVrednosti_cb";
            this.prikaziVrednosti_cb.Size = new System.Drawing.Size(104, 17);
            this.prikaziVrednosti_cb.TabIndex = 11;
            this.prikaziVrednosti_cb.Text = "Prikazi Vrednosti";
            this.prikaziVrednosti_cb.UseVisualStyleBackColor = true;
            this.prikaziVrednosti_cb.CheckedChanged += new System.EventHandler(this.prikaziVrednosti_cb_CheckedChanged);
            // 
            // prikaziKolicine_cb
            // 
            this.prikaziKolicine_cb.AutoSize = true;
            this.prikaziKolicine_cb.Checked = true;
            this.prikaziKolicine_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.prikaziKolicine_cb.Location = new System.Drawing.Point(12, 12);
            this.prikaziKolicine_cb.Name = "prikaziKolicine_cb";
            this.prikaziKolicine_cb.Size = new System.Drawing.Size(97, 17);
            this.prikaziKolicine_cb.TabIndex = 10;
            this.prikaziKolicine_cb.Text = "Prikazi Kolicine";
            this.prikaziKolicine_cb.UseVisualStyleBackColor = true;
            this.prikaziKolicine_cb.CheckedChanged += new System.EventHandler(this.prikaziKolicine_cb_CheckedChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 58);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(1057, 531);
            this.dataGridView1.TabIndex = 9;
            // 
            // fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 601);
            this.Controls.Add(this.help_btn);
            this.Controls.Add(this.posaljiKaoIzvestaj_btn);
            this.Controls.Add(this.prikaziVrednosti_cb);
            this.Controls.Add(this.prikaziKolicine_cb);
            this.Controls.Add(this.dataGridView1);
            this.Name = "fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi";
            this.Text = "Zbirno Po Robi";
            this.Load += new System.EventHandler(this.fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button help_btn;
        private System.Windows.Forms.Button posaljiKaoIzvestaj_btn;
        private System.Windows.Forms.CheckBox prikaziVrednosti_cb;
        private System.Windows.Forms.CheckBox prikaziKolicine_cb;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}