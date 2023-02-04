
namespace TDOffice_v2
{
    partial class IzborRobe_DodatniFilteri
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_RobaVanPopisaBr = new System.Windows.Forms.CheckBox();
            this.cb_RobaSaPrometom = new System.Windows.Forms.CheckBox();
            this.cb_NePopisanaRoba = new System.Windows.Forms.CheckBox();
            this.tb_RobaVanDok = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtp_Do = new System.Windows.Forms.DateTimePicker();
            this.dtp_Od = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_Grupa = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmb_PodGrupa = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb_Proizvodjac = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_Dobavljac = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_SamoRobaSaStanjemNula = new System.Windows.Forms.CheckBox();
            this.cb_BezRobeSaStanjemNula = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_SveIspodOptimalnihZaliha = new System.Windows.Forms.CheckBox();
            this.cb_SveIspodKriticnihZaliha = new System.Windows.Forms.CheckBox();
            this.btn_Primeni = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_RobaVanPopisaBr);
            this.groupBox1.Controls.Add(this.cb_RobaSaPrometom);
            this.groupBox1.Controls.Add(this.cb_NePopisanaRoba);
            this.groupBox1.Controls.Add(this.tb_RobaVanDok);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtp_Do);
            this.groupBox1.Controls.Add(this.dtp_Od);
            this.groupBox1.Location = new System.Drawing.Point(15, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 134);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "IzvorRobe";
            // 
            // cb_RobaVanPopisaBr
            // 
            this.cb_RobaVanPopisaBr.AutoSize = true;
            this.cb_RobaVanPopisaBr.Location = new System.Drawing.Point(6, 94);
            this.cb_RobaVanPopisaBr.Name = "cb_RobaVanPopisaBr";
            this.cb_RobaVanPopisaBr.Size = new System.Drawing.Size(138, 19);
            this.cb_RobaVanPopisaBr.TabIndex = 18;
            this.cb_RobaVanPopisaBr.Text = "Roba van popisa br:";
            this.cb_RobaVanPopisaBr.UseVisualStyleBackColor = true;
            this.cb_RobaVanPopisaBr.Click += new System.EventHandler(this.cb_Click);
            // 
            // cb_RobaSaPrometom
            // 
            this.cb_RobaSaPrometom.AutoSize = true;
            this.cb_RobaSaPrometom.Location = new System.Drawing.Point(7, 69);
            this.cb_RobaSaPrometom.Name = "cb_RobaSaPrometom";
            this.cb_RobaSaPrometom.Size = new System.Drawing.Size(135, 19);
            this.cb_RobaSaPrometom.TabIndex = 17;
            this.cb_RobaSaPrometom.Text = "Roba sa prometom";
            this.cb_RobaSaPrometom.UseVisualStyleBackColor = true;
            this.cb_RobaSaPrometom.Click += new System.EventHandler(this.cb_Click);
            // 
            // cb_NePopisanaRoba
            // 
            this.cb_NePopisanaRoba.AutoSize = true;
            this.cb_NePopisanaRoba.Location = new System.Drawing.Point(7, 29);
            this.cb_NePopisanaRoba.Name = "cb_NePopisanaRoba";
            this.cb_NePopisanaRoba.Size = new System.Drawing.Size(127, 19);
            this.cb_NePopisanaRoba.TabIndex = 16;
            this.cb_NePopisanaRoba.Text = "Ne popisana roba";
            this.cb_NePopisanaRoba.UseVisualStyleBackColor = true;
            this.cb_NePopisanaRoba.Click += new System.EventHandler(this.cb_Click);
            // 
            // tb_RobaVanDok
            // 
            this.tb_RobaVanDok.Location = new System.Drawing.Point(151, 91);
            this.tb_RobaVanDok.Name = "tb_RobaVanDok";
            this.tb_RobaVanDok.Size = new System.Drawing.Size(87, 20);
            this.tb_RobaVanDok.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(253, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 15);
            this.label6.TabIndex = 13;
            this.label6.Text = "Do:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(147, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "Od:";
            // 
            // dtp_Do
            // 
            this.dtp_Do.CustomFormat = "dd.MM.yyyy";
            this.dtp_Do.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Do.Location = new System.Drawing.Point(256, 65);
            this.dtp_Do.Name = "dtp_Do";
            this.dtp_Do.Size = new System.Drawing.Size(92, 20);
            this.dtp_Do.TabIndex = 11;
            // 
            // dtp_Od
            // 
            this.dtp_Od.CustomFormat = "dd.MM.yyyy";
            this.dtp_Od.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Od.Location = new System.Drawing.Point(146, 65);
            this.dtp_Od.Name = "dtp_Od";
            this.dtp_Od.Size = new System.Drawing.Size(92, 20);
            this.dtp_Od.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Grupa";
            // 
            // cmb_Grupa
            // 
            this.cmb_Grupa.FormattingEnabled = true;
            this.cmb_Grupa.Location = new System.Drawing.Point(91, 9);
            this.cmb_Grupa.Name = "cmb_Grupa";
            this.cmb_Grupa.Size = new System.Drawing.Size(186, 21);
            this.cmb_Grupa.TabIndex = 3;
            this.cmb_Grupa.Tag = "1";
            this.cmb_Grupa.SelectedIndexChanged += new System.EventHandler(this.cmb_Grupa_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "PodGrupa";
            // 
            // cmb_PodGrupa
            // 
            this.cmb_PodGrupa.FormattingEnabled = true;
            this.cmb_PodGrupa.Location = new System.Drawing.Point(91, 46);
            this.cmb_PodGrupa.Name = "cmb_PodGrupa";
            this.cmb_PodGrupa.Size = new System.Drawing.Size(186, 21);
            this.cmb_PodGrupa.TabIndex = 5;
            this.cmb_PodGrupa.Tag = "2";
            this.cmb_PodGrupa.SelectedIndexChanged += new System.EventHandler(this.cmb_PodGrupa_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Proizvodjac";
            // 
            // cmb_Proizvodjac
            // 
            this.cmb_Proizvodjac.FormattingEnabled = true;
            this.cmb_Proizvodjac.Location = new System.Drawing.Point(91, 80);
            this.cmb_Proizvodjac.Name = "cmb_Proizvodjac";
            this.cmb_Proizvodjac.Size = new System.Drawing.Size(186, 21);
            this.cmb_Proizvodjac.TabIndex = 7;
            this.cmb_Proizvodjac.Tag = "3";
            this.cmb_Proizvodjac.SelectedIndexChanged += new System.EventHandler(this.cmb_Proizvodjac_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Dobavljac";
            // 
            // cmb_Dobavljac
            // 
            this.cmb_Dobavljac.FormattingEnabled = true;
            this.cmb_Dobavljac.Location = new System.Drawing.Point(91, 111);
            this.cmb_Dobavljac.Name = "cmb_Dobavljac";
            this.cmb_Dobavljac.Size = new System.Drawing.Size(186, 21);
            this.cmb_Dobavljac.TabIndex = 9;
            this.cmb_Dobavljac.Tag = "4";
            this.cmb_Dobavljac.SelectedIndexChanged += new System.EventHandler(this.cmb_Dobavljac_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_SamoRobaSaStanjemNula);
            this.groupBox2.Controls.Add(this.cb_BezRobeSaStanjemNula);
            this.groupBox2.Location = new System.Drawing.Point(15, 295);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(354, 100);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "StanjeRobe";
            // 
            // cb_SamoRobaSaStanjemNula
            // 
            this.cb_SamoRobaSaStanjemNula.AutoSize = true;
            this.cb_SamoRobaSaStanjemNula.Location = new System.Drawing.Point(6, 59);
            this.cb_SamoRobaSaStanjemNula.Name = "cb_SamoRobaSaStanjemNula";
            this.cb_SamoRobaSaStanjemNula.Size = new System.Drawing.Size(180, 19);
            this.cb_SamoRobaSaStanjemNula.TabIndex = 20;
            this.cb_SamoRobaSaStanjemNula.Text = "Samo roba sa stanjem nula";
            this.cb_SamoRobaSaStanjemNula.UseVisualStyleBackColor = true;
            this.cb_SamoRobaSaStanjemNula.Click += new System.EventHandler(this.cb_Click);
            // 
            // cb_BezRobeSaStanjemNula
            // 
            this.cb_BezRobeSaStanjemNula.AutoSize = true;
            this.cb_BezRobeSaStanjemNula.Location = new System.Drawing.Point(7, 19);
            this.cb_BezRobeSaStanjemNula.Name = "cb_BezRobeSaStanjemNula";
            this.cb_BezRobeSaStanjemNula.Size = new System.Drawing.Size(168, 19);
            this.cb_BezRobeSaStanjemNula.TabIndex = 19;
            this.cb_BezRobeSaStanjemNula.Text = "Bez robe sa stanjem nula";
            this.cb_BezRobeSaStanjemNula.UseVisualStyleBackColor = true;
            this.cb_BezRobeSaStanjemNula.Click += new System.EventHandler(this.cb_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_SveIspodOptimalnihZaliha);
            this.groupBox3.Controls.Add(this.cb_SveIspodKriticnihZaliha);
            this.groupBox3.Location = new System.Drawing.Point(15, 401);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(354, 91);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Tag = "OptKrit";
            // 
            // cb_SveIspodOptimalnihZaliha
            // 
            this.cb_SveIspodOptimalnihZaliha.AutoSize = true;
            this.cb_SveIspodOptimalnihZaliha.Location = new System.Drawing.Point(10, 66);
            this.cb_SveIspodOptimalnihZaliha.Name = "cb_SveIspodOptimalnihZaliha";
            this.cb_SveIspodOptimalnihZaliha.Size = new System.Drawing.Size(179, 19);
            this.cb_SveIspodOptimalnihZaliha.TabIndex = 22;
            this.cb_SveIspodOptimalnihZaliha.Text = "Sve ispod optimalnih zaliha";
            this.cb_SveIspodOptimalnihZaliha.UseVisualStyleBackColor = true;
            this.cb_SveIspodOptimalnihZaliha.Click += new System.EventHandler(this.cb_Click);
            // 
            // cb_SveIspodKriticnihZaliha
            // 
            this.cb_SveIspodKriticnihZaliha.AutoSize = true;
            this.cb_SveIspodKriticnihZaliha.Location = new System.Drawing.Point(10, 19);
            this.cb_SveIspodKriticnihZaliha.Name = "cb_SveIspodKriticnihZaliha";
            this.cb_SveIspodKriticnihZaliha.Size = new System.Drawing.Size(163, 19);
            this.cb_SveIspodKriticnihZaliha.TabIndex = 21;
            this.cb_SveIspodKriticnihZaliha.Text = "Sve ispod kriticnih zaliha";
            this.cb_SveIspodKriticnihZaliha.UseVisualStyleBackColor = true;
            this.cb_SveIspodKriticnihZaliha.Click += new System.EventHandler(this.cb_Click);
            // 
            // btn_Primeni
            // 
            this.btn_Primeni.Location = new System.Drawing.Point(288, 498);
            this.btn_Primeni.Name = "btn_Primeni";
            this.btn_Primeni.Size = new System.Drawing.Size(75, 23);
            this.btn_Primeni.TabIndex = 12;
            this.btn_Primeni.Text = "Primeni";
            this.btn_Primeni.UseVisualStyleBackColor = true;
            this.btn_Primeni.Click += new System.EventHandler(this.btn_Primeni_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(288, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 23);
            this.button1.TabIndex = 13;
            this.button1.Tag = "1";
            this.button1.Text = "<>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(288, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 23);
            this.button2.TabIndex = 14;
            this.button2.Tag = "2";
            this.button2.Text = "<>";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(288, 80);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(30, 23);
            this.button3.TabIndex = 15;
            this.button3.Tag = "3";
            this.button3.Text = "<>";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(288, 112);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(30, 23);
            this.button4.TabIndex = 16;
            this.button4.Tag = "4";
            this.button4.Text = "<>";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // IzborRobe_DodatniFilteri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 528);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_Primeni);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmb_Dobavljac);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmb_Proizvodjac);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmb_PodGrupa);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmb_Grupa);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "IzborRobe_DodatniFilteri";
            this.Text = "Izbor robe <<Dodatni filter>>";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_RobaVanDok;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtp_Do;
        private System.Windows.Forms.DateTimePicker dtp_Od;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_Grupa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmb_PodGrupa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_Proizvodjac;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_Dobavljac;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_Primeni;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox cb_RobaVanPopisaBr;
        private System.Windows.Forms.CheckBox cb_RobaSaPrometom;
        private System.Windows.Forms.CheckBox cb_NePopisanaRoba;
        private System.Windows.Forms.CheckBox cb_SamoRobaSaStanjemNula;
        private System.Windows.Forms.CheckBox cb_BezRobeSaStanjemNula;
        private System.Windows.Forms.CheckBox cb_SveIspodOptimalnihZaliha;
        private System.Windows.Forms.CheckBox cb_SveIspodKriticnihZaliha;
    }
}