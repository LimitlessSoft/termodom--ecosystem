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
            magacin_cmb = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            destinacioniVrDok_txt = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            destinacioniBrDok_txt = new System.Windows.Forms.TextBox();
            button1 = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            label5 = new System.Windows.Forms.Label();
            odDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            label6 = new System.Windows.Forms.Label();
            doDatuma_dtp = new System.Windows.Forms.DateTimePicker();
            checkBox1 = new System.Windows.Forms.CheckBox();
            checkBox2 = new System.Windows.Forms.CheckBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // magacin_cmb
            // 
            magacin_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            magacin_cmb.FormattingEnabled = true;
            magacin_cmb.Location = new System.Drawing.Point(80, 76);
            magacin_cmb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            magacin_cmb.Name = "magacin_cmb";
            magacin_cmb.Size = new System.Drawing.Size(332, 23);
            magacin_cmb.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 80);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(56, 15);
            label1.TabIndex = 1;
            label1.Text = "Magacin:";
            // 
            // destinacioniVrDok_txt
            // 
            destinacioniVrDok_txt.Location = new System.Drawing.Point(135, 112);
            destinacioniVrDok_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            destinacioniVrDok_txt.Name = "destinacioniVrDok_txt";
            destinacioniVrDok_txt.Size = new System.Drawing.Size(107, 23);
            destinacioniVrDok_txt.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(14, 115);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(107, 15);
            label3.TabIndex = 4;
            label3.Text = "Destinacioni VrDok";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(14, 145);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(107, 15);
            label4.TabIndex = 6;
            label4.Text = "Destinacioni BrDok";
            // 
            // destinacioniBrDok_txt
            // 
            destinacioniBrDok_txt.Location = new System.Drawing.Point(135, 142);
            destinacioniBrDok_txt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            destinacioniBrDok_txt.Name = "destinacioniBrDok_txt";
            destinacioniBrDok_txt.Size = new System.Drawing.Size(107, 23);
            destinacioniBrDok_txt.TabIndex = 5;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(614, 112);
            button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(324, 53);
            button1.TabIndex = 7;
            button1.Text = "Prenesi robu - dopuni dokument";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_ClickAsync;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.Controls.Add(label5);
            panel1.Controls.Add(odDatuma_dtp);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(doDatuma_dtp);
            panel1.Location = new System.Drawing.Point(420, 66);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(545, 40);
            panel1.TabIndex = 16;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(271, 14);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(69, 15);
            label5.TabIndex = 18;
            label5.Text = "Do datuma:";
            // 
            // odDatuma_dtp
            // 
            odDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            odDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            odDatuma_dtp.Location = new System.Drawing.Point(96, 9);
            odDatuma_dtp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            odDatuma_dtp.Name = "odDatuma_dtp";
            odDatuma_dtp.Size = new System.Drawing.Size(167, 23);
            odDatuma_dtp.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(16, 13);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(70, 15);
            label6.TabIndex = 17;
            label6.Text = "Od datuma:";
            // 
            // doDatuma_dtp
            // 
            doDatuma_dtp.CustomFormat = "dd.MMMM.yyyy";
            doDatuma_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            doDatuma_dtp.Location = new System.Drawing.Point(350, 9);
            doDatuma_dtp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            doDatuma_dtp.Name = "doDatuma_dtp";
            doDatuma_dtp.Size = new System.Drawing.Size(167, 23);
            doDatuma_dtp.TabIndex = 16;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(14, 35);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(933, 19);
            checkBox1.TabIndex = 17;
            checkBox1.Text = "Sabiram sve izlaze robe (15 i 19) iz selektovanog magacina, baze 2023TCMD i to kopiram u postojeci destinacioni dokument u bazi TERMODOM2023 (od datuma, do datuma)\r\n";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new System.Drawing.Point(12, 12);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new System.Drawing.Size(856, 19);
            checkBox2.TabIndex = 18;
            checkBox2.Text = "Uzimam stanje robe iz magacina gde je stanje manje od 0 i apsolutnu vrednost tog stanja unosim u dokument. Radi samo nad bazom 2023TCMD (do datuma)";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // fm_PrenosRobeDopuna_Index
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(979, 182);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(panel1);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(destinacioniBrDok_txt);
            Controls.Add(label3);
            Controls.Add(destinacioniVrDok_txt);
            Controls.Add(label1);
            Controls.Add(magacin_cmb);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "fm_PrenosRobeDopuna_Index";
            Text = "fm_PrenosRobeDopuna_Index";
            Load += fm_PrenosRobeDopuna_Index_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox magacin_cmb;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}