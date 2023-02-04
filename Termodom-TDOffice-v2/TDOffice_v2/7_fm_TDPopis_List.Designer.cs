
namespace TDOffice_v2
{
    partial class _7_fm_TDPopis_List
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
            this.doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tipPopisa_cmb = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Help = new System.Windows.Forms.Button();
            this.btn_KreirajPopisSaStavkama = new System.Windows.Forms.Button();
            this.nova_btn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slogova_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 117);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(775, 342);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // doDatuma_dtp
            // 
            this.doDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.doDatuma_dtp.Location = new System.Drawing.Point(295, 3);
            this.doDatuma_dtp.Name = "doDatuma_dtp";
            this.doDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.doDatuma_dtp.TabIndex = 4;
            this.doDatuma_dtp.CloseUp += new System.EventHandler(this.odDatuma_dtp_CloseUp);
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odDatuma_dtp.Location = new System.Drawing.Point(77, 3);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.odDatuma_dtp.TabIndex = 3;
            this.odDatuma_dtp.CloseUp += new System.EventHandler(this.odDatuma_dtp_CloseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Od datuma:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Do datuma:";
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(77, 29);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(228, 21);
            this.magacin_cmb.TabIndex = 7;
            this.magacin_cmb.SelectedIndexChanged += new System.EventHandler(this.magacin_cmb_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Magacin";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.tipPopisa_cmb);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.magacin_cmb);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.odDatuma_dtp);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.doDatuma_dtp);
            this.panel2.Location = new System.Drawing.Point(12, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(775, 58);
            this.panel2.TabIndex = 17;
            // 
            // tipPopisa_cmb
            // 
            this.tipPopisa_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tipPopisa_cmb.FormattingEnabled = true;
            this.tipPopisa_cmb.Items.AddRange(new object[] {
            "Sve Vrste",
            "Popis Za Nabavku",
            "Vanredni Popis"});
            this.tipPopisa_cmb.Location = new System.Drawing.Point(545, 29);
            this.tipPopisa_cmb.Name = "tipPopisa_cmb";
            this.tipPopisa_cmb.Size = new System.Drawing.Size(228, 21);
            this.tipPopisa_cmb.TabIndex = 9;
            this.tipPopisa_cmb.SelectedIndexChanged += new System.EventHandler(this.tipPopisa_cmb_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn);
            this.panel1.Controls.Add(this.btn_Help);
            this.panel1.Controls.Add(this.btn_KreirajPopisSaStavkama);
            this.panel1.Controls.Add(this.nova_btn);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(775, 35);
            this.panel1.TabIndex = 16;
            // 
            // btn_Help
            // 
            this.btn_Help.Location = new System.Drawing.Point(706, 6);
            this.btn_Help.Name = "btn_Help";
            this.btn_Help.Size = new System.Drawing.Size(66, 29);
            this.btn_Help.TabIndex = 16;
            this.btn_Help.Text = "HELP";
            this.btn_Help.UseVisualStyleBackColor = true;
            this.btn_Help.Click += new System.EventHandler(this.btn_Help_Click);
            // 
            // btn_KreirajPopisSaStavkama
            // 
            this.btn_KreirajPopisSaStavkama.Location = new System.Drawing.Point(37, 3);
            this.btn_KreirajPopisSaStavkama.Name = "btn_KreirajPopisSaStavkama";
            this.btn_KreirajPopisSaStavkama.Size = new System.Drawing.Size(158, 29);
            this.btn_KreirajPopisSaStavkama.TabIndex = 15;
            this.btn_KreirajPopisSaStavkama.Text = "Kreiraj popis sa stavkama";
            this.btn_KreirajPopisSaStavkama.UseVisualStyleBackColor = true;
            this.btn_KreirajPopisSaStavkama.Click += new System.EventHandler(this.btn_KreirajPopisSaStavkama_Click);
            // 
            // nova_btn
            // 
            this.nova_btn.BackgroundImage = global::TDOffice_v2.Properties.Resources.new_icon;
            this.nova_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.nova_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nova_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.nova_btn.Location = new System.Drawing.Point(4, 3);
            this.nova_btn.Name = "nova_btn";
            this.nova_btn.Size = new System.Drawing.Size(27, 29);
            this.nova_btn.TabIndex = 13;
            this.nova_btn.UseVisualStyleBackColor = true;
            this.nova_btn.Click += new System.EventHandler(this.nova_btn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slogova_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 466);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(799, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slogova_lbl
            // 
            this.slogova_lbl.Name = "slogova_lbl";
            this.slogova_lbl.Size = new System.Drawing.Size(55, 17);
            this.slogova_lbl.Text = "Slogova: ";
            // 
            // kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn
            // 
            this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn.Location = new System.Drawing.Point(201, 3);
            this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn.Name = "kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn";
            this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn.Size = new System.Drawing.Size(295, 29);
            this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn.TabIndex = 17;
            this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn.Text = "Kreiraj I Uvuci Stavke Po Popisu Iz Komercijalnog";
            this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn.UseVisualStyleBackColor = true;
            this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn.Click += new System.EventHandler(this.kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn_Click);
            // 
            // _7_fm_TDPopis_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 488);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "_7_fm_TDPopis_List";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TDPopis - Tabelarni pregled";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this._7_fm_TDPopis_List_FormClosed);
            this.Load += new System.EventHandler(this._7_fm_TDPopis_List_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button nova_btn;
        private System.Windows.Forms.ComboBox tipPopisa_cmb;
        private System.Windows.Forms.Button btn_KreirajPopisSaStavkama;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slogova_lbl;
        private System.Windows.Forms.Button btn_Help;
        private System.Windows.Forms.Button kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn;
    }
}