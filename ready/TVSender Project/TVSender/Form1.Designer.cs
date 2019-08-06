namespace TVSender
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.дубликатыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemEditTestF4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemПапкаВнешнихТестовMain = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemВнешниеТесты = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemВстроенныеТестыMain = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаF10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemУправление = new System.Windows.Forms.ToolStripMenuItem();
            this.записьПрошивокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemВстроенныеТестыСправка = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemКомандыВнешнихТестов = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemПапкаВнешнихТестовСправка = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemФайлыНастроек = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSMItemChangelog = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSound = new System.Windows.Forms.ToolStripMenuItem();
            this.отключитьПортToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redratЗахватСигналаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autostartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOperation = new System.Windows.Forms.ToolStripMenuItem();
            this.папкаF5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.pass_lbl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.timerBlink = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.Soundbar = new System.Windows.Forms.TrackBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonText = new System.Windows.Forms.Button();
            this.buttonCode = new System.Windows.Forms.Button();
            this.SoundLbl = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.labelPort1 = new System.Windows.Forms.Label();
            this.labelPort2 = new System.Windows.Forms.Label();
            this.txtOperationName = new System.Windows.Forms.TextBox();
            this.lblOperationName = new System.Windows.Forms.Label();
            this.labelUSB = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Soundbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.Window;
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(13, 89);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(295, 240);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 46;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Tile;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "xrossa2.png");
            this.imageList1.Images.SetKeyName(1, "2000px-Dialog-error-round.svg.png");
            this.imageList1.Images.SetKeyName(2, "refresh_error.png");
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.autostartToolStripMenuItem,
            this.MenuItemOperation,
            this.папкаF5ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(679, 25);
            this.menuStrip1.TabIndex = 50;
            this.menuStrip1.Text = "sdfsdfsd";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripMenuItem1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 21);
            this.toolStripMenuItem1.Text = "Операции (F1)";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItemSelectModel_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(96, 21);
            this.toolStripMenuItem2.Text = "Настройки (F2)";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItemOptions_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.дубликатыToolStripMenuItem,
            this.toolSMItemEditTestF4,
            this.toolSMItemПапкаВнешнихТестовMain,
            this.toolSMItemВнешниеТесты,
            this.toolSMItemВстроенныеТестыMain,
            this.справкаF10ToolStripMenuItem,
            this.MenuItemSound,
            this.отключитьПортToolStripMenuItem,
            this.redratЗахватСигналаToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.ShortcutKeyDisplayString = "";
            this.toolStripMenuItem3.ShowShortcutKeys = false;
            this.toolStripMenuItem3.Size = new System.Drawing.Size(96, 21);
            this.toolStripMenuItem3.Text = "Открыть.. (F3)";
            this.toolStripMenuItem3.DropDownClosed += new System.EventHandler(this.toolStripMenuItem3_DropDownClosed);
            this.toolStripMenuItem3.DropDownOpened += new System.EventHandler(this.toolStripMenuItem3_DropDownOpened);
            // 
            // дубликатыToolStripMenuItem
            // 
            this.дубликатыToolStripMenuItem.Name = "дубликатыToolStripMenuItem";
            this.дубликатыToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.дубликатыToolStripMenuItem.Text = "Дубликаты!";
            this.дубликатыToolStripMenuItem.Click += new System.EventHandler(this.дубликатыToolStripMenuItem_Click);
            // 
            // toolSMItemEditTestF4
            // 
            this.toolSMItemEditTestF4.Name = "toolSMItemEditTestF4";
            this.toolSMItemEditTestF4.Size = new System.Drawing.Size(246, 22);
            this.toolSMItemEditTestF4.Text = "Открыть внешний тест (F4)";
            this.toolSMItemEditTestF4.Click += new System.EventHandler(this.editTestF4ToolStripMenuItem_Click);
            // 
            // toolSMItemПапкаВнешнихТестовMain
            // 
            this.toolSMItemПапкаВнешнихТестовMain.Name = "toolSMItemПапкаВнешнихТестовMain";
            this.toolSMItemПапкаВнешнихТестовMain.Size = new System.Drawing.Size(246, 22);
            this.toolSMItemПапкаВнешнихТестовMain.Text = "Папка внешних тестов (Shift +F4)";
            this.toolSMItemПапкаВнешнихТестовMain.Click += new System.EventHandler(this.папкаТестовToolStripMenuItem_Click);
            // 
            // toolSMItemВнешниеТесты
            // 
            this.toolSMItemВнешниеТесты.Name = "toolSMItemВнешниеТесты";
            this.toolSMItemВнешниеТесты.Size = new System.Drawing.Size(246, 22);
            this.toolSMItemВнешниеТесты.Text = "Внешние тесты (F10)";
            this.toolSMItemВнешниеТесты.Click += new System.EventHandler(this.созданныеFToolStripMenuItem_Click);
            // 
            // toolSMItemВстроенныеТестыMain
            // 
            this.toolSMItemВстроенныеТестыMain.Name = "toolSMItemВстроенныеТестыMain";
            this.toolSMItemВстроенныеТестыMain.Size = new System.Drawing.Size(246, 22);
            this.toolSMItemВстроенныеТестыMain.Text = "Встроенные тесты";
            this.toolSMItemВстроенныеТестыMain.Click += new System.EventHandler(this.встроенныеТестыToolStripMenuItem_Click);
            // 
            // справкаF10ToolStripMenuItem
            // 
            this.справкаF10ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolSMItemУправление,
            this.записьПрошивокToolStripMenuItem,
            this.toolSMItemВстроенныеТестыСправка,
            this.toolSMItemКомандыВнешнихТестов,
            this.toolSMItemПапкаВнешнихТестовСправка,
            this.toolSMItemФайлыНастроек,
            this.toolSMItemChangelog});
            this.справкаF10ToolStripMenuItem.Name = "справкаF10ToolStripMenuItem";
            this.справкаF10ToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.справкаF10ToolStripMenuItem.Text = "Справка";
            // 
            // toolSMItemУправление
            // 
            this.toolSMItemУправление.Name = "toolSMItemУправление";
            this.toolSMItemУправление.Size = new System.Drawing.Size(249, 22);
            this.toolSMItemУправление.Text = "Управление";
            this.toolSMItemУправление.Click += new System.EventHandler(this.управлениеToolStripMenuItem_Click);
            // 
            // записьПрошивокToolStripMenuItem
            // 
            this.записьПрошивокToolStripMenuItem.Name = "записьПрошивокToolStripMenuItem";
            this.записьПрошивокToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.записьПрошивокToolStripMenuItem.Text = "Запуск сторонних .exe";
            this.записьПрошивокToolStripMenuItem.Click += new System.EventHandler(this.exeLauncherToolStripMenuItem_Click);
            // 
            // toolSMItemВстроенныеТестыСправка
            // 
            this.toolSMItemВстроенныеТестыСправка.Name = "toolSMItemВстроенныеТестыСправка";
            this.toolSMItemВстроенныеТестыСправка.Size = new System.Drawing.Size(249, 22);
            this.toolSMItemВстроенныеТестыСправка.Text = "Встроенные тесты";
            this.toolSMItemВстроенныеТестыСправка.Click += new System.EventHandler(this.встроенныеТестыToolStripMenuItem_Click);
            // 
            // toolSMItemКомандыВнешнихТестов
            // 
            this.toolSMItemКомандыВнешнихТестов.Name = "toolSMItemКомандыВнешнихТестов";
            this.toolSMItemКомандыВнешнихТестов.Size = new System.Drawing.Size(249, 22);
            this.toolSMItemКомандыВнешнихТестов.Text = "Команды внешних тестов (F11)";
            this.toolSMItemКомандыВнешнихТестов.Click += new System.EventHandler(this.командыПрограммированияToolStripMenuItem_Click);
            // 
            // toolSMItemПапкаВнешнихТестовСправка
            // 
            this.toolSMItemПапкаВнешнихТестовСправка.Name = "toolSMItemПапкаВнешнихТестовСправка";
            this.toolSMItemПапкаВнешнихТестовСправка.Size = new System.Drawing.Size(249, 22);
            this.toolSMItemПапкаВнешнихТестовСправка.Text = "Папка внешних тестов (Shift + F4)";
            this.toolSMItemПапкаВнешнихТестовСправка.Click += new System.EventHandler(this.папкаТестовToolStripMenuItem_Click);
            // 
            // toolSMItemФайлыНастроек
            // 
            this.toolSMItemФайлыНастроек.Name = "toolSMItemФайлыНастроек";
            this.toolSMItemФайлыНастроек.Size = new System.Drawing.Size(249, 22);
            this.toolSMItemФайлыНастроек.Text = ".opt и .wav файлы";
            this.toolSMItemФайлыНастроек.Click += new System.EventHandler(this.файлыНастроекToolStripMenuItem_Click);
            // 
            // toolSMItemChangelog
            // 
            this.toolSMItemChangelog.Name = "toolSMItemChangelog";
            this.toolSMItemChangelog.Size = new System.Drawing.Size(249, 22);
            this.toolSMItemChangelog.Text = "Changelog";
            this.toolSMItemChangelog.Click += new System.EventHandler(this.changelogToolStripMenuItem_Click);
            // 
            // MenuItemSound
            // 
            this.MenuItemSound.Name = "MenuItemSound";
            this.MenuItemSound.Size = new System.Drawing.Size(246, 22);
            this.MenuItemSound.Text = "Звук";
            this.MenuItemSound.Click += new System.EventHandler(this.MenuItemSound_Click);
            // 
            // отключитьПортToolStripMenuItem
            // 
            this.отключитьПортToolStripMenuItem.Name = "отключитьПортToolStripMenuItem";
            this.отключитьПортToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.отключитьПортToolStripMenuItem.Text = "отключить порт";
            this.отключитьПортToolStripMenuItem.Click += new System.EventHandler(this.отключитьПортToolStripMenuItem_Click);
            // 
            // redratЗахватСигналаToolStripMenuItem
            // 
            this.redratЗахватСигналаToolStripMenuItem.Name = "redratЗахватСигналаToolStripMenuItem";
            this.redratЗахватСигналаToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.redratЗахватСигналаToolStripMenuItem.Text = "Redrat захват сигнала (F8)";
            this.redratЗахватСигналаToolStripMenuItem.Click += new System.EventHandler(this.redratЗахватСигналаToolStripMenuItem_Click);
            // 
            // autostartToolStripMenuItem
            // 
            this.autostartToolStripMenuItem.Name = "autostartToolStripMenuItem";
            this.autostartToolStripMenuItem.Size = new System.Drawing.Size(88, 21);
            this.autostartToolStripMenuItem.Text = "Autostart (F5)";
            this.autostartToolStripMenuItem.Click += new System.EventHandler(this.autostartToolStripMenuItem_Click);
            // 
            // MenuItemOperation
            // 
            this.MenuItemOperation.Name = "MenuItemOperation";
            this.MenuItemOperation.Size = new System.Drawing.Size(104, 21);
            this.MenuItemOperation.Text = "Operation.ini (F6)";
            this.MenuItemOperation.Click += new System.EventHandler(this.MenuItemOperation_Click);
            // 
            // папкаF5ToolStripMenuItem
            // 
            this.папкаF5ToolStripMenuItem.Name = "папкаF5ToolStripMenuItem";
            this.папкаF5ToolStripMenuItem.Size = new System.Drawing.Size(73, 21);
            this.папкаF5ToolStripMenuItem.Text = "Папка (F7)";
            this.папкаF5ToolStripMenuItem.Click += new System.EventHandler(this.папкаF5ToolStripMenuItem_Click);
            // 
            // timerStart
            // 
            this.timerStart.Interval = 1000;
            this.timerStart.Tick += new System.EventHandler(this.timer);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(13, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(366, 29);
            this.label3.TabIndex = 59;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pass_lbl
            // 
            this.pass_lbl.BackColor = System.Drawing.SystemColors.Control;
            this.pass_lbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pass_lbl.Font = new System.Drawing.Font("Verdana", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pass_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pass_lbl.Location = new System.Drawing.Point(12, 344);
            this.pass_lbl.Name = "pass_lbl";
            this.pass_lbl.Size = new System.Drawing.Size(248, 41);
            this.pass_lbl.TabIndex = 60;
            this.pass_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pass_lbl.Click += new System.EventHandler(this.pass_lbl_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(19)))), ((int)(((byte)(14)))));
            this.label5.Location = new System.Drawing.Point(10, 397);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(612, 24);
            this.label5.TabIndex = 71;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.Color.DarkRed;
            this.label6.Location = new System.Drawing.Point(12, 425);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(610, 22);
            this.label6.TabIndex = 72;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.Color.ForestGreen;
            this.label7.Location = new System.Drawing.Point(262, 344);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 20);
            this.label7.TabIndex = 73;
            this.label7.Text = "0";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(262, 364);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 21);
            this.label8.TabIndex = 74;
            this.label8.Text = "0";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerBlink
            // 
            this.timerBlink.Interval = 1000;
            this.timerBlink.Tick += new System.EventHandler(this.timerBlink_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(624, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 18);
            this.label1.TabIndex = 78;
            this.label1.Text = "F12";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(334, 65);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(330, 264);
            this.textBox1.TabIndex = 48;
            this.textBox1.TabStop = false;
            this.textBox1.Visible = false;
            // 
            // textBoxText
            // 
            this.textBoxText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxText.Location = new System.Drawing.Point(334, 65);
            this.textBoxText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxText.Multiline = true;
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxText.Size = new System.Drawing.Size(330, 264);
            this.textBoxText.TabIndex = 66;
            this.textBoxText.TabStop = false;
            this.textBoxText.Visible = false;
            // 
            // textBoxCode
            // 
            this.textBoxCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxCode.Location = new System.Drawing.Point(334, 65);
            this.textBoxCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxCode.Multiline = true;
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCode.Size = new System.Drawing.Size(330, 264);
            this.textBoxCode.TabIndex = 67;
            this.textBoxCode.TabStop = false;
            // 
            // Soundbar
            // 
            this.Soundbar.Location = new System.Drawing.Point(478, 333);
            this.Soundbar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Soundbar.Maximum = 100;
            this.Soundbar.Name = "Soundbar";
            this.Soundbar.Size = new System.Drawing.Size(188, 45);
            this.Soundbar.TabIndex = 57;
            this.Soundbar.TickFrequency = 10;
            this.Soundbar.Visible = false;
            this.Soundbar.Scroll += new System.EventHandler(this.Soundbar_Scroll);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::TVSender.Properties.Resources.audio;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(636, 362);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 26);
            this.pictureBox1.TabIndex = 61;
            this.pictureBox1.TabStop = false;
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.Lime;
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(32)))), ((int)(((byte)(2)))));
            this.buttonStart.Location = new System.Drawing.Point(334, 341);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(138, 44);
            this.buttonStart.TabIndex = 45;
            this.buttonStart.TabStop = false;
            this.buttonStart.Text = "Старт";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(392, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 29);
            this.label2.TabIndex = 56;
            // 
            // buttonText
            // 
            this.buttonText.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonText.Location = new System.Drawing.Point(521, 309);
            this.buttonText.Name = "buttonText";
            this.buttonText.Size = new System.Drawing.Size(42, 19);
            this.buttonText.TabIndex = 69;
            this.buttonText.TabStop = false;
            this.buttonText.Text = "ТЕКСТ";
            this.buttonText.UseVisualStyleBackColor = true;
            this.buttonText.Click += new System.EventHandler(this.buttonText_Click);
            // 
            // buttonCode
            // 
            this.buttonCode.Enabled = false;
            this.buttonCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCode.Location = new System.Drawing.Point(561, 309);
            this.buttonCode.Name = "buttonCode";
            this.buttonCode.Size = new System.Drawing.Size(33, 19);
            this.buttonCode.TabIndex = 70;
            this.buttonCode.TabStop = false;
            this.buttonCode.Text = "КОД";
            this.buttonCode.UseVisualStyleBackColor = true;
            this.buttonCode.Click += new System.EventHandler(this.buttonCode_Click);
            // 
            // SoundLbl
            // 
            this.SoundLbl.Location = new System.Drawing.Point(601, 370);
            this.SoundLbl.Name = "SoundLbl";
            this.SoundLbl.Size = new System.Drawing.Size(35, 15);
            this.SoundLbl.TabIndex = 58;
            this.SoundLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Red;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(478, 21);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(187, 44);
            this.button4.TabIndex = 76;
            this.button4.TabStop = false;
            this.button4.Text = "Забраковать";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // labelPort1
            // 
            this.labelPort1.AutoSize = true;
            this.labelPort1.BackColor = System.Drawing.SystemColors.Window;
            this.labelPort1.Location = new System.Drawing.Point(339, 314);
            this.labelPort1.Name = "labelPort1";
            this.labelPort1.Size = new System.Drawing.Size(52, 13);
            this.labelPort1.TabIndex = 79;
            this.labelPort1.Text = "COM1 off";
            this.labelPort1.Visible = false;
            // 
            // labelPort2
            // 
            this.labelPort2.AutoSize = true;
            this.labelPort2.BackColor = System.Drawing.SystemColors.Window;
            this.labelPort2.Location = new System.Drawing.Point(408, 314);
            this.labelPort2.Name = "labelPort2";
            this.labelPort2.Size = new System.Drawing.Size(52, 13);
            this.labelPort2.TabIndex = 80;
            this.labelPort2.Text = "COM2 off";
            // 
            // txtOperationName
            // 
            this.txtOperationName.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtOperationName.Enabled = false;
            this.txtOperationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtOperationName.Location = new System.Drawing.Point(13, 65);
            this.txtOperationName.Name = "txtOperationName";
            this.txtOperationName.ReadOnly = true;
            this.txtOperationName.Size = new System.Drawing.Size(295, 27);
            this.txtOperationName.TabIndex = 82;
            this.txtOperationName.TabStop = false;
            this.txtOperationName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOperationName
            // 
            this.lblOperationName.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblOperationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOperationName.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblOperationName.Location = new System.Drawing.Point(14, 66);
            this.lblOperationName.Name = "lblOperationName";
            this.lblOperationName.Size = new System.Drawing.Size(293, 23);
            this.lblOperationName.TabIndex = 83;
            this.lblOperationName.Text = "KTS all";
            this.lblOperationName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblOperationName.Click += new System.EventHandler(this.lblOperationName_Click);
            // 
            // labelUSB
            // 
            this.labelUSB.AutoSize = true;
            this.labelUSB.BackColor = System.Drawing.SystemColors.Window;
            this.labelUSB.Location = new System.Drawing.Point(470, 314);
            this.labelUSB.Name = "labelUSB";
            this.labelUSB.Size = new System.Drawing.Size(44, 13);
            this.labelUSB.TabIndex = 84;
            this.labelUSB.Text = "USB off";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(592, 309);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 19);
            this.button1.TabIndex = 85;
            this.button1.TabStop = false;
            this.button1.Text = "Очистить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(679, 393);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelUSB);
            this.Controls.Add(this.lblOperationName);
            this.Controls.Add(this.txtOperationName);
            this.Controls.Add(this.labelPort2);
            this.Controls.Add(this.labelPort1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.SoundLbl);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonCode);
            this.Controls.Add(this.buttonText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pass_lbl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.Soundbar);
            this.Controls.Add(this.textBoxCode);
            this.Controls.Add(this.textBoxText);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TV Tester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Soundbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Timer timerStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label pass_lbl;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timerBlink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSound;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem папкаF5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаF10ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemEditTestF4;
        private System.Windows.Forms.ToolStripMenuItem MenuItemOperation;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemКомандыВнешнихТестов;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemВнешниеТесты;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemФайлыНастроек;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemВстроенныеТестыСправка;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemChangelog;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.TrackBar Soundbar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonText;
        private System.Windows.Forms.Button buttonCode;
        private System.Windows.Forms.Label SoundLbl;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label labelPort1;
        private System.Windows.Forms.Label labelPort2;
        private System.Windows.Forms.TextBox txtOperationName;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemУправление;
        private System.Windows.Forms.Label lblOperationName;
        private System.Windows.Forms.ToolStripMenuItem записьПрошивокToolStripMenuItem;
        private System.Windows.Forms.Label labelUSB;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemПапкаВнешнихТестовMain;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemВстроенныеТестыMain;
        private System.Windows.Forms.ToolStripMenuItem toolSMItemПапкаВнешнихТестовСправка;
        private System.Windows.Forms.ToolStripMenuItem redratЗахватСигналаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отключитьПортToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem дубликатыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autostartToolStripMenuItem;
    }
}

