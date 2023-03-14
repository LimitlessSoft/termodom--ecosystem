
namespace TDOffice_v2
{
    partial class fm_Izvestaj_OdobreniRabati_PosaljiKaoIzvestaj
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
            this.tb_NaslovIzvestaja = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.help_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.komentar_rtb = new System.Windows.Forms.RichTextBox();
            this.korisnicima_clb = new System.Windows.Forms.CheckedListBox();
            this.posalji_btn = new System.Windows.Forms.Button();
            this.korisnicimaMagacina_gb = new System.Windows.Forms.GroupBox();
            this.izvestajKakavVIdim_rb = new System.Windows.Forms.RadioButton();
            this.korisnicimaMagacinaCeoIzvestaj_rb = new System.Windows.Forms.RadioButton();
            this.korisnicimaMagacinaSamoSvojePodatke_rb = new System.Windows.Forms.RadioButton();
            this.korisnicimaMagacina_gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_NaslovIzvestaja
            // 
            this.tb_NaslovIzvestaja.Location = new System.Drawing.Point(12, 406);
            this.tb_NaslovIzvestaja.Name = "tb_NaslovIzvestaja";
            this.tb_NaslovIzvestaja.Size = new System.Drawing.Size(507, 20);
            this.tb_NaslovIzvestaja.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 388);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 15);
            this.label2.TabIndex = 23;
            this.label2.Text = "Naslov izvestaja";
            // 
            // help_btn
            // 
            this.help_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.help_btn.Location = new System.Drawing.Point(367, 13);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(152, 38);
            this.help_btn.TabIndex = 22;
            this.help_btn.Text = "HELP";
            this.help_btn.UseVisualStyleBackColor = true;
            this.help_btn.Click += new System.EventHandler(this.help_btn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 426);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "Komentar:";
            // 
            // komentar_rtb
            // 
            this.komentar_rtb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.komentar_rtb.Location = new System.Drawing.Point(9, 442);
            this.komentar_rtb.Name = "komentar_rtb";
            this.komentar_rtb.Size = new System.Drawing.Size(509, 116);
            this.komentar_rtb.TabIndex = 20;
            this.komentar_rtb.Text = "";
            // 
            // korisnicima_clb
            // 
            this.korisnicima_clb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.korisnicima_clb.CheckOnClick = true;
            this.korisnicima_clb.FormattingEnabled = true;
            this.korisnicima_clb.Location = new System.Drawing.Point(10, 113);
            this.korisnicima_clb.Name = "korisnicima_clb";
            this.korisnicima_clb.Size = new System.Drawing.Size(509, 274);
            this.korisnicima_clb.TabIndex = 19;
            // 
            // posalji_btn
            // 
            this.posalji_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.posalji_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posalji_btn.Location = new System.Drawing.Point(9, 564);
            this.posalji_btn.Name = "posalji_btn";
            this.posalji_btn.Size = new System.Drawing.Size(509, 55);
            this.posalji_btn.TabIndex = 18;
            this.posalji_btn.Text = "Posalji";
            this.posalji_btn.UseVisualStyleBackColor = true;
            this.posalji_btn.Click += new System.EventHandler(this.posalji_btn_Click);
            // 
            // korisnicimaMagacina_gb
            // 
            this.korisnicimaMagacina_gb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.korisnicimaMagacina_gb.Controls.Add(this.izvestajKakavVIdim_rb);
            this.korisnicimaMagacina_gb.Controls.Add(this.korisnicimaMagacinaCeoIzvestaj_rb);
            this.korisnicimaMagacina_gb.Controls.Add(this.korisnicimaMagacinaSamoSvojePodatke_rb);
            this.korisnicimaMagacina_gb.Location = new System.Drawing.Point(10, 13);
            this.korisnicimaMagacina_gb.Name = "korisnicimaMagacina_gb";
            this.korisnicimaMagacina_gb.Size = new System.Drawing.Size(332, 94);
            this.korisnicimaMagacina_gb.TabIndex = 17;
            this.korisnicimaMagacina_gb.TabStop = false;
            this.korisnicimaMagacina_gb.Text = "Salji izvestaj";
            // 
            // izvestajKakavVIdim_rb
            // 
            this.izvestajKakavVIdim_rb.AutoSize = true;
            this.izvestajKakavVIdim_rb.Enabled = false;
            this.izvestajKakavVIdim_rb.Location = new System.Drawing.Point(6, 42);
            this.izvestajKakavVIdim_rb.Name = "izvestajKakavVIdim_rb";
            this.izvestajKakavVIdim_rb.Size = new System.Drawing.Size(162, 19);
            this.izvestajKakavVIdim_rb.TabIndex = 5;
            this.izvestajKakavVIdim_rb.Text = "Izvestaj Bas Kakav Vidim";
            this.izvestajKakavVIdim_rb.UseVisualStyleBackColor = true;
            // 
            // korisnicimaMagacinaCeoIzvestaj_rb
            // 
            this.korisnicimaMagacinaCeoIzvestaj_rb.AutoSize = true;
            this.korisnicimaMagacinaCeoIzvestaj_rb.Checked = true;
            this.korisnicimaMagacinaCeoIzvestaj_rb.Location = new System.Drawing.Point(6, 65);
            this.korisnicimaMagacinaCeoIzvestaj_rb.Name = "korisnicimaMagacinaCeoIzvestaj_rb";
            this.korisnicimaMagacinaCeoIzvestaj_rb.Size = new System.Drawing.Size(93, 19);
            this.korisnicimaMagacinaCeoIzvestaj_rb.TabIndex = 4;
            this.korisnicimaMagacinaCeoIzvestaj_rb.TabStop = true;
            this.korisnicimaMagacinaCeoIzvestaj_rb.Text = "Ceo Izvestaj";
            this.korisnicimaMagacinaCeoIzvestaj_rb.UseVisualStyleBackColor = true;
            this.korisnicimaMagacinaCeoIzvestaj_rb.CheckedChanged += new System.EventHandler(this.korisnicimaMagacinaCeoIzvestaj_rb_CheckedChanged);
            // 
            // korisnicimaMagacinaSamoSvojePodatke_rb
            // 
            this.korisnicimaMagacinaSamoSvojePodatke_rb.AutoSize = true;
            this.korisnicimaMagacinaSamoSvojePodatke_rb.Location = new System.Drawing.Point(6, 19);
            this.korisnicimaMagacinaSamoSvojePodatke_rb.Name = "korisnicimaMagacinaSamoSvojePodatke_rb";
            this.korisnicimaMagacinaSamoSvojePodatke_rb.Size = new System.Drawing.Size(93, 19);
            this.korisnicimaMagacinaSamoSvojePodatke_rb.TabIndex = 3;
            this.korisnicimaMagacinaSamoSvojePodatke_rb.Text = "Za Magacin";
            this.korisnicimaMagacinaSamoSvojePodatke_rb.UseVisualStyleBackColor = true;
            this.korisnicimaMagacinaSamoSvojePodatke_rb.CheckedChanged += new System.EventHandler(this.korisnicimaMagacinaSamoSvojePodatke_rb_CheckedChanged);
            // 
            // fm_Izvestaj_OdobreniRabati_PosaljiKaoIzvestaj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 631);
            this.Controls.Add(this.tb_NaslovIzvestaja);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.help_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.komentar_rtb);
            this.Controls.Add(this.korisnicima_clb);
            this.Controls.Add(this.posalji_btn);
            this.Controls.Add(this.korisnicimaMagacina_gb);
            this.Name = "fm_Izvestaj_OdobreniRabati_PosaljiKaoIzvestaj";
            this.Text = "fm_Izvestaj_OdobreniRabati_PosaljiKaoIzvestaj";
            this.korisnicimaMagacina_gb.ResumeLayout(false);
            this.korisnicimaMagacina_gb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_NaslovIzvestaja;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button help_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox komentar_rtb;
        private System.Windows.Forms.CheckedListBox korisnicima_clb;
        private System.Windows.Forms.Button posalji_btn;
        private System.Windows.Forms.GroupBox korisnicimaMagacina_gb;
        private System.Windows.Forms.RadioButton izvestajKakavVIdim_rb;
        private System.Windows.Forms.RadioButton korisnicimaMagacinaCeoIzvestaj_rb;
        private System.Windows.Forms.RadioButton korisnicimaMagacinaSamoSvojePodatke_rb;
    }
}