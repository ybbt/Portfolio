

namespace _4inLine
{
    /// <summary>
    /// class represents a game board
    /// </summary>
    class Board
    {
        /// <summary>
        /// cell array
        /// </summary>
        private Cell[,] Cells;

        /// <summary>
        /// constructor of Board
        /// </summary>
        /// <param name="w">Width of Board</param>
        /// <param name="h">Height of Board</param>
        public Board(int w, int h)
        {
            Cells = new Cell[w, h];

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    Cells[i, j] = new Cell(new LogicCell(i, j));
                }
            }
        }

        /// <summary>
        /// getting the desired cell
        /// </summary>
        /// <param name="logicCell">logical cell</param>
        /// <returns>cell</returns>
        public Cell GetCell(LogicCell logicCell)
        {
            return Cells[logicCell.LogicI, logicCell.LogicJ];
        }
    }
}
