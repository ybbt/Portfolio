
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace _4inLine
{
    /// <summary>
    /// class represents a player
    /// </summary>
    class Player
    {
        /// <summary>
        /// сollection of coins in a queue
        /// </summary>
        private Queue<Coin> QueueCoins;// = new Queue<Coin>((int)(7 * 6 / 2));

        /// <summary>
        /// player color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// player Id 
        /// </summary>
        public int IdPlayer { get; set; }

        /// <summary>
        /// player name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// coonstructor
        /// </summary>
        /// <param name="name">playername</param>
        /// <param name="idPlayer">player id</param>
        /// <param name="color"player color></param>
        public Player(string name, int idPlayer, Color color, int numOfCoins)
        {
            Name = name;
            IdPlayer = idPlayer;
            Color = color;

            QueueCoins = new Queue<Coin>(numOfCoins);

            for (int i = 0; i < numOfCoins; i++)
            {
                QueueCoins.Enqueue(new Coin(Color, idPlayer, i));
            }

        }

        /// <summary>
        /// drop coin from queue
        /// </summary>
        /// <returns>coin</returns>
        public Coin DropCoin()
        {
            return QueueCoins.Dequeue();
        }

        /// <summary>
        /// quant of coins
        /// </summary>
        /// <returns>quant of coins</returns>
        public int CoinsQuant()
        {
            return QueueCoins.Count();
        }
    }
}
