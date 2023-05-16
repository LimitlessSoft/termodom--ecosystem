
namespace TDOffice_v2
{
    partial class _1370_fm_NulaStavke
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
            this.panelZaglavlje = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.gb_AnalizaCena = new System.Windows.Forms.GroupBox();
            this.btn_UnesiRabat = new System.Windows.Forms.Button();
            this.btn_Proveri = new System.Windows.Forms.Button();
            this.btnFormirajCene = new System.Windows.Forms.Button();
            this.btnAnaliziraj = new System.Windows.Forms.Button();
            this.dtp_Do = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp_Od = new System.Windows.Forms.DateTimePicker();
            this.lblOd = new System.Windows.Forms.Label();
            this.cmbIzborGodine = new System.Windows.Forms.ComboBox();
            this.lblIzborGodine = new System.Windows.Forms.Label();
            this.gb_PocetnoStanje = new System.Windows.Forms.GroupBox();
            this.cb_PocetnoStanje = new System.Windows.Forms.CheckBox();
            this.rb_PoslednjaNavavnaCena = new System.Windows.Forms.RadioButton();
            this.rb_ProsecnaNabavnaCena = new System.Windows.Forms.RadioButton();
            this.panelPodaci = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelZaglavlje.SuspendLayout();
            this.gb_AnalizaCena.SuspendLayout();
            this.gb_PocetnoStanje.SuspendLayout();
            this.panelPodaci.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelZaglavlje
            // 
            this.panelZaglavlje.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelZaglavlje.Controls.Add(this.lblInfo);
            this.panelZaglavlje.Controls.Add(this.gb_AnalizaCena);
            this.panelZaglavlje.Location = new System.Drawing.Point(12, 12);
            this.panelZaglavlje.Name = "panelZaglavlje";
            this.panelZaglavlje.Size = new System.Drawing.Size(795, 133);
            this.panelZaglavlje.TabIndex = 1;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblInfo.ForeColor = System.Drawing.Color.Red;
            this.lblInfo.Location = new System.Drawing.Point(7, 99);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(298, 18);
            this.lblInfo.TabIndex = 5;
            this.lblInfo.Text = "Nabavne i prodajne cene su BEZ PPOREZA";
            // 
            // gb_AnalizaCena
            // 
            this.gb_AnalizaCena.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_AnalizaCena.Controls.Add(this.btn_UnesiRabat);
            this.gb_AnalizaCena.Controls.Add(this.btn_Proveri);
            this.gb_AnalizaCena.Controls.Add(this.btnFormirajCene);
            this.gb_AnalizaCena.Controls.Add(this.btnAnaliziraj);
            this.gb_AnalizaCena.Controls.Add(this.dtp_Do);
            this.gb_AnalizaCena.Controls.Add(this.label1);
            this.gb_AnalizaCena.Controls.Add(this.dtp_Od);
            this.gb_AnalizaCena.Controls.Add(this.lblOd);
            this.gb_AnalizaCena.Controls.Add(this.cmbIzborGodine);
            this.gb_AnalizaCena.Controls.Add(this.lblIzborGodine);
            this.gb_AnalizaCena.Controls.Add(this.gb_PocetnoStanje);
            this.gb_AnalizaCena.Location = new System.Drawing.Point(10, 4);
            this.gb_AnalizaCena.Name = "gb_AnalizaCena";
            this.gb_AnalizaCena.Size = new System.Drawing.Size(782, 92);
            this.gb_AnalizaCena.TabIndex = 4;
            this.gb_AnalizaCena.TabStop = false;
            this.gb_AnalizaCena.Text = "Analiza cena kroz marze";
            // 
            // btn_UnesiRabat
            // 
            this.btn_UnesiRabat.Location = new System.Drawing.Point(479, 63);
            this.btn_UnesiRabat.Name = "btn_UnesiRabat";
            this.btn_UnesiRabat.Size = new System.Drawing.Size(96, 23);
            this.btn_UnesiRabat.TabIndex = 10;
            this.btn_UnesiRabat.Text = "Unesi rabat";
            this.btn_UnesiRabat.UseVisualStyleBackColor = true;
            // 
            // btn_Proveri
            // 
            this.btn_Proveri.Location = new System.Drawing.Point(406, 63);
            this.btn_Proveri.Name = "btn_Proveri";
            this.btn_Proveri.Size = new System.Drawing.Size(67, 23);
            this.btn_Proveri.TabIndex = 9;
            this.btn_Proveri.Text = "Proveri";
            this.btn_Proveri.UseVisualStyleBackColor = true;
            // 
            // btnFormirajCene
            // 
            this.btnFormirajCene.Location = new System.Drawing.Point(304, 63);
            this.btnFormirajCene.Name = "btnFormirajCene";
            this.btnFormirajCene.Size = new System.Drawing.Size(96, 23);
            this.btnFormirajCene.TabIndex = 8;
            this.btnFormirajCene.Text = "Formiraj Cene";
            this.btnFormirajCene.UseVisualStyleBackColor = true;
            // 
            // btnAnaliziraj
            // 
            this.btnAnaliziraj.Location = new System.Drawing.Point(664, 29);
            this.btnAnaliziraj.Name = "btnAnaliziraj";
            this.btnAnaliziraj.Size = new System.Drawing.Size(101, 23);
            this.btnAnaliziraj.TabIndex = 7;
            this.btnAnaliziraj.Text = "Analiziraj";
            this.btnAnaliziraj.UseVisualStyleBackColor = true;
            // 
            // dtp_Do
            // 
            this.dtp_Do.Location = new System.Drawing.Point(539, 32);
            this.dtp_Do.Name = "dtp_Do";
            this.dtp_Do.Size = new System.Drawing.Size(112, 20);
            this.dtp_Do.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(536, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Od";
            // 
            // dtp_Od
            // 
            this.dtp_Od.Location = new System.Drawing.Point(421, 33);
            this.dtp_Od.Name = "dtp_Od";
            this.dtp_Od.Size = new System.Drawing.Size(112, 20);
            this.dtp_Od.TabIndex = 4;
            // 
            // lblOd
            // 
            this.lblOd.AutoSize = true;
            this.lblOd.Location = new System.Drawing.Point(418, 17);
            this.lblOd.Name = "lblOd";
            this.lblOd.Size = new System.Drawing.Size(21, 13);
            this.lblOd.TabIndex = 3;
            this.lblOd.Text = "Od";
            // 
            // cmbIzborGodine
            // 
            this.cmbIzborGodine.FormattingEnabled = true;
            this.cmbIzborGodine.Location = new System.Drawing.Point(307, 32);
            this.cmbIzborGodine.Name = "cmbIzborGodine";
            this.cmbIzborGodine.Size = new System.Drawing.Size(96, 21);
            this.cmbIzborGodine.TabIndex = 2;
            // 
            // lblIzborGodine
            // 
            this.lblIzborGodine.AutoSize = true;
            this.lblIzborGodine.Location = new System.Drawing.Point(304, 12);
            this.lblIzborGodine.Name = "lblIzborGodine";
            this.lblIzborGodine.Size = new System.Drawing.Size(65, 13);
            this.lblIzborGodine.TabIndex = 1;
            this.lblIzborGodine.Text = "Izbor godine";
            // 
            // gb_PocetnoStanje
            // 
            this.gb_PocetnoStanje.Controls.Add(this.cb_PocetnoStanje);
            this.gb_PocetnoStanje.Controls.Add(this.rb_PoslednjaNavavnaCena);
            this.gb_PocetnoStanje.Controls.Add(this.rb_ProsecnaNabavnaCena);
            this.gb_PocetnoStanje.Location = new System.Drawing.Point(18, 16);
            this.gb_PocetnoStanje.Name = "gb_PocetnoStanje";
            this.gb_PocetnoStanje.Size = new System.Drawing.Size(269, 76);
            this.gb_PocetnoStanje.TabIndex = 0;
            this.gb_PocetnoStanje.TabStop = false;
            // 
            // cb_PocetnoStanje
            // 
            this.cb_PocetnoStanje.AutoSize = true;
            this.cb_PocetnoStanje.Location = new System.Drawing.Point(6, 3);
            this.cb_PocetnoStanje.Name = "cb_PocetnoStanje";
            this.cb_PocetnoStanje.Size = new System.Drawing.Size(97, 17);
            this.cb_PocetnoStanje.TabIndex = 5;
            this.cb_PocetnoStanje.Text = "Pocetno stanje";
            this.cb_PocetnoStanje.UseVisualStyleBackColor = true;
            // 
            // rb_PoslednjaNavavnaCena
            // 
            this.rb_PoslednjaNavavnaCena.AutoSize = true;
            this.rb_PoslednjaNavavnaCena.Checked = true;
            this.rb_PoslednjaNavavnaCena.Location = new System.Drawing.Point(6, 42);
            this.rb_PoslednjaNavavnaCena.Name = "rb_PoslednjaNavavnaCena";
            this.rb_PoslednjaNavavnaCena.Size = new System.Drawing.Size(143, 17);
            this.rb_PoslednjaNavavnaCena.TabIndex = 1;
            this.rb_PoslednjaNavavnaCena.TabStop = true;
            this.rb_PoslednjaNavavnaCena.Text = "Poslednja nabavna cena";
            this.rb_PoslednjaNavavnaCena.UseVisualStyleBackColor = true;
            // 
            // rb_ProsecnaNabavnaCena
            // 
            this.rb_ProsecnaNabavnaCena.AutoSize = true;
            this.rb_ProsecnaNabavnaCena.Location = new System.Drawing.Point(6, 19);
            this.rb_ProsecnaNabavnaCena.Name = "rb_ProsecnaNabavnaCena";
            this.rb_ProsecnaNabavnaCena.Size = new System.Drawing.Size(142, 17);
            this.rb_ProsecnaNabavnaCena.TabIndex = 0;
            this.rb_ProsecnaNabavnaCena.Text = "Prosecna nabavna cena";
            this.rb_ProsecnaNabavnaCena.UseVisualStyleBackColor = true;
            // 
            // panelPodaci
            // 
            this.panelPodaci.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPodaci.Controls.Add(this.dataGridView1);
            this.panelPodaci.Location = new System.Drawing.Point(12, 151);
            this.panelPodaci.Name = "panelPodaci";
            this.panelPodaci.Size = new System.Drawing.Size(795, 201);
            this.panelPodaci.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(789, 185);
            this.dataGridView1.TabIndex = 0;
            // 
            // _1370_fm_NulaStavke
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 364);
            this.Controls.Add(this.panelPodaci);
            this.Controls.Add(this.panelZaglavlje);
            this.Name = "_1370_fm_NulaStavke";
            this.Text = "Definisanje prodajne cene NULA stavke";
            this.panelZaglavlje.ResumeLayout(false);
            this.panelZaglavlje.PerformLayout();
            this.gb_AnalizaCena.ResumeLayout(false);
            this.gb_AnalizaCena.PerformLayout();
            this.gb_PocetnoStanje.ResumeLayout(false);
            this.gb_PocetnoStanje.PerformLayout();
            this.panelPodaci.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelZaglavlje;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.GroupBox gb_AnalizaCena;
        private System.Windows.Forms.Button btn_UnesiRabat;
        private System.Windows.Forms.Button btn_Proveri;
        private System.Windows.Forms.Button btnFormirajCene;
        private System.Windows.Forms.Button btnAnaliziraj;
        private System.Windows.Forms.DateTimePicker dtp_Do;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_Od;
        private System.Windows.Forms.Label lblOd;
        private System.Windows.Forms.ComboBox cmbIzborGodine;
        private System.Windows.Forms.Label lblIzborGodine;
        private System.Windows.Forms.GroupBox gb_PocetnoStanje;
        private System.Windows.Forms.CheckBox cb_PocetnoStanje;
        private System.Windows.Forms.RadioButton rb_PoslednjaNavavnaCena;
        private System.Windows.Forms.RadioButton rb_ProsecnaNabavnaCena;
        private System.Windows.Forms.Panel panelPodaci;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}