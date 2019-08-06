using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing;
using System;
namespace TVSender
{
    partial class WhiteBalanceMinolta
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
            if (disposing && (components != null) )
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
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.richTextBox38 = new System.Windows.Forms.RichTextBox();
            this.richTextBox37 = new System.Windows.Forms.RichTextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label18 = new System.Windows.Forms.Label();
            this.richTextBoxToleranceLv = new System.Windows.Forms.RichTextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.richTextBox32 = new System.Windows.Forms.RichTextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.buttonConnectUSB = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label41 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboBoxToleranceXY = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.richTextBox9 = new System.Windows.Forms.RichTextBox();
            this.richTextBox8 = new System.Windows.Forms.RichTextBox();
            this.richTextBox7 = new System.Windows.Forms.RichTextBox();
            this.richTextBox6 = new System.Windows.Forms.RichTextBox();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxXYPreset = new System.Windows.Forms.ComboBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.labelMinusLv = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.richTextBoxG = new System.Windows.Forms.RichTextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.richTextBoxWarmLvPercent = new System.Windows.Forms.RichTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.richTextBoxNormLvPercent = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBoxCoolLvPercent = new System.Windows.Forms.RichTextBox();
            this.richTextBoxMaxLv = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBox21 = new System.Windows.Forms.RichTextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.richTextBoxNormLv = new System.Windows.Forms.RichTextBox();
            this.richTextBoxWarmLv = new System.Windows.Forms.RichTextBox();
            this.richTextBoxCoolLv = new System.Windows.Forms.RichTextBox();
            this.richTextBoxNormY = new System.Windows.Forms.RichTextBox();
            this.richTextBoxWarmY = new System.Windows.Forms.RichTextBox();
            this.richTextBoxCoolY = new System.Windows.Forms.RichTextBox();
            this.richTextBoxNormX = new System.Windows.Forms.RichTextBox();
            this.richTextBoxWarmX = new System.Windows.Forms.RichTextBox();
            this.richTextBoxCoolX = new System.Windows.Forms.RichTextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.richTextBox22 = new System.Windows.Forms.RichTextBox();
            this.richTextBox23 = new System.Windows.Forms.RichTextBox();
            this.richTextBox24 = new System.Windows.Forms.RichTextBox();
            this.richTextBox25 = new System.Windows.Forms.RichTextBox();
            this.richTextBox26 = new System.Windows.Forms.RichTextBox();
            this.richTextBox27 = new System.Windows.Forms.RichTextBox();
            this.richTextBox28 = new System.Windows.Forms.RichTextBox();
            this.richTextBox29 = new System.Windows.Forms.RichTextBox();
            this.richTextBox30 = new System.Windows.Forms.RichTextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.radioButtonGreen128No = new System.Windows.Forms.RadioButton();
            this.radioButtonGreen128Yes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBox20 = new System.Windows.Forms.RichTextBox();
            this.buttonAverage = new System.Windows.Forms.Button();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label39 = new System.Windows.Forms.Label();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.numericUpDownAverageMax = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMinusLv = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.checkBoxLimitRGB = new System.Windows.Forms.CheckBox();
            this.label36 = new System.Windows.Forms.Label();
            this.groupBoxSets = new System.Windows.Forms.GroupBox();
            this.label43 = new System.Windows.Forms.Label();
            this.numericUpDownMaxRGB = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAverageMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinusLv)).BeginInit();
            this.groupBoxSets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxRGB)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(647, 562);
            this.richTextBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox2.Multiline = false;
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(269, 30);
            this.richTextBox2.TabIndex = 4;
            this.richTextBox2.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(336, 562);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox1.Multiline = false;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(319, 30);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(186, 35);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(152, 44);
            this.buttonSave.TabIndex = 59;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(21, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Serial port";
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(91, 45);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(71, 24);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox7);
            this.groupBox1.Controls.Add(this.comboBox6);
            this.groupBox1.Controls.Add(this.comboBox4);
            this.groupBox1.Controls.Add(this.label50);
            this.groupBox1.Controls.Add(this.label49);
            this.groupBox1.Controls.Add(this.label48);
            this.groupBox1.Controls.Add(this.label47);
            this.groupBox1.Controls.Add(this.label46);
            this.groupBox1.Controls.Add(this.label45);
            this.groupBox1.Controls.Add(this.label40);
            this.groupBox1.Controls.Add(this.label44);
            this.groupBox1.Controls.Add(this.label42);
            this.groupBox1.Controls.Add(this.label35);
            this.groupBox1.Controls.Add(this.label34);
            this.groupBox1.Controls.Add(this.label33);
            this.groupBox1.Controls.Add(this.richTextBox38);
            this.groupBox1.Controls.Add(this.richTextBox37);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.richTextBoxToleranceLv);
            this.groupBox1.Controls.Add(this.label32);
            this.groupBox1.Controls.Add(this.richTextBox32);
            this.groupBox1.Location = new System.Drawing.Point(10, 216);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(300, 182);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Brightness setting  ";
            // 
            // comboBox7
            // 
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Items.AddRange(new object[] {
            "60",
            "55"});
            this.comboBox7.Location = new System.Drawing.Point(237, 146);
            this.comboBox7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(40, 23);
            this.comboBox7.TabIndex = 67;
            this.comboBox7.Text = "60";
            // 
            // comboBox6
            // 
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Items.AddRange(new object[] {
            "65",
            "60"});
            this.comboBox6.Location = new System.Drawing.Point(179, 146);
            this.comboBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(40, 23);
            this.comboBox6.TabIndex = 66;
            this.comboBox6.Text = "65";
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "65",
            "60"});
            this.comboBox4.Location = new System.Drawing.Point(119, 146);
            this.comboBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(40, 23);
            this.comboBox4.TabIndex = 42;
            this.comboBox4.Text = "65";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(14, 151);
            this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(56, 15);
            this.label50.TabIndex = 65;
            this.label50.Text = "Target Lv";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(238, 127);
            this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(40, 15);
            this.label49.TabIndex = 64;
            this.label49.Text = "Warm";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(172, 127);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(57, 15);
            this.label48.TabIndex = 63;
            this.label48.Text = "Standard";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(122, 127);
            this.label47.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(33, 15);
            this.label47.TabIndex = 62;
            this.label47.Text = "Cool";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(276, 151);
            this.label46.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(18, 15);
            this.label46.TabIndex = 61;
            this.label46.Text = "%";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(158, 151);
            this.label45.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(18, 15);
            this.label45.TabIndex = 59;
            this.label45.Text = "%";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(218, 151);
            this.label40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(18, 15);
            this.label40.TabIndex = 57;
            this.label40.Text = "%";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(14, 130);
            this.label44.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(103, 15);
            this.label44.TabIndex = 56;
            this.label44.Text = "Panasonic/Sanyo";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(270, 91);
            this.label42.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(18, 15);
            this.label42.TabIndex = 54;
            this.label42.Text = "%";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(180, 66);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(40, 15);
            this.label35.TabIndex = 53;
            this.label35.Text = "Warm";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(126, 66);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(54, 15);
            this.label34.TabIndex = 52;
            this.label34.Text = "Stardard";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(89, 66);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(33, 15);
            this.label33.TabIndex = 51;
            this.label33.Text = "Cool";
            // 
            // richTextBox38
            // 
            this.richTextBox38.Location = new System.Drawing.Point(181, 86);
            this.richTextBox38.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox38.Multiline = false;
            this.richTextBox38.Name = "richTextBox38";
            this.richTextBox38.Size = new System.Drawing.Size(35, 24);
            this.richTextBox38.TabIndex = 50;
            this.richTextBox38.Text = "";
            this.richTextBox38.TextChanged += new System.EventHandler(this.richTextBox38_TextChanged);
            // 
            // richTextBox37
            // 
            this.richTextBox37.Location = new System.Drawing.Point(134, 86);
            this.richTextBox37.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox37.Multiline = false;
            this.richTextBox37.Name = "richTextBox37";
            this.richTextBox37.Size = new System.Drawing.Size(35, 24);
            this.richTextBox37.TabIndex = 49;
            this.richTextBox37.Text = "";
            this.richTextBox37.TextChanged += new System.EventHandler(this.richTextBox37_TextChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(14, 92);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(54, 15);
            this.label25.TabIndex = 48;
            this.label25.Text = "(cd/m^2)";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(164, 30);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(101, 19);
            this.radioButton2.TabIndex = 47;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Input target Lv";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(20, 30);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(87, 19);
            this.radioButton1.TabIndex = 46;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Use Max Lv";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(229, 66);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(61, 15);
            this.label18.TabIndex = 45;
            this.label18.Text = "Tolerance";
            // 
            // richTextBoxToleranceLv
            // 
            this.richTextBoxToleranceLv.Location = new System.Drawing.Point(248, 86);
            this.richTextBoxToleranceLv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxToleranceLv.Multiline = false;
            this.richTextBoxToleranceLv.Name = "richTextBoxToleranceLv";
            this.richTextBoxToleranceLv.Size = new System.Drawing.Size(21, 24);
            this.richTextBoxToleranceLv.TabIndex = 44;
            this.richTextBoxToleranceLv.Text = "";
            this.richTextBoxToleranceLv.TextChanged += new System.EventHandler(this.richTextBox33_TextChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(14, 70);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(56, 15);
            this.label32.TabIndex = 39;
            this.label32.Text = "Target Lv";
            // 
            // richTextBox32
            // 
            this.richTextBox32.Location = new System.Drawing.Point(88, 86);
            this.richTextBox32.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox32.Multiline = false;
            this.richTextBox32.Name = "richTextBox32";
            this.richTextBox32.Size = new System.Drawing.Size(35, 24);
            this.richTextBox32.TabIndex = 36;
            this.richTextBox32.Text = "";
            this.richTextBox32.TextChanged += new System.EventHandler(this.richTextBox32_TextChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(38, 152);
            this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(74, 15);
            this.label31.TabIndex = 40;
            this.label31.Text = "xy Tolerance";
            // 
            // buttonConnectUSB
            // 
            this.buttonConnectUSB.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnectUSB.Location = new System.Drawing.Point(186, 95);
            this.buttonConnectUSB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonConnectUSB.Name = "buttonConnectUSB";
            this.buttonConnectUSB.Size = new System.Drawing.Size(151, 44);
            this.buttonConnectUSB.TabIndex = 46;
            this.buttonConnectUSB.Text = "Connect CA210";
            this.buttonConnectUSB.UseVisualStyleBackColor = true;
            this.buttonConnectUSB.Click += new System.EventHandler(this.buttonConnectUSB_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Enabled = false;
            this.buttonStart.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(384, 320);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(120, 51);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label41);
            this.groupBox5.Controls.Add(this.comboBox3);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.comboBox1);
            this.groupBox5.Controls.Add(this.buttonConnectUSB);
            this.groupBox5.Controls.Add(this.buttonSave);
            this.groupBox5.Location = new System.Drawing.Point(686, 45);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox5.Size = new System.Drawing.Size(354, 160);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Device setting ";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(22, 111);
            this.label41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(57, 15);
            this.label41.TabIndex = 61;
            this.label41.Text = "Baudrate";
            // 
            // comboBox3
            // 
            this.comboBox3.Enabled = false;
            this.comboBox3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "115200",
            "128000"});
            this.comboBox3.Location = new System.Drawing.Point(82, 106);
            this.comboBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(87, 24);
            this.comboBox3.TabIndex = 60;
            this.comboBox3.Text = "115200";
            this.comboBox3.TextChanged += new System.EventHandler(this.comboBox3_TextChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.comboBoxToleranceXY);
            this.groupBox6.Controls.Add(this.label31);
            this.groupBox6.Controls.Add(this.label30);
            this.groupBox6.Controls.Add(this.richTextBox9);
            this.groupBox6.Controls.Add(this.richTextBox8);
            this.groupBox6.Controls.Add(this.richTextBox7);
            this.groupBox6.Controls.Add(this.richTextBox6);
            this.groupBox6.Controls.Add(this.richTextBox5);
            this.groupBox6.Controls.Add(this.richTextBox4);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.comboBoxXYPreset);
            this.groupBox6.Location = new System.Drawing.Point(10, 412);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox6.Size = new System.Drawing.Size(300, 185);
            this.groupBox6.TabIndex = 21;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = " Color temp setting ";
            // 
            // comboBoxToleranceXY
            // 
            this.comboBoxToleranceXY.FormattingEnabled = true;
            this.comboBoxToleranceXY.Items.AddRange(new object[] {
            "0.005",
            "0.010"});
            this.comboBoxToleranceXY.Location = new System.Drawing.Point(125, 149);
            this.comboBoxToleranceXY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBoxToleranceXY.Name = "comboBoxToleranceXY";
            this.comboBoxToleranceXY.Size = new System.Drawing.Size(77, 23);
            this.comboBoxToleranceXY.TabIndex = 41;
            this.comboBoxToleranceXY.TextChanged += new System.EventHandler(this.comboBox2_TextChanged);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(14, 28);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(41, 15);
            this.label30.TabIndex = 12;
            this.label30.Text = "Select";
            // 
            // richTextBox9
            // 
            this.richTextBox9.Location = new System.Drawing.Point(129, 110);
            this.richTextBox9.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox9.Multiline = false;
            this.richTextBox9.Name = "richTextBox9";
            this.richTextBox9.ReadOnly = true;
            this.richTextBox9.Size = new System.Drawing.Size(64, 24);
            this.richTextBox9.TabIndex = 11;
            this.richTextBox9.Text = "";
            this.richTextBox9.TextChanged += new System.EventHandler(this.richTextBox9_TextChanged);
            // 
            // richTextBox8
            // 
            this.richTextBox8.Location = new System.Drawing.Point(129, 78);
            this.richTextBox8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox8.Multiline = false;
            this.richTextBox8.Name = "richTextBox8";
            this.richTextBox8.ReadOnly = true;
            this.richTextBox8.Size = new System.Drawing.Size(64, 24);
            this.richTextBox8.TabIndex = 10;
            this.richTextBox8.Text = "";
            this.richTextBox8.TextChanged += new System.EventHandler(this.richTextBox8_TextChanged);
            // 
            // richTextBox7
            // 
            this.richTextBox7.Location = new System.Drawing.Point(211, 110);
            this.richTextBox7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox7.Multiline = false;
            this.richTextBox7.Name = "richTextBox7";
            this.richTextBox7.ReadOnly = true;
            this.richTextBox7.Size = new System.Drawing.Size(64, 24);
            this.richTextBox7.TabIndex = 9;
            this.richTextBox7.Text = "";
            this.richTextBox7.TextChanged += new System.EventHandler(this.richTextBox7_TextChanged);
            // 
            // richTextBox6
            // 
            this.richTextBox6.Location = new System.Drawing.Point(211, 78);
            this.richTextBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox6.Multiline = false;
            this.richTextBox6.Name = "richTextBox6";
            this.richTextBox6.ReadOnly = true;
            this.richTextBox6.Size = new System.Drawing.Size(64, 24);
            this.richTextBox6.TabIndex = 8;
            this.richTextBox6.Text = "";
            this.richTextBox6.TextChanged += new System.EventHandler(this.richTextBox6_TextChanged);
            // 
            // richTextBox5
            // 
            this.richTextBox5.Location = new System.Drawing.Point(41, 110);
            this.richTextBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox5.Multiline = false;
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.ReadOnly = true;
            this.richTextBox5.Size = new System.Drawing.Size(64, 24);
            this.richTextBox5.TabIndex = 7;
            this.richTextBox5.Text = "";
            this.richTextBox5.TextChanged += new System.EventHandler(this.richTextBox5_TextChanged);
            // 
            // richTextBox4
            // 
            this.richTextBox4.Location = new System.Drawing.Point(41, 78);
            this.richTextBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox4.Multiline = false;
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.ReadOnly = true;
            this.richTextBox4.Size = new System.Drawing.Size(64, 24);
            this.richTextBox4.TabIndex = 6;
            this.richTextBox4.Text = "";
            this.richTextBox4.TextChanged += new System.EventHandler(this.richTextBox4_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 114);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 15);
            this.label10.TabIndex = 5;
            this.label10.Text = "Y";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 82);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 15);
            this.label9.TabIndex = 4;
            this.label9.Text = "X";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(132, 59);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 15);
            this.label8.TabIndex = 3;
            this.label8.Text = "Standard";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(223, 59);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 15);
            this.label7.TabIndex = 2;
            this.label7.Text = "Warm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(55, 59);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "Cool";
            // 
            // comboBoxXYPreset
            // 
            this.comboBoxXYPreset.FormattingEnabled = true;
            this.comboBoxXYPreset.Items.AddRange(new object[] {
            "KTC/TCL_26inches_above",
            "KTC/TCL_26inches_below",
            "BNC",
            "BenQ_LED",
            "ROWA",
            "Leader/Shownic",
            "BOE",
            "Hyundai",
            "Daewoo",
            "SINGSUNG_26inches_above",
            "SINGSUNG_26inches_below",
            "PHILIPS_export",
            "PHILIPS_32inches_above",
            "PHILIPS_32inches_below",
            "PANDA",
            "Panasonic/Sanyo",
            "User_defined_color_temp"});
            this.comboBoxXYPreset.Location = new System.Drawing.Point(77, 22);
            this.comboBoxXYPreset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBoxXYPreset.Name = "comboBoxXYPreset";
            this.comboBoxXYPreset.Size = new System.Drawing.Size(208, 23);
            this.comboBoxXYPreset.TabIndex = 0;
            this.comboBoxXYPreset.TextChanged += new System.EventHandler(this.comboBox5_TextChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.labelMinusLv);
            this.groupBox7.Controls.Add(this.label37);
            this.groupBox7.Controls.Add(this.richTextBoxG);
            this.groupBox7.Controls.Add(this.label23);
            this.groupBox7.Controls.Add(this.richTextBoxWarmLvPercent);
            this.groupBox7.Controls.Add(this.label19);
            this.groupBox7.Controls.Add(this.richTextBoxNormLvPercent);
            this.groupBox7.Controls.Add(this.label5);
            this.groupBox7.Controls.Add(this.richTextBoxCoolLvPercent);
            this.groupBox7.Controls.Add(this.richTextBoxMaxLv);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Controls.Add(this.richTextBox21);
            this.groupBox7.Controls.Add(this.label17);
            this.groupBox7.Controls.Add(this.label16);
            this.groupBox7.Controls.Add(this.label15);
            this.groupBox7.Controls.Add(this.label14);
            this.groupBox7.Controls.Add(this.label13);
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.richTextBoxNormLv);
            this.groupBox7.Controls.Add(this.richTextBoxWarmLv);
            this.groupBox7.Controls.Add(this.richTextBoxCoolLv);
            this.groupBox7.Controls.Add(this.richTextBoxNormY);
            this.groupBox7.Controls.Add(this.richTextBoxWarmY);
            this.groupBox7.Controls.Add(this.richTextBoxCoolY);
            this.groupBox7.Controls.Add(this.richTextBoxNormX);
            this.groupBox7.Controls.Add(this.richTextBoxWarmX);
            this.groupBox7.Controls.Add(this.richTextBoxCoolX);
            this.groupBox7.Location = new System.Drawing.Point(582, 215);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox7.Size = new System.Drawing.Size(459, 282);
            this.groupBox7.TabIndex = 22;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = " Result ";
            // 
            // labelMinusLv
            // 
            this.labelMinusLv.AutoSize = true;
            this.labelMinusLv.Location = new System.Drawing.Point(184, 174);
            this.labelMinusLv.Name = "labelMinusLv";
            this.labelMinusLv.Size = new System.Drawing.Size(0, 15);
            this.labelMinusLv.TabIndex = 36;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(354, 224);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(16, 15);
            this.label37.TabIndex = 35;
            this.label37.Text = "G";
            // 
            // richTextBoxG
            // 
            this.richTextBoxG.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBoxG.Location = new System.Drawing.Point(372, 209);
            this.richTextBoxG.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxG.Multiline = false;
            this.richTextBoxG.Name = "richTextBoxG";
            this.richTextBoxG.ReadOnly = true;
            this.richTextBoxG.Size = new System.Drawing.Size(54, 43);
            this.richTextBoxG.TabIndex = 34;
            this.richTextBoxG.Text = "";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(241, 224);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(40, 15);
            this.label23.TabIndex = 33;
            this.label23.Text = "Warm";
            // 
            // richTextBoxWarmLvPercent
            // 
            this.richTextBoxWarmLvPercent.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxWarmLvPercent.Location = new System.Drawing.Point(284, 209);
            this.richTextBoxWarmLvPercent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxWarmLvPercent.Multiline = false;
            this.richTextBoxWarmLvPercent.Name = "richTextBoxWarmLvPercent";
            this.richTextBoxWarmLvPercent.ReadOnly = true;
            this.richTextBoxWarmLvPercent.Size = new System.Drawing.Size(50, 43);
            this.richTextBoxWarmLvPercent.TabIndex = 32;
            this.richTextBoxWarmLvPercent.Text = "";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(117, 224);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(57, 15);
            this.label19.TabIndex = 31;
            this.label19.Text = "Standard";
            // 
            // richTextBoxNormLvPercent
            // 
            this.richTextBoxNormLvPercent.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxNormLvPercent.Location = new System.Drawing.Point(174, 209);
            this.richTextBoxNormLvPercent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxNormLvPercent.Multiline = false;
            this.richTextBoxNormLvPercent.Name = "richTextBoxNormLvPercent";
            this.richTextBoxNormLvPercent.ReadOnly = true;
            this.richTextBoxNormLvPercent.Size = new System.Drawing.Size(50, 43);
            this.richTextBoxNormLvPercent.TabIndex = 30;
            this.richTextBoxNormLvPercent.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 224);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 15);
            this.label5.TabIndex = 29;
            this.label5.Text = "Cool";
            // 
            // richTextBoxCoolLvPercent
            // 
            this.richTextBoxCoolLvPercent.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxCoolLvPercent.Location = new System.Drawing.Point(60, 209);
            this.richTextBoxCoolLvPercent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxCoolLvPercent.Multiline = false;
            this.richTextBoxCoolLvPercent.Name = "richTextBoxCoolLvPercent";
            this.richTextBoxCoolLvPercent.ReadOnly = true;
            this.richTextBoxCoolLvPercent.Size = new System.Drawing.Size(52, 43);
            this.richTextBoxCoolLvPercent.TabIndex = 28;
            this.richTextBoxCoolLvPercent.Text = "";
            // 
            // richTextBoxMaxLv
            // 
            this.richTextBoxMaxLv.Location = new System.Drawing.Point(117, 171);
            this.richTextBoxMaxLv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxMaxLv.Multiline = false;
            this.richTextBoxMaxLv.Name = "richTextBoxMaxLv";
            this.richTextBoxMaxLv.ReadOnly = true;
            this.richTextBoxMaxLv.Size = new System.Drawing.Size(60, 24);
            this.richTextBoxMaxLv.TabIndex = 27;
            this.richTextBoxMaxLv.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 176);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 15);
            this.label4.TabIndex = 26;
            this.label4.Text = "Max panel Lv ";
            // 
            // richTextBox21
            // 
            this.richTextBox21.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox21.ForeColor = System.Drawing.Color.Red;
            this.richTextBox21.Location = new System.Drawing.Point(272, 76);
            this.richTextBox21.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox21.Multiline = false;
            this.richTextBox21.Name = "richTextBox21";
            this.richTextBox21.ReadOnly = true;
            this.richTextBox21.Size = new System.Drawing.Size(154, 99);
            this.richTextBox21.TabIndex = 25;
            this.richTextBox21.Text = "";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(315, 43);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 19);
            this.label17.TabIndex = 22;
            this.label17.Text = "Result";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(132, 33);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(57, 15);
            this.label16.TabIndex = 21;
            this.label16.Text = "Standard";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(203, 33);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 15);
            this.label15.TabIndex = 20;
            this.label15.Text = "Warm";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(62, 33);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 15);
            this.label14.TabIndex = 19;
            this.label14.Text = "Cool";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(22, 135);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(19, 15);
            this.label13.TabIndex = 18;
            this.label13.Text = "Lv";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 99);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(12, 15);
            this.label12.TabIndex = 17;
            this.label12.Text = "y";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 62);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(12, 15);
            this.label11.TabIndex = 16;
            this.label11.Text = "x";
            // 
            // richTextBoxNormLv
            // 
            this.richTextBoxNormLv.Location = new System.Drawing.Point(122, 131);
            this.richTextBoxNormLv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxNormLv.Multiline = false;
            this.richTextBoxNormLv.Name = "richTextBoxNormLv";
            this.richTextBoxNormLv.ReadOnly = true;
            this.richTextBoxNormLv.Size = new System.Drawing.Size(52, 24);
            this.richTextBoxNormLv.TabIndex = 15;
            this.richTextBoxNormLv.Text = "";
            // 
            // richTextBoxWarmLv
            // 
            this.richTextBoxWarmLv.Location = new System.Drawing.Point(192, 131);
            this.richTextBoxWarmLv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxWarmLv.Multiline = false;
            this.richTextBoxWarmLv.Name = "richTextBoxWarmLv";
            this.richTextBoxWarmLv.ReadOnly = true;
            this.richTextBoxWarmLv.Size = new System.Drawing.Size(52, 24);
            this.richTextBoxWarmLv.TabIndex = 14;
            this.richTextBoxWarmLv.Text = "";
            // 
            // richTextBoxCoolLv
            // 
            this.richTextBoxCoolLv.Location = new System.Drawing.Point(52, 131);
            this.richTextBoxCoolLv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxCoolLv.Multiline = false;
            this.richTextBoxCoolLv.Name = "richTextBoxCoolLv";
            this.richTextBoxCoolLv.ReadOnly = true;
            this.richTextBoxCoolLv.Size = new System.Drawing.Size(52, 24);
            this.richTextBoxCoolLv.TabIndex = 13;
            this.richTextBoxCoolLv.Text = "";
            // 
            // richTextBoxNormY
            // 
            this.richTextBoxNormY.Location = new System.Drawing.Point(122, 95);
            this.richTextBoxNormY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxNormY.Multiline = false;
            this.richTextBoxNormY.Name = "richTextBoxNormY";
            this.richTextBoxNormY.ReadOnly = true;
            this.richTextBoxNormY.Size = new System.Drawing.Size(52, 24);
            this.richTextBoxNormY.TabIndex = 12;
            this.richTextBoxNormY.Text = "";
            // 
            // richTextBoxWarmY
            // 
            this.richTextBoxWarmY.Location = new System.Drawing.Point(192, 96);
            this.richTextBoxWarmY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxWarmY.Multiline = false;
            this.richTextBoxWarmY.Name = "richTextBoxWarmY";
            this.richTextBoxWarmY.ReadOnly = true;
            this.richTextBoxWarmY.Size = new System.Drawing.Size(52, 24);
            this.richTextBoxWarmY.TabIndex = 11;
            this.richTextBoxWarmY.Text = "";
            // 
            // richTextBoxCoolY
            // 
            this.richTextBoxCoolY.Location = new System.Drawing.Point(52, 95);
            this.richTextBoxCoolY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxCoolY.Multiline = false;
            this.richTextBoxCoolY.Name = "richTextBoxCoolY";
            this.richTextBoxCoolY.ReadOnly = true;
            this.richTextBoxCoolY.Size = new System.Drawing.Size(52, 24);
            this.richTextBoxCoolY.TabIndex = 10;
            this.richTextBoxCoolY.Text = "";
            // 
            // richTextBoxNormX
            // 
            this.richTextBoxNormX.Location = new System.Drawing.Point(122, 59);
            this.richTextBoxNormX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxNormX.Multiline = false;
            this.richTextBoxNormX.Name = "richTextBoxNormX";
            this.richTextBoxNormX.ReadOnly = true;
            this.richTextBoxNormX.Size = new System.Drawing.Size(52, 24);
            this.richTextBoxNormX.TabIndex = 9;
            this.richTextBoxNormX.Text = "";
            // 
            // richTextBoxWarmX
            // 
            this.richTextBoxWarmX.Location = new System.Drawing.Point(192, 59);
            this.richTextBoxWarmX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxWarmX.Multiline = false;
            this.richTextBoxWarmX.Name = "richTextBoxWarmX";
            this.richTextBoxWarmX.ReadOnly = true;
            this.richTextBoxWarmX.Size = new System.Drawing.Size(52, 24);
            this.richTextBoxWarmX.TabIndex = 8;
            this.richTextBoxWarmX.Text = "";
            // 
            // richTextBoxCoolX
            // 
            this.richTextBoxCoolX.Location = new System.Drawing.Point(52, 59);
            this.richTextBoxCoolX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxCoolX.Multiline = false;
            this.richTextBoxCoolX.Name = "richTextBoxCoolX";
            this.richTextBoxCoolX.ReadOnly = true;
            this.richTextBoxCoolX.Size = new System.Drawing.Size(52, 24);
            this.richTextBoxCoolX.TabIndex = 7;
            this.richTextBoxCoolX.Text = "";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label21);
            this.groupBox8.Controls.Add(this.label22);
            this.groupBox8.Controls.Add(this.label26);
            this.groupBox8.Controls.Add(this.label27);
            this.groupBox8.Controls.Add(this.label28);
            this.groupBox8.Controls.Add(this.label29);
            this.groupBox8.Controls.Add(this.richTextBox22);
            this.groupBox8.Controls.Add(this.richTextBox23);
            this.groupBox8.Controls.Add(this.richTextBox24);
            this.groupBox8.Controls.Add(this.richTextBox25);
            this.groupBox8.Controls.Add(this.richTextBox26);
            this.groupBox8.Controls.Add(this.richTextBox27);
            this.groupBox8.Controls.Add(this.richTextBox28);
            this.groupBox8.Controls.Add(this.richTextBox29);
            this.groupBox8.Controls.Add(this.richTextBox30);
            this.groupBox8.Location = new System.Drawing.Point(10, 45);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox8.Size = new System.Drawing.Size(242, 160);
            this.groupBox8.TabIndex = 23;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = " Initial setting ";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(100, 26);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(57, 15);
            this.label21.TabIndex = 36;
            this.label21.Text = "Standard";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(176, 26);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(40, 15);
            this.label22.TabIndex = 35;
            this.label22.Text = "Warm";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(49, 26);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(33, 15);
            this.label26.TabIndex = 34;
            this.label26.Text = "Cool";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(18, 123);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(15, 15);
            this.label27.TabIndex = 33;
            this.label27.Text = "B";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(18, 90);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(16, 15);
            this.label28.TabIndex = 32;
            this.label28.Text = "G";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(18, 55);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(16, 15);
            this.label29.TabIndex = 31;
            this.label29.Text = "R";
            // 
            // richTextBox22
            // 
            this.richTextBox22.Location = new System.Drawing.Point(164, 119);
            this.richTextBox22.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox22.Multiline = false;
            this.richTextBox22.Name = "richTextBox22";
            this.richTextBox22.ReadOnly = true;
            this.richTextBox22.Size = new System.Drawing.Size(54, 24);
            this.richTextBox22.TabIndex = 30;
            this.richTextBox22.Text = "";
            // 
            // richTextBox23
            // 
            this.richTextBox23.Location = new System.Drawing.Point(101, 119);
            this.richTextBox23.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox23.Multiline = false;
            this.richTextBox23.Name = "richTextBox23";
            this.richTextBox23.ReadOnly = true;
            this.richTextBox23.Size = new System.Drawing.Size(54, 24);
            this.richTextBox23.TabIndex = 29;
            this.richTextBox23.Text = "";
            // 
            // richTextBox24
            // 
            this.richTextBox24.Location = new System.Drawing.Point(38, 119);
            this.richTextBox24.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox24.Multiline = false;
            this.richTextBox24.Name = "richTextBox24";
            this.richTextBox24.ReadOnly = true;
            this.richTextBox24.Size = new System.Drawing.Size(54, 24);
            this.richTextBox24.TabIndex = 28;
            this.richTextBox24.Text = "";
            // 
            // richTextBox25
            // 
            this.richTextBox25.Location = new System.Drawing.Point(164, 85);
            this.richTextBox25.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox25.Multiline = false;
            this.richTextBox25.Name = "richTextBox25";
            this.richTextBox25.ReadOnly = true;
            this.richTextBox25.Size = new System.Drawing.Size(54, 24);
            this.richTextBox25.TabIndex = 27;
            this.richTextBox25.Text = "";
            // 
            // richTextBox26
            // 
            this.richTextBox26.Location = new System.Drawing.Point(101, 84);
            this.richTextBox26.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox26.Multiline = false;
            this.richTextBox26.Name = "richTextBox26";
            this.richTextBox26.ReadOnly = true;
            this.richTextBox26.Size = new System.Drawing.Size(54, 24);
            this.richTextBox26.TabIndex = 26;
            this.richTextBox26.Text = "";
            // 
            // richTextBox27
            // 
            this.richTextBox27.Location = new System.Drawing.Point(38, 84);
            this.richTextBox27.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox27.Multiline = false;
            this.richTextBox27.Name = "richTextBox27";
            this.richTextBox27.ReadOnly = true;
            this.richTextBox27.Size = new System.Drawing.Size(54, 24);
            this.richTextBox27.TabIndex = 25;
            this.richTextBox27.Text = "";
            // 
            // richTextBox28
            // 
            this.richTextBox28.Location = new System.Drawing.Point(164, 50);
            this.richTextBox28.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox28.Multiline = false;
            this.richTextBox28.Name = "richTextBox28";
            this.richTextBox28.ReadOnly = true;
            this.richTextBox28.Size = new System.Drawing.Size(54, 24);
            this.richTextBox28.TabIndex = 24;
            this.richTextBox28.Text = "";
            // 
            // richTextBox29
            // 
            this.richTextBox29.Location = new System.Drawing.Point(101, 50);
            this.richTextBox29.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox29.Multiline = false;
            this.richTextBox29.Name = "richTextBox29";
            this.richTextBox29.ReadOnly = true;
            this.richTextBox29.Size = new System.Drawing.Size(54, 24);
            this.richTextBox29.TabIndex = 23;
            this.richTextBox29.Text = "";
            // 
            // richTextBox30
            // 
            this.richTextBox30.Location = new System.Drawing.Point(38, 50);
            this.richTextBox30.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox30.Multiline = false;
            this.richTextBox30.Name = "richTextBox30";
            this.richTextBox30.ReadOnly = true;
            this.richTextBox30.Size = new System.Drawing.Size(54, 24);
            this.richTextBox30.TabIndex = 22;
            this.richTextBox30.Text = "";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(15, 18);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(93, 15);
            this.label38.TabIndex = 56;
            this.label38.Text = "Enable G<128?";
            // 
            // radioButtonGreen128No
            // 
            this.radioButtonGreen128No.AutoSize = true;
            this.radioButtonGreen128No.Location = new System.Drawing.Point(182, 16);
            this.radioButtonGreen128No.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButtonGreen128No.Name = "radioButtonGreen128No";
            this.radioButtonGreen128No.Size = new System.Drawing.Size(41, 19);
            this.radioButtonGreen128No.TabIndex = 55;
            this.radioButtonGreen128No.TabStop = true;
            this.radioButtonGreen128No.Text = "No";
            this.radioButtonGreen128No.UseVisualStyleBackColor = true;
            this.radioButtonGreen128No.CheckedChanged += new System.EventHandler(this.radioButtonGreen128No_CheckedChanged);
            // 
            // radioButtonGreen128Yes
            // 
            this.radioButtonGreen128Yes.AutoSize = true;
            this.radioButtonGreen128Yes.Location = new System.Drawing.Point(132, 16);
            this.radioButtonGreen128Yes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButtonGreen128Yes.Name = "radioButtonGreen128Yes";
            this.radioButtonGreen128Yes.Size = new System.Drawing.Size(45, 19);
            this.radioButtonGreen128Yes.TabIndex = 54;
            this.radioButtonGreen128Yes.TabStop = true;
            this.radioButtonGreen128Yes.Text = "Yes";
            this.radioButtonGreen128Yes.UseVisualStyleBackColor = true;
            this.radioButtonGreen128Yes.CheckedChanged += new System.EventHandler(this.radioButtonGreen128Yes_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(325, -3);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(342, 42);
            this.label1.TabIndex = 26;
            this.label1.Text = "Minolta White Balance";
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Location = new System.Drawing.Point(350, 536);
            this.labelError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(530, 15);
            this.labelError.TabIndex = 24;
            this.labelError.Text = "Open serial port and connect CA210, then click \"start\" or press Spacebar / Enter " +
    "on the keyboard";
            this.labelError.Visible = false;
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(917, 562);
            this.richTextBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox3.Multiline = false;
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(124, 30);
            this.richTextBox3.TabIndex = 27;
            this.richTextBox3.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.richTextBox20);
            this.groupBox2.Controls.Add(this.buttonAverage);
            this.groupBox2.Controls.Add(this.buttonDefault);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(327, 45);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(232, 179);
            this.groupBox2.TabIndex = 60;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Data saving ";
            // 
            // richTextBox20
            // 
            this.richTextBox20.Location = new System.Drawing.Point(136, 60);
            this.richTextBox20.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox20.Multiline = false;
            this.richTextBox20.Name = "richTextBox20";
            this.richTextBox20.ReadOnly = true;
            this.richTextBox20.Size = new System.Drawing.Size(61, 26);
            this.richTextBox20.TabIndex = 28;
            this.richTextBox20.Text = "";
            // 
            // buttonAverage
            // 
            this.buttonAverage.Enabled = false;
            this.buttonAverage.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAverage.Location = new System.Drawing.Point(125, 112);
            this.buttonAverage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonAverage.Name = "buttonAverage";
            this.buttonAverage.Size = new System.Drawing.Size(91, 36);
            this.buttonAverage.TabIndex = 64;
            this.buttonAverage.Text = "Average";
            this.buttonAverage.UseVisualStyleBackColor = true;
            this.buttonAverage.Click += new System.EventHandler(this.buttonAverage_Click);
            // 
            // buttonDefault
            // 
            this.buttonDefault.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDefault.Location = new System.Drawing.Point(20, 112);
            this.buttonDefault.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(91, 36);
            this.buttonDefault.TabIndex = 63;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(130, 35);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(87, 15);
            this.label20.TabIndex = 60;
            this.label20.Text = "Saved number";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(25, 32);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 55);
            this.button3.TabIndex = 59;
            this.button3.Text = "Clear saved data";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(15, 20);
            this.label39.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(110, 15);
            this.label39.TabIndex = 57;
            this.label39.Text = "Return command?";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(132, 17);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(45, 19);
            this.radioButton5.TabIndex = 58;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Yes";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(182, 17);
            this.radioButton6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(41, 19);
            this.radioButton6.TabIndex = 57;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "No";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label39);
            this.groupBox3.Controls.Add(this.radioButton5);
            this.groupBox3.Controls.Add(this.radioButton6);
            this.groupBox3.Location = new System.Drawing.Point(327, 226);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(232, 43);
            this.groupBox3.TabIndex = 54;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton7);
            this.groupBox4.Controls.Add(this.radioButton8);
            this.groupBox4.Location = new System.Drawing.Point(577, 45);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Size = new System.Drawing.Size(97, 103);
            this.groupBox4.TabIndex = 62;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Target";
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(13, 31);
            this.radioButton7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(39, 19);
            this.radioButton7.TabIndex = 63;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "TV";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.CheckedChanged += new System.EventHandler(this.radioButton7_CheckedChanged);
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(13, 60);
            this.radioButton8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(65, 19);
            this.radioButton8.TabIndex = 62;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Monitor";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label38);
            this.groupBox10.Controls.Add(this.radioButtonGreen128No);
            this.groupBox10.Controls.Add(this.radioButtonGreen128Yes);
            this.groupBox10.Location = new System.Drawing.Point(327, 269);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox10.Size = new System.Drawing.Size(232, 43);
            this.groupBox10.TabIndex = 64;
            this.groupBox10.TabStop = false;
            // 
            // numericUpDownAverageMax
            // 
            this.numericUpDownAverageMax.Location = new System.Drawing.Point(109, 19);
            this.numericUpDownAverageMax.Name = "numericUpDownAverageMax";
            this.numericUpDownAverageMax.Size = new System.Drawing.Size(42, 21);
            this.numericUpDownAverageMax.TabIndex = 65;
            // 
            // numericUpDownMinusLv
            // 
            this.numericUpDownMinusLv.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMinusLv.Location = new System.Drawing.Point(109, 45);
            this.numericUpDownMinusLv.Name = "numericUpDownMinusLv";
            this.numericUpDownMinusLv.Size = new System.Drawing.Size(42, 21);
            this.numericUpDownMinusLv.TabIndex = 66;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 15);
            this.label2.TabIndex = 67;
            this.label2.Text = "Average Number";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(8, 47);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(79, 15);
            this.label24.TabIndex = 68;
            this.label24.Text = "Minus Max Lv";
            // 
            // checkBoxLimitRGB
            // 
            this.checkBoxLimitRGB.AutoSize = true;
            this.checkBoxLimitRGB.Location = new System.Drawing.Point(419, 510);
            this.checkBoxLimitRGB.Name = "checkBoxLimitRGB";
            this.checkBoxLimitRGB.Size = new System.Drawing.Size(15, 14);
            this.checkBoxLimitRGB.TabIndex = 69;
            this.checkBoxLimitRGB.UseVisualStyleBackColor = true;
            this.checkBoxLimitRGB.Visible = false;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(350, 509);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(63, 15);
            this.label36.TabIndex = 70;
            this.label36.Text = "Limit RGB";
            this.label36.Visible = false;
            // 
            // groupBoxSets
            // 
            this.groupBoxSets.Controls.Add(this.label43);
            this.groupBoxSets.Controls.Add(this.numericUpDownMaxRGB);
            this.groupBoxSets.Controls.Add(this.label24);
            this.groupBoxSets.Controls.Add(this.label2);
            this.groupBoxSets.Controls.Add(this.numericUpDownMinusLv);
            this.groupBoxSets.Controls.Add(this.numericUpDownAverageMax);
            this.groupBoxSets.Location = new System.Drawing.Point(341, 397);
            this.groupBoxSets.Name = "groupBoxSets";
            this.groupBoxSets.Size = new System.Drawing.Size(192, 107);
            this.groupBoxSets.TabIndex = 71;
            this.groupBoxSets.TabStop = false;
            this.groupBoxSets.Text = "Настройки";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(9, 72);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(57, 15);
            this.label43.TabIndex = 70;
            this.label43.Text = "Max RGB";
            // 
            // numericUpDownMaxRGB
            // 
            this.numericUpDownMaxRGB.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownMaxRGB.Location = new System.Drawing.Point(97, 70);
            this.numericUpDownMaxRGB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMaxRGB.Name = "numericUpDownMaxRGB";
            this.numericUpDownMaxRGB.Size = new System.Drawing.Size(54, 21);
            this.numericUpDownMaxRGB.TabIndex = 69;
            this.numericUpDownMaxRGB.Click += new System.EventHandler(this.numericUpDownMaxRGB_Click);
            // 
            // WhiteBalanceMinolta
            // 
            this.ClientSize = new System.Drawing.Size(1050, 598);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.checkBoxLimitRGB);
            this.Controls.Add(this.groupBoxSets);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "WhiteBalanceMinolta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Horizont TV WBAA Tool V8.1 Minolta";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorSystemTools_FormClosing);
            this.Load += new System.EventHandler(this.ColorSystemTools_Load);
            this.Shown += new System.EventHandler(this.ColorSystemTools_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ColorSystemTools_KeyPress);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAverageMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinusLv)).EndInit();
            this.groupBoxSets.ResumeLayout(false);
            this.groupBoxSets.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxRGB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        //private IContainer components;        
        private RichTextBox richTextBox2;
        private RichTextBox richTextBox1;
        private Button buttonSave;
        private Label label3;
        private ComboBox comboBox1;
        private GroupBox groupBox1;
        private Button buttonConnectUSB;
        private Button buttonStart;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private GroupBox groupBox8;
        private RichTextBox richTextBox9;
        private RichTextBox richTextBox8;
        private RichTextBox richTextBox7;
        private RichTextBox richTextBox6;
        private RichTextBox richTextBox5;
        private RichTextBox richTextBox4;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private ComboBox comboBoxXYPreset;
        private RichTextBox richTextBoxCoolX;
        private RichTextBox richTextBox21;
        private Label label17;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label label11;
        private RichTextBox richTextBoxNormLv;
        private RichTextBox richTextBoxWarmLv;
        private RichTextBox richTextBoxCoolLv;
        private RichTextBox richTextBoxNormY;
        private RichTextBox richTextBoxWarmY;
        private RichTextBox richTextBoxCoolY;
        private RichTextBox richTextBoxNormX;
        private RichTextBox richTextBoxWarmX;
        private Label label21;
        private Label label22;
        private Label label26;
        private Label label27;
        private Label label28;
        private Label label29;
        private RichTextBox richTextBox22;
        private RichTextBox richTextBox23;
        private RichTextBox richTextBox24;
        private RichTextBox richTextBox25;
        private RichTextBox richTextBox26;
        private RichTextBox richTextBox27;
        private RichTextBox richTextBox28;
        private RichTextBox richTextBox29;
        private RichTextBox richTextBox30;
        private Label label31;
        private Label label32;
        private RichTextBox richTextBox32;
        private Label label30;
        private Label label1;
        private Label labelError;
        private RichTextBox richTextBox3;
        private RichTextBox richTextBoxMaxLv;
        private Label label4;
        private GroupBox groupBox2;
        private Button button3;
        private Button buttonAverage;
        private Button buttonDefault;
        private Label label20;
        private RichTextBox richTextBox20;
        private Label label18;
        private RichTextBox richTextBoxToleranceLv;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private RichTextBox richTextBoxCoolLvPercent;
        private Label label23;
        private RichTextBox richTextBoxWarmLvPercent;
        private Label label19;
        private RichTextBox richTextBoxNormLvPercent;
        private Label label5;
        private Label label25;
        private RichTextBox richTextBox38;
        private RichTextBox richTextBox37;
        private Label label35;
        private Label label33;
        private Label label37;
        private RichTextBox richTextBoxG;
        private RadioButton radioButtonGreen128Yes;
        private RadioButton radioButtonGreen128No;
        private Label label38;
        private ComboBox comboBoxToleranceXY;
        private RadioButton radioButton5;
        private RadioButton radioButton6;
        private Label label39;
        private GroupBox groupBox3;
        private Label label41;
        private ComboBox comboBox3;
        private Label label42;
        private GroupBox groupBox4;
        private RadioButton radioButton7;
        private RadioButton radioButton8;
        private Label label40;
        private Label label44;
        private GroupBox groupBox10;
        private Label label49;
        private Label label48;
        private Label label47;
        private Label label46;
        private Label label45;
        private Label label34;
        private ComboBox comboBox7;
        private ComboBox comboBox6;
        private ComboBox comboBox4;
        private Label label50;
        private NumericUpDown numericUpDownAverageMax;
        private NumericUpDown numericUpDownMinusLv;
        private Label label2;
        private Label label24;
        private CheckBox checkBoxLimitRGB;
        private Label label36;
        private Label labelMinusLv;
        private GroupBox groupBoxSets;
        private Label label43;
        private NumericUpDown numericUpDownMaxRGB;
    }
}
