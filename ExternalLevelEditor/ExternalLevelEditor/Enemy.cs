using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Justin Neft
// Holds the data for each instance of an enemy.
namespace ExternalLevelEditor
{
    class Enemy
    {
        #region Fields

        List<String> enemyOptions;
        List<String> playerOptions;
        int priority;

        string enemyType;

        int x;
        int y;

        #endregion Fields

        #region Properties

        public int Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
            }
        }

        public string EnemyType
        {
            get
            {
                return enemyType;
            }
            set
            {
                enemyType = value;
            }
        }

        public List<String> PlayerOptions
        {
            get
            {
                return playerOptions;
            }
        }

        public List<String> EnemyOptions
        {
            get
            {
                return enemyOptions;
            }
        }

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
        /// The constructor for an enemy.
        /// </summary>
        public Enemy()
        {
            enemyOptions = new List<String>();
            playerOptions = new List<String>();
            priority = 0;
        }
    }
}
