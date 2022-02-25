using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Justin Neft
// Holds the relevant data about the player, namely its position.
namespace ExternalLevelEditor
{
    class Player
    {

        #region Fields

        int x;
        int y;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the y position of the player.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Gets or sets the y position of the player.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// Makes a new player class.
        /// </summary>
        public Player(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
