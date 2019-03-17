

namespace _4inLine
{
    /// <summary>
    /// class represents a single cell game board
    /// </summary>
    class Cell
    {
        /// <summary>
        /// coin in cell
        /// </summary>
        private Coin coin;

        /// <summary>
        /// logical cell
        /// </summary>
        private readonly LogicCell logicCell;


        public Cell(LogicCell logicCell)
        {
            this.logicCell = logicCell;
        }

        /// <summary>
        /// coin in cell
        /// </summary>
        public Coin Coin
        {
            set { coin = value; }
            get { return coin; }
        }

        /// <summary>
        /// logical cell
        /// </summary>
        public LogicCell LogicCell
        {
            get { return logicCell; }
        }



    }
}
