using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace My__analog__Paint
{
    public partial class Form1 : Form
    {
        //DLL из gdi32.dll для создания лупы и заливки
        #region
        [DllImportAttribute("gdi32.dll", EntryPoint = "GetPixel", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetPixel(IntPtr hDC, int x, int y);
        
        [DllImportAttribute("gdi32.dll", EntryPoint = "StretchBlt", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern uint StretchBlt(IntPtr hDC, int xDest, int yDest, int widthD, int heightD, IntPtr hdcSrc, int xSrc, int ySrc, int widthS, int heightS, uint dwRop);

        [DllImportAttribute("gdi32.dll", EntryPoint = "ExtFloodFill", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern uint ExtFloodFill(IntPtr hdc, int nXStart, int nYStart, uint crFill, uint fuFillType);

        [DllImportAttribute("gdi32.dll", EntryPoint = "CreateSolidBrush", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CreateSolidBrush(uint crColor);

        [DllImportAttribute("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern uint SelectObject(IntPtr hDC, uint hObject);

        [DllImportAttribute("gdi32.dll", EntryPoint = "DeleteObject", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern uint DeleteObject(uint hObject);
        #endregion
        //-------------------------------------------
        
        int HeightScreen = Screen.PrimaryScreen.WorkingArea.Height;//разрешение эрана
        int WidthScreen = Screen.PrimaryScreen.WorkingArea.Width;//разрешение эрана
        
        //public drawButton pressBut = drawButton.line;//метка выбранного инструмента
        
        public static int i_plus = 0; //счетчик нажатий на плюс

        drawButton ArrowWay = drawButton.arrowR;
        drawButton FillArrowWay = drawButton.fillArrowR;

        public event ChangePaintParam EventLineWidth;//событие изменения толщины линии
        public event ChangePressBut EventPressButt;//событие изменения нажатой кнопки выбора инструмента рисования
        
        MyCanvas MC; //Холст
        
        //Буфферизация 
        private BufferedGraphicsContext context;        
        //--------------------

        /// <summary>
        /// конструктор
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            
            this.Size = new Size((int)(WidthScreen*0.9), (int)(HeightScreen*0.9));//размер формы
            this.Location = new Point((int)(WidthScreen * 0.05), (int)(HeightScreen * 0.05));//положение формы
            
            this.splitContainer1.Location = new Point(0, 25); //положение SplitContainer с учетом ToolStrip
            this.splitContainer1.SplitterDistance = (this.Height-50)/5;//this.Width/6; //размер зоны по собственные ЭУ
            this.splitContainer2.SplitterDistance = (this.Height-50)/4;
            this.splitContainer3.SplitterDistance = (int)(((this.Height-50)/4)*1);
            
            this.splitContainer1.Height = this.Height-60;
            this.splitContainer2.Height = this.splitContainer1.Height-50;
            this.splitContainer2.Width = this.splitContainer1.Panel1.Width;
            this.splitContainer3.Height = this.splitContainer2.Height-this.splitContainer2.SplitterDistance;

            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.HorizontalScroll.Enabled = false;

            

            this.toolStripButton_line.Checked = true;//кнопка действия нажатая при открытии

            ColorChooser CC = new ColorChooser();//панель выбора цвета
            CC.Top = 5;
            CC.Left = 20;
            
            CC.Width = this.splitContainer1.Panel1.Width-CC.Left*2;
            CC.Height = (int)((float)this.splitContainer2.Panel1.Height/**0.75*/)-CC.Top*2;
            CC.Anchor = /*AnchorStyles.Bottom | */AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            TextureChooser TC = new TextureChooser(); //панель выбора штриховки
            TC.Top = 0;
            TC.Left = 20;
            TC.Width = this.splitContainer1.Panel1.Width-TC.Left*2;
            TC.Height = this.splitContainer3.SplitterDistance-TC.Top*2;
            TC.Anchor = /*AnchorStyles.Bottom | */AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            LineChuser LC = new LineChuser();//панель выбора типа линии
            LC.Top = 0;
            LC.Left = 20;
            LC.Width = this.splitContainer1.Panel1.Width - LC.Left*2;
            LC.Height = (this.splitContainer3.Panel2.Height/*-TC.Height*/ - LC.Top*2)/2;
            LC.Anchor = /*AnchorStyles.Bottom | */AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            MC = new MyCanvas();
            MC.Top = 10;
            MC.Left = 10;
            MC.Width = this.splitContainer1.Panel2.Width-20;
            MC.Height = this.splitContainer1.Panel2.Height-20;

            if (this.Width !=0 && this.Height != 0)
            {
                MC.CountHist = (int)(200 / ((decimal)(MC.Width * MC.Height * 3)/(decimal)1000000));
            }
            
            //Добавление собственных элементов в форму
            this.splitContainer2.Panel1.Controls.Add(CC);
            this.splitContainer3.Panel1.Controls.Add(TC);
            this.splitContainer3.Panel2.Controls.Add(LC);
            this.splitContainer1.Panel2.Controls.Add(MC);
            //---------------------

            //положение и размер счетчика (толщина линии)
            this.numericUpDown1.Location = new Point(20, LC.Height+5);
            this.numericUpDown1.Width = LC.Width;
            //---------------------------


            //буфферизация
            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(MC.Width + 1, MC.Height + 1);
            
            MC.bgGraph_1st = context.Allocate(MC.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
                        
            MC.bgGraph_2nd = context.Allocate(MC.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
            //---------------

            MC.EventChangeButton += new ChangeBackForward(ChangeButtonBF);//привязка к событию функции изменения состояния кнопок Вперед-Назад
            CC.EventColorChange += new ChangePaintParam(MC.MyCanvas_ChangePaintParam);//привязка к событию изменения цвета
            MC.EventChangeColorPipet += new ChangePaintParam(CC.ChangeColorPipet);//привязка к событию изменения цвета по пипетке
            TC.EventTextureChange += new ChangePaintParam(MC.MyCanvas_ChangePaintParam);//привязка к событию изменения штриховки
            LC.EventChangeLineStyle += new ChangePaintParam(MC.MyCanvas_ChangePaintParam);//привязка к событию изменения типа линии
            this.EventLineWidth += new ChangePaintParam(MC.MyCanvas_ChangePaintParam);//привязка к событию изменения толщины линии
            this.EventPressButt += new ChangePressBut(MC.MyCanvas_ChangePressButt);//привязка к событию изменения нажатой кнопки выбора Инструмента рисования

            this.DoubleBuffered = true;
        }

        //обработчик события изменения статуса кнопок Back и Farward
        void ChangeButtonBF(bool Back, bool Forward)
        {
            if (Back == true)
            {
                this.toolStripButton_Back.Enabled = true;
            }
            else
            {
                this.toolStripButton_Back.Enabled = false;
            }
            if (Forward == true)
            {
                this.toolStripButton_Forward.Enabled = true;
            }
            else
            {
                this.toolStripButton_Forward.Enabled = false;
            }
        } 
        
        //обработчик отрисовки для создания кульмана для холста
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 2; i <= 5; i++)
            {
                ControlPaint.DrawBorder3D(g, MC.Top-i, MC.Left-i, MC.Width+i*2, MC.Height+i*2, Border3DStyle.Sunken);
            }
            for (int i = 1; i <= 2; i++)
            {
                ControlPaint.DrawBorder3D(g, MC.Top-i, MC.Left-i, MC.Width+i*2, MC.Height+i*2, Border3DStyle.Raised);
            }            
        }
        
        //обработчики кнопок туллбара
        #region
        private void toolStripButton_Click(object sender, EventArgs e)
        {
            this.toolStripButton_line.Checked = false;
            this.toolStripButton_rect.Checked = false;
            this.toolStripButton_ell.Checked = false;
            this.toolStripButton_fillRect.Checked = false;
            this.toolStripButton_fillEll.Checked = false;
            this.toolStripButton_Brush.Checked = false;
            this.toolStripButton_Pen.Checked = false;
            this.toolStripButton_Eraser.Checked = false;
            this.toolStripButton_FloodFill.Checked = false;
            this.toolStripButton_pipet.Checked = false;
            this.toolStripButton_MainFillArrow.Checked = false;
            this.toolStripButton_Text.Checked = false;
            this.toolStripButton_Batman.Checked = false;
            this.toolStripButton_FillBatman.Checked = false;
            this.toolStripButton_MainArrow.Checked = false;

            //для исчезновения пипетки
            MC.bgGraph_1st.Render(MC.bgGraph_2nd.Graphics);            
            this.MC.Invalidate();
            //------------------------

            //Изменение курсора
            MC.Cursor = Cursors.Default;

            //Исчезновение текстового поля
            MC.Controls[0].Visible = false;

            //линия
            if (sender == this.toolStripButton_line)
            {
                this.toolStripButton_line.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.line));
            }
            //контур прямоуг
            else if (sender == this.toolStripButton_rect)
            {
                this.toolStripButton_rect.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.rect));
            }
            //контур эллипса
            else if (sender == this.toolStripButton_ell)
            {
                this.toolStripButton_ell.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.ell));
            }
            //залитый прямоуг
            else if (sender == this.toolStripButton_fillRect)
            {
                this.toolStripButton_fillRect.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.fillRect));
            }
            //залитый эллипс
            else if (sender == this.toolStripButton_fillEll)
            {
                this.toolStripButton_fillEll.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.fillEll));
            }
            //кисть
            else if (sender == this.toolStripButton_Brush)
            {
                this.toolStripButton_Brush.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.brush));
                MC.Cursor=MC.myBcur;
            }
            //карандаш
            else if (sender == this.toolStripButton_Pen)
            {
                this.toolStripButton_Pen.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.pen));
                MC.Cursor=MC.myPcur;
            }
            //ластик
            else if (sender == this.toolStripButton_Eraser)
            {
                this.toolStripButton_Eraser.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.eraser));
                MC.Cursor = MC.myNcur;//замена курсора
            }
            //заливка
            else if (sender == this.toolStripButton_FloodFill)
            {
                this.toolStripButton_FloodFill.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.floodfill));
            }
            //пипетка
            else if (sender == this.toolStripButton_pipet)
            {
                this.toolStripButton_pipet.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.pipete));
                MC.Cursor=MC.myPIPcur;
            }
            //стрелка
            else if (sender == this.toolStripButton_MainArrow)
            {
                this.toolStripButton_MainArrow.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(ArrowWay));
            }
            //залитая стрелка
            else if (sender == this.toolStripButton_MainFillArrow)
            {
                this.toolStripButton_MainFillArrow.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(FillArrowWay));
            }
            //текст
            else if (sender == this.toolStripButton_Text)
            {
                this.toolStripButton_Text.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.text));
            }
            //контур Бэтмена
            else if (sender == this.toolStripButton_Batman)
            {
                this.toolStripButton_Batman.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.batman));
            }
            //залитый бэтмен
            else if (sender == this.toolStripButton_FillBatman)
            {
                this.toolStripButton_FillBatman.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(drawButton.fillBatman));
            }
        }
        
        private void создать_ToolStripButton_Click(object sender, EventArgs e)
        {
            Form2 F = new Form2();
            DialogResult Dr = F.ShowDialog(); //показать модальное окно 
            if (Dr == DialogResult.OK)
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "Картинки (*.bmp)|*.bmp";
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(MC.Width, MC.Height, MC.bgGraph_2nd.Graphics);
                    bmp.Save(SFD.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                 
            }
            
            else if (Dr == DialogResult.Cancel)
            {
                return;
            }

            //-----------------------------------------------
            MC.bgGraph_2nd.Graphics.Clear(Color.White);

            MC.bgGraph_2nd.Render(MC.bgGraph_1st.Graphics);
            //------------------------------------------------

            #region
            //int w=100, h=100;

            //Form3 F3 = new Form3();
            //DialogResult Dr3 = F3.ShowDialog(); //показать модальное окно 
            //if (Dr3 == DialogResult.OK)
            //{
            //    w = (int)F3.numericUpDown1.Value;
            //    h = (int)F3.numericUpDown2.Value;
            //}

            //else if (Dr == DialogResult.Cancel)
            //{
            //    return;
            //}

            //MC.Dispose();

            //MC = new MyCanvas();

            //MC.Width = w;
            //MC.Height = h;

            //MC.Top = 10;
            //MC.Left = 10;

            //MC.bgGraph_2nd.Dispose();

            //MC.bgGraph_2nd.Dispose();

            //context = BufferedGraphicsManager.Current;
            //context.MaximumBuffer = new Size(MC.Width + 1, MC.Height + 1);

            //MC.bgGraph_1st = context.Allocate(MC.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

            //MC.bgGraph_2nd = context.Allocate(MC.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

            //MC.bgGraph_2nd.Graphics.Clear(Color.White);

            //MC.bgGraph_2nd.Render(MC.bgGraph_1st.Graphics);

            ////---------------

            //Graphics g = this.splitContainer1.Panel2.CreateGraphics();
            //g.Clear(SystemColors.ButtonFace);
            //for (int i = 2; i <= 5; i++)
            //{
            //    ControlPaint.DrawBorder3D(g, MC.Top-i, MC.Left-i, MC.Width+i*2, MC.Height+i*2, Border3DStyle.Sunken);
            //}
            //for (int i = 1; i <= 2; i++)
            //{
            //    ControlPaint.DrawBorder3D(g, MC.Top-i, MC.Left-i, MC.Width+i*2, MC.Height+i*2, Border3DStyle.Raised);
            //}

            //g.Dispose();
            ////---------------------

            //this.splitContainer1.Panel2.Controls.Add(MC);

            //((MyCanvas)this.splitContainer1.Panel2.Controls[0]).EventChangeButton += new ChangeBackForward(ChangeButtonBF);
            //((ColorChooser)this.splitContainer2.Panel1.Controls[0]).EventColorChange += new ChangePaintParam(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePaintParam);
            //((MyCanvas)this.splitContainer1.Panel2.Controls[0]).EventChangeColorPipet += new ChangePaintParam(((ColorChooser)this.splitContainer2.Panel1.Controls[0]).ChangeColorPipet);
            //((TextureChooser)this.splitContainer3.Panel1.Controls[0]).EventTextureChange += new ChangePaintParam(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePaintParam);
            //((LineChuser)this.splitContainer3.Panel2.Controls[1]).EventChangeLineStyle += new ChangePaintParam(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePaintParam);
            //this.EventLineWidth += new ChangePaintParam(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePaintParam);
            //this.EventPressButt += new ChangePressBut(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePressButt);
            #endregion
            ////-----------------------------

            ChangeButtonBF(true, false); // Back - активна

            MC.History.Clear();
            MC.numHist = 0;
            this.toolStripButton_Back.Enabled = false;


            //Добавление чистого листа в историю 
            Bitmap tmp_bmp = new Bitmap(this.MC.Width, this.MC.Height);
            Graphics gbmp = Graphics.FromImage(tmp_bmp);

            MC.bgGraph_2nd.Render(gbmp);

            //if (MC.History.Count >= MC.CountHist && MC.History.Count != 0)
            //{
            //    MC.History.RemoveAt(0);
            //}
            MC.History.Add(tmp_bmp);
            
            
            MC.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numericUpDown1.Value < numericUpDown1.Minimum)
            {
                numericUpDown1.Value = (int)numericUpDown1.Minimum;
            }
            else if((int)numericUpDown1.Value > numericUpDown1.Maximum)
            {
                numericUpDown1.Value = (int)numericUpDown1.Maximum;
            }
            this.EventLineWidth(this, new PaintParamEventArgs(PaintParam.WidthLine, (int)this.numericUpDown1.Value));
        }

        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)numericUpDown1.Value < numericUpDown1.Minimum)
            {
                numericUpDown1.Value = (int)numericUpDown1.Minimum;
            }
            else if ((int)numericUpDown1.Value > numericUpDown1.Maximum)
            {
                numericUpDown1.Value = (int)numericUpDown1.Maximum;
            }
            //*Form1.*/ActWidthLine = (int)numericUpDown1.Value;
            this.EventLineWidth(this, new PaintParamEventArgs(PaintParam.WidthLine, (int)this.numericUpDown1.Value));
        }

        private void открыть_ToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Картинки (*.bmp)|*.bmp";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                Image FonIm = Image.FromFile(OFD.FileName);

                Graphics gb1 = MC.bgGraph_1st.Graphics;
                Graphics gb2 = MC.bgGraph_2nd.Graphics;
                MC.bgGraph_1st.Render(gb2);

                //создание нового размера листа
                #region
                //MC.Dispose();

                //MC = new MyCanvas();

                //MC.Width = FonIm.Width;
                //MC.Height = FonIm.Height;

                //MC.Top = 10;
                //MC.Left = 10;

                //MC.bgGraph_2nd.Dispose();

                //MC.bgGraph_2nd.Dispose();

                //context = BufferedGraphicsManager.Current;
                //context.MaximumBuffer = new Size(MC.Width + 1, MC.Height + 1);

                //MC.bgGraph_1st = context.Allocate(MC.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

                //MC.bgGraph_2nd = context.Allocate(MC.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

                //MC.bgGraph_2nd.Graphics.Clear(Color.White);

                //MC.bgGraph_2nd.Render(MC.bgGraph_1st.Graphics);

                ////---------------

                //Graphics g = this.splitContainer1.Panel2.CreateGraphics();
                //g.Clear(SystemColors.ButtonFace);
                //for (int i = 2; i <= 5; i++)
                //{
                //    ControlPaint.DrawBorder3D(g, MC.Top-i, MC.Left-i, MC.Width+i*2, MC.Height+i*2, Border3DStyle.Sunken);
                //}
                //for (int i = 1; i <= 2; i++)
                //{
                //    ControlPaint.DrawBorder3D(g, MC.Top-i, MC.Left-i, MC.Width+i*2, MC.Height+i*2, Border3DStyle.Raised);
                //}

                //g.Dispose();
                ////---------------------

                //this.splitContainer1.Panel2.Controls.Add(MC);

                //((MyCanvas)this.splitContainer1.Panel2.Controls[0]).EventChangeButton += new ChangeBackForward(ChangeButtonBF);
                //((ColorChooser)this.splitContainer2.Panel1.Controls[0]).EventColorChange += new ChangePaintParam(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePaintParam);
                //((MyCanvas)this.splitContainer1.Panel2.Controls[0]).EventChangeColorPipet += new ChangePaintParam(((ColorChooser)this.splitContainer2.Panel1.Controls[0]).ChangeColorPipet);
                //((TextureChooser)this.splitContainer3.Panel1.Controls[0]).EventTextureChange += new ChangePaintParam(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePaintParam);
                //((LineChuser)this.splitContainer3.Panel2.Controls[1]).EventChangeLineStyle += new ChangePaintParam(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePaintParam);
                //this.EventLineWidth += new ChangePaintParam(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePaintParam);
                //this.EventPressButt += new ChangePressBut(((MyCanvas)this.splitContainer1.Panel2.Controls[0]).MyCanvas_ChangePressButt);
                #endregion

                gb2.DrawImage(FonIm, new Rectangle(0, 0, this.MC.Width, this.MC.Height), new Rectangle(0, 0, FonIm.Width, (int)((float)FonIm.Width/((float)this.MC.Width/(float)this.MC.Height))), GraphicsUnit.Pixel);
                
                MC.bgGraph_2nd.Render(gb1);

                //Добавление лисат в историю 
                Bitmap tmp_bmp = new Bitmap(this.MC.Width, this.MC.Height);
                Graphics gbmp = Graphics.FromImage(tmp_bmp);

                MC.bgGraph_2nd.Render(gbmp);

                if (MC.History.Count >= MC.CountHist)
                {
                    MC.History.RemoveAt(0);
                }
                MC.History.Add(tmp_bmp);
                if (this.toolStripButton_Back.Enabled == false)
                {
                    this.toolStripButton_Back.Enabled = true;
                }
                //---------------------

                this.MC.Invalidate();
            }
        }

        private void сохранить_ToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "Картинки (*.bmp)|*.bmp";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                //Сохраняю в историю
                Bitmap bmp = new Bitmap(MC.Width, MC.Height, MC.bgGraph_2nd.Graphics);
                Graphics gbmp = Graphics.FromImage(bmp);
                                
                MC.bgGraph_2nd.Render(gbmp);
                //-----------------

                bmp.Save(SFD.FileName, System.Drawing.Imaging.ImageFormat.Bmp); 
            } 
        }

        private void toolStripButton_Back_Click(object sender, EventArgs e)
        {
            this.toolStripButton_Forward.Enabled = true;
            MC.isWalkHistory = true;

            MC.numHist +=1;

            Graphics gb1 = MC.bgGraph_1st.Graphics;
            Graphics gb2 = MC.bgGraph_2nd.Graphics;

            Bitmap tmp_bmp = MC.History[MC.History.Count-MC.numHist-1];

            gb2.DrawImage(tmp_bmp, new Rectangle(0, 0, MC.Width, MC.Height));
            MC.bgGraph_2nd.Render(gb1);
            this.MC.Invalidate();

            if (MC.numHist >= MC.History.Count-1)
            {
                this.toolStripButton_Back.Enabled = false;
            }
        }

        private void toolStripButton_Forward_Click(object sender, EventArgs e)
        {
            this.toolStripButton_Back.Enabled = true;

            MC.numHist -=1;

            Graphics gb1 = MC.bgGraph_1st.Graphics;
            Graphics gb2 = MC.bgGraph_2nd.Graphics;

            Bitmap tmp_bmp = MC.History[MC.History.Count-MC.numHist-1];

            gb2.DrawImage(tmp_bmp, new Rectangle(0, 0, MC.Width, MC.Height));
            MC.bgGraph_2nd.Render(gb1);
            this.MC.Invalidate();

            if (MC.numHist <= 0)
            {
                this.toolStripButton_Forward.Enabled = false;
                MC.isWalkHistory = false;
            }
        }

        private void toolStripButton_plus_Click(object sender, EventArgs e)
        {
            if (i_plus<5)
            {
                Graphics gb2 = MC.bgGraph_2nd.Graphics;
                Graphics gb1 = MC.bgGraph_1st.Graphics;

                this.toolStripButton_minus.Enabled = true;
                
                int w = (int)(MC.Width/(2*Math.Pow(2.0, (double)i_plus)));
                int h = (int)(MC.Height/(2*Math.Pow(2.0, (double)i_plus)));

                Form1.StretchBlt(gb2.GetHdc(), 0, 0, MC.Width, MC.Height, gb1.GetHdc(), (MC.Width-w)/2, (MC.Height-h)/2, w, h, 0xcc0020);
                gb2.ReleaseHdc();
                gb1.ReleaseHdc();
                //gb2.DrawRectangle(Pens.Black, new Rectangle(e.X - 50, e.Y - 50, 100, 100));

                i_plus++;

                this.MC.Invalidate();
            }
            if (i_plus==5)
            {
                this.toolStripButton_plus.Enabled = false;
            } 
        }

        private void toolStripButton_minus_Click(object sender, EventArgs e)
        {
            if (i_plus>0)
            {
                Graphics gb2 = MC.bgGraph_2nd.Graphics;
                Graphics gb1 = MC.bgGraph_1st.Graphics;

                this.toolStripButton_plus.Enabled = true;

                i_plus--;

                int w = (int)(MC.Width/(2*Math.Pow(2.0, (double)i_plus-1)));
                int h = (int)(MC.Height/(2*Math.Pow(2.0, (double)i_plus-1)));

                Form1.StretchBlt(gb2.GetHdc(), 0, 0, MC.Width, MC.Height, gb1.GetHdc(), (MC.Width-w)/2, (MC.Height-h)/2, w, h, 0xcc0020);
                gb2.ReleaseHdc();
                gb1.ReleaseHdc();
                //gb2.DrawRectangle(Pens.Black, new Rectangle(e.X - 50, e.Y - 50, 100, 100));

                this.MC.Invalidate();
                //MessageBox.Show(""+i_plus);
            }
            if (i_plus==0)
            {
                this.toolStripButton_minus.Enabled = false;
            }
        }

        private void toolStripButton_Click_Arrow(object sender, EventArgs e)
        {
            if (sender == this.toolStripButton_ArrowR || sender == this.toolStripButton_ArrowL)
            {
                if (sender == this.toolStripButton_ArrowR)
                {
                    this.toolStripButton_MainArrow.Image = this.toolStripButton_ArrowR.Image;
                    toolStripButton_Click(this.toolStripButton_ArrowR, e);
                    ArrowWay = drawButton.arrowR;
                }
                else if (sender == this.toolStripButton_ArrowL)
                {
                    this.toolStripButton_MainArrow.Image = this.toolStripButton_ArrowL.Image;
                    toolStripButton_Click(this.toolStripButton_ArrowL, e);
                    ArrowWay = drawButton.arrowL;
                }
                this.toolStripButton_MainArrow.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(ArrowWay));
            }
            if (sender == this.toolStripButton_FillArrowR || sender == this.toolStripButton_FillArrowL)
            {
                if (sender == this.toolStripButton_FillArrowR)
                {
                    this.toolStripButton_MainFillArrow.Image = this.toolStripButton_FillArrowR.Image;
                    toolStripButton_Click(this.toolStripButton_FillArrowR, e);
                    FillArrowWay = drawButton.fillArrowR;
                }
                else if (sender == this.toolStripButton_FillArrowL)
                {
                    this.toolStripButton_MainFillArrow.Image = this.toolStripButton_FillArrowL.Image;
                    toolStripButton_Click(this.toolStripButton_FillArrowL, e);
                    FillArrowWay = drawButton.fillArrowL;
                }
                this.toolStripButton_MainFillArrow.Checked = true;
                this.EventPressButt(this, new ChangePressButtonEventArgs(FillArrowWay));
            }
        }
        #endregion

        

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Invalidate();
        }
    }
        
    public delegate void ChangeBackForward(bool Back, bool Forward); //Делегат для события изменения активности кнопок Back и Farward
    public delegate void ChangePaintParam(object sender, PaintParamEventArgs e); //Делегат для события изменения параметров инструментов рисования
    public delegate void ChangePressBut(object sender, ChangePressButtonEventArgs e); //Делегат для события изменения кнопки выбора инструмента
    public delegate void DrawText(object sender, DrawTextEventArgs e); //Делегат для события получения графикса для Текста
   
    
    /// <summary>
    /// Панель выбора цвета
    /// </summary>
    class ColorChooser : Control
    {
        private int dx = 3; 
        private int dy = 2;
        
        //основные цвета
        private Color[] color = { Color.Black, Color.Gray, Color.DarkGray, Color.LightGray, Color.Red, Color.Blue, 
                                    Color.Green, Color.Yellow, Color.DarkRed, Color.DarkBlue, Color.DarkGreen, Color.Gold, 
                                    Color.DarkSlateGray, Color.SlateGray, Color.LightSlateGray, Color.White };
        //--------------

        //дополнительные цвета
        private Color[] privateColor = {SystemColors.ButtonFace, SystemColors.ButtonFace, SystemColors.ButtonFace, SystemColors.ButtonFace};
        //----------

        private Color FColor = Color.Black;
        private Color BColor = Color.White;
        
        public event ChangePaintParam EventColorChange;//событие изменения цвета (основного и фонового)

        /// <summary>
        /// обработчик события смены цвета пипеткой        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChangeColorPipet(object sender, PaintParamEventArgs e)
        {
            if (e.NumParam == PaintParam.FColor)
            {
                this.FColor = e.FColor;
            }
            else if (e.NumParam == PaintParam.BColor)
            {
                this.BColor = e.BColor;
            }
            this.Invalidate();
        }

        /// <summary>
        /// конструктор
        /// </summary>
        public ColorChooser()
        {
            this.Paint += new PaintEventHandler(ColorChooser_Paint);

            this.SizeChanged += new EventHandler(ColorChooser_SizeChanged);

            this.MouseClick += new MouseEventHandler(ColorChooser_MouseClick);

            this.MouseDoubleClick += new MouseEventHandler(ColorChooser_MouseDoubleClick);

            this.DoubleBuffered = true;
        }

        void ColorChooser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int w = (this.Width - 5 * dx + 1) / 4;

                int h = (int)((this.Height - 8 * dy) / 6.5);

                for (int i = 0; i < 4; i++)
                {
                    Rectangle R = new Rectangle((int)(dx+(w+dx)*i), this.Height-2*dy-h, w, h);

                    if (R.Contains(e.Location))
                    {
                        ColorDialog CD = new ColorDialog();

                        if (CD.ShowDialog() == DialogResult.OK)
                        {
                            privateColor[i] = CD.Color;
                            this.Invalidate();
                        }
                        this.Invalidate();
                        break;
                    }
                }
            }
            
        }

        void ColorChooser_MouseClick(object sender, MouseEventArgs e)
        {
            int w = (this.Width - 5 * dx + 1) / 4;

            int h = (int)((this.Height - 8 * dy) / 6.5);

            //основные цвета
            #region
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle R = new Rectangle((int)(dx+(w+dx)*j), (int)((dy+1.5*h)+dy+(h+dy)*i), w, h);

                    if (R.Contains(e.Location))
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            this.FColor = color[i * 4 + j];
                            //Form1.ActFColor = this.FColor;
                            this.EventColorChange(this, new PaintParamEventArgs(PaintParam.FColor, this.FColor, Color.White));
                        }
                        else if (e.Button == MouseButtons.Right)
                        {
                            this.BColor = color[i * 4 + j];
                            //Form1.ActBColor = this.BColor;
                            this.EventColorChange(this, new PaintParamEventArgs(PaintParam.BColor, Color.Black, this.BColor));
                        }
                        this.Invalidate();
                        break;
                    }
                }
            }
            #endregion

            //дополнительные цвета
            #region
            for (int i = 0; i < 4; i++)
            {
                Rectangle R = new Rectangle((dx+(w+dx)*i), this.Height-2*dy-h, w, h);

                if (R.Contains(e.Location))
                {
                    if (e.Button == MouseButtons.Left && privateColor[i] != SystemColors.ButtonFace)
                    {
                        this.FColor = privateColor[i];
                        this.EventColorChange(this, new PaintParamEventArgs(PaintParam.FColor, this.FColor, Color.White));
                    }
                    else if (e.Button == MouseButtons.Right && privateColor[i] != SystemColors.ButtonFace)
                    {
                        this.BColor = privateColor[i];
                        this.EventColorChange(this, new PaintParamEventArgs(PaintParam.BColor, Color.Black, this.BColor));
                    }
                    this.Invalidate();
                    break;
                }
            }
            #endregion
        }

        void ColorChooser_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void ColorChooser_Paint(object sender, PaintEventArgs e)
        {
            int w = (this.Width - 5 * dx + 1) / 4;

            int h = (int)((this.Height - 8 * dy) / 6.5);

            Graphics g = e.Graphics;
            g.Clear(SystemColors.ControlLight);
            
            ControlPaint.DrawBorder3D(g, 0, 0, Width-1, Height-1, Border3DStyle.Sunken);
            
            //основные цвета
            g.FillRectangle(new SolidBrush(this.BColor), (int)(0.375 * Width), (int)(dy + 0.5 * h), (int)(0.5 * Width), (int)(h));
            ControlPaint.DrawBorder3D(g, (int)(0.375 * Width), (int)(dy + 0.5 * h), (int)(0.5 * Width), (int)(h), Border3DStyle.Raised);
            //--------------
            
            //дополнительные цвета
            g.FillRectangle(new SolidBrush(this.FColor), (int)(0.125 * Width), (int)dy, (int)(0.5 * Width), (int)(h));
            ControlPaint.DrawBorder3D(g, (int)(0.125 * Width), (int)dy, (int)(0.5 * Width), (int)(h), Border3DStyle.Raised);
            //-----------------------

            //основные цвета
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    g.FillRectangle(new SolidBrush(color[i*4+j]), (int)(dx+(w+dx)*j), (int)((dy+1.5*h)+dy+(h+dy)*i), w, h);
                    ControlPaint.DrawBorder3D(g, (int)(dx+(w+dx)*j), (int)((dy+1.5*h)+dy+(h+dy)*i), w, h, Border3DStyle.Bump);
                }
            }
            //-----------------

            //дополнительные цвета
            for (int i = 0; i < 4; i++)
            {
                g.FillRectangle(new SolidBrush(privateColor[0*4+i]), (int)(dx+(w+dx)*i), this.Height-2*dy-h, w, h);
                ControlPaint.DrawBorder3D(g, (int)(dx+(w+dx)*i), this.Height-2*dy-h, w, h, Border3DStyle.Bump);
            }
            //-----------------___
        }
    }

    /// <summary>
    /// Панель выбора штриховки
    /// </summary>
    class TextureChooser : Control
    {
        private int dx = 5;
        private int dy = 5;

        private HatchStyle[] ArTexStyle = { HatchStyle.Cross, HatchStyle.DiagonalCross, HatchStyle.Vertical, HatchStyle.ForwardDiagonal, 
                                              HatchStyle.Horizontal, HatchStyle.BackwardDiagonal, HatchStyle.LargeCheckerBoard, HatchStyle.SolidDiamond };

        private HatchBrush textures = new HatchBrush(HatchStyle.Cross, Color.Black, Color.White);

        public event ChangePaintParam EventTextureChange;

        /// <summary>
        /// конструктор
        /// </summary>
        public TextureChooser()
        {
            this.Paint += new PaintEventHandler(TextueChooser_Paint);

            this.SizeChanged += new EventHandler(TextueChooser_SizeChanged);

            this.MouseClick += new MouseEventHandler(TextueChooser_MouseClick);

            this.DoubleBuffered = true;
        }
                
        void TextueChooser_MouseClick(object sender, MouseEventArgs e)
        {
            int w = (this.Width - 3 * dx + 1) / 2;
            int h = (int)((this.Height - 7 * dy) / 5.5);
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Rectangle R = new Rectangle((int)(dx+(w+dx)*j), (int)((dy+1.5*h)+dy+(h+dy)*i), w, h);

                    if (R.Contains(e.Location))
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            HatchBrush HB = new HatchBrush(ArTexStyle[i*2+j], Color.Black, Color.White);
                            this.EventTextureChange(this, new PaintParamEventArgs(PaintParam.Texture, ArTexStyle[i*2+j], ArTexStyle[7]));
                            //this.EventTextureChange(this, new PaintParamEventArgs(PaintParam.Solid, ArTexStyle[7]));
                            this.textures = HB;
                        }
                        this.Invalidate();
                        break;
                    }
                }
            }
        }

        void TextueChooser_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void TextueChooser_Paint(object sender, PaintEventArgs e)
        {
            int w = (this.Width - 3 * dx + 1) / 2;
            
            int h = (int)((this.Height - 6 * dy) / 5.5);

            Graphics g = e.Graphics;
            g.Clear(SystemColors.ControlLight);
            
            ControlPaint.DrawBorder3D(g, 0, 0, Width-1, Height-1, Border3DStyle.Sunken); //граница ЭУ

            if (textures.HatchStyle == ArTexStyle[7])//создание сплошной заливки
            {
                textures = new HatchBrush(HatchStyle.Cross, Color.Black, Color.Black);
            }

            //квадрат с активной штриховкой
            g.FillRectangle(textures, (int)(dx), (int)dy, w*2+dx, (int)(h+h/2));
            
            ControlPaint.DrawBorder3D(g, (int)(dx), (int)dy, w*2+dx, (int)(h+h/2), Border3DStyle.Raised);
            //-------------------------

            //квадраты выбора штриховки
            #region
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Color BackHatchColor = Color.White;

                    if (i==3 && j==1)//создание сплошной заливки
                    {
                        BackHatchColor = Color.Black;
                    }

                    HatchBrush tex = new HatchBrush(ArTexStyle[i*2+j], Color.Black, BackHatchColor);

                    g.FillRectangle(tex, (int)(dx+(w+dx)*j), (int)((dy+1.5*h)+dy+(h+dy)*i), w, h);
                    
                    ControlPaint.DrawBorder3D(g, (int)(dx+(w+dx)*j), (int)((dy+1.5*h)+dy+(h+dy)*i), w, h, Border3DStyle.Bump);
                }
            }
            #endregion
        }        
    }

    /// <summary>
    /// Панель выбора типа линии
    /// </summary>
    class LineChuser : Control
    {
        enum pressLine { Dash, DashDot, DashDotDot, Dot, Solid };

        pressLine PressLine = pressLine.Solid;
        pressLine newPressLine = pressLine.Solid;

        private int dx = 0;
        private int dy = 0;

        private DashStyle[] LineStyle = { DashStyle.Dash, DashStyle.DashDot, DashStyle.DashDotDot, DashStyle.Dot, DashStyle.Solid };

        private DashStyle Lstyle = DashStyle.Solid;// выбранный тип линии

        private bool isDawn = false;

        public event ChangePaintParam EventChangeLineStyle; //событие изменения стиля линии

        /// <summary>
        /// конструктор
        /// </summary>
        public LineChuser()
        {
            this.Paint +=new PaintEventHandler(LineChuser_Paint);
            //this.MouseClick +=new MouseEventHandler(LineChuser_MouseClick);
            this.MouseDown +=new MouseEventHandler(LineChuser_MouseDown);
            this.MouseMove +=new MouseEventHandler(LineChuser_MouseMove);
            this.MouseUp +=new MouseEventHandler(LineChuser_MouseUp);            

            this.DoubleBuffered = true;
        }        

        void LineChuser_MouseUp(object sender, MouseEventArgs e)
        {
            isDawn = false;
            PressLine = newPressLine;
            this.Invalidate();
        }

        void LineChuser_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDawn == true)
            {
                int w = (this.Width - 2 * dx + 1) / 1;

                int h = (int)((this.Height - 6 * dy) / 5);

                for (pressLine i = pressLine.Dash; i <= pressLine.Solid; i++)
                {
                    Rectangle R = new Rectangle((int)(dx), (int)((dy)+dy+(h+dy)*(int)i), w, h);

                    if (R.Contains(e.Location))
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            this.Lstyle = LineStyle[(int)i];
                            newPressLine = i;
                            this.EventChangeLineStyle(this, new PaintParamEventArgs(PaintParam.LineStyle, this.Lstyle));
                        }

                        this.Invalidate();
                        break;
                    }

                }
            }
            
        }

        void LineChuser_MouseDown(object sender, MouseEventArgs e)
        {
            isDawn = true;

            int w = (this.Width - 2 * dx + 1) / 1;

            int h = (int)((this.Height - 6 * dy) / 5);

            for (pressLine i = pressLine.Dash; i <= pressLine.Solid; i++)
            {
                Rectangle R = new Rectangle((int)(dx), (int)((dy)+dy+(h+dy)*(int)i), w, h);

                if (R.Contains(e.Location))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Lstyle = LineStyle[(int)i];
                        newPressLine = i;
                        this.EventChangeLineStyle(this, new PaintParamEventArgs(PaintParam.LineStyle, this.Lstyle));
                    }

                    this.Invalidate();
                    break;
                }

            }
        }        

        void LineChuser_Paint(object sender, PaintEventArgs e)
        {
            int w = (this.Width - 2 * dx + 1) / 1;

            int h = (int)((this.Height - 6 * dy) / 5);

            Graphics g = e.Graphics;
            g.Clear(SystemColors.ButtonFace);

            for (pressLine i = pressLine.Dash; i <= pressLine.Solid; i++)
            {

                g.FillRectangle(new SolidBrush(SystemColors.ButtonFace), (int)(dx), (int)((dy)+dy+(h+dy)*(int)i), w, h);
                
                if (i == PressLine || i == newPressLine)//нажатая кнопка
                {
                     ControlPaint.DrawButton(g, (int)(dx), (int)((dy) + dy + (h + dy) * (int)i), w, h, ButtonState.Pushed);
                     Pen SamplePen = new Pen(Color.Black, 3);
                     SamplePen.DashStyle = LineStyle[(int)i];
                     g.DrawLine(SamplePen, dx+6, dy+h/2+h*(int)i+1, dx+w-(7+2), dy+h/2+h*(int)i+1);
                     SamplePen.Dispose();
                }
                else //ненажатая кнопка
                {
                    ControlPaint.DrawButton(g, (int)(dx), (int)((dy) + dy + (h + dy) * (int)i), w, h, ButtonState.Normal);
                    Pen SamplePen = new Pen(Color.Black, 3);
                    SamplePen.DashStyle = LineStyle[(int)i];
                    g.DrawLine(SamplePen, dx+6, dy+h/2+h*(int)i, dx+w-(6+3), dy+h/2+h*(int)i);
                    SamplePen.Dispose();
                }
            }
       }
    }

    /// <summary>
    /// Панель создания текста
    /// </summary>
    class TextInput : Control
    {
        public event DrawText EventDrawText; //событие для вставки текста на холст

        /// <summary>
        /// конструктор
        /// </summary>
        public TextInput()
        {
            TextBox TB = new TextBox();
            Button BF = new Button();
            Button BC = new Button();
            Button BE = new Button();

        
            TB.Location = new Point(0, 0);
            TB.Multiline = true;
            TB.WordWrap = false;
            //TB.TextAlign = HorizontalAlignment.Left;
            TB.Size = new Size(this.Width, this.Height-30);
            TB.Focus();
            TB.Font = new Font("Arial", 25);
            this.Controls.Add(TB);

            BF.Location = new Point(0, this.Controls[0].Height);
            BF.Size = new Size(50, 30);
            BF.Text = "Шрифт";
            this.Controls.Add(BF);

            BC.Location = new Point(50, this.Controls[0].Height);
            BC.Size = new Size(50, 30);
            BC.Text = "Цвет";
            this.Controls.Add(BC);

            BE.Location = new Point(100, this.Controls[0].Height);
            BE.Size = new Size(50, 30);
            BE.Text = "ОК";
            this.Controls.Add(BE);

            //this.Paint += new PaintEventHandler(TextInput_Paint);
            this.SizeChanged += new EventHandler(TextInput_SizeChanged);

            this.Controls[1].MouseClick += new MouseEventHandler(TextInput_Button_Font_MouseClick);
            this.Controls[2].MouseClick += new MouseEventHandler(TextInput_Button_Color_MouseClick);
            this.Controls[3].MouseClick += new MouseEventHandler(TextInput_Button_Enter_MouseClick);
        }

        public void changLocationButton()
        {
            //this.Controls[0].Location = new Point(0, 30);
            this.Controls[1].Location = new Point(0, 0);
            this.Controls[2].Location = new Point(50, 0);
            this.Controls[3].Location = new Point(100, 0);
            this.Invalidate();
        }

        public void changToDefaultLocationButton()
        {
            this.Controls[0].Location = new Point(0, 0);
            this.Controls[1].Location = new Point(0, this.Controls[0].Height);
            this.Controls[2].Location = new Point(50, this.Controls[0].Height);
            this.Controls[3].Location = new Point(100, this.Controls[0].Height);
        }


        /// <summary>
        /// обработчик события установки фонового цвета формы создания текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">передаваемые параметры</param>
        public void SetBackTextColor(object sender, PaintParamEventArgs e)
        {
            this.Controls[0].BackColor = e.BColor;
        }

        
        
        /// <summary>
        /// Обработчик кнопки ОК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextInput_Button_Enter_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            //MessageBox.Show(((TextBox)this.Controls[0]).Lines[0]);
            this.EventDrawText(this, new DrawTextEventArgs(this.Controls[0].Text, this.Controls[0].Font, this.Controls[0].ForeColor, this.Location));
            this.Controls[0].Text = "";//.Remove(0);
        }

        /// <summary>
        /// обработчик кнопки Цвет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextInput_Button_Color_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog CD = new ColorDialog();
            if (DialogResult.OK == CD.ShowDialog())
            {
                this.Controls[0].ForeColor = CD.Color;
            }
        }

        /// <summary>
        /// обработчик кнопки Шрифт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextInput_Button_Font_MouseClick(object sender, MouseEventArgs e)
        {
            FontDialog FD = new FontDialog();

            while (true)
            {
                try
                {
                    FD.Font = this.Controls[0].Font;
                    if (FD.ShowDialog() == DialogResult.OK)
                    {
                        this.Controls[0].Font = FD.Font;
                    }
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show("Данный шрифт не подходит. \nВыберите, пожалуйста другой.");
                    continue;
                }
            }
        }

        void TextInput_SizeChanged(object sender, EventArgs e)
        {
            this.Controls[0].Size = new Size(this.Width, this.Height-30);
            this.Controls[1].Location = new Point(0, this.Controls[0].Height);
            this.Controls[2].Location = new Point(50, this.Controls[0].Height);
            this.Controls[3].Location = new Point(100, this.Controls[0].Height);
        }

        void TextInput_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //g.Clear(Color.Red);

            
        }
    }

    /// <summary>
    /// Холст
    /// </summary>
    class MyCanvas : Control
    {
        Color ActFColor = Color.Black;//выбранный основной цвет
        Color ActBColor = Color.White;//выбранный фоновый цвет
        HatchStyle ActTexture = HatchStyle.Cross;//вырбранная штриховка
        DashStyle ActLineStyle = DashStyle.Solid;//выбранный тип линии
        int ActWidthLine = 5;//выбранная толщина линии      

        HatchStyle Solid = HatchStyle.SolidDiamond;

        Point StartPred = new Point(0, 0); //для кривой
        Point StartP = new Point(0, 0); //начальная точка для рисования кистью
        
        bool isPaint = false; //метка начала рисования
        bool isLens = false; //метка для начала линзы

        drawButton pressBut = drawButton.line;//метка выбранного инструмента

        GraphicsPath PathBrush = new GraphicsPath();//путь для кисти и карандаша
        GraphicsPath GlobalPathBrush = new GraphicsPath(); //полный путь для кисти и карандаша

        int count = 0;//оптимизация отрисовки пути 
        
        //Буфферизация   
        private BufferedGraphicsContext context;

        public BufferedGraphics bgGraph_1st;
                
        public BufferedGraphics bgGraph_2nd;
        //------------------

        Pen WorkPen; //кисть для рисования

        SolidBrush WorkBrush; //кисть для заливки
        
        public List<Bitmap> History = new List<Bitmap>();// Список для запоминания истории
        public int CountHist;

        public int numHist = 0;//количество изображений в памяти
        public bool isWalkHistory = false;//метка совершаемого отката 

        public Cursor myNcur;
        public Cursor myBcur;
        public Cursor myPcur;
        public Cursor myPIPcur;

        public event ChangeBackForward EventChangeButton; //Событие для изменения активности кнопок Back и Forward
        public event ChangePaintParam EventChangeColorPipet; //Событие для изменения цвета в панели выбора цветов после работы пипетки
        //public event ChangePaintParam EventSetBackTextColor; //Событие для установки цвета в панели текста

        /// <summary>
        /// Конструктор
        /// </summary>
        public MyCanvas()
        {
            WorkPen = new Pen(ActFColor, ActWidthLine); //кисть для рисования
            WorkBrush = new SolidBrush(ActFColor); //кисть для заливки

            context = BufferedGraphicsManager.Current;
            bgGraph_1st = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)); //инициализирую, чтоб можно было достать из визуального Панели Элементов
            bgGraph_2nd = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)); //инициализирую, чтоб можно было достать из визуального Панели Элементов
            //this.Paint += new PaintEventHandler(MyCanvas_Paint);

            if (this.Width !=0 && this.Height != 0)
            {
                CountHist = (int)(200 / ((decimal)(this.Width * this.Height * 3)/(decimal)1024));
            }

            this.SizeChanged += new EventHandler(MyCanvas_SizeChanged);

            this.MouseDown +=new MouseEventHandler(MyCanvas_MouseDown);

            this.MouseUp +=new MouseEventHandler(MyCanvas_MouseUp);

            this.MouseMove +=new MouseEventHandler(MyCanvas_MouseMove);

            this.MouseClick += new MouseEventHandler(MyCanvas_MouseClick);

            this.Paint +=new PaintEventHandler(MyCanvas_Paint2);

            this.DoubleBuffered = true;
            
            myNcur = new Cursor(GetType(), "Cursor3.cur");
            myBcur = new Cursor(GetType(), "Cursor4.cur");
            myPcur = new Cursor(GetType(), "Cursor5.cur");
            myPIPcur = new Cursor(GetType(), "Cursor6.cur");

            //ЭУ для добавления текста
            TextInput TI = new TextInput();
            TI.Size = new Size(200, 200);            
            TI.Visible = false;
            this.Controls.Add(TI);
            //-------------------------------

            

            TI.EventDrawText += new DrawText(this.MyCanvas_DrawText_SetGraphics); //Делегат для события отрисовки текста
            //this.EventSetBackTextColor += new ChangePaintParam(TI.SetBackTextColor); //Делегат для события установки цвета текста
        }

       
        /// <summary>
        /// обработчик события изменения инструмента рисования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Параметры для обработчика выбора инструмента рисования</param>
        public void MyCanvas_ChangePressButt(object sender, ChangePressButtonEventArgs e)
        {
            this.pressBut = e.pressBut;
        }

        
        /// <summary>
        /// обработчик события изменения параметров рисования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Параметры для обработчика выбора параметров рисования</param>
        public void MyCanvas_ChangePaintParam(object sender, PaintParamEventArgs e)
        {
            if (e.NumParam == PaintParam.FColor)
            {
                this.ActFColor = e.FColor;
            }
            else if (e.NumParam == PaintParam.BColor)
            {
                this.ActBColor = e.BColor;
            }
            else if (e.NumParam == PaintParam.Texture)
            {
                this.ActTexture = e.Texture;
            }
            else if (e.NumParam == PaintParam.LineStyle)
            {
                this.ActLineStyle = e.LineStyle;
            }
            else if (e.NumParam == PaintParam.WidthLine)
            {
                this.ActWidthLine = e.WidthLine;
            }
            else if (e.NumParam == PaintParam.Solid)
            {
                this.Solid = e.Solid;
            }
        }

        /// <summary>
        /// Обработчик события вставки текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">параметры для обработчика вставки текста</param>
        public void MyCanvas_DrawText_SetGraphics(object sender, DrawTextEventArgs e)
        {
            Graphics gb2 = bgGraph_2nd.Graphics;
            bgGraph_1st.Render(gb2);

            gb2.DrawString(e.Text, e.Font, new SolidBrush(e.ForeColor), new Point (e.Location.X+6, e.Location.Y+3));
            ((TextInput)sender).Visible = false;
            //bgGraph_2nd.Render(bgGraph_1st.Graphics);

            //сохранение в историю
            Bitmap tmp_bmp = new Bitmap(this.Width, this.Height, gb2);
            Graphics gbmp = Graphics.FromImage(tmp_bmp);

            bgGraph_2nd.Render(bgGraph_1st.Graphics);
            bgGraph_2nd.Render(gbmp);

            if (History.Count >= CountHist)
            {
                History.RemoveAt(0);
            }
            History.Add(tmp_bmp);
            //----------------------

            this.Invalidate();
        }

        void MyCanvas_MouseClick(object sender, MouseEventArgs e)  
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics gb1 = bgGraph_1st.Graphics;
                Graphics gb2 = bgGraph_2nd.Graphics;
                
                // заливка
                #region
                if (pressBut == drawButton.floodfill)
                {
                    uint clr = Form1.GetPixel(gb2.GetHdc(), e.X, e.Y);
                    gb2.ReleaseHdc();

                    
                    uint HBRUSH = Form1.CreateSolidBrush((uint)((ActFColor.B << 16) + (ActFColor.G << 8) + ActFColor.R));
                    
                    Form1.SelectObject(gb2.GetHdc(), HBRUSH);
                    gb2.ReleaseHdc();

                    Form1.ExtFloodFill(gb2.GetHdc(), e.X, e.Y, clr, 1);   
                    gb2.ReleaseHdc();

                    Bitmap tmp_bmp = new Bitmap(this.Width, this.Height, gb2);
                    Graphics gbmp = Graphics.FromImage(tmp_bmp);

                    bgGraph_2nd.Render(gb1);
                    bgGraph_2nd.Render(gbmp);

                    
                    if (History.Count >= CountHist)
                    {
                        History.RemoveAt(0);
                    }
                    History.Add(tmp_bmp);

                    this.Invalidate();
                }
                #endregion
            } 
        }

        void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isPaint == true)
                {
                    //графикс буферов
                    Graphics gb1 = bgGraph_1st.Graphics;
                    Graphics gb2 = bgGraph_2nd.Graphics;
                   
                    HatchBrush WorkHB = new HatchBrush(ActTexture, ActFColor, ActBColor);

                    if (ActTexture == Solid) 
                    {
                        WorkHB = new HatchBrush(ActTexture, ActBColor, ActBColor);
                    }
                    
                    //изменение свойств кистей
                    WorkPen.Color = ActFColor;
                    WorkPen.Width = ActWidthLine;
                    WorkPen.DashStyle = ActLineStyle;
                    WorkPen.DashCap = DashCap.Flat;
                    WorkBrush.Color = ActFColor;
                    //--------------------------

                    //линия
                    if (pressBut == drawButton.line)
                    {
                        bgGraph_1st.Render(gb2);
                        gb2.DrawLine(WorkPen, StartP, new Point(e.X, e.Y));
                    }

                    //контур прямоугольника
                    if (pressBut == drawButton.rect)
                    {
                        bgGraph_1st.Render(gb2);
                        int width = Math.Abs(StartP.X - e.X);
                        int height = Math.Abs(StartP.Y - e.Y);

                        int x = (StartP.X > e.X) ? e.X : StartP.X;
                        int y = (StartP.Y > e.Y) ? e.Y : StartP.Y;

                        gb2.DrawRectangle(WorkPen, new Rectangle(x, y, width, height));
                    }
                    
                    //контур эллипса
                    if (pressBut == drawButton.ell)
                    {
                        bgGraph_1st.Render(gb2);
                        gb2.DrawEllipse(WorkPen, new Rectangle(StartP.X, StartP.Y, (e.X-StartP.X), (e.Y-StartP.Y)));
                    }

                    //залитый прямоуг
                    if (pressBut == drawButton.fillRect)
                    {
                        bgGraph_1st.Render(gb2);
                        int width = Math.Abs(StartP.X - e.X);
                        int height = Math.Abs(StartP.Y - e.Y);

                        int x = (StartP.X > e.X) ?  e.X : StartP.X;
                        int y = (StartP.Y > e.Y) ? e.Y : StartP.Y;

                       
                        gb2.FillRectangle(WorkHB, new Rectangle(x, y, width, height));
                        gb2.DrawRectangle(WorkPen, new Rectangle(x, y, width, height));
                    }

                    //залитый эллипс
                    if (pressBut == drawButton.fillEll)
                    {
                        bgGraph_1st.Render(gb2);
                        gb2.FillEllipse(WorkHB, new Rectangle(StartP.X, StartP.Y, (e.X-StartP.X), (e.Y-StartP.Y)));
                        gb2.DrawEllipse(WorkPen, new Rectangle(StartP.X, StartP.Y, (e.X-StartP.X), (e.Y-StartP.Y)));
                    }

                    //кисть
                    #region
                    if (/*Form1.*/pressBut == drawButton.brush)
                    {
                        WorkPen.StartCap = LineCap.Round;
                        WorkPen.EndCap = LineCap.Round;
                        WorkPen.DashCap = DashCap.Round;

                        if (ActLineStyle == DashStyle.Solid)
                        {
                            //вариант 1
                            WorkPen.DashStyle = DashStyle.Solid;

                            gb2.DrawLine(WorkPen, StartP, e.Location);

                            //if (Math.Sqrt(Math.Pow(Math.Abs(StartP.X-e.X), 2)+Math.Pow(Math.Abs(StartP.Y-e.Y), 2))<10)
                            //{
                            //    gb2.DrawLine(WorkPen, StartP, e.Location);
                            //}
                            //else
                            //{
                            //    gb2.DrawCurve(WorkPen, new Point[] { StartPred, StartP, e.Location });
                            //}
                            
                            StartPred = StartP;
                            //----------------------------
                        }
                        else
                        {
                            //вариант 2
                            PathBrush.AddLine(StartP, e.Location);
                            GlobalPathBrush.AddLine(StartP, e.Location);

                            gb2.DrawPath(WorkPen, PathBrush);

                            //оптимизация варианта 2
                            count++;
                            if (count == 100)
                            {
                                PathBrush.Reset();
                                count=0;
                            }
                            //--------------------
                        }
                        
                        StartP = e.Location;
                    }
                    #endregion //кисть

                    //карандаш
                    #region
                    if (pressBut == drawButton.pen)
                    {
                        WorkPen.Width = 2;

                        WorkPen.StartCap = LineCap.Round;
                        WorkPen.EndCap = LineCap.Round;
                        WorkPen.DashCap = DashCap.Round;

                        //gb2.DrawLine(WorkPen, StartP, e.Location});//вариант 1  

                        PathBrush.AddLine(StartP, e.Location);//вариант 2                    
                        gb2.DrawPath(WorkPen, PathBrush);//вариант 2

                        //оптимизация варианта 2
                        count++;
                        if (count == 100)
                        {
                            PathBrush.Reset();
                            count=0;
                        }
                        //--------------------

                        StartP = e.Location;
                        //bgGraph_2nd.Render(gb1);
                    }
                    #endregion

                    //ластик
                    if (pressBut == drawButton.eraser)
                    {
                        WorkPen.Width = 10;
                        WorkBrush = new SolidBrush(Color.White);
                        gb2.FillRectangle(WorkBrush, new Rectangle(StartP.X, StartP.Y, 32, 32));

                        StartP = e.Location;
                    }

                    //контуры стрелок
                    #region
                    else if (pressBut == drawButton.arrowR || pressBut == drawButton.arrowL)
                    {
                        bgGraph_1st.Render(gb2);

                        int x = (StartP.X > e.X) ? e.X : StartP.X;
                        int x2 = (StartP.X > e.X) ? StartP.X : e.X;
                        int y = (StartP.Y > e.Y) ? e.Y : StartP.Y;
                        int y2 = (StartP.Y > e.Y) ? StartP.Y : e.Y;

                        FigureArrow Arrow = new FigureArrow(gb2, new Point(x, y), new Point(x2, y2));

                        if (pressBut == drawButton.arrowR)
                        {
                            Arrow.DrawArrowR(WorkPen);
                        }
                        else if (pressBut == drawButton.arrowL)
                        {
                            Arrow.DrawArrowL(WorkPen);
                        }
                    }
                    #endregion

                    //закрашенные стрелки
                    #region
                    else if (pressBut == drawButton.fillArrowR || pressBut == drawButton.fillArrowL)
                    {
                        bgGraph_1st.Render(gb2);

                        int x = (StartP.X > e.X) ? e.X : StartP.X;
                        int x2 = (StartP.X > e.X) ? StartP.X : e.X;
                        int y = (StartP.Y > e.Y) ? e.Y : StartP.Y;
                        int y2 = (StartP.Y > e.Y) ? StartP.Y : e.Y;

                        FigureArrow Arrow = new FigureArrow(gb2, new Point(x, y), new Point(x2, y2));

                        if (pressBut == drawButton.fillArrowR)
                        {
                            Arrow.FillArrowR(WorkPen, WorkHB);
                        }
                        else if (pressBut == drawButton.fillArrowL)
                        {
                            Arrow.FillArrowL(WorkPen, WorkHB);
                        }
                    }
                    #endregion

                    //контур Бэтмена
                    #region
                    else if (pressBut == drawButton.batman)
                    {
                        bgGraph_1st.Render(gb2);

                        int x = (StartP.X > e.X) ? e.X : StartP.X;
                        int x2 = (StartP.X > e.X) ? StartP.X : e.X;
                        int y = (StartP.Y > e.Y) ? e.Y : StartP.Y;
                        int y2 = (StartP.Y > e.Y) ? StartP.Y : e.Y;

                        FigureBatman Bat = new FigureBatman(gb2, new Point(x, y), new Point(x2, y2));
                        Bat.DrawBatman(WorkPen);
                    }
                    #endregion

                    //закрашенный Бэтмен
                    #region
                    else if (pressBut == drawButton.fillBatman)
                    {
                        bgGraph_1st.Render(gb2);

                        int x = (StartP.X > e.X) ? e.X : StartP.X;
                        int x2 = (StartP.X > e.X) ? StartP.X : e.X;
                        int y = (StartP.Y > e.Y) ? e.Y : StartP.Y;
                        int y2 = (StartP.Y > e.Y) ? StartP.Y : e.Y;

                        FigureBatman Bat = new FigureBatman(gb2, new Point(x, y), new Point(x2, y2));

                        Bat.FillBatman(WorkPen, WorkHB);

                    }
                    #endregion

                    //вставка текста
                    #region
                    else if (pressBut == drawButton.text)
                    {
                        bgGraph_1st.Render(gb2);

                        Pen TextPen = new Pen(Color.DarkGray);
                        TextPen.DashStyle = DashStyle.Dash;
                        TextPen.Width = 1;

                        int TextX = e.X;
                        int TextY = e.Y;

                        if (e.X > this.Width)
                        {
                            TextX = this.Width-5;
                        }
                        if (e.X < 0)
                        {
                            TextX = 5;
                        }
                        if (e.Y > this.Height)
                        {
                            TextY = this.Height-5;
                        }
                        if (e.Y < 0)
                        {
                            TextY = 5;
                        }

                        int width = Math.Abs(StartP.X - TextX);
                        int height = Math.Abs(StartP.Y - TextY);
                                                
                        int x = (StartP.X > TextX) ? TextX : StartP.X;
                        int y = (StartP.Y > TextY) ? TextY : StartP.Y;

                        gb2.DrawRectangle(TextPen, x, y, width, height);

                    }
                    #endregion

                    this.Invalidate();
                }

                
            }

            //отрисовка линзы
            else if (e.Button == MouseButtons.Right)
            {
                if (isLens == true && pressBut != drawButton.pipete)
                {
                    MakeLens(e);
                }
            }

            //Отрисовка пипетки
            if (/*Form1.*/pressBut == drawButton.pipete)
            {
                bgGraph_1st.Render(bgGraph_2nd.Graphics);
                MakePipet(e.Location);
            }
        }
         
        void MyCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPaint = true;//начало рисования

                EventChangeButton(true, false); //Событие Back - активна

                //если были хождения по истории - стереть лишний список
                if (isWalkHistory == true)
                {
                    History.RemoveRange(History.Count-numHist, numHist);
                    numHist = 0;
                }
                //-------------------

                Graphics gb1 = bgGraph_1st.Graphics;
                Graphics gb2 = bgGraph_2nd.Graphics;

                bgGraph_1st.Render(gb2);
                
                Form1.i_plus = 0;

                StartP = new Point(e.X, e.Y);
                StartPred = StartP;

                if (pressBut == drawButton.brush || pressBut == drawButton.pen || pressBut == drawButton.eraser)
                {
                    //отрисовка кисти
                    if (pressBut == drawButton.brush)
                    {
                        gb2.FillEllipse(WorkBrush, new Rectangle(StartP.X-ActWidthLine/2, StartP.Y-ActWidthLine/2, ActWidthLine, ActWidthLine));

                        bgGraph_2nd.Render(gb1);
                    }

                    //отрисовка карандаша
                    else if (/*Form1.*/pressBut == drawButton.pen)
                    {
                        //gb2.FillEllipse(WorkBrush, new Rectangle(StartP.X, StartP.Y, 1, 1));      
                        gb2.FillRectangle(WorkBrush, new Rectangle(StartP.X, StartP.Y, 1, 1));
                        bgGraph_2nd.Render(gb1);
                    }

                    //отрисовкка ластика
                    else if (/*Form1.*/pressBut == drawButton.eraser)
                    {
                        WorkPen.Width = 10;
                        WorkBrush = new SolidBrush(Color.White);
                        gb2.FillRectangle(WorkBrush, new Rectangle(StartP.X/*-25/2*/, StartP.Y/*-25/2*/, 32, 32)); bgGraph_2nd.Render(gb1);                        
                    }
                    
                    Invalidate();
                }

                //показ пипетки
                if (pressBut == drawButton.pipete)
                {
                    MakePipet(e.Location);
                }
            }

            //показ ЛИНЗЫ
            else if (e.Button == MouseButtons.Right)
            {
                if (pressBut != drawButton.pipete)
                {
                    isLens = true;
                    MakeLens(e);
                }                
            }
            
        }
        
        void MyCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //при заливке никаких действий
                if (pressBut == drawButton.floodfill)
                {
                    return;
                }

                isPaint = false;//конец рисования                

                Graphics gb1 = bgGraph_1st.Graphics;//
                Graphics gb2 = bgGraph_2nd.Graphics;

                //получение основного цвета пипеткой
                #region
                if (/*Form1.*/pressBut == drawButton.pipete)
                {
                    uint clr = Form1.GetPixel(gb2.GetHdc(), e.X, e.Y);
                    //g.Dispose();
                    gb2.ReleaseHdc();

                    //MessageBox.Show(String.Format("Красный {0:X}", clr&255));
                    //MessageBox.Show(String.Format("Зеленый {0}", clr>>8&255));
                    //MessageBox.Show(String.Format("Синий {0:X}", clr>>16&255));

                    MakePipet(e.Location);

                    //this.Invalidate(); 
                    
                    /*Form1.Set*/ActFColor = (Color.FromArgb((int)clr & 255, (int)clr >> 8 & 255, (int)clr >> 16 & 255));
                    this.EventChangeColorPipet(this, new PaintParamEventArgs(PaintParam.FColor, this.ActFColor, Color.White));

                    //bgGraph_1st.Render(gb2);
                    return;
                }
                #endregion

                //оптимизированный вывод прерывистых линий на экран после окончания рисования
                if (/*Form1.*/pressBut == drawButton.brush && ActLineStyle != DashStyle.Solid)
                {
                    bgGraph_1st.Render(gb2);
                    gb2.DrawPath(WorkPen, GlobalPathBrush);
                    this.Invalidate();
                }

                //Отрисовка елемента для ввода текста
                #region 
                if (pressBut == drawButton.text)
                {
                    bgGraph_1st.Render(gb2);
                    this.Invalidate();

                    int TextX = e.X;
                    int TextY = e.Y;

                    if (e.X > this.Width)
                    {
                        TextX = this.Width-5;
                    }
                    if (e.X < 0)
                    {
                        TextX = 5;
                    }
                    if (e.Y > this.Height)
                    {
                        TextY = this.Height-5;
                    }
                    if (e.Y < 0)
                    {
                        TextY = 5;
                    }

                    int width = Math.Abs(StartP.X - TextX);
                    int height = Math.Abs(StartP.Y - TextY);
                    
                    
                    
                    int x = (StartP.X > TextX) ? TextX : StartP.X;
                    int y = (StartP.Y > TextY) ? TextY : StartP.Y;

                    if (width < 150)
                    {
                        width = 150;

                        if ((x + width) > this.Width)
                        {
                            x = TextX - 150;
                        }
                    }
                    if (height < 50)
                    {
                        height = 50;

                        //if ((y + height + 30) > this.Height)
                        //{
                        //    y = TextY - (50+30);

                        //    //((TextInput)this.Controls[0]).changLocationButton();
                        //}
                    }
                    if ((y + height + 30) > this.Height)
                    {
                        y = TextY - (height+30);

                        //((TextInput)this.Controls[0]).changLocationButton();
                    }

                    this.Controls[0].Size = new Size(width, height+30);
                    //((TextBox)this.Controls[0]).BorderStyle = BorderStyle.FixedSingle;

                    this.Controls[0].Location = new Point(x, y);

                    //this.Controls[0].Controls[0].Visible = false;
                    //this.Controls[0].Controls[0].Focus();

                    this.Controls[0].Visible = true;

                    //uint clr = Form1.GetPixel(gb2.GetHdc(), e.X, e.Y);
                    //gb2.ReleaseHdc();
                    //Color Color = (Color.FromArgb((int)clr & 255, (int)clr >> 8 & 255, (int)clr >> 16 & 255));

                    //Graphics gti = this.Controls[0].Controls[0].CreateGraphics();

                    //Form1.StretchBlt(gti.GetHdc(), 0, 0, this.Controls[0].Controls[0].Width, this.Controls[0].Controls[0].Height, gb2.GetHdc(), x, y, width, height, 0xcc0020);

                    //gb2.ReleaseHdc();
                    //gti.ReleaseHdc();

                    //EventSetBackTextColor(this, new PaintParamEventArgs(0, Color.Transparent, Color.Transparent));
                }
                #endregion

                //возвращение параметров кисти к заданным
                WorkPen.StartCap = LineCap.Square;
                WorkPen.EndCap = LineCap.Square;
                WorkPen.DashCap = DashCap.Flat;
                //--------------------------------

                PathBrush.Reset();//сброс пути кисти или карандаша
                GlobalPathBrush.Reset();//сброс глобального пути кисти или карандаша

                if (pressBut != drawButton.text)
                {
                    //сохранение в историю
                    Bitmap tmp_bmp = new Bitmap(this.Width, this.Height, gb2);
                    Graphics gbmp = Graphics.FromImage(tmp_bmp);

                    bgGraph_2nd.Render(gb1);
                    bgGraph_2nd.Render(gbmp);

                    if (History.Count >= CountHist)
                    {
                        History.RemoveAt(0);
                    }
                    History.Add(tmp_bmp);
                    //----------------------
                }
                
            }
            else if (e.Button == MouseButtons.Right)
            {
                //получение фонового цвета пипеткой
                #region
                if (pressBut == drawButton.pipete)
                {
                    Graphics gb1 = bgGraph_1st.Graphics;//
                    Graphics gb2 = bgGraph_2nd.Graphics;

                    uint clr = Form1.GetPixel(gb2.GetHdc(), e.X, e.Y);
                    
                    gb2.ReleaseHdc();

                    //MessageBox.Show(String.Format("Красный {0:X}", clr&255));
                    //MessageBox.Show(String.Format("Зеленый {0}", clr>>8&255));
                    //MessageBox.Show(String.Format("Синий {0:X}", clr>>16&255));

                    MakePipet(e.Location);

                    this.Invalidate();

                    ActBColor = (Color.FromArgb((int)clr & 255, (int)clr >> 8 & 255, (int)clr >> 16 & 255));
                    this.EventChangeColorPipet(this, new PaintParamEventArgs(PaintParam.BColor, Color.Black, this.ActBColor));
                    
                    return;
                }
                #endregion

                isLens = false;

                //избавление от прикрепленного региона в функции создания линзы средствами WinAPI
                bgGraph_2nd.Dispose();
                bgGraph_2nd = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
                //---------------------------------------------------------

                bgGraph_1st.Render(bgGraph_2nd.Graphics);
                this.Invalidate();
            }
        }

        /// <summary>
        /// Отрисовывает экранную лупу
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        void MakeLens(MouseEventArgs e)
        {
            Graphics gb2 = bgGraph_2nd.Graphics;
            //Graphics gb1 = bgGraph_1st.Graphics;
            //Graphics g = this.CreateGraphics();

            GraphicsPath p = new GraphicsPath(/*FillMode.Alternate*/);

            p.AddEllipse(new Rectangle(e.X - 50, e.Y - 50, 100, 100));

            Region R = new Region(p);

            //gb2.Clip = R;

            bgGraph_1st.Render(gb2);

            //-----------------------
            IntPtr gr2 = bgGraph_2nd.Graphics.GetHdc();

            bgGraph_2nd.Graphics.ReleaseHdc();

            IntPtr HR = R.GetHrgn(bgGraph_2nd.Graphics);

            uint oldReg = Form1.SelectObject(gr2, (uint)HR);

            //gr2 = bgGraph_2nd.Graphics.GetHdc();
            //-----------------------

            Form1.StretchBlt(gb2.GetHdc(), e.X - 50, e.Y - 50, 100, 100, bgGraph_1st.Graphics.GetHdc(), e.X - 20, e.Y - 20, 40, 40, 0xcc0020);
            gb2.ReleaseHdc();
            bgGraph_1st.Graphics.ReleaseHdc();

            //gb2.DrawRectangle(Pens.Black, new Rectangle(e.X - 50, e.Y - 50, 100, 100));

            //this.Invalidate();            

            Form1.SelectObject(gr2, oldReg);

            R.ReleaseHrgn(HR);

            bgGraph_2nd.Graphics.DrawEllipse(Pens.Black, new Rectangle(e.X - 49, e.Y - 49, 98, 98));

            ////--------------------
            //R.Dispose();
            //R = new Region(new Rectangle(0, 0, this.Width, this.Height));
            //HR = R.GetHrgn(bgGraph_2nd.Graphics);
            //oldReg = Form1.SelectObject(gr2, (uint)HR);
            ////----------------------           


            Form1.DeleteObject((uint)HR);
            Form1.DeleteObject(oldReg);

            this.Invalidate();

            
        }

        /// <summary>
        /// Отрисовывает пипетку
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        void MakePipet(Point P)
        {
            int dx = 75;
            int dy = 75;
            int dxx = 45;
            int dyy = 45;

            Graphics gb2 = bgGraph_2nd.Graphics;
            //Graphics gb1 = bgGraph_1st.Graphics;
            //Graphics g = this.CreateGraphics();

            GraphicsPath p = new GraphicsPath(FillMode.Alternate);

            p.AddEllipse(new Rectangle(P.X - 50, P.Y - 50, 100, 100));

            Region R = new Region(p);

            
            //bgGraph_1st.Render(gb2);

            //Graphics g = Graphics.FromHwnd(this.Handle);

            if ((P.X - dx) < 0)
            {
                dx = -5;
                dxx = -35;
            }
            if ((P.Y - dy) < 0)
            {
                dy = -5;
                dyy = -35;
            }

            Form1.StretchBlt(gb2.GetHdc(), P.X - dx, P.Y - dy, 70, 70, bgGraph_1st.Graphics.GetHdc(), P.X - 3, P.Y - 3, 7, 7, 0xcc0020);
            
            gb2.ReleaseHdc();
            bgGraph_1st.Graphics.ReleaseHdc();

            
            gb2.DrawRectangle(Pens.Black, new Rectangle(P.X - dx, P.Y - dy, 70, 70));
            gb2.DrawRectangle(Pens.Black, new Rectangle(P.X - dxx, P.Y - dyy, 10, 10)); 
            
            this.Invalidate();                       
        }
                        
        void MyCanvas_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void MyCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
           bgGraph_2nd.Render(g);
        }

        void MyCanvas_Paint2(object sender, PaintEventArgs e)
        {
            Graphics gb1 = bgGraph_1st.Graphics;
            Graphics gb2 = bgGraph_2nd.Graphics;
            Graphics g = e.Graphics;

            //gb1.Clear(Color.White);
            gb2.Clear(Color.White);
            //g.Clear(Color.White);

            //Добавление первоначально чистого листа в историю
            Bitmap tmp_bmp = new Bitmap(this.Width, this.Height, gb2);
            Graphics gbmp = Graphics.FromImage(tmp_bmp);

            bgGraph_2nd.Render(gb1);
            bgGraph_2nd.Render(g);
            bgGraph_2nd.Render(gbmp);

            if (History.Count >= CountHist)
            {
                History.RemoveAt(0);
            }
            History.Add(tmp_bmp); 
            //---------------------
            
            this.Paint -=new PaintEventHandler(MyCanvas_Paint2);
            this.Paint +=new PaintEventHandler(MyCanvas_Paint);
        }
    }

    //public partial class TextBoxTransparent : TextBox
    //{
    //    public TextBoxTransparent()
    //    {

    //        SetStyle(ControlStyles.SupportsTransparentBackColor  | 
    //             ControlStyles.OptimizedDoubleBuffer  | 
    //             ControlStyles.AllPaintingInWmPaint  | 
    //             ControlStyles.ResizeRedraw  | 
    //             ControlStyles.UserPaint, true);
    //        BackColor  =  Color.Transparent;
    //    }


    //}

    /// <summary>
    /// класс для передачи параметров в события изменения Параметров рисования
    /// </summary>
    public class PaintParamEventArgs: EventArgs
    {
        public Color FColor;//выбранный основной цвет
        public Color BColor;//выбранный фоновый цвет
        public HatchStyle Texture;//вырбранная штриховка
        public DashStyle LineStyle;//выбранный тип линии
        public int WidthLine;//выбранная толщина линии
        public HatchStyle Solid;

        public PaintParam NumParam;

        public PaintParamEventArgs(PaintParam NumParam, Color FC, Color BC)
        {
            this.FColor = FC;
            this.BColor = BC;
            this.NumParam = NumParam;
        }

        public PaintParamEventArgs(PaintParam NumParam, HatchStyle Tex, HatchStyle Solid)
        {
            this.Texture = Tex;
            this.Solid = Solid;
            this.NumParam = NumParam;
        }

        public PaintParamEventArgs(PaintParam NumParam, DashStyle LS)
        {
            this.LineStyle = LS;
            this.NumParam = NumParam;
        }

        public PaintParamEventArgs(PaintParam NumParam, int WL)
        {
            this.WidthLine = WL;
            this.NumParam = NumParam;
        }
    }

    /// <summary>
    /// класс для передачи параметров в событие изменения нажатой кнопки
    /// </summary>
    public class ChangePressButtonEventArgs : EventArgs
    {
        public drawButton pressBut;//нажатая кнопка

        public ChangePressButtonEventArgs(drawButton pressBut)
        {
            this.pressBut = pressBut;
        }
    }

    /// <summary>
    /// класс для передачи параметров графикса в форму Текста
    /// </summary>
    public class DrawTextEventArgs : EventArgs
    {
        public String Text;
        public Font Font;
        public Color ForeColor;
        public Point Location;

        public DrawTextEventArgs(String Text, Font Font, Color ForeColor, Point Location)
        {
            this.Text = Text;
            this.Font = Font;
            this.ForeColor = ForeColor;
            this.Location = Location;
        }
    }    

    /// <summary>
    /// Фигура Стрелка
    /// </summary>
    class FigureArrow
    {
        Point A, B, C, D, E, F, G;
        Graphics g;
        GraphicsPath GPArrow = new GraphicsPath();

        public FigureArrow(Graphics g, Point Beg, Point End)
        {
            A = new Point(Beg.X, (End.Y-Beg.Y)/4+Beg.Y);
            B = new Point((End.X-Beg.X)/2+Beg.X, (End.Y-Beg.Y)/4+Beg.Y);
            C = new Point((End.X-Beg.X)/2+Beg.X, Beg.Y);
            D = new Point(End.X, (End.Y-Beg.Y)/2+Beg.Y);
            E = new Point((End.X-Beg.X)/2+Beg.X, End.Y);
            F = new Point((End.X-Beg.X)/2+Beg.X, 3*(End.Y-Beg.Y)/4+Beg.Y);
            G = new Point(Beg.X, 3*(End.Y-Beg.Y)/4+Beg.Y);

            this.g = g;
            
            GPArrow.StartFigure();

            GPArrow.AddLine(A, B);
            GPArrow.AddLine(B, C);
            GPArrow.AddLine(C, D);
            GPArrow.AddLine(D, E);
            GPArrow.AddLine(E, F);
            GPArrow.AddLine(F, G); 
            GPArrow.AddLine(G, A);

            GPArrow.CloseFigure();
        }

        public void DrawArrowR(Pen WorkPen)
        {
            g.DrawPath(WorkPen, GPArrow);
        }
        public void DrawArrowL(Pen WorkPen)
        {
            Matrix X = new Matrix();
            X.RotateAt(180, new Point((D.X - A.X)/2+A.X, (E.Y - C.Y)/2+C.Y));
            GPArrow.Transform(X);
            g.DrawPath(WorkPen, GPArrow);
        }

        public void FillArrowR(Pen WorkPen, HatchBrush WorkHB)
        {
            g.FillPath(WorkHB, GPArrow);
            g.DrawPath(WorkPen, GPArrow);
        }

        public void FillArrowL(Pen WorkPen, HatchBrush WorkHB)
        {
            Matrix X = new Matrix();
            X.RotateAt(180, new Point((D.X - A.X)/2+A.X, (E.Y - C.Y)/2+C.Y));
            GPArrow.Transform(X);

            g.FillPath(WorkHB, GPArrow);
            g.DrawPath(WorkPen, GPArrow);
        }
    }

    /// <summary>
    /// Фигура Значек Бэтмена
    /// </summary>
    class FigureBatman
    {
        Point P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19;
        Graphics g;
        GraphicsPath GPBatman = new GraphicsPath();

        public FigureBatman(Graphics G, Point Beg, Point End)
        {
            int w = Math.Abs(End.X-Beg.X);
            int h = Math.Abs(End.Y-Beg.Y);

            P1 = Beg;
            P2 = new Point(Beg.X+6*w/23, Beg.Y);
            P3 = new Point(Beg.X+7*w/23, Beg.Y+h/7);
            P4 = new Point(Beg.X+10*w/23, Beg.Y+2*h/7);
            P5 = new Point(Beg.X+11*w/23, Beg.Y);
            P6 = new Point(Beg.X+11*w/23, Beg.Y+h/7);
            P7 = new Point(Beg.X+12*w/23, Beg.Y+h/7);
            P8 = new Point(Beg.X+12*w/23, Beg.Y);
            P9 = new Point(Beg.X+13*w/23, Beg.Y+2*h/7);
            P10 = new Point(Beg.X+16*w/23, Beg.Y+h/7);
            P11 = new Point(Beg.X+17*w/23, Beg.Y);
            P12 = new Point(End.X, Beg.Y);
            P13 = new Point(Beg.X+21*w/23, Beg.Y+h/7);
            P14 = new Point(Beg.X+20*w/23, Beg.Y+3*h/7);
            P15 = new Point(Beg.X+15*w/23, Beg.Y+4*h/7);
            P16 = new Point((Beg.X+115*w/230), End.Y);
            P17 = new Point((Beg.X+8*w/23), Beg.Y+4*h/7);
            P18 = new Point((Beg.X+3*w/23), Beg.Y+3*h/7);
            P19 = new Point((Beg.X+2*w/23), Beg.Y+h/7);

            this.g = G;
            
            GPBatman.StartFigure();

            GPBatman.AddLine(P1, P2);
            GPBatman.AddCurve(new Point[] { P2, P3, P4 });
            GPBatman.AddLine(P4, P5);
            GPBatman.AddLine(P5, P6);
            GPBatman.AddLine(P6, P7);
            GPBatman.AddLine(P7, P8);
            GPBatman.AddLine(P8, P9);
            GPBatman.AddCurve(new Point[] { P9, P10, P11 });
            GPBatman.AddLine(P11, P12);
            GPBatman.AddCurve(new Point[] { P12, P13, P14 });
            GPBatman.AddCurve(new Point[] { P14, P15, P16 });
            GPBatman.AddCurve(new Point[] { P16, P17, P18 });
            GPBatman.AddCurve(new Point[] { P18, P19, P1 });

            GPBatman.CloseFigure();
        }

        public void DrawBatman(Pen WorkPen)
        {
            g.DrawPath(WorkPen, GPBatman);
        }

        public void FillBatman(Pen WorkPen, HatchBrush WorkHB)
        {
            g.FillPath(WorkHB, GPBatman);
            g.DrawPath(WorkPen, GPBatman);
        }
    }

    public enum PaintParam { FColor, BColor, Texture, LineStyle, WidthLine, Solid };
    
    public enum drawButton { line, rect, fillRect, ell, fillEll, brush, pen, floodfill ,eraser, pipete, arrowR, 
        arrowL, fillArrowR, fillArrowL, text, batman, fillBatman };
    
    
}
