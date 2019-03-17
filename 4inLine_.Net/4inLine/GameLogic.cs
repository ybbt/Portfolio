using System;


namespace _4inLine
{
    class GameLogic
    {
        /// <summary>
        /// number of cells in width
        /// </summary>
        private const int width  = 7;

        public int Width
        {
             get { return width; }
        }

        /// <summary>
        /// number of cells in height
        /// </summary>
        private const int height = 6;

        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// number of coins in a row to win
        /// </summary>
        private const int CoinsInLine = 4;

        /// <summary>
        /// number of potential winning choices in line with the coin
        /// </summary>
        private int step;
        
        /// <summary>
        /// logical board
        /// </summary>
        private int[,] logicBoard;

        /// <summary>
        /// logical board
        /// </summary>
        public int[,] LogicBoard
        {
            get { return logicBoard; }
            set { logicBoard = value; }
        }


        public GameLogic()
        {
            this.LogicBoard = new int[width, Height];

            this.step = CoinsInLine;
        }

        
        /// <summary>
        /// getting cells in which the coin falls
        /// </summary>
        /// <param name="logicI">logical coordinate i of the cell click</param>
        /// <param name="idPlayer">id active player</param>
        /// <returns>logical cell</returns>
        public LogicCell GetLogicCell(int logicI, int idPlayer)
        {
            if (idPlayer != 1 && idPlayer != -1)
            {
                throw new Exception("idPlayer is invalid");
            }
            try
            {
                for (int j = LogicBoard.GetLength(1) - 1; j >= 0; j--)
                {
                    if (LogicBoard[logicI, j] == 0)
                    {
                        //mark coins in the cell
                        LogicBoard[logicI, j] = idPlayer;
                        return new LogicCell(logicI, j);

                    }
                }

                return null;
            }
            catch (Exception)
            {
                if (logicI >= width)
                {
                    throw new Exception("specified number exceeds the number of columns");
                }
                
                else throw;
            }
        }

        /// <summary>
        /// check for 4 coins in a row
        /// </summary>
        /// <param name="logicCellc">logical cell</param>
        /// <returns>array of logical cell, where 4 coins in a row</returns>
        public LogicCell[] Find_4inLine(LogicCell logicCellc)
        {
            try
            {
                if (logicCellc == null) return null;

                int idPlayer = LogicBoard[logicCellc.LogicI, logicCellc.LogicJ];

                //factors considering the direction of search 4 in row
                int factorI = 1;
                int factorJ = 1;

                for (int k = (int)Way.DiagonalFist; k <= (int)Way.Horizontal; k++)
                {
                    switch (k)
                    {
                        case (int)Way.DiagonalFist:
                            factorI = 1;
                            factorJ = 1;
                            break;
                        case (int)Way.DiagonalSecond:
                            factorI = -1;
                            factorJ = 1;
                            break;
                        case (int)Way.Vertical:
                            factorI = 0;
                            factorJ = 1;
                            break;
                        case (int)Way.Horizontal:
                            factorI = 1;
                            factorJ = 0;
                            break;
                    }

                    //search is performed by adding the values of neighboring cells that can take the values 0, 1 or -1. 
                    //If 4 neighboring cells contain coins of the same color, then the sum of the logical values of these cells will be 4 or -4
                    for (int j = 0; j < step; j++)
                    {
                        int testSum = 0;

                        for (int i = 0; i < CoinsInLine; i++)
                        {
                            if (logicCellc.LogicJ - (i - j) * factorJ >= 0 &&
                                logicCellc.LogicJ - (i - j) * factorJ < LogicBoard.GetLength(1) &&
                                logicCellc.LogicI - (i - j) * factorI >= 0 &&
                                logicCellc.LogicI - (i - j) * factorI < LogicBoard.GetLength(0))
                            {
                                testSum += LogicBoard[logicCellc.LogicI - (i - j) * factorI, logicCellc.LogicJ - (i - j) * factorJ];
                            }
                            else break;
                        }
                        if (testSum == (CoinsInLine * idPlayer))
                        {
                            LogicCell[] ArrayLC = new LogicCell[CoinsInLine];
                            for (int i = 0; i < CoinsInLine; i++)
                            {
                                ArrayLC[i] = new LogicCell(logicCellc.LogicI - (i - j) * factorI, logicCellc.LogicJ - (i - j) * factorJ);
                            }
                            return ArrayLC;
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

    }

    /// <summary>
    /// helper class containing a logical coordinates logical cell
    /// </summary>
    public class LogicCell
    {
        /// <summary>
        /// logical coordinate i
        /// </summary>
        public int LogicI { get; set; }

        /// <summary>
        /// logical coordinate j
        /// </summary>
        public int LogicJ { get; set; }

        public LogicCell(int logicI, int logicJ)
        {
            this.LogicI = logicI;
            this.LogicJ = logicJ;
        }
    }

    /// <summary>
    /// enumeration directions check
    /// </summary>
    public enum Way
    {
        DiagonalFist = 0, Vertical = 1, DiagonalSecond = 2, Horizontal = 3
    }
}
