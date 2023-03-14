namespace TDOffice_v2
{
    partial class fm_PrenosRobeDopuna_Index
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
            this.magacin_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.destinacioniVrDok_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.destinacioniBrDok_txt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // magacin_cmb
            // 
            this.magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.magacin_cmb.FormattingEnabled = true;
            this.magacin_cmb.Location = new System.Drawing.Point(69, 34);
            this.magacin_cmb.Name = "magacin_cmb";
            this.magacin_cmb.Size = new System.Drawing.Size(285, 21);
            this.magacin_cmb.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Magacin:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(714, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sabiram sve izlaze robe (15 i 19) iz selektovanog magacina, baze 2023TCMD i to ko" +
    "piram u postojeci destinacioni dokument u bazi TERMODOM2023";
            // 
            // destinacioniVrDok_txt
            // 
            this.destinacioniVrDok_txt.Location = new System.Drawing.Point(116, 65);
            this.destinacioniVrDok_txt.Name = "destinacioniVrDok_txt";
            this.destinacioniVrDok_txt.Size = new System.Drawing.Size(92, 20);
            this.destinacioniVrDok_txt.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Destinacioni VrDok";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Destinacioni BrDok";
            // 
            // destinacioniBrDok_txt
            // 
            this.destinacioniBrDok_txt.Location = new System.Drawing.Point(116, 91);
            this.destinacioniBrDok_txt.Name = "destinacioniBrDok_txt";
            this.destinacioniBrDok_txt.Size = new System.Drawing.Size(92, 20);
            this.destinacioniBrDok_txt.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(526, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(278, 46);
            this.button1.TabIndex = 7;
            this.button1.Text = "Prenesi robu - dopuni dokument";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.odDatuma_dtp);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.doDatuma_dtp);
            this.panel1.Location = new System.Drawing.Point(360, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(467, 35);
            this.panel1.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(232, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Do datuma:";
            // 
            // odDatuma_dtp
            // 
            this.odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.odDatuma_dtp.Location = new System.Drawing.Point(82, 8);
            this.odDatuma_dtp.Name = "odDatuma_dtp";
            this.odDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.odDatuma_dtp.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Od datuma:";
            // 
            // doDatuma_dtp
            // 
            this.doDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            this.doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.doDatuma_dtp.Location = new System.Drawing.Point(300, 8);
            this.doDatuma_dtp.Name = "doDatuma_dtp";
            this.doDatuma_dtp.Size = new System.Drawing.Size(144, 20);
            this.doDatuma_dtp.TabIndex = 16;
            // 
            // fm_PrenosRobeDopuna_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 136);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.destinacioniBrDok_txt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.destinacioniVrDok_txt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.magacin_cmb);
            this.Name = "fm_PrenosRobeDopuna_Index";
            this.Text = "fm_PrenosRobeDopuna_Index";
            this.Load += new System.EventHandler(this.fm_PrenosRobeDopuna_Index_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox destinacioniVrDok_txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox destinacioniBrDok_txt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker odDatuma_dtp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker doDatuma_dtp;
    }
}