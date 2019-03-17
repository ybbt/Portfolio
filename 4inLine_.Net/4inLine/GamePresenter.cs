using System;
using System.Drawing;


namespace _4inLine
{
    /// <summary>
    /// class responsible for the process of the game
    /// </summary>
    class GamePresenter
    {
        /// <summary>
        /// reference to view
        /// </summary>
        private readonly IGameView gameView;

        /// <summary>
        ///  reference to the class in charge of the state of the game
        /// </summary>
        private Game gameModel;

        /// <summary>
        ///  reference to the class in charge of the state of the game
        /// </summary>
        internal Game GameModel
        {
            get { return gameModel; }
        }
        
        public GamePresenter(IGameView view, Game model, String namePl1, Color colorPl1, String namePl2, Color colorPl2)
        {
            this.gameView = view;
            this.gameModel = model;

            SetPlayers(namePl1, colorPl1, namePl2, colorPl2);

            GetActivPlayer(); 
        }

        /// <summary>
        /// set player param
        /// </summary>
        /// <param name="namePl1">player1 name</param>
        /// <param name="colorPl1">player1 color</param>
        /// <param name="namePl2">player2 name</param>
        /// <param name="colorPl2">player2 color</param>
        public void SetPlayers(String namePl1, Color colorPl1, String namePl2, Color colorPl2)
        {
            GameModel.SetPlayer1(namePl1, colorPl1);
            GameModel.SetPlayer2(namePl2, colorPl2);
        }

        /// <summary>
        /// setting an active player
        /// </summary>
        public void GetActivPlayer()
        {
            Tuple<String, Color> ActivPlayerResult = GameModel.ChangeActivPlayer();

            gameView.DisplayActivPlayer(ActivPlayerResult.Item1, ActivPlayerResult.Item2);
        }


        /// <summary>
        /// setting game progress
        /// </summary>
        /// <param name="logicI">logical coordinate i </param>
        /// <param name="logicJ">logical coordinate j</param>
        public void SetProgress(int logicI, int logicJ)
        {
            //if GameModel exist and GameStatus not Stop
            if (GameModel != null && gameModel.GameStatus != false)
            {
                Tuple<LogicCell, LogicCell[], Color, int, int> ProgressResult = GameModel.GameProgress(logicI, logicJ);

                if (ProgressResult == null)
                {
                    return;
                }

                //if not 4 in row
                if (ProgressResult.Item2 != null)
                {
                    Color color = ProgressResult.Item3;
                    Color invColor = Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);

                    for (int i = 0; i < ProgressResult.Item2.Length; i++)
                    {
                        gameView.DrawCoin(ProgressResult.Item2[i], invColor);
                    }
                    gameModel.StopGame();
                    gameView.DisplayWin(gameModel.ActivPlayer.Name);
                }
                else
                {
                    //if the coins are over in players
                    if (ProgressResult.Item4 == 0 && ProgressResult.Item5 == 0)
                    {
                        gameModel.StopGame();
                        gameView.DrawCoin(ProgressResult.Item1, ProgressResult.Item3);
                        gameView.DisplayDeadend();
                        return;
                    }
                    else
                    {
                        //gameView.CoinAnimation(new LogicCell(ProgressResult.Item1.LogicI, 0), ProgressResult.Item1,
                        //    ProgressResult.Item3);
                        gameView.DrawCoin(ProgressResult.Item1, ProgressResult.Item3);
                    }
                }
                GetActivPlayer();
            }

        }

        /// <summary>
        /// completion of the current game
        /// </summary>
        public void EndGame()
        {
            this.gameModel = null;
        }


    }
}
