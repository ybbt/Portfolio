using System;

using System.Drawing;

using System.Windows.Forms;

namespace _4inLine
{
    interface IGameView
    {
        void DisplayWin(String name);
        void DisplayDeadend();
        void DisplayActivPlayer(String name, Color color);
        void DrawCoin(LogicCell logicCell, Color coinColor);
    }

    public partial class FormMain : Form, IGameView
    {
        //for bufferiing
        protected BufferedGraphicsContext context;

        public BufferedGraphics bgGraph_1st;

        public BufferedGraphics bgGraph_2nd;
        //------------------

        /// <summary>
        /// reference to process of the game
        /// </summary>
        private GamePresenter gamePresent;

        private int wNumCoin;
        private int hNumCoin;

        public FormMain()
        {
            InitializeComponent();

            //enabling double buffering
            this.DoubleBuffered = true;

            //creating controls
            Button btnStartGame = new Button()
            {
                Name = "btnStartGame",
                Text = "Start Game",
                ForeColor = Color.MediumBlue,
                BackColor = SystemColors.ControlDark,
                Dock = DockStyle.Fill
            };
            btnStartGame.MouseClick += btnStartGame_MouseClick;
            TLP.Controls.Add(btnStartGame, 2, 0);

            TextBox tbPlayer1Name = new TextBox()
            {
                Name = "tbPlayer1",
                Text = "Player 1",
                Dock = DockStyle.Fill
                 
            };
            TLP.Controls.Add(tbPlayer1Name, 0, 0);
            TLP.SetColumnSpan(tbPlayer1Name, 2);

            TextBox tbPlayer2Name = new TextBox()
            {
                Name = "tbPlayer2",
                Text = "Player 2",
                Dock = DockStyle.Fill
            };
            TLP.Controls.Add(tbPlayer2Name, 3, 0);
            TLP.SetColumnSpan(tbPlayer2Name, 2);

            PictureBox pbPlr1Color = new PictureBox()
            {
                BackColor = Color.Green,
                Name = "pbPlr1Color",
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill
            };
            TLP.Controls.Add(pbPlr1Color, 0, 1);

            Button btnPlr1ColorChng = new Button()
            {
                Name = "btnPlr1ColorChng",
                Text = "Change color",
                BackColor = SystemColors.Control,
                Dock = DockStyle.Fill
            };
            btnPlr1ColorChng.MouseClick += btnPlr1ColorChng_MouseClick;
            TLP.Controls.Add(btnPlr1ColorChng, 1, 1);

            PictureBox pbPlr2Color = new PictureBox()
            {
                BackColor = Color.Red,
                Name = "pbPlr2Color",
                BorderStyle = BorderStyle.FixedSingle,
                 Dock = DockStyle.Fill
            };
            TLP.Controls.Add(pbPlr2Color, 4, 1);

            Button btnPlr2ColorChng = new Button()
            {
                Name = "btnPlr2ColorChng",
                Text = "Change color",
                BackColor = SystemColors.Control,
                Dock = DockStyle.Fill
            };
            btnPlr2ColorChng.MouseClick += btnPlr1ColorChng_MouseClick; 
            TLP.Controls.Add(btnPlr2ColorChng, 3, 1);

            Label lblActPlayer = new Label()
            {
                Name = "lblActPlayer",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Blue,
                Margin = new Padding(3),
                BorderStyle = BorderStyle.FixedSingle
            };
            TLP.Controls.Add(lblActPlayer, 2, 1);

            
        }

        private void btnPlr1ColorChng_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog
            {
                AllowFullOpen = false,
                ShowHelp = true
            };

            if (sender as Button == TLP.Controls["btnPlr1ColorChng"])
            {
                MyDialog.Color = TLP.Controls["pbPlr1Color"].BackColor;
            }
            else if (sender as Button == TLP.Controls["btnPlr2ColorChng"])
            {
                MyDialog.Color = TLP.Controls["pbPlr2Color"].BackColor;
            }
            
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                if (MyDialog.Color == Color.Black)
                {
                    MyDialog.Color = Color.White;
                }
                if (sender as Button == TLP.Controls["btnPlr1ColorChng"])
                {
                    if (MyDialog.Color == TLP.Controls["pbPlr2Color"].BackColor)
                    {
                        MyDialog.Color = Color.FromArgb(MyDialog.Color.A, 255 - MyDialog.Color.R, 255 - MyDialog.Color.G,
                            255 - MyDialog.Color.B);
                    }
                    TLP.Controls["pbPlr1Color"].BackColor = MyDialog.Color;
                }
                else if (sender as Button == TLP.Controls["btnPlr2ColorChng"])
                {
                    if (MyDialog.Color == TLP.Controls["pbPlr1Color"].BackColor)
                    {
                        MyDialog.Color = Color.FromArgb(MyDialog.Color.A, 255 - MyDialog.Color.R, 255 - MyDialog.Color.G,
                            255 - MyDialog.Color.B);
                    }
                    TLP.Controls["pbPlr2Color"].BackColor = MyDialog.Color;
                }
            }
        }

        private void btnStartGame_MouseClick(object sender, MouseEventArgs e)
        {
            if (gamePresent == null || gamePresent.GameModel == null || gamePresent.GameModel.GameStatus == false)
            {
                ((TextBox)(TLP.Controls["tbPlayer1"])).ReadOnly = true;
                ((TextBox)(TLP.Controls["tbPlayer2"])).ReadOnly = true;
                TLP.Controls["btnPlr1ColorChng"].Enabled = false;
                TLP.Controls["btnPlr2ColorChng"].Enabled = false;
                TLP.Controls["btnStartGame"].Text = "New Game";

                if (gamePresent.GameModel != null && gamePresent.GameModel.GameStatus == false)
                {
                    gamePresent.GameModel.GameStatus = true;
                }

                gamePresent = new GamePresenter(this, new Game(), TLP.Controls["tbPlayer1"].Text, TLP.Controls["pbPlr1Color"].BackColor, TLP.Controls["tbPlayer2"].Text, TLP.Controls["pbPlr2Color"].BackColor);
                    
            }
            else
            {
                gamePresent.EndGame();
                DrawCoinSpace();
                ((TextBox)(TLP.Controls["tbPlayer1"])).ReadOnly = false;
                ((TextBox)(TLP.Controls["tbPlayer2"])).ReadOnly = false;
                TLP.Controls["btnPlr1ColorChng"].Enabled = true;
                TLP.Controls["btnPlr2ColorChng"].Enabled = true;
                TLP.Controls["btnStartGame"].Text = "Start Game";
            }
            
        }

        /// <summary>
        /// display information about a victory
        /// </summary>
        /// <param name="name">name of the winning player</param>
        public void DisplayWin(String name)
        {
            MessageBox.Show("Player \"" + name + "\" won!");
        }

        /// <summary>
        /// display information about the draw
        /// </summary>
        public void DisplayDeadend()
        {
            MessageBox.Show("The game ended in a draw");
        }

        /// <summary>
        /// display information about the active player
        /// </summary>
        /// <param name="name">name of the active player</param>
        /// <param name="color">color of the active player</param>
        public void DisplayActivPlayer(String name, Color color)
        {
            TLP.Controls["lblActPlayer"].Text = name;
            TLP.Controls["lblActPlayer"].BackColor = color;
            if (color.R == 128 && color.G == 128 && color.B == 128)
            {
                TLP.Controls["lblActPlayer"].ForeColor = Color.Black;
            }
            else TLP.Controls["lblActPlayer"].ForeColor = Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);

        }

        /// <summary>
        /// method of drawing the coins in logic cell
        /// </summary>
        /// <param name="logicCell">logic cell</param>
        /// <param name="coinColor">coin color</param>
        public void DrawCoin(LogicCell logicCell, Color coinColor)
        {
            DrawEllipse(logicCell.LogicI, logicCell.LogicJ, coinColor);
        }


        /// <summary>
        /// method of drawing space for the coin
        /// </summary>
        private void DrawCoinSpace()
        {
            for (int j = 0; j < hNumCoin/*6*/; j++)
            {
                for (int i = 0; i < wNumCoin/*7*/; i++)
                {
                    Graphics gb1 = bgGraph_1st.Graphics;
                    Color C = Color.FromKnownColor(KnownColor.Blue);

                    Brush B = new SolidBrush(/*Color.Blue*/C);
                    Pen P = new Pen(B, 4);

                    Rectangle R = GetCoinSpaceParam(i, j);

                    int EllipseWidth = R.Width + 4;
                    int EllipseHeight = R.Height + 4;

                    int EllipseX = R.X - 2;
                    int EllipseY = R.Y - 2;

                    gb1.DrawEllipse(/*Pens.Blue*/P, new Rectangle(EllipseX, EllipseY, EllipseWidth, EllipseHeight));

                    int FilEllipseX = R.X;
                    int FilEllipseY = R.Y;

                    int FilEllipseWidth = R.Width;
                    int FilEllipseHeight = R.Height;

                    gb1.FillEllipse(Brushes.Black, new Rectangle(FilEllipseX, FilEllipseY, FilEllipseWidth, FilEllipseHeight));
                }
            }
            Invalidate();
        }

        /// <summary>
        /// method of drawing ellipse
        /// </summary>
        /// <param name="i">logical coordinate i</param>
        /// <param name="j">logical coordinate j</param>
        /// <param name="c">ellipse color</param>
        private void DrawEllipse(int i, int j, Color c)
        {

            Graphics gb1 = bgGraph_1st.Graphics;
            Brush B = new SolidBrush(c);

            Rectangle R = GetCoinSpaceParam(i, j);

            int FillEllipseX = R.X;
            int FillEllipseY = R.Y;

            int FilEllipseWidth = R.Width;
            int FilEllipseHeight = R.Height;

            gb1.FillEllipse(B, new Rectangle(FillEllipseX, FillEllipseY, FilEllipseWidth, FilEllipseHeight));

            Invalidate(); 
        }

        /// <summary>
        /// getting cell parameters
        /// </summary>
        /// <param name="i">logical coordinate i</param>
        /// <param name="j">logical coordinate j</param>
        /// <returns></returns>
        private Rectangle GetCellParam(int i, int j)
        {
            int CellWidth = (int)((this.Width - 15) / wNumCoin);
            int CellHeight = (int)((this.Height * ((double)hNumCoin / (double)wNumCoin) - 35) / hNumCoin);

            //int CellWidth = (int)((this.Width - 15) / 7);
            //int CellHeight = (int)((this.Height * (6.0 / 7.0) - 35) / 6);

            int CellX = (int)(CellWidth * i);
            int CellY = (int)(CellHeight * j) + (int)((this.Height * (1.0 / (double)wNumCoin)));
            //int CellY = (int)(CellHeight * j) + (int)((this.Height * (1.0 / 7.0)));

            return new Rectangle(CellX, CellY, CellWidth, CellHeight);
        }

        /// <summary>
        /// getting cell size
        /// </summary>
        /// <returns></returns>
        private Size GetCellSize()
        {
            int CellWidth = (int)((this.Width - 15) / wNumCoin/*7*/);
            int CellHeight = (int)((this.Height * ((double)hNumCoin/*6.0*/ / (double)wNumCoin/*7.0*/) - 35) / hNumCoin/*6*/);

            //int CellWidth = (int)((this.Width - 15) / 7);
            //int CellHeight = (int)((this.Height * (6.0 / 7.0) - 35) / 6);

            return new Size(CellWidth, CellHeight);
        }

        /// <summary>
        /// getting cell coord
        /// </summary>
        /// <param name="x">physical coordinate x</param>
        /// <param name="y">physical coordinate y</param>
        /// <returns></returns>
        private Point GetCellCoord(int x, int y)
        {
            Size S = GetCellSize();

            int CellWidth = S.Width;
            int CellHeight = S.Height;

            int i = x/CellWidth;
            int j = y/CellHeight-1;

            return new Point(i,j);
        }

        /// <summary>
        /// getting parameter space for coins
        /// </summary>
        /// <param name="i">logical coordinate i</param>
        /// <param name="j">logical coordinate j</param>
        /// <returns></returns>
        private Rectangle GetCoinSpaceParam(int i, int j)
        {
            Rectangle R = GetCellParam(i, j);

            int CellWidth = R.Width;
            int CellHeight = R.Height;

            int CellX = R.X;
            int CellY = R.Y;

            int FilEllipseX = (int)(CellWidth * 0.1 + CellX);
            int FilEllipseY = (int)(CellHeight * 0.1 + CellY);

            int FilEllipseWidth = (int)(CellWidth * 0.8);
            int FilEllipseHeight = (int)(CellHeight * 0.8);

            return new Rectangle(FilEllipseX, FilEllipseY, FilEllipseWidth, FilEllipseHeight);
        }


        private void FormMain_Load(object sender, EventArgs e)
        {
            gamePresent = new GamePresenter(this, new Game(), TLP.Controls["tbPlayer1"].Text, TLP.Controls["pbPlr1Color"].BackColor, TLP.Controls["tbPlayer2"].Text, TLP.Controls["pbPlr2Color"].BackColor);
            gamePresent.GameModel.GameStatus = false;

            wNumCoin = gamePresent.GameModel.GameLogic.Width;
            hNumCoin = gamePresent.GameModel.GameLogic.Height;

            //parametres of main form
            int HeightScreen = Screen.PrimaryScreen.WorkingArea.Height;
            int WidthScreen = Screen.PrimaryScreen.WorkingArea.Width;

            int HeightForm = (int) (HeightScreen * 0.5);
            int WidthForm = (int) (HeightForm);
            
            this.Size = new Size(WidthForm, HeightForm);
            this.Location = new Point((int)((WidthScreen - WidthForm)/2), (int)((HeightScreen - HeightForm)/2));
            //---------------------------

            //for buffering
            context = BufferedGraphicsManager.Current;

            bgGraph_1st = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)); 
            bgGraph_2nd = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)); 
            //-------------------

            bgGraph_1st.Graphics.Clear(Color.DarkSlateBlue);


            //Style of controls----------------------------------------
            TLP.Size = new Size(this.Width - 15, GetCellSize().Height);

            TLP.Controls["btnStartGame"].Font = new Font("Microsoft Sans Serif", (float)TLP.Controls["btnStartGame"].Height / 3,  FontStyle.Bold);
            TLP.Controls["tbPlayer1"].Font = new Font("Microsoft Sans Serif", (float)TLP.Controls["btnStartGame"].Height / 3, FontStyle.Bold);
            TLP.Controls["tbPlayer2"].Font = new Font("Microsoft Sans Serif", (float)TLP.Controls["btnStartGame"].Height / 3, FontStyle.Bold);
            TLP.Controls["lblActPlayer"].Font = new Font("Microsoft Sans Serif", (float)(TLP.Controls["btnStartGame"].Height / 2.5), FontStyle.Bold); 
            TLP.Controls["btnPlr1ColorChng"].Font = new Font("Microsoft Sans Serif", (float)(TLP.Controls["btnStartGame"].Height / 4), FontStyle.Bold);
            TLP.Controls["btnPlr2ColorChng"].Font = new Font("Microsoft Sans Serif", (float)(TLP.Controls["btnStartGame"].Height / 4), FontStyle.Bold);
            //----------------------------------------

            //Drawing coin spaces
            DrawCoinSpace();
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            bgGraph_1st.Render(g);
        }

        private void FormMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (gamePresent != null && gamePresent.GameModel != null)
            {
                Point P = GetCellCoord(e.X, e.Y);

                gamePresent.SetProgress(P.X, P.Y);
            }
            
        }

        

        
    }                         
    
}
