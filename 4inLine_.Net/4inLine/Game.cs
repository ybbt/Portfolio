using System;
using System.Drawing;


//using System.Threading.Tasks;

namespace _4inLine
{
    /// <summary>
    /// class responsible for the state of the game
    /// </summary>
    class Game
    {
        /// <summary>
        /// player one
        /// </summary>
        private Player player1;

        /// <summary>
        /// player two
        /// </summary>
        private Player player2;

        /// <summary>
        /// the internal rules of the game
        /// </summary>
        private GameLogic gameLogic = new GameLogic();

        public GameLogic GameLogic
        {
            get { return gameLogic; }
        }

        /// <summary>
        /// game board
        /// </summary>
        public Board GameBoard { get; } 

        /// <summary>
        /// active player
        /// </summary>
        private Player activPlayer = null;

        /// <summary>
        /// game status (game - "true", stop game - "false")
        /// </summary>
        private bool gameStatus = false;


        public Game()
        {
            gameStatus = true;
            GameBoard = new Board(gameLogic.Width, gameLogic.Height);
        }

        /// <summary>
        /// change game status to stop
        /// </summary>
        public void StopGame()
        {
            gameStatus = false;
        }

        /// <summary>
        /// player one
        /// </summary>
        public Player Player1
        {
            get { return player1; }
        }

        /// <summary>
        /// player two
        /// </summary>
        internal Player Player2
        {
            get { return player2; }
        }

        /// <summary>
        /// game status (game - "true", stop game - "false")
        /// </summary>
        public bool GameStatus
        {
            set { this.gameStatus = value; }
            get { return gameStatus; }
        }

        /// <summary>
        /// active player
        /// </summary>
        internal Player ActivPlayer
        {
            get { return activPlayer; }
        }

        /// <summary>
        /// set param player1
        /// </summary>
        /// <param name="name">player name</param>
        /// <param name="color">color coins of player</param>
        public void SetPlayer1(String name, Color color)
        {
            this.player1 = new Player(name, 1, color, (gameLogic.Width * gameLogic.Height / 2));
        }

        /// <summary>
        /// set param player2
        /// </summary>
        /// <param name="name">player name</param>
        /// <param name="color">color of player</param>
        public void SetPlayer2(String name, Color color)
        {
            this.player2 = new Player(name, -1, color, (gameLogic.Width * gameLogic.Height / 2));
        }

        /// <summary>
        /// change active player
        /// </summary>
        /// <returns>player name and color of player</returns>
        public Tuple<String, Color> ChangeActivPlayer()
        {
            if (ActivPlayer == null)
            {
                activPlayer = player1;
            }
            else if (ActivPlayer == player1)
            {
                activPlayer = player2;
            }
            else if (ActivPlayer == player2)
            {
                activPlayer = player1;
            }
            return new Tuple<string, Color>(ActivPlayer.Name, ActivPlayer.Color);
        }

        /// <summary>
        /// game progress
        /// </summary>
        /// <param name="logicI">coordinate i logical cell</param>
        /// <param name="logicJ">coordinate j logical cell</param>
        /// <returns></returns>
        public Tuple<LogicCell, LogicCell[], Color, int, int> GameProgress(int logicI, int logicJ)
        {
            LogicCell logicCell = gameLogic.GetLogicCell(logicI, ActivPlayer.IdPlayer);

            if (logicCell == null)
            {
                return null;
            }

            //getting cells in which the coin falls
            Cell cell = GameBoard.GetCell(logicCell);

            //move the coin from the player on the board
            cell.Coin = ActivPlayer.DropCoin();

            //find 4 in row
            LogicCell[] ALC = gameLogic.Find_4inLine(logicCell);

            return new Tuple<LogicCell, LogicCell[], Color, int, int>(cell.LogicCell, ALC, cell.Coin.Color, Player1.CoinsQuant(), Player2.CoinsQuant());
        }
    }
}
