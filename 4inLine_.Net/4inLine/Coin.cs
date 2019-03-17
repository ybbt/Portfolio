using System.Drawing;

namespace _4inLine
{
    /// <summary>
    /// class represents a single coin game board
    /// </summary>
    class Coin
    {

        private int idPlayer;
        private int number;

        /// <summary>
        /// color coins
        /// </summary>
        private Color color;

        public Coin(Color color, int idPlayer, int number)
        {
            this.color = color;
            this.idPlayer = idPlayer;
            this.number = number;
        }

        /// <summary>
        /// color coins
        /// </summary>
        public Color Color
        {
            get { return color; }
        }

        
    }
}