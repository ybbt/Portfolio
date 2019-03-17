namespace My__analog__Paint
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.создатьToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.открытьToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.сохранитьToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_line = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_rect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_ell = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Batman = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_MainArrow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton_Arrow = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButton_ArrowR = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton_ArrowL = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton_fillRect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_fillEll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_FillBatman = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_MainFillArrow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton_FillArrow = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButton_FillArrowR = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton_FillArrowL = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton_Text = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Brush = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Pen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_FloodFill = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Eraser = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_pipet = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Back = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Forward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_plus = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_minus = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.SizeChanged += new System.EventHandler(this.splitContainer1_Panel2_SizeChanged);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(984, 612);
            this.splitContainer1.SplitterDistance = 621;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(618, 612);
            this.splitContainer2.SplitterDistance = 214;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.numericUpDown1);
            this.splitContainer3.Size = new System.Drawing.Size(618, 394);
            this.splitContainer3.SplitterDistance = 159;
            this.splitContainer3.TabIndex = 0;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDown1.Location = new System.Drawing.Point(12, 202);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(87, 26);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown1_KeyPress);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.создатьToolStripButton,
            this.открытьToolStripButton,
            this.сохранитьToolStripButton,
            this.toolStripSeparator1,
            this.toolStripButton_line,
            this.toolStripButton_rect,
            this.toolStripButton_ell,
            this.toolStripButton_Batman,
            this.toolStripButton_MainArrow,
            this.toolStripSplitButton_Arrow,
            this.toolStripButton_fillRect,
            this.toolStripButton_fillEll,
            this.toolStripButton_FillBatman,
            this.toolStripButton_MainFillArrow,
            this.toolStripSplitButton_FillArrow,
            this.toolStripButton_Text,
            this.toolStripSeparator3,
            this.toolStripButton_Brush,
            this.toolStripButton_Pen,
            this.toolStripButton_FloodFill,
            this.toolStripButton_Eraser,
            this.toolStripSeparator4,
            this.toolStripButton_pipet,
            this.toolStripSeparator5,
            this.toolStripButton_Back,
            this.toolStripButton_Forward,
            this.toolStripButton_plus,
            this.toolStripButton_minus,
            this.toolStripSplitButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // создатьToolStripButton
            // 
            this.создатьToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.создатьToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("создатьToolStripButton.Image")));
            this.создатьToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.создатьToolStripButton.Name = "создатьToolStripButton";
            this.создатьToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.создатьToolStripButton.Text = "&Создать";
            this.создатьToolStripButton.Click += new System.EventHandler(this.создать_ToolStripButton_Click);
            // 
            // открытьToolStripButton
            // 
            this.открытьToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.открытьToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("открытьToolStripButton.Image")));
            this.открытьToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.открытьToolStripButton.Name = "открытьToolStripButton";
            this.открытьToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.открытьToolStripButton.Text = "&Открыть";
            this.открытьToolStripButton.Click += new System.EventHandler(this.открыть_ToolStripButton_Click);
            // 
            // сохранитьToolStripButton
            // 
            this.сохранитьToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.сохранитьToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("сохранитьToolStripButton.Image")));
            this.сохранитьToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.сохранитьToolStripButton.Name = "сохранитьToolStripButton";
            this.сохранитьToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.сохранитьToolStripButton.Text = "&Сохранить";
            this.сохранитьToolStripButton.Click += new System.EventHandler(this.сохранить_ToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_line
            // 
            this.toolStripButton_line.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_line.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_line.Image")));
            this.toolStripButton_line.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_line.Name = "toolStripButton_line";
            this.toolStripButton_line.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_line.Text = "Линия";
            this.toolStripButton_line.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_rect
            // 
            this.toolStripButton_rect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_rect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_rect.Image")));
            this.toolStripButton_rect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_rect.Name = "toolStripButton_rect";
            this.toolStripButton_rect.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_rect.Text = "Прямоугольник";
            this.toolStripButton_rect.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_ell
            // 
            this.toolStripButton_ell.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_ell.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ell.Image")));
            this.toolStripButton_ell.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ell.Name = "toolStripButton_ell";
            this.toolStripButton_ell.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_ell.Text = "Эллипс";
            this.toolStripButton_ell.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_Batman
            // 
            this.toolStripButton_Batman.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Batman.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Batman.Image")));
            this.toolStripButton_Batman.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Batman.Name = "toolStripButton_Batman";
            this.toolStripButton_Batman.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Batman.Text = "Контур Бэтмена";
            this.toolStripButton_Batman.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_MainArrow
            // 
            this.toolStripButton_MainArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_MainArrow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_MainArrow.Image")));
            this.toolStripButton_MainArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_MainArrow.Name = "toolStripButton_MainArrow";
            this.toolStripButton_MainArrow.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_MainArrow.Text = "Контур стрелки";
            this.toolStripButton_MainArrow.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripSplitButton_Arrow
            // 
            this.toolStripSplitButton_Arrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripSplitButton_Arrow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_ArrowR,
            this.toolStripButton_ArrowL});
            this.toolStripSplitButton_Arrow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton_Arrow.Image")));
            this.toolStripSplitButton_Arrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton_Arrow.Name = "toolStripSplitButton_Arrow";
            this.toolStripSplitButton_Arrow.Size = new System.Drawing.Size(13, 22);
            this.toolStripSplitButton_Arrow.Text = "Выбор стрелки";
            // 
            // toolStripButton_ArrowR
            // 
            this.toolStripButton_ArrowR.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ArrowR.Image")));
            this.toolStripButton_ArrowR.Name = "toolStripButton_ArrowR";
            this.toolStripButton_ArrowR.Size = new System.Drawing.Size(112, 22);
            this.toolStripButton_ArrowR.Text = "правая";
            this.toolStripButton_ArrowR.Click += new System.EventHandler(this.toolStripButton_Click_Arrow);
            // 
            // toolStripButton_ArrowL
            // 
            this.toolStripButton_ArrowL.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ArrowL.Image")));
            this.toolStripButton_ArrowL.Name = "toolStripButton_ArrowL";
            this.toolStripButton_ArrowL.Size = new System.Drawing.Size(112, 22);
            this.toolStripButton_ArrowL.Text = "левая";
            this.toolStripButton_ArrowL.Click += new System.EventHandler(this.toolStripButton_Click_Arrow);
            // 
            // toolStripButton_fillRect
            // 
            this.toolStripButton_fillRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_fillRect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_fillRect.Image")));
            this.toolStripButton_fillRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_fillRect.Name = "toolStripButton_fillRect";
            this.toolStripButton_fillRect.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_fillRect.Text = "Залитый прямоугольник";
            this.toolStripButton_fillRect.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_fillEll
            // 
            this.toolStripButton_fillEll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_fillEll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_fillEll.Image")));
            this.toolStripButton_fillEll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_fillEll.Name = "toolStripButton_fillEll";
            this.toolStripButton_fillEll.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_fillEll.Text = "Залитый эллипс";
            this.toolStripButton_fillEll.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_FillBatman
            // 
            this.toolStripButton_FillBatman.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_FillBatman.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_FillBatman.Image")));
            this.toolStripButton_FillBatman.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_FillBatman.Name = "toolStripButton_FillBatman";
            this.toolStripButton_FillBatman.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_FillBatman.Text = "Заполненнный Бэтмен";
            this.toolStripButton_FillBatman.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_MainFillArrow
            // 
            this.toolStripButton_MainFillArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_MainFillArrow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_MainFillArrow.Image")));
            this.toolStripButton_MainFillArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_MainFillArrow.Name = "toolStripButton_MainFillArrow";
            this.toolStripButton_MainFillArrow.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_MainFillArrow.Text = "Залитая стрелка";
            this.toolStripButton_MainFillArrow.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripSplitButton_FillArrow
            // 
            this.toolStripSplitButton_FillArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripSplitButton_FillArrow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_FillArrowR,
            this.toolStripButton_FillArrowL});
            this.toolStripSplitButton_FillArrow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton_FillArrow.Image")));
            this.toolStripSplitButton_FillArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton_FillArrow.Name = "toolStripSplitButton_FillArrow";
            this.toolStripSplitButton_FillArrow.Size = new System.Drawing.Size(13, 22);
            this.toolStripSplitButton_FillArrow.Text = "Виды стрелок";
            // 
            // toolStripButton_FillArrowR
            // 
            this.toolStripButton_FillArrowR.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_FillArrowR.Image")));
            this.toolStripButton_FillArrowR.Name = "toolStripButton_FillArrowR";
            this.toolStripButton_FillArrowR.Size = new System.Drawing.Size(112, 22);
            this.toolStripButton_FillArrowR.Text = "правая";
            this.toolStripButton_FillArrowR.Click += new System.EventHandler(this.toolStripButton_Click_Arrow);
            // 
            // toolStripButton_FillArrowL
            // 
            this.toolStripButton_FillArrowL.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_FillArrowL.Image")));
            this.toolStripButton_FillArrowL.Name = "toolStripButton_FillArrowL";
            this.toolStripButton_FillArrowL.Size = new System.Drawing.Size(112, 22);
            this.toolStripButton_FillArrowL.Text = "левая";
            this.toolStripButton_FillArrowL.Click += new System.EventHandler(this.toolStripButton_Click_Arrow);
            // 
            // toolStripButton_Text
            // 
            this.toolStripButton_Text.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Text.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Text.Image")));
            this.toolStripButton_Text.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Text.Name = "toolStripButton_Text";
            this.toolStripButton_Text.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Text.Text = "А";
            this.toolStripButton_Text.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Brush
            // 
            this.toolStripButton_Brush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Brush.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Brush.Image")));
            this.toolStripButton_Brush.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Brush.Name = "toolStripButton_Brush";
            this.toolStripButton_Brush.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Brush.Text = "Кисть";
            this.toolStripButton_Brush.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_Pen
            // 
            this.toolStripButton_Pen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Pen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Pen.Image")));
            this.toolStripButton_Pen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Pen.Name = "toolStripButton_Pen";
            this.toolStripButton_Pen.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Pen.Text = "Карандаш";
            this.toolStripButton_Pen.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_FloodFill
            // 
            this.toolStripButton_FloodFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_FloodFill.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_FloodFill.Image")));
            this.toolStripButton_FloodFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_FloodFill.Name = "toolStripButton_FloodFill";
            this.toolStripButton_FloodFill.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_FloodFill.Text = "Заливка";
            this.toolStripButton_FloodFill.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButton_Eraser
            // 
            this.toolStripButton_Eraser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Eraser.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Eraser.Image")));
            this.toolStripButton_Eraser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Eraser.Name = "toolStripButton_Eraser";
            this.toolStripButton_Eraser.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Eraser.Text = "Ластик";
            this.toolStripButton_Eraser.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_pipet
            // 
            this.toolStripButton_pipet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_pipet.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_pipet.Image")));
            this.toolStripButton_pipet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_pipet.Name = "toolStripButton_pipet";
            this.toolStripButton_pipet.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_pipet.Text = "Пипетка";
            this.toolStripButton_pipet.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Back
            // 
            this.toolStripButton_Back.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Back.Enabled = false;
            this.toolStripButton_Back.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Back.Image")));
            this.toolStripButton_Back.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Back.Name = "toolStripButton_Back";
            this.toolStripButton_Back.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Back.Text = "Назад";
            this.toolStripButton_Back.Click += new System.EventHandler(this.toolStripButton_Back_Click);
            // 
            // toolStripButton_Forward
            // 
            this.toolStripButton_Forward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Forward.Enabled = false;
            this.toolStripButton_Forward.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Forward.Image")));
            this.toolStripButton_Forward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Forward.Name = "toolStripButton_Forward";
            this.toolStripButton_Forward.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Forward.Text = "Вперед";
            this.toolStripButton_Forward.Click += new System.EventHandler(this.toolStripButton_Forward_Click);
            // 
            // toolStripButton_plus
            // 
            this.toolStripButton_plus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_plus.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_plus.Image")));
            this.toolStripButton_plus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_plus.Name = "toolStripButton_plus";
            this.toolStripButton_plus.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_plus.Text = "Увеличить";
            this.toolStripButton_plus.Click += new System.EventHandler(this.toolStripButton_plus_Click);
            // 
            // toolStripButton_minus
            // 
            this.toolStripButton_minus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_minus.Enabled = false;
            this.toolStripButton_minus.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_minus.Image")));
            this.toolStripButton_minus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_minus.Name = "toolStripButton_minus";
            this.toolStripButton_minus.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_minus.Text = "Уменьшить";
            this.toolStripButton_minus.Click += new System.EventHandler(this.toolStripButton_minus_Click);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton2.Image")));
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(16, 22);
            this.toolStripSplitButton2.Text = "toolStripSplitButton2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(984, 612);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "My_First_Paint";
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ToolStripButton создатьToolStripButton;
        private System.Windows.Forms.ToolStripButton открытьToolStripButton;
        private System.Windows.Forms.ToolStripButton сохранитьToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_line;
        private System.Windows.Forms.ToolStripButton toolStripButton_rect;
        private System.Windows.Forms.ToolStripButton toolStripButton_ell;
        private System.Windows.Forms.ToolStripButton toolStripButton_fillRect;
        private System.Windows.Forms.ToolStripButton toolStripButton_fillEll;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton_Brush;
        private System.Windows.Forms.ToolStripButton toolStripButton_Pen;
        private System.Windows.Forms.ToolStripButton toolStripButton_FloodFill;
        private System.Windows.Forms.ToolStripButton toolStripButton_Eraser;
        private System.Windows.Forms.ToolStripButton toolStripButton_Back;
        private System.Windows.Forms.ToolStripButton toolStripButton_Forward;
        private System.Windows.Forms.ToolStripButton toolStripButton_plus;
        private System.Windows.Forms.ToolStripButton toolStripButton_minus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton_pipet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton_MainFillArrow;
        private System.Windows.Forms.ToolStripButton toolStripButton_Text;
        private System.Windows.Forms.ToolStripButton toolStripButton_Batman;
        private System.Windows.Forms.ToolStripButton toolStripButton_FillBatman;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitButton_Arrow;
        private System.Windows.Forms.ToolStripMenuItem toolStripButton_ArrowR;
        private System.Windows.Forms.ToolStripMenuItem toolStripButton_ArrowL;
        private System.Windows.Forms.ToolStripButton toolStripButton_MainArrow;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitButton_FillArrow;
        private System.Windows.Forms.ToolStripMenuItem toolStripButton_FillArrowR;
        private System.Windows.Forms.ToolStripMenuItem toolStripButton_FillArrowL;
    }
}

